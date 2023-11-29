using Unity.Entities;
using Unity.Collections;

public partial struct PrefabSystem
{
	/// <summary>
	/// System that creates <seealso cref="Prefabs"/> singleton.
	/// </summary>
	[WorldSystemFilter( WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ClientSimulation )]
	[UpdateInGroup( typeof(InitializationSystemGroup) , OrderFirst=true )]
	public partial struct SingletonLifetimeSystem : ISystem
	{

		[Unity.Burst.BurstCompile]
		public void OnCreate ( ref SystemState state )
		{
			Entity singleton = state.EntityManager.CreateEntity();
			state.EntityManager.AddComponent<Prefabs>( singleton );
			SystemAPI.SetSingleton( new Prefabs{
				Registry = new NativeHashMap<FixedString64Bytes,Entity>( 128 , Allocator.Persistent )
			} );
			state.Enabled = false;
		}

		[Unity.Burst.BurstCompile]
		public void OnDestroy ( ref SystemState state )
		{
			if( SystemAPI.TryGetSingletonEntity<Prefabs>(out Entity singleton) )
			{
				var prefabs = SystemAPI.GetSingleton<Prefabs>();
				prefabs.Dependency.Complete();
				prefabs.Registry.Dispose();
				state.EntityManager.DestroyEntity( singleton );
			}
		}

		[Unity.Burst.BurstCompile]
		public void OnUpdate ( ref SystemState state ) {}

	}
}
