using System;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public Rigidbody player;
    public GameObject bulletPrefab;
    public ParticleSystem vacuumParticle;
    public RectTransform gaugePointer;
    public float vacuumRange = 20f;
    public float shakeIntensity = 0.2f;
    public float recoilAngle = 60f;
    public float shakeSpeed = 10f;
    public float pullSpeed = 10f;
    public float recoilSpeed = 10f;
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
    private Vector3 ogPos;
    private Quaternion ogRot;
    public Transform eyeCam;
    public GameObject impactSphere;

    void Start()
    {
        gaugePointer = GameObject.Find("GaugePointer").GetComponent<RectTransform>();
        vacuumParticle.Stop();
        ogPos= transform.localPosition;
        ogRot = transform.localRotation;
        bulletMode = false;
        canVacuum = true;
        knockbackMagnitude = 0f;
        knockbackMultiplier = 2f;
        gaugePointer.rotation = Quaternion.Euler(0,0,52);
    }

    void Update()
    {
        float knockMagPointer = Mathf.Lerp(52,-90,knockbackMagnitude/30f);
        gaugePointer.rotation = Quaternion.Euler(0,0,knockMagPointer);
        if(canVacuum)
        {
            knockbackMagnitude -= decrementRate * Time.deltaTime;
            knockbackMagnitude = Mathf.Clamp(knockbackMagnitude, 0 , maxCharge);
        }

        if(Input.GetButtonDown("Fire1"))
        {
            ResetGun();
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
        if(Input.GetButtonUp("Fire2"))
        {
            ResetGun();
            if(vacuumParticle.isPlaying)
            vacuumParticle.Stop();
        }
        chargeUI.text = "Compressed Air : " + String.Format("{0:0.00}",knockbackMagnitude);
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
        if(vacuumParticle.isPlaying == false)
        {
            vacuumParticle.Play();
        }
        GunShake();
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

    void GunShake()
    {
        Vector3 shakePos = ogPos + UnityEngine.Random.insideUnitSphere * shakeIntensity;
        float randos = UnityEngine.Random.Range(-shakeIntensity, shakeIntensity);
        Quaternion shakeRot = ogRot * Quaternion.Euler(randos, randos, randos);
        transform.localPosition = Vector3.Lerp(transform.localPosition, shakePos, Time.deltaTime * shakeSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, shakeRot, Time.deltaTime * shakeSpeed);
    }

    void ResetGun()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, ogPos, Time.deltaTime * shakeSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, ogRot, Time.deltaTime * shakeSpeed);
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
        a.GetComponent<EnemyKnockBackSphere>().knockForce = knockbackMagnitude;
    }
}
