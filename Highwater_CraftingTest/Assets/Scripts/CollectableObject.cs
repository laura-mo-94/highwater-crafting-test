using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CollectableObject: UsesActionPerformed{

	public virtual Dictionary<string, float> GetAttributes ()
	{
		return new Dictionary<string, float> ();
	}

	public virtual Dictionary<string, ActionsPerformed> GetPossibleActions (){
		return new Dictionary<string, ActionsPerformed> ();
	}
}
