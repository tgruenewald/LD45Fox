using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class GameState
{
    private static GameObject droplet = null;
    public static GameObject introMusic = null;
	public static Boolean hasEarthTotem = false;
	public static GameObject hungerCountText;
	public static GameObject appleCountText;
	public static int hungerCount = 0;  // at 100 you die
	public static int appleCount = 0;	// at 100 you create surplus
	public static int surplusCount = 0;

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
}