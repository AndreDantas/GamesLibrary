using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Timer : MonoBehaviour
{

    public float countdown = 60f;
    float currentTime;
    public TextMeshProUGUI timerText;
    public Image timerImage;
    public delegate void OnFinishEventHandler();
    public event OnFinishEventHandler OnFinish;
    bool counting = false;
    bool finished;
    private void OnValidate()
    {
        if (timerText)
            timerText.text = Mathf.Floor(countdown).ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            currentTime -= Time.deltaTime;
            UpdateTimer();
            UpdateImage();
            if (currentTime < 0)
            {
                counting = false;
                finished = true;
                if (OnFinish != null)
                    OnFinish();
            }
        }
    }

    public void StartTimer()
    {
        currentTime = countdown;
        finished = false;
        counting = true;
    }

    public void StopTimer()
    {
        counting = false;
    }

    public void ContinueTimer()
    {
        if (!finished)
            counting = true;
    }

    void UpdateTimer()
    {
        if (timerText && counting)
        {
            timerText.text = Mathf.CeilToInt(currentTime).ToString();

        }
    }

    void UpdateImage()
    {
        if (timerImage && counting)
        {
            timerImage.fillAmount = Map(0, countdown, 0, 1, currentTime);
        }
    }

    public static float Map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    public bool IsFinished()
    {
        return finished;
    }
}
