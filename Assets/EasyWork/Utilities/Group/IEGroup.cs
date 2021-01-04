using System.Collections.Generic;

namespace EasyWork.Utilities
{
    public interface IEGroup<T>
    {
        void CreateGroup(string groupName);
        void DestroyGroup(string groupName);
        void Add2Group(string groupName, T obj);
        void RemoveFromGroup(string groupName, T obj);
    }
}


