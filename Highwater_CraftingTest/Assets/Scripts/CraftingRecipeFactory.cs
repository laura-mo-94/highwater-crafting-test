using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CraftingRecipeFactory : MonoBehaviour {

	public delegate void CraftItem(List<BaseObject> ingredients);
	public GameObject baseObjectTemplate;
	public static CraftingRecipeFactory instance;
	public Dictionary<string, CraftItem> craftingList;

	// Use this for initialization
	void Start () {
		instance = this;
		CraftItem fishingRod = new CraftItem (CraftFishingRod);
		craftingList = new Dictionary<string, CraftItem> ();
		craftingList.Add ("Fishing Rod", fishingRod);
	}

	public void Craft(string name, List<BaseObject> ingredients)
	{
		craftingList [name](ingredients);
	}

	public void CraftFishingRod(List<BaseObject> ingredients)
	{
		GameObject ob = GameObject.Instantiate (baseObjectTemplate);
		ob.SetActive (true);

		BaseObject baseOb = ob.GetComponent<BaseObject> ();

		BaseObject rod = null;
		BaseObject line = null;
		BaseObject hook = null;

		for (int i = 0; i < ingredients.Count; ++i) 
		{
			if (ingredients [i].tags.Contains ("Rod")) 
			{
				rod = ingredients [i];
			} 
			else if (ingredients [i].tags.Contains ("Rope")) 
			{
				line = ingredients [i];
			} 
			else if (ingredients [i].tags.Contains ("Hook")) 
			{
				hook = ingredients [i];
			}
		}

		baseOb.changeName(rod.objectName + " Fishing Rod");
		ob.name = baseOb.objectName;

		baseOb.amount = 1;
		baseOb.comprisedOf.Add (rod);
		baseOb.comprisedOf.Add (line);
		baseOb.comprisedOf.Add (hook);

		SolidAttribute sol = ob.AddComponent<SolidAttribute> ();
		Dictionary<string, float> rodAttr = rod.GetAttributes ();
		Dictionary<string, float> lineAttr = line.GetAttributes ();
		Dictionary<string, float> hookAttr = hook.GetAttributes ();

		sol.durability = (rodAttr ["durability"] + lineAttr ["durability"] + hookAttr["durability"]) / 3f;
		sol.elasticity = (rodAttr ["elasticity"] + lineAttr ["elasticity"]) / 2f;
		sol.flexibility = (rodAttr ["flexibility"] + lineAttr ["flexibility"]) / 2f;
		sol.sharpness = hookAttr ["sharpness"];
		sol.stickiness = rodAttr ["stickiness"];
		sol.thickness = rodAttr ["thickness"];

		baseOb.SetUpBaseObject ();
		Inventory.instance.addItem (ob);
	}
}
