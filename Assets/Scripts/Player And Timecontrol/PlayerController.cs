using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed for press R to reset level. Just need for testing!


public class PlayerController : MonoBehaviour {

    //Serializing the field allows you to keep the variable private, whilst being able to access it within the unity editor like a public variable.

    //Initialise Serialized variables
    [SerializeField] private float movementSpeed; //Speed that player moves, edited through Unity Editor
    [SerializeField] private AnimationCurve jumpFallOff; //this is a animation curve, essentially a graph that dictates how much force is being put on the players jump at any time during the jump.
    [SerializeField] private float jumpMultiplier; //How much should the player jump?
    [SerializeField] private KeyCode jumpKey; //What key is jump? It's space!
    [SerializeField] private float slopeForce; //adds extra force when going down slopes to remove bouncing that comes with unity. (Unity can be buggy!)
    [SerializeField] private float slopeForceRayLength; //fires a ray out of your butt to see if there is a slope below you.
    [SerializeField] private KeyCode slowTimeKey;
    [SerializeField] GameObject Interactkey;

    //Initialise private variables
    private bool slowUsed; // Our current focus: Item, Enemy etc.
    private bool isJumping; //test to see if we're allready mid jump.
    private CharacterController controller; // holds the player controller, letting unity know that the controller is the player

    //Initialise public static variables. These are public static variables meaning they can be accessed by all other scripts.
    public static bool slowTime; //how mana the player has to slow time. 
    public static float interactMinDistance = 3f; // minimum distance needed to begin interacting with an interactable.
    public Interactable focus; // Our current focus: Item, Enemy etc.

    //Initialise public variables.
    public delegate void OnFocusChanged(Interactable newFocus);
    public OnFocusChanged onFocusChangedCallback;
    public float manaPercent; //how mana the player has to slow time. 

    //used to hold the current interactable object the player has currently pressed F on.
    Camera cam; // Reference to our camera

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        manaPercent = 100f;
        //let unity know that the controller is the player

        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Runs the private void for player movement
        PlayerMovement();
        Interact();
        ManageMana();


        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync("DevTestingZone");
        }
    }

    private void PlayerMovement()
    {
        // gets the movement input from the player using unity inputs.
        float horizontalInput = Input.GetAxis("Horizontal") * movementSpeed;
        float verticalInput = Input.GetAxis("Vertical") * movementSpeed;

        //move forward.
        Vector3 forwardMovement = transform.forward * verticalInput;
        //move left and right.
        Vector3 rightMovement = transform.right * horizontalInput;

        controller.SimpleMove(forwardMovement + rightMovement);



        ///this helps smooth out going down slopes!
        // This does this by adding an extra element of gravity when the player is on slopes. This extra amount of gravity is the slopeForce, which I have set to 3.
        // If this is set to anything less than 3 the player bounces when going down the slopes as they fall slower than the gravity forcing them down, and I didn't
        // want very high gravity in my game as it takes place in space.
        if ((verticalInput != 0 || horizontalInput != 0) && OnSlope())
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime / Time.timeScale);

        //runs to check if player jumps
        JumpInput();

    }

    private void ManageMana()
    {
        if ((Input.GetKey(slowTimeKey) && !(manaPercent < 0f))) //If the player has atleast SOME mana, and is holding the slow time key
        {
            if (manaPercent == 100f) // then if the player has 100% mana 
            {
                slowUsed = true; // Start slowing time.
            }
        }
        else if (manaPercent < 75f) //only stop mana drain once at least 25% has been used. 
        {
            slowUsed = false; //If the player lets go of the slowtime key, or runs out of mana, stop slowing time.

        }

        if (slowUsed == true)
        {
            slowTime = true;
            manaPercent -= 10f * Time.deltaTime; //Drain mana

        }
        else
        {
            slowTime = false;

            if (manaPercent > 100f)
            {
                manaPercent = 100f;

            }
            else if (manaPercent != 100f)
            {
                manaPercent += 10f * Time.deltaTime;//regen mana
            }
        }
    }

    private void Interact()
    {
        // If we press the F key

            // Shoot out a ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            // If we hit
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            SetFocus(interactable);
            

            if (interactable != null)
            {
                Interactkey.SetActive(true);
                float distance = Vector3.Distance(transform.position, focus.transform.position);

                if (!(distance < interactMinDistance))
                {
                    Interactkey.SetActive(false);
                    SetFocus(null);
                }
                else 
                {

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        SetFocus(null);
                        interactable.Interact();
                        Debug.Log(distance);
                    }
                }
            
            }
            else
            {
                Interactkey.SetActive(false);
        
                SetFocus(null);
            }
        }
    }

    void SetFocus(Interactable newFocus)
    {
        // Set our focus to what we hit
        // If it's not an interactable, simply set it to null
        focus = newFocus;
    }

    private bool OnSlope()
    {
        //First check if the player is jumping, if they are, dont bother checking if they're on a slope!
        if (isJumping)
            return false;

        
        ///Now we're going to fire a ray out of the player character directly downwards from the players current posistion. 
        //This line will store the information of the object that is currently being hit by the ray.
        RaycastHit hit;

        //this creates the ray, which will return true if it hits something that is sloped.
        //If this returns true we know we are on a slope!
        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up) // if the surface we just hit, doesnt return a ray cast of (0,1,0) then we have hit a slope. Because a slope would
                return true;              // return a angled ray cast back, like (0,0.4,0.9). Saying Vector3.up is the same as saying (0,1,0) because it is simply 1 up.


        //if it doesnt hit anything then return false!
        return false;
    }
   
    private void JumpInput()
    {
        //Check if the player presses space, and isnt allready jumping
        if(Input.GetKeyDown(jumpKey) && !isJumping)
        {
            //You are now jumping!
            isJumping = true;
            //Start the jump routine!
            StartCoroutine(JumpEvent());
        }
    }



    private IEnumerator JumpEvent() //This is the Jump Routine!
    {
        controller.slopeLimit = 90.0f; //this makes it smoother for jumping up ledges, otherwise you'll get odd clipping issues.

        float timeInAir = 0.0f; //variable to store how long the player has been in the air for

        do //start loop
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir); //uses a graph called jumpFallOff, which can be adjusted to simulate airtime and velocity in air. We pass the amount of time that has passed in for it to be used against the graph.
            controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime; //this just gives a tiny bit more airtime to smooth it all out
            yield return null; //end the jump routine
        } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above); //end the loop when the player is back on the ground and their head isn't touching a ceiling (Otherwise you get stuck!)

        //dont need to smooth out going up edges anymore cause your not jumping.
        controller.slopeLimit = 45.0f;
        //You're not jumping anymore because we're outside the loop now!
        isJumping = false;
    }
}
