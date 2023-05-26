#if UNITY_EDITOR
using UnityEngine;
using Unity.Entities;

[DisallowMultipleComponent]
public class PrefabAuthoring : MonoBehaviour
{
	public class Baker : Baker<PrefabAuthoring>
	{
		public override void Bake ( PrefabAuthoring authoring )
		{
			Entity entity = this.GetEntity( authoring , TransformUsageFlags.None );

			AddComponent<PrefabSystem.RequestPrefabRegistration>( entity );
			AddComponent<Prefab>( entity );
		}
	}

}
#endif
