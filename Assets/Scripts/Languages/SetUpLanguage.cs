using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[System.Serializable]
public static class GameLanguage
{
    public static LanguageObj language;
    public static readonly SystemLanguage DEFAULT_LANGUAGE = SystemLanguage.Portuguese;

    public static string GetTextFromMainLanguage(this List<PairLanguageText> list)
    {
        if (list == null || language == null)
            return "";
        if (list.Count == 0)
            return "";



        foreach (var item in list)
        {
            if (language.systLanguage == item.language)
            {
                return item.text;
            }
        }

        return list[0].text;
    }
    public static string GetText(this List<PairLanguageText> list, SystemLanguage language)
    {
        if (list == null)
            return "";
        if (list.Count == 0)
            return "";



        foreach (var item in list)
        {
            if (language == item.language)
            {
                return item.text;
            }
        }

        return list[0].text;
    }
}
[System.Serializable]
public class LanguageObj
{
    public SystemLanguage systLanguage;

    public LanguageObj(SystemLanguage lng)
    {
        systLanguage = lng;
    }

    public LanguageObj()
    {

    }
}
public class SetUpLanguage : MonoBehaviour
{
    [ValueDropdown("AvailableLanguages")]
    public SystemLanguage DEBUG_LANGUAGE = SystemLanguage.Portuguese;
    public static SetUpLanguage instance;
    public static List<SystemLanguage> AvailableLanguages { get { return new List<SystemLanguage> { SystemLanguage.Portuguese, SystemLanguage.English }; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        LoadLanguage();
    }
    void LoadLanguage()
    {
        LanguageObj l = SaveLoad.LoadFile<LanguageObj>("/language.lng");
        if (l == null)
        {
            GameLanguage.language = new LanguageObj(GameLanguage.DEFAULT_LANGUAGE);
            SaveLanguage();
        }
        else
        {
            GameLanguage.language = l;
        }
    }

    public static void SaveLanguage()
    {
        SaveLoad.SaveFile("/language.lng", GameLanguage.language);
    }

    public static void ChangeLanguage(SystemLanguage newLanguage)
    {
        if (AvailableLanguages.Contains(newLanguage))
        {
            GameLanguage.language = new LanguageObj(newLanguage);
            SaveLanguage();
        }
    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Large)]
    void SetDebugLanguage()
    {
        ChangeLanguage(DEBUG_LANGUAGE);
        if (Application.isPlaying)
            ChangeObjectsLanguage();
    }
#endif
    public static void ChangeObjectsLanguage()
    {
        foreach (var item in GameObject.FindObjectsOfType<ChangeTextLanguage>())
        {
            item.ChangeLanguage();
        }
    }
}
