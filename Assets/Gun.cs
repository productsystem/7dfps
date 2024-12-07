using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public Rigidbody player;
    public float vacuumRange = 20f;
    public float pullSpeed = 10f;
    public float pullDistance = 1f;
    public float maxCharge = 10f;
    public float incrementRate = 5f;
    public float decrementRate = 3f;
    public TextMeshProUGUI chargeUI;
    private float knockbackMagnitude;
    public Transform eyeCam;
    public GameObject impactSphere;

    void Start()
    {
        knockbackMagnitude = 0f;
    }

    void Update()
    {
        knockbackMagnitude -= decrementRate * Time.deltaTime;
        knockbackMagnitude = Mathf.Clamp(knockbackMagnitude, 0 , maxCharge);

        if(Input.GetButtonDown("Fire1"))
        {
            EnemyKnockBack();
            Knockback();
        }
        if(Input.GetButton("Fire2"))
        {
            knockbackMagnitude += incrementRate * Time.deltaTime;
            Vacuum();
        }
        chargeUI.text = knockbackMagnitude.ToString();
    }

    void Vacuum()
    {
        if(Physics.Raycast(eyeCam.position, eyeCam.forward, out RaycastHit ray, vacuumRange))
        {
            Rigidbody target = ray.collider.GetComponent<Rigidbody>();
            if(target != null && ray.collider.CompareTag("Enemy"))
            {
                Vector3 direction = (eyeCam.position - target.position).normalized;
                target.MovePosition(target.position + direction * pullSpeed * Time.deltaTime);
                if(Vector3.Distance(target.position, eyeCam.position) <= pullDistance)
                {
                    Destroy(target.gameObject);
                }
            }
        }
    }

    void Knockback()
    {
        Vector3 knockbackDir = -firePoint.forward;
        player.AddForce(knockbackDir * knockbackMagnitude, ForceMode.Impulse);
        knockbackMagnitude = 0;
    }

    void EnemyKnockBack()
    {
        GameObject a = Instantiate(impactSphere, firePoint.position, firePoint.rotation);
        // if(a != null)
        // {
        //     Debug.Log(a.name);
        // }
        a.GetComponent<EnemyKnockBackSphere>().knockForce = knockbackMagnitude;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.forward * 2);
        Gizmos.DrawSphere(firePoint.position + firePoint.forward * 2, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(eyeCam.position, eyeCam.forward * vacuumRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(eyeCam.position, pullDistance);
    }
}
