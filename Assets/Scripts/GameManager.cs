using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [System.Serializable]
    public class FlyingCameraNode
    {
        public Transform transform;
        public float timeToNextNode;
    }

    public bool flyingCam;
    public Animator flyingCamAnim;
    public bool started;
    public bool countdown;
    public float countdownTimer;
    public bool animatedFlyingCamera;

    public List<FlyingCameraNode> flyingCameraNodes;

    public List<GameUI> gameUIs;

    float countdownProgress;
    int countdownTime;

    float gameTime;

    int currentNode;
    float nodeProgress;

    bool finished;

    private void Start()
    {
        countdownProgress = countdownTimer;

        foreach (GameUI ui in gameUIs)
        {
            if (flyingCam || !countdown)
            {
                ui.countdownText.gameObject.SetActive(false);
            }

            if (!started)
            {
                ui.gameTimeText.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if(flyingCam)
        {
            if (!finished)
            {
                FlyingCam();
            }
            else
            {

            }
        }

        if(!started && countdown && !flyingCam)
        {
            countdownProgress -= Time.deltaTime;
            SetNewTimeText((int)Mathf.Ceil(countdownProgress));

            if(countdownProgress <= 0)
            {
                StartRace();
            }
        }

        if(started && !finished)
        {
            gameTime += Time.deltaTime;

            foreach (GameUI ui in gameUIs)
            {
                ui.gameTimeText.text = gameTime.ToString("0.00");
            }
        }
    }

    private void FlyingCam()
    {
        nodeProgress += Time.deltaTime / flyingCameraNodes[currentNode].timeToNextNode;

        if(nodeProgress > 1)
        {
            nodeProgress -= 1;
            currentNode++;

            if(currentNode == flyingCameraNodes.Count)
            {
                flyingCam = false;

                if(countdown)
                {
                    foreach (GameUI ui in gameUIs)
                    {
                        ui.countdownText.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void GetFlyingCamPosition(out Vector3 camPos, out Quaternion camRot, Vector3 finalPosition, Quaternion finalRotation)
    {


        /*
        int id_A = Mathf.Clamp(currentNode, 0, flyingCameraNodes.Count - 1);
        int id_B = Mathf.Clamp(currentNode +1, 0, flyingCameraNodes.Count - 1);
        int id_C = Mathf.Clamp(currentNode +2, 0, flyingCameraNodes.Count - 1);
        int id_D = Mathf.Clamp(currentNode +3, 0, flyingCameraNodes.Count - 1);

        Vector3 posA = flyingCameraNodes[id_A].transform.position;
        Vector3 posB = flyingCameraNodes[id_B].transform.position;
        Vector3 posC = flyingCameraNodes[id_C].transform.position;
        Vector3 posD = flyingCameraNodes[id_D].transform.position;
        Debug.Log(id_A + "   " + id_B + "   " + id_C + "   " + id_D);
        //Vector3 posA = currentNode - 1 >= 0 ? flyingCameraNodes[currentNode - 1].transform.position : flyingCameraNodes[0].transform.position;
        //Vector3 posB = flyingCameraNodes[currentNode].transform.position;
        //Vector3 posC = currentNode + 1 < flyingCameraNodes.Count ? flyingCameraNodes[currentNode + 1].transform.position : finalPosition;
        //Vector3 posD = currentNode + 2 < flyingCameraNodes.Count ? flyingCameraNodes[currentNode + 1].transform.position : finalPosition;



        //float t = 0.333f + (nodeProgress * 0.333f);
        float t = nodeProgress;
        float u = 1-t;

        float a = Mathf.Pow(u,3);
        float b = 3 * t * u * u;
        float c = 3 * t * t * u;
        float d = Mathf.Pow(t, 3);

        Debug.Log(a + "   " + b + "   " + c + "   " + d);
        camPos = (a * posA) + (b*posB) + (c*posC) + (d*posD);
        //camPos = Vector3.Lerp(posB, posC, nodeProgress);
        */
        if (animatedFlyingCamera)
        {
            camPos = flyingCamAnim.transform.position;
            camRot = flyingCamAnim.transform.rotation;
        }
        else
        {
            camPos = Vector3.Lerp(flyingCameraNodes[currentNode].transform.position, currentNode + 1 < flyingCameraNodes.Count ? flyingCameraNodes[currentNode + 1].transform.position : finalPosition, nodeProgress);
            camRot = Quaternion.Lerp(flyingCameraNodes[currentNode].transform.rotation, currentNode + 1 < flyingCameraNodes.Count ? flyingCameraNodes[currentNode + 1].transform.rotation : finalRotation, nodeProgress);
        }


    }

    private void SetNewTimeText(int newTime)
    {
        if (newTime != countdownTime)
        {
            Debug.Log(countdownTime);

            countdownTime = newTime;

            foreach (GameUI ui in gameUIs)
            {
                ui.animator.Play("Countdown");
                ui.countdownText.text = countdownTime > 0 ? countdownTime.ToString() : "GO!";
            }
        }
    }

    public void StartRace()
    {
        started = true;

        foreach (GameUI ui in gameUIs)
        {
            ui.gameTimeText.gameObject.SetActive(true);
        }
    }

    public void EndRace()
    {
        finished = true;

        foreach (GameUI ui in gameUIs)
        {
            ui.gameTimeText.gameObject.SetActive(false);

            ui.resultText.gameObject.SetActive(true);

            ui.finishTimeText.text = gameTime.ToString("0.00");
            ui.finishTimeText.gameObject.SetActive(true);
        }
    }
}
