using UnityEngine;
using Holoville.HOTween;
using UnityEngine.UI;

public class Notifier : MonoBehaviour
{

    private Text text;
    private CanvasGroup canvasGroup;
    private Slider slider;
    private float fadeDuration = 0.5f;
    void Awake()
    {
        text = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        slider = GetComponentInChildren<Slider>();
    }

    public void NotifyThresholdAdjusterUpdate(int currentlyTestedThreshold)
    {
        slider.normalizedValue = currentlyTestedThreshold/255.0f;
        string str = "Poszukiwanie najlepszego progu : (" + currentlyTestedThreshold + "/255)";
        text.text = str;
    }

    public void NotifyThresholdAdjusterFinished(int thresholdSet)
    {
        slider.normalizedValue = 1;
        string str = "Znaleziono najlepszy próg : <color=green>" + thresholdSet+"</color>";
        text.text = str;
        HOTween.Complete(canvasGroup);
        HOTween.To(canvasGroup, fadeDuration, "alpha", 0.0f,false,EaseType.Linear, 3).Play();
    }

    public void NotifyThresholdAdjusterStarted()
    {
        slider.normalizedValue = 0;
        HOTween.Complete(canvasGroup);
        HOTween.To(canvasGroup,fadeDuration,"alpha",1.0f).Play();
    }


	// Update is called once per frame
	void Update () {
	
	}
}
