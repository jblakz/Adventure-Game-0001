using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class Button : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

	private bool isPressed = false;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public virtual void OnPointerDown(PointerEventData data)
	{
		isPressed = true;
	}
	public virtual void OnPointerUp(PointerEventData data)
	{
		isPressed = false;
	}
	public bool Pressed()
	{
		return isPressed;
	}
}
