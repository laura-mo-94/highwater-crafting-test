using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BaseObject : CollectableObject {

	public string objectName;
	public List<CollectableObject> comprisedOf;
	public List<string> tags;
	public float amount;

	private ObjectAttribute lastAttribute;
	private Text textField;

	// Use this for initialization
	void Awake () 
	{
		SetUpBaseObject ();	
	}

	public void SetUpBaseObject()
	{
		ObjectAttribute[] components = GetComponents<ObjectAttribute> ();

		CollectableObject prev = null;
		for (int i = 0; i < components.Length; i++) 
		{
			components [i].SetComponent (prev, this);
			components [i].ReadyAttribute ();
			prev = components [i];
		}

		lastAttribute = (ObjectAttribute) prev;
		textField = GetComponentInChildren<Text> ();
	}
	
	public override Dictionary<string, float> GetAttributes ()
	{
		Dictionary<string, float> attributes = lastAttribute.GetAttributes ();

		attributes.Add ("amount", amount);

		return attributes;
	}

	public override Dictionary<string, List<Action>> GetPossibleActions()
	{
		Dictionary<string, List<Action>> possibleActions = lastAttribute.GetPossibleActions ();

		return possibleActions;
	}

	public void changeName(string name)
	{
		objectName = name;
		textField.text = name;
	}

	public void RemoveAttributes(List<string> names, bool excluding = false)
	{
		ObjectAttribute[] attributes =  GetComponents<ObjectAttribute> ();

		ObjectAttribute prev = null;

		for (int i = 0; i < attributes.Length; ++i) 
		{
			attributes [i].ChangeComponent (prev);
			if (!names.Contains(attributes [i].GetType ().Name) && excluding) 
			{
				Destroy (attributes [i]);
			} 
			else if (!excluding && names.Contains(attributes [i].GetType ().Name)) 
			{
				Destroy (attributes [i]);
			}
			else 
			{
				prev = attributes [i];
			}
		}

		lastAttribute = (ObjectAttribute) prev;
	}

	public BaseObject CreateDuplicate()
	{
		int amt = EnterAmountBehavior.instance.GetAmount ();

		if (amt < amount) 
		{
			BaseObject duplicate = GameObject.Instantiate (this.gameObject).GetComponent<BaseObject> ();
			duplicate.gameObject.name = gameObject.name;
			duplicate.transform.parent = transform.parent;

			duplicate.amount = amt;
			amount -= duplicate.amount;
			return duplicate;
		}

		return null;
	}
}
