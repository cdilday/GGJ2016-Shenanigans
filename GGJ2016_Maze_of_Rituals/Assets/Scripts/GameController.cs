using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Player player;
	public Maze_Generator mazeHandler;
	public Text countdownText;
	public Text timerText;

	public GameObject MMbutton;

	public bool hasWon = false;

	float startTime;


	// Use this for initialization
	void Start () {
		MMbutton.SetActive (false);
		player.canMove = false;
		startTime = 0;
		mazeHandler.width = PlayerPrefs.GetInt ("Width", 15);
		mazeHandler.height = PlayerPrefs.GetInt ("Height", 15);
		StartCoroutine(mazeHandler.GenerateMaze ());
		StartCoroutine (Countdown ());
	}

	void Update(){
		if(!hasWon && startTime != 0){
			float timer = Time.time - startTime;
			int minutes = Mathf.FloorToInt(timer / 60F);
			int seconds = Mathf.FloorToInt(timer - minutes * 60);
			string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

			timerText.text = niceTime;
		}
	}

	IEnumerator Countdown(){
		countdownText.text = "";
		yield return new WaitForSeconds (1);
		countdownText.text = "3";
		yield return new WaitForSeconds (1);
		countdownText.text = "2";
		yield return new WaitForSeconds (1);
		countdownText.text = "1";
		yield return new WaitForSeconds (1);
		countdownText.text = "GO";
		startTime = Time.time;
		player.canMove = true;
		yield return new WaitForSeconds (1);
		countdownText.text = "";
	}

	public void PlayerWon(){
		hasWon = true;
		countdownText.text = "Ritual Completed!";
		float bestTime = PlayerPrefs.GetFloat (mazeHandler.width + " " + mazeHandler.height + " score", 0);
		if (bestTime == 0 || Time.time - startTime < bestTime) {
			countdownText.text ="New best time for size!";
			PlayerPrefs.SetFloat (mazeHandler.width + " " + mazeHandler.height + " score", Time.time - startTime);
		}
		MMbutton.SetActive (true);
	}

	public void MainMenu(){
		Application.LoadLevel (0);
	}
}
