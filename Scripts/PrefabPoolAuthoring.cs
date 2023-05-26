#if UNITY_EDITOR
using UnityEngine;
using Unity.Entities;

namespace ECSPrefabLookup
{
	[DisallowMultipleComponent]
	[AddComponentMenu( nameof(ECSPrefabLookup)+"/Prefab Pool Authoring" )]
	public class PrefabPoolAuthoring : MonoBehaviour
	{
		[SerializeField] GameObject[] _prefabs = new GameObject[0];
		public class Baker : Baker<PrefabPoolAuthoring>
		{
			public override void Bake ( PrefabPoolAuthoring authoring )
			{
				Entity entity = this.GetEntity( authoring , TransformUsageFlags.None );

				var pool = AddBuffer<RequestPrefabPoolRegistration>( entity );
				foreach( var go in authoring._prefabs )
					pool.Add( new RequestPrefabPoolRegistration{ ID=go.name , Prefab=GetEntity(go,TransformUsageFlags.None) } );
			}
		}
	}
	#endif
}
