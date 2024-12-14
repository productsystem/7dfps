using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletKnockback = 10f;
    void Start()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Bullet");
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
        if(collision.collider.CompareTag("Bulletable"))
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("VoidWall");
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
        }
    }
}
