using UnityEngine;
using System.Collections;
using System;
using UnityRandom = UnityEngine.Random;

public class SpawnerController : MonoBehaviour
{
    public GameObject Monster;
    public bool TimeToActivate;
    public bool Active;
    public bool PlayerTriggeredClosing;
    public float SpawnIntervalBase;
    public Vector3 SpawnPositionShift = new Vector3(0, -1);
    private float timeFromLastSpawn;
    public bool EpicStart = false;


    private DateTime LastTimeSpawned;

    void Start() {
        if (EpicStart) {
            timeFromLastSpawn = SpawnIntervalBase;
        }
        else {
            timeFromLastSpawn = 0;
        }
    }
	
	void Update ()
    {
        
        if (Active)
        {
            timeFromLastSpawn += Time.deltaTime;

        }
        else {
            if (EpicStart) {
                timeFromLastSpawn = SpawnIntervalBase;
            }
        }

	    if (timeFromLastSpawn > SpawnIntervalBase )
        {
            SpawnMonster(transform.position);
            timeFromLastSpawn = 0;
        }
    }

    GameObject SpawnMonster(Vector2 spawnerPosition)
    {
        Vector3 pos = transform.position + SpawnPositionShift;
        try {
            GameObject monster = Instantiate(Monster, transform.position, Quaternion.identity) as GameObject;
        }
        catch {

        }
        //monster.transform.position = transform.position + new Vector3(SpawnPositionShift.x, SpawnPositionShift.y);
        return null;
    }
}
