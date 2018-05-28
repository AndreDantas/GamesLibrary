using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using WordListUI;
using UnityEngine.Events;

public class EditableWord : Word
{

    public TMP_InputField input;
    public TMP_InputField.CharacterValidation characterValidation = TMP_InputField.CharacterValidation.Name;
    public Button editButton;
    public Button deleteButton;

    private void Start()
    {
        if (input)
        {
            input.interactable = false;
            input.enabled = false;
            input.onSubmit.RemoveAndAddListener(OnInputSubmit);
            input.onDeselect.RemoveAndAddListener(x => input.text = currentText);
            input.characterValidation = characterValidation;
        }

        if (editButton)
        {

            editButton.onClick.RemoveAndAddListener(EditText);

        }
        if (deleteButton)
        {
            deleteButton.onClick.RemoveAndAddListener(gameObject.DestroySelf);
        }

        if (text)
            currentText = text.text;
    }

    public void EditText()
    {
        if (!input)
            return;
        input.enabled = true;
        input.interactable = true;
        input.Select();
        input.ActivateInputField();
    }

    public override void SetText(string text)
    {

        if (input)
        {

            input.text = text.FirstCharToUpper();
            currentText = text.FirstCharToUpper();
        }

    }

    public void OnInputSubmit(string newText)
    {
        input.enabled = false;
        //input.interactable = false;

        if (currentText.Trim().ToLower().RemoveZeroWidthSpace() != newText.Trim().ToLower().RemoveZeroWidthSpace())
        {
            string oldText = currentText.Trim().RemoveZeroWidthSpace();
            currentText = newText;
            onTextChanged?.Invoke(this, oldText.RemoveZeroWidthSpace(), newText.RemoveZeroWidthSpace());

        }
    }
}
