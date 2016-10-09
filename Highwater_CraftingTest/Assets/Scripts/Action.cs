using UnityEngine;
using System.Collections;

public class Action : UsesActionPerformed {

	string actionName;
	ActionsPerformed actionToPerform;
	int subActID;

	public Action(string n, ActionsPerformed act)
	{
		actionName = n;
		actionToPerform = act;
	}

	public string getName()
	{
		return actionName;
	}

	public ActionsPerformed getAction()
	{
		return actionToPerform;
	}

	public void setSubActID(int i)
	{
		subActID = i;
	}

	public int getSubActsID()
	{
		return subActID;
	}
}
