using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class GuiController : MonoBehaviour
{
    public Player GoodPlayer { get; set; }
    public Player EvilPlayer { get; set; }

    public GameObject GOEvilPlayer { get; set; }
    public GameObject GOGoodPlayer { get; set; }
    public PlayerMoneyController EvilMoneyController { get; set; }
    public PlayerMoneyController GoodMoneyController { get; set; }
    private WorldController worldController;
    // Spawn buttons
    public Button fighterButton;
    public Button archerButton;
    public Button mageButton;
    public Button tankButton;
    public Button waveButton;

    public Dictionary<SpawnType, Button> upgradeButtons = new Dictionary<SpawnType, Button>();
    public Dictionary<SpawnType, Button> spawnButtons = new Dictionary<SpawnType, Button>();

    public Dictionary<SpawnType, Button> evilUpgradeButtons = new Dictionary<SpawnType, Button>();

    public Dictionary<SpawnType, Button> evilSpawnButtons = new Dictionary<SpawnType, Button>();
    // Upgrade buttons
    public Button fighterUpgradeButton;
    public Button archerUpgradeButton;
    public Button mageUpgradeButton;
    public Button tankUpgradeButton;
    public Button castleUpgradeButton;

    // Evil spawn buttons
    public Button evilFighterButton;
    public Button evilArcherButton;
    public Button evilMageButton;
    public Button evilTankButton;
    public Button evilWaveButton;

    // Evil upgrade buttons
    // Upgrade buttons
    public Button evilFighterUpgradeButton;
    public Button evilArcherUpgradeButton;
    public Button evilMageUpgradeButton;
    public Button evilTankUpgradeButton;
    public Button evilCastleUpgradeButton;




    // Game mode toggle
    public Toggle singlePlayerToggle;

    private void AddButtonsToLists()
    {
        spawnButtons.Add(SpawnType.Fighter, fighterButton);
        spawnButtons.Add(SpawnType.Tank, tankButton);
        spawnButtons.Add(SpawnType.Mage, mageButton);
        spawnButtons.Add(SpawnType.Archer, archerButton);

        upgradeButtons.Add(SpawnType.Fighter, fighterUpgradeButton);
        upgradeButtons.Add(SpawnType.Tank, tankUpgradeButton);
        upgradeButtons.Add(SpawnType.Mage, mageUpgradeButton);
        upgradeButtons.Add(SpawnType.Archer, archerUpgradeButton);

        evilSpawnButtons.Add(SpawnType.Fighter, evilFighterButton);
        evilSpawnButtons.Add(SpawnType.Tank, evilTankButton);
        evilSpawnButtons.Add(SpawnType.Mage, evilMageButton);
        evilSpawnButtons.Add(SpawnType.Archer, evilArcherButton);

        evilUpgradeButtons.Add(SpawnType.Fighter, evilFighterUpgradeButton);
        evilUpgradeButtons.Add(SpawnType.Tank, evilTankUpgradeButton);
        evilUpgradeButtons.Add(SpawnType.Mage, evilMageUpgradeButton);
        evilUpgradeButtons.Add(SpawnType.Archer, evilArcherUpgradeButton);
    }

    private void UpdateTextByLevel(Text textField, SpawnType spawnType, Player player)
    {
        string text = textField.text;
        string level = player.MinionStatistics[spawnType].Level.ToString();
        textField.text = text.Remove(text.Length - 1) + level;
    }

    private void UpdateCostText(Text textbox, SpawnType spawnType, Player player, string previousCost)
    {
        string text = textbox.text;
        string cost = player.MinionStatistics[spawnType].Cost.ToString();
        text = text.Remove(text.Length - previousCost.Length) + player.MinionStatistics[spawnType].Cost;
        textbox.text = text;
    }

    private void InitPlayerMoneyGui()
    {
        GOEvilPlayer = GameObject.FindGameObjectWithTag("EvilPlayer");
        GOGoodPlayer = GameObject.FindGameObjectWithTag("GoodPlayer");
        GameObject goWorld = GameObject.FindGameObjectWithTag("World");
        worldController = goWorld.GetComponent<WorldController>();
        EvilMoneyController = GOEvilPlayer.GetComponent<PlayerMoneyController>();
        GoodMoneyController = GOGoodPlayer.GetComponent<PlayerMoneyController>();
        EvilPlayer = worldController.EvilPlayer;
        GoodPlayer = worldController.GoodPlayer;
        EvilMoneyController.Player = EvilPlayer;
        GoodMoneyController.Player = GoodPlayer;
        archerButton.GetComponentInChildren<Text>().text += " " + GoodPlayer.MinionStatistics[SpawnType.Archer].Cost;
        fighterButton.GetComponentInChildren<Text>().text += " " + GoodPlayer.MinionStatistics[SpawnType.Fighter].Cost;
        tankButton.GetComponentInChildren<Text>().text += " " + GoodPlayer.MinionStatistics[SpawnType.Tank].Cost;
        mageButton.GetComponentInChildren<Text>().text += " " + GoodPlayer.MinionStatistics[SpawnType.Mage].Cost;

        evilArcherButton.GetComponentInChildren<Text>().text += " " + EvilPlayer.MinionStatistics[SpawnType.Archer].Cost;
        evilFighterButton.GetComponentInChildren<Text>().text += " " + EvilPlayer.MinionStatistics[SpawnType.Fighter].Cost;
        evilTankButton.GetComponentInChildren<Text>().text += " " + EvilPlayer.MinionStatistics[SpawnType.Tank].Cost;
        evilMageButton.GetComponentInChildren<Text>().text += " " + EvilPlayer.MinionStatistics[SpawnType.Mage].Cost;

    }

    private void InitGoodButtons()
    {
        archerButton.onClick.AddListener(delegate
        {
            GoodPlayer.SpawnController.Spawn(SpawnType.Archer);
            GoodMoneyController.UpdatePlayerMoneyDisplay();
        });
        fighterButton.onClick.AddListener(delegate
        {
            GoodPlayer.SpawnController.Spawn(SpawnType.Fighter);
            GoodMoneyController.UpdatePlayerMoneyDisplay();
        });
        mageButton.onClick.AddListener(delegate
        {
            GoodPlayer.SpawnController.Spawn(SpawnType.Mage);
            GoodMoneyController.UpdatePlayerMoneyDisplay();
        });
        tankButton.onClick.AddListener(delegate
        {
            GoodPlayer.SpawnController.Spawn(SpawnType.Tank);
            GoodMoneyController.UpdatePlayerMoneyDisplay();
        });
    }
    private void InitEvilButtons()
    {
        evilArcherButton.onClick.AddListener(delegate
        {
            EvilPlayer.SpawnController.Spawn(SpawnType.Archer);
            EvilMoneyController.UpdatePlayerMoneyDisplay();
        });
        evilFighterButton.onClick.AddListener(delegate
        {
            EvilPlayer.SpawnController.Spawn(SpawnType.Fighter);
            EvilMoneyController.UpdatePlayerMoneyDisplay();
        });
        evilMageButton.onClick.AddListener(delegate
        {
            EvilPlayer.SpawnController.Spawn(SpawnType.Mage);
            EvilMoneyController.UpdatePlayerMoneyDisplay();
        });
        evilTankButton.onClick.AddListener(delegate
        {
            EvilPlayer.SpawnController.Spawn(SpawnType.Tank);
            EvilMoneyController.UpdatePlayerMoneyDisplay();
        });
    }

    private void InitGoodUpgradeButtons()
    {
        MinionAttribute attr = MinionAttribute.Damage;
        fighterUpgradeButton.onClick.AddListener(delegate
        {
            if (GoodPlayer.MinionStatistics[SpawnType.Fighter].Level < 10)
            {
                string previousCost = GoodPlayer.MinionStatistics[SpawnType.Fighter].Cost.ToString();
                GoodPlayer.SpawnController.UpgradeMinionType(SpawnType.Fighter, attr);
                Text textbox = fighterUpgradeButton.GetComponentInChildren<Text>();
                Text spawntextbox = fighterButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textbox, SpawnType.Fighter, GoodPlayer);
                UpdateCostText(spawntextbox, SpawnType.Fighter, GoodPlayer, previousCost);
            }
        });
        archerUpgradeButton.onClick.AddListener(delegate
        {
            if (GoodPlayer.MinionStatistics[SpawnType.Archer].Level < 10)
            {
                string previousCost = GoodPlayer.MinionStatistics[SpawnType.Archer].Cost.ToString();
                GoodPlayer.SpawnController.UpgradeMinionType(SpawnType.Archer, attr);
                Text textbox = archerUpgradeButton.GetComponentInChildren<Text>();
                Text spawntextbox = archerButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textbox, SpawnType.Archer, GoodPlayer);
                UpdateCostText(spawntextbox, SpawnType.Archer, GoodPlayer, previousCost);
            }
        });
        mageUpgradeButton.onClick.AddListener(delegate
        {
            if (GoodPlayer.MinionStatistics[SpawnType.Mage].Level < 10)
            {
                string previousCost = GoodPlayer.MinionStatistics[SpawnType.Mage].Cost.ToString();
                GoodPlayer.SpawnController.UpgradeMinionType(SpawnType.Mage, attr);
                Text textbox = mageUpgradeButton.GetComponentInChildren<Text>();
                Text spawntextbox = mageButton.GetComponentInChildren<Text>();

                UpdateTextByLevel(textbox, SpawnType.Mage, GoodPlayer);
                UpdateCostText(spawntextbox, SpawnType.Mage, GoodPlayer, previousCost);

            }
        });
        tankUpgradeButton.onClick.AddListener(delegate
        {
            if (GoodPlayer.MinionStatistics[SpawnType.Tank].Level < 10)
            {
                string previousCost = GoodPlayer.MinionStatistics[SpawnType.Tank].Cost.ToString();
                GoodPlayer.SpawnController.UpgradeMinionType(SpawnType.Tank, attr);
                Text textbox = tankUpgradeButton.GetComponentInChildren<Text>();
                Text spawntextbox = tankButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textbox, SpawnType.Tank, GoodPlayer);
                UpdateCostText(spawntextbox, SpawnType.Tank, GoodPlayer, previousCost);
            }
        });
        castleUpgradeButton.onClick.AddListener(delegate
        {
            // TODO: castle upgrade
        });
    }

    private void InitEvilUpgradeButtons()
    {
        MinionAttribute attr = MinionAttribute.Damage;

        evilFighterUpgradeButton.onClick.AddListener(delegate
        {
            if (EvilPlayer.MinionStatistics[SpawnType.Fighter].Level < 10)
            {
                string previousCost = EvilPlayer.MinionStatistics[SpawnType.Fighter].Cost.ToString();
                EvilPlayer.SpawnController.UpgradeMinionType(SpawnType.Fighter, attr);
                Text textbox = evilFighterUpgradeButton.GetComponentInChildren<Text>();
                Text spawntextbox = evilFighterButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textbox, SpawnType.Fighter, EvilPlayer);
                UpdateCostText(spawntextbox, SpawnType.Fighter, EvilPlayer, previousCost);
            }
        });
        evilArcherUpgradeButton.onClick.AddListener(delegate
        {
            if (EvilPlayer.MinionStatistics[SpawnType.Archer].Level < 10)
            {
                string previousCost = EvilPlayer.MinionStatistics[SpawnType.Archer].Cost.ToString();
                EvilPlayer.SpawnController.UpgradeMinionType(SpawnType.Archer, attr);
                Text spawntextbox = evilArcherButton.GetComponentInChildren<Text>();
                Text textbox = evilArcherUpgradeButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textbox, SpawnType.Archer, EvilPlayer);
                UpdateCostText(spawntextbox, SpawnType.Archer, EvilPlayer, previousCost);
            }
        });
        evilMageUpgradeButton.onClick.AddListener(delegate
        {
            if (EvilPlayer.MinionStatistics[SpawnType.Mage].Level < 10)
            {
                string previousCost = EvilPlayer.MinionStatistics[SpawnType.Mage].Cost.ToString();
                EvilPlayer.SpawnController.UpgradeMinionType(SpawnType.Mage, attr);
                Text textbox = evilMageUpgradeButton.GetComponentInChildren<Text>();
                Text spawntextbox = evilMageButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textbox, SpawnType.Mage, EvilPlayer);
                UpdateCostText(spawntextbox, SpawnType.Mage, EvilPlayer, previousCost);
            }
        });
        evilTankUpgradeButton.onClick.AddListener(delegate
        {
            if (EvilPlayer.MinionStatistics[SpawnType.Tank].Level < 10)
            {
                string previousCost = EvilPlayer.MinionStatistics[SpawnType.Tank].Cost.ToString();
                EvilPlayer.SpawnController.UpgradeMinionType(SpawnType.Tank, attr);
                Text textbox = evilTankUpgradeButton.GetComponentInChildren<Text>();
                Text spawntextbox = evilTankButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textbox, SpawnType.Tank, EvilPlayer);
                UpdateCostText(spawntextbox, SpawnType.Tank, EvilPlayer, previousCost);
            }
        });
        evilCastleUpgradeButton.onClick.AddListener(delegate
        {
            // TODO: castle upgrade
        });
    }

    private void Start()
    {
        AddButtonsToLists();
        InitPlayerMoneyGui();
        InitGoodButtons();
        InitEvilButtons();
        InitEvilUpgradeButtons();
        InitGoodUpgradeButtons();

        singlePlayerToggle.onValueChanged.AddListener(delegate
        {
            worldController.SinglePlayer = singlePlayerToggle.isOn;
        });
    }

    private void Update()
    {
        Text upgradetextbox;
        Text spawntextbox;
        Player player = EvilPlayer;
        foreach (SpawnType s in upgradeButtons.Keys)
        {
            upgradetextbox = evilUpgradeButtons[s].GetComponentInChildren<Text>();
            spawntextbox = evilSpawnButtons[s].GetComponentInChildren<Text>();
            string previousCost = player.MinionStatistics[s].Cost.ToString();
            UpdateTextByLevel(upgradetextbox, s, player);
            UpdateCostText(spawntextbox, s, player, previousCost);
        }

    }

}
