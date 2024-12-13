using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        GameObject score = GameObject.Find("Score");
        if(score != null)
        {
            score.GetComponent<TextMeshProUGUI>().text = "Electrums : " + LevelLoader.collectables;
        }
    }
}
