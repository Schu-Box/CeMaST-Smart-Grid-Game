using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatSlider : MonoBehaviour {

	public Slider mainSlider;
	public GameObject mainFillArea;
	public Slider secondSlider;
	public GameObject secondFillArea;
	public GameObject background;

	private float maxRectHeight;

	public TextMeshProUGUI capText;
	public TextMeshProUGUI midText;
	public TextMeshProUGUI minText;
	public TextMeshProUGUI finalAmountText;

	public bool moneySlider = true;

	public Color fillColor; //Color of remaining slider section
	public Color backgroundColor;
	public Color secondBackgroundColor;

	void Start() {
		maxRectHeight = background.GetComponent<RectTransform>().rect.height;
	}

	public void SetSlider(int maxValue, int minValue, int startValue, int firstSubtraction, int secondSubtraction) {
		//Change background/fill top rect size value 
		float endValue = startValue - firstSubtraction - secondSubtraction;
		if(moneySlider) {
			capText.text = "$" + maxValue;
			midText.text = "$" + (((maxValue - minValue) / 2) + minValue);
			minText.text = "$" + minValue;
			finalAmountText.text = "$" + endValue + " remaining";
		} else {
			capText.text = maxValue + " kWh";
			midText.text = (((maxValue - minValue) / 2) + minValue) + " kWh";
			minText.text = minValue + " kWh";
			finalAmountText.text = endValue + " kWh used";
		}

		float startingSliderPoint = ((float)startValue - (float)minValue) / ((float)maxValue - (float)minValue);
		Vector2 offset = new Vector2(0, -(maxRectHeight - (maxRectHeight * startingSliderPoint)));
		
		RectTransform backgroundRect = background.GetComponent<RectTransform>();
		RectTransform mainFillAreaRect = mainFillArea.GetComponent<RectTransform>();
		backgroundRect.offsetMax = offset;
		mainFillAreaRect.offsetMax = offset;

		RectTransform secondFillAreaRect = secondFillArea.GetComponent<RectTransform>();
		secondFillAreaRect.offsetMax = offset;

		//fillArea.GetComponentInChildren<Image>().color = fillColor;
		/*
		if(change > 0) { //If the money increased
			background.GetComponent<Image>().color = increaseColor;
			//slider.value = ((float)lastMonthValue + (float)change) / (float)lastMonthValue; //THIS IS WRONG!
		} else if (change == 0) { //If it stayed the same
			Debug.Log("Value didn't change this week");
		} else { //Value decreased
			background.GetComponent<Image>().color = decreaseColor;
			slider.value = ((float)thisMonthValue) / (float)lastMonthValue;
		}
		*/

		float mainValue = ((float)startValue - (float)firstSubtraction - (float)minValue) / ((float)startValue - (float)minValue);
		mainSlider.value = mainValue;

		float secondValue = ((float)startValue - (float)firstSubtraction - (float)secondSubtraction - (float)minValue) / ((float)startValue - (float)minValue);
		secondSlider.value = secondValue;
	}
}
