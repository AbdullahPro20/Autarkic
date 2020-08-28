using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadlevel : MonoBehaviour
{
    public string levelname;
    public bool loadonstart;
    public bool wait;
    public float NUM;
    // Start is called before the first frame update
    void Start()
    {
        if (wait) {

            StartCoroutine(ExampleCoroutine());

        }

        if (loadonstart)
        {
            SceneManager.LoadScene(levelname);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadlevels() {

       // SceneManager.LoadScene(levelname);
       // move.score = 0;
        StartCoroutine(ExampleCoroutine1());
    }
    public void exitlevel() {

        Application.Quit();
    }

    IEnumerator ExampleCoroutine()
    {
   
       
        yield return new WaitForSeconds(NUM);
 
        SceneManager.LoadScene(levelname);
    }
    IEnumerator ExampleCoroutine1() 
    {
        yield return new WaitForSeconds(1);
       
        SceneManager.LoadScene(levelname);
       
    }
}
