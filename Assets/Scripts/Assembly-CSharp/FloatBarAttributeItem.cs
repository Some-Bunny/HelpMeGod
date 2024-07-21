using System;
using UnityEngine.UI;


public class FloatBarAttributeItem : AttributeItem
{
	
	
	 
	public override object Value
	{
		get
		{
			return this.input.value;
		}
		set
		{
			AC atr = AttributeDatabase.allAttributes[AttributeDatabase.LongToShortName(this.propertyName)];
			this.input.minValue = (float)atr.possibleValues[0];
			this.input.maxValue = (float)atr.possibleValues[1];
			this.input.value = (float)value;
		}
	}	
	public Slider input;
}
