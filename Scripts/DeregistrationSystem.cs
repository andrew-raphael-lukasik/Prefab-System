using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

public partial struct PrefabSystem
{
	/// <summary>
	/// System that removes entries from <seealso cref="Prefabs"/> singleton.
	/// </summary>
	[WorldSystemFilter( WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ClientSimulation )]
	[UpdateInGroup( typeof(InitializationSystemGroup) , OrderFirst=true )]
	[UpdateAfter( typeof(SingletonLifetimeSystem) )]
	[RequireMatchingQueriesForUpdate]
	public partial struct DeregistrationSystem : ISystem
	{
		[Unity.Burst.BurstCompile]
		public void OnCreate ( ref SystemState state )
		{
			state.RequireForUpdate<Prefabs>();
		}

		[Unity.Burst.BurstCompile]
		public void OnDestroy ( ref SystemState state )
		{
			
		}

		[Unity.Burst.BurstCompile]
		public void OnUpdate ( ref SystemState state )
		{
			var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer( state.WorldUnmanaged );
			var singleton = SystemAPI.GetSingleton<Prefabs>();
			var prefabs = singleton.Registry;
			
			state.Dependency = new DeregisterPrefabJob
			{
				ECB = ecb ,
				Prefabs = prefabs ,
			}.Schedule( JobHandle.CombineDependencies(state.Dependency,singleton.Dependency) );
			singleton.Dependency = state.Dependency;

			SystemAPI.SetSingleton( singleton );// updates singleton.Dependency
		}

		[Unity.Burst.BurstCompile]
		[WithNone( typeof(Prefab) )]
		partial struct DeregisterPrefabJob : IJobEntity
		{
			public EntityCommandBuffer ECB;
			public NativeHashMap<FixedString64Bytes,Entity> Prefabs;
			public void Execute ( in Entity entity , in PrefabSystemID prefabID )
			{
				Prefabs.Remove( prefabID.Value );
				ECB.DestroyEntity( entity );
			}
		}
	}

}
