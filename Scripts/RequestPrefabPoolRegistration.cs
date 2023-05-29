using Unity.Entities;
using Unity.Collections;

public partial struct PrefabSystem
{
	/// <summary>
	/// Request to register multiple prefabs.
	/// Consumed by <seealso cref="RegistrationSystem"/>.
	/// </summary>
	public struct RequestPrefabPoolRegistration : IBufferElementData
	{
		public FixedString64Bytes PrefabID;
		public Entity Prefab;
	}
}
