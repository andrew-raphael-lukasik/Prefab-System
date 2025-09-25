#if UNITY_EDITOR
using UnityEngine;
using Unity.Entities;

using PrefabSystem.Systems;

namespace PrefabSystem.Authoring
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Prefab System/Prefab Pool Authoring")]
    public class PrefabPoolAuthoring : MonoBehaviour
    {
        [SerializeField] GameObject[] _prefabs = new GameObject[0];
        public class Baker : Baker<PrefabPoolAuthoring>
        {
            public override void Bake(PrefabPoolAuthoring authoring)
            {
                if (authoring._prefabs==null || authoring._prefabs.Length==0)
                    return;

                Entity entity = GetEntity(authoring, TransformUsageFlags.None);

                var pool = AddBuffer<PrefabPoolBakingData>(entity);
                foreach(var prefab in authoring._prefabs)
                {
                    pool.Add(new PrefabPoolBakingData{
                        Key     = prefab.name,
                        Prefab  = GetEntity(prefab, TransformUsageFlags.None),
                    });
                }
            }
        }
    }
}
#endif
