using System.Collections.Generic;
using UnityEngine;

public enum EventName
{
    SelectedPiece,
    ChosenSquare,
    Move,
    StateChange,
    StartGame,
    PlayerMovedPiece,
    StartPromotion,
    GameEnd,
}

public class EventData
{
    public readonly GameObject GameObject;
    public readonly Piece Piece;
    public readonly BoardSquare BoardSquare;
    public readonly string Text;
    public readonly State OldState;
    public readonly State NewState;

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

    public EventData(State oldState, State newState, string text)
    {
        OldState = oldState;
        NewState = newState;
        Text = text;
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
        Debug.Log("Event: " + eventName);
        if (events.ContainsKey(eventName))
        {
            toFire.Enqueue(new KeyValuePair<EventName, EventData>(eventName, eventData));
        }

        if (toFire.Count == 1)
        {
            while (toFire.Count > 0)
            {
                var e = toFire.Peek();
                events[e.Key](e.Value);
                toFire.Dequeue();
            }
        }
       
    }
}