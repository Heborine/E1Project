using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    [SerializeField] GameObject[] npcArr;
    [SerializeField] GameObject gate;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < npcArr.Length; i++) 
        {
            if (npcArr[i] != null) { return; }
        }
        gate.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
