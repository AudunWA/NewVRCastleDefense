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

    private void UpdateCostText(Text textBox, SpawnType spawnType, Player player, string previousCost)
    {
        string text = textBox.text;
        string cost = player.MinionStatistics[spawnType].Cost.ToString();
        text = text.Remove(text.Length - previousCost.Length) + player.MinionStatistics[spawnType].Cost;
        textBox.text = text;
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
        fighterUpgradeButton.onClick.AddListener(delegate
        {
            if (GoodPlayer.MinionStatistics[SpawnType.Fighter].Level < 10)
            {
                string previousCost = GoodPlayer.MinionStatistics[SpawnType.Fighter].Cost.ToString();
                GoodPlayer.SpawnController.UpgradeMinionType(SpawnType.Fighter);
                Text textBox = fighterUpgradeButton.GetComponentInChildren<Text>();
                Text spawnTextbox = fighterButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textBox, SpawnType.Fighter, GoodPlayer);
                UpdateCostText(spawnTextbox, SpawnType.Fighter, GoodPlayer, previousCost);
            }
        });
        archerUpgradeButton.onClick.AddListener(delegate
        {
            if (GoodPlayer.MinionStatistics[SpawnType.Archer].Level < 10)
            {
                string previousCost = GoodPlayer.MinionStatistics[SpawnType.Archer].Cost.ToString();
                GoodPlayer.SpawnController.UpgradeMinionType(SpawnType.Archer);
                Text textBox = archerUpgradeButton.GetComponentInChildren<Text>();
                Text spawnTextbox = archerButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textBox, SpawnType.Archer, GoodPlayer);
                UpdateCostText(spawnTextbox, SpawnType.Archer, GoodPlayer, previousCost);
            }
        });
        mageUpgradeButton.onClick.AddListener(delegate
        {
            if (GoodPlayer.MinionStatistics[SpawnType.Mage].Level < 10)
            {
                string previousCost = GoodPlayer.MinionStatistics[SpawnType.Mage].Cost.ToString();
                GoodPlayer.SpawnController.UpgradeMinionType(SpawnType.Mage);
                Text textBox = mageUpgradeButton.GetComponentInChildren<Text>();
                Text spawnTextbox = mageButton.GetComponentInChildren<Text>();

                UpdateTextByLevel(textBox, SpawnType.Mage, GoodPlayer);
                UpdateCostText(spawnTextbox, SpawnType.Mage, GoodPlayer, previousCost);

            }
        });
        tankUpgradeButton.onClick.AddListener(delegate
        {
            if (GoodPlayer.MinionStatistics[SpawnType.Tank].Level < 10)
            {
                string previousCost = GoodPlayer.MinionStatistics[SpawnType.Tank].Cost.ToString();
                GoodPlayer.SpawnController.UpgradeMinionType(SpawnType.Tank);
                Text textBox = tankUpgradeButton.GetComponentInChildren<Text>();
                Text spawnTextbox = tankButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textBox, SpawnType.Tank, GoodPlayer);
                UpdateCostText(spawnTextbox, SpawnType.Tank, GoodPlayer, previousCost);
            }
        });
        castleUpgradeButton.onClick.AddListener(delegate
        {
            // TODO: castle upgrade
        });
    }

    private void InitEvilUpgradeButtons()
    {
        evilFighterUpgradeButton.onClick.AddListener(delegate
        {
            if (EvilPlayer.MinionStatistics[SpawnType.Fighter].Level < 10)
            {
                string previousCost = EvilPlayer.MinionStatistics[SpawnType.Fighter].Cost.ToString();
                EvilPlayer.SpawnController.UpgradeMinionType(SpawnType.Fighter);
                Text textBox = evilFighterUpgradeButton.GetComponentInChildren<Text>();
                Text spawnTextbox = evilFighterButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textBox, SpawnType.Fighter, EvilPlayer);
                UpdateCostText(spawnTextbox, SpawnType.Fighter, EvilPlayer, previousCost);
            }
        });
        evilArcherUpgradeButton.onClick.AddListener(delegate
        {
            if (EvilPlayer.MinionStatistics[SpawnType.Archer].Level < 10)
            {
                string previousCost = EvilPlayer.MinionStatistics[SpawnType.Archer].Cost.ToString();
                EvilPlayer.SpawnController.UpgradeMinionType(SpawnType.Archer);
                Text spawnTextbox = evilArcherButton.GetComponentInChildren<Text>();
                Text textBox = evilArcherUpgradeButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textBox, SpawnType.Archer, EvilPlayer);
                UpdateCostText(spawnTextbox, SpawnType.Archer, EvilPlayer, previousCost);
            }
        });
        evilMageUpgradeButton.onClick.AddListener(delegate
        {
            if (EvilPlayer.MinionStatistics[SpawnType.Mage].Level < 10)
            {
                string previousCost = EvilPlayer.MinionStatistics[SpawnType.Mage].Cost.ToString();
                EvilPlayer.SpawnController.UpgradeMinionType(SpawnType.Mage);
                Text textBox = evilMageUpgradeButton.GetComponentInChildren<Text>();
                Text spawnTextbox = evilMageButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textBox, SpawnType.Mage, EvilPlayer);
                UpdateCostText(spawnTextbox, SpawnType.Mage, EvilPlayer, previousCost);
            }
        });
        evilTankUpgradeButton.onClick.AddListener(delegate
        {
            if (EvilPlayer.MinionStatistics[SpawnType.Tank].Level < 10)
            {
                string previousCost = EvilPlayer.MinionStatistics[SpawnType.Tank].Cost.ToString();
                EvilPlayer.SpawnController.UpgradeMinionType(SpawnType.Tank);
                Text textBox = evilTankUpgradeButton.GetComponentInChildren<Text>();
                Text spawnTextbox = evilTankButton.GetComponentInChildren<Text>();
                UpdateTextByLevel(textBox, SpawnType.Tank, EvilPlayer);
                UpdateCostText(spawnTextbox, SpawnType.Tank, EvilPlayer, previousCost);
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
        Text upgradeTextBox;
        Text spawnTextbox;
        Player player = EvilPlayer;
        foreach (SpawnType s in upgradeButtons.Keys)
        {
            upgradeTextBox = evilUpgradeButtons[s].GetComponentInChildren<Text>();
            spawnTextbox = evilSpawnButtons[s].GetComponentInChildren<Text>();
            string previousCost = player.MinionStatistics[s].Cost.ToString();
            UpdateTextByLevel(upgradeTextBox, s, player);
            UpdateCostText(spawnTextbox, s, player, previousCost);
        }

    }

}
