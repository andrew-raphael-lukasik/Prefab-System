using Unity.Entities;
using Unity.Collections;

namespace ECSPrefabLookup
{
	public struct RequestPrefabRegistration : IComponentData
	{
		public FixedString64Bytes PrefabID;
	}
}
