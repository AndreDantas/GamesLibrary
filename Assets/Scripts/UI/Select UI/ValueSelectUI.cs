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
public class ValueSelectUI : SelectUI
{
    public string valueUnit = "";
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
            _minValue = UtilityFunctions.ClampMax(_minValue, _maxValue);
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
    [Space(15)]
    public OnValueSelectChanged OnValueChanged;

    protected override void OnValidate()
    {
        minValue = UtilityFunctions.ClampMax(minValue, maxValue);
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

    public override void UpdateUI()
    {
        value = Mathf.Clamp(value, minValue, maxValue);

        if (increaseButton)
        {
            increaseButton.SetActive(!(value == maxValue));

        }
        if (decreaseButton)
        {
            decreaseButton.SetActive(!(value == minValue));

        }
        if (valueText)
            valueText.text = value.ToString() + valueUnit;
    }
}
