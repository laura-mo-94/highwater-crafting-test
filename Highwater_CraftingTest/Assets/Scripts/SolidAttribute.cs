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

	Dictionary<string, int> actionsCompleted;

	public override void ReadyAttribute()
	{
		attributes = new Dictionary<string, float> ();

		attributes.Add ("flexibility", flexibility);
		attributes.Add ("durability", durability);
		attributes.Add ("elasticity", elasticity);
		attributes.Add ("stickiness", stickiness);

		 actionsCompleted = new Dictionary<string, int> ();
		actionsCompleted.Add ("Weave", 0);
		actionsCompleted.Add ("Sharpen", 0);
	}
		
	public override Dictionary<string, ActionsPerformed> GetPossibleActions()
	{
		Dictionary<string, ActionsPerformed> possibleActions = new Dictionary<string, ActionsPerformed> ();
		if (component != null) 
		{
			possibleActions = component.GetPossibleActions ();
		}

		if (flexibility < sharpenLowerThreshold && elasticity < sharpenLowerThreshold && durability > sharpenUpperThreshold && actionsCompleted["Sharpen"] < 5) 
		{
			ActionsPerformed sharpen = new ActionsPerformed (Sharpen);
			possibleActions.Add ("Sharpen", Sharpen);
		}

		if (flexibility > weaveThreshold && actionsCompleted["Weave"] > 0) 
		{
			ActionsPerformed weave = new ActionsPerformed (Weave);
			possibleActions.Add ("Weave", Weave);
		}



		return possibleActions;
	}

	public void Weave(Dictionary<string, float> variables)
	{
		string name;
		float weaveType = variables ["weaveType"];

		GameObject baseOb = this.gameObject;


		if (weaveType < 5f) 
		{
			if (thickness > 0.5f) 
			{
				name = "Thread";
			} 
			else 
			{
				name = "Rope";
			}

			baseOb.name = baseObject.objectName + " " + name;
			durability = durability * weaveType * elasticity / 2f;
			thickness = thickness * 4f;
		} 
		else if (weaveType < 10f) 
		{
			name = "Basket";
			baseOb.name = baseObject.objectName + " " + name;
			durability = durability * (weaveType - 4f) * elasticity / 2f;
			thickness = thickness * 2f;
		}

		actionsCompleted ["Weave"] = actionsCompleted ["Weave"] + 1;
			
	}

	public void Sharpen(Dictionary<string, float> variables)
	{
		if (sharpness > 3f) 
		{
			if (!this.gameObject.name.Contains ("Blade")) 
			{
				this.gameObject.name = this.gameObject.name + " Blade";
			}
		}

		sharpness = sharpness * 1.2f;

		actionsCompleted ["Sharpen"] = actionsCompleted ["Sharpen"] + 1;
		attributes ["sharpness"] = sharpness;
	}
}
