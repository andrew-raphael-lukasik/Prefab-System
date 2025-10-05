using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

using Debug = UnityEngine.Debug;

namespace PrefabSystem.Systems
{
    /// <summary>
    /// Converts <seealso cref="RegisterPrefab"/> entries into <seealso cref="Prefabs"/> entries.
    /// </summary>
    /// <remarks>
    /// Hosts <seealso cref="Prefabs"/> singleton.
    /// </remarks>
    [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.Presentation | WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
    [RequireMatchingQueriesForUpdate]
    [Unity.Burst.BurstCompile]
    public partial struct PrefabSystem : ISystem
    {
        EntityQuery _query;

        [Unity.Burst.BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _query = new EntityQueryBuilder(Allocator.Persistent)
                .WithAll<RegisterPrefab, Prefab>()
                .Build(ref state);

            state.RequireForUpdate(_query);
        }

        [Unity.Burst.BurstCompile]
        void ISystem.OnDestroy(ref SystemState state)
        {
            if (SystemAPI.TryGetSingletonRW<Prefabs>(out var prefabsRef))
            {
                prefabsRef.ValueRW.Dependency.Complete();
                if (prefabsRef.ValueRW.Lookup.IsCreated) prefabsRef.ValueRW.Lookup.Dispose();
            }
        }

        [Unity.Burst.BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.HasSingleton<Prefabs>())
            {
                state.EntityManager.AddComponent<Prefabs>(state.SystemHandle);
                SystemAPI.SetSingleton(new Prefabs{
                    Lookup = new NativeHashMap<FixedString64Bytes,Entity>(128, Allocator.Persistent),
                    Dependency = default,
                });
            }

            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var singletonRef = SystemAPI.GetSingletonRW<Prefabs>();

            state.Dependency = JobHandle.CombineDependencies(state.Dependency, singletonRef.ValueRW.Dependency);
            state.Dependency = new RegisterPrefabJob{
                ECB             = ecb,
                Prefabs         = singletonRef.ValueRW.Lookup,
                WorldName       = state.WorldUnmanaged.Name,
            }.Schedule(state.Dependency);
            singletonRef.ValueRW.Dependency = state.Dependency;

            state.Dependency.Complete();
            if (ecb.ShouldPlayback) ecb.Playback(state.EntityManager);
        }

        [Unity.Burst.BurstCompile]
        [WithAll(typeof(Prefab))]
        [WithAbsent(typeof(PrefabID))]
        partial struct RegisterPrefabJob : IJobEntity
        {
            public EntityCommandBuffer ECB;
            public NativeHashMap<FixedString64Bytes,Entity> Prefabs;
            [ReadOnly] public FixedString128Bytes WorldName;
            public void Execute(in Entity entity, in RegisterPrefab request)
            {
                if( Prefabs.TryAdd(request.Key,entity) )
                {
                    #if DEBUG && !PREFAB_SYSTEM_DISABLE_LOG_MESSAGES
                    Debug.Log($"{WorldName} {DebugName}: '{request.Key}' prefab registered successfully.");
                    #endif
                }
                #if DEBUG && !PREFAB_SYSTEM_DISABLE_LOG_MESSAGES
                else if (Prefabs.ContainsKey(request.Key))
                    Debug.LogWarning($"{WorldName} {DebugName}: prefab under '{request.Key}' ID was registrated already");
                else
                    Debug.LogWarning($"{WorldName} {DebugName}: '{request.Key}' prefab registration failed");
                #endif

                ECB.AddComponent(entity, new PrefabID{
                    Key = request.Key
                });
                ECB.RemoveComponent<RegisterPrefab>(entity);
            }
            public static FixedString64Bytes DebugName {get;} = nameof(RegisterPrefabJob);
        }
    }

    /// <summary>
    /// Inner prefab ID data for <seealso cref="PrefabSystem"/> and <seealso cref="PrefabDestroyedSystem"/> systems.
    /// </summary>
    struct PrefabID : ICleanupComponentData
    {
        public FixedString64Bytes Key;
    }

}
