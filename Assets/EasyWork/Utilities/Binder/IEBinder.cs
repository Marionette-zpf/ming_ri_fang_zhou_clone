
namespace EasyWork.Utilities
{
    public interface IEBinder<TKey, TValue>
    {
        void Binding(TKey key, TValue value);
        void Rebind(TKey key, TValue value);

        TValue GetValue(TKey key);
    }

}

