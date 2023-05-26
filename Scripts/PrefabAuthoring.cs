#if UNITY_EDITOR
using UnityEngine;
using Unity.Entities;

namespace ECSPrefabLookup
{
	[DisallowMultipleComponent]
	[AddComponentMenu( nameof(ECSPrefabLookup)+"/Prefab Authoring" )]
	public class PrefabAuthoring : MonoBehaviour
	{
		public class Baker : Baker<PrefabAuthoring>
		{
			public override void Bake ( PrefabAuthoring authoring )
			{
				Entity entity = this.GetEntity( authoring , TransformUsageFlags.None );

				AddComponent<RequestPrefabRegistration>( entity );
				AddComponent<Prefab>( entity );
			}
		}
	}
}
#endif
