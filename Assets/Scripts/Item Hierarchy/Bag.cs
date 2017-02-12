using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Class Purpose : have an item that stores other items
*/
public class Bag : Equippable {

	//===============================================[Variables]====================================================

	public List<Item> Bag_Inventory = new List<Item>();

	public float Bag_CurrentWeight{
		get{ 
			float a = 0f;
			foreach (Item i in Bag_Inventory) {
				a += i.Item_Weight;
			}
			return a;
		}
	}

	public int Bag_Current_Items{
		get{ 
			return Bag_Inventory.Count;
		}
	}

	//--------------------------------------------------------------------------------------------------------------

	public Bag(){
		Item_Int = 0;
		Item_Dex = 0;
		Item_Str = 0;
		Equip_Spot = 3;
		Item_Weight = 5f;
		Item_Name = "Basic Bag";
		Item_Value = 200;
	}

	//--------------------------------------------------------------------------------------------------------------

	public Bag(string n, int v, float w){
		Item_Int = 0;
		Item_Dex = 0;
		Item_Str = 0;
		Equip_Spot = 3;
		Item_Weight = w;
		Item_Name = n;
		Item_Value = v;
	}

	//============================================[Unity Functions]=================================================


	//===============================================[Functions]====================================================

	public void AddItem(Item i){
		Bag_Inventory.Add (i);
	}

	//--------------------------------------------------------------------------------------------------------------

	public void RemoveItem(Item i){
		Bag_Inventory.Remove (i);
	}

	//--------------------------------------------------------------------------------------------------------------

	public void RemoveItemAtLocation(int i){
		Bag_Inventory.RemoveAt (i);
	}

	//==============================================================================================================	

	//--------------------------------------------------------------------------------------------------------------

}	// End of Class
