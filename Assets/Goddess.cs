using UnityEngine;
using System.Collections;

public class Goddess : MonoBehaviour
{
    public float offsetY = 1f;
    Vector3 initialPos;
    SpriteRenderer sr;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        initialPos = transform.position;
    }

    void Update()
    {
        // update position
        var offset = (Mathf.Sin(Time.timeSinceLevelLoad)*2f - 1f) * offsetY;
        transform.position = new Vector3(initialPos.x, initialPos.y + offset, initialPos.z);

        // fade in & out
        var fadeValue = Mathf.Sin(Time.timeSinceLevelLoad * 1.5f) * 0.4f + 0.1f;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fadeValue);
    }
}
