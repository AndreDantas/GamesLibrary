using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChangeTextLanguage : MonoBehaviour
{



#if UNITY_EDITOR
    void CheckForTextComponent()
    {
        var components = gameObject.GetComponents(typeof(Component));
        bool hasText = false;
        foreach (var component in components)
        {
            if (component is Text || component is TextMeshProUGUI)
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
