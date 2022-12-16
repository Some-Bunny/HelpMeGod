using System;

namespace UnityEngine
{
	
	public class IsometricRuleTile<T> : IsometricRuleTile
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
