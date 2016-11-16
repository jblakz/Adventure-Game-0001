using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KunaiManager : MonoBehaviour
{

	public static int number;
	Text text;

	void Start()
	{
		text = GetComponent<Text>();
		number = 0;
	}
	void Update()
	{
		if (number < 0)
			number = 0;
		text.text = "" + number;
	}
	public static void AddNumbers(int point)
	{
		number += point;
	}
	public static void Reset()
	{
		number = 0;
	}
}