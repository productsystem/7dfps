using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static int levelNum = 1;
    public static int collectables = 0;
    public void IncrementLevels()
    {
        levelNum++;
    }
}
