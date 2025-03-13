using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;
using Random=UnityEngine.Random;
using TMPro;

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

    [SerializeField]
    public GameObject GameOver;

    [SerializeField]
    public TMP_Text timeText;

    // [SerializeField]
    public bool isHittingGround;

    [SerializeField]
    public GameObject deathRegion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            isHittingGround = deathRegion.GetComponent<Death>().onGroundDying;

            Vector3 deathRegionOffset = new Vector3(0f, 0.475f, 0);
            // Preserve the relative position by roating the offset by the player's rotation
            Vector3 rotatedOffset = transform.rotation * deathRegionOffset;
            deathRegion.transform.position = transform.position + rotatedOffset;
            deathRegion.transform.rotation = transform.rotation;

            Move();
            GetComponent<Rigidbody2D>().position = new Vector2(tableTopRb.transform.position.x, tableTopRb.transform.position.y);

            if(isHittingGround)
            {
                deathTimer += Time.deltaTime;

                if(deathTimer >= timeOfDeath && !isDead)
                {
                    isDead = true;
                    leftLegHinge.enabled = false;
                    rightLegHinge.enabled = false;
                }
            }
            else
            {
                deathTimer = 0f; 
            }

            DisplayTime();
        }
        else
        {
            GameOver.SetActive(true);
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
            grabbablePerson.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0f, 0f);
            grabbablePerson.GetComponent<Rigidbody2D>().gravityScale = 0;
            grabbablePerson.GetComponent<Collider2D>().enabled = false;
            personGrabbed = true;
        } else if (grabbablePerson != null && personGrabbed == true) {
            if (grabbablePerson.CompareTag("NPC")) grabbablePerson.GetComponent<BaseNPC>().SetThrown(true);

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwForce = (mousePosition - (Vector2)grabbablePerson.transform.position).normalized;
            Debug.Log(mousePosition.x + ", " + grabbablePerson.transform.position.x);
            Debug.Log(mousePosition.y + ", " + grabbablePerson.transform.position.y);
            Debug.Log(throwForce);

            float throwTorque;
            float torqueDir = Random.Range(0f, 1f);
            if (torqueDir < 0.5f)
                throwTorque = Random.Range(-100f, -75f);
            else
                throwTorque = Random.Range(75f, 100f);
            personGrabbed = false;
            grabbablePerson.GetComponent<Collider2D>().enabled = true;
            grabbablePerson.GetComponent<Rigidbody2D>().AddForce(throwStrength * throwForce);
            grabbablePerson.GetComponent<Rigidbody2D>().AddTorque(throwTorque);
            grabbablePerson.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            grabbablePerson = null;
            secondsSoFar = 0f;
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
        Debug.Log("Collision detected");
        Debug.Log(grabbablePerson == null);
        Debug.Log(throwable(collision));
        // Debug.Log(!collision.GetComponent<ThrowablePerson>().isGrabbed());
        if(grabbablePerson == null && throwable(collision) && !collision.GetComponent<ThrowablePerson>().isGrabbed()) {
            Debug.Log("Found person");
            grabbablePerson = collision.gameObject;
            grabbablePerson.GetComponent<ThrowablePerson>().setGrabbed(true);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(grabbablePerson == null && throwable(collision) && !collision.GetComponent<ThrowablePerson>().isGrabbed()) {
            grabbablePerson = collision.gameObject;
            grabbablePerson.GetComponent<ThrowablePerson>().setGrabbed(true);
        }

        // if(collision.CompareTag("Ground"))
        // {
        //     deathTimer += Time.deltaTime;

        //     if(deathTimer >= timeOfDeath && !isDead)
        //     {
        //         isDead = true;
        //         leftLegHinge.enabled = false;
        //         rightLegHinge.enabled = false;
        //     }
        // }
    }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if(other.CompareTag("Ground"))
    //     {
    //         deathTimer = 0f; 
    //     }
    // }

    bool throwable(Collider2D collision) {
        if (collision.CompareTag("Throwable") || collision.CompareTag("NPC")) return true;
        return false;
    }

    void DisplayTime()
    {
        int timeDisplay = (int)(5 - deathTimer);
        timeText.text = "Death Timer: " + timeDisplay.ToString();
    }
}
