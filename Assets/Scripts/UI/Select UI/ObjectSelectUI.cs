using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
[System.Serializable]
public class GameObjectAndObjectPair
{
    public GameObject gameObj;
    public object obj;
}
[System.Serializable]
public class OnObjectSelectChanged : UnityEvent<object>
{

}
public class ObjectSelectUI : SelectUI
{
    public List<GameObjectAndObjectPair> objects = new List<GameObjectAndObjectPair>();
    [Space(15)]
    public OnObjectSelectChanged OnObjectChanged;

    private void Start()
    {
        if (OnObjectChanged != null)
            OnObjectChanged.Invoke(objects[index]?.obj);
    }

    public virtual void SetOptionsObjects(List<object> objects)
    {
        if (this.objects == null)
            return;
        List<object> objs = this.objects.Select(o => o.obj).ToList();
        objs = objs.FillList(objects);
        for (int i = 0; i < this.objects.Count; i++)
        {
            this.objects[i].obj = objs[i];
        }
    }

    public virtual void ChangeOption(int direction)
    {
        if (objects != null ? objects.Count > 0 : false)
        {
            if (direction > 0)
                index++;
            if (direction < 0)
                index--;
            if (index >= objects.Count)
            {
                if (cycle)
                    index = 0;
                else
                    index = objects.Count - 1;
            }
            else if (index < 0)
            {
                if (cycle)
                    index = objects.Count - 1;
                else
                    index = 0;
            }
            if (OnObjectChanged != null)
                OnObjectChanged.Invoke(objects[index].obj);
            UpdateUI();
        }
    }
    public override void UpdateUI()
    {
        if (valueText)
            valueText.gameObject.SetActive(false);
        if (objects != null ? objects.Count > 0 : false)
        {
            if (decreaseButton)
                decreaseButton.SetActive(true);
            if (increaseButton)
                increaseButton.SetActive(true);
            foreach (GameObjectAndObjectPair gpair in objects)
            {
                gpair.gameObj?.SetActive(false);
            }
            if (!cycle)
            {
                if (index == 0)
                    decreaseButton.SetActive(false);
                if (index == objects.Count - 1)
                    increaseButton.SetActive(false);
            }
            objects[index].gameObj?.SetActive(true);
        }
        else
        {
            if (valueText)
                valueText.text = "";

            if (decreaseButton)
                decreaseButton.SetActive(false);
            if (increaseButton)
                increaseButton.SetActive(false);
        }
    }

    public virtual object GetCurrentValue()
    {
        if (objects != null ? objects.Count > 0 : false)
        {
            if (index < objects.Count && index >= 0)
                return objects[index].obj;

        }
        return null;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    public void ClearOptions()
    {
        objects.Clear();
        UpdateUI();
    }
}
