using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class GameState
{
    private static GameObject droplet = null;
    public static GameObject introMusic = null;
	public static Boolean hasEarthTotem = false;
	public static GameObject fullnessCountText;
	public static GameObject appleCountText;

	public static GameObject appleTotalCountText;
	public static GameObject JetPackFuelText;
	public static GameObject JetPackFuelPacketText;

	public static int appleTotalCount = 0;
	public static int fullnessCount = 20;  // at 0 you die
	public static int appleCount = 30;	// at 100 you create surplus
	public static int surplusCount = 0;

	public static bool hasJetpack = false;
	public static bool hasDoubleJump = false;
	public static bool hasSprint = false;
	public static bool isGamePaused = false;
	public static int jetFuelPacketes = 0;

	public static string currentLevel = "scene1";

	public static void SetPlayerDroplet(GameObject droplet){
		GameState.droplet = droplet;
	}


	public static Player GetPlayerDroplet(){
		if(droplet == null){
			droplet = GameObject.FindGameObjectWithTag("Player");
			if(droplet == null) {
                Debug.Log("Getting player droplet but it is null");
                return null;
            }
				
		}

		return droplet.GetComponent<Player>();
	}
	 public static void resetAll() {
	  appleTotalCount = 0;
	  fullnessCount = 20;  // at 0 you die
	  appleCount = 10;	// at 100 you create surplus
	  surplusCount = 0;
	  jetFuelPacketes = 0;
	  hasJetpack = false;
	  hasDoubleJump = false;
	  hasSprint = false;
	  isGamePaused = false;	
	}

	
}