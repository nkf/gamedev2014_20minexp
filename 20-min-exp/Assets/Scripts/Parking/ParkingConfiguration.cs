using UnityEngine;
using System.Collections;

public class ParkingConfiguration : MonoBehaviour {

    public int SpawnPercentage = 80;
    public static float SpawnPct;
    public string MainCharactersName;
    public string[] ParkingNames;
    private static string[] _names;
    public static string MainName;
    void Awake() {
        SpawnPct = (SpawnPercentage/100f);
        MainName = MainCharactersName;
        _names = ParkingNames;
        _names.Shuffle();
    }

    private static int _index;
    public static string GetRandomName() {
        if (_index <= _names.Length) _index = 0; 
        return _names[_index++];
    }
    
}
