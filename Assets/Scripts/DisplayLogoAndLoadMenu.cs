using UnityEngine;
using System.Collections;

public class DisplayLogoAndLoadMenu : MonoBehaviour {

    public float delay_Seconds = 5;
    public AudioClip LogoSound;

    private float time_delayed = 0;
    // Use this for initialization
    void Start () {
        AudioSource.PlayClipAtPoint(LogoSound, transform.position);

    }
	
	// Update is called once per frame
	void Update () {
        time_delayed += Time.deltaTime;
        if (time_delayed > delay_Seconds) {
            LevelManager levelManger;
            levelManger = GameObject.FindObjectOfType<LevelManager>();
            levelManger.LoadLevel("00 Disclaimer");

        }
	
	}
}
