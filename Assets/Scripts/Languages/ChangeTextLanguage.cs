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
    [MultiLineProperty(10)]
    public string text;

    public PairLanguageText(SystemLanguage language, string text)
    {
        this.language = language;
        this.text = text;
    }


    public static implicit operator string(PairLanguageText t)
    {
        return t.text;
    }

}
[DisallowMultipleComponent]
public class ChangeTextLanguage : MonoBehaviour
{
    protected TextMeshProUGUI textTMP;
    protected Text text;
    public List<PairLanguageText> languagesAndText = new List<PairLanguageText>();
    private void Start()
    {
        textTMP = GetComponent<TextMeshProUGUI>();
        text = GetComponent<Text>();

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
                    if (textTMP)
                        textTMP.text = lt.text;
                    if (text)
                        text.text = lt.text;
                    return;
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
            if (component is TextMeshProUGUI || component is Text)
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
