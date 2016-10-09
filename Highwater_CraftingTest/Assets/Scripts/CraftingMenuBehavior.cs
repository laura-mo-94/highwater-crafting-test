using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class CraftingMenuBehavior : MonoBehaviour {

	public GameObject craftingItemTemplate;
	public GameObject recipePanel;
	public Button continueButton;
	public Text emptyText;
	public GameObject craftButton;

	Dictionary<string, Recipe> recipes;
	string currentReqChoice;
	string recipeChoice;
	Transform objectPanel;
	Animator anim;
	Animator animSelect;
	public Text selectText;

	int baseCurrentState;
	int selectPanelCurrentState;
	List<string> itemsSelected;
	int current;

	Image currentHighlighted;

	// Use this for initialization
	void Start () {
		recipes = new Dictionary<string, Recipe> ();
		Recipe fishrod = new Recipe ("Fishing Rod");
		fishrod.addRequirement (new Requirement (1, "Rod"));
		fishrod.addRequirement (new Requirement (1, "Rope"));
		fishrod.addRequirement (new Requirement (1, "Hook"));

		recipes.Add (fishrod.getRecipeName (), fishrod);
		objectPanel = GameObject.Find ("InventoryPanel").transform;

		anim = GetComponent<Animator>();
		animSelect = GameObject.Find("Select X Panel").GetComponent<Animator>();
		itemsSelected = new List<string> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChooseRecipe(string r)
	{
		recipeChoice = r;
		selectPanelCurrentState = 1;
		animSelect.SetInteger ("state", selectPanelCurrentState);

		DisplayPossibleItems (0);
	}

	public void cancelCrafting()
	{
		selectPanelCurrentState = 2;
		animSelect.SetInteger ("state", selectPanelCurrentState);
		itemsSelected.Clear ();
	}

	public void open()
	{
		baseCurrentState = 1;
		anim.SetInteger("state", baseCurrentState);
		continueButton.gameObject.SetActive (true);
		craftButton.SetActive (false);
	}

	public void close()
	{
		baseCurrentState = 2;
		anim.SetInteger ("state", baseCurrentState);
	}
		
	public void next()
	{
		if (current + 1 >= recipes [recipeChoice].getRequirements ().Count - 1) {
			continueButton.gameObject.SetActive (false);
			craftButton.SetActive (true);
		}

		DisplayPossibleItems(current+1);
	}

	public void DisplayPossibleItems(int cur)
	{
		Recipe rec = recipes [recipeChoice];
		current = cur;
		continueButton.interactable = false;
		emptyText.gameObject.SetActive (false);

		string itemTag = rec.getRequirements () [current].getObjectType ();
		selectText.text = "Select " + itemTag;
		int totalPossible = 0;

		for (int j = 0; j < recipePanel.transform.childCount; ++j) 
		{
			if (!recipePanel.transform.GetChild (j).gameObject.name.Equals(craftingItemTemplate.name) && !recipePanel.transform.GetChild(j).gameObject.name.Equals(emptyText.name) ) 
			{
				Destroy(recipePanel.transform.GetChild (j).gameObject);
			}
		}

		for(int i = 0; i < objectPanel.childCount; ++i)
		{
			if (objectPanel.transform.GetChild (i).GetComponent<BaseObject> ().tags.Contains (itemTag)) 
			{
				GameObject ob = GameObject.Instantiate (craftingItemTemplate);
				ob.transform.parent = recipePanel.transform;

				ob.GetComponentInChildren<Text> ().text = objectPanel.transform.GetChild (i).GetComponent<BaseObject> ().objectName;
				ob.name = ob.GetComponentInChildren<Text> ().text;
				ob.SetActive (true);
				totalPossible++;
			}
		}

		if (totalPossible == 0) 
		{
			emptyText.gameObject.SetActive (true);
		}
	}

	public void SelectItem(Text name)
	{
		if (itemsSelected.Count == current) {
			itemsSelected.Add (name.text);
		} else {
			itemsSelected [current] = name.text;
		}

		continueButton.interactable = true;
	}

	public void Highlight(Image b)
	{
		if (currentHighlighted != null) 
		{
			currentHighlighted.color = Color.white;
		}

		currentHighlighted = b;
		currentHighlighted.color = Color.grey;
	}

	public void Craft()
	{
		List<BaseObject> ingredients = new List<BaseObject> ();

		for (int i = 0; i < itemsSelected.Count; ++i) {
			ingredients.Add(Inventory.instance.getItem (itemsSelected [i]));
		}

		CraftingRecipeFactory.instance.Craft (recipeChoice, ingredients);

		for (int i = 0; i < itemsSelected.Count; ++i) {
			Inventory.instance.useItem (itemsSelected [i], recipes [recipeChoice].getRequirements() [i].getAmountRequired ());
		}

		cancelCrafting ();
	}
}
