using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantAttribute : ObjectAttribute 
{
	public float waterContent;
	public float toughness;
	public float sweet;
	public float bitter;
	public float sour;
	public float salty;
	public float spicy;

	public float stomachEffect;
	public float pneumoniaEffect;

	Dictionary<string, bool> actionCompleted;

	public override void ReadyAttribute()
	{
		attributes = new Dictionary<string, float> ();
		actionCompleted = new Dictionary<string, bool> ();

		attributes.Add ("waterContent", waterContent);
		attributes.Add ("toughness", toughness);
		attributes.Add ("sweet", sweet);
		attributes.Add ("bitter", bitter);
		attributes.Add ("sour", sour);
		attributes.Add ("salty", salty);
		attributes.Add ("spicy", spicy);

		actionCompleted.Add ("Dry", false);
		actionCompleted.Add ("Cook", false);
	}

	public override Dictionary<string, List<Action>> GetPossibleActions()
	{
		Dictionary<string, List<Action>> possibleActions = new Dictionary<string, List<Action>> ();

		if (component != null) 
		{
			possibleActions = component.GetPossibleActions ();
		}

		if (!possibleActions.ContainsKey (defaultCategoryName)) 
		{
			possibleActions.Add (defaultCategoryName, new List<Action> ());
		}

		if (!actionCompleted ["Cook"]) 
		{
			ActionsPerformed cookAct = new ActionsPerformed (Cook);
			Action cook = new Action ("Cook", cookAct);
			possibleActions [defaultCategoryName].Add (cook);
		}
	
		if (!actionCompleted ["Dry"]) 
		{
			ActionsPerformed dryAct = new ActionsPerformed (Dry);
			Action dry = new Action ("Dry", dryAct);
			possibleActions[defaultCategoryName].Add (dry);
		}

		if (toughness < 3) 
		{
			ActionsPerformed eatAct = new ActionsPerformed (Eat);
			Action eat = new Action ("Eat", eatAct);
			possibleActions [defaultCategoryName].Add (eat);
		}

		return possibleActions;
	}

	public void Cook(Dictionary<string, float> variables)
	{
		toughness = toughness / 2f;
		waterContent = waterContent / 1.2f;

		attributes ["toughness"] = toughness;
		attributes ["waterContent"] = waterContent;

		if (waterContent > 6f && !gameObject.name.Contains ("Soup")) 
		{
			gameObject.name = gameObject.name + " Soup";
			GetComponent<BaseObject> ().changeName (gameObject.name);
		} 
		else if (waterContent < 6f) 
		{
			gameObject.name = "Cooked " + gameObject.name;
			GetComponent<BaseObject> ().changeName (gameObject.name);
		}

		actionCompleted ["Cook"] = true;

		if (toughness >= 3f) 
		{
			GetComponent<BaseObject> ().RemoveAttributes (new List<string> (){ "SolidAttribute" });
		}
	}

	public void Dry(Dictionary<string, float> variables)
	{
		waterContent = waterContent / 4f;
		toughness = toughness * 1.5f;

		attributes ["waterContent"] = waterContent;
		attributes ["toughness"] = toughness;

		gameObject.name = "Dried " + gameObject.name;
		GetComponent<BaseObject> ().changeName (gameObject.name);

		actionCompleted ["Dry"] = true;
	}

	public void Eat(Dictionary<string, float> variables)
	{
		if (stomachEffect < 0) 
		{
			Debug.Log ("You're gonna barf");
		} 
		else if (stomachEffect > 0) 
		{
			Debug.Log ("You're gonna stop barfing");
		}	

		if (pneumoniaEffect < 0) 
		{
			Debug.Log ("You're gonna get pneumonia bub.");
		} 
		else if (pneumoniaEffect > 0) 
		{
			Debug.Log ("You're gonna get less feverish whoo");
		}

		SubpanelBehavior.instance.close ();
		GameObject.Destroy (this.gameObject);
	}
}