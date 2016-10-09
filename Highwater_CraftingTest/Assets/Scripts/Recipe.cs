using System.Collections.Generic;

public class Recipe  {

	List<Requirement> requirements;
	string recipeName;

	public Recipe(string name)
	{
		recipeName = name;
		requirements = new List<Requirement> ();
	}

	public void addRequirement(Requirement req)
	{
		requirements.Add (req);
	}

	public List<Requirement> getRequirements()
	{
		return requirements;
	}

	public bool checkCompleted()
	{
		for (int i = 0; i < requirements.Count; ++i) 
		{
			if (!requirements [i].isFullfilled ()) {
				return false;
			}	
		}

		return true;
	}

	public string getRecipeName()
	{
		return recipeName;
	}
}
