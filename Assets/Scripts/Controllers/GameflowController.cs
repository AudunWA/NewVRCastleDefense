using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameflowController
{
    public Player EvilPlayer { get; set; }
    public Player GoodPlayer { get; set; }
    public Player Winner { get; set; }
    public Dictionary<SpawnType, MinionStat> MinionStatAdditions { get; set; }    
    private const float COST_UPGRADE_LEVEL_FACTOR = 1.2f;
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


    public void UpgradeMinionStat(SpawnType spawnType, Player player)
    {
        if (player == null) return;
        int cost = player.MinionStatistics[spawnType].LevelUpgradeCost;
        if (!player.WithdrawMoney(cost)) return;
        player.MinionStatistics[spawnType] += MinionStatAdditions[spawnType];
    }
}
