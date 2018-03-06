using UnityEngine;
using System.Collections;

public class PortalCommander : MonoBehaviour {

    public bool playerInBounds = false;
    public bool isOpen = false;



    public Sprite PortalOpen;
    public Sprite PortalClose;
    public float AutoOpenTime = 15f;

    public AudioClip soundOpen;
    public AudioClip soundClose;
    public AudioClip soundIsOppen;
    private AudioSource audioSource; 


    private SpriteRenderer spriteRenderer;
    private SpawnerController spawner;
    
    private const string HeroNAME = "Catomancer";

    void Start()
    {
        spriteRenderer = gameObject.GetComponentInParent<SpriteRenderer>();
        spawner = gameObject.GetComponentInParent<SpawnerController>();
        audioSource = gameObject.GetComponent<AudioSource>();
        ClosePortal();
        Invoke("OpenPortal", AutoOpenTime);
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space) && playerInBounds && isOpen) {
            ClosePortal();
            Debug.Log("CloseREQUEST");
            Invoke("OpenPortal", AutoOpenTime);



        }




    }

    void ClosePortal() {
        spriteRenderer.sprite = PortalClose;
        spawner.Active = false;
        isOpen = false;
        audioSource.PlayOneShot(soundClose);
    }

    void OpenPortal() {
        spriteRenderer.sprite = PortalOpen;
        spawner.Active = true;
        isOpen = true;
        audioSource.PlayOneShot(soundOpen);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        ///Debug.Log("WindowCollision " + collision.collider);

    }



    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == HeroNAME) {
            playerInBounds = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.name == HeroNAME) {
            playerInBounds = false;

        }
    }
}