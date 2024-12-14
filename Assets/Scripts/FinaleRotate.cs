using UnityEngine;

public class FinaleRotate : MonoBehaviour
{
    public float rotationSpeed = 50f;

    private Vector3 randomRotation;

    void Start()
    {
        randomRotation = GenerateRandomDirection();
    }

    void Update()
    {
        transform.Rotate(randomRotation * rotationSpeed * Time.deltaTime);
    }

    
    private Vector3 GenerateRandomDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        return new Vector3(x, y, z).normalized;
    }
}
