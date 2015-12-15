using System;
using UnityEngine;
using System.Linq;
using Assets.Utils;
using UnityEngine.UI;

public class Puppet : MonoBehaviour
{
    private const float minDistanceToExitGreet = 7f; //Minimum distance to greet when leaving
    private const float minDistanceToEnterGreet = 5f; //Minimum distance to greet when entering
    public static float minDistanceToGreet = minDistanceToEnterGreet; //Minimum distance able to trigger greet 
    public static float turningSpeed = 3.0f; //Determines how fast objects rotate toward each other while greeting
    public Animation Animation;
    private Puppet mGreeter;
    private Action updateAction;
    private Quaternion defaultLocalRotation;

    public void Awake()
    {
        updateAction = InvisibleUpdate;
        defaultLocalRotation = transform.localRotation;
    } 


    
    public void SetInvisible()
    {
        SetIdle();
        updateAction = InvisibleUpdate;
    }

    public void SetIdle()
    {
        mGreeter = null;
        SetGreetingPopUpEnabled(false);
        minDistanceToGreet = minDistanceToEnterGreet;
        updateAction = IdleUpdate;
    }

    public void SetGreeting(Puppet greeter)
    {
        if(greeter == null)  throw new ArgumentException("greeter cannot be null");
        if (greeter == mGreeter) return;
        mGreeter = greeter;
        SetGreetingPopUpEnabled(true);
        minDistanceToGreet = minDistanceToExitGreet;
        Animation.Play();
        updateAction = GreetUpdate;
    }

    private void SetGreetingPopUpEnabled(bool show)
    {
        var firstOrDefault = GetComponentsInChildren<Canvas>(true).FirstOrDefault();
        if (firstOrDefault != null) firstOrDefault.gameObject.SetActive(show);
        if (show)
        {
            GetComponentInChildren<Text>().text = "Greetings " + mGreeter.name;
        }
    }

    private void OnMarkerLost(ARMarker marker)
    {
        SetInvisible();
        print(name + "OnMarkerLost");
    }

    private void OnMarkerFound(ARMarker marker)
    {
        SetIdle();
        print(name + "OnMarkerFound");
    }

    private bool IsGreeting()
    {
        return mGreeter != null;
    }


    // Update is called once per frame
    private void LateUpdate()
    {
        updateAction();
        PrintDebug();
    }

    public void OnDisable()
    {
        PrintDebug();
    }

    //Called once per frame if object is invisible
    protected virtual void InvisibleUpdate()
    {

    }

    //Called once per frame if object is visible but not greeting
    protected virtual void IdleUpdate()
    {
        CheckIfShouldGreet();
        transform.localRotation = Quaternion.Slerp(transform.localRotation, defaultLocalRotation, Time.deltaTime*turningSpeed);
    }

    protected virtual void GreetUpdate()
    {
        Vector3 target = mGreeter.transform.position - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(target);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turningSpeed);
        CheckIfShouldGreet();
    }

    private void CheckIfShouldGreet()
    {
        Puppet candidate = Utils.GetClosestObject(this, minDistanceToGreet); //Check if objects are near each other. this fun returns null if not
        if(candidate != null) SetGreeting(candidate);
        else SetIdle();
    }

    private void PrintDebug()
    {
        string output = "state = ";//+ updateAction.Method.Name;
        if (IsGreeting())
        {
            output += " | distance to " + mGreeter.name + " " + " " +
                      Vector3.Distance(mGreeter.transform.position, transform.position);
        }
        MonoDebugger.Instance.printForKey(name + " state", output);
    }
}
