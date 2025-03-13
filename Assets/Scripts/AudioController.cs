using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;


public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource animatedTable;
    [SerializeField] AudioSource tableBam;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.F)){
            animatedTable.Play();
        }
        else{
            animatedTable.Stop();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        tableBam.Play();
    }
}
