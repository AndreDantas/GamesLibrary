using System.Collections.Generic;

[System.Serializable]
public class ObjectPair<T>
{
    public T first;
    public T second;

    public ObjectPair(T first, T second)
    {
        this.first = first;
        this.second = second;
    }

    public override bool Equals(object obj)
    {
        var pair = obj as ObjectPair<T>;
        return pair != null &&
               EqualityComparer<T>.Default.Equals(first, pair.first) &&
               EqualityComparer<T>.Default.Equals(second, pair.second);
    }

    public override int GetHashCode()
    {
        var hashCode = 405212230;
        hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(first);
        hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(second);
        return hashCode;
    }

    public static bool operator ==(ObjectPair<T> a, ObjectPair<T> b)
    {
        if ((object)a == null)
            return false;
        if ((object)b == null)
            return false;

        return ((object)a.first == (object)b.first && (object)a.second == (object)b.second);
    }
    public static bool operator !=(ObjectPair<T> a, ObjectPair<T> b)
    {
        if ((object)a == null)
            return false;
        if ((object)b == null)
            return false;

        return !((object)a.first == (object)b.first && (object)a.second == (object)b.second);
    }

    public override string ToString()
    {
        return "(" + first.ToString() + ", " + second.ToString() + ")";
    }
}