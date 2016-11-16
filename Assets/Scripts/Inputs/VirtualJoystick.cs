using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

	private Image bgImg;
	private Image jsImg;
	private Vector3 inputVector;

	// Use this for initialization
	void Start()
	{
		bgImg = GetComponent<Image>();
		jsImg = transform.GetChild(0).GetComponent<Image>();

	}

	// Update is called once per frame
	void Update()
	{

	}

	public virtual void OnDrag(PointerEventData peData)
	{
		Vector2 pos;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform,
			peData.position,
			peData.pressEventCamera,
			out pos))
		{
			pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
			pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

			inputVector = new Vector3(pos.x, 0, pos.y);
			inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

			//Move Joystick
			jsImg.rectTransform.anchoredPosition =
				new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3),
				inputVector.z * (bgImg.rectTransform.sizeDelta.y / 3));
		}

	}

	public virtual void OnPointerDown(PointerEventData peData)
	{
		OnDrag(peData);
	}
	public virtual void OnPointerUp(PointerEventData peData)
	{
		inputVector = Vector3.zero;
		jsImg.rectTransform.anchoredPosition = Vector3.zero;
	}
	public float Horizontal()
	{
		if (inputVector.x != 0)
			return inputVector.x;
		else return Input.GetAxis("Horizontal");
	}
	public float Vertical()
	{
		if (inputVector.z != 0)
			return inputVector.z;
		else return Input.GetAxis("Vertical");
	}
}
