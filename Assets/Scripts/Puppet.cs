using System;
using UnityEngine;
using System.Linq;
using Assets.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Puppet : MonoBehaviour
{
    public enum State
    {
        Invisible,
        Idle,
        Greeting
    }

    private const float minDistanceToExitGreet = 7f; //Minimum distance to greet when leaving
    private const float minDistanceToEnterGreet = 5f; //Minimum distance to greet when entering
    public static float minDistanceToGreet = minDistanceToEnterGreet; //Minimum distance able to trigger greet 
    public static float turningSpeed = 3.0f; //Determines how fast objects rotate toward each other while greeting
    public Animation Animation;

    private State mState;
    private State state
    {
        get { return mState; }
        set
        {
            if (value == mState) return; //Same state. Nothing to do
            if (value == State.Invisible || value == State.Idle)
            {
                greeter = null;
            }
            else if (value == State.Greeting)
            {
                if (greeter == null) throw new AssertionException("assign greeter before changing state to Greeting", "");
            }
            mState = value;
        }
    }

    private Action updateAction //function which gets called from update depending on state (invisible,idle,greeting)
    {
        get
        {
            switch (state)
            {
                case State.Invisible:
                    return InvisibleUpdate;
                case State.Idle:
                    return IdleUpdate;
                case State.Greeting:
                    return GreetUpdate;
                default:
                    return InvisibleUpdate;
            }
        }
    }

    private Puppet mGreeter;
    private Puppet greeter //Object with whom object greets. Null when no greeting occurs 
    {
        get { return mGreeter; }
        set
        {
            if (value == mGreeter) return;
            mGreeter = value;
            if (mGreeter == null)
            {
                SetGreetingPopUpEnabled(false);
                minDistanceToGreet = minDistanceToEnterGreet;
                state = State.Idle;
                return;
            }
            state = State.Greeting;
            SetGreetingPopUpEnabled(true);
            minDistanceToGreet = minDistanceToExitGreet;
            Animation.Play();
        }
    }

    private void SetGreetingPopUpEnabled(bool show)
    {
        var firstOrDefault = GetComponentsInChildren<Canvas>(true).FirstOrDefault();
        if (firstOrDefault != null) firstOrDefault.gameObject.SetActive(show);
        if (show)
        {
            GetComponentInChildren<Text>().text = "Greetings " + greeter.name;
        }
    }

    private void OnMarkerLost(ARMarker marker)
    {
        //If greeting in progress stop greeting
        state = State.Invisible;
        print(name + "OnMarkerLost");
    }

    private void OnMarkerFound(ARMarker marker)
    {
        //Find closest active Object within specified maximum range. If such found interact with it
        state = State.Idle;
        print(name + "OnMarkerFound");
    }

    private bool IsGreeting()
    {
        return state == State.Greeting;
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
        //transform.localRotation= new Quatransform.localRotation new Vector3(0,1,0);
        CheckIfShouldGreet();
    }

    protected virtual void GreetUpdate()
    {
        Vector3 target = greeter.transform.position - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(target);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turningSpeed);
        CheckIfShouldGreet();
    }

    private void CheckIfShouldGreet()
    {
        greeter = Utils.GetClosestObject(this, minDistanceToGreet); //Check if objects are near each other. this fun returns null if not
    }

    private void PrintDebug()
    {
        string output = state + " | greeter = " + greeter;
        if (IsGreeting())
        {
            output += " | distance to " + greeter.name + " " +" "+
                      Vector3.Distance(greeter.transform.position, transform.position);
        }
        MonoDebugger.Instance.printForKey(name + " state", output);
    }
}
