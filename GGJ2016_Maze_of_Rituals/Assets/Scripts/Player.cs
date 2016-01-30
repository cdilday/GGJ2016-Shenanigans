using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//distance player travels in a frame

	public float speed;
	CharacterController characterController;
	GameObject mainCamera;


	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		mainCamera.transform.position = new Vector3 (transform.position.x, transform.position.y, -10);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			if (!Input.GetKey (KeyCode.S) && !Input.GetKey(KeyCode.DownArrow)) {
				//MoveUp
				characterController.Move(new Vector3(0, speed, 0)); 
			}
		} else if (Input.GetKey (KeyCode.S) || Input.GetKey( KeyCode.DownArrow)) {
			//MoveDown
			characterController.Move(new Vector3(0, -1 * speed, 0)); 
		}

		if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			if (!Input.GetKey (KeyCode.D) && !Input.GetKey(KeyCode.RightArrow)) {
				//MoveLeft
				characterController.Move(new Vector3(-1 * speed, 0, 0)); 
			}
		} else if (Input.GetKey (KeyCode.D) || Input.GetKey( KeyCode.RightArrow)) {
			//MoveRight
			characterController.Move(new Vector3(speed, 0, 0)); 
		}
		mainCamera.transform.position = new Vector3 (transform.position.x, transform.position.y, -10);

	}
}
