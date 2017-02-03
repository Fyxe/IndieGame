using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded_Checker : MonoBehaviour {

	RaycastHit h;
	Ray r;

	bool IsGrounded = false;

	void Update(){
		r = new Ray (transform.position + Vector3.up * 0.1f,Vector3.down);

		if (Physics.Raycast (r, out h)) {
			if (h.distance < 0.11f) {
				Debug.DrawRay (transform.position,r.direction,Color.green);
				IsGrounded = true;
			} else {
				Debug.DrawRay (transform.position,r.direction,Color.red);
				IsGrounded = false;
			}
		} else {
			Debug.DrawRay (transform.position,r.direction,Color.blue);
			IsGrounded = false;
		}

		transform.root.GetComponent<Player> ().IsGrounded = IsGrounded;
	}

}
