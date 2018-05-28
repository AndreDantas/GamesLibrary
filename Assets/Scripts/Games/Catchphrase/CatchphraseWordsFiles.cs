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
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static List<string> GetWordsFromFile(TextAsset file)
    {
        if (file == null)
            return new List<string>();
        return file.ToString().RemoveLineEndings().Split(',').ToList();
    }
}
