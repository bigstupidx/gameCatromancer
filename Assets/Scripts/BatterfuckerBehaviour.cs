using UnityEngine;
using System.Collections;

public class BatterfuckerBehaviour : MonoBehaviour {

    public GameObject explosionPrefab;

    public float Speed = 10f;
    public int Damage = 10;
    private float SinusAmpRatio = 0.1f;
    private float SinusFreqRatio = 2f;
    public float Y_sprite_padding = 5;
    public float X_sprite_padding = 5;


    private Vector3 speed_vector;
    private float x_max, x_min, y_max, y_min;
    private PlayerController Cat;

    private const int ProjectileHeroLAYER = 10;

    public AudioClip SpawnSound;
    //public AudioClip HitSound;
    public AudioClip DieSound;
    private AudioSource audioSource;



    // Use this for initialization
    void Start () {

        Cat = GameObject.FindObjectOfType<PlayerController>();
        audioSource = gameObject.GetComponent<AudioSource>();
        SinusAmpRatio = Random.RandomRange(0, 0.25f);
        SinusFreqRatio = Random.RandomRange(0, 2f);

        transform.localScale = new Vector3(transform.localScale.x * Random.value > 0.5f ? 1 : -1, transform.localScale.y, transform.localScale.z);

        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMostCamera = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0, distance));
        Vector3 rightMostCamera = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0, distance));
        Vector3 topMostCamera = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 botMostCamera = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance));
        print(rightMostCamera.x);
        x_min = leftMostCamera.x + X_sprite_padding;
        x_max = rightMostCamera.x - X_sprite_padding;
        y_min = topMostCamera.y + Y_sprite_padding;
        y_max = botMostCamera.y - Y_sprite_padding;

     
    
        float x_speed = Random.Range(0, Speed);
        float y_speed = Speed - x_speed;
        speed_vector = new Vector3(x_speed, y_speed,0);


        audioSource.PlayOneShot(SpawnSound);
        InvokeRepeating("SpontaniusDirectionChange", 3f, 3f);

    }


    

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("BatfackerCollision");
        Debug.Log("xxx" + collision.collider.gameObject.layer);

        speed_vector = Cat.transform.position - transform.position;
        

    }

    
    void DIE() {
        AudioSource.PlayClipAtPoint(DieSound, transform.position);
        Destroy(gameObject);
        GameObject expl = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(expl, 0.5f);

    }

    void OnTriggerEnter2D(Collider2D collider) {
        speed_vector = Cat.transform.position - transform.position;
        if (collider.gameObject.layer == ProjectileHeroLAYER) {
            DIE();
        }
    }

    // Update is called once per frame
    void Update () {

        float dt = Time.deltaTime;
        transform.position += new Vector3(speed_vector.x * dt + Mathf.Sin(Time.timeSinceLevelLoad * SinusFreqRatio) * SinusAmpRatio, speed_vector.y * dt + Mathf.Sin(Time.timeSinceLevelLoad * SinusFreqRatio) * SinusAmpRatio, 0);
        //print(Mathf.Sin(Time.timeSinceLevelLoad*SinusFreqRatio)*SinusAmpRatio);
        //Clam boundaries

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, x_min, x_max), Mathf.Clamp(transform.position.y, y_min, y_max), 0);


    }

    void SpontaniusDirectionChange () {
        //speed_vector = Cat.transform.position - transform.position;
        float rand_x = Random.RandomRange(-Speed, Speed);
        float rand_y;
        if (Random.Range(0,1) < 0.5f) {
            rand_y = Speed + rand_x;

        }
        else {
            rand_y = -Speed + rand_x;
        }
        rand_y = Mathf.Clamp(rand_y, -Speed, Speed);
        speed_vector = new Vector3(rand_x, rand_y, 0);






    }
}
