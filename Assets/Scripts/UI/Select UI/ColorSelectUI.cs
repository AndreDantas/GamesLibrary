using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class OnColorSelectChanged : UnityEvent<Color>
{

}
public class ColorSelectUI : SelectUI
{

    #region Public Fields
    [Required("A reference to a Image component is required.")]
    public Image image;
    public List<Color> selectColors = new List<Color>();
    public OnColorSelectChanged OnColorSelect;
    #endregion

    #region Private Fields
    [ShowInInspector, ReadOnly]
    Color currentColor;
    #endregion

    private void Awake()
    {
        if (OnColorSelect != null)
            OnColorSelect.Invoke(selectColors[index]);
        UpdateUI();
    }

    [ButtonGroup("ModifyValue"), HideInPlayMode]
    void Decrease()
    {
        ChangeOption(-1);
    }

    [ButtonGroup("ModifyValue"), HideInPlayMode]
    void Increase()
    {
        ChangeOption(1);
    }

    public void SetColors(List<Color> colors)
    {
        if (colors == null)
            return;
        selectColors = new List<Color>();
        foreach (var item in colors)
        {
            selectColors.Add(new Color(item.r, item.g, item.b));
        }
    }

    public void SetCurrentColor(Color c)
    {
        if (selectColors == null)
            return;
        for (int i = 0; i < selectColors.Count; i++)
        {
            //Debug.Log("Comparing: " + c + " - " + selectColors[i] + " -> " + selectColors[i].Compare(c));
            if (selectColors[i].Compare(c))
            {
                currentColor = selectColors[i];
                index = i;
                UpdateUI();
                return;
            }
        }
    }
    public void SetCurrentColor(int index)
    {
        if (selectColors == null)
            return;
        if (index >= 0 && index < selectColors.Count)
        {
            currentColor = selectColors[index];
            this.index = index;
            UpdateUI();
        }
    }
    public virtual void ChangeOption(int direction)
    {
        if (selectColors != null ? selectColors.Count > 0 : false)
        {
            if (direction > 0)
                index++;
            if (direction < 0)
                index--;
            if (index >= selectColors.Count)
            {
                if (cycle)
                    index = 0;
                else
                    index = selectColors.Count - 1;
            }
            else if (index < 0)
            {
                if (cycle)
                    index = selectColors.Count - 1;
                else
                    index = 0;
            }
            currentColor = selectColors[index];
            if (OnColorSelect != null)
                OnColorSelect.Invoke(currentColor);
            UpdateUI();
        }
    }

    public override void UpdateUI()
    {

        if (selectColors != null ? selectColors.Count > 0 : false)
        {
            if (decreaseButton)
                decreaseButton.SetActive(true);
            if (increaseButton)
                increaseButton.SetActive(true);

            if (!cycle)
            {
                if (index == 0)
                    decreaseButton?.SetActive(false);
                if (index == selectColors.Count - 1)
                    increaseButton?.SetActive(false);
            }
            currentColor = selectColors[index];
            image.color = currentColor;
#if UNITY_EDITOR
            EditorUtility.SetDirty(image);
#endif
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
}
