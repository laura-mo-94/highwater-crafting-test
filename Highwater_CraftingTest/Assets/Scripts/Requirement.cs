public class Requirement 
{
	int amountRequired;
	string objectType;

	public Requirement(int amount, string type)
	{
		amountRequired = amount;
		objectType = type;
	}

	public bool isFullfilled()
	{
		return (amountRequired <= 0);
	}

	public void submitObject(BaseObject ob)
	{
		if (ob.tags.Contains (objectType)) 
		{
			amountRequired--;	
		}
	}

	public string getObjectType()
	{
		return objectType;
	}

	public int getAmountRequired()
	{
		return amountRequired;
	}
}
