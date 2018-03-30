using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[System.Serializable]
public struct FocusObjInfo
{
    public GameObject focusObj;
    public Transform parent;
    public Vector3 originLocalPos;
    public int hierarchyIndex;
}
public class ObjectFocus : MonoBehaviour
{
    public static ObjectFocus instance;
    public GameObject focusBackground;
    public delegate void OnFocus();
    public OnFocus OnEnableFocus;
    public OnFocus OnDisableFocus;
    protected bool lockBackground = false;
    List<FocusObjInfo> focusObjects;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        DisableFocus();
    }

    public void SetFocusObjects(List<GameObject> objects)
    {
        focusObjects = new List<FocusObjInfo>();
        foreach (GameObject obj in objects)
        {
            FocusObjInfo temp = new FocusObjInfo();
            temp.parent = obj.transform.parent;
            temp.originLocalPos = obj.transform.localPosition;
            temp.focusObj = obj;
            temp.hierarchyIndex = obj.transform.GetSiblingIndex();
            focusObjects.Add(temp);
        }
    }

    private void OnEnable()
    {
        SceneController.OnBack += BackgroundClick;
    }

    private void OnDisable()
    {
        SceneController.OnBack -= BackgroundClick;
        DisableFocus();
    }

    public void BackgroundClick()
    {
        if (!lockBackground)
            DisableFocus();
    }

    public void EnableFocus(bool lockBg = false)
    {
        if (focusObjects != null ? focusObjects.Count == 0 : true)
            return;
        lockBackground = lockBg;
        foreach (FocusObjInfo obj in focusObjects)
        {
            if (obj.focusObj)
            {
                obj.focusObj.transform.SetParent(transform);

            }
        }
        SceneController.instance.SetCanMove(false);
        if (focusBackground)
            focusBackground.SetActive(true);
        if (OnEnableFocus != null)
            OnEnableFocus();

    }

    public void DisableFocus()
    {
        if (focusBackground)
            focusBackground.SetActive(false);

        if (focusObjects != null ? focusObjects.Count == 0 : true)
            return;
        foreach (FocusObjInfo obj in focusObjects)
        {
            if (obj.focusObj)
            {
                obj.focusObj.transform.SetParent(obj.parent);
                obj.focusObj.transform.SetSiblingIndex(obj.hierarchyIndex);
            }
        }
        SceneController.instance.SetCanMove(true);
        if (OnDisableFocus != null)
            OnDisableFocus();
        lockBackground = false;
    }


}
