using UnityEngine;

public class EnvironmentInitialize : MonoBehaviour
{
    public PhysicMaterial groundMat;
    public int groundLayer;

    void Awake()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.layer = groundLayer;
            Collider col = child.GetComponent<Collider>();
            col.material = groundMat;
        }

    }
}
