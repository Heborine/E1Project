using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float effectTiming;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    public void FlyOutOfWindow() 
    {
    }
}
