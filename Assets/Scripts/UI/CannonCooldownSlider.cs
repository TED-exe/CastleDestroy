using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonCooldownSlider : MonoBehaviour
{
    [SerializeField] private Slider loadSlider;
    [SerializeField] private bool isPlaying;
    private float maxCooldownHolder;
    private float currCooldownHolder;

    private void Awake()
    {
        StopLoadSlide();
    }
    private void Update()
    {
        if (isPlaying)
            LoadSlider();
    }

    private void LoadSlider()
    {
        currCooldownHolder -= Time.deltaTime;
        loadSlider.value = currCooldownHolder/maxCooldownHolder;
        if(loadSlider.value <= 0)
        {
            StopLoadSlide();
        }
    }

    public void StartLoadSlide(Component sender, object data)
    {
        for (int i = 0; i < loadSlider.transform.childCount; i++)
        {
            loadSlider.transform.GetChild(i).gameObject.SetActive(true);
        }
        currCooldownHolder = (float)data;
        maxCooldownHolder = currCooldownHolder;
        isPlaying = true;
    }
    private void StopLoadSlide()
    {
        for(int i = 0; i < loadSlider.transform.childCount; i++)
        {
            loadSlider.transform.GetChild(i).gameObject.SetActive(false);
        }
        isPlaying = false;
    }
}
