using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Seal : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer incompleteSeal = null;

    [SerializeField]
    Sprite sealMask = null;

    [SerializeField]
    [Range(1, 1000)]
    int PaintRadius = 1;

    [SerializeField]
    LayerMask raycastMask;

    public Text CompletedUiText;

    Texture2D sealTexture;

    Color[] maskPixels;
    Color[] sealPixels;
    Candle[] candles;

    float sealCompletion;
    private float CompletedSoundLength = 0.2f;
    private float ActualCompletedSoundLength = 0f;
    private AudioSource audioSource;
    private bool isDrawing = false;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.mute = true;
        sealCompletion = 0f;
        sealTexture = incompleteSeal.sprite.texture;
        maskPixels = sealMask.texture.GetPixels();
        sealPixels = sealTexture.GetPixels();

        // clear image
        for (int i = 0; i < sealPixels.Length; i++)
        {
            sealPixels[i] = new Color(sealPixels[i].r, sealPixels[i].g, sealPixels[i].b, 0f);
        }

        sealTexture.SetPixels(sealPixels);
        sealTexture.Apply();
        incompleteSeal.enabled = true;

        var collider = gameObject.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;

        var candleObjects = GameObject.FindGameObjectsWithTag("Candle");

        candles = new Candle[candleObjects.Length];
        for(int i = 0; i < candleObjects.Length; i++)
        {
            candles[i] = candleObjects[i].GetComponent<Candle>();
        }
    }

    public float GetCompletion()
    {
        return sealCompletion;
    }

    void UpdateSealSprite(Vector2 positionPercent)
    {
        StartDrawingSound();
        Vector2 point = new Vector2(positionPercent.x * sealTexture.width, positionPercent.y * sealTexture.height);
        int pixelIndex = (int)(sealTexture.width * point.y + point.x);

        for (int y = (int)point.y - PaintRadius; y < (int)point.y + PaintRadius; y++)
        {
            for (int x = (int)point.x - PaintRadius; x < (int)point.x + PaintRadius; x++)
            {
                if (x >= 0 && x < sealTexture.width && y >= 0 && y < sealTexture.height)
                {
                    var distance = Vector2.Distance(point, new Vector2(x, y));
                    if (distance < PaintRadius)
                    {
                        pixelIndex = (int)(sealTexture.width * y + x);

                        if (maskPixels[pixelIndex].a == 0f)
                            sealPixels[pixelIndex].a = 1f;
                    }
                }
            }
        }

        sealCompletion = CalculateSealCompletion();
        UpdateUiTextValue(sealCompletion);


        if (sealCompletion >= 0.98f)
        {
            if (PlayerPrefs.GetInt("CurrentLevel") == 5)
            {
                // trigger cat god appearance
            }
            else
            {
                PlayerPrefs.SetInt("PreviousLevel", PlayerPrefs.GetInt("CurrentLevel"));
                SceneManager.LoadScene("success");
            }
        }


        sealTexture.SetPixels(sealPixels);
        sealTexture.Apply();
    }

    void StartDrawingSound() {
        isDrawing = true;
        audioSource.mute = false;
        ActualCompletedSoundLength = 0;

        Invoke("UnsetDrawingSound", CompletedSoundLength);
    }

    void UnsetIsDrawing() {
        isDrawing = false;
    }

    void StopDrawingSound() {
        audioSource.mute = true;
        
    }

    void Update() {
        if (isDrawing) {
            ActualCompletedSoundLength += Time.deltaTime;
        }

        if(ActualCompletedSoundLength > CompletedSoundLength) {
            StopDrawingSound();
        }
    }


    void UpdateUiTextValue(float value) {
        CompletedUiText.text = (value * 100).ToString().Split('.')[0] + "%";
    }

    float CalculateSealCompletion()
    {
        float paintedPixels = 0;
        float visiblePixels = 0;

        for (int y = 0; y < sealTexture.height; y++)
        {
            for (int x = 0; x < sealTexture.width; x++)
            {
                var pixelIndex = (int)(sealTexture.width * y + x);

                if (maskPixels[pixelIndex].a == 0f)
                {
                    visiblePixels++;
                    if (sealPixels[pixelIndex].a == 1f)
                        paintedPixels++;
                }
            }
        }
        
        return paintedPixels/visiblePixels;
    }

    public void PaintSealAtPosition(Vector2 position)
    {
        foreach (var candle in candles)
        {
            if (!candle.isLit)
                return;
        }

        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, 1f, raycastMask);
        Vector2 percentage = new Vector2();

        if (hit.collider != null && hit.collider.tag == "Seal")
        {
            var other = hit.collider.gameObject;
            var imageSize = other.GetComponent<SpriteRenderer>().bounds.size;

            var bottomLeftPoint = (other.transform.position - imageSize * 0.5f);
            var topRightPoint = (other.transform.position + imageSize * 0.5f);

            percentage.x = (position.x - bottomLeftPoint.x) / (topRightPoint.x - bottomLeftPoint.x);
            percentage.y = (position.y - bottomLeftPoint.y) / (topRightPoint.y - bottomLeftPoint.y);

            UpdateSealSprite(percentage);
        }
    }


}
