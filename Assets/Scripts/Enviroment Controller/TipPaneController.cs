using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TipPaneController : MonoBehaviour
{
	public bool msgShown;

	void Start()
	{
		msgShown = false;
	}

	void Update()
	{

	}

	public void ShowMessage(string msgString, float timeToFade)
	{
		if (msgShown == false && msgString != "")
		{
			msgShown = true;
			GetComponent<Image>().enabled = true;
			GetComponentInChildren<Text>().text = msgString;
			GetComponentInChildren<Text>().enabled = true;
			if (timeToFade >= 0f)
				StartCoroutine(CloseMessageCo(timeToFade));
		}
	}
	public IEnumerator CloseMessageCo(float delay)
	{
		yield return new WaitForSeconds(delay);
		GetComponent<Image>().enabled = false;
		GetComponentInChildren<Text>().text = "";
		GetComponentInChildren<Text>().enabled = false;
		msgShown = false;
		StopAllCoroutines();
	}

}