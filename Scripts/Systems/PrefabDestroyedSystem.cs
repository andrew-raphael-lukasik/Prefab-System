using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

using Debug = UnityEngine.Debug;

namespace PrefabSystem.Systems
{
    /// <summary>
    /// Removes entries from <seealso cref="Prefabs"/> when registered prefab entity no longer exists.
    /// </summary>
    [WorldSystemFilter(WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.Editor)]
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
    [RequireMatchingQueriesForUpdate]
    [Unity.Burst.BurstCompile]
    public partial struct PrefabDestroyedSystem : ISystem
    {
        [Unity.Burst.BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Prefabs>();
            state.RequireForUpdate<PrefabID>();
        }

        [Unity.Burst.BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var prefabs = SystemAPI.GetSingleton<Prefabs>();
            var lookup = prefabs.Lookup;
            
            state.Dependency = JobHandle.CombineDependencies(state.Dependency, prefabs.Dependency);
            state.Dependency = new UnregisterPrefabJob{
                ECB     = ecb,
                Prefabs = lookup,
            }.Schedule(state.Dependency);

            SystemAPI.SetSingleton(prefabs);// updates singleton.Dependency
        }

        [Unity.Burst.BurstCompile]
        [WithAbsent(typeof(Prefab))]
        partial struct UnregisterPrefabJob : IJobEntity
        {
            public EntityCommandBuffer ECB;
            public NativeHashMap<FixedString64Bytes,Entity> Prefabs;
            public void Execute(in Entity entity, in PrefabID prefabID)
            {
                if (Prefabs.TryGetValue(prefabID.Key, out Entity prefab))
                {
                    if (prefab==entity)
                    {
                        Prefabs.Remove(prefabID.Key);
                        Debug.Log($"{DebugName}: prefab with '{prefabID.Key}' ID detected as destroyed, unregistering.");
                    }
                    else
                    {
                        Debug.Log($"{DebugName}: prefab with '{prefabID.Key}' ID detected as destroyed, but registered & destroyed entities do not match - ignoring.");
                    }

                    ECB.RemoveComponent<PrefabID>(entity);
                }
            }
            public static FixedString64Bytes DebugName {get;} = nameof(UnregisterPrefabJob);
        }
    }

}
