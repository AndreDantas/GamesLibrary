[System.Serializable]
public struct ObjectPair<T>
{
    public T first;
    public T second;

    public ObjectPair(T first, T second)
    {
        this.first = first;
        this.second = second;
    }
}