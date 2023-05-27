using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

namespace ECSPrefabLookup
{
	/// <summary>
	/// Singleton that holds prefab lookup data.
	/// Maintained by <seealso cref="PrefabSystem"/>.
	/// </summary>
	public struct Lookup : IComponentData
	{
		public NativeHashMap<FixedString64Bytes,Entity> Prefabs;
		public JobHandle Dependency;
	}
}
