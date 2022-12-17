using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class Listen : Attribute
{
    public readonly EventName EventName;

    public Listen(EventName eventName)
    {
        EventName = eventName;
    }
}

public abstract class EventListener : MonoBehaviour
{
    private Dictionary<EventName, Event> registeredEvents = new();

    protected virtual void OnEnable()
    {
        foreach (var listenerMethod in listenerMethods())
        {
            var eventName = ((Listen)listenerMethod.GetCustomAttributes(typeof(Listen), false)[0]).EventName;
            var eventDelegate = (Event)Delegate.CreateDelegate(typeof(Event), this,  listenerMethod);
            registeredEvents[eventName] = eventDelegate;
            EventSystem.Register(eventName, eventDelegate);
        }
    }

    protected virtual void OnDisable()
    {
        foreach (var (eventName, eventDelegate) in registeredEvents)
        {
            EventSystem.Unregister(eventName, eventDelegate);
            registeredEvents = new();
        }
    }

    private MethodInfo[] listenerMethods()
    {
        return GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(info => info.GetCustomAttributes(typeof(Listen), true).Length > 0).ToArray();
    }
}