using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public static class GameLanguage
{
    public static LanguageObj language;
    public static readonly SystemLanguage DEFAULT_LANGUAGE = SystemLanguage.Portuguese;
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

    private void Awake()
    {
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

    public static void ChangeObjectsLanguage()
    {

    }
}
