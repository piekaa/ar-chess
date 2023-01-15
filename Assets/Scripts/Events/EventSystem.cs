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
    ClockUpdate,
    ARSpawn,
    GameFound,
}

public class EventData
{
    public readonly GameObject GameObject;
    public readonly Piece Piece;
    public readonly BoardSquare BoardSquare;
    public readonly string Text;
    public readonly Fen Fen;
    public readonly State OldState;
    public readonly State NewState;
    public readonly ClockData ClockData;

    public readonly Vector3 Position;
    public readonly Quaternion Rotation;

    public EventData(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public EventData(string text)
    {
        Text = text;
    }

    public EventData(string text, Fen fen)
    {
        Text = text;
        Fen = fen;
    }

    public EventData(string text, ClockData clockData, Fen fen)
    {
        Text = text;
        ClockData = clockData;
        Fen = fen;
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

    public EventData(ClockData clockData)
    {
        ClockData = clockData;
    }

    public EventData(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
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
        // Debug.Log("Event: " + eventName);
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