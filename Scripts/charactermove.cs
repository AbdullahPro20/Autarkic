using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactermove : MonoBehaviour
{ public bool rightb;
    public bool leftb;
    public int speed =10;
    public GameObject camera;
    public bool upb;
    public bool downb;
   public bool fallingback;
    public GameObject fallpoint;

    
    void Start()
    {
        
    }

    
    void Update()
    {  if (leftb)
        {
            this.transform.Rotate(-Vector3.up * speed * Time.deltaTime);
        }
        if (rightb) { this.transform.Rotate(Vector3.up * speed*Time.deltaTime);
        }

        if (upb)
        {
            int i = Mathf.Clamp(5, 0, 10);
            camera.transform.Rotate(-Vector3.left * speed * Time.deltaTime);
        }
        if (downb)
        {
            camera.transform.Rotate(-Vector3.right * speed * Time.deltaTime);
        }
 

           
          
    }
    public void left() {


        
    }

    public void right()
    {
        this.transform.Rotate(Vector3.right * Time.deltaTime);
    }
}
