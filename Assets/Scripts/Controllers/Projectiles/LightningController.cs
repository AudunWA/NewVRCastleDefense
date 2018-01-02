using UnityEngine;

public class LightningController : MonoBehaviour
{

    public GameEntity minion;

    void Start()
    {
        Vector3 position = new Vector3(minion.Position.x, minion.Position.y + 40f, minion.Position.z);
        transform.position = position;    }

    private void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 position = new Vector3(minion.Position.x, minion.Position.y + 40f, minion.Position.z);
        transform.position = position;

        //TODO FIX THIS
        if (minion != null)
        {
            Minion min = minion as Minion;
            if (min != null && min.State == Minion.MinionState.Dead)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
