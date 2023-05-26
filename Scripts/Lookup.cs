using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

namespace ECSPrefabLookup
{
	public struct Lookup : IComponentData
	{
		public NativeHashMap<FixedString64Bytes,Entity> Prefabs;
		public JobHandle Dependency;
	}
}
