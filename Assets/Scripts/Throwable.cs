using UnityEngine;

public class ThrowablePerson : MonoBehaviour
{
    bool grabbed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isGrabbed() {
        return grabbed;
    }

    public void setGrabbed(bool b) {
        grabbed = b;
    }
}
