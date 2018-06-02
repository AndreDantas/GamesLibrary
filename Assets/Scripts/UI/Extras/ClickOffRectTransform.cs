using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;
public class ClickOffRectTransform : MonoBehaviour
{

    [SerializeField] bool shouldStartVisible;

    GameObject _myGameObj;
    GameObject _invisibleBG;

    public UnityEvent OnClickOff;


    void Awake()
    {
        setupInvisibleBG();
        _myGameObj = gameObject;
        if (!shouldStartVisible)
            HideInvisibleBG();
    }

    void setupInvisibleBG()
    {
        _invisibleBG = new GameObject("InvisibleBG");

        InvisibleBGCtrl tempInvisibleBGCtrl = _invisibleBG.AddComponent<InvisibleBGCtrl>();
        tempInvisibleBGCtrl.setParentCtrl(this);

        Image tempImage = _invisibleBG.AddComponent<Image>();
        tempImage.color = new Color(1f, 1f, 1f, 0f);

        RectTransform tempTransform = _invisibleBG.GetComponent<RectTransform>();
        tempTransform.anchorMin = new Vector2(0f, 0f);
        tempTransform.anchorMax = new Vector2(1f, 1f);
        tempTransform.offsetMin = new Vector2(0f, 0f);
        tempTransform.offsetMax = new Vector2(0f, 0f);
        tempTransform.SetParent(GetComponentsInParent<Transform>()[1], false);
        tempTransform.SetSiblingIndex(transform.GetSiblingIndex()); // put it right beind this panel in the hierarchy
    }

    void OnEnable()
    {
        _invisibleBG.SetActive(true);
    }

    public virtual void ClickOff()
    {
        OnClickOff?.Invoke();
        //_myGameObj.SetActive(false);

    }

    public virtual void HideInvisibleBG()
    {
        _invisibleBG.SetActive(false);
    }
    public virtual void ShowInvisibleBG()
    {
        _invisibleBG.SetActive(true);
    }
}
