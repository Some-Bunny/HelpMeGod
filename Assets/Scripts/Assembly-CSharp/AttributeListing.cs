using System;


public class AttributeListing
{
	
	public AttributeListing(string _tileName, params AC[] _attributes)
	{
		this.tileName = _tileName;
		this.attributes = _attributes;
	}

	
	public string tileName;

	
	public AC[] attributes;
}
