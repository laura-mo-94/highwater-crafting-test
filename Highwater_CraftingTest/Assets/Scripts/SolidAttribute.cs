using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolidAttribute : ObjectAttribute {

	public float flexibility;
	public float durability;
	public float elasticity;
	public float stickiness;
	public float thickness;
	public float sharpness;

	float sharpenLowerThreshold = 0.2f;
	float sharpenUpperThreshold = 0.7f;
	float weaveThreshold = 0.8f;

	Dictionary<string, bool> actionCompleted;

	public override void ReadyAttribute()
	{
		attributes = new Dictionary<string, float> ();
		actionCompleted = new Dictionary<string, bool> ();

		attributes.Add ("flexibility", flexibility);
		attributes.Add ("durability", durability);
		attributes.Add ("elasticity", elasticity);
		attributes.Add ("stickiness", stickiness);
		attributes.Add ("sharpness", sharpness);
		attributes.Add ("thickness", thickness);
		actionCompleted.Add ("Weave", false);
		actionCompleted.Add ("Sharpen", false);
	}
		
	public override Dictionary<string, List<Action>> GetPossibleActions()
	{
		Dictionary<string, List<Action>> possibleActions = new Dictionary<string, List<Action>> ();
	
		if (!possibleActions.ContainsKey (defaultCategoryName)) 
		{
			possibleActions.Add (defaultCategoryName, new List<Action> ());
		}

		if (component != null) 
		{
			possibleActions = component.GetPossibleActions ();
		}

		if (flexibility < sharpenLowerThreshold && elasticity < sharpenLowerThreshold && durability > sharpenUpperThreshold && !actionCompleted["Sharpen"]) 
		{
			ActionsPerformed sharpen = new ActionsPerformed (Sharpen);
			Action act = new Action ("Sharpen", sharpen);
			possibleActions [defaultCategoryName].Add (act);
		}

		if (flexibility > weaveThreshold && !actionCompleted["Weave"]) 
		{
			possibleActions.Add("Weave", new List<Action>());

			ActionsPerformed wRope = new ActionsPerformed (WeaveRope);
			Action weaveRope = new Action ("Weave Rope", wRope);
			possibleActions["Weave"].Add (weaveRope);

			ActionsPerformed wBasket = new ActionsPerformed (WeaveBasket);
			Action weaveBasket = new Action ("Weave Basket", wBasket);
			possibleActions["Weave"].Add (weaveBasket);
		}

		return possibleActions;
	}

	public void WeaveRope(Dictionary<string, float> variables)
	{
		string name;
		GameObject baseOb = this.gameObject;
		BaseObject baseObComponent = baseOb.GetComponent<BaseObject> ();

		if (thickness > 0.5f) 
		{
			name = baseObject.objectName + " Thread";
		} 
		else 
		{
			name = baseObject.objectName + " Rope";
		}

		baseOb.name =  name;
		baseObject.changeName(name);
		baseObject.tags.Add ("Rope");
		durability = durability * elasticity / 2f;

		actionCompleted ["Weave"] = true;
		thickness = thickness * 4f;

		baseObComponent.RemoveAttributes (new List<string> () {GetType ().Name}, true);
	}

	public void WeaveBasket(Dictionary<string, float> variables)
	{
		string name;
		GameObject baseOb = this.gameObject;

		name = baseObject.objectName + " Basket";
		durability = durability  * elasticity / 2f;
		thickness = thickness * 2f;

		actionCompleted ["Weave"] = true;

		baseOb.name = name;
		baseObject.changeName(name);

		baseOb.GetComponent<BaseObject> ().RemoveAttributes (new List<string> (){GetType ().Name}, true);
	}

	public void Sharpen(Dictionary<string, float> variables)
	{
		if (sharpness > 3f) 
		{
			if (!this.gameObject.name.Contains ("Blade")) 
			{
				this.gameObject.name = this.gameObject.name + " Blade";
				GetComponent<BaseObject> ().changeName (this.gameObject.name);
			}
		} 
		else 
		{
			this.gameObject.name = "Sharpened " + this.gameObject.name;
			GetComponent<BaseObject> ().changeName (this.gameObject.name);
		}

		sharpness = sharpness * 1.5f;
		actionCompleted ["Sharpen"] = true;
		attributes ["sharpness"] = sharpness;
	}
}
