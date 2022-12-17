using System.Collections.Generic;
using UnityEngine;

public enum EventName
{
    SelectedPiece,
    ChosenSquare,
    Move,
    StateChange,
}

public class EventData
{
    public readonly GameObject GameObject;
    public readonly Piece Piece;
    public readonly BoardSquare BoardSquare;
    public readonly string Text;
    public readonly State State;

    public EventData(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public EventData(string text)
    {
        Text = text;
    }

    public EventData(Piece piece)
    {
        Piece = piece;
    }

    public EventData(BoardSquare boardSquare)
    {
        BoardSquare = boardSquare;
    }

    public EventData(State state)
    {
        State = state;
    }
}

public delegate void Event(EventData eventData);

public class EventSystem
{
    private static Dictionary<EventName, Event> events = new();

    private static Queue<KeyValuePair<EventName, EventData>> toFire = new(); 

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
        Debug.Log("Event" + eventName);
        if (events.ContainsKey(eventName))
        {
            toFire.Enqueue(new KeyValuePair<EventName, EventData>(eventName, eventData));
        }

        KeyValuePair<EventName, EventData> e;
        while(toFire.TryDequeue(out e))
        {
            events[e.Key](e.Value);
        }
    }
}