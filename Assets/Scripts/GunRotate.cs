using UnityEngine;

public class GunRotate : MonoBehaviour
{
    public float rotateSpeed = 5f;
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }
}
