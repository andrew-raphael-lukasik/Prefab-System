using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

namespace PrefabSystem
{
    /// <summary>
    /// Singleton that holds prefab lookup data.
    /// Created by <seealso cref="PrefabSystem"/>.
    /// </summary>
    public struct Prefabs : IComponentData
    {
        public NativeHashMap<FixedString64Bytes,Entity> Lookup;
        public JobHandle Dependency;

        [System.Obsolete("Renamed to `Lookup`")]
        public NativeHashMap<FixedString64Bytes,Entity> Registry => Lookup;
    }
}
