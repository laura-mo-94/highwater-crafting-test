using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseObject : CollectableObject {

	public string objectName;
	public List<CollectableObject> comprisedOf;
	public float amount;

	private ObjectAttribute lastAttribute;

	// Use this for initialization
	void Start () 
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
	}
	
	public override Dictionary<string, float> GetAttributes ()
	{
		Dictionary<string, float> attributes = lastAttribute.GetAttributes ();

		attributes.Add ("amount", amount);

		return attributes;
	}

	public override Dictionary<string, ActionsPerformed> GetPossibleActions()
	{
		Dictionary<string, ActionsPerformed> possibleActions = lastAttribute.GetPossibleActions ();

		return possibleActions;
	}

}
