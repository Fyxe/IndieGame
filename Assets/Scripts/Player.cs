using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class Purpose: Store and compute data that is used for the player
*/

public class Player : MonoBehaviour {		

	//===============================================[Variables]====================================================

	[Header("Attributes")]
	public string Player_Name 		= "Bob";

	[Header("Statistics")]
	public float HP_Current 		= 100f;
	public float HP_Max 			= 100f;

	public float Stamina_Current	= 100f;
	public float Stamina_Max		= 100f;

	[Header("Movement")]
	public int Move_Mode 			= 0;
	public float Move_Speed 		= 20f;
	public float Jump_Force 		= 100f;
	public float FaceLerp			= 0.1f;
	int Move_Crouch_Store			= 0;
	int Move_Crawl_Store			= 0;
	public bool IsCrouched 			= false;
	public bool IsGrounded  		= false;
	public bool IsCrawling 			= false;
		
	[Header("Combat")]
	public bool IsInCombat 			= false;
	bool Cor_IsInCombat 			= false;
	Coroutine Beta;
	public float CombatDelay		= 5f;

	[Header("Abilities")]
	public bool CanDash 			= false;
	float DoubleTapWaitTime			= 0.2f;
	float Waiting = 0;
	bool DashActivated 				= false;
	float DashSpeed 				= 5.0f;
	Vector3 TempDash;
	Vector3 Dash;
	float DashDelay					= 0.2f;
	float CurrentDashTime			= 0;
	public string LastKey;

	[Header("UnityVariables")]
	public Animator anim;
	Rigidbody rb;
	float Jump_Delay 				= 0.1f;
	float Jump_Next 				= 0;
	bool Cor_SlowUpdate_Helper 		= false;							
	float SlowUpdateTime 			= 0.1f;							
	Coroutine Alpha;										

	//============================================[Unity Functions]=================================================
	#region UnityFunctions

	void Awake(){
		rb = GetComponent<Rigidbody> ();
		//anim = GetComponent<Animator> ();
	}

	//--------------------------------------------------------------------------------------------------------------

	void Start(){		
		if (!Cor_SlowUpdate_Helper) {
			Alpha = StartCoroutine (SlowUpdate_Helper ());
		}
	}																					

	//--------------------------------------------------------------------------------------------------------------

	void Update(){
		User_Input ();
	}

	#endregion
	//===============================================[Functions]====================================================
	#region Functions

	void User_Input(){
		Input_Movement();
		Calc_Facing ();
		Input_Combat  ();
		Calc_Animation ();
	}

	//--------------------------------------------------------------------------------------------------------------

	void Input_Movement (){
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			Increment_Move_Mode ();
		} else if (Input.GetKeyDown (KeyCode.LeftControl)) {
			Move_Crouch_Store = Move_Mode;
			Move_Mode = 3;
			IsCrouched = true;
			IsCrawling = false;
		} else if (Input.GetKeyUp (KeyCode.LeftControl)) {			
			Move_Mode = Move_Crouch_Store;
			IsCrouched = false;
			if (Move_Mode == 4) {
				IsCrawling = true;
			}
		} else if (Input.GetKeyDown (KeyCode.Z)) {
			if (IsCrawling) {
				Move_Mode = Move_Crawl_Store;
				IsCrawling = false;
			} else {
				Move_Crawl_Store = Move_Mode;
				Move_Mode = 4;
				IsCrawling = true;
			}				
		} else if (Input.GetKeyDown (KeyCode.W)) {			
			if (LastKey == "w") {
				if (Time.time < Waiting) {
					DashActivated = true;
					Dash = new Vector3 (0, 0, DashSpeed);
				} else {
					Waiting = Time.time + DoubleTapWaitTime;	
				}
			} else {
				LastKey = "w";
				Waiting = Time.time + DoubleTapWaitTime;	
			}			

		} else if (Input.GetKeyDown (KeyCode.A)) {	
			if (LastKey == "a") {
				if (Time.time < Waiting){
					DashActivated = true;
					Dash = new Vector3 (-DashSpeed, 0, 0);
				} else {
					Waiting = Time.time + DoubleTapWaitTime;	
				}
			} else {
				LastKey = "a";
				Waiting = Time.time + DoubleTapWaitTime;	
			}	
		} else if (Input.GetKeyDown (KeyCode.S)) {		
			if (LastKey == "s") {	
				if (Time.time < Waiting) {
					DashActivated = true;
					Dash = new Vector3 (0, 0, -DashSpeed);
				} else {
					Waiting = Time.time + DoubleTapWaitTime;	
				}
			} else {
				LastKey = "s";
				Waiting = Time.time + DoubleTapWaitTime;	
			}	
		} else if (Input.GetKeyDown (KeyCode.D)) {		
			if (LastKey == "d") {	
				if (Time.time < Waiting) {
					DashActivated = true;
					Dash = new Vector3 (DashSpeed, 0, 0);
				} else {
					Waiting = Time.time + DoubleTapWaitTime;	
				}
			} else {
				LastKey = "d";
				Waiting = Time.time + DoubleTapWaitTime;	
			}	
		}

		if (DashActivated && CurrentDashTime < Time.time) {			
			CurrentDashTime = Time.time + DashDelay;
			DashActivated = false;
		}

		if (Time.time > CurrentDashTime) {
			Dash = Vector3.zero;
			DashActivated = false;
		}

		float Damper = 0.1f;
		float xx = Input.GetAxis ("Horizontal") * Get_MoveSpeed() * Damper;
		float zz = Input.GetAxis ("Vertical")   * Get_MoveSpeed() * Damper;

		Vector3 Velo = rb.velocity;

		if (IsGrounded) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				Jump ();
			}
		} else {	// If not IsGrounded
			xx *= 0.5f;
			zz *= 0.5f;
		}

		Velo.x = xx;
		Velo.z = zz;

		rb.velocity = Velo + Dash;
	}

	//--------------------------------------------------------------------------------------------------------------

	void Calc_Facing(){
		Vector3 Faceing = Vector3.Normalize (new Vector3 (Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical")));

		float damper = 1;
		float stopped_damper = 1;
		bool FaceMove = true;

		switch (Move_Mode) {
		case 0:						// Walking
			damper = 0.9f;	
			break;
		case 1:						// Running
			damper = 0.55f;
			break;
		case 2:						// Sprinting
			damper = 1;
			FaceMove = false;
			break;
		case 3: 					// Crouching
			damper = 0.8f;
			stopped_damper = 0.5f;			
			break;
		case 4:						// Crawling
			stopped_damper = 0.3f;
			FaceMove = false;
			break;
		default:
			break;
		}

		if (Faceing != Vector3.zero) {	// If the player is moving
			if (FaceMove) {
				RaycastHit h;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out h)) {
					Vector3 Here = h.point;
					Here.y = transform.position.y;
					transform.forward = Vector3.Slerp (transform.forward, transform.forward + (Here - transform.position), FaceLerp * damper);

				}	
			} else {
				transform.forward = Vector3.Slerp (transform.forward, Faceing, FaceLerp);
			}

		} else {						// if the player is not moving			
			RaycastHit hhh;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hhh)) {
				Vector3 Here = hhh.point;
				Here.y = transform.position.y;
				transform.forward = Vector3.Slerp (transform.forward, transform.forward + (Here - transform.position), FaceLerp * stopped_damper);

			}
		}



	}

	//--------------------------------------------------------------------------------------------------------------

	void Calc_Animation (){
		anim.SetFloat ("MoveSpeed",Mathf.Abs(rb.velocity.magnitude));
		anim.SetBool ("Grounded", IsGrounded);
	}

	//--------------------------------------------------------------------------------------------------------------

	void Increment_Move_Mode(){
		Move_Mode++;
		if (Move_Mode > 2) {
			Move_Mode = 0;
		}
		IsCrouched = false;
		IsCrawling = false;

	}

	//--------------------------------------------------------------------------------------------------------------

	void Jump(){
		if (Time.time > Jump_Next) {
			Jump_Next = Time.time + Jump_Delay;
			rb.AddForce (0f, Jump_Force, 0f);
		}
	}

	//--------------------------------------------------------------------------------------------------------------

	void Input_Combat(){
		
	}

	//--------------------------------------------------------------------------------------------------------------

	float Get_MoveSpeed(){
		switch (Move_Mode) {
		case 0:
			return Move_Speed * 0.8f;	// Walking
		case 1:
			return Move_Speed;			// Running
		case 2:
			return Move_Speed * 1.3f;	// Sprinting
		case 3:
			return Move_Speed * 0.5f;	// Crouching
		case 4:
			return Move_Speed * 0.2f;	// Crawling
		default:
			return Move_Speed * 4f;		// SUPER
		}
	}

	//--------------------------------------------------------------------------------------------------------------
	#region HP_Commands

	void Heal(float amount){		
		HP_Current = Mathf.Clamp (HP_Current + amount,0f,HP_Max);
		Debug.Log ("Player was healed by a ghost for: " + amount + " hp.");
	}

	//--------------------------------------------------------------------------------------------------------------

	void Hurt(float amount){
		HP_Current = Mathf.Clamp (HP_Current - amount,0f,HP_Max);

		Debug.Log ("Player was hit by a ghost for: " + amount + " dmg.");

		if (!Cor_IsInCombat) {
			Beta = StartCoroutine (IsInCombat_Helper ());
		} else {
			StopCoroutine (Beta);
			Beta = StartCoroutine (IsInCombat_Helper ());
		}

		if (HP_Current == 0) {
			PlayerHasDied ();
		}
	}

	#endregion
	//--------------------------------------------------------------------------------------------------------------

	void PlayerHasDied(){
		
	}

	//--------------------------------------------------------------------------------------------------------------

	void SlowUpdate(){

	}

	//--------------------------------------------------------------------------------------------------------------
	#region IEnumerator		

	IEnumerator SlowUpdate_Helper(){						
		Cor_SlowUpdate_Helper = true;								

		SlowUpdate ();

		yield return new WaitForSeconds (SlowUpdateTime);

		Cor_SlowUpdate_Helper = false;		
	}

	//--------------------------------------------------------------------------------------------------------------

	IEnumerator IsInCombat_Helper(){
		Cor_IsInCombat = true;

		IsInCombat = true;

		yield return new WaitForSeconds (CombatDelay);

		IsInCombat = false;

		Cor_IsInCombat = false;
	}

	#endregion
	//--------------------------------------------------------------------------------------------------------------

	#endregion
	//==============================================================================================================

}	// End of class
