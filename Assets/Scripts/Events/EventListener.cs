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

[AttributeUsage(AttributeTargets.Class)]
public class MyState : Attribute
{
    public readonly State State;

    public MyState(State state)
    {
        State = state;
    }
}

public abstract class EventListener : MonoBehaviour
{
    private Dictionary<EventName, Event> registeredEvents = new();
    private State? myState;
    private bool active = false;
    
    protected virtual void OnEnable()
    {
        var attributes = GetType().GetCustomAttributes(typeof(MyState), false);
        if (attributes.Length != 0)
        {
            myState = ((MyState)attributes[0]).State;
            EventSystem.Register(EventName.StateChange, OnStateChange);
        }
        else
        {
            RegisterEvents();
        }
    }

    protected virtual void OnDisable()
    {
        UnregisterEvents();
        if (myState != null)
        {
            EventSystem.Unregister(EventName.StateChange, OnStateChange);    
        }
    }

    private void OnStateChange(EventData eventData)
    {
        if (eventData.State == myState)
        {
            RegisterEvents();
            active = true;
        }
        else
        {
            UnregisterEvents();
            active = false;
        }
    }

    private void RegisterEvents()
    {
        foreach (var listenerMethod in listenerMethods())
        {
            var eventName = ((Listen)listenerMethod.GetCustomAttributes(typeof(Listen), false)[0]).EventName;
            var eventDelegate = (Event)Delegate.CreateDelegate(typeof(Event), this, listenerMethod);
            registeredEvents[eventName] = eventDelegate;
            EventSystem.Register(eventName, eventDelegate);
        }
    }

    private void UnregisterEvents()
    {
        foreach (var (eventName, eventDelegate) in registeredEvents)
        {
            EventSystem.Unregister(eventName, eventDelegate);
            
        }
        registeredEvents = new();
    }

    private MethodInfo[] listenerMethods()
    {
        return GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(info => info.GetCustomAttributes(typeof(Listen), true).Length > 0).ToArray();
    }

    protected virtual void Update()
    {
        if (myState == null || active)
        {
            MyUpdate();
        }
    }

    protected abstract void MyUpdate();
}