using System.Collections.Generic;
using UnityEngine;

public class AIController
{
    private GameAI gameAI;
    private GameflowController gameflowController;
    private List<SpawnType> spawnTypes;
    private Dictionary<SpawnType, bool> availableSpawnTypes;
    public int AILevel { get; set; }
    public AIController(Player aiPlayer, Player player, int aiLevel)
    {
        gameAI = new GameAI();
        gameAI.Player = aiPlayer;
        gameAI.OtherPlayer = player;
        gameAI.Level = aiLevel; // Is set i worldcontroller from Lobby
        Debug.Log("Level "+aiLevel);
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
        if (gameAI.CurrentAction == GameAI.AIAction.Spawn)
        {
            gameAI.Player.SpawnController.Spawn(gameAI.CurrentSpawnType);
        }
        else if (gameAI.CurrentAction == GameAI.AIAction.Upgrade)
        {
            MinionAttribute attr = gameAI.CurrentAttribute;
            gameAI.Player.SpawnController.UpgradeMinionType(gameAI.CurrentUpgradeType, attr);
        }

        gameAI.FindNextAction();
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
