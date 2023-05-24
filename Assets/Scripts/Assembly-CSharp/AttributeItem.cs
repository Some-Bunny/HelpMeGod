using System;
using System.Collections.Generic;
using Boo.Lang;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public abstract class AttributeItem : MonoBehaviour
{
	
	
	
	public abstract object Value { get; set; }

	
	public void ValueChanged()
	{
		Tile tile = InputHandler.Instance.selectedTile;
		DataTile dataTile;
		if (tile && (dataTile = (tile as DataTile)) != null)
		{
            var Obj = JToken.FromObject(dataTile.data[this.propertyName]).ToString();
            dataTile.data[this.propertyName] = JToken.FromObject(this.Value);

			if (dataTile.isNode == true && dataTile.data[this.propertyName] != JToken.FromObject(dataTile.data[this.propertyName]))
			{

                int H = int.Parse(Obj);
                int H2 = int.Parse(JToken.FromObject(this.Value).ToString());
                Debug.LogError("Value Current: " + H.ToString());
                Debug.LogError("Value New: " + H2.ToString());

                for (int k = 0; k < NodePathLayerHandler.Instance.LayerCount; k++)
				{
                    var list = (NodePathLayerHandler.Instance.GetMap(k) as NodeMap).fuckYou;
					if (list.Contains(dataTile))
					{
                        foreach (var entry in list)
                        {
                            if (entry.placmentOrder == H2)
                            {

                                entry.placmentOrder = H;
                                dataTile.placmentOrder = H2;

                                entry.data[this.propertyName] = JToken.FromObject(H.ToString());
                                return;
                            }
                        }
                    }              
                }
            }
        }
    }

    public Text text;

	
	public string propertyName;
}
