using System.Collections.Generic;
using UnityEngine;

public enum EventName
{
    SelectedPiece,
    ChosenSquare,
    EnemyMove,
    PlayerMove,
}

public class EventData
{
    public readonly GameObject GameObject;
    public readonly string Text;

    public EventData(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public EventData(string text)
    {
        Text = text;
    }
}

public delegate void Event(EventData eventData);

public class EventSystem
{
    private static Dictionary<EventName, Event> events = new();

    public static void Register(EventName eventName, Event e)
    {
        if (!events.ContainsKey(eventName))
        {
            events[eventName] = e;
        }
        else
        {
            events[eventName] += e;
        }
    }

    public static void Unregister(EventName eventName, Event e)
    {
        events[eventName] -= e;
    }

    public static void Fire(EventName eventName, EventData eventData)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName](eventData);
        }
    }
}