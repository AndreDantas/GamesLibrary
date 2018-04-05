using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
[System.Serializable]
public class OnValueSelectChanged : UnityEvent<int>
{

}

public class ValueSelectUI : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    [SerializeField]
    int _maxValue;
    public int maxValue
    {
        get
        {
            return _maxValue;
        }
        set
        {
            if (_maxValue == value)
                return;
            _maxValue = value;
            UpdateUI();
        }
    }
    [SerializeField]
    int _minValue;
    public int minValue
    {
        get
        {
            return _minValue;
        }

        set
        {
            if (_minValue == value)
                return;

            _minValue = value;
            _minValue = MathOperations.ClampMax(_minValue, _maxValue);
            UpdateUI();
        }
    }
    [SerializeField]
    int _value;
    public int value
    {
        get
        {
            return _value;
        }
        internal set
        {
            if (_value == value)
                return;

            _value = value;

            _value = Mathf.Clamp(_value, _minValue, _maxValue);
            UpdateUI();
        }
    }
    public GameObject decreaseButton;
    public GameObject increaseButton;
    [Space(15)]
    public OnValueSelectChanged OnValueChanged;
    private void OnValidate()
    {
        minValue = MathOperations.ClampMax(minValue, maxValue);
        value = Mathf.Clamp(value, minValue, maxValue);
        UpdateUI();
    }
    public virtual void ModifyValue(int amount)
    {
        if (amount == 0)
            return;
        value += amount;
        value = Mathf.Clamp(value, minValue, maxValue);

        UpdateUI();
        if (OnValueChanged != null)
            OnValueChanged.Invoke(value);
    }

    public virtual void UpdateUI()
    {
        value = Mathf.Clamp(value, minValue, maxValue);

        if (increaseButton)
        {
            RectTransform rect = increaseButton.transform as RectTransform;
            increaseButton.SetActive(!(value == maxValue));
            rect.anchoredPosition = new Vector2(10f, 0f);
        }
        if (decreaseButton)
        {
            RectTransform rect = decreaseButton.transform as RectTransform;
            decreaseButton.SetActive(!(value == minValue));
            rect.anchoredPosition = new Vector2(-10f, 0f);
        }
        if (valueText)
            valueText.text = value.ToString();
    }
}
