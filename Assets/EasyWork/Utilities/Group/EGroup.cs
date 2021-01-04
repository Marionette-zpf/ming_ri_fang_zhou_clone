using System;
using System.Collections.Generic;

namespace EasyWork.Utilities
{
    public class EGroup<T> : IEGroup<T>
    {
        private Dictionary<string, List<T>> m_groups = new Dictionary<string, List<T>>();

        public event Action<string> OnCreateGroup;
        public event Action<string> OnDestroyGroup;
        public event Action<string, T> OnAdd2Group;
        public event Action<string, T> OnRemoveFromGroup;

        public void CreateGroup(string groupName)
        {
            if (m_groups.ContainsKey(groupName))
            {
                throw new Exception($"group {groupName} created");
            }
            else
            {
                m_groups.Add(groupName, new List<T>());
                OnCreateGroup?.Invoke(groupName);
            }
        }

        public void DestroyGroup(string groupName)
        {
            if (m_groups.ContainsKey(groupName))
            {
                throw new Exception($"group {groupName} created");
            }
            else
            {
                m_groups[groupName].Clear();
                m_groups.Remove(groupName);
                OnDestroyGroup?.Invoke(groupName);
            }
        }

        public void Add2Group(string groupName, T obj)
        {
            GetGroup(groupName).Add(obj);
            OnAdd2Group?.Invoke(groupName, obj);
        }

        public void RemoveFromGroup(string groupName, T obj)
        {
            GetGroup(groupName).Remove(obj);
            OnRemoveFromGroup?.Invoke(groupName, obj);
        }

        public List<T> GetGroup(string groupName)
        {
            if (m_groups.TryGetValue(groupName, out List<T> group))
            {
                return group;
            }
            else
            {
                throw new Exception("must create group");
            }
        }
    }

}

