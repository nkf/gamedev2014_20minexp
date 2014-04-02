using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using SimpleJSON;

public class OfficeWorkController : MonoBehaviour {

	// Managed in Unity UI
	public string contractPath;
	public Transform contractPrefab;
	public int TimeLimit; // How long should the "game" run, before it ends?

	// Everything else
	protected List<Contract>  _normalContracts;
	protected List<Contract>  _badassContracts;
	protected Transform[]         _spawnPoints;
	protected ContractBehaviour[] _loadedContracts;
	protected List<Contract>  _usedContracts;

	protected bool _loadNewContracts = true;
	protected int  _highlightedContract = 0;
	
	// Used for BADASS contract spawning randomness
	protected int _baseChance;
	protected int _increaseChance = 1;
	protected int _blocker;
	protected int _currentChance;
	protected int _currentBlocker = 0;
	// _currentBlocker will decrease for each contract loaded, and not for each iteration of contract loading...
	// which means that _blocker should AT least be larger than the total number of contracts displayed at a time. Preferably larger.

	// Use this for initialization
	void Start () {
		// TODO: For testing purposes. Can be deleted for final version
		GameState touchTheSingleton = Toolbox.Instance.gameState;


		// Read in the conversation from the JSON file and initialise the node collection
		JSONNode node = JSONNode.Parse( File.ReadAllText(@contractPath) );
		_normalContracts = new List<Contract>();
		_badassContracts = new List<Contract>();

		// Read and write normal contracts
		foreach (JSONNode n in node["normalContracts"].Childs) {
			_normalContracts.Add( new Contract(n["title"], n["description"], n["homefulProfit"].AsInt, n["businessProfit"].AsInt) );
		}
		// Read and write BADASS contracts
		foreach (JSONNode n in node["badassContracts"].Childs) {
			_badassContracts.Add( new Contract(n["title"], n["description"], n["homefulProfit"].AsInt, n["businessProfit"].AsInt) );
		}

		// Save references for all the contract spawn points
		ArrayList sp = new ArrayList();
		foreach (Transform child in transform) {
			if (child.name.Equals("Spawn"))
				sp.Add(child);
		}
		_spawnPoints = new Transform[sp.Count];
		_loadedContracts = new ContractBehaviour[sp.Count];
		_usedContracts = new List<Contract>();
		sp.CopyTo(_spawnPoints);

		// Initialize BADASS randomizer based on the number of normal contracts
		_baseChance    = node["normalContracts"].Count;
		_currentChance = _baseChance;
		_blocker       = (int) Math.Round((node.Count/4.0) * _spawnPoints.Length);
	}
	
	// Update is called once per frame
	void Update () {
		if (_loadNewContracts)
			LoadContracts();

		if (_normalContracts.Count <= 0)
			Debug.Log ("Geemu Ovaa");

		// Controls
		if (Input.GetKeyDown(KeyCode.LeftArrow) && _highlightedContract != 0) {
			_highlightedContract--;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) && _highlightedContract != _loadedContracts.Length-1) {
			_highlightedContract++;
		}
		if (Input.GetKeyDown(KeyCode.Return)) {
			if (_normalContracts.Count > 0)
		    	SelectContract( _loadedContracts[_highlightedContract].Contract );
		}
	}

	private bool SpawnBadassContract() {
		int val = (int) Mathf.Round( UnityEngine.Random.value * _currentChance );

		if (val == 0 && _currentBlocker == 0) {
			_currentBlocker = _blocker;
			_currentChance = _baseChance;
			return true;
		} else {
			if(_currentBlocker > 0)
				_currentBlocker--;
			_currentChance -= _increaseChance;
			return false;
		}
	}


	private void LoadContracts() {
		// Find a random contracts and instantiate them
		for (int i = 0; i < _loadedContracts.Length; i++) {
			// Spawn new prefab and assign a contract object (with contract content)
			Transform tra = Instantiate(contractPrefab, _spawnPoints[i].position, Quaternion.identity) as Transform;
			
			int randIndex;
			if (SpawnBadassContract()) {
				randIndex = (int) Mathf.Round( UnityEngine.Random.Range(0, _badassContracts.Count) );
				tra.gameObject.GetComponent<ContractBehaviour>().Contract = _badassContracts.Count > 0 ? _badassContracts[randIndex] : _usedContracts[randIndex];
			} else {
				randIndex = (int) Mathf.Round( UnityEngine.Random.Range(0, _normalContracts.Count) );
				// If the game has run out of contracts on this frame, find a random one already used.
				tra.gameObject.GetComponent<ContractBehaviour>().Contract = _normalContracts.Count > 0 ? _normalContracts[randIndex] : _usedContracts[randIndex];
				
				// Move from available contracts to used contracts
				if(_normalContracts.Count > 0) {
					_usedContracts.Add(_normalContracts[randIndex]);
					_normalContracts.Remove(_normalContracts[randIndex]);
				}
			}
			
			// Destroy the old game object if applicable, replace with the new prefab
			if (_loadedContracts[i] != null)
				Destroy(_loadedContracts[i].gameObject);
			_loadedContracts[i] = tra.gameObject.GetComponent<ContractBehaviour>();
		}
		
		_loadNewContracts = false;
	}

	public void SelectContract(Contract contract) {
//		_highlightedContract = 0;
		_loadNewContracts = true;
		Toolbox.Instance.gameState.MoneyCounter += contract.HomefulProfit;
		// TODO: Business profit
	}

	void OnGUI() {
		Vector3 pos = Camera.main.WorldToScreenPoint( _loadedContracts[_highlightedContract].transform.position );
		Rect selectionPos = new Rect(pos.x, Screen.height-pos.y, 20, 20);
		GUIHelpers.DrawQuad(selectionPos, Color.blue);
	}

}
