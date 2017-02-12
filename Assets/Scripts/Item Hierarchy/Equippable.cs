using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose: Hold attributes for items that can be equipped
*/
public abstract class Equippable : Item {

	//============================================[Unity Functions]=================================================

	[Header("Equippable Stats")]
	public int Item_Int	= 0;
	public int Item_Dex	= 0;
	public int Item_Str	= 0;

	public int Equip_Spot = 0;
	/*
	0 - Head
	1 - Chest
	2 - Legs
	3 - Back
	4 - Feet
	5 - Hands
	6 - Neck
	7 - Finger	
	*/

	//===============================================[Functions]====================================================

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------


}	// End of class
