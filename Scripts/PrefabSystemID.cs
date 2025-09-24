using Unity.Entities;
using Unity.Collections;

public partial struct PrefabSystem
{
	/// <summary>
	/// Inner prefab ID data for <seealso cref="PrefabRegistrationSystem"/> and <seealso cref="DeregistrationSystem"/> systems.
	/// </summary>
	public struct PrefabSystemID : ICleanupComponentData
	{
		public FixedString64Bytes Value;
	}
}
