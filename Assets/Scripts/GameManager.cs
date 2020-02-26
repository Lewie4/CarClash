using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public bool started;
    public bool countdown;
    public float countdownTimer;

    public TextMeshProUGUI countdownText;
    public Animator animator;

    float countdownProgress;
    int countdownTime;

    private void Start()
    {
        countdownProgress = countdownTimer;

        if(!countdown)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(!started && countdown)
        {
            countdownProgress -= Time.deltaTime;
            SetNewTimeText((int)Mathf.Ceil(countdownProgress));

            if(countdownProgress <= 0)
            {
                StartRace();
            }
        }
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
    }

}
