using UnityEngine;
using System.Collections;

public class ContractBehaviour : MonoBehaviour {

	protected Contract _contract;
	public Contract Contract {
		get { return _contract; }
		set {
			_contract = value;
			TextMesh title = transform.Find ("Title").gameObject.GetComponent<TextMesh>();
			TextMesh descr = transform.Find ("Description").gameObject.GetComponent<TextMesh>();
			TextMesh price = transform.Find ("Price").gameObject.GetComponent<TextMesh>();

			title.text = _contract.Title;
			descr.text = _contract.Description;
			price.text = _contract.Price.ToString();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
