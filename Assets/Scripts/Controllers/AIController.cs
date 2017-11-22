using System.Collections.Generic;

public class AIController
{
    private GameAI gameAI;
    private SpawnController _spawnController;
    private List<SpawnType> spawnTypes;
    private Dictionary<SpawnType, bool> availableSpawnTypes;
    public AIController(Player aiPlayer, Player player)
    {
        gameAI = new GameAI();
        gameAI.Player = aiPlayer;
        gameAI.OtherPlayer = player; 
        spawnTypes = new List<SpawnType>
        {
            SpawnType.Archer,
            SpawnType.Fighter,
            SpawnType.Mage,
            SpawnType.Tank,
        };
        availableSpawnTypes = new Dictionary<SpawnType, bool>
        {
            { SpawnType.Archer, true },
            { SpawnType.Fighter, true},
            { SpawnType.Mage, true},
            { SpawnType.Tank, true},
        };
    }

    public void PlayAI()
    {
        SetAvailableAIActions();
        gameAI.FindNextAction();
        DoAIAction();
    }
    private void DoAIAction()
    {
       gameAI.Player.SpawnController.Spawn(gameAI.CurrentAction);
    }
    private void SetAvailableAIActions()
    {
        Dictionary<SpawnType, bool> availableTimers = gameAI.Player.SpawnController.GetAvailableSpawnTypeTimers();
        foreach (SpawnType s in spawnTypes)
        {
            availableSpawnTypes[s] = availableTimers[s] && gameAI.Player.MinionStatistics[s].Cost <= gameAI.Player.Money;
        }
        gameAI.AvailableAction = availableSpawnTypes;
    }
}
