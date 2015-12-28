using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{
    public GameObject target;
    public Camera camera;
    private Vector3 offset;
    private RectTransform rectTransform;
    // Use this for initialization
	void Awake ()
	{
	    offset = gameObject.transform.position - target.transform.position;
	    rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        rectTransform.position = camera.WorldToScreenPoint(target.transform.position);
	}
}
