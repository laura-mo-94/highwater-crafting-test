using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnterAmountBehavior : MonoBehaviour {

	public static EnterAmountBehavior instance;
	public Text numDisplay;
	public Button minus;
	public Button plus;

	int currentState;
	int currentAmount;
	int maxAmount;
	Animator anim;
	GameObject focus;

	// Use this for initialization
	void Start () {
		instance = this;
		anim = GetComponent<Animator> ();
	}
	
	public void Change(int i)
	{
		currentAmount += i;
		numDisplay.text = currentAmount.ToString();

		if (currentAmount < 1) {
			minus.gameObject.SetActive (false);
		} 
		else if (currentAmount == maxAmount) 
		{
			plus.gameObject.SetActive (false);
		} 
		else if (!minus.gameObject.activeInHierarchy) 
		{
			minus.gameObject.SetActive (true);
		} 
		else if (!plus.gameObject.activeInHierarchy) 
		{
			plus.gameObject.SetActive (true);
		}
	}

	public int GetAmount()
	{
		return currentAmount;
	}

	public void setMaxAmount(int max)
	{
		maxAmount = max;
	}

	public void open(GameObject f)
	{
		focus = f;
		maxAmount = (int)focus.GetComponent<BaseObject>().amount;
		currentAmount = 0;
		numDisplay.text = currentAmount.ToString();
		currentState = 1;
		anim.SetInteger ("state", currentState);

		minus.gameObject.SetActive (true);
		plus.gameObject.SetActive (true);
		Change (0);
	}

	public void close()
	{
		currentState = 2;
		anim.SetInteger ("state", currentState);

	}

	public void openSubpanel()
	{
		close ();
		SubpanelBehavior.instance.open (focus);
	}
}
