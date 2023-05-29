#if UNITY_EDITOR
using UnityEngine;
using Unity.Entities;

[DisallowMultipleComponent]
[AddComponentMenu( "Prefab System/Prefab Authoring" )]
public class PrefabAuthoring : MonoBehaviour
{
	public class Baker : Baker<PrefabAuthoring>
	{
		public override void Bake ( PrefabAuthoring authoring )
		{
			Entity entity = this.GetEntity( authoring , TransformUsageFlags.None );

			AddComponent<Prefab>( entity );
			AddComponent( entity , new PrefabSystem.RequestPrefabRegistration{
				PrefabID = authoring.name
			} );
		}
	}
}
#endif
