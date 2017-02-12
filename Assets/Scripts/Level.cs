﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose: Hold all data for current level and deal with level generation
*/
public class Level : MonoBehaviour {

	//===============================================[Variables]====================================================

	[Header("Debug")]
	public Transform StartPos;

	[Header("LevelStuff")]
	public string Level_Name 					= "New Level";

	int Difficulty;

	Player player;

	public List<LevelPiece> Dangerous_Pieces 	= new List<LevelPiece> ();
	public List<LevelPiece> Friendly_Pieces 	= new List<LevelPiece> ();
	public List<Item> Items 					= new List<Item>();

	public List<LevelPiece> Shop_Pieces 		= new List<LevelPiece> ();

	public LevelPiece StartPiece;
	public LevelPiece EndPiece;

	public int PiecesToGen = 10;

	//============================================[Unity Functions]=================================================

	void Awake(){
		player = FindObjectOfType<GameManager> ().player;
		Difficulty = FindObjectOfType<GameManager> ().Difficulty;
	}

	//--------------------------------------------------------------------------------------------------------------

	void Start(){		
		//StartCoroutine (GenerateLevel ());
		//GenerateLevel();
	}

	//===============================================[Functions]====================================================

	void GenerateLevel(){
		/*
		This function is a mess and i'm in the process of cleaning it. BUT IT WORKS DAMMIT
		 Here's the rundown:
			- It tries to put a random piece onto the next connection point
			- If it fails it rotates said piece until it fits
			- If it doesn't, it trys another piece at that location
			- If no pieces in any rotation fit that location it sets that location to closed and iterates back to
				the next open location.
			- If no pieces fit any of the previous locations it errors out (BUG neex to fix)
			
		If you can think of anything else that this should do let me know and i'll try it out

		BUGS:
			- THIS IS KILLING ME. If you're a programmer PLEASE look at this. The script below will randomly remove open connections from a piece that should still be open. BUT when you turn this into a co-routine it works fine. >_<
		*/

		//yield return new WaitForSeconds (0f);	

		// DEBUG =========
		float PlayBackSpeed = 0.01f;
		// ===============

		int Random_Piece = 0;			// Used to get a random piece from Dangerous_Pieces
		List<int> Pieces_Left = new List<int>();
		for (int i = 0; i < Dangerous_Pieces.Count; i++) {
			Pieces_Left.Add (i);
		}

		int Last_Connection = 0;		// The last piece connection used
		int Current_Connection = 0;		// The current place that will connect to the last levelpiece's place
		int Next_Conntection = 1;		// The next piece 

		List<int> Remaining_Connections = new List<int>();	// Allows to pick a next piece which isnt the current piece

		Vector3 Last_ConnectionPosition = StartPos.position;			// The position of the last connection
		Vector3 Last_DirectionFromConnection = Vector3.right;	// The direction from the last connection inwards


		GameObject p;					// Used as the piece currently being positioned
		LevelPiece lp;					// Used as the levelpiece of p
		LevelPiece Last_lp = null;		// Used as the levelpiece of last p

		float angle = 0;				// angle between last position inwards, and current piece's connection point outwards

		Random.seed = 1289438;

		for (int i = 0; i < 20; i++) {	// Currently uses a set number and generates pieces until it hits that number
			Pieces_Left.Clear();		// Clears the current cache of pieces left to choose from
			for (int j = 0; j < Dangerous_Pieces.Count; j++) {	// Resets pieces to chosoe from
				Pieces_Left.Add (j);
			}
			Random_Piece = Random.Range (0,Dangerous_Pieces.Count);		// Picks a piece from the given list
			Pieces_Left.Remove(Random_Piece);							// Removes that piece from being chosen again

			Remaining_Connections = new List<int> ();					// Clears remaining connections

			p = Instantiate (Dangerous_Pieces [Random_Piece].gameObject, StartPos.position, Quaternion.identity) as GameObject;	
			p.name = "piece " + i;
			lp = p.GetComponent<LevelPiece> ();							// These 3 statements generate the piece and name it


			for (int j = 0; j < lp.NumberOfSides; j++) {				// This adds all available sides that can be used to connect
				Remaining_Connections.Add(j);
			}


			Current_Connection = Random.Range (0,lp.NumberOfSides);		// Picks a random side to connect
			Remaining_Connections.Remove(Current_Connection);			// Removes that side from being chosen again

			if (Remaining_Connections.Count == 0) {
				Next_Conntection = 411;
			} else {
				Next_Conntection = Remaining_Connections[Random.Range (0,Remaining_Connections.Count)];	// Picks a random place for the next piece to connect
			}



			p.transform.position = Last_ConnectionPosition + (p.transform.position - lp.Get_Pos (Current_Connection));	// Moves the piece into place

			if (Last_lp != null) {	// Checks to prevent null, only for first piece
				angle = MathT.AngleSigned (lp.Get_Dir(Current_Connection),Last_lp.Get_Dir(Last_Connection) * (-1),Vector3.up); // calculates angle needed to rotate
				lp.PreviousConnection = Last_lp;	// Sets the previous piece for the current piece
			}

			p.transform.RotateAround(lp.Get_Pos(Current_Connection),Vector3.up,angle);	// Rotates piece into place






			if (!lp.CheckIfFits ()) {		// boxcast to see if it fits correctly
				while (lp.PreviousConnection != null && !lp.CheckIfFits ()) {						// Tries all previous pieces in chain
					while (lp.PreviousConnection.OpenPoints.Count > 1 && !lp.CheckIfFits ()) {	// try previous pieces open connections
						while (Pieces_Left.Count > 1 && !lp.CheckIfFits ()) {					// Try all pieces at connection
							while (Remaining_Connections.Count > 0 && !lp.CheckIfFits ()) {			// Try all rotations of current piece at connection							
								Current_Connection = Remaining_Connections [Random.Range (0, Remaining_Connections.Count)];		// Picks a random point on piece to connect to last piece
								Remaining_Connections.Remove (Current_Connection);												// Removes this point from being chosen again
								if (Remaining_Connections.Count < 1) {															// if there are no more connections, then this piece is an end piece
									// TODO? add functionality to do something when this occurs

									break;
								} else {	// If there are more connections, set it so that it uses them
									Next_Conntection = Remaining_Connections [Random.Range (0, Remaining_Connections.Count)];		// The next connection is any one of the open pieces left
									Debug.Log ("Trying new connection " + Current_Connection);	
									p.transform.position = Last_ConnectionPosition + (p.transform.position - lp.Get_Pos (Current_Connection));	// Moves the piece into place

								}


								if (Last_lp != null) {	// Checks to prevent null, only for first piece (may change when start piece is implemented)
									angle = MathT.AngleSigned (lp.Get_Dir (Current_Connection), Last_lp.Get_Dir (Last_Connection) * (-1), Vector3.up); // calculates angle needed to rotate
									lp.PreviousConnection = Last_lp;											// Set the last piece's previous connection
								} else {
									angle = MathT.AngleSigned (lp.Get_Dir (Current_Connection), Last_DirectionFromConnection, Vector3.up); // calculates angle needed to rotate
								}

								p.transform.RotateAround (lp.Get_Pos (Current_Connection), Vector3.up, angle);	// Rotates piece into place

								//yield return new WaitForSeconds (0f);	// Debug to show what is happening // TODO BUG when this is removed some pieces will have open coneections points closed
							}	// End of while (tries all rotations)
							Debug.Log ("-------------------------- No rotations of current piece will work, changing piece. --------------------------");
							Pieces_Left.Remove (Random_Piece);								// Since that piece didn't work, remove it from possible pieces to be chosen
							if (Pieces_Left.Count == 0 && !lp.CheckIfFits()) {				// If there are no pieces left
								break;
							}

							if (!lp.CheckIfFits ()) {										// if the piece doesnt fit try a different connection position

								Remaining_Connections = new List<int> ();					// Clears remaining connections

								Random_Piece = Pieces_Left [Random.Range (0, Pieces_Left.Count - 1)];
								Debug.Log ("Trying new piece (" + Random_Piece + ") at last piece's connection position " + Last_Connection);


								Destroy (p.gameObject);										// Destroy this piece and choose another from those left

								p = Instantiate (Dangerous_Pieces [Random_Piece].gameObject, StartPos.position, Quaternion.identity) as GameObject;	
								p.name = "piece " + i;
								lp = p.GetComponent<LevelPiece> ();							// These 3 statements generate the piece and name it
								lp.PreviousConnection = Last_lp;							// Set the piece's previous connection

								for (int j = 0; j < lp.NumberOfSides; j++) {				// This adds all available sides that can be used to connect
									Remaining_Connections.Add (j);
								}

							}			
							//yield return new WaitForSeconds (0.0f);		// Debug
						} // End of while (tries all pieces)
						Debug.Log ("-------------------------------- No pieces fit the current connection postition. --------------------------------");

						lp.PreviousConnection.OpenPoints.Remove (Last_Connection); 			// Since no pieces fit on this connection, remove it
						if (lp.PreviousConnection.OpenPoints.Count == 0) {					// If there are no more connections to choose from, break
							break;
						}

						if (!lp.CheckIfFits ()) {											// If it still didn't fit

							Pieces_Left.Clear ();											// Clear the pieces left
							for (int j = 0; j < Dangerous_Pieces.Count; j++) {				// And reset them to choose from all
								Pieces_Left.Add (j);
							}

							Destroy (p.gameObject);											// Destroy this piece and choose another from those left

							p = Instantiate (Dangerous_Pieces [Random_Piece].gameObject, StartPos.position, Quaternion.identity) as GameObject;	
							p.name = "piece " + i;
							lp = p.GetComponent<LevelPiece> ();								// These 3 statements generate the piece and name it
							lp.PreviousConnection = Last_lp;

							for (int j = 0; j < lp.NumberOfSides; j++) {					// This adds all available sides that can be used to connect
								Remaining_Connections.Add (j);
							}


							if (lp.PreviousConnection.OpenPoints.Count > 0) {								// If the previous piece has an open connection
								Last_Connection = lp.PreviousConnection.Get_Connection ();					// Set the last piece's connection to another open connection
								Debug.Log ("Changing last connection point to " + Last_Connection);
								lp.PreviousConnection.OpenPoints.Remove (Last_Connection);					// Remove this point from being chosen again
								Last_ConnectionPosition = lp.PreviousConnection.Get_Pos (Last_Connection);	// Set the last position to the new point

							} else {														// If there are no more open points on the last piece, change the last piece
								break;
							}

						}	// End of if
						//yield return new WaitForSeconds (0.0f);			// Debug
					}	// End of while (tries all connection points of last piece)
					//yield return new WaitForSeconds (0.0f);				// Debug
					Debug.Log ("------------------------ No pieces fit any of the current connection postitions. ------------------------");


					if (!lp.CheckIfFits ()) {												// If it STILL doesn't fit						

						if (lp.PreviousConnection.PreviousConnection == null) {				// If the 2nd previous piece doesn't exist, break
							break;
						}

						while (lp.PreviousConnection.PreviousConnection != null) {			// If it does, check if 
							lp.PreviousConnection = lp.PreviousConnection.PreviousConnection; 	// Set the last piece to the last piece's last piece
							if (lp.PreviousConnection.OpenPoints.Count > 0) {				// If the previous piece has an open position, try it
								break;
							}
						}


						Last_Connection = lp.PreviousConnection.Get_Connection ();			// These next 3 set the next piece's connection areas

						Last_ConnectionPosition = lp.PreviousConnection.Get_Pos (Last_Connection);
						Last_lp = lp.PreviousConnection;

					}						
				}	// End of while (tries last piece's last piece)
			}	// End of if

			if (lp.CheckIfFits ()) {		// Debug
				Debug.Log ("===================! " + lp.name + " placed sucessfully at position " + Last_Connection + " !===================");			
			} else {
				Debug.Log("Nothing else will fit!");
					Destroy (p.gameObject);
					//return;
				break;
			}

			lp.OpenPoints.Remove (Current_Connection);					// Remove the connected point from open connections

			Last_Connection = Next_Conntection;							// Sets the last connection to be the next one chosen
			Last_ConnectionPosition = lp.Get_Pos (Next_Conntection);	// Gets it's position
			Last_lp = lp;												// Saves the levelpiece for further use

		}

	}	// End of function


	//--------------------------------------------------------------------------------------------------------------

	void AddPiece(Transform LastPlace, int place){		
		/*
			DOES NOT WORK. Was trying to think through the algorihm at 2:00am and it just wasnt computing.
			Will revisit this when I've got the time.
		*/
		int r = Random.Range (0,Dangerous_Pieces.Count - 1);
		int r_start = r;
		bool Fits = false;
		while (!Fits && r != r_start) {
			/*
			if (Physics.BoxCast (Vector3.zero, Dangerous_Pieces [r].HalfExtents, Dangerous_Pieces [r].Direction)) {
				r = Iterate (r, Dangerous_Pieces.Count - 1);
			} else {
				Fits = true;
			}
			*/
		}

		if (r == r_start) {
			return;
		} else {
			// Place Piece
		}

	}

	//--------------------------------------------------------------------------------------------------------------

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------





}	// End of class
