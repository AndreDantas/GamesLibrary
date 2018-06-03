using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public struct ToggleLanguage
{
    public Toggle toggle;
    public SystemLanguage language;
}
public class LanguageOptionsController : MonoBehaviour
{

    public List<ToggleLanguage> toggleLanguages = new List<ToggleLanguage>();

    private void OnEnable()
    {
        foreach (var item in toggleLanguages)
        {
            if (item.language == GameLanguage.language.systLanguage)
                item.toggle.isOn = true;
            else
                item.toggle.isOn = false;
        }
    }
}
