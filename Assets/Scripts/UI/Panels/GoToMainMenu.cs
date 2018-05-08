using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
public class GoToMainMenu : MonoBehaviour
{

    public Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(LoadMainMenu);
    }

    public void LoadMainMenu()
    {

        SceneController sc = FindObjectOfType<SceneController>();
        if (sc)
            ModalWindow.Choice("Ir para o menu principal?", new List<UnityAction>() { sc.GoToMainMenu, this.DestroySelf }, null);
    }

#if UNITY_EDITOR

    protected virtual void Reset()
    {
        if (!gameObject.CheckForComponent<Button>())
        {
            UnityEditor.EditorUtility.DisplayDialog("Button component not present!",
              string.Format("The component {0} can't be added because a Button component isn't present.", this.GetType()),
              "Cancel");

            DestroyImmediate(this);
        }

    }
#endif
}
