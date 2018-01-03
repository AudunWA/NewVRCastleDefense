using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameflowController
{
    public Player EvilPlayer { get; set; }
    public Player GoodPlayer { get; set; }
    public Player Winner { get; set; }
    public Dictionary<SpawnType, MinionStat> MinionStatAdditions { get; set; }    
    public WorldController WorldController { get; set; }
    public GameflowController(Player evilPlayer, Player goodPlayer)
    {
        EvilPlayer = evilPlayer;
        GoodPlayer = goodPlayer;
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
        player.MoneyIncrementFactor += 1; // Increase amount of earn money per update, payment for kill
        player.SpawnController.GetTimer.moneyTimerLim += 0.3f; // Make update slightly less frequent
    }


    public bool UpgradeMinionStat(SpawnType spawnType, Player player, MinionAttribute attr)
    {
        int cost = player.MinionStatistics[spawnType].LevelUpgradeCost[attr];
        if (!player.WithdrawMoney(cost)) return false;
        Debug.Log(player.PlayerType+" upgrades: "+ attr + " on " + spawnType);
        MinionStat stat = player.MinionStatistics[spawnType];
        MinionStat addition = MinionStatAdditions[spawnType];
        player.MinionStatistics[spawnType] = stat.Upgrade(stat, addition, attr);
        return true;
    }
}
