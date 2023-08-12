using System;
using System.Collections.Generic;
using Boo.Lang;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;


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
			if (dataTile.isNode == true && this.propertyName == "Node Order")
			{

                int H = int.Parse(Obj);
                int H2 = int.Parse(JToken.FromObject(this.Value).ToString());
                Debug.LogError(System.DateTime.Now);

                Debug.LogError("Value Current: " + H.ToString());
                Debug.LogError("Value New: " + H2.ToString());

                for (int k = 0; k < NodePathLayerHandler.Instance.LayerCount; k++)
				{
                    var nodeMap = (NodePathLayerHandler.Instance.GetMap(k) as NodeMap);
                    var list = nodeMap.fuckYou;
					if (list.Contains(dataTile))
					{
                        foreach (var entry in list)
                        {
                            if (entry.PositionInNodeMap(nodeMap) == H2)
                            {
                                //Debug.LogError("Plac Pre entry: " + entry.placmentOrder);
                                //Debug.LogError("Plac Pre dataTile: " + dataTile.placmentOrder);

                                var entry1 = nodeMap.fuckYou[H2];
                                nodeMap.fuckYou[H2] = nodeMap.fuckYou[H];
                                nodeMap.fuckYou[H] = entry1;

                                //entry.placmentOrder = H;
                                //dataTile.placmentOrder = H2;

                                //Debug.LogError("Plac Post entry: " + entry.placmentOrder);
                                //Debug.LogError("Plac Post dataTile: " + dataTile.placmentOrder);


                                entry.data[this.propertyName] = JToken.FromObject(H.ToString());
                                nodeMap.UpdateAtrributeList();
                                InputHandler.Instance.UpdateNodeLines();

                                //var allTiles = nodeMap.AllTiles();

                                //var pos1 = nodeMap.GetComponent<Tilemap>().LocalToCell(dataTile.position);
                                //var pos2 = nodeMap.GetComponent<Tilemap>().LocalToCell(entry.position);

                                //string h1 = nodeMap.tileDatabase.AllEntries[dataTile.name];
                                //string h2 = nodeMap.tileDatabase.AllEntries[entry.name];

                                //var tmp = nodeMap.tileDatabase.AllEntries[dataTile.name];
                                //nodeMap.tileDatabase.AllEntries[dataTile.name] = nodeMap.tileDatabase.AllEntries[entry.name];
                                //nodeMap.tileDatabase.AllEntries[entry.name] = tmp;

                                return;
                            }
                        }
                    }              
                }
            }
            else if (this.propertyName == "Node Type" && dataTile.isNode == true)
            {
                dataTile.data[this.propertyName] = JToken.FromObject(this.Value);
                dataTile.name = JToken.FromObject(this.Value).ToString();

                for (int k = 0; k < NodePathLayerHandler.Instance.LayerCount; k++)
                {

                    var nodeMap = (NodePathLayerHandler.Instance.GetMap(k) as NodeMap);
                    Sprite s;

                    nodeMap.spriteSwitch.TryGetValue((this.Value).ToString(), out s);
                    Debug.Log(JToken.FromObject(this.Value).ToString());

                    if (s) 
                    {
                        dataTile.sprite = s;//dataTile.placmentOrder
                        var fuckYou = FindObjectsOfType<Tilemap>();
                        foreach (var pieceoffuckingshit in fuckYou)
                        {
                            pieceoffuckingshit.RefreshAllTiles();
                        }
                        return;

                    }
                }
            }
        }
    }

    public Text text;

	
	public string propertyName;
}
