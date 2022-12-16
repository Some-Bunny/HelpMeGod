using System;
using UnityEngine.UI;


public class FloatAttributeItem : AttributeItem
{
	
	
	 
	public override object Value
	{
		get
		{
			return float.Parse(this.input.text);
		}
		set
		{
			this.input.text = value.ToString();
		}
	}

	
	public InputField input;
}
