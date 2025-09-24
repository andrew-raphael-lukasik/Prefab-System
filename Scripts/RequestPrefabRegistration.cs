using Unity.Entities;
using Unity.Collections;

public partial struct PrefabSystem
{
	/// <summary>
	/// Request to register a prefab.
	/// Consumed by <seealso cref="PrefabRegistrationSystem"/>.
	/// </summary>
	public struct RequestPrefabRegistration : IComponentData
	{
		public FixedString64Bytes PrefabID;
	}
}
