using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
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
        float clampedRightLegTorque = Mathf.Clamp(rightLegTorque * torqueForce, -maxTorque, maxTorque);
        float clampedLeftLegTorque = Mathf.Clamp(leftLegTorque * torqueForce, -maxTorque, maxTorque);
        
        rightLegRb.AddTorque(clampedRightLegTorque);
        leftLegRb.AddTorque(clampedLeftLegTorque);
    }

    void OnRightLeg(InputValue val){
        rightLegTorque = val.Get<float>();
    }
    void OnLeftLeg(InputValue val){
        leftLegTorque = val.Get<float>();
    }
}
