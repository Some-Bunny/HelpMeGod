using System;
using UnityEngine.UI;


public class BooleanAttributeItem : AttributeItem
{	 
	public override object Value
	{
		get
		{
			return this.toggle.isOn;
		}
		set
		{
			this.toggle.isOn = bool.Parse(value.ToString());
		}
	}

	
	public Toggle toggle;
}
