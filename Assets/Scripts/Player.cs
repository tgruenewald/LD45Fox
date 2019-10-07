using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	private float move;
	public static int defalutJumpNum = 3;
	public int jumpNum = defalutJumpNum;
	private bool jump = false;
	public float maxSpeed = 3.0f;
	public float maxRunSpeed = 6.0f;
	public float jumpForce = 20f;
	public bool grounded = true;
    public LayerMask whatIsGround;
	public bool hasDoubleJump = false;
	public bool hasJetPack = false;
	public bool hasSprint = false;
	public int jetPackFuel = 100;
	public int jetpackMaxFuel = 100;
	bool fuelMessage = true;
	public float jetpackstrength;
	private CircleCollider2D groundCheck;
	private Animator animator;
	private bool facingRight = true;

	public GameObject floatingText;
	bool fuelReady = true;

	private CharacterController playerCharController;
	  
	// Use this for initialization
	IEnumerator Fuel() {
		while (true) {
			yield return new WaitForSeconds(0.5f);
			if (GameState.appleTotalCount <= 0) {
				// change to next level
				// SpawnPoint.SwitchToLevel (this.gameObject);
				GameState.resetAll();
				Destroy(gameObject);
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}

			GameState.JetPackFuelText.GetComponent<Text>().text = jetPackFuel.ToString();
			hasDoubleJump = GameState.hasDoubleJump;
			hasJetPack = GameState.hasJetpack;
			hasSprint = GameState.hasSprint;
			GameState.fullnessCountText.GetComponent<Text>().text = GameState.fullnessCount.ToString();
			GameState.appleCountText.GetComponent<Text>().text = GameState.appleCount.ToString();
			GameState.JetPackFuelPacketText.GetComponent<Text>().text = GameState.jetFuelPacketes.ToString();
		}
	}

	string GetFullnessText() {
		if (GameState.fullnessCount > 15) {
			return "Hungry";
		} 
		if (GameState.fullnessCount > 10) {
			return "Starving";
		} 
		if (GameState.fullnessCount > 5) {
			return "FAMISHED!";
		} 		
		return "Dying!!!!";
	}

	IEnumerator Fullness() {
		while (true) {
			yield return new WaitForSeconds(3);

			GameObject[] apples = GameObject.FindGameObjectsWithTag("apple");
			GameState.appleTotalCount = apples.Length;			
			GameState.appleTotalCountText.GetComponent<Text>().text = GameState.appleTotalCount.ToString();

			fuelMessage = true;
			if (!GameState.isGamePaused) {
				if (GameState.appleCount > 0) {
					GameState.appleCount--;
				} else {
					// hungry
					yield return new WaitForSeconds(0.5f);
					if (GameState.fullnessCount < 20) {
						floatingText.GetComponent<TextMesh>().text = GetFullnessText();
						Instantiate(floatingText, transform.position, Quaternion.identity, transform);
					}

					GameState.fullnessCount--;
					if (GameState.fullnessCount <= 0) {
						// you died
						GameState.resetAll();
						Destroy(gameObject);
						SceneManager.LoadScene("gameover");
					}
				}
			}

			GameState.fullnessCountText.GetComponent<Text>().text = GameState.fullnessCount.ToString();
			GameState.appleCountText.GetComponent<Text>().text = GameState.appleCount.ToString();
			
		}
	}
    public void SpawnAt(GameObject myPlayer)
    {
		Debug.Log("Spawning again");
		Camera.main.GetComponent<SmoothCamera>().target = myPlayer;
//		myPlayer.GetComponent<BoxCollider2D> ().enabled = true;
		GameState.appleCountText = GameObject.Find("AppleCount");
		GameState.fullnessCountText = GameObject.Find("FullnessCount");
		GameObject[] apples = GameObject.FindGameObjectsWithTag("apple");
		GameState.appleTotalCountText = GameObject.Find("AppleTotalCount");
		GameState.JetPackFuelText = GameObject.Find("JetPackFuelText");
		GameState.JetPackFuelPacketText = GameObject.Find("JetPackFuelPacketText");
		GameState.appleTotalCountText.GetComponent<Text>().text = apples.Length.ToString();
		GameState.appleTotalCount = apples.Length;
		StartCoroutine("Fullness");
		StartCoroutine("Fuel");


    }
	void Awake(){
		DontDestroyOnLoad (this.gameObject); 
	}	
	void Start () {
		playerCharController = GetComponent<CharacterController>();
		groundCheck = GetComponent<CircleCollider2D>();
		animator = GetComponent<Animator>();
	}
		
	

	
	// Update is called once per frame
	void Update () {
		if (GameState.isGamePaused) {
			// playerCharController.enabled = false;

			return;
		}

		grounded = groundCheck.IsTouchingLayers (whatIsGround);

		move = Input.GetAxis ("Horizontal");
		if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
		if (move != 0) {
			animator.SetBool("isMoving", true);
		}
		else {
			animator.SetBool("isMoving", false);
		}
		if (hasJetPack && Input.GetButton("Fly") && GameState.jetFuelPacketes <= 0) {
			if (fuelMessage) {
				floatingText.GetComponent<TextMesh>().text = "No fuel";
				Instantiate(floatingText, transform.position, Quaternion.identity, transform);							
				fuelMessage = false;
			}
			animator.SetBool("IsFlying",false);
		}
		if (hasJetPack && GameState.jetFuelPacketes > 0) {
			if(Input.GetButton("Fly"))
			{
				animator.SetBool("IsFlying",true);

				if (fuelMessage) {
					floatingText.GetComponent<TextMesh>().text = GameState.jetFuelPacketes.ToString() + " fuel left";
					Instantiate(floatingText, transform.position, Quaternion.identity, transform);				
					fuelMessage = false;
				}

				Debug.Log("fly");
			}
			else
			{
				animator.SetBool("IsFlying",false);
			}
			if (jetPackFuel <= 0) {
				animator.SetBool("IsFlying",false);
				if (fuelReady) {
					GameState.jetFuelPacketes--;
					fuelReady = false;
				}

			}
		}

		if ((jump || !grounded) && !Input.GetButton("Fly")) {
			animator.SetBool("isJumping", true);
		}
		else {
			animator.SetBool("isJumping", false);
		}
		
		if(GetComponent<Rigidbody2D> ().velocity.x > 3.0f || GetComponent<Rigidbody2D> ().velocity.x < -3.0f)
		{
			animator.SetBool("isRunning", true);
		}
		else
		{
			animator.SetBool("isRunning",false);
		}
		
		jump = Input.GetButtonDown ("Jump") || Input.GetButtonDown ("Vertical");
		
		//Debug.Log ("move = " + move);
	}
	void FixedUpdate () {
		if(grounded)
			{
				jumpNum = defalutJumpNum;
				if(jetPackFuel < jetpackMaxFuel)
				{
					jetPackFuel++;
				}
				else {
					fuelReady = true; 
				}

			}
		if(Input.GetButton("Run") && hasSprint )
		{
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (move * maxRunSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
		else
		{
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (move * maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
		if (grounded && jump) {
			Debug.Log("single jump");
			GetComponent<Rigidbody2D> ().AddForce(new Vector2(0f, 4f * jumpForce), ForceMode2D.Impulse);
		} else {

			if(hasDoubleJump && jump && jumpNum > 0)
			{
				Debug.Log("double jump");
				floatingText.GetComponent<TextMesh>().text ="Double Jump";
				Instantiate(floatingText, transform.position, Quaternion.identity, transform);	
				GetComponent<Rigidbody2D> ().AddForce(new Vector2(0f, 8f * jumpForce), ForceMode2D.Impulse);
				jumpNum--;
				
			}
		}

		if(hasJetPack && Input.GetButton("Fly") && jetPackFuel > 0 && GameState.jetFuelPacketes > 0)
		{
			GetComponent<Rigidbody2D> ().AddForce(new Vector2(0f, jetpackstrength), ForceMode2D.Impulse);
			jetPackFuel--;
		}
	}
    void Flip()
    {
        //Debug.Log("switching...");
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }	

	string GetAppleText() {
		if (GameState.fullnessCount < 20) {
			return "Hungry";
		} else {
			return GameState.appleCount.ToString() + " Extra";
		}
	}
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "apple" ) {
			floatingText.GetComponent<TextMesh>().text = GetAppleText();
			Instantiate(floatingText, coll.gameObject.transform.position, Quaternion.identity, coll.gameObject.transform);
			coll.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			Destroy(coll.gameObject, 2f);
			GameState.hasEarthTotem = true;
			GameState.appleCount++;
			
			if (GameState.fullnessCount < 20) {
				GameState.fullnessCount += 1;
				GameState.appleCount--;
			}

			GameState.appleCountText.GetComponent<Text>().text = GameState.appleCount.ToString();

		}
		if (coll.gameObject.tag == "earth" ) {
			if (GameState.hasEarthTotem) {
				BoxCollider2D[] components = coll.gameObject.GetComponents<BoxCollider2D>();
				foreach(BoxCollider2D component in components) {
					component.enabled = false;
				}
			}
		}		
		if (coll.gameObject.tag == "store" ) {
			// SpawnPoint.SwitchToLevel (this.gameObject);
			GameState.isGamePaused  = true;
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			Debug.Log("Loading store");
			SceneManager.LoadScene("store2", LoadSceneMode.Additive);
		}
		if (coll.gameObject.tag == "exit" ) {
			SpawnPoint.SwitchToLevel (this.gameObject);
			Debug.Log("Loading scene2");
			SceneManager.LoadScene("scene2");
		}		
	}

	// void OnTriggerExit2D(Collider2D coll){
	// 	if (coll.gameObject.layer == LayerMask.NameToLayer ("Water"))
	// 		isTouchingWater = false;
	// }
	
}
