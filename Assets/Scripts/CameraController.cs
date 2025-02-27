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
        Vector3 newPos = transform.position;

        // Horizontal follow
        if(player.transform.position.x - transform.position.x > 0.5) newPos.x = player.transform.position.x - 0.7f;
        else if(player.transform.position.x - transform.position.x < -0.3) newPos.x = player.transform.position.x + 0.7f;

        if(player.transform.position.y - transform.position.y > 0.3) newPos.y = player.transform.position.y - 0.4f;
        else if(player.transform.position.y - transform.position.y < -0.3) newPos.y = player.transform.position.y + 0.4f;

        transform.position = newPos;
    }
}
