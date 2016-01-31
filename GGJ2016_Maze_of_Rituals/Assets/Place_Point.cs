using UnityEngine;
using System.Collections;

public class Place_Point : MonoBehaviour {

	public bool placed;
	public int type;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		placed = false;
		spriteRenderer = GetComponent <SpriteRenderer> ();
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "Player" && other.GetComponent<Player>().inventory[type] == true){
			other.GetComponent<Player>().inventory[type] = false;
			//TODO: play place sound
			switch(type){
			case 0:
				spriteRenderer.color = Color.red;
				break;
			case 1:
				spriteRenderer.color = Color.green;
				break;
			case 2:
				spriteRenderer.color = Color.blue;
				break;
			}
			placed = true;
		}
	}
}
