using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantAttribute : ObjectAttribute {

	public float waterContent;
	public float toughness;
	public float sweet;
	public float bitter;
	public float sour;
	public float salty;
	public float spicy;

	public override void ReadyAttribute()
	{
		attributes = new Dictionary<string, float> ();

		attributes.Add ("waterContent", waterContent);
		attributes.Add ("toughness", toughness);
		attributes.Add ("sweet", sweet);
		attributes.Add ("bitter", bitter);
		attributes.Add ("sour", sour);
		attributes.Add ("salty", salty);
		attributes.Add ("spicy", spicy);
	}

	public override Dictionary<string, ActionsPerformed> GetPossibleActions()
	{
		Dictionary<string, ActionsPerformed> possibleActions = new Dictionary<string, ActionsPerformed> ();
		if (component != null) 
		{
			possibleActions = component.GetPossibleActions ();
		}


		ActionsPerformed cook = new ActionsPerformed(Cook);
		ActionsPerformed dry = new ActionsPerformed (Dry);

		possibleActions.Add ("Cook", cook);
		possibleActions.Add ("Dry", dry);

		return possibleActions;
	}

	public void Cook(Dictionary<string, float> variables)
	{
		Debug.Log ("Cook");
		toughness = toughness / 2f;
		waterContent = waterContent * 2f;

		attributes ["toughness"] = toughness;
		attributes ["waterContent"] = waterContent;
	}

	public void Dry(Dictionary<string, float> variables)
	{
		waterContent = waterContent / 4f;
		attributes ["waterContent"] = waterContent;
	}

}
