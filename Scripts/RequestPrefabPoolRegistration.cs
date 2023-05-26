using Unity.Entities;
using Unity.Collections;

namespace ECSPrefabLookup
{
	public struct RequestPrefabPoolRegistration : IBufferElementData
	{
		public FixedString64Bytes ID;
		public Entity Prefab;
	}
}
