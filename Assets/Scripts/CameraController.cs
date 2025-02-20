using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x - transform.position.x > 0.3) transform.position = new Vector2(player.transform.position.x - 0.3f, transform.position.y);
        else if(player.transform.position.x - transform.position.x < -0.3) transform.position = new Vector2(player.transform.position.x + 0.3f, transform.position.y);
    }
}
