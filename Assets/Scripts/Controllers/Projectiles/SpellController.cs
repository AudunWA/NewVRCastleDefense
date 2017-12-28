using UnityEngine;

public class SpellController : MonoBehaviour {

    public GameEntity minion;
    public Minion parentMinion;
    bool moving = true;

    private void OnEnable()
    {
        Invoke("Destroy", 2.0f);
        float random = Random.Range(0.3f, 2.5f);
        if (parentMinion != null)
        {
            Vector3 spellPosition = new Vector3(0, 10.0f, 0);
            spellPosition = spellPosition + parentMinion.Position;
            transform.position = spellPosition;
        }
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //TODO FIX THIS
        if (minion != null)
        {
            Minion min = minion as Minion;
            if (min != null && min.State == Minion.minionState.Dead)
            {
                gameObject.SetActive(false);
            }
            if (moving)
            {
                float distance = Vector3.Distance(gameObject.transform.position, minion.Position);

                transform.Translate(Vector3.forward * Time.deltaTime * 50);

                var rotation = Quaternion.LookRotation(minion.Position - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1000);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == parentMinion.gameObject)
        {
            return;
        }
        if (collision.gameObject.tag == "minion")
        {
            MinionController controller = collision.gameObject.GetComponent<MinionController>();
            if (parentMinion.Player != controller.Minion.Player)
            {
                controller.Minion.TakeDamage(parentMinion.Damage);
                gameObject.SetActive(false);
            }
        } else if (collision.gameObject.tag.Contains("Castle"))
        {
            CastleController controller = collision.gameObject.GetComponent<CastleController>();
            controller.Castle.TakeDamage(parentMinion.Damage);
            gameObject.SetActive(false);
                
        }
    }
}
