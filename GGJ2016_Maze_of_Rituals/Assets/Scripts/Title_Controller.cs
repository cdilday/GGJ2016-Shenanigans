using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Title_Controller : MonoBehaviour {

	public Text desc;
	public Text HSText;

	public Text widthText;
	public Text heightText;

	public Slider width;
	public Slider height;

	void Start(){
		width.onValueChanged.AddListener(WidthChange);
		height.onValueChanged.AddListener(HeightChange);
		UpdateHSText ();
	}

	public void Press_Start(){
		PlayerPrefs.SetInt ("Width", (int) width.value);
		PlayerPrefs.SetInt ("Height", (int) height.value);
		desc.text = "Generating Maze";
		Application.LoadLevel (1);
	}

	public void WidthChange(float value){
		widthText.text = "Width: " + (int) value;
		UpdateHSText ();
	}

	public void HeightChange(float value){
		heightText.text = "Height: " + (int) value;
		UpdateHSText ();
	}

	void UpdateHSText(){
		float time = PlayerPrefs.GetFloat ((int)width.value + " " + (int)height.value + " score", 0);
		if (time == 0) {
			HSText.text = "Fastest time for size: Not completed";
		} else {
			int minutes = Mathf.FloorToInt(time / 60F);
			int seconds = Mathf.FloorToInt(time - minutes * 60);
			string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
			HSText.text = "Fastest time for size: " + niceTime;
		}
	}
}
