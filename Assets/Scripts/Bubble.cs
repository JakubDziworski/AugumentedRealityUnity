using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Bubble : MonoBehaviour
{

    public Camera camera;
    private CanvasGroup canvasGroup;
    private const float animDuration = 0.5f;
    private Vector3 defaultScale;

	// Use this for initialization
	void Awake () 
    {
	    canvasGroup = GetComponent<CanvasGroup>();
            HOTween.defEaseType = EaseType.EaseInOutExpo;
	    defaultScale = canvasGroup.transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
                camera.transform.rotation * Vector3.up);
	}

    public void Show()
    {
        StopAnimations();
        HOTween.To(canvasGroup, 0.1f, "alpha", 1f, false).Play();
        HOTween.To(canvasGroup.transform, animDuration, "localScale", defaultScale, false, EaseType.EaseOutBack, 0).Play();
    }

    public void Hide()
    {
        StopAnimations();
        HOTween.To(canvasGroup, animDuration / 2.0f, "alpha", 0f, false, EaseType.EaseInOutExpo, animDuration - 0.1f).Play();
        HOTween.To(canvasGroup.transform, animDuration, "localScale", new Vector3(), false, EaseType.EaseInBack, 0).Play();
    }

    private void StopAnimations()
    {
        HOTween.Complete(canvasGroup);
        HOTween.Complete(canvasGroup.transform);
    }
}
