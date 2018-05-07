using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System;
using Sirenix.OdinInspector;
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
        return UnityEngine.Random.Range(0, 2) * 2 - 1;
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
    /// Returns a random element from a IEnumerable.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    /// <summary>
    /// Returns a IEnumerable with random elements from the source IEnumerable.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="source"></param>
    /// <param name="count">The amount of elements to pick.</param>
    /// <returns></returns>
    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }


    /// <summary>
    /// Returns a list with all the items of the two-dimensional array.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="twoDimensionalArray"></param>
    /// <returns></returns>
    public static List<T> GetItems<T>(this T[,] twoDimensionalArray)
    {
        if (twoDimensionalArray == null)
            return null;
        List<T> result = new List<T>();

        for (int i = 0; i < twoDimensionalArray.GetLength(0); i++)
        {
            for (int j = 0; j < twoDimensionalArray.GetLength(1); j++)
            {
                result.Add(twoDimensionalArray[i, j]);
            }
        }
        return result;
    }

    /// <summary>
    /// Checks if the (x,y) coordinate is valid in the two-dimensional array.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="twoDimensionalArray"></param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate</param>
    /// <returns></returns>
    public static bool ValidCoordinate<T>(this T[,] twoDimensionalArray, int x, int y)
    {
        if (twoDimensionalArray == null)
            return false;

        return (x >= 0 && x < twoDimensionalArray.GetLength(0) &&
                y >= 0 && y < twoDimensionalArray.GetLength(1));
    }

    /// <summary>
    /// Rotates a two-dimensional array by +90.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static T[,] RotateMatrix90R<T>(this T[,] matrix)
    {
        T[,] ret = new T[matrix.GetLength(0), matrix.GetLength(1)];

        for (int i = 0; i < matrix.GetLength(0); ++i)
        {
            for (int j = 0; j < matrix.GetLength(1); ++j)
            {
                ret[i, j] = matrix[matrix.GetLength(0) - j - 1, i];
            }
        }

        return ret;
    }

    /// <summary>
    /// Rotates a two-dimensional array by -90.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static T[,] RotateMatrix90L<T>(this T[,] matrix)
    {

        T[,] ret = new T[matrix.GetLength(0), matrix.GetLength(1)];

        for (int i = 0; i < matrix.GetLength(0); ++i)
        {
            for (int j = 0; j < matrix.GetLength(1); ++j)
            {
                ret[i, j] = matrix[j, matrix.GetLength(1) - i - 1];
            }
        }

        return ret;
    }
    /// <summary>
    /// Rotates a two-dimensional array by 180.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static T[,] Reverse<T>(this T[,] matrix)
    {

        return matrix.RotateMatrix90R().RotateMatrix90R();
    }
    public static bool Check2DArray<T>(this T[,] data, T[,] find) where T : class
    {
        if (data == null || find == null)
            return false;
        int dataLen = data.Length; // length of the whole data
        int findLen = find.Length; // length of the whole find

        for (int i = 0; i < dataLen; i++) // iterate through data
        {
            int dataX = i % data.GetLength(0); // get current column index
            int dataY = i / data.GetLength(0); // get current row index

            bool okay = true; // declare result placeholder for that check
            for (int j = 0; j < findLen && okay; j++) // iterate through find
            {
                int findX = j % find.GetLength(1); // current column in find
                int findY = j / find.GetLength(1); // current row in find

                int checkedX = findX + dataX; // column index in data to check
                int checkedY = findY + dataY; // row index in data to check

                // check if checked index is not going outside of the data boundries 
                if (checkedX >= data.GetLength(0) || checkedY >= data.GetLength(1))
                {
                    // we are outside of the data boundries
                    // set flag to false and break checks for this data row and column
                    okay = false;
                    break;
                }

                // we are still inside of the data boundries so check if values matches
                okay = data[dataY + findY, dataX + findX] == find[findY, findX]; // check if it matches
            }
            if (okay) // if all values from both fragments are equal
                return true; // return true

        }
        return false;
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

    public static void DestroySelf(this UnityEngine.Object c)
    {
        UnityEngine.Object.Destroy(c);
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
            UnityEngine.Object.Destroy(destroyList[i]);
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
    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
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