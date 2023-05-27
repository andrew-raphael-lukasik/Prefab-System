using Unity.Entities;
using Unity.Collections;

namespace ECSPrefabLookup
{
	/// <summary>
	/// Request to register a prefab.
	/// Consumed by <seealso cref="PrefabSystem"/>.
	/// </summary>
	public struct RequestPrefabRegistration : IComponentData
	{
		public FixedString64Bytes PrefabID;
	}
}
