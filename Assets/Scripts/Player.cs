using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;
using Random=UnityEngine.Random;

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
    public float jumpForce = 100f;

    [Header("Leg Assignments")]
    [SerializeField]
    public GameObject leftLeg;
    [SerializeField]
    public GameObject rightLeg;
    [SerializeField]
    public Rigidbody2D leftLegRb;
    [SerializeField]
    public Rigidbody2D rightLegRb;
    [SerializeField]
    public HingeJoint2D leftLegHinge;
    [SerializeField]
    public HingeJoint2D rightLegHinge;

    [Header("Torque Settings")]
    [SerializeField]
    private float torqueForce = 2f;
    [SerializeField]
    private float maxTorque = 2f;
    [SerializeField]
    private float throwStrength = 0.5f;
    float rightLegTorque = 0f;
    float leftLegTorque = 0f;

    bool isGrounded = true;
    public bool isDead = false;
    public float deathTimer = 0f;
    [SerializeField]
    public float timeOfDeath = 5f;
    bool personGrabbed = false;
    float secondsToGrab = 0.5f, secondsSoFar = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

    void OnGrab(InputValue val)
    {
        if (grabbablePerson != null && personGrabbed == false) {
            Debug.Log("Person grabbed");
            grabbablePerson.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0f, 0f);
            grabbablePerson.GetComponent<Rigidbody2D>().gravityScale = 0;
            grabbablePerson.GetComponent<Collider2D>().enabled = false;
            personGrabbed = true;
        } else if (grabbablePerson != null && personGrabbed == true) {
            Debug.Log("Person thrown");
            Vector2 throwForce;
            float direction = Random.Range(0f, 1f);
            if (direction < 0.5f)
                throwForce = new Vector2(Random.Range(-5f, -3.5f), Mathf.Abs(Random.Range(1.5f, 3.5f))).normalized;
            else
                throwForce = new Vector2(Random.Range(3.5f, 5f), Mathf.Abs(Random.Range(1.5f, 3.5f))).normalized;
            personGrabbed = false;
            grabbablePerson.GetComponent<Collider2D>().enabled = true;
            if (grabbablePerson.GetComponent<Rigidbody2D>().linearVelocity == new Vector2(0f, 0f))
                grabbablePerson.GetComponent<Rigidbody2D>().AddForce(throwStrength * throwForce);
            grabbablePerson.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            grabbablePerson = null;
        }
    }

    void Move(){
        rightLegRb.AddTorque(Mathf.Clamp(rightLegTorque * torqueForce, -maxTorque, maxTorque));
        leftLegRb.AddTorque(Mathf.Clamp(leftLegTorque * torqueForce, -maxTorque, maxTorque));
    }

    void OnJump(){
        if(isGrounded && !isDead)
        {
            float leftLegRotation = leftLegRb.rotation;
            float rightLegRotation = rightLegRb.rotation;
            
            Vector2 leftLegDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * leftLegRotation), Mathf.Sin(Mathf.Deg2Rad * leftLegRotation));
            Vector2 rightLegDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * rightLegRotation), Mathf.Sin(Mathf.Deg2Rad * rightLegRotation));
            Vector2 averageDirection = (leftLegDirection + rightLegDirection).normalized;

            averageDirection.y += 2f;
            averageDirection.Normalize();

            tableTopRb.AddForce(averageDirection * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            leftLeg.SetActive(false);
            rightLeg.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
            // transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3 (0, 20f, 0), Time.deltaTime * 10f);
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0,0), Time.deltaTime * 1.5f);
            // StartCoroutine(RegrowLegs());        
            leftLeg.SetActive(true);
            rightLeg.SetActive(true);
        }
    }

    IEnumerator RegrowLegs()
    {
        Vector3 target = transform.position + new Vector3(0, 0.3f, 0);
        float moveSp = 1.5f;
        float timeElapsed = 0f;
        tableTopRb.gravityScale = 0f;
        while (timeElapsed < 1f)
        {
            transform.position = Vector3.Lerp(transform.position, target, timeElapsed);
            timeElapsed += Time.deltaTime * moveSp;
            yield return null;
        }
        transform.position = target;

        Quaternion targetRot = Quaternion.Euler(0, 0, 0);
        float rotSp = 1.5f;
        timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSp * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;

        // leftLegRb.rotation = 0f;
        // rightLegRb.rotation = 0f;

        leftLeg.SetActive(true);
        rightLeg.SetActive(true);

        leftLegRb.rotation = 0f;
        rightLegRb.rotation = 0f;
        leftLeg.transform.localPosition = new Vector3(0, -0.001f, 0);
        rightLeg.transform.localPosition = new Vector3(0.562f, -0.001f, 0);

        tableTopRb.gravityScale = 1f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Throwable") && !collision.GetComponent<ThrowablePerson>().isThrown()) {
            grabbablePerson = collision.gameObject;
            grabbablePerson.GetComponent<ThrowablePerson>().setThrown(true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            deathTimer += Time.deltaTime;

            if(deathTimer >= timeOfDeath && !isDead)
            {
                isDead = true;
                leftLegHinge.enabled = false;
                rightLegHinge.enabled = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            deathTimer = 0f; 
        }
    }
}
