using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public Rigidbody player;
    public float maxCharge = 10f;
    public float incrementRate = 5f;
    public float decrementRate = 3f;
    public TextMeshProUGUI chargeUI;
    private float knockbackMagnitude;
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
        }
        chargeUI.text = knockbackMagnitude.ToString();
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
}
