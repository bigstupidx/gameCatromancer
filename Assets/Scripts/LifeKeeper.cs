using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeKeeper : MonoBehaviour
{
    private GameObject HeartPrefab;
    private PlayerController Player;

    [SerializeField]
    Sprite LifeOn;

    [SerializeField]
    Sprite LifeOff;

    void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        NextFreePosition();
    }
    
    Transform NextFreePosition()
    {
        int lifes = Player.LifesCount;
        int count = 0;
        //Generet heart icon
        foreach (Transform child in transform)
        {
            count++;
            //print(child.name);
            if (count <= lifes)
            {
                child.GetComponent<SpriteRenderer>().sprite = LifeOn;
            }
            else
            {
                child.GetComponent<SpriteRenderer>().sprite = LifeOff;
            }
        }
        return null;
    }
}
