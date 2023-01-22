﻿using System;
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

    ArUiSmallButtonClick,
    ArUiGameModeAddTime,
    ArUiGameModeSubtractTime,
    ArUiGameModeAddIncrement,
    ArUiGameModeSubtractIncrement,
    ArUiMainButtonClick,
    
    ArUiPvpGameSelected,
    ArUiFriendGameSelected,
    ArUiStockfishGameSelected,

    Surrender,
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


    public EventData()
    {
    }

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

public class EventSystem : MonoBehaviour
{
    private static EventSystem instance;

    public static EventSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventSystem>();
            }

            return instance;
        }
    }

    private Dictionary<EventName, Event> events = new();

    private Queue<KeyValuePair<EventName, EventData>> toFire = new();

    public void Register(EventName eventName, Event e)
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

    public void Unregister(EventName eventName, Event e)
    {
        events[eventName] -= e;
    }

    public void Fire(EventName eventName, EventData eventData)
    {
        Debug.Log("Queued event: " + eventName);

        if (events.ContainsKey(eventName))
        {
            toFire.Enqueue(new KeyValuePair<EventName, EventData>(eventName, eventData));
        }
    }

    private void Update()
    {
        Queue<KeyValuePair<EventName, EventData>> toFireNow = new();

        while (toFire.Count > 0)
        {
            toFireNow.Enqueue(toFire.Peek());
            toFire.Dequeue();
        }

        while (toFireNow.Count > 0)
        {
            var e = toFireNow.Peek();
            Debug.Log("Fired event: " + e.Key);
            events[e.Key](e.Value);
            toFireNow.Dequeue();
        }
    }
}