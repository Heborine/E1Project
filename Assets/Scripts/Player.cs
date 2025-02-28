using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Table Settings")]
    [SerializeField]
    public GameObject grabPosition;
    [SerializeField]
    public GameObject grabbablePerson;
    [SerializeField]
    public Rigidbody2D tableTopRb;
    [SerializeField]
    public float jumpForce = 10f;
    [Header("Leg Assignments")]
    [SerializeField]
    public GameObject leftLeg;
    [SerializeField]
    public GameObject rightLeg;
    [SerializeField]
    public Rigidbody2D leftLegRb;
    [SerializeField]
    public Rigidbody2D rightLegRb;
    [Header("Torque Settings")]
    [SerializeField]
    private float torqueForce = 2f;
    [SerializeField]
    private float maxTorque = 2f;
    float rightLegTorque = 0f;
    float leftLegTorque = 0f;
    // [SerializeField]
    // private HingeJoint2D leftHinge;
    // [SerializeField]
    // private HingeJoint2D rightHinge;

    // bool isLeftLegGrounded = false;
    // bool isRightLegGrounded = false;
    bool isGrounded = false;
    public bool isDead = false;
    public float deathTimer = 0f;
    [SerializeField]
    public float timeOfDeath = 5f;
    bool personGrabbed = false;
    float secondsToGrab = 0.5f, secondsSoFar = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // tableTopRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            Move();
            GetComponent<Rigidbody2D>().position = new Vector2(tableTopRb.transform.position.x, tableTopRb.transform.position.y);
        }
    }

    void FixedUpdate()
    {
        if (personGrabbed && (Mathf.Abs(grabPosition.transform.position.x - grabbablePerson.transform.position.x) >= 0.05f || Mathf.Abs(grabPosition.transform.position.y - grabbablePerson.transform.position.y) >= 0.05f)) {
            secondsSoFar += Time.deltaTime;
            float t = secondsSoFar / secondsToGrab;
            grabbablePerson.transform.position = Vector2.Lerp(grabbablePerson.transform.position, new Vector2(grabPosition.transform.position.x, grabPosition.transform.position.y), t);
        }
    }

    void OnRightLegMove(InputValue val)
    {
        rightLegTorque = val.Get<float>();
    }
    void OnLeftLegMove(InputValue val)
    {
        leftLegTorque = val.Get<float>();
    }

    void OnClick(InputValue val)
    {
        if(grabbablePerson != null) {
            grabbablePerson.GetComponent<Rigidbody2D>().gravityScale = 0;
            // grabbablePerson.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            grabbablePerson.GetComponent<Collider2D>().enabled = false;
            personGrabbed = true;
        }
    }

    void Move(){
        rightLegRb.AddTorque(Mathf.Clamp(rightLegTorque * torqueForce, -maxTorque, maxTorque));
        leftLegRb.AddTorque(Mathf.Clamp(leftLegTorque * torqueForce, -maxTorque, maxTorque));
    }

    void OnJump(){
        if(isGrounded && !isDead)
        // if (isLeftLegGrounded || isRightLegGrounded)
        {
            // float leftLegRotation = Mathf.Clamp(Mathf.Abs(leftLegRb.rotation), 0f, 90f);
            // float rightLegRotation = Mathf.Clamp(Mathf.Abs(rightLegRb.rotation), 0f, 90f);
            float leftLegRotation = leftLegRb.rotation;
            float rightLegRotation = rightLegRb.rotation;
            
            Vector2 leftLegDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * leftLegRotation), Mathf.Sin(Mathf.Deg2Rad * leftLegRotation));
            Vector2 rightLegDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * rightLegRotation), Mathf.Sin(Mathf.Deg2Rad * rightLegRotation));
            Vector2 averageDirection = (leftLegDirection + rightLegDirection).normalized;

            averageDirection.y += 2f;
            averageDirection.Normalize();

            tableTopRb.AddForce(averageDirection * jumpForce, ForceMode2D.Impulse);
            // isLeftLegGrounded = false;
            // isRightLegGrounded = false;
            isGrounded = false;
            // rightHinge.enabled(false);
            leftLeg.SetActive(false);
            rightLeg.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("landed");
            isGrounded = true;
            leftLeg.SetActive(true);
            rightLeg.SetActive(true);
            // isLeftLegGrounded = true;
            // if (collision.collider == leftLegRb.GetComponent<Collider2D>())
            // {
            //     isLeftLegGrounded = true;
            // }

            // if (collision.collider == rightLegRb.GetComponent<Collider2D>())
            // {
            //     isRightLegGrounded = true;
            // }
        }
    }

    // void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         Debug.Log("lift off");
    //         isLeftLegGrounded = false;
    //         // if (collision.collider == leftLegRb.GetComponent<Collider>())
    //         // {
    //         //     isLeftLegGrounded = false;
    //         // }

    //         // if (collision.collider == rightLegRb.GetComponent<Collider>())
    //         // {
    //         //     isRightLegGrounded = false;
    //         // }
    //     }
    // }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnTrigger");
        // Debug.Log(this.CompareTag("DeathRegion"));
        // Debug.Log(other.CompareTag("Ground"));
        if(other.CompareTag("Ground"))
        // if(this.CompareTag("DeathRegion") && other.CompareTag("Ground"))
        {
            deathTimer += Time.deltaTime;

            if(deathTimer >= timeOfDeath && !isDead)
            {
                isDead = true;
                Debug.Log("Player is dead!");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        // if(this.CompareTag("DeathRegion") && other.CompareTag("Ground"))
        {
            deathTimer = 0f; 
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if(collision.CompareTag("Throwable")) {
            grabbablePerson = collision.gameObject;
            Debug.Log("Found throwable person: " + grabbablePerson);
        }
    }
}
