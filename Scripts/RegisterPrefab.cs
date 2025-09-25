using Unity.Entities;
using Unity.Collections;

namespace PrefabSystem
{
    /// <summary>
    /// Request to register a prefab.
    /// Consumed by <seealso cref="PrefabSystem"/>.
    /// </summary>
    public struct RegisterPrefab : IComponentData
    {
        public FixedString64Bytes Key;
    }
}
