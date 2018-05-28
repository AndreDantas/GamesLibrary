using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class ErrorTextController : MonoBehaviour
{

    public TextMeshProUGUI errorText;
    [ShowInInspector]
    public float errorDuration { get; internal set; } = 5f;
    protected float elapsedTime;
    protected bool showingError = false;

    protected virtual void Start()
    {
        if (!errorText)
            errorText = GetComponent<TextMeshProUGUI>();
        HideError();
    }
    private void Update()
    {
        if (errorText == null)
            return;
        if (showingError)
        {
            errorText.enabled = true;
            if (errorDuration > 0f)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime > errorDuration)
                {
                    elapsedTime = 0;
                    showingError = false;
                    errorText.enabled = false;
                    return;
                }

            }
        }
        else
        {

            HideError();

        }
    }

    public virtual void ShowError(string errorMsg, float duration = 5f)
    {
        if (errorText == null)
            return;
        showingError = true;
        errorText.text = errorMsg;
        errorDuration = UtilityFunctions.ClampMin(duration, 0f);
        elapsedTime = 0f;
    }

    public virtual void HideError()
    {
        if (errorText == null)
            return;
        errorText.enabled = false;
    }
}
