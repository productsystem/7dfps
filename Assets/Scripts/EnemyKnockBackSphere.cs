using UnityEngine;

public class EnemyKnockBackSphere : MonoBehaviour
{
    public float knockForce;
    public float knockMultiplier = 10f;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.forward * knockForce * knockMultiplier, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
