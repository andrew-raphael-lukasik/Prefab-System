#if UNITY_EDITOR
using UnityEngine;
using Unity.Entities;

[DisallowMultipleComponent]
public class PrefabSystemPoolAuthoring : MonoBehaviour
{

	[SerializeField] GameObject[] _prefabs = new GameObject[0];

	public class Baker : Baker<PrefabSystemPoolAuthoring>
	{
		public override void Bake ( PrefabSystemPoolAuthoring authoring )
		{
			Entity entity = this.GetEntity( authoring , TransformUsageFlags.WorldSpace );
			var pool = AddBuffer<PrefabSystem.RequestPrefabPoolRegistration>( entity );
			foreach( var go in authoring._prefabs )
				pool.Add( new PrefabSystem.RequestPrefabPoolRegistration{ ID=go.name , Prefab=GetEntity(go,TransformUsageFlags.None) } );
		}
	}

}
#endif
