﻿using System.Collections;
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

    /// <summary>
    /// Returns the factorial of the given number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int Factorial(this int number)
    {
        int result = 1;
        while (number != 1)
        {
            result = result * number;
            number = number - 1;
        }
        return result;
    }

    /// <summary>
    /// Returns the sign of the number.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int Sign(int value)
    {
        if (value >= 0)
            return 1;
        else
            return -1;
    }
    /// <summary>
    /// Returns -1 or 1 at random.
    /// </summary>
    /// <returns></returns>
    public static float RandomSign()
    {
        return Random.Range(0, 2) * 2 - 1;
    }

    /// <summary>
    /// The width of the screen.
    /// </summary>
    public static float ScreenWidth
    {
        get
        {
            return ScreenHeight * Screen.width / Screen.height;
        }
    }
    /// <summary>
    /// The height of the screen.
    /// </summary>
    public static float ScreenHeight
    {
        get
        {
            return Camera.main.orthographicSize * 2.0f;
        }
    }

    /// <summary>
    /// Used to modify the "t" value in a lerp function.
    /// </summary>
    /// <param name="lerpMode"></param>
    /// <param name="t"></param>
    /// <returns></returns>
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
    /// Fills a list with the contents of another list, without increasing or decreasing the size.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="l">The list to fill.</param>
    /// <param name="other">The list that will be used to fill.</param>
    /// <returns></returns>
    public static List<T> FillList<T>(this List<T> l, List<T> other)
    {
        if (l == null || other == null)
            return l;
        for (int i = 0; i < l.Count; i++)
        {
            if (other.Count - 1 < i)
                break;
            l[i] = other[i];
        }

        return l;
    }

    /// <summary>
    /// Returns a list with all the items of the bidimensional array.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="bidimensionalArray"></param>
    /// <returns></returns>
    public static List<T> GetItems<T>(this T[,] bidimensionalArray)
    {
        if (bidimensionalArray == null)
            return null;
        List<T> result = new List<T>();

        for (int i = 0; i < bidimensionalArray.GetLength(0); i++)
        {
            for (int j = 0; j < bidimensionalArray.GetLength(1); j++)
            {
                result.Add(bidimensionalArray[i, j]);
            }
        }
        return result;
    }

    /// <summary>
    /// Is the list empty?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(this List<T> list)
    {
        return list?.Count == 0;
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
    public static Vector3 RoundVector3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }

    public static Vector2 RoundVector2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }
    public static void ChangeParentScale(this Transform parent, Vector3 scale)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            child.parent = null;
            children.Add(child);
        }
        parent.localScale = scale;
        foreach (Transform child in children) child.parent = parent;
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
    /// Removes and adds a listener.
    /// </summary>
    /// <param name="y"></param>
    /// <param name="listener"></param>
    public static void RemoveAndAddListener(this UnityEvent y, UnityAction listener)
    {
        y.RemoveListener(listener);
        y.AddListener(listener);
    }
    /// <summary>
    /// Removes and adds a listener.
    /// </summary>
    /// <param name="y"></param>
    /// <param name="listener"></param>
    public static void RemoveAndAddListener<T>(this UnityEvent<T> y, UnityAction<T> listener)
    {
        y.RemoveListener(listener);
        y.AddListener(listener);
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