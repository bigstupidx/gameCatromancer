using UnityEngine;
using System.Collections;
using System;
using UnityRandom = UnityEngine.Random;

public class DemonDogBehavior : MonoBehaviour
{
    Transform playerTransform;
    public Vector3 SpawnShift;
    private float x_max, x_min, y_max, y_min;

    public int hitsToKill = 2;
    public GameObject explosionPrefab;
    private const int ProjectileHeroLAYER = 10;

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    float projectileOffset;

    [SerializeField]
    float shotIntervalBase;

    private DateTime ShotInterval { get; set; }

    GameObject currentProjectile;
    private AudioSource audioSource;
    public AudioClip SpawnSound;
    public AudioClip HitSound;
    public AudioClip DieSound;

    void Start ()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.transform.position = transform.position + SpawnShift;
        ShotInterval = DateTime.Now;
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(SpawnSound);
    }

    void DIE() {
        Destroy(gameObject);
        GameObject expl = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        AudioSource.PlayClipAtPoint(DieSound,transform.position);
        Destroy(expl, 0.5f);

    }

    void OnTriggerEnter2D(Collider2D collider) {


        if (collider.gameObject.layer == ProjectileHeroLAYER) {
            hitsToKill--;
            if (hitsToKill <= 0) {
                DIE();
            }
            audioSource.PlayOneShot(HitSound);

        }
    }

    void Update ()
    {
        if (DateTime.Now - ShotInterval > TimeSpan.FromSeconds(shotIntervalBase + (UnityRandom.value * 2)))
        {
            currentProjectile = CreateProjectile();
            ShotInterval = DateTime.Now;
        }
        
        gameObject.transform.position += new Vector3(UnityRandom.value - 0.5f, UnityRandom.value - 0.5f, 0);
    }

    GameObject CreateProjectile()
    {
        Vector2 direction = (Vector2)playerTransform.position - (Vector2)transform.position;
        direction.Normalize();

        GameObject projectile = Instantiate(projectilePrefab);
        projectile.GetComponent<Projectile>().direction = direction;

        // Get Angle in Radians
        float AngleRad =
            Mathf.Atan2(playerTransform.position.y - gameObject.transform.position.y, playerTransform.position.x - gameObject.transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        projectile.GetComponent<Projectile>().transform.rotation = Quaternion.Euler(0, 0, AngleDeg);

        projectile.transform.position = transform.position + new Vector3(projectileOffset * direction.x, projectileOffset * direction.y);

        return projectile;
    }
}
