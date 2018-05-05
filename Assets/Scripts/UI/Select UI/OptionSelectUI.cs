using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
[System.Serializable]
public struct StringObjectPair
{
    public string name;
    public object obj;

    public StringObjectPair(string name, object obj)
    {
        this.name = name;
        this.obj = obj;
    }
}
[System.Serializable]
public class OnOptionSelectChanged : UnityEvent<object>
{

}
public class OptionSelectUI : SelectUI
{

    public List<StringObjectPair> options = new List<StringObjectPair>();
    [Space(15)]
    public OnOptionSelectChanged OnOptionChanged;


    public virtual void ChangeOption(int direction)
    {
        if (options != null ? options.Count > 0 : false)
        {
            if (direction > 0)
                index++;
            if (direction < 0)
                index--;
            if (index >= options.Count)
            {
                if (cycle)
                    index = 0;
                else
                    index = options.Count - 1;
            }
            else if (index < 0)
            {
                if (cycle)
                    index = options.Count - 1;
                else
                    index = 0;
            }
            if (OnOptionChanged != null)
                OnOptionChanged.Invoke(options[index].obj);
            UpdateUI();
        }
    }
    public override void UpdateUI()
    {
        if (options != null ? options.Count > 0 : false)
        {
            if (decreaseButton)
                decreaseButton.SetActive(true);
            if (increaseButton)
                increaseButton.SetActive(true);
            valueText.text = options[index].name;
            if (!cycle)
            {
                if (index == 0)
                    decreaseButton.SetActive(false);
                if (index == options.Count - 1)
                    increaseButton.SetActive(false);
            }
            if (valueText)
                valueText.text = options[index].name;
        }
        else
        {
            if (valueText)
                valueText.text = "";

            if (decreaseButton)
                decreaseButton.SetActive(false);
            if (increaseButton)
                increaseButton.SetActive(false);
        }
    }

    public virtual void SetCurrentOption(string s)
    {
        foreach (StringObjectPair sp in options)
        {
            if (sp.name == s)
            {

                index = options.IndexOf(sp);
                UpdateUI();
                return;
            }
        }
    }
    public void SetCurrentOption(int index)
    {
        if (options == null)
            return;
        if (index >= 0 && index < options.Count)
        {
            this.index = index;
            UpdateUI();
        }
    }
    public virtual object GetCurrentValue()
    {
        if (options != null ? options.Count > 0 : false)
        {
            if (index < options.Count && index >= 0)
                return options[index].obj;

        }
        return null;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    public virtual void ClearOptions()
    {
        options.Clear();
        UpdateUI();
    }
}
