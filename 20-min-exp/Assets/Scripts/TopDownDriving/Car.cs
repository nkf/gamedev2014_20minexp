using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

	public static Car PLAYER;

	protected SplineController _splineController;

	// Use this for initialization
	void Start () {
		PLAYER = this;
		_splineController = GetComponent<SplineController>() as SplineController;
	}

	/// <summary>
	/// Enable/disable control of the car.
	/// 
	/// Note that it switches controls to AUTO which may cause the car to move automatically if the spline system changes.
	/// </summary>
	/// <param name="enable">If set to <c>true</c> enable.</param>
	public void SetControls(bool enable) {
		if (enable)
			_splineController.mode = SplineController.Mode.KEYBOARD;
		else
			_splineController.mode = SplineController.Mode.AUTO;
	}
}
