using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class PairLanguageImage
{
    public SystemLanguage language;
    [MultiLineProperty(10)]
    public Sprite image;

    public PairLanguageImage(SystemLanguage language, Sprite img)
    {
        this.language = language;
        this.image = img;
    }

}
[DisallowMultipleComponent]
public class ChangeImageLanguage : MonoBehaviour
{

    protected Image image;

    public List<PairLanguageImage> languagesAndText = new List<PairLanguageImage>();
    private void Start()
    {
        image = GetComponent<Image>();
        ChangeLanguage();
    }



    public void ChangeLanguage()
    {
        if (languagesAndText != null ? languagesAndText.Count > 0 : false)
        {
            foreach (PairLanguageImage lt in languagesAndText)
            {
                if (lt.language == GameLanguage.language.systLanguage)
                {
                    image.sprite = lt.image;
                    return;
                }
            }
        }

    }


#if UNITY_EDITOR
    void CheckForImageComponent()
    {
        var components = gameObject.GetComponents(typeof(Component));
        bool hasImage = false;
        foreach (var component in components)
        {
            if (component is Image)
                hasImage = true;


        }
        if (!hasImage)
        {
            UnityEditor.EditorUtility.DisplayDialog("Image component not present!",
               string.Format("The component {0} can't be added because a Image component isn't present.", this.GetType()),
               "Cancel");

            DestroyImmediate(this);
        }
    }

    protected virtual void Reset()
    {
        Invoke("CheckForImageComponent", 0);
    }
#endif
}
