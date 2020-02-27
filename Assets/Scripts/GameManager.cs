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

    public TextMeshProUGUI gameTimeText;

    float countdownProgress;
    int countdownTime;

    float gameTime;

    private void Start()
    {
        countdownProgress = countdownTimer;

        if(!countdown)
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
        if(!started && countdown)
        {
            countdownProgress -= Time.deltaTime;
            SetNewTimeText((int)Mathf.Ceil(countdownProgress));

            if(countdownProgress <= 0)
            {
                StartRace();
            }
        }

        if(started)
        {
            gameTime += Time.deltaTime;
            gameTimeText.text = System.Math.Round(gameTime, 2).ToString();
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

        gameTimeText.gameObject.SetActive(true);
    }

}
