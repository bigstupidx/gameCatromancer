using UnityEngine;
using System.Collections;
using System;

public class LevelController : MonoBehaviour
{
    public int BaseEventInterval;

    private DateTime lastEvent;

	void Start ()
    {
	    
	}
	
	void Update ()
    {
	    if (DateTime.Now - lastEvent > TimeSpan.FromSeconds(5))
        {
            // zrób event: otwórz portal lub rozwiń zwój lub otwórz okno
        }
	}
}
