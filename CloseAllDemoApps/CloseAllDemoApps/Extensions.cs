using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CloseAllDemoApps.Services;

namespace CloseAllDemoApps
{
    public static class Extensions
    {
        public static string PropertyList(this object obj)
        {
            var message = "";
            try
            {
                if (obj == null)
                {
                    message = "NULL";
                }
                else
                {
                    var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(info => info.CanRead);
                    var sb = new StringBuilder();
                    foreach (var p in props.Where(p => p != null))
                    {
                        if (IsPropertyACollection(p))
                        {
                            var enumerable = (IEnumerable)p.GetValue(obj, null);
                            var innerSb = new StringBuilder();
                            foreach (var item in enumerable)
                            {
                                innerSb.Append(item != null ? Convert.ToChar(item) : ' ');
                            }
                            sb.AppendLine(p.Name + ": " + innerSb);
                        }
                        else
                            sb.AppendLine(p.Name + ": " + p.GetValue(obj, null));
                    }
                    message = sb.ToString();
                }
            }
            catch (Exception e)
            {
                StatusLogService.Service.LogError("Failed to print object: " + obj + " : " + e.Message);
            }
            return message;
        }

        public static bool IsPropertyACollection(PropertyInfo property)
        {
            return property.PropertyType.GetInterface(typeof(IEnumerable<byte>).FullName) != null;
        }

        public static string CreateExceptionMessage(this Exception exception)
        {
            try
            {
                var sb = new StringBuilder();
                sb.CreateExceptionMessage(exception);

                if (exception.InnerException == null) return sb.ToString();
                sb.AppendLine("InnerException - level: 1");
                sb.CreateExceptionMessage(exception.InnerException);

                if (exception.InnerException.InnerException == null) return sb.ToString();
                sb.AppendLine("InnerException - level: 2");
                sb.CreateExceptionMessage(exception.InnerException.InnerException);

                if (exception.InnerException.InnerException.InnerException == null) return sb.ToString();
                sb.AppendLine("InnerException - level: 3");
                sb.CreateExceptionMessage(exception.InnerException.InnerException.InnerException);
                return sb.ToString();
            }
            catch (Exception e)
            {
                StatusLogService.Service.LogError("Failed to create exception message for exception: " + e.Message);
            }
            return exception.StackTrace;
        }

        private static void CreateExceptionMessage(this StringBuilder sb, Exception exception)
        {
            sb.AppendLine("Type: " + exception.GetType().Name);
            sb.AppendLine("Message: " + exception.Message);
            sb.AppendLine("StackTrace: " + exception.StackTrace);
        }

    }
}
