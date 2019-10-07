using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SpawnPoint : MonoBehaviour {
	void Awake() {
		var playerDroplet = GameState.GetPlayerDroplet();
		if (playerDroplet != null)
		{
			playerDroplet.SpawnAt(playerDroplet.gameObject);
			// playerDroplet.SpawnAt(gameObject);

		}
		else
		{
			Debug.Log("First time creating droplet");
			var gObj = (GameObject)Instantiate(Resources.Load("prefab/Droplet"), GetComponent<Transform>().position, GetComponent<Transform>().rotation) ;
			Debug.Log("Droplet is " + gObj);
			gObj.GetComponent<Player>().SpawnAt(gObj);
		}		
	}
	void Start () {


	}
	public void buyJetfuel() {
		if (GameState.appleCount >= 5) {
			Debug.Log("You have a jet fuel");
			GameState.appleCount -= 5;
			GameState.jetFuelPacketes += 1;
		}
	}	
	public void buyJetpack() {
		if (!GameState.hasJetpack && GameState.appleCount >= 20) {
			Debug.Log("You have a jet pack");
			GameState.appleCount -= 20;
			GameState.hasJetpack = true;
		}
	}	

	public void buyDoubleJump() {
		if (!GameState.hasDoubleJump && GameState.appleCount >= 10) {
			Debug.Log("You have double jump");
			GameState.appleCount -= 10;
			GameState.hasDoubleJump = true;
		}
	}	

	public void buySprint() {
		if (!GameState.hasSprint && GameState.appleCount >= 5) {
			Debug.Log("You have sprint");
			GameState.appleCount -= 5;
			GameState.hasSprint = true;
		}
	}	
	public void exitStore() {
		GameState.isGamePaused = false;
		// SpawnPoint.SwitchToLevel (this.gameObject);

		GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		SceneManager.UnloadSceneAsync("store2");
		//SceneManager.LoadScene(GameState.currentLevel, );
	}	
	public static void SwitchToLevel(GameObject playerObject)
	{
		playerObject.GetComponent<Transform>().position = GameObject.FindObjectsOfType<SpawnPoint>()[0].GetComponent<Transform>().position;
		// playerObject.GetComponent<BoxCollider2D> ().enabled = false;
		GameState.SetPlayerDroplet(playerObject);
		//GameState.GetPlayerDroplet().StopAllAudio();
	}	
}
