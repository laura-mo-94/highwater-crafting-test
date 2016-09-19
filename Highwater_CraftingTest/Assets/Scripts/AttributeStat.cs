using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttributeStat : MonoBehaviour {

	Slider slider;
	public Text label;

	// Use this for initialization
	void Awake () {
		slider = GetComponent<Slider> ();
	}

	public void setLabel(string labelText)
	{
		label.text = labelText;
	}

	public void setValue(float val)
	{
		slider.value = val;
	}

}
