using System;
using UnityEngine.UI;


public class StringAttributeItem : AttributeItem
{
	
	
	 
	public override object Value
	{
		get
		{
			return this.input.text;
		}
		set
		{
			this.input.text = value.ToString();
		}
	}

	
	public InputField input;
}
