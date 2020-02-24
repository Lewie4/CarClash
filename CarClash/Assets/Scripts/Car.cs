﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        [Range(0.0f, 2000.0f)] public float acceleration = 1500f;
        [Range(0.0f, 500.0f)] public float gravity = 100f;
        [Range(0.0f, 1.0f)] public float drift = 1f;
        [Range(0.0f, 100.0f)] public float boostPower = 35f;
        [Range(0.0f, 1.0f)] public float slowMoPower = 0.2f;
        public bool alwaysSmoke = false;
        public bool airBoost = true;
        public float vibrateAmount = 5f;
        public float vibrateSpeed = 35f;
        public ParticleSystem.MinMaxCurve smokeSize = new ParticleSystem.MinMaxCurve(0.3f, 0.6f);
    }

    public CameraController camera;

    public int playerNum;

    [Header("Components")]
    public Transform vehicleModel;
    public Rigidbody sphere;
    public ParticleSystem smokeLeft;
    public ParticleSystem smokeRight;
    public Transform wheelFrontLeft;
    public Transform wheelFrontRight;
    public Transform body;
    public Transform container;
    public TrailRenderer trailLeft;
    public TrailRenderer trailRight;
    public GameObject arrow;
    public GameObject arrowPower;

    public Settings settings;

    float speed;
    float speedTarget;

    bool isTurning;
    float rotate;

    bool nearGround;
    bool onGround;

    float currentSlowMo = 1f;

    Vector3 startRot;
    Vector3 clickOffset;
    float screenXDistance, screenYDistance;
    float boostMod;

    Vector3 orthogonalVector;

    Vector3 containerBase;

    //[SerializeField] TMPro.TextMeshProUGUI text;

    private void Awake()
    {
        screenXDistance = Screen.width / 5;
        screenYDistance = Screen.height / 5;

        containerBase = container.localPosition;
    }

    private void Update()
    {
        //Acceleration
        speedTarget = Mathf.SmoothStep(speedTarget, speed, currentSlowMo * Time.deltaTime * 12f);
        speed = 0f;

        Accelerate();

        //Time Control
        if (Input.GetMouseButtonDown(0))
        {
            StartTurn();
        }
        else if (Input.GetMouseButton(0))
        {
            if (isTurning &&!(nearGround || settings.airBoost))
            {
                FinishTurn();
            }

            ControlSteer();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            FinishTurn();
        }

        camera.UpdatePP(isTurning);

        RaycastHit hitOn;
        RaycastHit hitNear;

        onGround = Physics.Raycast(transform.position, Vector3.down, out hitOn, 1.2f);
        nearGround = Physics.Raycast(transform.position, Vector3.down, out hitNear, 2.0f);

        //Normal
        if (hitNear.collider != null)
        {
            //Debug.Log(hitNear.normal);
            vehicleModel.up = Vector3.Lerp(vehicleModel.up, hitNear.normal, Time.deltaTime * 8); //hitNear.normal; //Vector3.Lerp(vehicleModel.up, hitNear.normal, Time.deltaTime * debug);
            vehicleModel.Rotate(0, transform.eulerAngles.y, 0);
        }

        if (isTurning)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, startRot.y + rotate, 0));
            vehicleModel.localRotation = Quaternion.Euler(new Vector3(vehicleModel.localEulerAngles.x, 0, vehicleModel.localEulerAngles.z + Mathf.Sin(Time.time * settings.vibrateSpeed) * settings.vibrateAmount * boostMod));
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(sphere.velocity);  // Fix this later ,orthogonalVector);
            vehicleModel.localRotation = Quaternion.Euler(new Vector3(vehicleModel.localEulerAngles.x, 0 , vehicleModel.localEulerAngles.z));
        }        

        if (wheelFrontLeft != null)
        {
            wheelFrontLeft.localRotation = Quaternion.Euler(0, rotate / 2, 0);
        }
        if (wheelFrontRight != null)
        {
            wheelFrontRight.localRotation = Quaternion.Euler(0, rotate / 2, 0);
        }

        float tilt = 0.0f;

        container.localPosition = containerBase + new Vector3(0, Mathf.Abs(tilt) / 2000, 0);
        container.localRotation = Quaternion.Slerp(container.localRotation, Quaternion.Euler(0, rotate / 8, tilt), Time.deltaTime * 10.0f);

        //Movement
        if (nearGround)
        {
            sphere.AddForce(vehicleModel.forward * speedTarget * currentSlowMo * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            orthogonalVector = Vector3.zero;

            sphere.AddForce(vehicleModel.forward * (speedTarget / 10) * currentSlowMo * Time.deltaTime, ForceMode.Acceleration);

            //Simulated gravity
            sphere.AddForce(Vector3.down * settings.gravity * currentSlowMo * Time.deltaTime, ForceMode.Acceleration);
        }

        transform.position = sphere.transform.position + new Vector3(0, 0.35f, 0);

        //Ground Drag
        Vector3 localVelocity = transform.InverseTransformVector(sphere.velocity);
        localVelocity.x *= 0.9f + (settings.drift / 10);

        if (nearGround)
        {
            sphere.velocity = transform.TransformVector(localVelocity);
        }       

        bool drifting = Vector3.Angle(sphere.velocity, vehicleModel.forward) > 30.0f;

        if (trailLeft != null)
        {
            trailLeft.emitting = drifting && onGround;
        }
        if (trailRight != null)
        {
            trailRight.emitting = drifting && onGround;
        }

        if(smokeLeft != null)
        {
            TyreSmoke(smokeLeft, isTurning || drifting || settings.alwaysSmoke);
        }
        if (smokeRight != null)
        {
            TyreSmoke(smokeRight, isTurning || drifting || settings.alwaysSmoke);
        }
    }

    private void TyreSmoke(ParticleSystem particleSystem, bool smokeActive)
    {
        ParticleSystem.EmissionModule smokeEmission = particleSystem.emission;
        smokeEmission.enabled = smokeActive;

        if (smokeEmission.enabled)
        {
            ParticleSystem.MainModule smokeMain = particleSystem.main;
            smokeMain.startSize = new ParticleSystem.MinMaxCurve(settings.smokeSize.constantMin * boostMod, settings.smokeSize.constantMax * boostMod);
        }
    }

    private void Accelerate()
    {
        speed = settings.acceleration;
    }

    private void StartTurn()
    {
        if (nearGround || settings.airBoost)
        {
            isTurning = true;
            arrow.SetActive(true);

            currentSlowMo = settings.slowMoPower;
            startRot = transform.eulerAngles;

            clickOffset = Input.mousePosition;
        }

    }

    private void FinishTurn()
    {
        isTurning = false;
        arrow.SetActive(false);

        currentSlowMo = 1f;

        if (nearGround || settings.airBoost)
        {
            sphere.AddForce(vehicleModel.forward * settings.boostPower * boostMod, ForceMode.VelocityChange);
        }

        rotate = 0;
    }

    private void ControlSteer()
    {
        /*Vector3 worldPoint = Vector3.zero;
        Vector3 cameraPoint = Input.mousePosition;

        Ray cameraRay = Camera.main.ScreenPointToRay(cameraPoint);
        var plane = new Plane(Vector3.up, new Vector3(0, vehicleModel.position.y, 0));
        float worldPointRange;
        if (plane.Raycast(cameraRay, out worldPointRange))
        {
            worldPoint = cameraRay.GetPoint(worldPointRange);
        }

        //Debug.DrawLine((vehicleModel.position - clickOffset) + (Vector3.up * 0.01f), worldPoint + (Vector3.up * 0.01f), Color.red, Time.deltaTime);

        return Vector3.Normalize((vehicleModel.position) - worldPoint);*/

        Vector3 cameraPoint = Input.mousePosition;
        Vector3 dir = cameraPoint - clickOffset;
        dir = new Vector3(dir.x / screenXDistance, 0);

        //rotate = Vector3.Normalize(dir).x * -90;
        rotate = Mathf.Clamp(dir.x * -30, -110f, 110f);

        var start = new Vector3(clickOffset.x / screenXDistance, clickOffset.y / screenYDistance);
        var end = new Vector3(cameraPoint.x / screenXDistance, cameraPoint.y / screenYDistance);

        boostMod = Mathf.Clamp01(Vector3.Distance(start, end));

        arrowPower.transform.localScale = new Vector3(1, boostMod, 1);

        //Debug.Log("First click: " + clickOffset + " Current: " + cameraPoint + " Rotate: " + rotate + " Boost Mod: " + boostMod);

        //text.text = "Rotate: " + rotate + " Boost Mod: " + boostMod;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PhysicsObject>())
        {
            other.GetComponent<PhysicsObject>().Hit(sphere.velocity);
        }
    }

    /*
    public void OnCollisionStay(Collision collision)
    {
        orthogonalVector = collision.contacts[0].point - vehicleModel.position;
        Debug.Log(orthogonalVector);
    }*/
}
