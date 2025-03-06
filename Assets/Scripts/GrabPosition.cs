using UnityEngine;

public class GrabPosition : MonoBehaviour
{
    [SerializeField]
    float yOffset = 0.8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.parent.transform.parent.transform.position.x, transform.parent.transform.parent.transform.position.y + yOffset);
    }
}
