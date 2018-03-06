using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransientScreenController : MonoBehaviour
{
    public GameObject ContinueSign;
    public int SceneNr;
    public bool Died;

    private DateTime AppearanceTime;
    private bool CanBeClosed;

    void Start()
    {
        CanBeClosed = false;
        ContinueSign.GetComponent<Renderer>().enabled = false;
        AppearanceTime = DateTime.Now;
    }

    void Update()
    {
        if (DateTime.Now - AppearanceTime > TimeSpan.FromSeconds(2))
        {
            ContinueSign.GetComponent<Renderer>().enabled = true;
            CanBeClosed = true;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Return) ||
            Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Space))
        {
            if (CanBeClosed)
            {
                if (Died || Application.loadedLevelName == "endgame")
                {
                    SceneManager.LoadScene("menu");
                }
                else
                {
                    int newLevel = PlayerPrefs.GetInt("PreviousLevel") + 1;
                    PlayerPrefs.SetInt("CurrentLevel", newLevel);

                    if (newLevel == 4)
                    {
                        SceneManager.LoadScene("endgame");
                    }
                    else if (newLevel == 3)
                    {
                        SceneManager.LoadScene("gameplay");
                    }
                    else
                    {
                        SceneManager.LoadScene("level" + newLevel);
                    }
                }
            }
        }
    }
}
