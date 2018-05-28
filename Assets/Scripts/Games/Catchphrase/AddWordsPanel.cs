using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using WordListUI;
using System;
public class AddWordsPanel : GamePanel
{

    public WordList wordList;
    public TMP_InputField addWordInput;
    public ErrorTextController errorController;

    [HideInInspector]
    public static List<string> addedWords;
    [HideInInspector]
    public List<string> defaultWords;

    public override IEnumerator Enter()
    {

        yield return base.Enter();
        LoadWords();
        if (wordList)
        {
            wordList.onWordChanged += OnWordChanged;
            wordList.onWordRemoved += OnWordRemoved;
            wordList.onWordAdded += OnWordAdded;
            wordList.scroll.verticalNormalizedPosition = 1;
        }
    }

    public override IEnumerator Exit()
    {
        if (wordList)
        {
            wordList.onWordChanged -= OnWordChanged;
            wordList.onWordRemoved -= OnWordRemoved;
            wordList.onWordAdded -= OnWordAdded;

        }
        return base.Exit();
    }

    bool ValidateInput(string word, bool showErrors = true)
    {
        if (wordList == null)
            return false;
        if (word == "" && !wordList.allowEmpty)
        {
            if (errorController && showErrors)
                errorController.ShowError(SystemTranslations.EMPTY_INPUT_ERROR.Get(), 5f);
            return false;
        }
        if ((addedWords.ContainsIgnoreCase(word) && !wordList.allowDuplicates) || defaultWords.ContainsIgnoreCase(word))
        {
            if (errorController && showErrors)
                errorController.ShowError(SystemTranslations.REPEATED_WORD_INPUT_ERROR.Get(), 5f);
            return false;
        }

        return true;
    }

    public void AddWordToList()
    {
        if (wordList == null || addWordInput == null)
            return;

        string word = addWordInput.text.Trim().RemoveZeroWidthSpace();

        if (!ValidateInput(word))
            return;

        if (addedWords == null)
            addedWords = new List<string>();
        addedWords.Add(word);
        wordList.AddWord(word);
        SaveWords();
        addWordInput.text = "";
    }

    void OnWordAdded(Word w, string word)
    {

    }


    void OnWordChanged(Word w, string oldWord, string newWord)
    {


        if (!ValidateInput(newWord))
        {

            wordList.ChangeWord(w, newWord, oldWord);
            w.SetText(oldWord);
            return;
        }

        for (int i = 0; i < addedWords.Count; i++)
        {
            if (addedWords[i].Equals(oldWord.RemoveZeroWidthSpace(), StringComparison.InvariantCultureIgnoreCase))
            {
                addedWords[i] = newWord;
                SaveWords();
                return;
            }

        }
    }
    void OnWordRemoved(string word)
    {
        for (int i = 0; i < addedWords.Count; i++)
        {
            if (addedWords[i].Equals(word.RemoveZeroWidthSpace(), StringComparison.InvariantCultureIgnoreCase))
            {
                addedWords.RemoveAt(i);
                SaveWords();
                return;
            }

        }
    }


    void LoadWords()
    {
        string path = "catchprase_extra_words_" + GameLanguage.language.systLanguage.ToString().ToLower() + ".dat";

        addedWords = SaveLoad.LoadFile<List<string>>(path);


        if (wordList)
        {
            wordList.ResetList();
            wordList.AddList(addedWords);
        }



        foreach (var item in CatchphraseWordsFiles.instance.wordLists)
        {
            if (GameLanguage.language.systLanguage == item.language)
            {
                defaultWords = CatchphraseWordsFiles.GetWordsFromFile(item.wordList);
            }
        }
    }

    void SaveWords()
    {
        string path = "catchprase_extra_words_" + GameLanguage.language.systLanguage.ToString().ToLower() + ".dat";
        SaveLoad.SaveFile(path, addedWords);

    }
}
