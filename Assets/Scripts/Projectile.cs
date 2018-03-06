using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;

    SpriteRenderer spriteRenderer;
    bool seen;

    public Vector2 direction { get; set; }
    public GameObject player { get; set; }

    public AudioClip MissleLunch;
    //public AudioClip MissleDestroyed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AudioSource.PlayClipAtPoint(MissleLunch,transform.position);
    }

    void Update()
    {
        if (spriteRenderer.isVisible)
            seen = true;

        if (seen && !spriteRenderer.isVisible)
            Destroy();

        transform.position += new Vector3(direction.x * speed, direction.y * speed, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy();
    }

    void Destroy()
    {
        //AudioSource.PlayClipAtPoint(MissleDestroyed, transform.position);
        Destroy(gameObject);
    }
}
