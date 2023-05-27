using Unity.Entities;
using Unity.Collections;

namespace ECSPrefabLookup
{
	/// <summary>
	/// Inner prefab ID data for <seealso cref="PrefabSystem"/>
	/// </summary>
	public struct PrefabSystemID : ICleanupComponentData
	{
		public FixedString64Bytes Value;
	}
}
