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
}
