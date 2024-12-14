using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGoal : MonoBehaviour
{
    public Transform player;
    private GameObject levelLoader;
    public float pullForce = 20f;
    public float pullRadius = 2f;
    void Awake()
    {
        levelLoader = GameObject.Find("LevelLoader");
    }

    void Update()
    {
        Vector3 horizontalDistance = new Vector3(player.position.x,0,player.position.z) - new Vector3(transform.position.x,0,transform.position.z);
        if(horizontalDistance.magnitude < pullRadius && player.position.y < transform.position.y)
        {
            Vector3 pullDirection = (transform.position - player.position).normalized;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0,pullForce, 0);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelLoader.GetComponent<LevelLoader>().IncrementLevels();
            SceneManager.LoadScene(LevelLoader.levelNum);
        }
    }
}
