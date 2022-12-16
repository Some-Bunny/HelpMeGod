using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Tilemaps
{
	
	[AddComponentMenu("Tilemap/Grid Information")]
	[Serializable]
	public class GridInformation : MonoBehaviour, ISerializationCallbackReceiver
	{
		

		internal Dictionary<GridInformation.GridInformationKey, GridInformation.GridInformationValue> PositionProperties
		{
			get
			{
				return this.m_PositionProperties;
			}
		}

		 
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			Grid grid = base.GetComponentInParent<Grid>();
			bool flag = grid == null;
			if (!flag)
			{
				this.m_PositionIntKeys.Clear();
				this.m_PositionIntValues.Clear();
				this.m_PositionStringKeys.Clear();
				this.m_PositionStringValues.Clear();
				this.m_PositionFloatKeys.Clear();
				this.m_PositionFloatValues.Clear();
				this.m_PositionDoubleKeys.Clear();
				this.m_PositionDoubleValues.Clear();
				this.m_PositionObjectKeys.Clear();
				this.m_PositionObjectValues.Clear();
				this.m_PositionColorKeys.Clear();
				this.m_PositionColorValues.Clear();
				foreach (KeyValuePair<GridInformation.GridInformationKey, GridInformation.GridInformationValue> kvp in this.m_PositionProperties)
				{
					switch (kvp.Value.type)
					{
					case GridInformationType.Integer:
						this.m_PositionIntKeys.Add(kvp.Key);
						this.m_PositionIntValues.Add((int)kvp.Value.data);
						break;
					case GridInformationType.String:
						this.m_PositionStringKeys.Add(kvp.Key);
						this.m_PositionStringValues.Add(kvp.Value.data as string);
						break;
					case GridInformationType.Float:
						this.m_PositionFloatKeys.Add(kvp.Key);
						this.m_PositionFloatValues.Add((float)kvp.Value.data);
						break;
					case GridInformationType.Double:
						this.m_PositionDoubleKeys.Add(kvp.Key);
						this.m_PositionDoubleValues.Add((double)kvp.Value.data);
						break;
					case GridInformationType.UnityObject:
						goto IL_1F8;
					case GridInformationType.Color:
						this.m_PositionColorKeys.Add(kvp.Key);
						this.m_PositionColorValues.Add((Color)kvp.Value.data);
						break;
					default:
						goto IL_1F8;
					}
					continue;
					IL_1F8:
					this.m_PositionObjectKeys.Add(kvp.Key);
					this.m_PositionObjectValues.Add(kvp.Value.data as Object);
				}
			}
		}

		 
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.m_PositionProperties.Clear();
			for (int i = 0; i != Math.Min(this.m_PositionIntKeys.Count, this.m_PositionIntValues.Count); i++)
			{
				GridInformation.GridInformationValue positionValue;
				positionValue.type = GridInformationType.Integer;
				positionValue.data = this.m_PositionIntValues[i];
				this.m_PositionProperties.Add(this.m_PositionIntKeys[i], positionValue);
			}
			for (int j = 0; j != Math.Min(this.m_PositionStringKeys.Count, this.m_PositionStringValues.Count); j++)
			{
				GridInformation.GridInformationValue positionValue2;
				positionValue2.type = GridInformationType.String;
				positionValue2.data = this.m_PositionStringValues[j];
				this.m_PositionProperties.Add(this.m_PositionStringKeys[j], positionValue2);
			}
			for (int k = 0; k != Math.Min(this.m_PositionFloatKeys.Count, this.m_PositionFloatValues.Count); k++)
			{
				GridInformation.GridInformationValue positionValue3;
				positionValue3.type = GridInformationType.Float;
				positionValue3.data = this.m_PositionFloatValues[k];
				this.m_PositionProperties.Add(this.m_PositionFloatKeys[k], positionValue3);
			}
			for (int l = 0; l != Math.Min(this.m_PositionDoubleKeys.Count, this.m_PositionDoubleValues.Count); l++)
			{
				GridInformation.GridInformationValue positionValue4;
				positionValue4.type = GridInformationType.Double;
				positionValue4.data = this.m_PositionDoubleValues[l];
				this.m_PositionProperties.Add(this.m_PositionDoubleKeys[l], positionValue4);
			}
			for (int m = 0; m != Math.Min(this.m_PositionObjectKeys.Count, this.m_PositionObjectValues.Count); m++)
			{
				GridInformation.GridInformationValue positionValue5;
				positionValue5.type = GridInformationType.UnityObject;
				positionValue5.data = this.m_PositionObjectValues[m];
				this.m_PositionProperties.Add(this.m_PositionObjectKeys[m], positionValue5);
			}
			for (int n = 0; n != Math.Min(this.m_PositionColorKeys.Count, this.m_PositionColorValues.Count); n++)
			{
				GridInformation.GridInformationValue positionValue6;
				positionValue6.type = GridInformationType.Color;
				positionValue6.data = this.m_PositionColorValues[n];
				this.m_PositionProperties.Add(this.m_PositionColorKeys[n], positionValue6);
			}
		}

		 
		public bool SetPositionProperty<T>(Vector3Int position, string name, T positionProperty)
		{
			throw new NotImplementedException("Storing this type is not accepted in GridInformation");
		}

		 
		public bool SetPositionProperty(Vector3Int position, string name, int positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.Integer, positionProperty);
		}

		 
		public bool SetPositionProperty(Vector3Int position, string name, string positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.String, positionProperty);
		}

		 
		public bool SetPositionProperty(Vector3Int position, string name, float positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.Float, positionProperty);
		}

		 
		public bool SetPositionProperty(Vector3Int position, string name, double positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.Double, positionProperty);
		}

		 
		public bool SetPositionProperty(Vector3Int position, string name, Object positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.UnityObject, positionProperty);
		}

		 
		public bool SetPositionProperty(Vector3Int position, string name, Color positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.Color, positionProperty);
		}

		 
		private bool SetPositionProperty(Vector3Int position, string name, GridInformationType dataType, object positionProperty)
		{
			Grid grid = base.GetComponentInParent<Grid>();
			bool flag = grid != null && positionProperty != null;
			bool result;
			if (flag)
			{
				GridInformation.GridInformationKey positionKey;
				positionKey.position = position;
				positionKey.name = name;
				GridInformation.GridInformationValue positionValue;
				positionValue.type = dataType;
				positionValue.data = positionProperty;
				this.m_PositionProperties[positionKey] = positionValue;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		 
		public T GetPositionProperty<T>(Vector3Int position, string name, T defaultValue) where T : Object
		{
			GridInformation.GridInformationKey positionKey;
			positionKey.position = position;
			positionKey.name = name;
			GridInformation.GridInformationValue positionValue;
			bool flag = this.m_PositionProperties.TryGetValue(positionKey, out positionValue);
			T result;
			if (flag)
			{
				bool flag2 = positionValue.type != GridInformationType.UnityObject;
				if (flag2)
				{
					throw new InvalidCastException("Value stored in GridInformation is not of the right type");
				}
				result = (positionValue.data as T);
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		 
		public int GetPositionProperty(Vector3Int position, string name, int defaultValue)
		{
			GridInformation.GridInformationKey positionKey;
			positionKey.position = position;
			positionKey.name = name;
			GridInformation.GridInformationValue positionValue;
			bool flag = this.m_PositionProperties.TryGetValue(positionKey, out positionValue);
			int result;
			if (flag)
			{
				bool flag2 = positionValue.type > GridInformationType.Integer;
				if (flag2)
				{
					throw new InvalidCastException("Value stored in GridInformation is not of the right type");
				}
				result = (int)positionValue.data;
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		 
		public string GetPositionProperty(Vector3Int position, string name, string defaultValue)
		{
			GridInformation.GridInformationKey positionKey;
			positionKey.position = position;
			positionKey.name = name;
			GridInformation.GridInformationValue positionValue;
			bool flag = this.m_PositionProperties.TryGetValue(positionKey, out positionValue);
			string result;
			if (flag)
			{
				bool flag2 = positionValue.type != GridInformationType.String;
				if (flag2)
				{
					throw new InvalidCastException("Value stored in GridInformation is not of the right type");
				}
				result = (string)positionValue.data;
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		 
		public float GetPositionProperty(Vector3Int position, string name, float defaultValue)
		{
			GridInformation.GridInformationKey positionKey;
			positionKey.position = position;
			positionKey.name = name;
			GridInformation.GridInformationValue positionValue;
			bool flag = this.m_PositionProperties.TryGetValue(positionKey, out positionValue);
			float result;
			if (flag)
			{
				bool flag2 = positionValue.type != GridInformationType.Float;
				if (flag2)
				{
					throw new InvalidCastException("Value stored in GridInformation is not of the right type");
				}
				result = (float)positionValue.data;
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		 
		public double GetPositionProperty(Vector3Int position, string name, double defaultValue)
		{
			GridInformation.GridInformationKey positionKey;
			positionKey.position = position;
			positionKey.name = name;
			GridInformation.GridInformationValue positionValue;
			bool flag = this.m_PositionProperties.TryGetValue(positionKey, out positionValue);
			double result;
			if (flag)
			{
				bool flag2 = positionValue.type != GridInformationType.Double;
				if (flag2)
				{
					throw new InvalidCastException("Value stored in GridInformation is not of the right type");
				}
				result = (double)positionValue.data;
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		 
		public Color GetPositionProperty(Vector3Int position, string name, Color defaultValue)
		{
			GridInformation.GridInformationKey positionKey;
			positionKey.position = position;
			positionKey.name = name;
			GridInformation.GridInformationValue positionValue;
			bool flag = this.m_PositionProperties.TryGetValue(positionKey, out positionValue);
			Color result;
			if (flag)
			{
				bool flag2 = positionValue.type != GridInformationType.Color;
				if (flag2)
				{
					throw new InvalidCastException("Value stored in GridInformation is not of the right type");
				}
				result = (Color)positionValue.data;
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		 
		public bool ErasePositionProperty(Vector3Int position, string name)
		{
			GridInformation.GridInformationKey positionKey;
			positionKey.position = position;
			positionKey.name = name;
			return this.m_PositionProperties.Remove(positionKey);
		}

		 
		public virtual void Reset()
		{
			this.m_PositionProperties.Clear();
		}

		 
		public Vector3Int[] GetAllPositions(string propertyName)
		{
			return (from x in this.m_PositionProperties.Keys.ToList<GridInformation.GridInformationKey>().FindAll((GridInformation.GridInformationKey x) => x.name == propertyName)
			select x.position).ToArray<Vector3Int>();
		}

		
		private Dictionary<GridInformation.GridInformationKey, GridInformation.GridInformationValue> m_PositionProperties = new Dictionary<GridInformation.GridInformationKey, GridInformation.GridInformationValue>();

		
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionIntKeys = new List<GridInformation.GridInformationKey>();

		
		[SerializeField]
		[HideInInspector]
		private List<int> m_PositionIntValues = new List<int>();

		
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionStringKeys = new List<GridInformation.GridInformationKey>();

		
		[SerializeField]
		[HideInInspector]
		private List<string> m_PositionStringValues = new List<string>();

		
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionFloatKeys = new List<GridInformation.GridInformationKey>();

		
		[SerializeField]
		[HideInInspector]
		private List<float> m_PositionFloatValues = new List<float>();

		
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionDoubleKeys = new List<GridInformation.GridInformationKey>();

		
		[SerializeField]
		[HideInInspector]
		private List<double> m_PositionDoubleValues = new List<double>();

		
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionObjectKeys = new List<GridInformation.GridInformationKey>();

		
		[SerializeField]
		[HideInInspector]
		private List<Object> m_PositionObjectValues = new List<Object>();

		
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionColorKeys = new List<GridInformation.GridInformationKey>();

		
		[SerializeField]
		[HideInInspector]
		private List<Color> m_PositionColorValues = new List<Color>();

		
		internal struct GridInformationValue
		{
			
			public GridInformationType type;

			
			public object data;
		}

		
		[Serializable]
		internal struct GridInformationKey
		{
			
			public Vector3Int position;

			
			public string name;
		}
	}
}
