using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Random = UnityEngine.Random;

public class MinionController : MonoBehaviour
{

    public event EventHandler<MinionStateEventArgs> StateChanged;

    private new Rigidbody rigidbody;
    private double maxHealth;
    public Image HealthBar;
    private float attackTimer = 0.0f;
    private bool moving = true;
    private FightSoundController fightSoundController;
    private WorldController worldController;
    private ObjectPooling spellPool;
    private ObjectPooling arrowPool;

    private NavMeshAgent agent;
    [SerializeField] private GameEntity targetEntity;

    private bool lightningAvailable = true;

    public Minion Minion;
    public Player Owner { get; set; }

    public NavMeshAgent Agent => agent;

    private Castle GetEnemyCastle()
    {
        return worldController.GetOtherPlayer(Owner).Castle;
    }

    // Use this for initialization
    private void Start()
    {
        GameObject goWorld = GameObject.FindWithTag("World");
        worldController = goWorld.GetComponent<WorldController>();
        fightSoundController = GetComponent<FightSoundController>();
        maxHealth = Minion.Health;
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        spellPool = GameObject.FindGameObjectWithTag("spellPool").GetComponent<ObjectPooling>();
        arrowPool = GameObject.FindGameObjectWithTag("arrowPool").GetComponent<ObjectPooling>();
        Agent.speed = Minion.Movementspeed;
        HealthBar.color = (Owner.PlayerType == PlayerType.Evil) ? Color.red : Color.green;

        StartCoroutine(UpdateTarget());
    }

    private IEnumerator UpdateTarget()
    {
        while (true)
        {
            if (targetEntity != null && targetEntity.gameObject == null)
            {
                targetEntity = null;
            }
            switch (Minion.State)
            {
                case Minion.MinionState.Moving:
                    Agent.isStopped = false;
                    GameEntity oldEntity = targetEntity;
                    FindNewTargetEntity();

                    if (Vector3.Distance(transform.position, targetEntity.GetAttackPosition(transform.position)) <=
                        Minion.Range)
                    {
                        Minion.State = Minion.MinionState.Fighting;
                        break;
                    }

                    // Hack to fix castle targeting 'jumping' in some cases
                    if (targetEntity == oldEntity && targetEntity is Castle)
                        break;

                    Vector3 destination = targetEntity.GetAttackPosition(transform.position);
                    Agent.destination = new Vector3(destination.x, 0, destination.z);
                    break;

                case Minion.MinionState.Fighting:
                    if (targetEntity == null ||
                        Vector3.Distance(transform.position, targetEntity.GetAttackPosition(transform.position)) >
                        Minion.Range)
                    {
                        Minion.State = Minion.MinionState.Moving;
                        break;
                    }
                    break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Used to render visual debug info.
    /// Uncomment Selected if you only want to see debug info of selected minions
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Draw calculated path
        Gizmos.color = GetStateColor();
        Vector3[] path = Agent.path.corners;
        Vector3 lastCorner = transform.position;
        foreach (Vector3 corner in path)
        {
            Gizmos.DrawLine(lastCorner, corner);
            lastCorner = corner;
        }
        Gizmos.DrawLine(lastCorner, Agent.destination);

        // Draw AI state as sphere above
        Gizmos.color = GetStateColor();
        Gizmos.DrawSphere(transform.position + new Vector3(0, 2.5f, 0), 0.5f);

        // Draw view range
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, Minion.Range);
    }

    /// <summary>
    /// Finds a color for a given AI state
    /// </summary>
    /// <returns></returns>
    private Color GetStateColor()
    {
        switch (Minion.State)
        {
            case Minion.MinionState.Moving:
                return Color.green;
            case Minion.MinionState.Fighting:
                return Color.red;
            case Minion.MinionState.Dead:
                return Color.black;
            case Minion.MinionState.Waiting:
                return Color.yellow;
            case Minion.MinionState.EnemyFound:
                return new Color(1, 0.54902f, 0); // Dark orange
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Minion.State == Minion.MinionState.Fighting)
            HandleAttack();
        UpdateVariables();
    }

    private void FindNewTargetEntity()
    {
        GameEntity newTarget = targetEntity;
        Collider[] inRange = Physics.OverlapSphere(Minion.Position, Minion.Range + 30.0f);
        foreach (Collider collision in inRange)
        {
            MinionController otherMinionController = collision.gameObject.GetComponent<MinionController>();
            if (otherMinionController == null || otherMinionController.Minion.Player == null ||
                otherMinionController.Minion.Player == Minion.Player)
                continue;

            if(!otherMinionController.Minion.IsAlive)
                continue;

            if (newTarget == null)
            {
                newTarget = otherMinionController.Minion;
            }
            else if (Vector3.Distance(transform.position, b: newTarget.GetAttackPosition(transform.position)) >
                     Vector3.Distance(transform.position,
                         otherMinionController.Minion.GetAttackPosition(transform.position)))
            {
                newTarget = otherMinionController.Minion;
            }
        }

        // Select evil castle if no minions has been found
        if (newTarget == null)
            newTarget = GetEnemyCastle();
        targetEntity = newTarget;
    }

    private void UpdateVariables()
    {
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, (float) (Minion.Health / maxHealth), 0.5f);

        // Update position for data class
        Minion.Position = GetComponent<Rigidbody>().position;
        if (!Minion.IsAlive && Minion.State != Minion.MinionState.Dead)
        {
            Minion.State = Minion.MinionState.Dead;
            agent.isStopped = true;
            if (gameObject.GetComponentInChildren<DummyArrowController>() != null)
            {
                while (gameObject.GetComponentInChildren<DummyArrowController>())
                {
                    DummyArrowController dummyarrows = gameObject.GetComponentInChildren<DummyArrowController>();
                    dummyarrows.unParent();
                    dummyarrows.gameObject.SetActive(false);
                }
            }

            Destroy(gameObject, 2f);
            bool removed = Minion.Player.RemoveMinion(Minion);
            StateChanged?.Invoke(this, new MinionStateEventArgs(Minion));
        }
    }

    /// <summary>
    /// Called before each physics update, do physics stuff here
    /// </summary>
    private void FixedUpdate()
    {
    }

    private void HandleAttack()
    {
        if (targetEntity == null || !targetEntity.IsAlive)
        {
            lightningAvailable = true;
            targetEntity = null;
            return;
        }

        Agent.isStopped = true;

        if (attackTimer >= Minion.AttackCooldownTime)
        {
            attackTimer = 0.0f;
            if (Minion is Archer)
            {
                ShootProjectile();
            }
            else if (Minion is Mage)
            {
                ShootSpell();
            }
            else
            {
                fightSoundController.PlayRandomFightSound();
                targetEntity.TakeDamage(Minion.Damage);
            }
            BroadcastMessage("OnAttack", Minion.AttackCooldownTime);
        }
        attackTimer += Time.deltaTime;
    }

    private void ShootProjectile() //Archer only for now
    {
        GameObject go = arrowPool.GetPooledObject();

        go.GetComponent<ArrowController>().targetMinion = targetEntity;
        go.GetComponent<ArrowController>().parentGameObject = gameObject;
        go.GetComponent<ArrowController>().parentMinion = Minion;
        if (targetEntity is Minion)
        {
            Minion target = (Minion) targetEntity;
            if (target.State == Minion.MinionState.Moving || target.State == Minion.MinionState.EnemyFound)
            {
                go.GetComponent<ArrowController>().moving = true;
                go.GetComponent<ArrowController>().enemyMovementspeed = target.Movementspeed;
            }
            else
            {
                go.GetComponent<ArrowController>().moving = false;
            }
        }
        else
        {
            go.GetComponent<ArrowController>().moving = false;
        }
        go.SetActive(true);
    }

    public float AngleCalculator(Vector3 thisObject, Vector3 targetObject)
    {
        Vector2 vec1 = new Vector2(thisObject.x, thisObject.z);
        Vector2 vec2 = new Vector2(targetObject.x, targetObject.z);

        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

    //THIS IS FOR SPELLS

    private void ShootSpell()
    {
        GetComponentInChildren<Animator>().SetTrigger("throw");
        Invoke("ActuallyShootSpell", 0.5f);

    }

    private void ActuallyShootSpell()
    {
        GameObject go = spellPool.GetPooledObject();
        go.GetComponent<SpellController>().minion = targetEntity;
        go.GetComponent<SpellController>().parentMinion = Minion;
        go.SetActive(true);

    }

    //THIS IS FOR SPELLS
    public GameObject lightning;

    private void ShootLightning() //Archer only for now
    {
        GameObject go = (GameObject) Instantiate(lightning);
        go.AddComponent<LightningController>(); //initiates the script
        go.GetComponent<LightningController>().minion = targetEntity;
    }

    public class MinionStateEventArgs : EventArgs
    {
        public Minion Minion { get; set; }

        public MinionStateEventArgs(Minion minion)
        {
            Minion = minion;
        }
    }

    /// <summary>
    /// Called when an arrow shot by a player hits this minion
    /// </summary>
    public void OnHitByPlayerArrow()
    {
        Minion.TakeDamage(50);
    }
}