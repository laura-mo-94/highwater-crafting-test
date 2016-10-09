using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActionButton : UsesActionPerformed {

	Dictionary<string, float> vars = null;
	ActionsPerformed action;
	Text btnText;
	string actionsID;

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
		SubpanelBehavior.instance.ClearSubActionPanel ();
		action (vars);
		if (SubpanelBehavior.instance.currentState < 2) {

			SubpanelBehavior.instance.RefreshSubPanel ();
		}
	}

	public void setActionsID(string i)
	{
		actionsID = i;
	}

	public void subPanelAction(Dictionary<string, float> variables)
	{
		SubpanelBehavior.instance.CreateSubAction (actionsID);
	}
}
