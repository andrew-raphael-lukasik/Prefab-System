#if UNITY_EDITOR
using UnityEngine;
using Unity.Entities;

namespace PrefabSystem.Authoring
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Prefab System/Prefab Authoring")]
    public class PrefabAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PrefabAuthoring>
        {
            public override void Bake(PrefabAuthoring authoring)
            {
                Entity entity = GetEntity(authoring, TransformUsageFlags.None);

                AddComponent<Prefab>(entity);
                AddComponent(entity, new RegisterPrefab{
                    Key = authoring.name
                });
            }
        }
    }
}
#endif
