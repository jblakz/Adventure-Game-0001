using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class UIButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

	private bool isPressed = false;
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
