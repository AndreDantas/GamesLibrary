using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using CielaSpike;
using System;
namespace WordListUI
{
    public class WordList : MonoBehaviour
    {
        /// <summary>
        /// Reference to the ScrollRect
        /// </summary>
        public ScrollRect scroll;

        /// <summary>
        /// The GameObject where the words will be added.
        /// </summary>
        public GameObject content;

        /// <summary>
        /// Overlay when loading the words.
        /// </summary>
        public GameObject loadingOverlay;

        /// <summary>
        /// True if the application is quitting.
        /// </summary>
        bool quitting;

        /// <summary>
        /// The prefab of the word. (Needs to have a Word component)
        /// </summary>
        public GameObject wordPrefab;


        /// <summary>
        /// If the same word can be added again.
        /// </summary>
        public bool allowDuplicates = false;
        /// <summary>
        /// If empty strings are allowed.
        /// </summary>
        public bool allowEmpty = false;

        protected bool loadingList;

        [ReadOnly, SerializeField]
        protected List<string> currentWords = new List<string>();


        public delegate void OnWordAddedEventHandler(Word w, string word);
        /// <summary>
        /// Event when a word is added to the list.
        /// </summary>
        public OnWordAddedEventHandler onWordAdded;

        public delegate void OnWordChangedEventHandler(Word w, string oldWord, string newWord);
        /// <summary>
        /// Event when a word changes.
        /// </summary>
        public OnWordChangedEventHandler onWordChanged;

        public delegate void OnWordRemovedEventHandler(string word);
        /// <summary>
        /// Event when a word is removed.
        /// </summary>
        public OnWordRemovedEventHandler onWordRemoved;


        private void OnApplicationQuit()
        {
            quitting = true;
        }

        protected virtual void Start()
        {
            if (loadingOverlay)
                loadingOverlay.Deactivate();

        }

        public virtual void AddWord(string word)
        {
            if (!content || !wordPrefab || (currentWords.Contains(word) && !allowDuplicates))
                return;
            if (word.Trim() == "" && !allowEmpty)
                return;

            var wordObj = Instantiate(wordPrefab, content.transform);

            var w = wordObj.GetComponent<Word>();
            if (w)
            {
                w.SetText(word.FirstCharToUpper());
                w.onDestroy += OnWordDestroy;
                w.onTextChanged.RemoveAndAddListener(OnWordChanged);
                currentWords.Add(word);
                onWordAdded?.Invoke(w, word);
            }

        }

        public virtual void ResetList()
        {
            if (content)
                content.transform.DestroyChildren<Word>();
            currentWords = new List<string>();
            loadingList = false;
        }

        Task task;
        public virtual void AddList(List<string> words)
        {
            if (task?.State == TaskState.Running)
                return;
            task?.Cancel();
            StopAllCoroutines();
            if (!content || !wordPrefab)
                return;
            if (!wordPrefab.CheckForComponent<Word>())
                return;
            if (words == null ? true : words.Count == 0)
                return;
            StartCoroutine(AddWords(words));


        }


        protected virtual IEnumerator AddWords(List<string> words)
        {
            loadingList = true;
            if (loadingOverlay)
                loadingOverlay.Activate();
            this.StartCoroutineAsync(AddWordsToList(words), out task);

            while (task.State != TaskState.Done)
                yield return null;
            if (loadingOverlay)
                loadingOverlay.Deactivate();
            loadingList = false;
        }

        protected virtual IEnumerator AddWordsToList(List<string> words)
        {
            if (words == null)
                yield break;

            for (int i = 0; i < words.Count; i++)
            {
                yield return Ninja.JumpToUnity;
                AddWord(words[i]);
                yield return Ninja.JumpBack;
            }
        }

        public virtual void ChangeWord(Word w, string oldWord, string newWord)
        {
            if (!w)
                return;


            for (int i = 0; i < currentWords.Count; i++)
            {
                if (currentWords[i].Equals(oldWord.RemoveZeroWidthSpace(), StringComparison.InvariantCultureIgnoreCase))
                {
                    currentWords[i] = newWord.RemoveZeroWidthSpace();
                    w.SetText(newWord);

                    break;
                }

            }




        }

        protected virtual void OnWordDestroy(Word wordObj, string s)
        {
            if (quitting)
                return;
            for (int i = 0; i < currentWords.Count; i++)
            {
                if (currentWords[i].Equals(s.RemoveZeroWidthSpace(), StringComparison.InvariantCultureIgnoreCase))
                {
                    currentWords.RemoveAt(i);
                    onWordRemoved?.Invoke(s);
                    break;
                }

            }

        }

        protected virtual void OnWordChanged(object sender, string oldWord, string newWord)
        {
            Word w = (Word)sender;
            if (!w)
                return;
            if ((currentWords.ContainsIgnoreCase(newWord.RemoveZeroWidthSpace()) && !allowDuplicates) ||
                (newWord.Trim().RemoveZeroWidthSpace() == "" && !allowEmpty))
            {
                w.SetText(oldWord);
            }
            else
            {
                for (int i = 0; i < currentWords.Count; i++)
                {
                    if (currentWords[i].Equals(oldWord.RemoveZeroWidthSpace(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        currentWords[i] = newWord.RemoveZeroWidthSpace();
                        onWordChanged?.Invoke(w, oldWord, newWord);
                        break;
                    }

                }

            }

        }
    }
}
