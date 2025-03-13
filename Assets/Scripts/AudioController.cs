using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.Scripting.APIUpdating;


public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource animatedTable;
    [SerializeField] AudioSource tableBam;
    bool grounded = true;
    void OnCollisionEnter(Collision collision)
        {
            grounded = true;
            Debug.Log("onground");
        }

     void OnCollisionExit(Collision collision)
        {
            grounded = false;
            Debug.Log("air");
        }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

         if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.F)){
            if (!animatedTable.isPlaying && grounded == true){
                animatedTable.Play();
         }
         }
         else{
            animatedTable.Stop();
         }
        
        
    }


}
