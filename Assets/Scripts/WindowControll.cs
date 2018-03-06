using UnityEngine;

public class WindowControll : MonoBehaviour {
    public bool playerInBounds = false;

    public AudioClip SoundWindowsOpen;
    public AudioClip SoundWindowsClose;
    public Sprite WindowOpen;
    public Sprite WindowClose;
    public float AutoOpenTime;

    private SpriteRenderer spriteRenderer;
    private SpawnerController spawner;
    private AudioSource audioSource;

    public bool isOpen = false;
    
    private const string HeroNAME = "Catomancer";
    // Use this for initialization
    void Start()
    {
        spriteRenderer = gameObject.GetComponentInParent<SpriteRenderer>();
        spawner = gameObject.GetComponentInParent<SpawnerController>();
        audioSource = gameObject.GetComponent<AudioSource>();
        CloseWindow();
        Invoke("OpenWindow", AutoOpenTime);
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space) && playerInBounds && isOpen) {
            CloseWindow();
            Invoke("OpenWindow", AutoOpenTime);
        }
    }

    void CloseWindow() {
        spriteRenderer.sprite = WindowClose;
        
        spawner.Active = false;
        isOpen = false;

        audioSource.PlayOneShot(SoundWindowsClose);
    }

    void OpenWindow() {
        spriteRenderer.sprite = WindowOpen;
        spawner.Active = true;
        isOpen = true;

        audioSource.PlayOneShot(SoundWindowsOpen);
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