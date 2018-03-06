using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
	void OnMouseUp()
    {
        PlayerPrefs.SetInt("CurrentLevel", 1);
        SceneManager.LoadScene("level1");
	}
}
