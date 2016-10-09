using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SubpanelBehavior : UsesActionPerformed {

	public int currentState = 1;
	public static SubpanelBehavior instance;
	public Text objectName;

	Animator anim;
	GameObject scrollAttr;
	GameObject actScrollAttr;
	GameObject subActScrollAttr;

	Dictionary<string, GameObject> attrObjects;
	Dictionary<string, List<Action>> actionsByID;

	List<GameObject> actionButtons;

	bool itemChanged;
	BaseObject duplicate;
	BaseObject original;

	void Start()
	{
		instance = this;
		anim = GetComponent<Animator> ();
		scrollAttr = GameObject.Find ("ScrollAttributesContent");
		actScrollAttr = GameObject.Find ("ScrollActionsContent");
		subActScrollAttr = GameObject.Find ("ScrollSubActionsContent");
		attrObjects = new Dictionary<string, GameObject> ();
		actionsByID = new Dictionary<string, List<Action>> ();

		actionButtons = new List<GameObject> ();
	}

	public void open(GameObject ob)
	{
		currentState = 1;
		attrObjects.Clear ();
		actionsByID.Clear ();

		anim.SetInteger ("state", currentState);

		original = ob.GetComponent<BaseObject>();
		duplicate = original.CreateDuplicate ();
		itemChanged = false;

		populate ();
	}

	public void close()
	{
		currentState = 2;
		anim.SetInteger ("state", currentState);
		depopulate ();
		attrObjects.Clear ();
		actionsByID.Clear ();
		actionButtons.Clear ();

		if (!itemChanged) 
		{
			if (duplicate != null) 
			{
				Destroy (duplicate.gameObject);
			}
		} 
		else 
		{
			if (duplicate != null) 
			{
				Inventory.instance.addItem (duplicate.gameObject);
			}
			else 
			{
				Inventory.instance.addItem (original.gameObject);
			}
		}
	}

	public void depopulate()
	{
		GameObject[] children = new GameObject [subActScrollAttr.transform.childCount + actScrollAttr.transform.childCount + scrollAttr.transform.childCount];
		int current = 0;

		for (int i = 0; i < subActScrollAttr.transform.childCount; ++i) 
		{
			children [i] = subActScrollAttr.transform.GetChild (i).gameObject;
		}

		current = subActScrollAttr.transform.childCount;

		for (int j = current; j < actScrollAttr.transform.childCount + current; ++j) {
			children [j] = actScrollAttr.transform.GetChild (j - current).gameObject;
		}

		current += actScrollAttr.transform.childCount;

		for (int k = current; k < scrollAttr.transform.childCount + current; ++k) 
		{
			children [k] = scrollAttr.transform.GetChild (k - current).gameObject;
		}

		for (int j = 0; j < children.Length; ++j) 
		{
			GameObject.Destroy (children [j]);
		}
	}

	public void populate()
	{
		BaseObject obj;

		if (duplicate != null) 
		{
			obj = duplicate;
		} 
		else 
		{
			obj = original;
		}
			
		objectName.text = obj.objectName;

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

		Dictionary<string, List<Action>> acts = obj.GetPossibleActions ();
		GameObject origAct = (GameObject)Resources.Load ("Prefabs/UIPrefabs/ActionButton");

		List<string> keys = new List<string> (acts.Keys);

		for(int i = 0; i < keys.Count; ++i)
		{
			if (keys[i].Equals ("base")) 
			{
				for (int j = 0; j < acts [keys [i]].Count; ++j) 
				{
					GameObject act = GameObject.Instantiate (origAct);
					act.GetComponent<ActionButton> ().setAction (acts [keys [i]] [j].getAction (), acts [keys [i]] [j].getName ());
					act.transform.parent = actScrollAttr.transform;
					act.transform.localScale = Vector3.one;
					actionButtons.Add (act);
				}
			}
			else 
			{
				GameObject act = GameObject.Instantiate (origAct);
				ActionButton button = act.GetComponent<ActionButton> ();

				ActionsPerformed subAct = new ActionsPerformed (button.subPanelAction);
				button.setAction (subAct, keys[i]);
				button.setActionsID (keys [i]);
				actionsByID.Add (keys [i], acts [keys [i]]);

				act.transform.parent = actScrollAttr.transform;
				act.transform.localScale = Vector3.one;
				actionButtons.Add (act);
			}
		}
	}

	public void RefreshSubPanel()
	{
		BaseObject obj;

		if (duplicate != null) 
		{
			obj = duplicate;
		} 
		else 
		{
			obj = original;
		}

		objectName.text = obj.objectName;
		Dictionary<string, float> attr = obj.GetAttributes ();
		itemChanged = true;

		foreach(string key in attr.Keys) 
		{
			attrObjects [key].GetComponent<AttributeStat> ().setValue (attr [key]);
		}

		Dictionary<string, List<Action>> acts = obj.GetPossibleActions ();
		GameObject origAct = (GameObject)Resources.Load ("Prefabs/UIPrefabs/ActionButton");

		List<string> keys = new List<string> (acts.Keys);

		int buttonUsed = 0;

		for(int i = 0; i < keys.Count; ++i)
		{
			GameObject act;

			if (keys[i].Equals ("base")) 
			{
				for (int j = 0; j < acts [keys [i]].Count; ++j) 
				{
					if (buttonUsed < actionButtons.Count) 
					{
						act = actionButtons [buttonUsed];
						act.SetActive (true);
						buttonUsed++;
					} 
					else 
					{
						act = GameObject.Instantiate (origAct);
						act.transform.parent = actScrollAttr.transform;
						act.transform.localScale = Vector3.one;
						actionButtons.Add (act);
					}

					act.GetComponent<ActionButton> ().setAction (acts [keys [i]] [j].getAction (), acts [keys [i]] [j].getName ());
				}
			}
			else 
			{
				if (buttonUsed < actionButtons.Count) 
				{
					act = actionButtons [buttonUsed];
					act.SetActive (true);
					buttonUsed++;
				} 
				else 
				{
					act = GameObject.Instantiate (origAct);
				}

				ActionButton button = act.GetComponent<ActionButton> ();

				ActionsPerformed subAct = new ActionsPerformed (button.subPanelAction);
				button.setAction (subAct, keys[i]);
				button.setActionsID (keys [i]);

				if (actionsByID.ContainsKey (keys [i])) 
				{
					actionsByID [keys [i]] = acts [keys [i]];
				} 
				else 
				{
					actionsByID.Add (keys [i], acts [keys [i]]);
				}
			}
		}

		if (buttonUsed < actionButtons.Count) 
		{
			for (int j = buttonUsed; j < actionButtons.Count; ++j) 
			{
				actionButtons [j].SetActive (false);
			}
		}
	}

	public void CreateSubAction(string id)
	{
		GameObject origAct = (GameObject)Resources.Load ("Prefabs/UIPrefabs/ActionButton");
		List<Action> actions = actionsByID [id];

		for(int i = 0; i < actions.Count; ++i)
		{
			GameObject act = GameObject.Instantiate (origAct);
			act.GetComponent<ActionButton> ().setAction (actions[i].getAction(), actions[i].getName());
			act.transform.parent = subActScrollAttr.transform;
			act.transform.localScale = Vector3.one;
		}
	}

	public void ClearSubActionPanel()
	{
		GameObject[] children = new GameObject [subActScrollAttr.transform.childCount];

		for (int i = 0; i < subActScrollAttr.transform.childCount; ++i) 
		{
			children [i] = subActScrollAttr.transform.GetChild (i).gameObject;
		}

		for (int j = 0; j < children.Length; ++j) 
		{
			GameObject.Destroy (children [j]);
		}
	}
}
