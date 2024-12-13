using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    [Header("Vacuum Controls")]
    public float vacuumRange = 10f;
    public float vacuumAngle = 60f;
    public float pullSpeed = 10f;
    public float pullDistance = 1f;
    public ParticleSystem vacuumParticle;
    private bool canVacuum;
    [Space(10)]
    [Header("Knockback")]
    public Transform firePoint;
    public Rigidbody player;
    public float guagePointerInitialAngle = 52f;
    public float gaugePointerFinalAngle = -90f;
    private RectTransform gaugePointer;
    public float shakeIntensity = 0.2f;
    public float shakeSpeed = 10f;
    public float maxCharge = 10f;
    public float incrementRate = 5f;
    public float decrementRate = 3f;
    public float knockbackMultiplier = 2f;
    private float knockbackMagnitude;

    [Space(10)]
    [Header("Collectable Interface")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public TextMeshProUGUI collectablesUI;
    private int currentLevelCollectables = 0;
    private bool bulletMode;
    private Vector3 ogPos;
    private Quaternion ogRot;
    public Transform eyeCam;
    public GameObject impactSphere;

    void Start()
    {
        currentLevelCollectables = 0;
        gaugePointer = GameObject.Find("GaugePointer").GetComponent<RectTransform>();
        vacuumParticle.Stop();
        ogPos= transform.localPosition;
        ogRot = transform.localRotation;
        bulletMode = false;
        canVacuum = true;
        knockbackMagnitude = 0f;
        knockbackMultiplier = 2f;
        gaugePointer.rotation = Quaternion.Euler(0,0,guagePointerInitialAngle);
    }

    void Update()
    {
        DeathPlane();

        GaugeBar();

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
        collectablesUI.text = "Electrums : " + LevelLoader.collectables;
    }


    
    void DeathPlane()
    {
        if(transform.position.y < -20f)
        {
            SceneManager.LoadScene(0);
        }
    }

    void GaugeBar()
    {
        float knockMagPointer = Mathf.Lerp(52,gaugePointerFinalAngle,knockbackMagnitude/30f);
        gaugePointer.rotation = Quaternion.Euler(0,0,knockMagPointer);
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
        Collider[] hitColliders = Physics.OverlapSphere(eyeCam.position, vacuumRange);

        foreach (Collider collider in hitColliders)
        {
            if(collider.CompareTag("Enemy"))
            {
                Vector3 directionToTarget = (collider.transform.position - eyeCam.position).normalized;
                float angleBetween = Vector3.Angle(eyeCam.forward, directionToTarget);

                if (angleBetween <= vacuumAngle / 2f)
                {
                    Rigidbody target = collider.GetComponent<Rigidbody>();

                    if (target != null)
                    {
                        Vector3 pullDirection = (eyeCam.position - target.position).normalized;
                        target.MovePosition(target.position + pullDirection * pullSpeed * Time.deltaTime);

                        if (Vector3.Distance(target.position, eyeCam.position) <= pullDistance)
                        {
                            string typeGet = target.GetComponent<EnemyBehaviour>().type;
                            canVacuum = false;
                            Upgrade(typeGet);
                            Destroy(target.gameObject);
                        }
                    }
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

        if(type == "Collectable")
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Electrum");
            canVacuum = true;
            currentLevelCollectables++;
            LevelLoader.collectables++;
        }

        if(type == "Strength")
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Super");
            knockbackMagnitude = maxCharge * knockbackMultiplier;
        }

        if(type == "Bullet")
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Void");
            bulletMode = true;
            knockbackMagnitude = 0;
        }
    }

    void Knockback()
    {
        if(knockbackMagnitude > 0)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Knock");
        }
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

    public void ResetCollectables()
    {
        LevelLoader.collectables -= currentLevelCollectables;
    }
}
