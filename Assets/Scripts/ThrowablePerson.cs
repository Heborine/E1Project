using UnityEngine;

public class ThrowablePerson : MonoBehaviour
{
    bool thrown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isThrown() {
        return thrown;
    }

    public void setThrown(bool b) {
        thrown = b;
    }
}
