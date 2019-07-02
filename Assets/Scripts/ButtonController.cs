using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour {

	private Button button;
	private Image image;
	private TextMeshProUGUI text;

	private Sprite startSprite;
	private Vector3 startOffset;

	private bool raised = true;
	private bool mouseDown = false;

	void Awake() {
		button = GetComponent<Button>();
		image = GetComponent<Image>();
		text = GetComponentInChildren<TextMeshProUGUI>();
		
		startSprite = image.sprite;
		startOffset = text.transform.localPosition;
	}

	void OnMouseDown() {
		mouseDown = true;

		Pressed();
	}

	void OnMouseUp() {
		mouseDown = false;

		Unpressed();
	}

	void OnMouseExit() {
		Unpressed();
	}

	void OnMouseEnter() {
		if(mouseDown) {
			Pressed();
		}
	}

	public void Pressed() {
		raised = false;
		text.transform.localPosition = Vector3.zero;
	}

	public void Unpressed() {
		if(button.interactable) {
			raised = true;
			text.transform.localPosition = startOffset;
		}
	}

	public Button GetButton() {
		return button;
	}

	public void SetText(string t) {
		text.text = t;
	}

	public void Enable() {
		button.interactable = true;

		text.transform.localPosition = startOffset;
		text.color = Color.white;
	}

	public void Disable() {
		button.interactable = false;

		text.transform.localPosition = Vector3.zero;
		text.color = Color.Lerp(Color.white, Color.black, 0.5f);
	}
}
