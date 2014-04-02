using UnityEngine;
using System.Collections;

public class ContractBehaviour : MonoBehaviour {

	protected Contract _contract;

	public Contract Contract {
		get { return _contract; }
		set {
			// Find the TextMeshes in this prefab and assign the values of the Contract to those TextMeshes.
			_contract = value;
			TextMesh title = transform.Find ("Title").gameObject.GetComponent<TextMesh>();
			TextMesh descr = transform.Find ("Description").gameObject.GetComponent<TextMesh>();
			TextMesh homeProfit     = transform.Find ("HomefulProfit").gameObject.GetComponent<TextMesh>();
			TextMesh businessProfit = transform.Find ("BusinessProfit").gameObject.GetComponent<TextMesh>();

			title.text = _contract.Title;
			descr.text = _contract.Description;
			businessProfit.text = _contract.BusinessProfit.ToString();
			homeProfit.text     = _contract.HomefulProfit.ToString();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override bool Equals (object obj)
	{
		if (obj == null)
			return false;
		if (ReferenceEquals (this, obj))
			return true;
		if (obj.GetType () != typeof(ContractBehaviour))
			return false;
		ContractBehaviour other = (ContractBehaviour)obj;
		return _contract == other._contract;
	}
	

	public override int GetHashCode ()
	{
		unchecked {
			return (_contract != null ? _contract.GetHashCode () : 0);
		}
	}
	

}
