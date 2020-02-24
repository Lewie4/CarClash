using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DebugOptions : Singleton<DebugOptions>
{
    private Car targetCar;
    private CameraController cameraController;

    private Car.Settings carSettings = null;
    private CameraController.Settings cameraSettings = null;

    [SerializeField] private Slider accSlider;
    [SerializeField] private TextMeshProUGUI accValue;

    [SerializeField] private Slider slowMoSlider;
    [SerializeField] private TextMeshProUGUI slowMoValue;

    [SerializeField] private Slider boostSlider;
    [SerializeField] private TextMeshProUGUI boostValue;

    [SerializeField] private Toggle airBoostToggle;

    [SerializeField] private Slider gravitySlider;
    [SerializeField] private TextMeshProUGUI gravityValue;

    [SerializeField] private Slider driftSlider;
    [SerializeField] private TextMeshProUGUI driftValue;

    [SerializeField] private Slider vibrationAmountSlider;
    [SerializeField] private TextMeshProUGUI vibrationAmountValue;

    [SerializeField] private Slider vibrationSpeedSlider;
    [SerializeField] private TextMeshProUGUI vibrationSpeedValue;

    [SerializeField] private Slider cameraFollowSpeedSlider;
    [SerializeField] private TextMeshProUGUI cameraFollowSpeedValue;

    [SerializeField] private Slider cameraRotationSpeedSlider;
    [SerializeField] private TextMeshProUGUI cameraRotationSpeedValue;

    [SerializeField] private Toggle cameraFollowRotationToggle;

    [SerializeField] private Slider cameraEffectLerpTimeSlider;
    [SerializeField] private TextMeshProUGUI cameraEffectLerpTimeValue;

    [SerializeField] private Slider cameraEffectCAIntensitySlider;
    [SerializeField] private TextMeshProUGUI cameraEffectCAIntensityValue;

    [SerializeField] private Slider cameraEffectVIntensitySlider;
    [SerializeField] private TextMeshProUGUI cameraEffectVIntensityValue;

    [SerializeField] private Slider cameraEffectDollyDistanceSlider;
    [SerializeField] private TextMeshProUGUI cameraEffectDollyDistanceValue;

    private int sceneCount;

    private int currentCar;

    private void Awake()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
    }

    private void OnEnable()
    {
        GetStuff();

        Setup();
    }

    private void GetStuff()
    {
        if (targetCar == null)
        {
            foreach (Car car in FindObjectsOfType<Car>())
            {
                if (car.playerNum == currentCar)
                {
                    targetCar = car;
                }
            }
        }

        if (cameraController == null)
        {
            foreach (CameraController camera in FindObjectsOfType<CameraController>())
            {
                if (camera.GetComponent<Car>().playerNum == currentCar)
                {
                    cameraController = camera;
                }
            }
        }
    }

    private void Setup()
    {
        if (targetCar == null || cameraController == null)
        {
            GetStuff();
        }

        if (carSettings == null || cameraSettings == null)
        {

            carSettings = targetCar.settings;
            cameraSettings = cameraController.settings;

            accSlider.value = carSettings.acceleration;
            accValue.text = accSlider.value.ToString();

            slowMoSlider.value = carSettings.slowMoPower;
            slowMoValue.text = slowMoSlider.value.ToString();

            boostSlider.value = carSettings.boostPower;
            boostValue.text = boostSlider.value.ToString();

            airBoostToggle.isOn = carSettings.airBoost;

            gravitySlider.value = carSettings.gravity;
            gravityValue.text = gravitySlider.value.ToString();

            driftSlider.value = carSettings.drift;
            driftValue.text = driftSlider.value.ToString();

            vibrationAmountSlider.value = carSettings.vibrateAmount;
            vibrationAmountValue.text = vibrationAmountSlider.value.ToString();

            vibrationSpeedSlider.value = carSettings.vibrateSpeed;
            vibrationSpeedValue.text = vibrationSpeedSlider.value.ToString();

            cameraFollowSpeedSlider.value = cameraSettings.followSpeed;
            cameraFollowSpeedValue.text = cameraFollowSpeedSlider.value.ToString();

            cameraRotationSpeedSlider.value = cameraSettings.rotationSpeed;
            cameraRotationSpeedValue.text = cameraRotationSpeedSlider.value.ToString();

            cameraFollowRotationToggle.isOn = cameraSettings.followRotation;

            cameraEffectLerpTimeSlider.value = cameraSettings.lerpTime;
            cameraEffectLerpTimeValue.text = cameraEffectLerpTimeSlider.value.ToString();

            cameraEffectCAIntensitySlider.value = cameraSettings.chromaticAberrationIntensity;
            cameraEffectCAIntensityValue.text = cameraEffectCAIntensitySlider.value.ToString();

            cameraEffectVIntensitySlider.value = cameraSettings.vignetteIntensity;
            cameraEffectVIntensityValue.text = cameraEffectVIntensitySlider.value.ToString();

            cameraEffectDollyDistanceSlider.value = cameraSettings.cameraDollyDistance;
            cameraEffectDollyDistanceValue.text = cameraEffectDollyDistanceSlider.value.ToString();
        }
        else
        {
            ApplySettings();
        }
    }

    public void PauseGame(bool isPausing)
    {
        Time.timeScale = isPausing ? 0 : 1;
    }

    public void ChangeAcceleration()
    {
        carSettings.acceleration = accSlider.value;
        accValue.text = accSlider.value.ToString();

        ApplySettings();
    }

    public void ChangeSlowMoPower()
    {
        carSettings.slowMoPower = slowMoSlider.value;
        slowMoValue.text = slowMoSlider.value.ToString();

        ApplySettings();
    }

    public void ChangeBoost()
    {
        carSettings.boostPower = boostSlider.value;
        boostValue.text = boostSlider.value.ToString();

        ApplySettings();
    }

    public void ChangeAirBoost()
    {
        carSettings.airBoost = airBoostToggle.isOn;

        ApplySettings();
    }

    public void ChangeGravity()
    {
        carSettings.gravity = gravitySlider.value;
        gravityValue.text = gravitySlider.value.ToString();

        ApplySettings();
    }

    public void ChangeDrift()
    {
        carSettings.drift = driftSlider.value;
        driftValue.text = driftSlider.value.ToString();

        ApplySettings();
    }

    public void ChangeVibrationAmount()
    {
        carSettings.vibrateAmount = vibrationAmountSlider.value;
        vibrationAmountValue.text = vibrationAmountSlider.value.ToString();

        ApplySettings();
    }

    public void ChangeVibrationSpeed()
    {
        carSettings.vibrateSpeed = vibrationSpeedSlider.value;
        vibrationSpeedValue.text = vibrationSpeedSlider.value.ToString();

        ApplySettings();
    }

    public void ChangeCameraFollowSpeed()
    {
        cameraSettings.followSpeed = cameraFollowSpeedSlider.value;
        driftValue.text = cameraFollowSpeedSlider.value.ToString();

        ApplySettings();
    }

    public void ChangeCameraRotationSpeed()
    {
        cameraSettings.rotationSpeed = cameraRotationSpeedSlider.value;
        cameraRotationSpeedValue.text = cameraRotationSpeedSlider.value.ToString();

        ApplySettings();
    }

    public void ChangeCameraFollowRotation()
    {
        cameraSettings.followRotation = cameraFollowRotationToggle.isOn;

        ApplySettings();
    }

    public void ChangeCameraEffectLerpTime()
    {
        cameraSettings.lerpTime = cameraEffectLerpTimeSlider.value;
        cameraEffectLerpTimeValue.text = cameraEffectLerpTimeSlider.value.ToString();

        ApplySettings();
    }

    public void ChangeCameraEffectCAIntensity()
    {
        cameraSettings.chromaticAberrationIntensity = cameraEffectCAIntensitySlider.value;
        cameraEffectCAIntensityValue.text = cameraEffectCAIntensitySlider.value.ToString();

        ApplySettings();
    }

    public void ChangeCameraEffectVIntensity()
    {
        cameraSettings.vignetteIntensity = cameraEffectVIntensitySlider.value;
        cameraEffectVIntensityValue.text = cameraEffectVIntensitySlider.value.ToString();

        ApplySettings();
    }

    public void ChangeCameraEffectDollyDistance()
    {
        cameraSettings.cameraDollyDistance = cameraEffectDollyDistanceSlider.value;
        cameraEffectDollyDistanceValue.text = cameraEffectDollyDistanceSlider.value.ToString();

        ApplySettings();
    }

    private void ApplySettings()
    {
        if (targetCar == null || cameraController == null)
        {
            GetStuff();
        }

        targetCar.settings = carSettings;
        cameraController.settings = cameraSettings;
    }

    public void ChangeScene(bool increase)
    {
        int destinationScene = SceneManager.GetActiveScene().buildIndex;
        if(increase)
        {
            destinationScene++;

            if(destinationScene >= sceneCount)
            {
                destinationScene = 1;
            }            
        }
        else
        {
            destinationScene--;

            if(destinationScene < 1)
            {
                destinationScene = sceneCount - 1;
            }
        }

        PauseGame(false);

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        StartCoroutine(LoadSceneAndSetActive(destinationScene));
    }

    public static IEnumerator LoadSceneAndSetActive(int destinationScene)
    {
        AsyncOperation async;
        async = SceneManager.LoadSceneAsync(destinationScene, LoadSceneMode.Additive);
        while (!async.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(destinationScene));
    }
}
