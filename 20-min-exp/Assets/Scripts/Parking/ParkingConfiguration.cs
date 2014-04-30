using UnityEngine;
using System.Collections;

public class ParkingConfiguration : MonoBehaviour {

    public int SpawnPercentage = 80;
    public static float SpawnPct;
    public string MainCharactersName;
    public string[] ParkingNames;
    private static string[] _names;
    public static string MainName;
    public GameObject[] CarPrefabs;
    public static GameObject[] Cars;
    void Awake() {
        SpawnPct = (SpawnPercentage/100f);
        MainName = MainCharactersName;
        Cars = CarPrefabs;
        _names = ParkingNames;
        _names.Shuffle();
    }

    private static int _index;
    private static string GetRandomName() {
        if (_index >= _names.Length) _index = 0; 
        return _names[_index++];
    }


    private static Spot[] _spawnPattern;
    public static void CreateSpawnPattern() {
        _spawnPattern = new Spot[CarSpawn.All.Count];
        var cap = (int)Mathf.Floor(SpawnPct * CarSpawn.All.Count);
        for(int i = 0; i < CarSpawn.All.Count; i++) {
            var name = GetRandomName();
            if (i < cap) _spawnPattern[i] = new Spot { Name = name, Taken = true };
            else         _spawnPattern[i] = new Spot { Name = name, Taken = false };
        }
        _spawnPattern.Shuffle();
    }
    public struct Spot {
        public bool Taken;
        public string Name;
    }

    public static Spot GetSpotSpawn(int index) {
        if(_spawnPattern == null) CreateSpawnPattern();
        return _spawnPattern[index];
    }
}
