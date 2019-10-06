using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	private float move;
	public static int defalutJumpNum = 1;
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
	public float jetpackstrength;
	private CircleCollider2D groundCheck;
	private Animator animator;
	private bool facingRight = true;
	  
	// Use this for initialization
	IEnumerator Fuel() {
		while (true) {
			yield return new WaitForSeconds(0.5f);
			GameState.JetPackFuelText.GetComponent<Text>().text = jetPackFuel.ToString();
			hasDoubleJump = GameState.hasDoubleJump;
			hasJetPack = GameState.hasJetpack;
			hasSprint = GameState.hasSprint;

		}
	}

	IEnumerator Fullness() {
		while (true) {
			yield return new WaitForSeconds(3);
			if (GameState.appleCount > 0) {
				GameState.appleCount--;
			} else {
				GameState.fullnessCount--;
			}
			GameState.fullnessCountText.GetComponent<Text>().text = GameState.fullnessCount.ToString();
			GameState.appleCountText.GetComponent<Text>().text = GameState.appleCount.ToString();
			
		}
	}
    public void SpawnAt(GameObject myPlayer)
    {
		Camera.main.GetComponent<SmoothCamera>().target = myPlayer;
//		myPlayer.GetComponent<BoxCollider2D> ().enabled = true;
		GameState.appleCountText = GameObject.Find("AppleCount");
		GameState.fullnessCountText = GameObject.Find("FullnessCount");
		GameObject[] apples = GameObject.FindGameObjectsWithTag("apple");
		GameState.appleTotalCountText = GameObject.Find("AppleTotalCount");
		GameState.JetPackFuelText = GameObject.Find("JetPackFuelText");
		GameState.appleTotalCountText.GetComponent<Text>().text = apples.Length.ToString();
		GameState.appleTotalCount = apples.Length;
		StartCoroutine("Fullness");
		StartCoroutine("Fuel");


    }
	void Awake(){
		DontDestroyOnLoad (this.gameObject); 
	}	
	void Start () {
		groundCheck = GetComponent<CircleCollider2D>();
		animator = GetComponent<Animator>();
	}
		
	

	
	// Update is called once per frame
	void Update () {
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
		
		if (hasJetPack) {
			if(Input.GetButton("Fly"))
			{
				animator.SetBool("IsFlying",true);
				Debug.Log("fly");
			}
			else
			{
				animator.SetBool("IsFlying",false);
			}
			if (jetPackFuel <= 0) {
				animator.SetBool("IsFlying",false);
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
				GetComponent<Rigidbody2D> ().AddForce(new Vector2(0f, 4f * jumpForce), ForceMode2D.Impulse);
				jumpNum--;
				
			}
		}

		if(hasJetPack && Input.GetButton("Fly") && jetPackFuel > 0)
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
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "apple" ) {
			Destroy(coll.gameObject);
			GameState.hasEarthTotem = true;
			GameState.appleCount++;
			GameState.appleTotalCount--;
			if (GameState.fullnessCount < 100) {
				GameState.fullnessCount++;
				GameState.appleCount--;
			}
			GameState.appleCountText.GetComponent<Text>().text = GameState.appleCount.ToString();
			GameState.appleTotalCountText.GetComponent<Text>().text = GameState.appleTotalCount.ToString();
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
			SpawnPoint.SwitchToLevel (this.gameObject);
			Debug.Log("Loading store");
			SceneManager.LoadScene("store2");
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
