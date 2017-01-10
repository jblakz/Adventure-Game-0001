using UnityEngine;

using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelExit : MonoBehaviour
{

	public int levelNumber;
	private JumpButton jumpButton;
	private TipPaneController tipPane;

	private bool isReached;

	void Start()
	{
		isReached = false;
		jumpButton = FindObjectOfType<JumpButton>();
		tipPane = FindObjectOfType<TipPaneController>();
	}

	void Update()
	{
		if (isReached)
		{
			string msgString = "Press 'Jump' to go to next level";
			tipPane.ShowMessage(msgString, 3f);
			if (Input.GetKey(KeyCode.Space) || jumpButton.Pressed())
			{
				if (levelNumber == 3)
				{
					if (PlayerPrefs.GetInt("playableLevel") <= levelNumber)
						PlayerPrefs.SetInt("playableLevel", levelNumber);
					SceneManager.LoadScene("About");
				}
				else
				{
					if (PlayerPrefs.GetInt("playableLevel") <= levelNumber)
						PlayerPrefs.SetInt("playableLevel", levelNumber + 1);
					SceneManager.LoadScene("Level " + (levelNumber + 1));
				}
			}
		}
	}
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag == "Player")
		{
			isReached = true;
		}
	}
	void OnTriggerExit2D(Collider2D target)
	{
		if (target.tag == "Player")
		{
			isReached = false;
		}
	}
}