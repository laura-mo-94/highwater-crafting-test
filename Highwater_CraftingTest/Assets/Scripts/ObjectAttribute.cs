using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ObjectAttribute : CollectableObject {

	protected CollectableObject component;
	protected BaseObject baseObject;
	protected Dictionary<string, float> attributes;

	public void SetComponent(CollectableObject c, BaseObject b)
	{
		component = c;
		baseObject = b;
	}

	public override Dictionary<string, float> GetAttributes ()
	{
		Dictionary<string, float> thisAttr = new Dictionary<string, float> ();

		foreach (string key in attributes.Keys) 
		{
			thisAttr.Add (key, attributes [key]);
		}

		if (component != null) 
		{
			Dictionary<string, float> otherAttributes = component.GetAttributes ();
			foreach (string key in otherAttributes.Keys) {
				thisAttr.Add (key, otherAttributes [key]);
			}
		}

		return thisAttr;
	}

	public virtual void ReadyAttribute()
	{
		
	}
}
