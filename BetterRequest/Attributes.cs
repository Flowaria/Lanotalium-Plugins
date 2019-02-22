using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lanotalium.Plugin.BetterRequest
{
    /// Field Attributes
    [AttributeUsage(AttributeTargets.Field)]
    public class RequestTypeAttribute : Attribute
    {
        public RequestType Type;

        public RequestTypeAttribute(RequestType type)
        {
            Type = type;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class WidthRatioAttribute : Attribute
    {
        public float Width;

        public WidthRatioAttribute(float width)
        {
            Width = width;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ToolTipAttribute : Attribute
    {
        public string Content;

        public ToolTipAttribute(string content)
        {
            Content = content;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class FileFilterAttribute : Attribute
    {
        public string Filter;

        public FileFilterAttribute(string filter)
        {
            Filter = filter;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class BetterRequestAttribute : Attribute
    {
        public string Name, Description;

        public BetterRequestAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class GroupAttribute : Attribute
    {
        public int Group;

        public GroupAttribute(int group)
        {
            Group = group;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class InlineAttribute : Attribute{ public InlineAttribute(){} }

    /// Group Attributes
    [AttributeUsage(AttributeTargets.Class)]
    public class GroupDefinitionAttribute : Attribute
    {
        internal struct GroupDefinition
        {
            public int groupid;
            public GroupType type;
            public string name;

            public GroupDefinition(int id_, GroupType type_, string name_)
            {
                groupid = id_;
                type = type_;
                name = name_;
            }
        }

        internal List<GroupDefinition> def = new List<GroupDefinition>();

        public GroupDefinitionAttribute(params string[] group)
        {
            foreach(var i in group)
            {
                GroupType type = GroupType.Separator;
                string name = i;

                if (i.Contains(';'))
                {
                    var s = i.Split(';');
                    if (s.Length == 2)
                    {
                        if (s[1].Equals("fold"))
                            type = GroupType.Folder_Folded;
                        else if (s[1].Equals("unfold"))
                            type = GroupType.Folder;

                        name = s[0];
                    }
                    else
                        continue;
                }
                else
                    continue;

                def.Add(new GroupDefinition(def.Count, type, name));
            }
        }

        public GroupType GetType(string name)
        {
            return GetType(def.FindIndex(x=>x.name.Equals(name)));
        }

        public GroupType GetType(int id)
        {
            return id < def.Count ? def[id].type : GroupType.Separator;
        }
    }

    public enum GroupType
    {
        Separator,
        Folder, //Unfolded
        Folder_Folded
    }
}
