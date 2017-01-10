using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionMenuController : MonoBehaviour {

	public GameObject muteButton;
	public GameObject unmuteButton;
	public AudioClip clickSound;

	private float startVolume;

	public void ToMain()
	{
		SceneManager.LoadScene("MainMenu");
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
	public void ToAbout()
	{
		GetComponent<AudioSource>().PlayOneShot(clickSound);
		SceneManager.LoadScene("About");
	}
	// Use this for initialization
	void Start () {
		startVolume = AudioListener.volume;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Escape))
			ToMain();
	}
	private void SwitchButton(GameObject button, bool sw)
	{
		button.GetComponent<Image>().enabled = sw;
		button.GetComponent<Button>().enabled = sw;
		button.GetComponentInChildren<Text>().enabled = sw;
	}
}
