using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//distance player travels in a frame
	public bool canMove;
	public float speed;
	public float sightDistance;
	CharacterController characterController;
	GameObject mainCamera;
	Animator animator;
	SpriteRenderer spriteRenderer;
	public Sprite [] defaultSprites;
	public bool [] inventory;


	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		mainCamera.transform.position = new Vector3 (transform.position.x, transform.position.y, -10);
		for (int i = 0; i < 3; i++)
			inventory [i] = false;
	}
	
	// Update is called once per frame
	void Update () {

		bool isMoving = false;
		if (canMove) {
			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
				if (!Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.DownArrow)) {
					//MoveUp
					characterController.Move (new Vector3 (0, speed, 0)); 
					animator.SetBool ("Moving", true);
					isMoving = true;
					animator.SetInteger ("Direction", 0);
				}
			} else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
				//MoveDown
				characterController.Move (new Vector3 (0, -1 * speed, 0)); 
				animator.SetBool ("Moving", true);
				isMoving = true;
				animator.SetInteger ("Direction", 2);
			}

			if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
				if (!Input.GetKey (KeyCode.D) && !Input.GetKey (KeyCode.RightArrow)) {
					//MoveLeft
					characterController.Move (new Vector3 (-1 * speed, 0, 0));  
					animator.SetBool ("Moving", true);
					isMoving = true;
					animator.SetInteger ("Direction", 3);
				}
			} else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
				//MoveRight
				characterController.Move (new Vector3 (speed, 0, 0)); 
				animator.SetBool ("Moving", true);
				isMoving = true;
				animator.SetInteger ("Direction", 1);
			}
		}
		mainCamera.transform.position = new Vector3 (transform.position.x, transform.position.y, -10);
		//animations

		if (!isMoving) {
			animator.SetBool("Moving", false);
			spriteRenderer.sprite = defaultSprites[animator.GetInteger("Direction")];
		}
	}
}
