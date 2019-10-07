using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Store : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameState.isGamePaused = true;
        StartCoroutine("SurplusApples");
    }

	IEnumerator SurplusApples() {
		while (true) {
            GameObject.Find("SurplusApples").GetComponent<Text>().text = "Apples to spend: " + GameState.appleCount.ToString();
			yield return new WaitForSeconds(0.5f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
