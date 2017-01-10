using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuController : MonoBehaviour
{
	private int playableLevel;
	private Button level2Select;
	private Button level3Select;
	void Awake()
	{
		playableLevel = PlayerPrefs.GetInt("playableLevel");
		level2Select = GameObject.Find("Level 2").GetComponent<Button>();
		level3Select = GameObject.Find("Level 3").GetComponent<Button>();
		EnableLevelSelect();
    }
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			ToMain();
	}
	public void ToLevel(int level)
	{
		SceneManager.LoadScene("Level " + level.ToString());
	}
	public void ToMain()
	{
		SceneManager.LoadScene("MainMenu");
	}
	private void EnableLevelSelect()
	{
		if (playableLevel == 3)
		{
			level2Select.interactable = true;
			level3Select.interactable = true;
		}
		if (playableLevel == 2)
		{
			level2Select.interactable = true;
			level3Select.interactable = false;
		}
		if (playableLevel == 1)
		{
			level2Select.interactable = false;
			level3Select.interactable = false;
		}
	}
}