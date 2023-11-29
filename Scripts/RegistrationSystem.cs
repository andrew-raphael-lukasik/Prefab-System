using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

using Debug = UnityEngine.Debug;

public partial struct PrefabSystem
{
	/// <summary>
	/// System that adds entries to <seealso cref="Prefabs"/> singleton.
	/// </summary>
	[WorldSystemFilter( WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ClientSimulation )]
	[UpdateInGroup( typeof(InitializationSystemGroup) , OrderFirst=true )]
	[UpdateAfter( typeof(SingletonLifetimeSystem) )]
	[RequireMatchingQueriesForUpdate]
	public partial struct RegistrationSystem : ISystem
	{

		[Unity.Burst.BurstCompile]
		public void OnCreate ( ref SystemState state ) {}

		[Unity.Burst.BurstCompile]
		public void OnDestroy ( ref SystemState state ) {}

		[Unity.Burst.BurstCompile]
		public void OnUpdate ( ref SystemState state )
		{
			var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer( state.WorldUnmanaged );
			var singleton = SystemAPI.GetSingleton<Prefabs>();
			var prefabs = singleton.Registry;

			state.Dependency = new RegisterPrefabPoolJob
			{
				ECB = ecb ,
				Prefabs = prefabs ,
				WorldName = state.WorldUnmanaged.Name ,
			}.Schedule( JobHandle.CombineDependencies(state.Dependency,singleton.Dependency) );
			singleton.Dependency = state.Dependency;

			state.Dependency = new RegisterPrefabJob{
				ECB = ecb ,
				Prefabs = prefabs ,
				WorldName = state.WorldUnmanaged.Name ,
			}.Schedule( JobHandle.CombineDependencies(state.Dependency,singleton.Dependency) );
			singleton.Dependency = state.Dependency;

			SystemAPI.SetSingleton( singleton );// updates singleton.Dependency
		}

		[Unity.Burst.BurstCompile]
		partial struct RegisterPrefabPoolJob : IJobEntity
		{
			public EntityCommandBuffer ECB;
			public NativeHashMap<FixedString64Bytes,Entity> Prefabs;
			[ReadOnly] public FixedString128Bytes WorldName;
			public void Execute ( in Entity entity , in DynamicBuffer<RequestPrefabPoolRegistration> buffer )
			{
				int numBefore = Prefabs.Count;
				foreach( var request in buffer )
				{
					if( Prefabs.TryAdd( request.PrefabID , request.Prefab ) )
						Debug.Log($"{WorldName} RegisterPrefabPoolJob: '{request.PrefabID}' prefab registered successfully.");
					else
						Debug.LogWarning($"{WorldName} RegisterPrefabPoolJob: '{request.PrefabID}' prefab registration failed!");
				}
				int numAfter = Prefabs.Count;

				ECB.RemoveComponent<RequestPrefabPoolRegistration>( entity );
				ECB.DestroyEntity( entity );

				Debug.Log($"{WorldName} RegisterPrefabPoolJob: RequestPrefabPoolRegistration processed, {numAfter-numBefore} prefabs added");
			}
		}

		[Unity.Burst.BurstCompile]
		[WithAll( typeof(Prefab) )]
		[WithNone( typeof(PrefabSystemID) )]
		partial struct RegisterPrefabJob : IJobEntity
		{
			public EntityCommandBuffer ECB;
			public NativeHashMap<FixedString64Bytes,Entity> Prefabs;
			[ReadOnly] public FixedString128Bytes WorldName;
			public void Execute ( in Entity entity , in RequestPrefabRegistration request )
			{
				if( Prefabs.TryAdd(request.PrefabID,entity) )
					Debug.Log($"{WorldName} RegisterPrefabJob: '{request.PrefabID}' prefab registered successfully.");
				else
					Debug.LogWarning($"{WorldName} RegisterPrefabJob: '{request.PrefabID}' prefab registration failed!");
				
				ECB.AddComponent( entity , new PrefabSystemID{ Value = request.PrefabID } );
				ECB.RemoveComponent<RequestPrefabRegistration>( entity );
			}
		}
	}
}
