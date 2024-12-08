using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public Rigidbody player;
    public GameObject bulletPrefab;
    public float vacuumRange = 20f;
    public float pullSpeed = 10f;
    public float bulletSpeed = 10f;
    public float pullDistance = 1f;
    public float maxCharge = 10f;
    public float incrementRate = 5f;
    public float decrementRate = 3f;
    public float knockbackMultiplier = 2f;
    public TextMeshProUGUI chargeUI;
    private float knockbackMagnitude;
    private bool bulletMode;
    private bool canVacuum;
    public Transform eyeCam;
    public GameObject impactSphere;

    void Start()
    {
        bulletMode = false;
        canVacuum = true;
        knockbackMagnitude = 0f;
        knockbackMultiplier = 2f;
    }

    void Update()
    {
        if(canVacuum)
        {
            knockbackMagnitude -= decrementRate * Time.deltaTime;
            knockbackMagnitude = Mathf.Clamp(knockbackMagnitude, 0 , maxCharge);
        }

        if(Input.GetButtonDown("Fire1"))
        {
            if(!bulletMode)
            {
                EnemyKnockBack();
                Knockback();
            }
            else
            {
                Bullet();
            }
        }
        if(Input.GetButton("Fire2") && canVacuum)
        {
            knockbackMagnitude += incrementRate * Time.deltaTime;
            Vacuum();
        }
        chargeUI.text = "Compressed Air : " + knockbackMagnitude.ToString();
    }

    void Bullet()
    {
        GameObject bullet = Instantiate(bulletPrefab,firePoint.transform.position, firePoint.transform.rotation);
        Rigidbody bulletrb = bullet.GetComponent<Rigidbody>();
        bulletrb.velocity = firePoint.forward * bulletSpeed;
        bulletMode = false;
        canVacuum = true;
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
                    string typeGet = target.GetComponent<EnemyBehaviour>().type;
                    Upgrade(typeGet);
                    canVacuum = false;
                    Destroy(target.gameObject);
                }
            }
        }
    }

    void Upgrade(string type)
    {
        if(type == "Static")
        {
            knockbackMagnitude = maxCharge;
        }

        if(type == "Strength")
        {
            knockbackMagnitude = maxCharge * knockbackMultiplier;
        }

        if(type == "Bullet")
        {
            bulletMode = true;
            knockbackMagnitude = 0;
        }
    }

    void Knockback()
    {
        Vector3 knockbackDir = -firePoint.forward;
        player.AddForce(knockbackDir * knockbackMagnitude, ForceMode.Impulse);
        knockbackMagnitude = 0;
        canVacuum = true;
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
