using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;


class PiekaJsonSerializer
{
    
    private static HashSet<char> escape = new HashSet<char>()
    {
        {'"'},
        {'\\'}
    };

    private static HashSet<Type> raw = new HashSet<Type>()
    {
        {typeof(int)},
        {typeof(long)},
        {typeof(float)},
        {typeof(double)},
        {typeof(bool)},
    };

    private static HashSet<Type> nonRawSimpleTypes = new HashSet<Type>()
    {
        {typeof(string)},
    };

    private static HashSet<string> methodBlacklist = new HashSet<string>()
    {
        {"GetHashCode"},
        {"GetType"},
    };

    public static string ToJson(object obj)
    {
        return ToJson(obj, new HashSet<object>());
    }

    public static string ToJson(object obj, HashSet<object> visited)
    {
        visited.Add(obj);
        StringBuilder sb = new StringBuilder();
        var t = obj.GetType();
        bool first = true;

        if (obj is IDictionary)
        {
            sb.Append("{");
            var dic = (IDictionary) obj;
            var enumerator = dic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key is string)
                {
                    if (visited.Contains(enumerator.Value) || enumerator.Value == null)
                    {
                        continue;
                    }

                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(",");
                    }

                    sb.Append("\"");
                    sb.Append(enumerator.Key);
                    sb.Append("\":");
                    appendValue(enumerator.Value, sb, visited);
                }
            }

            sb.Append("}");
            return sb.ToString();
        }

        if (typeof(IEnumerable).IsAssignableFrom(t))
        {
            sb.Append("[");
            IEnumerable enumerable = (IEnumerable) obj;

            foreach (var ob in enumerable)
            {
                if (visited.Contains(ob) || ob == null)
                {
                    continue;
                }

                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(",");
                }

                appendValue(ob, sb, visited);
            }

            sb.Append("]");
            return sb.ToString();
        }


        sb.Append("{");
        var fieldInfos = t.GetFields();
        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            var value = fieldInfo.GetValue(obj);
            if (visited.Contains(value) || value == null)
            {
                continue;
            }

            if (first)
            {
                first = false;
            }
            else
            {
                sb.Append(",");
            }

            sb.Append("\"");
            sb.Append(fieldInfo.Name);
            sb.Append("\"");
            sb.Append(":");

            appendValue(value, sb, visited);
        }

        var propertyInfos = t.GetProperties();
        foreach (PropertyInfo propertyInfo in propertyInfos)
        {
            if (propertyInfo.GetGetMethod() == null || propertyInfo.GetGetMethod().GetParameters().Length > 0)
            {
                continue;
            }

            var value = propertyInfo.GetValue(obj, null);
            if (visited.Contains(value) || value == null)
            {
                continue;
            }

            if (first)
            {
                first = false;
            }
            else
            {
                sb.Append(",");
            }

            sb.Append("\"");
            sb.Append(propertyInfo.Name);
            sb.Append("\"");
            sb.Append(":");
            appendValue(value, sb, visited);
        }

        var methodInfos = t.GetMethods();
        foreach (MethodInfo methodInfo in methodInfos)
        {
            if (!methodInfo.Name.StartsWith("Get"))
            {
                continue;
            }

            if (methodInfo.GetParameters().Length > 0)
            {
                continue;
            }

            if (methodBlacklist.Contains(methodInfo.Name))
            {
                continue;
            }

            var value = methodInfo.Invoke(obj, null);
            if (visited.Contains(value) || value == null)
            {
                continue;
            }

            if (first)
            {
                first = false;
            }
            else
            {
                sb.Append(",");
            }

            sb.Append("\"");
            var name = methodInfo.Name.Substring(3);
            sb.Append(StringUtils.ToFirstLower(name));
            sb.Append("\"");
            sb.Append(":");
            appendValue(value, sb, visited);
        }

        sb.Append("}");
        return sb.ToString();
    }

    private static void appendValue(object value, StringBuilder sb, HashSet<object> visited)
    {
        if (true.Equals(value))
        {
            sb.Append("true");
            return;
        }

        if (false.Equals(value))
        {
            sb.Append("false");
            return;
        }

        if (value is string)
        {
            sb.Append("\"");
            var str = (string) value;
            for (int i = 0; i < str.Length; i++)
            {
                if (escape.Contains(str[i]))
                {
                    sb.Append("\\");
                }

                sb.Append(str[i]);
            }

            sb.Append("\"");
            return;
        }

        if (value is Type)
        {
            sb.Append("\"");
            sb.Append(((Type) value).AssemblyQualifiedName);
            sb.Append("\"");
            return;
        }

        if (value.GetType().IsEnum)
        {
            sb.Append("\"");
            sb.Append(Enum.GetName(value.GetType(), value));
            sb.Append("\"");
        }

        else if (!raw.Contains(value.GetType()))
        {
            if (nonRawSimpleTypes.Contains(value.GetType()))
            {
                sb.Append("\"");
                sb.Append(value);
                sb.Append("\"");
            }
            else // it's object
            {
                sb.Append(ToJson(value, visited));
            }
        }
        else
        {
            sb.Append(value);
        }
    }
}