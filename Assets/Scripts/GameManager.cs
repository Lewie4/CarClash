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
    public bool started;
    public bool countdown;
    public float countdownTimer;

    public TextMeshProUGUI countdownText;
    public Animator animator;

    public TextMeshProUGUI gameTimeText;

    public List<FlyingCameraNode> flyingCameraNodes;

    public TextMeshProUGUI resultText;
    public TextMeshProUGUI finishTimeText;

    float countdownProgress;
    int countdownTime;

    float gameTime;

    int currentNode;
    float nodeProgress;

    bool finished;

    private void Start()
    {
        countdownProgress = countdownTimer;

        if(flyingCam || !countdown)
        {
            countdownText.gameObject.SetActive(false);
        }

        if(!started)
        {
            gameTimeText.gameObject.SetActive(false);
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
            gameTimeText.text = gameTime.ToString("0.00");
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
                    countdownText.gameObject.SetActive(true);
                }
            }
        }
    }

    public void GetFlyingCamPosition(out Vector3 camPos, out Quaternion camRot, Vector3 finalPosition, Quaternion finalRotation)
    {
        camPos = Vector3.Lerp(flyingCameraNodes[currentNode].transform.position, currentNode + 1 < flyingCameraNodes.Count ? flyingCameraNodes[currentNode + 1].transform.position : finalPosition, nodeProgress);
        camRot = Quaternion.Lerp(flyingCameraNodes[currentNode].transform.rotation, currentNode + 1 < flyingCameraNodes.Count ? flyingCameraNodes[currentNode + 1].transform.rotation : finalRotation, nodeProgress);
    }

    private void SetNewTimeText(int newTime)
    {
        if (newTime != countdownTime)
        {
            Debug.Log(countdownTime);

            animator.Play("Countdown");
            countdownTime = newTime;
            countdownText.text = countdownTime > 0 ? countdownTime.ToString() : "GO!";            
        }
    }

    public void StartRace()
    {
        started = true;

        gameTimeText.gameObject.SetActive(true);
    }

    public void EndRace()
    {
        finished = true;

        gameTimeText.gameObject.SetActive(false);

        resultText.gameObject.SetActive(true);

        finishTimeText.text = gameTime.ToString("0.00");
        finishTimeText.gameObject.SetActive(true);
    }
}
