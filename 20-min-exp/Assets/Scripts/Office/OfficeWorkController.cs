using UnityEngine;
using System.Collections;
using System.IO;
using SimpleJSON;

public class OfficeWorkController : MonoBehaviour {
	public string contractPath;

	// TODO: Load in contracts
	public static Contract[] Contracts;
	protected ContractBehaviour[] _loadedContracts;

	protected Transform[] _spawnPoints;

	public int TimeLimit; // Time limit in seconds.
	protected bool _loadNewContracts = true;

	public Transform contractPrefab;
	protected int _highlightedContract = 0;

	// Use this for initialization
	void Start () {

		// Read in the conversation from the JSON file and initialise the node collection
		JSONNode node = JSONNode.Parse( File.ReadAllText(@contractPath) );
		Contracts = new Contract[ node["allContracts"].Count ];

		int i = 0;
		foreach (JSONNode n in node["allContracts"].Childs) {
			Contracts[i] = new Contract(n["title"], n["description"], n["price"].AsInt);
			i++;
		}

		// Save references for all the contract spawn points
		ArrayList sp = new ArrayList();
		foreach (Transform child in transform) {
			if (child.name.Equals("Spawn"))
				sp.Add(child);
		}
		_spawnPoints = new Transform[sp.Count];
		_loadedContracts = new ContractBehaviour[sp.Count];
		sp.CopyTo(_spawnPoints);
	}
	
	// Update is called once per frame
	void Update () {
		if (_loadNewContracts) {
			// Destroy current contracts, before instantiating new ones.
			foreach (ContractBehaviour cb in _loadedContracts) {
				if (cb == null)
					continue;
				Destroy(cb.gameObject);
			}

			// Find a random contracts and instantiate them
			for (int i = 0; i < _loadedContracts.Length; i++) {
				int randIndex = (int) Mathf.Round(Random.value * (Contracts.Length-1));
				// Safeguard for the first run, where _loadedContracts is filled with nulls
				if (_loadedContracts[i] != null)
					// Keep randoming until the resulting contract is not the same as the current one
					// This does NOT prevent the other spawn points from spawning a contract which has just been shown at another spawn point
					while (Contracts[randIndex].Equals(_loadedContracts[i].Contract))
				    	randIndex = (int) Mathf.Round(Random.value * (Contracts.Length-1));


				Transform tra = Instantiate(contractPrefab, _spawnPoints[i].position, Quaternion.identity) as Transform;
				// Assign a contract object (with contract content) to the newly spawned contract prefab
				tra.gameObject.GetComponent<ContractBehaviour>().Contract = Contracts[randIndex];

				_loadedContracts[i] = tra.gameObject.GetComponent<ContractBehaviour>();
			}

			_loadNewContracts = false;
		}

		// Controls
		if (Input.GetKeyDown(KeyCode.LeftArrow) && _highlightedContract != 0) {
			_highlightedContract--;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) && _highlightedContract != _loadedContracts.Length-1) {
			_highlightedContract++;
		}
		if (Input.GetKeyDown(KeyCode.Return))
		    SelectContract( _loadedContracts[_highlightedContract].Contract );
	}

	public void SelectContract(Contract contract) {
//		_highlightedContract = 0;
		_loadNewContracts = true;
		Toolbox.Instance.gameState.MoneyCounter -= contract.Price;
	}

	void OnGUI() {
		Vector3 pos = Camera.main.WorldToScreenPoint( _loadedContracts[_highlightedContract].transform.position );
		Rect selectionPos = new Rect(pos.x, Screen.height-pos.y, 20, 20);
		GUIHelpers.DrawQuad(selectionPos, Color.blue);
	}

}
