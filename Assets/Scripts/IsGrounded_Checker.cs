using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded_Checker : MonoBehaviour {

	RaycastHit h;
	Ray r;

	int NumberOfExtraRays = 8;
	float Radius = 0.25f;
	bool IsGrounded = false;

	void Update(){
		r = new Ray (transform.position + Vector3.up * 0.1f,Vector3.down);
		IsGrounded = false;

		if (Physics.Raycast (r, out h)) {
			if (h.distance < 0.11f) {
				Debug.DrawRay (transform.position,r.direction,Color.green);
				IsGrounded = true;
			} else {
				Debug.DrawRay (transform.position,r.direction,Color.red);
				//IsGrounded = false;
			}
		} else {
			Debug.DrawRay (transform.position,r.direction,Color.blue);
			//IsGrounded = false;
		}

		for (int i = 0; i < NumberOfExtraRays; i++) {
			float x = transform.position.x + Radius * Mathf.Cos (2 * Mathf.PI * i / NumberOfExtraRays);
			float z = transform.position.z + Radius * Mathf.Sin (2 * Mathf.PI * i / NumberOfExtraRays);
			Vector3 NewPos = new Vector3 (x, transform.position.y + 0.2f, z);

			r = new Ray (NewPos,Vector3.down);

			if (Physics.Raycast (r, out h)) {
				if (h.distance < 0.22f) {
					Debug.DrawRay (NewPos,r.direction,Color.green);
					IsGrounded = true;
				} else {
					Debug.DrawRay (NewPos,r.direction,Color.red);
					//IsGrounded = false;
				}
			} else {
				Debug.DrawRay (NewPos,r.direction,Color.blue);
				//IsGrounded = false;
			}

		}


		transform.root.GetComponent<Player> ().IsGrounded = IsGrounded;
	}

}
