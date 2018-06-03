using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class ToggleEvents : MonoBehaviour
{
    public UnityEvent SwitchOn;
    [Space(10)]
    public UnityEvent SwitchOff;
    Toggle toggle;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        if (toggle)
        {
            toggle.onValueChanged.RemoveAndAddListener(OnToggleChanged);
        }
    }

    void OnToggleChanged(bool toggle)
    {
        if (toggle)
            SwitchOn?.Invoke();
        else
            SwitchOff?.Invoke();
    }

#if UNITY_EDITOR


    protected virtual void Reset()
    {

        if (!gameObject.CheckForComponent<Toggle>())
        {
            UnityEditor.EditorUtility.DisplayDialog("Toggle component not present!",
               string.Format("The component {0} can't be added because a Toggle component isn't present.", this.GetType()),
               "Cancel");
            DestroyImmediate(this);
        }

    }
#endif
}
