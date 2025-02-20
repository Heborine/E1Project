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
        rightLegRb.AddTorque(rightLegTorque * torqueForce);
        leftLegRb.AddTorque(leftLegTorque * torqueForce);
    }

    void OnRightLeg(InputValue val){
        rightLegTorque = val.Get<float>();
    }
    void OnLeftLeg(InputValue val){
        leftLegTorque = val.Get<float>();
    }
}
