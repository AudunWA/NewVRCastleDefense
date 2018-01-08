using UnityEngine;

public class ControllerMenu : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;

    GameObject rightController;

    GameObject controllerMenu;
    GameObject upgradeControllerMenu;

    GameObject selected;
    Quaternion menuRotation;

    Player player;

    private SpawnController _spawnController;
    public WorldController worldController;

    int position = 1;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        rightController = GameObject.FindGameObjectWithTag("rightcontroller");

        worldController = GameObject.FindGameObjectWithTag("World").GetComponent<WorldController>();
        player = worldController.GoodPlayer;

        // Activates the menu
        controllerMenu = Instantiate(Resources.Load("ControllerMenu", typeof(GameObject))) as GameObject;
        controllerMenu.transform.position = new Vector3(rightController.transform.position.x, rightController.transform.position.y, rightController.transform.position.z - 0.2f);
        controllerMenu.transform.SetParent(rightController.transform);
        controllerMenu.SetActive(false);

        upgradeControllerMenu = Instantiate(Resources.Load("UpgradeControllerMenu", typeof(GameObject))) as GameObject;
        upgradeControllerMenu.transform.position = new Vector3(rightController.transform.position.x, rightController.transform.position.y, rightController.transform.position.z - 0.2f);
        upgradeControllerMenu.transform.SetParent(rightController.transform);
        upgradeControllerMenu.SetActive(false);

        trackedObj = GetComponent<SteamVR_TrackedObject>();

    }

    // Use this for initialization
    void Start () {
        _spawnController = worldController.spawnController;
    }

    int check;
    bool activeChange;

    // Update is called once per frame
    void Update()
    {

        if (check != position || activeChange)
        {
            if (controllerMenu.gameObject.activeInHierarchy)
            {
                for (int i = 0; i < controllerMenu.transform.childCount; i++)
                {
                    controllerMenu.transform.GetChild(i).transform.localPosition = new Vector3(0.1f * i - (position * 0.1f), 0, 0);
                }
            } else if (upgradeControllerMenu.gameObject.activeInHierarchy)
            {
                for (int i = 0; i < upgradeControllerMenu.transform.childCount; i++)
                {
                    upgradeControllerMenu.transform.GetChild(i).transform.localPosition = new Vector3(0.1f * i - (position * 0.1f), 0, 0);
                }
            }
        }

        check = position;
        activeChange = false;

        if (controllerMenu.activeInHierarchy || upgradeControllerMenu.activeInHierarchy)
        {
            if (Controller.velocity.magnitude > 2.0f)
            {
                BringUpMenu();
            }
            if (Controller.angularVelocity.magnitude > 15.0f)
            {
                BringUpMenu();
            }
        }

        if (Controller.GetAxis().y < -0.6f && upgradeControllerMenu.activeInHierarchy && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            upgradeControllerMenu.SetActive(false);
            controllerMenu.SetActive(true);
            activeChange = true;
        }
        else if (Controller.GetAxis().y > 0.7f && controllerMenu.activeInHierarchy && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            controllerMenu.SetActive(false);
            upgradeControllerMenu.SetActive(true);
            activeChange = true;
        }
        else if (Controller.GetAxis().x < -0.7f && (controllerMenu.activeInHierarchy || upgradeControllerMenu.activeInHierarchy) && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (position > 2)
            {
            }
            else
            {
                position++;
            }
        }
        else if (Controller.GetAxis().x > 0.7f && (controllerMenu.activeInHierarchy || upgradeControllerMenu.activeInHierarchy) && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (position < 1)
            {
            }
            else
            {
                position--;
            }
        } else if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && Controller.GetAxis().x < 0.5f && Controller.GetAxis().x > -0.5f && Controller.GetAxis().y < 0.5f && Controller.GetAxis().y > -0.5f)
        {
            menuRotation = Controller.transform.rot;
            BringUpMenu();
        }

        if (Controller.GetHairTriggerDown() && selected != null && controllerMenu.activeInHierarchy)
        {
            if (selected.gameObject.name.Contains("Archer"))
            {
                player.SpawnController.Spawn(SpawnType.Archer);
            } else if (selected.gameObject.name.Contains("Fighter"))
            {
                player.SpawnController.Spawn(SpawnType.Fighter);
            } else if (selected.gameObject.name.Contains("Mage"))
            {
                player.SpawnController.Spawn(SpawnType.Mage);
            } else if (selected.gameObject.name.Contains("Tank"))
            {
                player.SpawnController.Spawn(SpawnType.Tank);
            }
        } else if (Controller.GetHairTriggerDown() && selected != null && upgradeControllerMenu.activeInHierarchy)
        {
            if (selected.gameObject.name.Contains("Archer"))
            {
                MinionAttribute attr = MinionAttribute.Damage;
                player.SpawnController.UpgradeMinionType(SpawnType.Archer, attr);
            }
            else if (selected.gameObject.name.Contains("Fighter"))
            {
                MinionAttribute attr = MinionAttribute.Damage;
                player.SpawnController.UpgradeMinionType(SpawnType.Fighter, attr);
            }
            else if (selected.gameObject.name.Contains("Mage"))
            {
                MinionAttribute attr = MinionAttribute.Damage;
                player.SpawnController.UpgradeMinionType(SpawnType.Mage, attr);
            }
            else if (selected.gameObject.name.Contains("Tank"))
            {
                MinionAttribute attr = MinionAttribute.Damage;
                player.SpawnController.UpgradeMinionType(SpawnType.Tank, attr);
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 fwd = rightController.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(rightController.transform.position, fwd * 50, Color.green);
        if (Physics.Raycast(rightController.transform.position, fwd, out hit, 50) && controllerMenu != null)
        {
            if (hit.collider.gameObject.GetComponent<HighlightScript>() != null)
            {
                if (controllerMenu.activeInHierarchy)
                {
                    for (int i = 0; i < controllerMenu.transform.childCount; i++)
                    {
                        controllerMenu.transform.GetChild(i).gameObject.GetComponent<HighlightScript>().highlight = false;
                    }
                } else if (upgradeControllerMenu.activeInHierarchy)
                {
                    for (int i = 0; i < upgradeControllerMenu.transform.childCount; i++)
                    {
                        upgradeControllerMenu.transform.GetChild(i).gameObject.GetComponent<HighlightScript>().highlight = false;
                    }
                }
                hit.collider.gameObject.GetComponent<HighlightScript>().highlight = true;
                selected = hit.collider.gameObject;
            }
        } 
    }

    void BringUpMenu()
    {
        if (controllerMenu.activeInHierarchy || upgradeControllerMenu.activeInHierarchy)
        {
            upgradeControllerMenu.SetActive(false);
            controllerMenu.SetActive(false);
            selected = null;
        } else
        {
            controllerMenu.SetActive(true);
        }
    }
}
