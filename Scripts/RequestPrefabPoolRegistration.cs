using Unity.Entities;
using Unity.Collections;

namespace ECSPrefabLookup
{
	/// <summary>
	/// Request to register multiple prefabs.
	/// Consumed by <seealso cref="PrefabSystem"/>.
	/// </summary>
	public struct RequestPrefabPoolRegistration : IBufferElementData
	{
		public FixedString64Bytes ID;
		public Entity Prefab;
	}
}
