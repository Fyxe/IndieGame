using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose : Creating a class to store a tiered currency
*/
public class Currency {

	//===============================================[Variables]====================================================

	public int Pocket;

	//============================================[Unity Functions]=================================================

	//===============================================[Functions]====================================================

	public void Add(int amount){
		Pocket += amount;
	}

	public void Subtract(int amount){
		Pocket -= amount;
	}

	public List<int> GetCoinage(){
		List<int> temp = new List<int> ();
		int tempint = Pocket;
		for (int i = 0; i < 4; i++) {			
			if (i == 3) {				
				temp.Add (tempint);
				tempint /= 100;
			} else {
				temp.Add (tempint % 100);
				tempint /= 100;
			}

		}
		foreach (var item in temp) {
			Debug.Log (item);
		}
		return temp;
	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------

}
