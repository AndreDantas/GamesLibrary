using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;


public abstract class SelectUI : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    public GameObject decreaseButton;
    public GameObject increaseButton;


    public abstract void UpdateUI();
}
