using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//distance player travels in a frame

	public float speed;
	public float sightDistance;
	CharacterController characterController;
	GameObject mainCamera;
	Animator animator;
	SpriteRenderer spriteRenderer;
	public Sprite [] defaultSprites;


	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		mainCamera.transform.position = new Vector3 (transform.position.x, transform.position.y, -10);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//isn't moving
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
		//animations
		if (characterController.velocity.x < speed && characterController.velocity.x > -1 * speed &&
			characterController.velocity.y < speed && characterController.velocity.y > -1 * speed) {
			animator.SetBool ("Moving", false);
			spriteRenderer.sprite = defaultSprites [animator.GetInteger ("Direction")];
		} else {
			animator.SetBool("Moving", true);
			if(characterController.velocity.x < 0)
				animator.SetInteger("Direction", 3);
			else
				animator.SetInteger("Direction", 1);

			if(characterController.velocity. y < 0)
				animator.SetInteger("Direction", 2);
			else
				animator.SetInteger("Direction", 0);
		}
	}
}
