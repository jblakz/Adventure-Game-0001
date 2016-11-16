﻿using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Figure owner;
	private Slider thisHealthBar;
	private float minVisibleHealth = -3f;

	// Use this for initialization
	void Start()
	{
		thisHealthBar = GetComponent<Slider>();
		Refresh();
	}
	public void Refresh()
	{
		if (owner != null && owner.enabled)
		{
			ToggleOn(true);
			float maxHealth = owner.maxHealth;
			thisHealthBar.maxValue = maxHealth;
			thisHealthBar.minValue = minVisibleHealth;
		}
		//whether there's not enough figure or owner has died
		else ToggleOn(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (owner != null)
		{
			Refresh();
			thisHealthBar.value = owner.health;
			if (GetComponentInChildren<Text>() != null)
				GetComponentInChildren<Text>().text = owner.health + " / " + owner.maxHealth;
		}
		else Refresh();
	}
	public void ToggleOn(bool toggle)
	{
		Image[] images = GetComponentsInChildren<Image>();
		foreach (var img in images)
			img.enabled = toggle;
	}
}