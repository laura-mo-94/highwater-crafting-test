using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SubpanelBehavior : UsesActionPerformed {

	int currentState = 1;
	public static SubpanelBehavior instance;

	Animator anim;
	GameObject scrollAttr;
	GameObject actScrollAttr;
	GameObject subPanel;

	Dictionary<string, GameObject> attrObjects;
	Dictionary<string, GameObject> actObjects;

	void Start()
	{
		instance = this;
		anim = GetComponent<Animator> ();
		scrollAttr = GameObject.Find ("ScrollAttributesContent");
		actScrollAttr = GameObject.Find ("ScrollActionsContent");

		attrObjects = new Dictionary<string, GameObject> ();
		actObjects = new Dictionary<string, GameObject> ();
	}

	public void open(GameObject ob)
	{
		currentState = 1;

		anim.SetInteger ("state", currentState);
		subPanel = ob;
	}

	public void close()
	{
		currentState = 2;
		anim.SetInteger ("state", currentState);
	}

	public void populate()
	{
		GameObject ob = subPanel;
		BaseObject obj = ob.GetComponent<BaseObject> ();

		Dictionary<string, float> attr = obj.GetAttributes ();
		GameObject orig = (GameObject)Resources.Load ("Prefabs/UIPrefabs/StatElement");
		foreach(string key in attr.Keys) 
		{
			GameObject stat = GameObject.Instantiate (orig);
			stat.transform.parent = scrollAttr.transform;
			stat.transform.localScale = Vector3.one;
			AttributeStat s = stat.GetComponent<AttributeStat> ();
			s.setValue(attr [key]);
			s.setLabel (key);

			attrObjects.Add (key, stat);
		}

		Dictionary<string, ActionsPerformed> acts = obj.GetPossibleActions ();
		GameObject origAct = (GameObject)Resources.Load ("Prefabs/UIPrefabs/ActionButton");

		foreach (string key in acts.Keys) 
		{
			GameObject act = GameObject.Instantiate (origAct);
			act.GetComponent<ActionButton> ().setAction (acts [key], key);
			act.transform.parent = actScrollAttr.transform;
			act.transform.localScale = Vector3.one;
			actObjects.Add (key, act);
		}
	}

	public void RefreshSubPanel()
	{
		GameObject ob = subPanel;
		BaseObject obj = ob.GetComponent<BaseObject> ();

		Dictionary<string, float> attr = obj.GetAttributes ();

		foreach(string key in attr.Keys) 
		{
			attrObjects [key].GetComponent<AttributeStat> ().setValue (attr [key]);
		}
	}
}
