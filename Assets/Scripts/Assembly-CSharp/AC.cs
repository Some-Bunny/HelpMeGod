using System;


public class AC
{
	
	public AC(string _longName, string _shortName, object _defaultValue, params object[] _possibleValues)
	{
		this.longName = _longName;
		this.shortName = _shortName;
		this.defaultValue = _defaultValue;
		this.possibleValues = _possibleValues;
	}

	
	public string longName;

	
	public string shortName;

	
	public object defaultValue;

	
	public object[] possibleValues;
}
