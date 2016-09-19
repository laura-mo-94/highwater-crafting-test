using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActionButton : UsesActionPerformed {

	Dictionary<string, float> vars = null;
	ActionsPerformed action;
	Text btnText;

	void Awake()
	{
		btnText = GetComponentInChildren<Text> ();
	}

	public void setAction(ActionsPerformed a, string name)
	{
		btnText.text = name;
		action = a;
	}

	public void setVariables(Dictionary<string, float> variables)
	{
		vars = variables;
	}

	public void performAction()
	{
		action (vars);
		SubpanelBehavior.instance.RefreshSubPanel ();
	}
}
