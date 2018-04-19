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

    private void Start()
    {
        CloseWindow();
    }
    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
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
            instance.yesButton.onClick.AddListener(instance.CloseWindow);
            if (yesAction != null)
                instance.yesButton.onClick.AddListener(yesAction);

        }
        if (instance.noButton)
        {
            instance.noButton.gameObject.SetActive(true);
            instance.noButton.onClick.RemoveAllListeners();
            instance.yesButton.onClick.AddListener(instance.CloseWindow);
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

    public static void Choice(string choiceText, List<UnityAction> yesActions, List<UnityAction> noActions)
    {
        if (instance.window == null)
            return;
        instance.window.SetActive(true);

        if (instance.yesButton)
        {
            instance.yesButton.gameObject.SetActive(true);
            instance.yesButton.onClick.RemoveAllListeners();
            instance.yesButton.onClick.AddListener(instance.CloseWindow);
            if (yesActions != null)
                foreach (UnityAction action in yesActions)
                {
                    if (action != null)
                    {

                        instance.yesButton.onClick.AddListener(action);
                    }
                }

        }
        if (instance.noButton)
        {
            instance.noButton.gameObject.SetActive(true);
            instance.noButton.onClick.RemoveAllListeners();
            instance.noButton.onClick.AddListener(instance.CloseWindow);
            if (noActions != null)
                foreach (UnityAction action in noActions)
                {
                    if (action != null)
                        instance.noButton.onClick.AddListener(action);
                }

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

    public static void Message(string msg, List<UnityAction> closeAction)
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
                foreach (UnityAction action in closeAction)
                {
                    if (action != null)
                        instance.closeButton.onClick.AddListener(action);
                }
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
