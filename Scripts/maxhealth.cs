using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maxhealth : MonoBehaviour
{
    public GameObject healthbar;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            healthbar.GetComponent<Healthbar>().SetHealth(100);
        }
    }
}
