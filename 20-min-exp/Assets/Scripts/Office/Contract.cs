using UnityEngine;
using System.Collections;

public class Contract  {
	protected string _title;
	protected string _description;
	protected int    _homefulProvisionPct;
	protected int    _businessProfit;

	public string Title       { get { return _title; } }
	public string Description { get { return _description; } }
	public int HomefulProvisionPct  { get { return _homefulProvisionPct; } }
	public int BusinessProfit { get { return _businessProfit; } }

	// TODO: Word wrap?

	public Contract(string title, string description, int homefulProfit, int businessProfit) {
		_title = title;
		_description = description;
		_homefulProvisionPct = homefulProfit;
		_businessProfit = businessProfit;
	}

	public override bool Equals (object obj)
	{
		if (obj == null)
			return false;
		if (ReferenceEquals (this, obj))
			return true;
		if (obj.GetType () != typeof(Contract))
			return false;
		Contract other = (Contract)obj;
		return _title == other._title && _description == other._description && _homefulProvisionPct == other._homefulProvisionPct && _businessProfit == other._businessProfit;
	}
	

	public override int GetHashCode ()
	{
		unchecked {
			return (_title != null ? _title.GetHashCode () : 0) ^ (_description != null ? _description.GetHashCode () : 0) ^ _homefulProvisionPct.GetHashCode () ^ _businessProfit.GetHashCode ();
		}
	}
	
	public override string ToString ()
	{
		return string.Format ("[Contract: _title={0}, _description={1}, _homefulProfit={2}, _businessProfit={3}]", _title, _description, _homefulProvisionPct, _businessProfit);
	}
	
}
