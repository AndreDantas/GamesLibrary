using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public struct WordListInfo
{
    public TextAsset wordList;
    public SystemLanguage language;
}
public class CatchphraseWordsFiles : MonoBehaviour
{

    public static CatchphraseWordsFiles instance;
    public List<WordListInfo> wordLists = new List<WordListInfo>();
    public List<string> extraWords = new List<string>();
    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        extraWords = new List<string>();
        instance = this;
        LoadWords();
    }

    public static List<string> GetWordsFromFile(TextAsset file)
    {
        if (file == null)
            return new List<string>();
        return file.ToString().RemoveLineEndings().Split(',').ToList();
    }


    public static void LoadWords()
    {
        string path = "catchprase_extra_words_" + GameLanguage.language.systLanguage.ToString().ToLower() + ".dat";

        instance.extraWords = SaveLoad.LoadFile<List<string>>(path);



    }

    public static void SaveWords()
    {
        string path = "catchprase_extra_words_" + GameLanguage.language.systLanguage.ToString().ToLower() + ".dat";
        SaveLoad.SaveFile(path, instance.extraWords);

    }
}
