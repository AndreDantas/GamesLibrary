using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;
[System.Serializable]
public class OnTextChanged : UnityEvent<object, string, string>
{

}
namespace WordListUI
{
    public class Word : MonoBehaviour
    {

        public TextMeshProUGUI text;
        public Image bg;
        [ReadOnly, PropertyOrder(-1)]
        public string currentText;
        [Space(10), PropertyOrder(99)]
        public OnTextChanged onTextChanged;

        public delegate void OnWordDestroyEventHandler(Word sender, string word);
        public OnWordDestroyEventHandler onDestroy;

        public virtual void SetText(string text)
        {
            if (this.text)
            {
                this.text.text = text;
                currentText = text;
            }

        }

        protected virtual void OnDestroy()
        {
            onDestroy?.Invoke(this, text.text.RemoveZeroWidthSpace());
        }
    }


}
