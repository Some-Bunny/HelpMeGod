using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class DataTile : Tile/*, IEquatable<DataTile>, IComparable<DataTile>*/
{
	
	public JObject data = new JObject();

	public int placmentOrder = -1;
	public Vector2 position;
    
    /*public override bool Equals(object obj)
    {
        if (obj == null) return false;
        DataTile objAsPart = obj as DataTile;
        if (objAsPart == null || (objAsPart != null && objAsPart.placmentOrder < 0) || (this.placmentOrder < 0)) return false;
        else return Equals(objAsPart);
    }

    public int CompareTo(DataTile comparePart)
    {      
        if (comparePart == null || (comparePart != null && comparePart.placmentOrder < 0) || (this.placmentOrder < 0))
            return 1;

        else
            return this.placmentOrder.CompareTo(comparePart.placmentOrder);
    }

    public int SortByNameAscending(string name1, string name2)
    {

        return name1.CompareTo(name2);
    }

    public override int GetHashCode()
    {
        return placmentOrder;
    }

    public bool Equals(DataTile other)
    {
        if (other == null || (other != null && other.placmentOrder < 0) || (this.placmentOrder < 0)) return false;
        return (this.placmentOrder.Equals(other.placmentOrder));
    }*/
}
