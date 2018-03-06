using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Sprite))]
public class PlayerController : MonoBehaviour
{
    public bool isImmortal = false;
    public int LifesCount = 5;

    private const int EnemyLAYER = 9;
    private const int ProjectileEnemyLAYER = 11;

    public Color defaultColor;

    public float ImmortalTime = 2f;
    public float blinkFrequency = 0.1f;
    public Color blinkColor;
    public Color glowColor;
    Sprite sprite;
    SpriteRenderer spriteRenderer;
    Vector2 direction;
    PolygonCollider2D polygonCollider;

    [SerializeField]
    float movementSpeed;

    [SerializeField]
    float projectileOffset;

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    Seal seal;

    GameObject currentProjectile;
    Animator animator;
    Vector3 originalScale;

    private AudioSource audioSource;
    public AudioClip[] soundCatHit;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        audioSource = gameObject.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        float distance = transform.position.z - Camera.main.transform.position.z;
    }

    void Update()
    {
        if (LifesCount == 0)
        {
            SceneManager.LoadScene("gameover");
        }

        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        bool lc = Input.GetButtonDown("Fire1");
        bool rc = Input.GetButton("Fire2");
        HandleMovement(direction);
        HandleMouse(lc, rc);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        int collision_layer = collider.gameObject.layer;
        if (collision_layer == ProjectileEnemyLAYER || collision_layer == EnemyLAYER)
        {
            HandleHit();
        }
    }

    private void HandleMouse(bool leftClick, bool rightClick)
    {
        if (leftClick && currentProjectile == null)
        {
            currentProjectile = CreateProjectile(Input.mousePosition);
        }
        if (rightClick)
        {
            seal.PaintSealAtPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void HandleMovement(Vector2 direction)
    {
        float SpeedX = direction.x * movementSpeed;
        float SpeedY = direction.y * movementSpeed;
        transform.position += new Vector3(SpeedX, SpeedY, 0);

        animator.SetBool("MoveLeftRight", direction.x != 0);

        if (direction.x < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = originalScale;
    }

    GameObject CreateProjectile(Vector2 mousePosition)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        GameObject projectile = Instantiate(projectilePrefab);
        projectile.GetComponent<Projectile>().direction = direction;

        // Get Angle in Radians
        float AngleRad =
            Mathf.Atan2(mousePosition.y - gameObject.transform.position.y, mousePosition.x - gameObject.transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        projectile.GetComponent<Projectile>().transform.rotation = Quaternion.Euler(0, 0, AngleDeg);

        projectile.transform.position = transform.position + new Vector3(projectileOffset * direction.x, projectileOffset * direction.y);

        return projectile;
    }

    void SetMortal()
    {
        isImmortal = false;
    }

    void SetImmortal()
    {
        isImmortal = true;
        Invoke("SetMortal", ImmortalTime);
        //polygonCollider.enabled = false;
    }

    void HandleHit()
    {

        if (!isImmortal)
        {
            audioSource.PlayOneShot(soundCatHit[Random.Range(0,3)]);
            SetImmortal();
            StartBlinkSprite();
            LifesCount--;
        }
    }

    void StartBlinkSprite()
    {
        InvokeRepeating("SetSpriteHi", 0, blinkFrequency);
        //WaitForSeconds(0.25f);
        InvokeRepeating("SetSpriteLow", blinkFrequency / 2, blinkFrequency);
        Invoke("StopBlinkSprie", ImmortalTime);
    }

    void StopBlinkSprie()
    {
        CancelInvoke("SetSpriteHi");
        CancelInvoke("SetSpriteLow");
        //polygonCollider.enabled = true;
        spriteRenderer.color = defaultColor;
    }

    void SetSpriteHi()
    {
        spriteRenderer.color = glowColor;
    }

    void SetSpriteLow()
    {
        spriteRenderer.color = blinkColor;
    }
}
