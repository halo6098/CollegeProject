using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    //Initialise Variables.
    bool first = true;
    public Rigidbody theRB;
    bool rampBack = true;

    //Initialise the _timeAdjuster variable to 1 so that it can be accessed and edited by the getter and setter.
    private float _timeAdjuster = 1;
    
    private float timeSlowAmount = 0.2f; //Dont want this to be serilized or you'll have to slow time manually on everything that the script is applied to.
    //I found that 0.1f is the best for slowing time.

    [SerializeField] private AnimationCurve slowTimeFalloff;

    [SerializeField] private float timeRampTimer = 0.0f;

    public float timeAdjuster
    {
        get { return _timeAdjuster; } //this is a nifty function known as the "get set" function. This allows 
        set
        {
            
            if (!first)
            {
                theRB.mass *= timeAdjuster;
                theRB.velocity /= timeAdjuster;
                theRB.angularVelocity /= timeAdjuster;
            }
            first = false;

            _timeAdjuster = value; //The value is the value passed into the function from _TimeAdjuster.

            theRB.mass /= timeAdjuster;
            theRB.velocity *= timeAdjuster;
            theRB.angularVelocity *= timeAdjuster;
        }
    }

    void Awake() //Awake is different to start. It is called AFTER the game has finished initialising, whilst start is called DURING.
    {
        theRB = GetComponent<Rigidbody>();
        timeAdjuster = _timeAdjuster;
    }



    /// <summary>
    /// Update runs once per frame. FixedUpdate can run once, zero, or several times per frame, depending on how many 
    /// physics frames per second are set in the time settings, and how fast/slow the framerate is.
    /// </summary>
    void Update()
    {
        // If player is holding the left mouse button, pass the adjusted time amount into the GetSet function.
        if ((PlayerController.slowTime))
        {
            timeAdjuster = timeSlowAmount;
            rampBack = true;
            timeRampTimer = 0;
        }
        else 
        {
            if (rampBack)
            {
                timeRampTimer += Time.deltaTime;
                if (timeRampTimer > 1)
                {
                    timeRampTimer = 1;
                    rampBack = false;
                }

                timeAdjuster = timeRampTimer;
            }
            
        }
    }

    private void FixedUpdate()
    {
        theRB.AddForce(Physics.gravity * timeAdjuster); // need to apply the time adjuster to the gravity also, but cant do it to physics.gravity as that effects player too!

        float delta = Time.fixedDeltaTime * timeAdjuster; //fixed delta time is the speed of the game. multiplied by our time adjuster, which in this case is 0.1.
        theRB.velocity += Physics.gravity / theRB.mass * delta;
    }
}
