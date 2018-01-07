using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameflowController
{
    public Player EvilPlayer { get; set; }
    public Player GoodPlayer { get; set; }
    public Player Winner { get; set; }
    public int MAX_E_ATTR_LVL { get; set; }
    public int MAX_ATTR_LVL { get; set; }
    public Dictionary<SpawnType, MinionStat> MinionStatAdditions { get; set; }    
    public WorldController WorldController { get; set; }
    public GameflowController(Player evilPlayer, Player goodPlayer)
    {
        EvilPlayer = evilPlayer;
        GoodPlayer = goodPlayer;
        MAX_ATTR_LVL = 10;
        MAX_E_ATTR_LVL = 12;
    }

    private Player GetOtherPlayer(Player player)
    {
        return WorldController.GetOtherPlayer(player);
    }
    public void OnMinionKilled(object sender, MinionController.MinionStateEventArgs e)
    {
        UpdatePlayerMoneyOnKill(e.Minion);
    }

    public void UpdatePlayerMoney(Player player)
    {
        if (player.SpawnController.GetTimer.IsIncrementMoneyAvailable())
        {
            player.IncrementMoney();
            player.SpawnController.GetTimer.StartMoneyTimer();
        }
    }
    private void UpdatePlayerMoneyOnKill(Minion minion)
    {
        Player player = GetOtherPlayer(minion.Player);
        player.Money += minion.Bounty;
    }

    public int GetUpgradeCost(SpawnType spawnType, Player player, MinionAttribute attr)
    {
        return player.MinionStatistics[spawnType].LevelUpgradeCost[attr];
    }


    public bool UpgradeMinionStat(SpawnType spawnType, Player player, MinionAttribute attr)
    {
        int cost = GetUpgradeCost(spawnType, player, attr);
        if (!player.WithdrawMoney(cost)) return false;
        int level = player.MinionStatistics[spawnType].Levels[attr];
        // Level cap of upgrading
        if ( level >= (player.PlayerType == PlayerType.Evil ? MAX_E_ATTR_LVL : MAX_ATTR_LVL)) return false;
        MinionStat stat = player.MinionStatistics[spawnType];
        MinionStat addition = MinionStatAdditions[spawnType];
        player.MinionStatistics[spawnType] = stat.Upgrade(stat, addition, attr);
        return true;
    }
}
