using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonContentGroup : MonoBehaviour
{
    public List<ButtonContent> buttons = new List<ButtonContent>();

    private void Awake()
    {
        if (buttons != null)
        {
            foreach (ButtonContent b in buttons)
                b.group = this;
        }
    }
    public virtual void OnContentClose(ButtonContent button)
    {
        UpdateGroupLayout();
    }
    public virtual void OnContentOpen(ButtonContent button)
    {
        if (buttons == null)
            return;
        foreach (ButtonContent b in buttons)
        {
            if (b != button && b.open)
            {
                b.Close();
            }
        }

        UpdateGroupLayout();
    }

    public virtual void UpdateGroupLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}
