using System;
using System.Reflection;

namespace EnvironmentProcessStartRepro
{
    public static class ReflectionHelper
    {
        private static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            FieldInfo fieldInfo;
            do
            {
                fieldInfo = type.GetField(fieldName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            } while (fieldInfo == null && type != null);
            return fieldInfo;
        }

        public static T GetFieldValue<T>(this object source, string fieldName) where T:class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var type = source.GetType();
            var fieldInfo = GetFieldInfo(type, fieldName);
            if (fieldInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldName), $"Couldn't find field {fieldName} in type {type.FullName}");
            }
            return fieldInfo.GetValue(source) as T;
        }

        public static void SetFieldValue<T>(this object target, string fieldName, T val)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var type = target.GetType();
            var fieldInfo = GetFieldInfo(type, fieldName);
            if (fieldInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldName), $"Couldn't find field {fieldName} in type {type.FullName}");
            }
            if (!fieldInfo.FieldType.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException($"Invalid type got {typeof(T)} expected {fieldInfo.FieldType}", nameof(val));
            }
            fieldInfo.SetValue(target, val);
        }
    }
}