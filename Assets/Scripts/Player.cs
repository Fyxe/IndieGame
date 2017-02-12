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
	public Currency Money = new Currency();

	[Header("Statistics")]
	public float HP_Current 		= 100f;
	public float HP_Max 			= 100f;
	float HP_Delay					= 1f;
	float HP_NextUpdate;
	public float Stamina_Current	= 100f;
	public float Stamina_Max		= 100f;
	float Stamina_Delay				= 1f;
	float Stamina_NextUpdate;
	float Stamina_Drain_Amount 		= 1f;
	float Stamina_Drain_Delay		= 0.15f;
	float Stamina_Drain_NextUpdate;

	[Header("Equippables")]
	public Equippable Player_Head;
	public Equippable Player_Chest;
	public Equippable Player_Legs;
	public Equippable Player_Feet;
	public Equippable Player_Gloves;
	public Equippable Player_Neck;
	public Equippable Player_Finger;

	public Equippable Player_LeftHand;
	public Equippable Player_RightHand;

	public Equippable Player_Backpack;

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
	public bool IsMoving			= false;
	public bool IsExausted			= false;
		
	[Header("Combat")]
	public bool IsInCombat 			= false;
	bool Cor_IsInCombat 			= false;
	Coroutine Beta;
	public float CombatDelay		= 5f;

	[Header("Abilities")]
	public bool CanDash 			= false;
	float DoubleTap_WaitTime		= 0.2f;
	float DoubleTap_NextUpdate	 	= 0;
	bool  DashActivated 			= false;
	float DashSpeed 				= 5.0f;
	Vector3 Dash;
	float Dash_Delay				= 0.2f;
	float Dash_NextUpdate			= 0;
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
	}

	//--------------------------------------------------------------------------------------------------------------

	void Start(){		
		if (!Cor_SlowUpdate_Helper) {
			Alpha = StartCoroutine (SlowUpdate_Helper ());
		}			

		Money.Add (100);
		Money.GetCoinage ();
	}																					

	//--------------------------------------------------------------------------------------------------------------

	void Update(){
		Calc_IsMoving ();
		User_Input ();
		Regeneration ();

	}

	#endregion
	//===============================================[Functions]====================================================
	#region Functions

	void SlowUpdate(){

	}

	//-------------------------------------------------------------------------------------------------------------

	void User_Input(){

		Input_Movement();
		Calc_Facing ();
		Input_Combat  ();
		Calc_Animation ();

	}		

	//--------------------------------------------------------------------------------------------------------------

	void Calc_Facing(){
		Vector3 MovementVector = Vector3.Normalize (new Vector3 (Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical")));

		float damper = 1;
		float stopped_damper = 1;
		bool Face_MousePosition = true;

		switch (Move_Mode) {
		case 0:						// Walking
			damper = 0.9f;	
			break;
		case 1:						// Running
			damper = 0.55f;
			break;
		case 2:						// Sprinting
			damper = 1;
			Face_MousePosition = false;
			break;
		case 3: 					// Crouching
			damper = 0.8f;
			stopped_damper = 0.5f;			
			break;
		case 4:						// Crawling
			damper = 0.3f;
			stopped_damper = 0.3f;
			Face_MousePosition = false;
			break;
		default:
			break;
		}

		if (MovementVector != Vector3.zero) {	// If the player is moving
			if (Face_MousePosition) {
				RaycastHit h;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out h)) {
					Vector3 Here = h.point;
					Here.y = transform.position.y;
					transform.forward = Vector3.Slerp (transform.forward, transform.forward + (Here - transform.position), FaceLerp * damper);

				}	
			} else {
				transform.forward = Vector3.Slerp (transform.forward, MovementVector, FaceLerp);
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

	void Input_Combat(){

	}


	//--------------------------------------------------------------------------------------------------------------
	#region Movement_Functions

	void Calc_IsMoving(){
		if (rb.velocity.magnitude != 0 && 
			(Input.GetKey (KeyCode.W) ||
				Input.GetKey (KeyCode.A) ||
				Input.GetKey (KeyCode.S) ||
				Input.GetKey (KeyCode.D))) {
			IsMoving = true;
		} else {
			IsMoving = false;
		}

		if (IsMoving && Move_Mode == 2) {
			if (Time.time > Stamina_Drain_NextUpdate) {
				Stamina_Drain_NextUpdate = Time.time + Stamina_Drain_Delay;
				Stamina_Decay (Stamina_Drain_Amount);
			}
		}
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
			if (LastKey == "w" && !IsExausted) {
				if (Time.time < DoubleTap_NextUpdate) {
					DashActivated = true;
					Stamina_Decay (10);
					Dash = new Vector3 (0, 0, DashSpeed);
				} else {
					DoubleTap_NextUpdate = Time.time + DoubleTap_WaitTime;	
				}
			} else {
				LastKey = "w";
				DoubleTap_NextUpdate = Time.time + DoubleTap_WaitTime;	
			}			

		} else if (Input.GetKeyDown (KeyCode.A)) {	
			if (LastKey == "a" && !IsExausted) {
				if (Time.time < DoubleTap_NextUpdate){
					DashActivated = true;
					Stamina_Decay (10);
					Dash = new Vector3 (-DashSpeed, 0, 0);
				} else {
					DoubleTap_NextUpdate = Time.time + DoubleTap_WaitTime;	
				}
			} else {
				LastKey = "a";
				DoubleTap_NextUpdate = Time.time + DoubleTap_WaitTime;	
			}	
		} else if (Input.GetKeyDown (KeyCode.S)) {		
			if (LastKey == "s" && !IsExausted) {	
				if (Time.time < DoubleTap_NextUpdate) {
					DashActivated = true;
					Stamina_Decay (10);
					Dash = new Vector3 (0, 0, -DashSpeed);
				} else {
					DoubleTap_NextUpdate = Time.time + DoubleTap_WaitTime;	
				}
			} else {
				LastKey = "s";
				DoubleTap_NextUpdate = Time.time + DoubleTap_WaitTime;	
			}	
		} else if (Input.GetKeyDown (KeyCode.D)) {		
			if (LastKey == "d" && !IsExausted) {	
				if (Time.time < DoubleTap_NextUpdate) {
					DashActivated = true;
					Stamina_Decay (10);
					Dash = new Vector3 (DashSpeed, 0, 0);
				} else {
					DoubleTap_NextUpdate = Time.time + DoubleTap_WaitTime;	
				}
			} else {
				LastKey = "d";
				DoubleTap_NextUpdate = Time.time + DoubleTap_WaitTime;	
			}	
		}

		if (DashActivated && Dash_NextUpdate < Time.time) {			
			Dash_NextUpdate = Time.time + Dash_Delay;
			DashActivated = false;
		}

		if (Time.time > Dash_NextUpdate) {
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
			//xx *= 0.5f;
			//zz *= 0.5f;
		}

		Velo.x = xx;
		Velo.z = zz;

		rb.velocity = Velo + Dash;
	}

	//--------------------------------------------------------------------------------------------------------------

	void Increment_Move_Mode(){
		Move_Mode++;
		if (IsExausted) {
			if (Move_Mode > 1) {
				Move_Mode = 0;
			}
		} else {
			if (Move_Mode > 2) {
				Move_Mode = 0;
			}
		}
		IsCrouched = false;
		IsCrawling = false;
	}

	//--------------------------------------------------------------------------------------------------------------

	void Increment_Move_Mode(int speed){
		Move_Mode = speed;
		if (IsExausted) {
			if (Move_Mode > 1) {
				Move_Mode = 0;
			}
		} else {
			if (Move_Mode > 2) {
				Move_Mode = 0;
			}
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

	#endregion
	//--------------------------------------------------------------------------------------------------------------
	#region Stat_Commands

	void Heal(float amount){		
		HP_Current = Mathf.Clamp (HP_Current + amount,0f,HP_Max);
		//Debug.Log ("Player was healed by a ghost for: " + amount + " hp.");
	}

	//--------------------------------------------------------------------------------------------------------------

	void Hurt(float amount){
		HP_Current = Mathf.Clamp (HP_Current - amount,0f,HP_Max);

		//Debug.Log ("Player was hit by a ghost for: " + amount + " dmg.");

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

	//-------------------------------------------------------------------------------------------------------------

	void Regeneration (){
		if(Time.time > HP_NextUpdate){
			HP_NextUpdate = Time.time + HP_Delay;
			Heal (1);
		}

		if (Time.time > Stamina_NextUpdate) {
			Stamina_NextUpdate = Time.time + Stamina_Delay;
			Stamina_Recover (1);
		}
	}

	//-------------------------------------------------------------------------------------------------------------

	void Stamina_Recover(float amount){
		Stamina_Current = Mathf.Clamp (Stamina_Current + amount,0f,Stamina_Max);
		if (Stamina_Current > Stamina_Max * 0.2f) {
			IsExausted = false;
		}
	}

	//-------------------------------------------------------------------------------------------------------------

	void Stamina_Decay(float amount){
		Stamina_Current = Mathf.Clamp (Stamina_Current - amount,0f,Stamina_Max);
		if (Stamina_Current == 0) {
			Increment_Move_Mode (0);
			IsExausted = true;
		}
	}

	#endregion
	//--------------------------------------------------------------------------------------------------------------

	void PlayerHasDied(){
		
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
