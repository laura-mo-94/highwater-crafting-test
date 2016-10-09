using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CollectableObject: UsesActionPerformed{

	public string defaultCategoryName = "base";

	public virtual Dictionary<string, float> GetAttributes ()
	{
		return new Dictionary<string, float> ();
	}

	public virtual Dictionary<string, List<Action>> GetPossibleActions (){
		return new Dictionary<string, List<Action>> ();
	}
}
