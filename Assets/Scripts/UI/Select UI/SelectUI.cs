using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public abstract class SelectUI : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    public GameObject decreaseButton;
    public GameObject increaseButton;
    public bool cycle;
    [ShowInInspector, ReadOnly]
    protected int index;
    public abstract void UpdateUI();
    protected virtual void OnValidate()
    {
        UpdateUI();
    }

}
