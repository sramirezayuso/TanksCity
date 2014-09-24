using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {

	private bool isPaused = false;
	public Texture2D resumeTexture;
	
	void Update () {
		if(Input.GetKeyDown("escape") && !isPaused)
		{
			print("Puased");
			Time.timeScale = 0.0f;
			isPaused = true;
		}
		else if(Input.GetKeyDown("escape") && isPaused)
		{
			print("Unpuased");
			Time.timeScale = 1.0f;
			isPaused = false;    
		}
	}
	//Create and check if the buttons are being pressed.
	void  OnGUI(){
		if(isPaused)
		{
			if(GUI.Button (new Rect(Screen.width-200,0,200,95), "Continue"))
			{
				Time.timeScale = 1.0f;
				isPaused = false;
			}
			if(GUI.Button (new Rect(Screen.width-200,95,200,95), "Restart"))
			{
				Application.LoadLevel(Application.loadedLevelName);
				Time.timeScale = 1.0f;
				isPaused = false;
			}
			if(GUI.Button (new Rect(Screen.width-200,190,200,95), "Main Menu"))
			{
				Application.LoadLevel("MainMenu");
				Time.timeScale = 1.0f;
				isPaused = false;
			}
			if(GUI.Button (new Rect(Screen.width-200,285,200,95), "Quit"))
			{
				Application.Quit();
			}
		}
	}
}
