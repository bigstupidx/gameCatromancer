using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }

    public static int LevelCount()
    {
        return SceneManager.sceneCountInBuildSettings;
    }

    public void LoadNextLevel()
    {
        int ActualScene = SceneManager.GetActiveScene().buildIndex;
        if (ActualScene >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene("Win");
        }
        SceneManager.LoadScene(ActualScene + 1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
