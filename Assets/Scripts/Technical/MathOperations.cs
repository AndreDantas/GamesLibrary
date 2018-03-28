using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum LerpMode
{
    EaseIn,
    EaseOut,
    Exponential,
    Smoothstep,
    None

}
public static class MathOperations
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
            return Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
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

    public static List<T> ToList<T>(T[] array)
    {
        List<T> result = new List<T>();
        foreach (T t in array)
        {
            result.Add(t);
        }
        return result;
    }

    public static Vector3 RoundVector3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }
    public static Vector2 RoundVector2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

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
}