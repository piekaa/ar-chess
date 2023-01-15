using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

[MyState(State.ArUi)]
public class UiClock : EventListener
{
    [SerializeField] private DisplayTime time;
    [SerializeField] private DisplayTime increment;

    [SerializeField] private MeshRenderer pvp;
    [SerializeField] private MeshRenderer friend;
    [SerializeField] private MeshRenderer stockfish;

    [SerializeField] private DigitMaterials digitMaterials;

    private MeshRenderer[] gameTypes;
    private int currentGameType = -1;


    private List<GameMode> pvpGameModes = new()
    {
        new GameMode(1, new() { 0 }),
        new GameMode(2, new() { 1 }),
        new GameMode(3, new() { 0, 2 }),
        new GameMode(5, new() { 0, 3 }),
        new GameMode(10, new() { 0, 5 }),
        new GameMode(15, new() { 10 }),
        new GameMode(30, new() { 0, 20 }),
    };

    private int currentGameMode = -1;
    private int currentIncrementIndex;


    private void Awake()
    {
        gameTypes = new[] { pvp, friend, stockfish };

        TurnOnNextGameType();
    }

    [Listen(EventName.ArUiChangeGameType)]
    private void ChangeGameType(EventData eventData)
    {
        TurnOnNextGameType();
    }

    [Listen(EventName.ArUiGameModeAddTime)]
    private void AddTime(EventData eventData)
    {
        NextGameMode();
    }

    [Listen(EventName.ArUiGameModeAddIncrement)]
    private void AddIncrement(EventData eventData)
    {
        NextIncrement();
    }
    
    private void TurnOnNextGameType()
    {
        foreach (var meshRenderer in gameTypes)
        {
            meshRenderer.material = digitMaterials.Off;
        }

        currentGameType++;
        currentGameType %= gameTypes.Length;

        gameTypes[currentGameType].material = digitMaterials.On;

        currentGameMode = -1;
        NextGameMode();
    }

    private void NextGameMode()
    {
        currentGameMode++;
        currentGameMode %= pvpGameModes.Count;

        time.seconds = pvpGameModes[currentGameMode].TimeSeconds;

        currentIncrementIndex = -1;
        NextIncrement();
    }

    [Listen(EventName.ArUiGameModeSubtractTime)]
    private void PreviousGameMode(EventData eventData)
    {
        currentGameMode--;
        currentGameMode = currentGameMode == -1 ? pvpGameModes.Count-1 : currentGameMode;

        time.seconds = pvpGameModes[currentGameMode].TimeSeconds;
        
        currentIncrementIndex = -1;
        NextIncrement();
    }
    
    
    private void NextIncrement()
    {
        var gameMode = pvpGameModes[currentGameMode]; 
        
        currentIncrementIndex++;
        currentIncrementIndex %= gameMode.IncrementsSeconds.Count;
        increment.seconds = gameMode.IncrementsSeconds[currentIncrementIndex];
    }
    
    [Listen(EventName.ArUiGameModeSubtractIncrement)]
    private void PreviousIncrement(EventData eventData)
    {
        var gameMode = pvpGameModes[currentGameMode]; 
        
        currentIncrementIndex--;
        currentIncrementIndex = currentIncrementIndex == -1 ? gameMode.IncrementsSeconds.Count-1 : currentIncrementIndex;

        increment.seconds = gameMode.IncrementsSeconds[currentIncrementIndex];
    }
    
}