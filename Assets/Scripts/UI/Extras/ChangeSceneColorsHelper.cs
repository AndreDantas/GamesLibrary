using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class ChangeSceneColorsHelper : MonoBehaviour
{

    public Color findColor;
    public Color replaceColor;
    [ButtonGroup("g1")]
    [Button(ButtonSizes.Large)]
    public void ReplaceColors()
    {
        foreach (var item in UtilityFunctions.FindObjectsOfTypeAll<Image>())
        {
            if (item.color.Compare(findColor))
                item.color = replaceColor.RGB();
        }
        foreach (var item in UtilityFunctions.FindObjectsOfTypeAll<TextMeshProUGUI>())
        {

            if (item.color.Compare(findColor))
                item.color = replaceColor.RGB();
        }
    }
    [ButtonGroup("g1")]
    [Button(ButtonSizes.Large)]
    public void RevertColors()
    {
        foreach (var item in UtilityFunctions.FindObjectsOfTypeAll<Image>())
        {
            if (item.color.Compare(replaceColor))
                item.color = findColor.RGB();
        }
        foreach (var item in UtilityFunctions.FindObjectsOfTypeAll<TextMeshProUGUI>())
        {
            if (item.color.Compare(replaceColor))
                item.color = findColor.RGB();
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
