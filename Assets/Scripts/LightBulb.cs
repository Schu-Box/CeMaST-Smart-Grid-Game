using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightBulb", menuName = "Light Bulb", order = 1)]
public class LightBulb : ScriptableObject {
	public string typeName;
	public int hoursBeforeBurnout;
	public int cost;
	public int kwh;
	public Sprite litSprite;
	public Sprite unlitSprite;
	public Sprite glowSprite;
}
