using UnityEngine;
using System.Collections;

public class Contract  {
	protected string _title;
	protected string _description;
	protected int _price;

	public string Title       { get { return _title; } }
	public string Description { get { return _description; } }
	public int Price          { get { return _price; } }

	// TODO: Word wrap.

	public Contract(string title, string description, int price) {
		_title = title;
		_description = description;
		_price = price;
	}

}
