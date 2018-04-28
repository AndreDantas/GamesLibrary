using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ButtonDropdownContent : ButtonContent
{
    public GameObject content;
    public float animTime = 0.5f;
    protected Vector2 contentSize;
    private void Awake()
    {
        if (content)
        {
            contentSize = ((RectTransform)content.transform).sizeDelta;
            content.SetActive(false);
        }

        open = false;
    }

    public override void Open()
    {
        if (content == null ? true : content.GetComponent<Tweener>() != null)
            return;
        content.SetActive(true);
        RectTransform rect = content.transform as RectTransform;
        rect.sizeDelta = new Vector2(contentSize.x, 0);
        Vector3Tweener tweener = rect.ScaleTo(new Vector2(contentSize.x, contentSize.y), animTime) as Vector3Tweener;
        open = true;
        if (group)
        {
            tweener.OnUpdateAnimation += group.UpdateGroupLayout;
            tweener.OnAnimationComplete += FocusAfterOpen;
            group.OnContentOpen(this);
        }
    }

    public override void Close()
    {
        if (content == null ? true : content.GetComponent<Tweener>() != null)
            return;

        RectTransform rect = content.transform as RectTransform;
        rect.sizeDelta = contentSize;
        Vector3Tweener tweener = rect.ScaleTo(new Vector2(contentSize.x, 0f), animTime) as Vector3Tweener;
        tweener.OnAnimationComplete += OnClose;
        if (group)
        {
            tweener.OnUpdateAnimation += group.UpdateGroupLayout;
        }
        open = false;
    }

    void FocusAfterOpen()
    {
        ScrollRectEnsureVisible s = GetComponentInParent<ScrollRectEnsureVisible>();
        if (s)
        {
            s.CenterOnItem(transform as RectTransform);
        }
    }

    void OnClose()
    {
        if (content)
            content.gameObject.SetActive(false);
        if (group)
            group.OnContentClose(this);
    }
}
