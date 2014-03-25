using UnityEngine;

/// <summary>
/// Toolbox pattern. The toolbox contains references to objects, that should be available at 
/// all times in the game.
/// 
/// Currently, this includes the money counter for the homeful guy's bank account and the 
/// level controller used to switch between Unity scenes.
/// </summary>
public class Toolbox : Singleton<Toolbox> {
	protected Toolbox () {} // guarantee this will be always a singleton only - can't use the constructor!
	
	public LevelController levelController;
	public GameObject gameStateObject;
	public GameState gameState;

	void Awake () {
		levelController = new LevelController();
		
		gameStateObject = new GameObject();
		gameState = gameStateObject.AddComponent("GameState") as GameState;
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(gameStateObject);
	}

}

