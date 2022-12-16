using System;
using System.Linq;
using UnityEngine.UI;


public class DropdownStringAttributeItem : AttributeItem
{
	
	
	 
	public override object Value
	{
		get
		{
			return this.dropdown.options[this.dropdown.value].text;
		}
		set
		{
			AC atr = AttributeDatabase.allAttributes[AttributeDatabase.LongToShortName(this.propertyName)];
			this.dropdown.options.Clear();
			foreach (object pv in atr.possibleValues)
			{
				this.dropdown.options.Add(new Dropdown.OptionData(pv.ToString()));
			}
			string val = atr.defaultValue.ToString();
			bool flag = atr.possibleValues.Contains(value);
			if (flag)
			{
				val = value.ToString();
			}
			this.dropdown.value = this.dropdown.options.FindIndex((Dropdown.OptionData od) => od.text == val);
		}
	}

	
	public Dropdown dropdown;
}
