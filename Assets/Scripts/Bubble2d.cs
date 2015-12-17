using UnityEngine;
using System.Linq;
using Assets.Scripts;
using Holoville.HOTween;

public class Bubble2d : MonoBehaviour
{

    public GameObject trackedObject;
    public Camera camera;
    private CanvasGroup canvasGroup;
    private const float animDuration = 0.5f;
    private Vector3 defaultScale;
    private RectTransform rectTransform;
    public float defaultWidth;
    public bool overlapping;
    // Use this for initialization
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        defaultWidth = rectTransform.offsetMax.x + rectTransform.offsetMin.x;
        HOTween.defEaseType = EaseType.EaseInOutExpo;
        defaultScale = canvasGroup.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = camera.WorldToScreenPoint(trackedObject.transform.position);
        checkOverlapping();
    }

    void checkOverlapping()
    {
        overlapping = false;
        FindObjectsOfType<Bubble2d>()
            .ToList()
            .Where(bubble => bubble != this)
            .Where(bubble => bubble.GetComponent<CanvasGroup>().alpha > 0)
            .Select(bubble => bubble.GetComponent<RectTransform>())
            .Where(otherRect => otherRect.Overlaps(rectTransform))
            .ToList().ForEach(SolveCollision);
    }

    public float leftOverlap;
    public float rightOverlap;
    public Rect collidingRect;
    public Rect thisRect;
    public Vector3 offest;
    public Vector3 lerpedOffset;

    private void SolveCollision(RectTransform collidingRectTransform)
    {
        overlapping = true;
        thisRect = new Rect(rectTransform.position,rectTransform.rect.size);
        collidingRect = new Rect(collidingRectTransform.position, collidingRectTransform.rect.size);
        leftOverlap = Mathf.Max(0, collidingRect.xMax - thisRect.xMin);
        rightOverlap = Mathf.Max(0, thisRect.xMax - collidingRect.xMin);
        leftOverlap = leftOverlap > thisRect.width/2.0 ? 0 : leftOverlap;
        rightOverlap = rightOverlap > thisRect.width/2.0 ? 0 : rightOverlap;
        offest = new Vector3(leftOverlap - rightOverlap, 0, 0);
        lerpedOffset = Vector3.Lerp(new Vector3(), offest, Time.deltaTime);
        rectTransform.position += offest;
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
