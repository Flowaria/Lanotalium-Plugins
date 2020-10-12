using EasyRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Lanotalium.Plugin.Simple.RTRequest
{
    public class RuntimeRequest
    {
        public static readonly TypeAttributes TYPEATTR =
            TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AnsiClass |
            TypeAttributes.BeforeFieldInit |
            TypeAttributes.AutoLayout;

        public static readonly MethodAttributes METHODATTR =
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName;

        public static readonly FieldAttributes FIELDATTR = FieldAttributes.Public | FieldAttributes.HasDefault;
        public static readonly FieldAttributes FIELDATTR_NODEF = FieldAttributes.Public;


        private List<RequestField> _Fields = new List<RequestField>();
        private EasyRequestManager _Manager;

        public RuntimeRequest(EasyRequestManager RequestManager)
        {
            _Manager = RequestManager;
        }

        public RequestField CreateField(string fieldName, string description, RequestFieldType type)
        {
            if (_Fields.Exists(x => x.FieldName.Equals(fieldName)))
            {
                return null;
            }

            var field = new RequestField(fieldName, description, type);
            _Fields.Add(field);

            return field;
        }

        public IEnumerator Request(string title = null)
        {
            if (_Fields.Count <= 0)
            {
                yield break;
            }

            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var guidAsembly = new AssemblyName(guid);

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(guidAsembly, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            var typeBuilder = moduleBuilder.DefineType(guidAsembly.FullName, TYPEATTR, null);

            typeBuilder.DefineDefaultConstructor(METHODATTR);

            var defValueWorks = new List<DefaultValueWork>();

            foreach (var field in _Fields)
            {
                Type fieldType = field.FieldTypeToType();

                FieldBuilder fieldBuilder;

                //Define Field + Add Default value worker
                if (field.HasDefaultValue)
                {
                    fieldBuilder = typeBuilder.DefineField(field.FieldName, fieldType, FIELDATTR);
                    defValueWorks.Add(new DefaultValueWork()
                    {
                        FieldName = field.FieldName,
                        Value = field.DefaultValue
                    });
                }
                else
                {
                    fieldBuilder = typeBuilder.DefineField(field.FieldName, fieldType, FIELDATTR_NODEF);
                }

                //Add name
                if (!string.IsNullOrEmpty(field.Description))
                {
                    AddNameAttribute(fieldBuilder, field.Description);
                }
                else
                {
                    AddNameAttribute(fieldBuilder, field.FieldName);
                }
                    
                //Add range attribute
                if (field.HasRange)
                {
                    AddRangeAttribute(fieldBuilder, field.MinValue, field.MaxValue);
                }
            }

            Type dataType = typeBuilder.CreateType();
            var type = typeof(Request<>).MakeGenericType(dataType);
            var requestContext = Activator.CreateInstance(type);
            var objectContext = requestContext.GetType().GetField("Object").GetValue(requestContext);

            //Do work: Default Value assign
            if (defValueWorks.Count > 0)
            {
                foreach (var work in defValueWorks)
                {
                    dataType.GetField(work.FieldName).SetValue(objectContext, work.Value);
                }
            }
            
            MethodInfo method = typeof(EasyRequestManager).GetMethod("Request");
            MethodInfo generic = method.MakeGenericMethod(dataType);

            //Open prompt
            yield return generic.Invoke(_Manager, new object[] { requestContext, title });

            var resultField = requestContext.GetType().GetField("Succeed");
            var success = (bool)resultField.GetValue(requestContext);

            var dataField = requestContext.GetType().GetField("Object");
            var data = dataField.GetValue(requestContext);

            Result = new RuntimeRequestResult(success, data);
        }

        private void AddRangeAttribute(FieldBuilder fieldBuilder, float min, float max)
        {
            var ctorParams = new Type[] { typeof(float), typeof(float) };
            var ctorInfo = typeof(RangeAttribute).GetConstructor(ctorParams);
            var rangeAttributeBuilder = new CustomAttributeBuilder(ctorInfo, new object[] { min, max });

            fieldBuilder.SetCustomAttribute(rangeAttributeBuilder);
        }

        private void AddNameAttribute(FieldBuilder fieldBuilder, string name)
        {
            var ctorParams = new Type[] { typeof(string) };
            var ctorInfo = typeof(NameAttribute).GetConstructor(ctorParams);
            var rangeAttributeBuilder = new CustomAttributeBuilder(ctorInfo, new object[] { name });

            fieldBuilder.SetCustomAttribute(rangeAttributeBuilder);
        }

        public RuntimeRequestResult Result { get; private set; } = null;
    }
}
