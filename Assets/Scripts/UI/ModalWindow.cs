using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class ModalWindow : MonoBehaviour
{
    public static ModalWindow instance;
    public GameObject window;
    public TextMeshProUGUI windowText;
    public Button yesButton;
    public Button noButton;
    public Button closeButton;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        CloseWindow();
    }

    public static void Choice(string choiceText, UnityAction yesAction = null, UnityAction noAction = null)
    {
        if (instance.window == null)
            return;
        instance.window.SetActive(true);

        if (instance.yesButton)
        {
            instance.yesButton.gameObject.SetActive(true);
            instance.yesButton.onClick.RemoveAllListeners();
            if (yesAction != null)
                instance.yesButton.onClick.AddListener(yesAction);
            instance.yesButton.onClick.AddListener(instance.CloseWindow);
        }
        if (instance.noButton)
        {
            instance.noButton.gameObject.SetActive(true);
            instance.noButton.onClick.RemoveAllListeners();
            if (noAction != null)
                instance.noButton.onClick.AddListener(noAction);
            instance.noButton.onClick.AddListener(instance.CloseWindow);
        }
        if (instance.closeButton)
        {
            instance.closeButton.gameObject.SetActive(false);
        }

        if (instance.windowText)
            instance.windowText.text = choiceText;
        LayoutRebuilder.ForceRebuildLayoutImmediate(instance.window.GetComponentInChildren<LayoutGroup>().gameObject.transform as RectTransform);
        SceneController.LockPanel();

    }

    public static void Message(string msg, UnityAction closeAction = null)
    {
        if (instance.window == null)
            return;
        if (instance.yesButton)
        {
            instance.yesButton.gameObject.SetActive(false);
        }
        if (instance.noButton)
        {
            instance.noButton.gameObject.SetActive(false);
        }
        if (instance.closeButton)
        {
            instance.closeButton.gameObject.SetActive(true);
            instance.closeButton.onClick.RemoveAllListeners();
            if (closeAction != null)
                instance.closeButton.onClick.AddListener(closeAction);
            instance.closeButton.onClick.AddListener(instance.CloseWindow);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(instance.window.GetComponentInChildren<LayoutGroup>().gameObject.transform as RectTransform);
        if (instance.windowText)
            instance.windowText.text = msg;
    }

    void CloseWindow()
    {
        if (window ? window.activeSelf == true : false)
        {
            window.SetActive(false);
            SceneController.UnlockPanel();
        }
    }

    public static bool IsActive()
    {
        if (instance)
        {
            if (instance.window)
                return instance.window.activeSelf;
        }

        return false;
    }
}
