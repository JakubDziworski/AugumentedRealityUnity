using UnityEngine;
using System.Linq;
using Assets.Scripts;
using Assets.Utils;
using Holoville.HOTween;

public class Bubble2d : MonoBehaviour
{

    public GameObject trackedObject;
    private CanvasGroup canvasGroup;
    private const float animDuration = 0.5f;
    private Vector3 defaultScale;
    private RectTransform rectTransform;
    private RectTransform slidingPart;
    // Use this for initialization
    void Awake()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        slidingPart = Utils.GetChildGameObject(gameObject, "slidingPanel").GetComponent<RectTransform>();
        canvasGroup.alpha = 0;
        HOTween.defEaseType = EaseType.EaseInOutExpo;
        defaultScale = canvasGroup.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = Camera.main.WorldToScreenPoint(trackedObject.transform.position);
        checkOverlapping();
    }


    void checkOverlapping()
    {
        FindObjectsOfType<Bubble2d>()
            .ToList()
            .Where(bubble => bubble != this)
            .Where(bubble => bubble.GetComponent<CanvasGroup>().alpha > 0)
            .Select(bubble => bubble.rectTransform)
            .Where(otherRect => otherRect.Overlaps(rectTransform))
            .ToList().ForEach(SolveCollision);
    }

    private void SolveCollision(RectTransform collidingRectTransform)
    {
        Rect thisRect = rectTransform.toRect();
        Rect collidingRect = collidingRectTransform.toRect();
        float leftOverlap = Mathf.Max(0, collidingRect.xMax - thisRect.xMin);
        float rightOverlap = Mathf.Max(0, thisRect.xMax - collidingRect.xMin);
        leftOverlap = leftOverlap > thisRect.width ? 0 : leftOverlap;
        rightOverlap = rightOverlap > thisRect.width ? 0 : rightOverlap;
        leftOverlap = Mathf.Min(leftOverlap, thisRect.width*0.7f);
        rightOverlap = Mathf.Min(rightOverlap, thisRect.width * 0.7f);
        slidingPart.offsetMax = new Vector2(-rightOverlap/2.0f, 0) ;
        slidingPart.offsetMin = new Vector2(leftOverlap/2.0f, 0);
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
