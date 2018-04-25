using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
[System.Serializable]
public enum LerpMode
{
    EaseIn,
    EaseOut,
    Exponential,
    Smoothstep,
    None

}
public static class UtilityFunctions
{

    public static float ClampMax(float value, float maxValue)
    {

        if (value > maxValue)
            value = maxValue;
        return value;
    }
    public static float ClampMin(float value, float minValue)
    {
        if (value < minValue)
            value = minValue;
        return value;
    }
    public static int ClampMax(int value, int maxValue)
    {
        if (value > maxValue)
            value = maxValue;
        return value;
    }
    public static int ClampMin(int value, int minValue)
    {
        if (value < minValue)
            value = minValue;
        return value;
    }
    public static void ClampToWindow(this RectTransform panelRectTransform, RectTransform parentRectTransform)
    {
        if (parentRectTransform == null)
            return;
        Vector3 pos = panelRectTransform.localPosition;

        Vector3 minPosition = parentRectTransform.rect.min - panelRectTransform.rect.min;
        Vector3 maxPosition = parentRectTransform.rect.max - panelRectTransform.rect.max;

        pos.x = Mathf.Clamp(panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);

        panelRectTransform.localPosition = pos;
    }

    public static void ClampToCanvas(this RectTransform panel)
    {

        Canvas[] components = panel.gameObject.GetComponentsInParent<Canvas>();
        if (components != null ? components.Length > 0 : false)
        {
            Canvas canvas = components[components.Length - 1];
            if (canvas)
                ClampToWindow(panel, canvas.transform as RectTransform);
        }
    }

    public static int Sign(int value)
    {
        if (value >= 0)
            return 1;
        else
            return -1;
    }
    public static float ScreenWidth
    {
        get
        {
            return ScreenHeight * Screen.width / Screen.height;
        }
    }

    public static float ScreenHeight
    {
        get
        {
            return Camera.main.orthographicSize * 2.0f;
        }
    }
    public static float ChangeLerpT(LerpMode lerpMode = LerpMode.EaseOut, float t = 0f)
    {
        switch (lerpMode)
        {
            case LerpMode.EaseIn:
                t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                break;
            case LerpMode.EaseOut:
                t = Mathf.Sin(t * Mathf.PI * 0.5f);
                break;
            case LerpMode.Exponential:
                t = t * t;
                break;
            case LerpMode.Smoothstep:
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                break;
            case LerpMode.None:
                break;
            default:
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                break;
        }
        return t;
    }

    public static float Map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    /// <summary>
    /// Converts an array to a list.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="array">The array to convert to a list.</param>
    /// <returns></returns>
    public static List<T> ToList<T>(this T[] array)
    {
        List<T> result = new List<T>();
        foreach (T t in array)
        {
            result.Add(t);
        }
        return result;
    }

    /// <summary>
    /// Tries to get a component in GameObject, adds new one if not found.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="g"></param>
    /// <returns></returns>
    public static Component GetOrAddComponent<T>(this GameObject g) where T : Component
    {
        if (g.GetComponent<T>() == null)
            return g.AddComponent<T>();
        else
            return g.GetComponent<T>();
    }

    /// <summary>
    /// Draws bounds on a DrawGizmos function.
    /// </summary>
    /// <param name="b"></param>
    public static void DrawBounds(Bounds b)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(b.center, b.size);
    }

    /// <summary>
    /// Checks if a component is present on GameObject.
    /// </summary>
    /// <typeparam name="T">The component type.</typeparam>
    /// <param name="g"></param>
    /// <returns></returns>
    public static bool CheckForComponent<T>(this GameObject g) where T : Component
    {
        bool hasComponent = false;
        foreach (Component comp in g.GetComponents<Component>())
        {

            if (comp is T)
            {
                hasComponent = true;
                break;
            }
        }

        return hasComponent;
    }

    public static void DestroySelf(this Object c)
    {
        Object.Destroy(c);
    }

    public static Vector3 RoundVector3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }

    public static Vector2 RoundVector2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    /// <summary>
    /// Destroys all child objects on transform.
    /// </summary>
    /// <param name="parent"></param>
    public static void DestroyChildren(this Transform parent)
    {
        List<GameObject> destroyList = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            destroyList.Add(child.gameObject);
        }

        for (int i = destroyList.Count - 1; i >= 0; i--)
        {
            Object.Destroy(destroyList[i]);
        }

    }



    /// <summary>
    /// Checks if the pointer was over a UI element.
    /// </summary>
    /// <returns></returns>
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}