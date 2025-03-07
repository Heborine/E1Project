using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public class BaseNPC : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float health;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    [SerializeField] Transform[] patrolPositions;
    [SerializeField] int nextPatrolPos = 0;
    [SerializeField] float posDetectRad;
    int incrementPos;

    [SerializeField] Player p;
    [SerializeField] float distShouldRun;
    [Tooltip("note - this is just for debugging you don't need to edit it")]
    [SerializeField] float distToPlayer;

    [SerializeField] GameObject alertIcon;
    [SerializeField] bool shouldIdle;
    [SerializeField] float timeToIdle;

    private bool thrown = false;

    private void Start()
    {
        alertIcon.SetActive(false);
    }

    private void FixedUpdate()
    {
        float distToPlayer = Vector2.Distance(transform.position, p.transform.position);
        if (distToPlayer < distShouldRun && p.GetComponent<Rigidbody2D>().linearVelocity != Vector2.zero)
        {
            RunAway();
            alertIcon.SetActive(true);
        }
        else if(shouldIdle)
        {
            Idle();
        }
        else
        {
            alertIcon.SetActive(false);
            Patrol();
        }
    }

    public bool IsThrown() { return thrown; }

    public void SetThrown(bool b) { thrown = b; }

    void Patrol() 
    {
        rb.linearVelocity = (Vector2)(patrolPositions[nextPatrolPos].position - transform.position).normalized * walkSpeed;
        if (Vector2.Distance(patrolPositions[nextPatrolPos].position, transform.position) < posDetectRad) 
        {
            if (nextPatrolPos == patrolPositions.Length -1 || nextPatrolPos == 0) 
            {
                incrementPos *= -1;
            }
            nextPatrolPos += incrementPos;
            IdleTimer(timeToIdle);
        }
    }

    void Idle() 
    {
        rb.linearVelocity = Vector2.zero;
    }

    void RunAway() 
    {
        rb.linearVelocity = -(Vector2)(p.transform.position - transform.position).normalized * runSpeed;
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
}
