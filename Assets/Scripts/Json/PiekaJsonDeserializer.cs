using System.Text;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.Collections;
using UnityEngine;

class PiekaJsonDeserializer
{
    private static Dictionary<string, Type> knownTypes = new Dictionary<string, Type>
    {
        { "UnityEngine.CircleCollider2D", typeof(CircleCollider2D) },
        { "UnityEngine.BoxCollider2D", typeof(BoxCollider2D) }
    };

    public static T FromJson<T>(string json)
    {
        var obj = Activator.CreateInstance<T>();
        Stack<string> keys = new Stack<string>();
        Stack<object> values = new Stack<object>();
        IJsonState state = new BeginJsonState();

        for (int i = 0; i < json.Length; i++)
        {
            char c = json[i];
            if (!state.InString() && char.IsWhiteSpace(c))
            {
                continue;
            }

            try
            {
                if (values.Count == 0)
                {
                    state.whereAmI(WhereAmI.NOWHERE);
                }
                else
                {
                    state.whereAmI(values.Peek() is JSONArray ? WhereAmI.ARRAY : WhereAmI.OBJECT);
                }

                var result = state.HandleChar(c);

                if (result != null)
                {
                    if (result.stateResultAction == StateResultAction.NEW_OBJECT)
                    {
                        if (values.Count == 0)
                        {
                            values.Push(obj);
                        }
                        else
                        {
                            var item = createValueObjectAndSet(values.Peek().GetType(), values.Peek(), keys.Peek());

                            // if (item != null)
                            {
                                values.Push(item);   
                            }
                        }
                    }

                    if (result.stateResultAction == StateResultAction.NEW_ARRAY)
                    {
                        if (values.Count == 0)
                        {
                            values.Push(new JSONArray(obj));
                        }
                        else
                        {
                            var item = createValueObjectAndSet(values.Peek().GetType(), values.Peek(), keys.Peek());

                            if (item != null)
                            {
                                values.Push(new JSONArray(item));
                            }
                        }
                    }

                    if (result.stateResultAction == StateResultAction.NEW_OBJECT_IN_ARRAY)
                    {
                        var jsonArray = (JSONArray)values.Peek();
                        var newObject = Activator.CreateInstance(jsonArray.collectionItemType);
                        jsonArray.Add(newObject);
                        values.Push(newObject);
                    }

                    if (result.Key != null)
                    {
                        keys.Push(result.Key);
                    }

                    if (result.Value != null)
                    {
                        setValue(keys.Pop(), result.Value, values.Peek());
                    }

                    if (result.ArrayValue != null)
                    {
                        var jsonArray = (JSONArray)values.Peek();
                        jsonArray.Add(getValueByType(result.ArrayValue, jsonArray.collectionItemType));
                    }

                    if (result.stateResultAction == StateResultAction.END_ARRAY)
                    {
                        values.Pop();
                    }

                    if (result.stateResultAction == StateResultAction.END_OBJECT)
                    {
                        values.Pop();
                    }

                    if (result.NextState != null)
                    {
                        state = result.NextState;
                    }
                }
            }
            catch (Exception e)
            {
                throw new PieksonException(
                    json.Substring(0, i) + "   (" + json[i] + ")  , state = " + state.GetType().Name +
                    "\nstack trace: " + e.StackTrace, e);
            }
        }

        if (!(state is EndObjectOrArrayState))
        {
            throw new PieksonException("unexpected json end");
        }

        return obj;
    }

    private static void setValue(string key, string value, object obj)
    {
        if (obj == null)
        {
            return;
        }
        
        Type t = obj.GetType();
        if (obj is IDictionary)
        {
            var dic = obj as IDictionary;
            if (!t.IsGenericType)
            {
                throw new InvalidCastException(t.GetType().Name + " is not generic");
            }

            var dicValueType = dic.GetType().GetGenericArguments()[1];
            dic.Add(key, getValueByType(value, dicValueType));
        }
        else
        {
            var fieldInfo = t.GetField(key);
            var propertyInfo = t.GetProperty(key);
            var methodInfo = t.GetMethod("Set" + StringUtils.ToFirstUpper(key));
            if (fieldInfo != null)
            {
                var fieldType = fieldInfo.FieldType;
                fieldInfo.SetValue(obj, getValueByType(value, fieldType));
            }
            else if (propertyInfo != null)
            {
                var propertyType = propertyInfo.PropertyType;
                if (propertyInfo.GetSetMethod() != null)
                {
                    propertyInfo.SetValue(obj, getValueByType(value, propertyType), null);
                }
            }
            else if (methodInfo != null)
            {
                var parameterType = methodInfo.GetParameters()[0].ParameterType;
                methodInfo.Invoke(obj, new object[] { getValueByType(value, parameterType) });
            }
        }
    }

    private static object getValueByType(string value, Type t)
    {
        if (t.IsAssignableFrom(typeof(int)))
        {
            return int.Parse(value);
        }

        if (t.IsAssignableFrom(typeof(long)))
        {
            return long.Parse(value);
        }

        if (t.IsAssignableFrom(typeof(float)))
        {
            return float.Parse(value);
        }

        if (t.IsAssignableFrom(typeof(double)))
        {
            return double.Parse(value);
        }

        if (t.IsAssignableFrom(typeof(string)))
        {
            return value;
        }

        if (t.IsAssignableFrom(typeof(bool)))
        {
            return bool.Parse(value);
        }

        if (t.IsEnum)
        {
            return Enum.Parse(t, value);
        }

        if (t.IsAssignableFrom(typeof(Type)))
        {
            var type = Type.GetType(value);
            if (type == null)
            {
                return knownTypes.Where((pair, i) => value.StartsWith(pair.Key)).FirstOrDefault();
            }

            return type;
        }

        return null;
    }

    private static object createValueObjectAndSet(Type objectType, object currentObject, string fieldName)
    {
        if (currentObject is IDictionary)
        {
            var dic = currentObject as IDictionary;
            var type = dic.GetType().GetGenericArguments()[1];
            var newObject = Activator.CreateInstance(type);

            dic.Add(fieldName, newObject);

            return newObject;
        }
        else
        {
            var currentObjType = objectType;
            var fieldInfo = currentObjType.GetField(fieldName);
            var propertyInfo = currentObjType.GetProperty(fieldName);
            var methodInfo = currentObjType.GetMethod("Set" + StringUtils.ToFirstUpper(fieldName));

            var type = fieldInfo != null ? fieldInfo.FieldType : null;
            type = propertyInfo != null ? propertyInfo.PropertyType : type;
            type = methodInfo != null ? methodInfo.GetParameters()[0].ParameterType : type;

            if (type == null)
            {
                return null;
            }

            var newObject = Activator.CreateInstance(type);

            if (fieldInfo != null)
            {
                var fieldType = fieldInfo.FieldType;
                fieldInfo.SetValue(currentObject, newObject);
            }
            else if (propertyInfo != null)
            {
                var propertyType = propertyInfo.PropertyType;
                if (propertyInfo.GetSetMethod() != null)
                {
                    propertyInfo.SetValue(currentObject, newObject, null);
                }
            }
            else if (methodInfo != null)
            {
                var parameterType = methodInfo.GetParameters()[0].ParameterType;
                methodInfo.Invoke(currentObject, new object[] { newObject });
            }

            return newObject;
        }
    }
}


class JSONArray
{
    private IList list;

    public Type collectionItemType;

    public JSONArray(object collection)
    {
        if (typeof(IList).IsAssignableFrom(collection.GetType()))
        {
            var collectionType = collection.GetType();
            if (!collectionType.IsGenericType)
            {
                throw new InvalidCastException(collection.GetType().Name + " is not generic");
            }

            collectionItemType = collectionType.GetGenericArguments()[0];
            list = collection as IList;
        }
        else
        {
            throw new InvalidCastException(collection.GetType().Name + " is not assingable from IList");
        }
    }

    public void Add(object value)
    {
        list.Add(value);
    }
}

public class PieksonException : Exception
{
    public PieksonException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public PieksonException(string message) : base(message)
    {
    }
}