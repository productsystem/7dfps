using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public string type;
    void Start()
    {
        if(type == null)
        {
            type = "Static";
        }
    }
}
