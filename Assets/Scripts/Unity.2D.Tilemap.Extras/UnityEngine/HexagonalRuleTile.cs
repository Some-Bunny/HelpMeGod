using System;

namespace UnityEngine
{
	
	public class HexagonalRuleTile<T> : HexagonalRuleTile
	{
		

		public sealed override Type m_NeighborType
		{
			get
			{
				return typeof(T);
			}
		}
	}
}
