using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CuplexApiCommon.Converters
{
    public static class ObjectMapper
    {
        private static readonly Dictionary<KeyValuePair<Type, Type>, object> Maps =
            new Dictionary<KeyValuePair<Type, Type>, object>();

        private static PropertyInfo[] _fromProperties;
        private static PropertyInfo[] _toProperties;
        private static FieldInfo[] _fromFields;
        private static FieldInfo[] _toFields;
        private static readonly object LockObject = new object();

        private static readonly IFormatProvider FormatProvider = new CultureInfo("Fr-fr");

        public static void Bind<TFrom, TTo>(Action<TFrom, TTo> map1 = null, Action<TTo, TFrom> map2 = null)
            where TFrom : class, new()
            where TTo : class, new()
        {
            AddMap<TFrom, TTo>(map1);
            AddMap<TTo, TFrom>(map2);
        }

        public static void Reset()
        {
            Maps.Clear();
        }

        public static void AddMap<TFrom, TTo>(Action<TFrom, TTo> map = null)
            where TFrom : class, new()
            where TTo : class, new()
        {
            var key = new KeyValuePair<Type, Type>(typeof(TFrom), typeof(TTo));
            if (Maps.ContainsKey(key))
            {
                Maps.Remove(key);
            }
            Maps.Add(key, map);
        }

        public static TTo Map<TFrom, TTo>(TFrom fromObj) where TTo : class, new()
        {
            return Map<TFrom, TTo>(fromObj, null);
        }

        public static TTo Map<TFrom, TTo>(TFrom fromObj, TTo toObj)
            where TTo : class, new()
        {
            lock (LockObject)
            {
                if (toObj == null)
                    toObj = new TTo();
                var key = new KeyValuePair<Type, Type>(typeof(TFrom), typeof(TTo));
                var map = (Action<TFrom, TTo>)Maps[key];

                if (!Maps.Any(x => x.Key.Equals(key)))
                    throw new Exception(string.Format("No map defined for {0} => {1}", typeof(TFrom).Name,
                        typeof(TTo).Name));

                var tFrom = typeof(TFrom);
                var tTo = typeof(TTo);

                _fromProperties = tFrom.GetProperties();
                _fromFields = tFrom.GetFields();
                _toProperties = tTo.GetProperties();
                _toFields = tTo.GetFields();

                SyncProperties(fromObj, toObj);
                SyncFields(fromObj, toObj);

                if (map != null)
                    map(fromObj, toObj);

                return toObj;
            }
        }

        private static void SyncProperties<TFrom, TTo>(TFrom @fromObj, TTo toObj)
        {
            var fromProperties = _fromProperties;
            var toProperties = _toProperties;
            var toFields = _toFields;

            if (fromProperties == null || !fromProperties.Any()) return;
            foreach (var fromProperty in fromProperties)
            {
                object fromValue;
                if (toProperties.Any(x => x.Name == fromProperty.Name))
                {
                    var toProperty = toProperties.FirstOrDefault(x => x.Name == fromProperty.Name);
                    if (toProperty != null && MatchingProps(fromProperty, toProperty))
                    {
                        fromValue = fromProperty.GetValue(fromObj, null);

                        //To handle nullable properties
                        var t = Nullable.GetUnderlyingType(fromProperty.PropertyType) ?? fromProperty.PropertyType;
                        var safeValue = (fromValue == null) ? null : Convert.ChangeType(fromValue, t, FormatProvider);

                        toProperty.SetValue(toObj, safeValue, null);
                    }
                }

                if (toFields.All(x => x.Name != fromProperty.Name)) continue;
                var toField = toFields.FirstOrDefault(x => x.Name == fromProperty.Name);
                if (!MatchingPropertyToField(fromProperty, toField)) continue;
                fromValue = fromProperty.GetValue(fromObj, null);
                if (toField != null) toField.SetValue(toObj, fromValue);
            }
        }

        private static void SyncFields<TFrom, TTo>(TFrom @fromObj, TTo toObj)
        {
            var fromFields = _fromFields;
            var toFields = _toFields;
            var toProperties = _toProperties;

            if (fromFields == null || !fromFields.Any()) return;
            foreach (var fromField in fromFields)
            {
                object fromValue;
                if (toFields.Any(x => x.Name == fromField.Name))
                {
                    var toField = toFields.FirstOrDefault(x => x.Name == fromField.Name);
                    if (toField != null && MatchingFields(fromField, toField))
                    {
                        fromValue = fromField.GetValue(fromObj);

                        //To handle nullable fields
                        var type = Nullable.GetUnderlyingType(fromValue.GetType()) ?? fromValue.GetType();
                        var safeValue = (fromValue == null) ? null : Convert.ChangeType(fromValue, type, FormatProvider);
                        toField.SetValue(toObj, safeValue);
                    }
                }

                if (toProperties.All(x => x.Name != fromField.Name)) continue;
                var toProperty = toProperties.FirstOrDefault(x => x.Name == fromField.Name);
                if (!MatchingFieldToProperty(fromField, toProperty)) continue;
                fromValue = fromField.GetValue(fromObj);
                if (toProperty != null) toProperty.SetValue(toObj, fromValue, null);
            }
        }

        private static readonly Func<PropertyInfo, PropertyInfo, bool> MatchingProps = (T1, T2) =>
        {
            var t1 = Nullable.GetUnderlyingType(T1.PropertyType) ?? T1.PropertyType;
            var t2 = Nullable.GetUnderlyingType(T2.PropertyType) ?? T2.PropertyType;
            return T1.Name == T2.Name && t1 == t2;
        };

        private static readonly Func<FieldInfo, FieldInfo, bool> MatchingFields = (T1, T2) =>
        {
            var t1 = Nullable.GetUnderlyingType(T1.FieldType) ?? T1.FieldType;
            var t2 = Nullable.GetUnderlyingType(T2.FieldType) ?? T2.FieldType;
            return T1.Name == T2.Name && t1 == t2;
        };

        private static readonly Func<PropertyInfo, FieldInfo, bool> MatchingPropertyToField =
            (T1, T2) => T1.Name == T2.Name && T1.PropertyType == T2.FieldType;

        private static readonly Func<FieldInfo, PropertyInfo, bool> MatchingFieldToProperty =
            (T1, T2) => T1.Name == T2.Name && T1.FieldType == T2.PropertyType;
    }

    public static class ObjectMapperExtensions
    {
        public static TTo Map<TFrom, TTo>(this object source)
            where TTo : class, new()
            where TFrom : class, new()
        {
            return ObjectMapper.Map<TFrom, TTo>((TFrom)source);
        }

        public static IEnumerable<TTo> Map<TFrom, TTo>(this IEnumerable<object> objects)
            where TTo : class, new()
            where TFrom : class, new()
        {
            return objects.Select(o => ObjectMapper.Map<TFrom, TTo>((TFrom)o)).ToList();
        }
    }
}
