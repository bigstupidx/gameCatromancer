using UnityEngine;
using System.Collections;

public class TextCloseEvilScript : MonoBehaviour {

    public GameObject textObject;

    private WindowControll window;

    // Use this for initialization
    void Start () {
        window = gameObject.GetComponent<WindowControll>();
        textObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	    if (window.isOpen)
        {
            textObject.SetActive(true);
        }
	}
}
