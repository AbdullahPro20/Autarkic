using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Examples;
using TouchControlsKit;
public class attack : MonoBehaviour
{
    public GameObject cam;
    public GameObject enemyhealth;
    public GameObject laser;
    public GameObject laserloc;
    public GameObject carrylady;
    public GameObject carryladypos;
    public GameObject Carry_Parent;
    public GameObject fallpoint;
    public float minRotation = -45;
    public float maxRotation = 45;
    public static bool hit;
   public GameObject x;
  
    void Start()
    {
        
    }
    public void lasermethod() {
        StartCoroutine(laserCoroutine());
    }
    IEnumerator ExampleCoroutine()
    {
    
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        this.gameObject.GetComponent<Animator>().SetBool("hit", true);
        hit = true;
    
        yield return new WaitForSeconds(0.3f);
        
        this.gameObject.GetComponent<Animator>().SetBool("hit", false);
    
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    IEnumerator laserCoroutine()
    {
            this.gameObject.GetComponent<Animator>().SetBool("laser", true);
        yield return new WaitForSeconds(0.3f);
        Instantiate(laser, laserloc.transform.position, transform.rotation);
        hit = false;
        yield return new WaitForSeconds(0.3f);

        this.gameObject.GetComponent<Animator>().SetBool("laser", false);
            }
    public void PlayerRotation(float horizontal, float vertical)
    {
        
    }
    private void Update()
    {
        Vector2 look = TCKInput.GetAxis("Touchpad");
        
            this.transform.Rotate(Vector3.up * look.x * 500 * Time.deltaTime);
       
            cam.transform.Rotate(Vector3.left * look.y * 500 * Time.deltaTime);

        Vector3 currentRotation = cam.transform.localRotation.eulerAngles;
        currentRotation.x = Mathf.Clamp(currentRotation.x, minRotation, maxRotation);
        cam.transform.localRotation = Quaternion.Euler(currentRotation);



        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(laserCoroutine());

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            this.gameObject.GetComponent<Animator>().SetBool("carry", true);
           
            GameObject x = Instantiate(carrylady, carryladypos.transform.position, carryladypos.transform.rotation) as GameObject;
            x.transform.SetParent(Carry_Parent.transform);

        }
    }
    


    public void pickingbody() {
        this.gameObject.GetComponent<Animator>().SetBool("carry", true);
        x = Instantiate(carrylady, carryladypos.transform.position, carryladypos.transform.rotation) as GameObject;
        x.transform.SetParent(Carry_Parent.transform);

    }
    void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(laserCoroutine());
        
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            StartCoroutine(ExampleCoroutine());
        }
    } public void attackmethod() {
        hit = true;
        StartCoroutine(ExampleCoroutine());
    }
}
