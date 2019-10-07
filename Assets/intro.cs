using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class intro : MonoBehaviour
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
                GameObject.Find("HelpText").GetComponent<Text>().text = "Now that you have double jump you can jump again in midair reach high up apples.";
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
