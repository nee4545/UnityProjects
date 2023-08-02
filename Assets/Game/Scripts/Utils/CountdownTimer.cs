using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 10f;
    public bool timerIsRunning = false;
    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }

    public void ResetTimer(float seconds)
    {
        timeRemaining = seconds;
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            { 
                timeRemaining = 0f;
                timerIsRunning = false;
            }
        }
    }
}
