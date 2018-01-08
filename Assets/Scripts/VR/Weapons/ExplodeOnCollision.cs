using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExplodeOnCollision : MonoBehaviour
{
    [SerializeField] private float maxDamage;
    [SerializeField] private float damageRadius;
    [SerializeField] private GameObject explosionParticles;
    [SerializeField] private GameObject explosionSound;
    public AudioClip bombAudio;
    private AudioSource audioSource;
    public float VolLowRange { get; set; }
    public float VolHighRange { get; set; }
    void OnCollisionEnter(Collision collision)
    {
        var colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider collider in colliders)
        {
            Minion minion = collider.gameObject.GetComponent<MinionController>()?.Minion;
            if(minion == null)
                continue;

            float damage = CalculateDamage(minion.Position);
            minion.TakeDamage(damage);
        }
        
        GameObject effect = Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
        GameObject sound = Instantiate(explosionSound);
        Destroy(effect, 2.5f);
        Destroy(sound, 5.0f);
       
        
        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        float damage = maxDamage * (1 - distance / damageRadius);
        return damage;
    }
}
