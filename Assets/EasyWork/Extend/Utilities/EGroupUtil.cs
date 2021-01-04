using EasyWork.Utilities;
using System;
using System.Collections.Generic;

namespace EasyWork.Extend.Utilities
{
    public static class EGroupUtil 
    {
        public static IEGroup<T> CreateGroup<T>(string groupName)
        {
            var group = ESingletonUtil.Get<EGroup<T>>();
            group.CreateGroup(groupName);
            return group;
        }

        public static IEGroup<T> GetGroup<T>()
        {
            return ESingletonUtil.Get<EGroup<T>>();
        }

        public static IEGroup<T> OnCreateGroupHandler<T>(this IEGroup<T> @this, Action<string> onCreateGroup)
        {
            var group = @this is EGroup<T> ? @this as EGroup<T> : ESingletonUtil.Get<EGroup<T>>();
            group.OnCreateGroup += onCreateGroup;

            return group;
        }

        public static IEGroup<T> OnDestroyGroupHandler<T>(this IEGroup<T> @this, Action<string> onDestroyGroup)
        {
            var group = @this is EGroup<T> ? @this as EGroup<T> : ESingletonUtil.Get<EGroup<T>>();
            group.OnDestroyGroup += onDestroyGroup;

            return group;
        }

        public static IEGroup<T> OnAdd2GroupHandler<T>(this IEGroup<T> @this, Action<string, T> onAdd2Group)
        {
            var group = @this is EGroup<T> ? @this as EGroup<T> : ESingletonUtil.Get<EGroup<T>>();
            group.OnAdd2Group += onAdd2Group;

            return group;
        }

        public static IEGroup<T> OnRemoveFromGroupHandler<T>(this IEGroup<T> @this, Action<string, T> onRemoveFromGroup)
        {
            var group = @this is EGroup<T> ? @this as EGroup<T> : ESingletonUtil.Get<EGroup<T>>();
            group.OnRemoveFromGroup += onRemoveFromGroup;

            return group;
        }

        public static IEGroup<T> Foreach<T>(this IEGroup<T> @this, string groupName, Action<T> call)
        {
            var groupController = @this is EGroup<T> ? @this as EGroup<T> : ESingletonUtil.Get<EGroup<T>>();

            var group = groupController.GetGroup(groupName);
            for (int i = 0; i < group.Count; i++)
            {
                call.Invoke(group[i]);
            }
            return groupController;
        }

        public static IList<T> ToList<T>(this IEGroup<T> @this, string groupName)
        {
            var groupController = @this is EGroup<T> ? @this as EGroup<T> : ESingletonUtil.Get<EGroup<T>>();

            return groupController.GetGroup(groupName);
        }

        public static void Add2Group<T>(this T @this, string groupName)
        {
            GetGroup<T>().Add2Group(groupName, @this);
        }

        public static void RemoveFromGroup<T>(this T @this, string groupName)
        {
            GetGroup<T>().RemoveFromGroup(groupName, @this);
        }
    }

}
