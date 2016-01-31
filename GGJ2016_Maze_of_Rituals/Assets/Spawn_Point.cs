using UnityEngine;
using System.Collections;

public class Spawn_Point : MonoBehaviour {
	//red, green, blue
	public Place_Point [] place_points;

	// Use this for initialization
	void Start () {
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "Player"){
			if(place_points [0].placed && place_points [0].placed && place_points [0].placed) {
				Debug.Log ( "Player Wins!");
			}
			else{
				Debug.Log ("Find the objects!");
			}
		}
	}
}
