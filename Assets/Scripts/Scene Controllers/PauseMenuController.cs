using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
	public GameObject muteButton;
	public GameObject unmuteButton;
	public GameObject hint;
	public AudioClip clickSound;

	private float startVolume;
	private LevelController level;
	private static bool paused;

	public void Awake()
	{
		startVolume = AudioListener.volume;
		level = FindObjectOfType<LevelController>();
	}

	public void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			PauseGame();
	}

	public void TogglePauseMenu(bool sw)
	{
		Image[] childImages = GetComponentsInChildren<Image>();
		foreach (var obj in childImages)
			obj.enabled = sw;
		Text[] childTexts = GetComponentsInChildren<Text>();
		foreach (var obj in childTexts)
			obj.enabled = sw;
	}
	public void ToggleHint(bool sw)
	{
		Image[] childImages = hint.GetComponentsInChildren<Image>();
		foreach (var obj in childImages)
			obj.enabled = sw;
		Text[] childTexts = hint.GetComponentsInChildren<Text>();
		foreach (var obj in childTexts)
			obj.enabled = sw;
	}
	public void PauseGame()
	{
		//Sound on click
		GetComponent<AudioSource>().PlayOneShot(clickSound);

		if (paused)
		{
			paused = false;
			Time.timeScale = 1f;
			TogglePauseMenu(false);
			ToggleHint(false);
		}
		else
		{
			paused = true;
			Time.timeScale = 0;
			TogglePauseMenu(true);
			if(AudioListener.volume == 0f)
			{
				SwitchButton(muteButton, false);
				SwitchButton(unmuteButton, true);
			}
		}
	}
	public void MuteAudio()
	{
		GetComponent<AudioSource>().PlayOneShot(clickSound);
		AudioListener.volume = 0f;
		SwitchButton(muteButton, false);
		SwitchButton(unmuteButton, true);
	}
	public void UnmuteAudio()
	{
		GetComponent<AudioSource>().PlayOneShot(clickSound);
		AudioListener.volume = startVolume;
		SwitchButton(muteButton, true);
		SwitchButton(unmuteButton, false);
	}
	public void ShowTutorial()
	{
		TogglePauseMenu(false);
		ToggleHint(true);
	}
	public void RestartLevel()
	{
		paused = true;
		PauseGame();
		level.ResetLevel();
	}
	public void ToMain()
	{
		paused = true;
		PauseGame();
		SceneManager.LoadScene("MainMenu");
	}
	private void SwitchButton(GameObject button, bool sw)
	{
		button.GetComponent<Image>().enabled = sw;
		button.GetComponent<Button>().enabled = sw;
		button.GetComponentInChildren<Text>().enabled = sw;
	}
}