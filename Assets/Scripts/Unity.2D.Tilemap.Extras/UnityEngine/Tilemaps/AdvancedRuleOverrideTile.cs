using System;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Tilemaps
{
	
	[MovedFrom(true, "UnityEngine", null, null)]
	[CreateAssetMenu(fileName = "New Advanced Rule Override Tile", menuName = "Tiles/Advanced Rule Override Tile")]
	[Serializable]
	public class AdvancedRuleOverrideTile : RuleOverrideTile
	{
		
		public RuleTile.TilingRuleOutput this[RuleTile.TilingRule originalRule]
		{
			get
			{
				foreach (RuleTile.TilingRuleOutput overrideRule in this.m_OverrideTilingRules)
				{
					bool flag = overrideRule.m_Id == originalRule.m_Id;
					if (flag)
					{
						return overrideRule;
					}
				}
				return null;
			}
			set
			{
				for (int i = this.m_OverrideTilingRules.Count - 1; i >= 0; i--)
				{
					bool flag = this.m_OverrideTilingRules[i].m_Id == originalRule.m_Id;
					if (flag)
					{
						this.m_OverrideTilingRules.RemoveAt(i);
						break;
					}
				}
				bool flag2 = value != null;
				if (flag2)
				{
					string json = JsonUtility.ToJson(value);
					RuleTile.TilingRuleOutput overrideRule = JsonUtility.FromJson<RuleTile.TilingRuleOutput>(json);
					this.m_OverrideTilingRules.Add(overrideRule);
				}
			}
		}

		 
		public void ApplyOverrides(IList<KeyValuePair<RuleTile.TilingRule, RuleTile.TilingRuleOutput>> overrides)
		{
			bool flag = overrides == null;
			if (flag)
			{
				throw new ArgumentNullException("overrides");
			}
			for (int i = 0; i < overrides.Count; i++)
			{
				this[overrides[i].Key] = overrides[i].Value;
			}
		}

		 
		public void GetOverrides(List<KeyValuePair<RuleTile.TilingRule, RuleTile.TilingRuleOutput>> overrides, ref int validCount)
		{
			bool flag = overrides == null;
			if (flag)
			{
				throw new ArgumentNullException("overrides");
			}
			overrides.Clear();
			bool flag2 = this.m_Tile;
			if (flag2)
			{
				foreach (RuleTile.TilingRule originalRule in this.m_Tile.m_TilingRules)
				{
					RuleTile.TilingRuleOutput overrideRule2 = this[originalRule];
					overrides.Add(new KeyValuePair<RuleTile.TilingRule, RuleTile.TilingRuleOutput>(originalRule, overrideRule2));
				}
			}
			validCount = overrides.Count;
			using (List<RuleTile.TilingRuleOutput>.Enumerator enumerator2 = this.m_OverrideTilingRules.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RuleTile.TilingRuleOutput overrideRule = enumerator2.Current;
					bool flag3 = !overrides.Exists((KeyValuePair<RuleTile.TilingRule, RuleTile.TilingRuleOutput> o) => o.Key.m_Id == overrideRule.m_Id);
					if (flag3)
					{
						RuleTile.TilingRule originalRule2 = new RuleTile.TilingRule
						{
							m_Id = overrideRule.m_Id
						};
						overrides.Add(new KeyValuePair<RuleTile.TilingRule, RuleTile.TilingRuleOutput>(originalRule2, overrideRule));
					}
				}
			}
		}

		 
		public override void Override()
		{
			bool flag = !this.m_Tile || !this.m_InstanceTile;
			if (!flag)
			{
				base.PrepareOverride();
				RuleTile tile = this.m_InstanceTile;
				tile.m_DefaultSprite = this.m_DefaultSprite;
				tile.m_DefaultGameObject = this.m_DefaultGameObject;
				tile.m_DefaultColliderType = this.m_DefaultColliderType;
				foreach (RuleTile.TilingRule rule in tile.m_TilingRules)
				{
					RuleTile.TilingRuleOutput overrideRule = this[rule];
					bool flag2 = overrideRule != null;
					if (flag2)
					{
						JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(overrideRule), rule);
					}
				}
			}
		}

		
		public Sprite m_DefaultSprite;

		
		public GameObject m_DefaultGameObject;

		
		public Tile.ColliderType m_DefaultColliderType = Tile.ColliderType.Sprite;

		
		public List<RuleTile.TilingRuleOutput> m_OverrideTilingRules = new List<RuleTile.TilingRuleOutput>();
	}
}
