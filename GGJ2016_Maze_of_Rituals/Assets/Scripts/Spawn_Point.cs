using UnityEngine;
using System.Collections;

public class Spawn_Point : MonoBehaviour {
	//red, green, blue
	public Place_Point [] place_points;

	GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "Player"){
			if(place_points [0].placed && place_points [1].placed && place_points [2].placed && !gameController.hasWon) {
				gameController.PlayerWon();
			}
			else{
				Debug.Log ("Find the objects!");
			}
		}
	}
}
