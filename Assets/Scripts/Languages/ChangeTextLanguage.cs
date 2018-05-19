using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
[System.Serializable]
public class PairLanguageText
{
    public SystemLanguage language;
    [TextArea]
    public string text;
}
public class ChangeTextLanguage : MonoBehaviour
{
    protected TextMeshProUGUI text;
    public List<PairLanguageText> languagesAndText = new List<PairLanguageText>();
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        ChangeLanguage();
    }



    public void ChangeLanguage()
    {
        if (languagesAndText != null ? languagesAndText.Count > 0 : false)
        {
            foreach (PairLanguageText lt in languagesAndText)
            {
                if (lt.language == GameLanguage.language.systLanguage)
                {
                    text.text = lt.text;
                    break;
                }
            }
        }

    }


#if UNITY_EDITOR
    void CheckForTextComponent()
    {
        var components = gameObject.GetComponents(typeof(Component));
        bool hasText = false;
        foreach (var component in components)
        {
            if (component is TextMeshProUGUI)
                hasText = true;


        }
        if (!hasText)
        {
            UnityEditor.EditorUtility.DisplayDialog("Text component not present!",
               string.Format("The component {0} can't be added because a Text component isn't present.", this.GetType()),
               "Cancel");

            DestroyImmediate(this);
        }
    }

    protected virtual void Reset()
    {
        Invoke("CheckForTextComponent", 0);
    }
#endif
}
