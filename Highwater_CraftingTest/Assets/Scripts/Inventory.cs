using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public static Inventory instance;
	public GameObject itemParent;

	Dictionary<string, BaseObject> inventory;


	// Use this for initialization
	void Start () {
		instance = this;
		inventory = new Dictionary<string, BaseObject> ();
		BaseObject ob;

		for (int i = 0; i < itemParent.transform.childCount; ++i) 
		{
			if (itemParent.transform.GetChild(i).gameObject.activeInHierarchy) 
			{
				ob = itemParent.transform.GetChild (i).GetComponent<BaseObject> ();

				inventory.Add (ob.objectName, ob);
			}

		}
	}

	public void useItem(string name, int amt)
	{
		inventory [name].amount -= amt;

		if (inventory [name].amount <= 0) {
			GameObject.Destroy (inventory [name].gameObject);
			inventory.Remove (name);
		}
	}

	public BaseObject getItem(string name)
	{
		return inventory [name];
	}

	public void addItem(GameObject ob)
	{
		inventory.Add (ob.GetComponent<BaseObject> ().objectName, ob.GetComponent<BaseObject> ());
		ob.transform.parent = itemParent.transform;
	}

}
