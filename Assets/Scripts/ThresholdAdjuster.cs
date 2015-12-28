using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ThresholdAdjuster : MonoBehaviour
{
    public Notifier notifierText;
    public struct VisibilityForThreshold
    {
        public int threshold;
        public int visibleMarkers;

        public VisibilityForThreshold(int threshold, int visibleMarkers)
        {
            this.threshold = threshold;
            this.visibleMarkers = visibleMarkers;
        }
    }

    private bool isRunning;
    private const float thresholdTestDuration = 0.02f;
    private ARController arController;
    private ARMarker[] markers;
    private VisibilityForThreshold[] thresholds;

    // Use this for initialization
	void Awake ()
	{
	    isRunning = false;
	    arController = gameObject.GetComponent<ARController>();
        arController.VideoThresholdMode = ARController.ARToolKitThresholdMode.Manual;
	    markers = gameObject.GetComponents<ARMarker>();
        thresholds = new VisibilityForThreshold[256];
	}

    public void Adjust()
    {
        if(isRunning) return;
        isRunning = true;
        if (notifierText != null) notifierText.NotifyThresholdAdjusterStarted();
        for (int i = 0; i <= 255; i++)
        {
            StartCoroutine(SetThreshold(thresholdTestDuration * i, i));
            StartCoroutine(CheckVisibility(thresholdTestDuration * (i + 1), i));
        }
    }

    private void SetBestThreshold()
    {
        int maxFound = thresholds.Max(threshold => threshold.visibleMarkers);
        int bestStart=0;
        int bestEnd=0;
        int currentStart=0;
        bool newSectionFound = false;
        for (int i = 0; i < thresholds.Length; i++)
        {
            VisibilityForThreshold threshold = thresholds[i];
            if (threshold.visibleMarkers != maxFound && newSectionFound)
            {
                int currentEnd = i - 1;
                if (Length(currentStart, currentEnd) > Length(bestStart, bestEnd))
                {
                    bestStart = currentStart;
                    bestEnd = currentEnd;
                }
                newSectionFound = false;
            }
            else if (threshold.visibleMarkers == maxFound && newSectionFound==false)
            {
                currentStart = i;
                newSectionFound = true;
            }
           
        }
        arController.VideoThreshold = (int) Mathf.Lerp(bestStart, bestEnd, 0.5f);
        isRunning = false;
        if (notifierText != null) notifierText.NotifyThresholdAdjusterFinished(arController.VideoThreshold);
    }

    private int Length(int start,int end)
    {
        return end - start;
    }

    IEnumerator CheckVisibility(float time, int threshold)
    {
        yield return new WaitForSeconds(time);
        int totalVisible = markers.Count(marker => marker.Visible);
        thresholds[threshold] = new VisibilityForThreshold(threshold, totalVisible);
        if (threshold == 255)
        {
            SetBestThreshold();
        }
    }

    IEnumerator SetThreshold(float time,int threshold)
    {
        yield return new WaitForSeconds(time);
        if (notifierText != null) notifierText.NotifyThresholdAdjusterUpdate(threshold);
        arController.VideoThreshold = threshold;
    }

}
