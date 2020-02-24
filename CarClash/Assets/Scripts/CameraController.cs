using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum View {
    Full,
    HalfTop,
    HalfBottom,
    HalfLeft,
    HalfRight,
    QuarterTopLeft,
    QuarterTopRight,
    QuarterBottomLeft,
    QuarterBottomRight };


public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        [Header("Settings")]
        public View view = View.Full;

        [Range(1, 20)] public float followSpeed = 8;
        [Range(1, 20)] public float rotationSpeed = 4;

        public bool followRotation = true;

        [Header("Post Processing")]
        public PostProcessVolume ppVolume;
        public float lerpTime = 0.35f;
        [Range(0, 1)] public float chromaticAberrationIntensity = 1f;
        [Range(0, 1)] public float vignetteIntensity = 0.2f;
        public float cameraDollyDistance = 3f;
    }

    public Camera camera;

    [Header("Components")]

    public Transform rig;

    public Settings settings;

    Vector3 cameraPositionOffset;
    Vector3 cameraRotationOffset;

    float lerpProgress;

    Vector3 cameraStartOffset;
    float initialFrustumHeight;

    void Awake()
    {
        cameraPositionOffset = rig.localPosition;
        cameraRotationOffset = rig.localEulerAngles;

        cameraStartOffset = camera.transform.localPosition;
        float startDistance = Vector3.Distance(camera.transform.position, rig.position);
        initialFrustumHeight = ComputeFrustumHeight(startDistance);

        UpdateCamera();
    }

    void UpdateCamera()
    {
        switch (settings.view)
        {

            case View.Full: camera.rect = new Rect(0, 0, 1, 1); break;
            case View.HalfTop: camera.rect = new Rect(0, .5f, 1, .5f); break;
            case View.HalfBottom: camera.rect = new Rect(0, 0, 1, .5f); break;
            case View.HalfLeft: camera.rect = new Rect(0, 0, .5f, 1); break;
            case View.HalfRight: camera.rect = new Rect(.5f, 0, .5f, 1); break;

            case View.QuarterTopLeft: camera.rect = new Rect(0, .5f, .5f, .5f); break;
            case View.QuarterTopRight: camera.rect = new Rect(.5f, .5f, .5f, .5f); break;
            case View.QuarterBottomLeft: camera.rect = new Rect(0, 0, .5f, .5f); break;
            case View.QuarterBottomRight: camera.rect = new Rect(.5f, 0, .5f, .5f); break;

        }

    }

    void FixedUpdate()
    {
        rig.position = Vector3.Lerp(rig.position, transform.position + cameraPositionOffset, Time.deltaTime * settings.followSpeed);
        if (settings.followRotation)
        {
            rig.rotation = Quaternion.Lerp(rig.rotation, Quaternion.Euler(transform.eulerAngles + cameraRotationOffset), Time.deltaTime * settings.rotationSpeed);
        }
    }

    public void UpdatePP(bool isTurning)
    {
        if (isTurning && lerpProgress < 1)
        {
            lerpProgress += Time.deltaTime / settings.lerpTime;
        }
        else if (!isTurning && lerpProgress > 0)
        {
            lerpProgress -= Time.deltaTime / settings.lerpTime;
        }

        lerpProgress = Mathf.Clamp01(lerpProgress);

        if(settings.ppVolume == null)
        {
            settings.ppVolume = camera.GetComponent<PostProcessVolume>();
        }

        if (settings.ppVolume != null && settings.ppVolume.profile.TryGetSettings(out ChromaticAberration ca))
        {
            ca.intensity.value = Mathf.Lerp(0, settings.chromaticAberrationIntensity, lerpProgress);
        }

        if (settings.ppVolume != null && settings.ppVolume.profile.TryGetSettings(out Vignette v))
        {
            v.intensity.value = Mathf.Lerp(0, settings.vignetteIntensity, lerpProgress);
        }

        camera.transform.localPosition = Vector3.Lerp(cameraStartOffset, new Vector3(cameraStartOffset.x, cameraStartOffset.y, cameraStartOffset.z + settings.cameraDollyDistance), lerpProgress);
        float currentDistance = Vector3.Distance(camera.transform.position, rig.position);
        camera.fieldOfView = ComputeFieldOfView(initialFrustumHeight, currentDistance);
    }

    private float ComputeFrustumHeight(float distance)
    {
        return (2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
    }

    private float ComputeFieldOfView(float height, float distance)
    {
        return (2.0f * Mathf.Atan(height * 0.5f / distance) * Mathf.Rad2Deg);
    }
}