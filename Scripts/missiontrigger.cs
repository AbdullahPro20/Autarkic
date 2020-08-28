using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missiontrigger : MonoBehaviour
{

    public GameObject missionon;
    public GameObject[] missionsoff;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player") {
            Destroy(this.gameObject);
            other.gameObject.SetActive(false);
            missionon.SetActive(true);
            for (int i = 0; i < missionsoff.Length; i++) {

                missionsoff[i].SetActive(false);
            }
        
        }
    }
}
