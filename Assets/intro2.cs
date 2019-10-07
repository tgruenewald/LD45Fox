using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class intro2 : MonoBehaviour
{
    // Start is called before the first frame update
    bool helpSceneLoaded = false;
    bool help3 = false;
    bool messageDone = false;
    bool messageDone2 = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator FirstMessage() {
			yield return new WaitForSeconds(1);
            if (!messageDone) {
                GameObject.Find("HelpText").GetComponent<Text>().text = "Congrats!  You got a difficult apple.  Get the last apple to the right, and then you can move to the next level.";                
                messageDone = true;
            }
    }
                    
	void OnTriggerEnter2D(Collider2D coll){

		if (coll.gameObject.tag == "Player" ) {
            if (!helpSceneLoaded) {
			    SceneManager.LoadScene("intro_2", LoadSceneMode.Additive);    
                helpSceneLoaded = true;
                
                StartCoroutine("FirstMessage");
            }


        }
    }
}
