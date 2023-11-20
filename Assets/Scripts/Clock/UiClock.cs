using System.Collections.Generic;
using UnityEngine;

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
    private EventName[] gameTypeEvents;
    private int currentGameType = -1;

    private List<List<GameMode>> gameModesByGameType;


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

    private static List<int> customIncrementsWithoutZero = new()
    {
        1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
        25, 30, 35, 40, 45, 60,
        90, 120, 150, 180
    };

    private static List<int> customIncrements = new()
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
        25, 30, 35, 40, 45, 60,
        90, 120, 150, 180
    };

    private List<GameMode> customGameModes = new()
    {
        new GameMode(0, customIncrementsWithoutZero),
        new GameMode(0, 15, customIncrements),
        new GameMode(0, 30, customIncrements),
        new GameMode(0, 45, customIncrements),
        new GameMode(1, customIncrements),
        new GameMode(1, 30, customIncrements),
        new GameMode(2, customIncrements),
        new GameMode(3, customIncrements),
        new GameMode(4, customIncrements),
        new GameMode(5, customIncrements),
        new GameMode(6, customIncrements),
        new GameMode(7, customIncrements),
        new GameMode(8, customIncrements),
        new GameMode(9, customIncrements),
        new GameMode(10, customIncrements),
        new GameMode(11, customIncrements),
        new GameMode(12, customIncrements),
        new GameMode(13, customIncrements),
        new GameMode(14, customIncrements),
        new GameMode(15, customIncrements),
        new GameMode(16, customIncrements),
        new GameMode(17, customIncrements),
        new GameMode(18, customIncrements),
        new GameMode(19, customIncrements),
        new GameMode(20, customIncrements),
        new GameMode(25, customIncrements),
        new GameMode(30, customIncrements),
        new GameMode(35, customIncrements),
        new GameMode(40, customIncrements),
        new GameMode(45, customIncrements),
        new GameMode(60, customIncrements),
        new GameMode(75, customIncrements),
        new GameMode(90, customIncrements),
        new GameMode(105, customIncrements),
        new GameMode(120, customIncrements),
        new GameMode(150, customIncrements),
        new GameMode(165, customIncrements),
        new GameMode(180, customIncrements),
    };

    private int currentGameMode = -1;
    private int currentIncrementIndex;


    private void Awake()
    {
        gameTypes = new[] { pvp, friend, stockfish };
        gameTypeEvents = new[]
            { EventName.ArUiPvpGameSelected, EventName.ArUiFriendGameSelected, EventName.ArUiStockfishGameSelected };
        gameModesByGameType = new() { pvpGameModes, customGameModes, customGameModes };
    }

    protected override void EnterActiveState()
    {
        currentGameType = -1;
        TurnOnNextGameType();
    }

    [Listen(EventName.ArUiSmallButtonClick)]
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
        currentGameMode %= gameModesByGameType[currentGameType].Count;

        time.Seconds = gameModesByGameType[currentGameType][currentGameMode].TimeSeconds;

        currentIncrementIndex = -1;
        NextIncrement();
    }

    [Listen(EventName.ArUiGameModeSubtractTime)]
    private void PreviousGameMode(EventData eventData)
    {
        currentGameMode--;
        currentGameMode = currentGameMode == -1 ? gameModesByGameType[currentGameType].Count - 1 : currentGameMode;

        time.Seconds = gameModesByGameType[currentGameType][currentGameMode].TimeSeconds;

        currentIncrementIndex = -1;
        NextIncrement();
    }
    
    private void NextIncrement()
    {
        var gameMode = gameModesByGameType[currentGameType][currentGameMode];

        currentIncrementIndex++;
        currentIncrementIndex %= gameMode.IncrementsSeconds.Count;
        increment.Seconds = gameMode.IncrementsSeconds[currentIncrementIndex];
    }

    [Listen(EventName.ArUiGameModeSubtractIncrement)]
    private void PreviousIncrement(EventData eventData)
    {
        var gameMode = gameModesByGameType[currentGameType][currentGameMode];

        currentIncrementIndex--;
        currentIncrementIndex =
            currentIncrementIndex == -1 ? gameMode.IncrementsSeconds.Count - 1 : currentIncrementIndex;

        increment.Seconds = gameMode.IncrementsSeconds[currentIncrementIndex];
    }

    [Listen(EventName.ArUiMainButtonClick)]
    private void OnMainButtonClick(EventData eventData)
    {
        var gameMode = gameModesByGameType[currentGameType][currentGameMode];
        EventSystem.Instance.Fire(gameTypeEvents[currentGameType],
            new EventData(new TimeControl(gameMode.TimeSeconds, gameMode.IncrementsSeconds[currentIncrementIndex])));
    }
}