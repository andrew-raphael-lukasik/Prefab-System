using Unity.Entities;
using Unity.Collections;

using Debug = UnityEngine.Debug;

namespace PrefabSystem.Systems
{
    /// <summary>
    /// Converts <seealso cref="PrefabPoolBakingData"/> into regular <seealso cref="RegisterPrefab"/>.
    /// </summary>
    [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    [UpdateInGroup(typeof(BakingSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    [Unity.Burst.BurstCompile]
    public partial struct PrefabPoolBakingSystem : ISystem
    {
        [Unity.Burst.BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PrefabPoolBakingData>();
        }

        [Unity.Burst.BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (pool, entity) in SystemAPI.Query< DynamicBuffer<PrefabPoolBakingData>  >().WithEntityAccess())
            {
                foreach (var request in pool)
                {
                    ecb.AddComponent(request.Prefab, new RegisterPrefab{
                        Key = request.Key
                    });
                }
                ecb.RemoveComponent<PrefabPoolBakingData>(entity);
                ecb.DestroyEntity(entity);
            }
            if (ecb.ShouldPlayback) ecb.Playback(state.EntityManager);
        }
    }

    /// <summary>
    /// Request to register multiple prefabs.
    /// Processed by <seealso cref="PrefabPoolBakingSystem"/>.
    /// </summary>
    public struct PrefabPoolBakingData : IBufferElementData
    {
        public FixedString64Bytes Key;
        public Entity Prefab;
    }
}
