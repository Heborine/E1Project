using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float effectTiming;
    private float scalePos;
    [SerializeField] float rotationSpeed;
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
            transform.localScale *= (1 - Time.deltaTime/effectTiming);
        }
    }

    public void FlyOutOfWindow() 
    {
        rb.freezeRotation = true;
        rb.angularVelocity = rotationSpeed;
        isDying = false;
        Destroy(gameObject, effectTiming);
    }
}
