using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewBehaviourScript : MonoBehaviour {

	public void start_the_game() {
		SceneManager.LoadScene("scene1");
	}
}
