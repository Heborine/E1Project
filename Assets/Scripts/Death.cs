using UnityEngine;

public class Death : MonoBehaviour
{
    public bool onGroundDying = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            onGroundDying = true;
            Debug.Log("Currently colliding with: " + collision.gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            onGroundDying = false;
        }
    }

}
