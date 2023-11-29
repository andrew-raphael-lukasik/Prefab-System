using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

public partial struct PrefabSystem
{
	/// <summary>
	/// Singleton that holds prefab lookup data.
	/// Created by <seealso cref="SingletonLifetimeSystem"/>.
	/// </summary>
	public struct Prefabs : IComponentData
	{
		public NativeHashMap<FixedString64Bytes,Entity> Registry;
		public JobHandle Dependency;
	}
}
