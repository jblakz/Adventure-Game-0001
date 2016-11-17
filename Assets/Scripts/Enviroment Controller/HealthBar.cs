using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Figure owner;
	public Image icon;
    private Slider thisHealthBar;
	private float minVisibleHealth = 0f;

	// Use this for initialization
	void Start()
	{
		thisHealthBar = GetComponent<Slider>();
		AssignOwner();
		Refresh();
	}
	public void Refresh()
	{
		if (owner != null && owner.enabled)
		{
			ToggleOn(true);
			float maxHealth = owner.maxHealth;
			thisHealthBar.maxValue = maxHealth;
			if (owner.tag == "Player")
				thisHealthBar.minValue = minVisibleHealth;
			else thisHealthBar.minValue = 0f;
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
		Text[] text = GetComponentsInChildren<Text>();
		foreach (var txt in text)
			txt.enabled = toggle;
	}
	public void AssignOwner()
	{
		if (owner == null)
			owner = GetComponentInParent<Figure>();
	}
	public void Reposition(int number)
	{
		Vector3 tempVector = transform.position;
		tempVector.y = tempVector.y + (number - 1) * 40f;
		transform.position = tempVector;
	}
}