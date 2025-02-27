using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Table Settings")]
    [SerializeField]
    public Rigidbody2D tableTopRb;
    [SerializeField]
    public float jumpForce = 10f;
    [Header("Leg Assignments")]
    [SerializeField]
    public Rigidbody2D leftLegRb;
    [SerializeField]
    public Rigidbody2D rightLegRb;
    [Header("Torque Settings")]
    [SerializeField]
    private float torqueForce = 2f;
    [SerializeField]
    private float maxTorque = 2f;
    float rightLegTorque = 0f;
    float leftLegTorque = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // tableTopRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void OnRightLegMove(InputValue val)
    {
        rightLegTorque = val.Get<float>();
    }
    void OnLeftLegMove(InputValue val)
    {
        leftLegTorque = val.Get<float>();
    }

    void Move(){
        rightLegRb.AddTorque(Mathf.Clamp(rightLegTorque * torqueForce, -maxTorque, maxTorque));
        leftLegRb.AddTorque(Mathf.Clamp(leftLegTorque * torqueForce, -maxTorque, maxTorque));
    }

    void OnJump(){
        // float leftLegRotation = Mathf.Clamp(Mathf.Abs(leftLegRb.rotation), 0f, 90f);
        // float rightLegRotation = Mathf.Clamp(Mathf.Abs(rightLegRb.rotation), 0f, 90f);
        float leftLegRotation = leftLegRb.rotation;
        float rightLegRotation = rightLegRb.rotation;
        
        Vector2 leftLegDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * leftLegRotation), Mathf.Sin(Mathf.Deg2Rad * leftLegRotation));
        Vector2 rightLegDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * rightLegRotation), Mathf.Sin(Mathf.Deg2Rad * rightLegRotation));
        Vector2 averageDirection = (leftLegDirection + rightLegDirection).normalized;

        averageDirection.y += 2f;
        averageDirection.Normalize();

        tableTopRb.AddForce(averageDirection * jumpForce, ForceMode2D.Impulse);
    }
}
