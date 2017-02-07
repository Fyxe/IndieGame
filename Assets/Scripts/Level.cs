using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose: Hold all data for current level and deal with level generation
*/
public class Level : MonoBehaviour {

	//===============================================[Variables]====================================================

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
		//GenerateLevel_Linear ();
		//GenerateLevel();
		G2();
	}

	//===============================================[Functions]====================================================

	void G2(){

		int Random_Piece = 0;

		int Current_Connection = 0;
		int Last_Connection = 0;
		List<int> Remaining_Connections = new List<int>();
		int Next_Conntection = 1;
		Vector3 Last_ConnectionPosition = Vector3.zero;
		Vector3 Last_DirectionFromConnection = Vector3.zero;

		GameObject p;
		LevelPiece lp;
		LevelPiece Last_lp = null;

		float angle = 0;

		for (int i = 0; i < 15; i++) {
			Random_Piece = Random.Range (0,Dangerous_Pieces.Count);
			Remaining_Connections = new List<int> ();

			p = Instantiate (Dangerous_Pieces [Random_Piece].gameObject, Vector3.zero, Quaternion.identity) as GameObject;	
			p.name = "Piece: " + i;
			lp = p.GetComponent<LevelPiece> ();

			for (int j = 0; j < lp.NumberOfSides; j++) {
				Remaining_Connections.Add(j);
			}

			Current_Connection = Random.Range (0,lp.NumberOfSides);
			Remaining_Connections.Remove(Current_Connection);
			Next_Conntection = Remaining_Connections[Random.Range (0,Remaining_Connections.Count)];

			p.transform.position = Last_ConnectionPosition + (p.transform.position - lp.Get_Pos (Current_Connection));

			if (Last_lp != null) {
				angle = AngleSigned (lp.Get_Dir(Current_Connection),Last_lp.Get_Dir(Last_Connection) * (-1),Vector3.up);

			}

			p.transform.RotateAround(lp.Get_Pos(Current_Connection),Vector3.up,angle);

			Debug.Log ("LastCon: " +Last_Connection+ " Cur Con: " + Current_Connection + " Next_con: " + Next_Conntection + " Piece: " + i + " angle: " + angle);

			Last_Connection = Next_Conntection;
			Last_ConnectionPosition = lp.Get_Pos (Next_Conntection);
			Last_lp = lp;
		}

	}

	float AngleSigned(Vector3 v1, Vector3 v2, Vector3 norm){
		return Mathf.Atan2 (Vector3.Dot(norm,Vector3.Cross(v1,v2)),Vector3.Dot(v1,v2)) * Mathf.Rad2Deg;
	}

	/*
	void GenerateLevel(){
		Random.seed = 1234;
		int ii;
		int Connect;
		int lastconnect = 0;
		int[] co = new int[]{ 0, 1, 2, 3 };
		List<int> ConnectLeft = new List<int> (co);
		Vector3 LastPos = Vector3.zero;
		Vector3 LastDir = Vector3.zero;
		GameObject p = null;
		LevelPiece lp;
		float angle = 0;
		float lastangle = 0;

		int name = 0;
		for (int i = 0; i < 15; i++) {
			ii = Random.Range (0, Dangerous_Pieces.Count);
			p = Instantiate (Dangerous_Pieces [ii].gameObject, LastPos, Quaternion.identity) as GameObject;
			lp = p.GetComponent<LevelPiece> ();
			p.name = "Piece " + name;

			ConnectLeft = new List<int>(co);

			Connect = Random.Range (0, lp.NumberOfSides);		// Pick a side
			ConnectLeft.Remove (Connect);

			int CL = ConnectLeft [Random.Range (0, ConnectLeft.Count)];
			angle = Vector3.Angle (LastDir,lp.Get_Dir(Connect));


			if (Connect == lastconnect && angle == 90) {
				//Debug.LogError ("ah");
				//angle += 90;
			} else {
				
			}

			p.transform.RotateAround (LastPos,Vector3.up,angle);
			 	

			Vector3 test = (p.transform.position - lp.Get_Pos(Connect));


			p.transform.position = LastPos + test;


			Debug.Log ("Piece " + name + " | Side: " + Connect + " | NextSide: " + CL + " | angle: " + angle);

			lastconnect = CL;
			LastPos = lp.Get_Pos (CL);	// Pick from sides left
			LastDir = p.transform.position - LastPos;
			lastangle = angle;
			name++;
		}
	}
	*/
	/*
	void GenerateLevel_Linear(){	// Does not check boundaries
		//GameObject s = Instantiate (StartPiece.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
		int PieceNumber = Random.Range (0,Dangerous_Pieces.Count - 1);
		int W = 0;
		int name = 0;
		Vector3 LastDir = Vector3.back;
		Vector3 LastPos = Vector3.zero;
		for (int i = 0; i < 16; i++) {
			PieceNumber = Random.Range (0,Dangerous_Pieces.Count - 1);
			W = Random.Range(0,4);
			GameObject p = null;
			LevelPiece lp = null;
			float angle = 0;

			switch (W) {
			case 0:	// North
				p = Instantiate (Dangerous_Pieces [PieceNumber].gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				lp = p.GetComponent<LevelPiece> ();


				angle = Vector3.Angle (lp.Get_NorthDir (), LastDir);
				p.transform.RotateAround (LastPos, Vector3.up,-angle);

				p.transform.position = LastPos + (lp.North.transform.position - p.transform.position);

				Debug.Log ("n");
				LastPos = lp.South.transform.position;
				break;
			case 1:	// South
				p = Instantiate (Dangerous_Pieces [PieceNumber].gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				lp = p.GetComponent<LevelPiece> ();

				angle = Vector3.Angle (lp.Get_SouthDir(),LastDir);
				p.transform.RotateAround (LastPos,Vector3.up,-angle);
							
				p.transform.position = LastPos + (lp.South.transform.position - p.transform.position);

				Debug.Log ("s");
				LastPos = lp.North.transform.position;
				break;
			case 2: // East
				p = Instantiate (Dangerous_Pieces [PieceNumber].gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				lp = p.GetComponent<LevelPiece> ();

				angle = Vector3.Angle (lp.Get_EastDir(),LastDir);
				p.transform.RotateAround (LastPos,Vector3.up,-angle);

				p.transform.position = LastPos + (lp.East.transform.position - p.transform.position);

				Debug.Log ("e");
				LastPos = lp.West.position;
				break;
			case 3: // West
				p = Instantiate (Dangerous_Pieces [PieceNumber].gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				lp = p.GetComponent<LevelPiece> ();

				angle = Vector3.Angle (lp.Get_WestDir(),LastDir);
				p.transform.RotateAround (LastPos,Vector3.up,-angle);

				p.transform.position = LastPos + (lp.West.transform.position - p.transform.position);

				Debug.Log ("w");
				LastPos = lp.East.transform.position;
				break;
			}
			p.name = "Piece: " + name;
			name++;
			LastDir =  p.transform.position - lp.West.transform.position;

		}
	}

		Level gen backup
		void GenerateLevel_Linear(){	// Does not check boundaries
		//GameObject s = Instantiate (StartPiece.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
		int PieceNumber = Random.Range (0,Dangerous_Pieces.Count - 1);
		Vector3 LastPos = Vector3.zero;
		for (int i = 0; i < 10; i++) {
			PieceNumber = Random.Range (0,Dangerous_Pieces.Count - 1);
			GameObject p = Instantiate(Dangerous_Pieces[PieceNumber].gameObject,
				LastPos + (Dangerous_Pieces[PieceNumber].transform.position - Dangerous_Pieces[PieceNumber].North.transform.position),
				Quaternion.identity) as GameObject;
			LastPos = p.GetComponent<LevelPiece>().South.transform.position;
		}
	}
	*/

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
			if (Physics.BoxCast (Vector3.zero, Dangerous_Pieces [r].HalfExtents, Dangerous_Pieces [r].Direction)) {
				r = Iterate (r, Dangerous_Pieces.Count - 1);
			} else {
				Fits = true;
			}
		}

		if (r == r_start) {
			return;
		} else {
			// Place Piece
		}

	}

	//--------------------------------------------------------------------------------------------------------------

	int Iterate(int r, int max_r){
		r++;
		if (r > max_r) {
			return r = 0;
		} else {
			return r;
		}
	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------





}	// End of class
