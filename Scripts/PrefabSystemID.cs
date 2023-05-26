using Unity.Entities;
using Unity.Collections;

namespace ECSPrefabLookup
{
	public struct PrefabSystemID : ICleanupComponentData
	{
		public FixedString64Bytes Value;
	}
}
