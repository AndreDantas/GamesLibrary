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

    private void OnDisable()
    {
        DisableFocus();
    }

    public void EnableFocus()
    {
        if (focusObjects != null ? focusObjects.Count == 0 : true)
            return;
        foreach (FocusObjInfo obj in focusObjects)
        {
            if (obj.focusObj)
            {
                obj.focusObj.transform.SetParent(transform);

            }
        }

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

        if (OnDisableFocus != null)
            OnDisableFocus();

    }
}
