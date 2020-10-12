using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Lanotalium.Plugin.Simple.RTRequest
{
    public class RequestField
    {
        public RequestFieldType FieldType { get; private set; }
        public string FieldName { get; private set; }
        public string Description { get; private set; }

        public bool HasRange { get; private set; } = false;
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        public bool HasDefaultValue { get; private set; } = false;
        public object DefaultValue { get; private set; }

        public RequestField(string name, string desc, RequestFieldType type)
        {
            FieldType = type;
            FieldName = name;
            Description = desc;
        }

        public RequestField SetRange(float min, float max)
        {
            if(FieldType == RequestFieldType.Int || FieldType == RequestFieldType.Float)
            {
                HasRange = true;
                MinValue = min;
                MaxValue = max;
            }
            return this;
        }

        public RequestField SetDefaultValue(object defaultValue)
        {
            if (FieldTypeToType() == defaultValue.GetType())
            {
                Debug.Log("Added Default Value");
                HasDefaultValue = true;
                DefaultValue = defaultValue;
            }
            return this;
        }

        public Type FieldTypeToType()
        {
            switch (FieldType)
            {
                case RequestFieldType.Int:
                    return typeof(int);

                case RequestFieldType.Float:
                    return typeof(float);

                case RequestFieldType.Bool:
                    return typeof(bool);

                case RequestFieldType.String:
                default:
                    return typeof(string);
            }
        }
    }
}
