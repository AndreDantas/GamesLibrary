using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;
[DisallowMultipleComponent]
public class AwakeEvents : MonoBehaviour
{
    public bool execute = true;
    [EnableIf("execute")]
    public UnityEvent OnAwake;

    private void Awake()
    {
        if (execute)
            OnAwake?.Invoke();
    }
}
