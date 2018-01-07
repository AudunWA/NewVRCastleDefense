using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerController : MonoBehaviour
{
    private WorldController wo;
    public float range = 150.0f;
    private Minion target;
    public float damage = 25f;
    private float attackTimer = 0.0f;
    private float coolDown = 3.0f;

	private GameObject go;

	public GameObject projectile;

	private ExplodeOnCollision bombArrow;
	private DuplicateArrows rainArrow;

    // Use this for initialization
    void Start ()
    {
	    
    }
	
	// Update is called once per frame
	void Update () {
        FindTarget();
	    if (target != null)
	    {
	        if (attackTimer >= coolDown)
	        {
	            if (target.Health > 0)
	            {
	                ShootProjectile();
	            }
	            else
	            {
	                target = null;
	            }
	            attackTimer = 0;
	        }
	        attackTimer += Time.deltaTime;
        }
	}

    private void ShootProjectile() //Archer only for now
    {
	    float type = Random.Range(0, 100);

	    go = Instantiate(projectile);
	    
	    bombArrow = go.GetComponent<ExplodeOnCollision>();
	    rainArrow = go.GetComponent<DuplicateArrows>();

	    if (type > 98)
	    {
		    Destroy(go.GetComponent<ExplodeOnCollision>());
		    rainArrow.SendMessage("ArrowFired",SendMessageOptions.DontRequireReceiver);
		    Debug.Log("rain");
	    } else if (type > 94)
	    {
		    Destroy(rainArrow = go.GetComponent<DuplicateArrows>());
		    Debug.Log("bomb");
	    }
	    else
	    {
		    Destroy(go.GetComponent<ExplodeOnCollision>());
		    Destroy(rainArrow = go.GetComponent<DuplicateArrows>());
		    Debug.Log("normal");
	    }



	    go.GetComponent<EnemyPlayerProjectileController>().targetMinion = target;
        go.GetComponent<EnemyPlayerProjectileController>().parentGameObject = gameObject;
        go.GetComponent<EnemyPlayerProjectileController>().enemyPlayer = this;


        if (target.State == Minion.MinionState.Moving || target.State == Minion.MinionState.EnemyFound)
        {
            go.GetComponent<EnemyPlayerProjectileController>().moving = true;
            go.GetComponent<EnemyPlayerProjectileController>().enemyMovementspeed = target.Movementspeed;
        }
        else
        {
            go.GetComponent<EnemyPlayerProjectileController>().moving = false;
        }

    }

    private void FindTarget()
    {
        Collider[] inRange = Physics.OverlapSphere(gameObject.transform.position, range);
        foreach (Collider collision in inRange)
        {
            MinionController otherMinionController = collision.gameObject.GetComponent<MinionController>();
            if (otherMinionController?.Minion.Player == null ||
                otherMinionController.Minion.Player.PlayerType == PlayerType.Evil)
            {
          
            }
            else
            {
                target = otherMinionController.Minion;
            }
        }
    }
}
