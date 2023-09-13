using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TimerText;
    public bool playing = false;
    private float timer;

    private void Awake()
    {
        StopCountingDown();
    }
    private void Update()
    {
        if (playing)
            CoutingDown();
    }
    private void CoutingDown()
    {
        timer -= Time.deltaTime;
        int seconds = Mathf.FloorToInt(timer % 60F);
        int milliseconds = Mathf.FloorToInt((timer * 100F) % 100F);
        TimerText.text = seconds.ToString("00") + ":" + milliseconds.ToString("00");
        if (timer <= 0)
        {
            StopCountingDown();
        }
    }
    public void StartCountingDown(Component sander,object data)
    {
        TimerText.enabled = true;
        timer = (float)data;
        playing = true;
    }
    public void StopCountingDown()
    {
        TimerText.enabled = false;
        playing = false;
    }
}

