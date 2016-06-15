using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GUIManager : MonoBehaviour {

	// Use this for initialization

    private GameObject adjustDialog;

	void Start () 
    {
        adjustDialog = GameObject.Find("adjustDialog");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.K))
        {
            adjustDialog.SetActive(true);
        }
	}
}
