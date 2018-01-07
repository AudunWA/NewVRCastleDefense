using System.Collections.Generic;
using UnityEngine;

public class AIController
{
    private GameAI gameAi;
    private GameAI goodGameAI;
    private GameflowController gameflowController;
    private bool friendlyAi;

    public bool FriendlyAi
    {
        get { return friendlyAi; }
        set { friendlyAi = value; }
    }
    private List<SpawnType> spawnTypes;
    private Dictionary<SpawnType, bool> availableSpawnTypes;
    public AIController(Player aiPlayer, Player player, int gameAiLevel, bool friendlyAi)
    {
        this.friendlyAi = friendlyAi;
        gameAi = new GameAI();
        gameAi.Level = gameAiLevel;
        gameAi.Player = aiPlayer;
        gameAi.OtherPlayer = player;
       
        goodGameAI = new GameAI();
        goodGameAI.Level = gameAiLevel;
        goodGameAI.Player = player;
        goodGameAI.OtherPlayer = aiPlayer;
     
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
        SetAvailableAIActions(gameAi);
        UpdateAi(gameAi);
        if (friendlyAi)
        {
            SetAvailableAIActions(goodGameAI);
            UpdateAi(goodGameAI);
        }
    }

    private void UpdateAi(GameAI gameAI)
    {
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
    private void SetAvailableAIActions(GameAI gameAI)
    {
        gameAI.AvailableTimers = gameAI.Player.SpawnController.GetAvailableSpawnTypeTimers();
        foreach (SpawnType s in spawnTypes)
        {
            availableSpawnTypes[s] = gameAI.Player.MinionStatistics[s].Cost <= gameAI.Player.Money;

        }
        gameAI.AvailableAction = availableSpawnTypes;
    }
}
