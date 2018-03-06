using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {


    public AudioClip[] LevelMusic;
    // Use this for initialization
    public static MusicManager instance = null;
    private AudioSource audioSource;

    private int lastSceneIdx = 0;


    private void getInstance() {
        if (instance != null) {
            Destroy(gameObject);

        }
        else {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    void Awake() {
        
        
        getInstance();
        
    }

    void Start () {
        

    }

    void Update() {
       

    }

    public void ChangeVolume(float volume) {
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = volume;
    }
	
	// Update is called once per frame
	void OnLevelWasLoaded (int level) {

        try {
            int ActualSceneIdx = SceneManager.GetActiveScene().buildIndex;
            AudioClip thisLevelMusic = LevelMusic[ActualSceneIdx];
            audioSource = GetComponent<AudioSource>();
           // if (thisLevelMusic && lastSceneIdx != level) {
                lastSceneIdx = level;
                audioSource.clip = thisLevelMusic;
                audioSource.Play();
           // }
        }
        catch {
            Debug.Log("not found clip " + name + "  " + level.ToString());
        }


    }
}
