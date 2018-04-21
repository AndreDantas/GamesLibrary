using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonContent : MonoBehaviour
{
    protected bool _open;
    public ButtonContentGroup group;
    public virtual bool open
    {
        get
        {
            return _open;
        }
        set
        {
            if (_open == value)
                return;
            _open = value;
            if (group)
            {
                if (_open)
                {

                    group.OnContentOpen(this);
                }
                else
                {

                    group.OnContentClose(this);
                }
            }

        }
    }

    public abstract void Open();
    public abstract void Close();

    public virtual void Toggle()
    {
        if (open)
            Close();
        else
            Open();
    }
}
