namespace EasyWork.Utilities
{
    public interface IEPool<T>
    {
        int Count { get; }
        T Get();
        void Recycle(T obj);
        void Clear();
    }

}

