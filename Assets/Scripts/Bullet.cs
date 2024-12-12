using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletKnockback = 10f;
    void Start()
    {
        Destroy(gameObject, 2f);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            Rigidbody enemyrb = collision.collider.GetComponent<Rigidbody>();
            Vector3 knockDirection = collision.transform.position - transform.position;
            knockDirection.Normalize();
            enemyrb.AddForce(knockDirection * bulletKnockback, ForceMode.Impulse);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
