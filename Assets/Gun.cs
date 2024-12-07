using System;
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
}
