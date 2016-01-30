using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

	public int type;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	public void giveType(int newType){
		type = newType;
		switch (type) {
		case 0:
			spriteRenderer.color = Color.red;
			name = "Red Item";
			break;
		case 1:
			spriteRenderer.color = Color.green;
			name = "Green Item";
			break;
		case 2:
			spriteRenderer.color = Color.blue;
			name = "Blue Item";
			break;
		}
	}
}
