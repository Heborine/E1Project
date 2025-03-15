using System.Collections;
using Unity.VisualScripting;
// using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Jobs;

public class BaseNPC : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float health;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    [SerializeField] Transform[] patrolPositions;
    [SerializeField] int nextPatrolPos = 0;
    [SerializeField] float posDetectRad;
    int incrementPos = 1;

    [SerializeField] Player p;
    [SerializeField] float distShouldRun;
    [Tooltip("note - this is just for debugging you don't need to edit it")]
    [SerializeField] float distToPlayer;

    [SerializeField] GameObject alertIcon;
    [SerializeField] bool shouldIdle;
    [SerializeField] float timeToIdle;

    private bool thrown = false;
    float dir = 1;

    Animator npcAnim;
    [SerializeField] string npcWalkAnimName = "isWalking";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        alertIcon.SetActive(false);
        npcAnim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (thrown) 
        {
            alertIcon.SetActive(true);
            StartCoroutine(getThrown()); 
            return; 
        }
        else {
            doAI();
        }

    }

    void doAI() 
    {
        npcAnim.SetBool(npcWalkAnimName, rb.linearVelocityX != 0);

        dir = transform.localScale.x;
        flip();

        float distToPlayer = Vector2.Distance(transform.position, p.transform.position);
        if (distToPlayer < distShouldRun && p.GetComponent<Rigidbody2D>().linearVelocity != Vector2.zero)
        {
            RunAway();
            alertIcon.SetActive(true);
        }
        else if (shouldIdle)
        {
            Idle();
        }
        else
        {
            alertIcon.SetActive(false);
            Patrol();
        }
    }

    IEnumerator getThrown() 
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 throwForce = (mousePosition - (Vector2)transform.position).normalized;
        Debug.Log(mousePosition.x + ", " + transform.position.x);
        Debug.Log(mousePosition.y + ", " + transform.position.y);
        Debug.Log(throwForce);

        float throwTorque;
        float torqueDir = Random.Range(0f, 1f);
        if (torqueDir < 0.5f)
            throwTorque = Random.Range(-100f, -75f);
        else
            throwTorque = Random.Range(75f, 100f);
        GetComponent<ThrowablePerson>().setGrabbed(false);
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().AddForce(50f * throwForce);
        GetComponent<Rigidbody2D>().AddTorque(throwTorque);
        GetComponent<Rigidbody2D>().gravityScale = 0.5f;

        yield return new WaitForSeconds(4f);
        thrown = false;
        alertIcon.SetActive(false);
    }

    public bool IsThrown() { return thrown; }

    public void SetThrown(bool b) { thrown = b; }

    void Patrol() 
    {
        rb.linearVelocityX = (walkSpeed * Mathf.Sign(patrolPositions[nextPatrolPos].position.x - transform.position.x));
        //rb.linearVelocity = (Vector2)(patrolPositions[nextPatrolPos].position - transform.position).normalized * walkSpeed;
        if (Mathf.Abs(patrolPositions[nextPatrolPos].position.x - transform.position.x) < posDetectRad) 
        {
            if (nextPatrolPos == patrolPositions.Length -1 || nextPatrolPos == 0) 
            {
                incrementPos *= -1;
            }
            nextPatrolPos += incrementPos;
            //IdleTimer(timeToIdle);
        }
    }

    void Idle() 
    {
        rb.linearVelocity = Vector2.zero;
    }

    void RunAway() 
    {
        //rb.linearVelocity = -(Vector2)(p.transform.position - transform.position).normalized * runSpeed;
        rb.linearVelocityX = -runSpeed * Mathf.Sign(p.transform.position.x - transform.position.x);
    }

    IEnumerator IdleTimer(float timing)
    {
        shouldIdle = true;
        yield return new WaitForSeconds(timing);
        shouldIdle = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Window") && IsThrown()) {
            this.GetComponent<DeathEffect>().FlyOutOfWindow();
        }
    }

    void flip() 
    {
        if(Mathf.Sign(rb.linearVelocityX) != dir) transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}
