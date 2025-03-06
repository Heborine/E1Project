using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float effectTiming;
    private float scalePos;
    [SerializeField] float rotationSpeed;
    [SerializeField] float shrinkFactor = 0.9f;
    bool isDying = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate() 
    {
        if (isDying) 
        {
            transform.localScale *= shrinkFactor * (1 - Time.deltaTime/effectTiming);
        }
    }

    public void FlyOutOfWindow() 
    {
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        rb.angularVelocity = rotationSpeed;
        isDying = true;
        Destroy(gameObject, effectTiming);
    }
}
