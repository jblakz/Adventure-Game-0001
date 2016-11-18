using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Figure owner;
	public Image icon;
    private Slider thisHealthBar;
	private float minVisibleHealth = -3f;

	// Use this for initialization
	void Start()
	{
		thisHealthBar = GetComponent<Slider>();
		AssignOwner();
		float maxHealth = owner.maxHealth;
		thisHealthBar.maxValue = maxHealth;
		if (owner.tag == "Player")
			thisHealthBar.minValue = minVisibleHealth;
		else thisHealthBar.minValue = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (owner.enabled)
		{
			thisHealthBar.value = owner.health;
			if (GetComponentInChildren<Text>() != null)
				GetComponentInChildren<Text>().text = owner.health + " / " + owner.maxHealth;
		}
		else ToggleOn(false);
	}
	public void ToggleOn(bool toggle)
	{
		Image[] images = GetComponentsInChildren<Image>();
		foreach (var img in images)
			img.enabled = toggle;
		Text[] text = GetComponentsInChildren<Text>();
		foreach (var txt in text)
			txt.enabled = toggle;

	}
	public void AssignOwner()
	{
		if (owner == null)
			owner = GetComponentInParent<Figure>();
	}
}