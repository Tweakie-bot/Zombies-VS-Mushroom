using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private int damage = 10;

    [SerializeField]
    private float lifeTime = 5f;


    public int Damage => damage;


    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }


    private void Update()
    {
        transform.position +=
            transform.forward
            * speed
            * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemyHealth =
            other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}