using Unity.Entities;
using Unity.Collections;

public partial struct PrefabSystem
{
	/// <summary>
	/// System that creates <seealso cref="Prefabs"/> singleton.
	/// </summary>
	[WorldSystemFilter( WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ClientSimulation )]
	[UpdateInGroup( typeof(InitializationSystemGroup) , OrderFirst=true )]
	[RequireMatchingQueriesForUpdate]
	public partial struct SingletonCreationSystem : ISystem
	{

		//[Unity.Burst.BurstCompile]
		public void OnCreate ( ref SystemState state )
		{
			Entity singleton = state.EntityManager.CreateEntity( ComponentType.ReadWrite<Prefabs>() );
			SystemAPI.SetSingleton( new Prefabs{
				Registry = new NativeHashMap<FixedString64Bytes,Entity>( 128 , Allocator.Persistent )
			} );
		}

		[Unity.Burst.BurstCompile]
		public void OnDestroy ( ref SystemState state )
		{
			if( SystemAPI.TryGetSingletonEntity<Prefabs>(out Entity singleton) )
				state.EntityManager.DestroyEntity( singleton );
		}

		[Unity.Burst.BurstCompile]
		public void OnUpdate ( ref SystemState state ) {}
	}
}
