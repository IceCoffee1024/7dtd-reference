using System;
using UnityEngine;

// Token: 0x02000523 RID: 1315
[CreateAssetMenu(menuName = "Cosmetics/Cosmetic Set")]
public class CosmeticSet : ScriptableObject
{
	// Token: 0x040020EB RID: 8427
	[Tooltip("Becomes the EntitlementSetEnum member name. Valid C# identifier, unique. This exact string is what the DLC catalog, drops.xml, news URLs, XML patch conditions and analytics use.")]
	public string EntitlementSetName;

	// Token: 0x040020EC RID: 8428
	[Tooltip("Addressables path prefix for this set's assets, e.g. DLC/MyCosmetic")]
	public string AddressablePathPrefix;

	// Token: 0x040020ED RID: 8429
	[Tooltip("Keeps this set out of the game while reserving everything about it. The enum member is written commented out, the addressable prefix is not registered, and the items are not added to the cosmetic map, so nothing in game can list, unlock or equip them. The enum ordinal and the item ids stay reserved, so ticking this on and off never renumbers anything. Use it for a set that exists for store or marketing setup before it ships. Note the art still builds, and items that are in items.xml still appear in the cosmetics UI, so keep the XML out too if it must be unseen.")]
	public bool ExcludeFromGame;

	// Token: 0x040020EE RID: 8430
	public CosmeticSet.Item[] Items = Array.Empty<CosmeticSet.Item>();

	// Token: 0x02000524 RID: 1316
	[Serializable]
	public struct Item
	{
		// Token: 0x040020EF RID: 8431
		public string ItemName;

		// Token: 0x040020F0 RID: 8432
		[InspectorReadOnly]
		[Tooltip("Written into save games. Assigned once by the baker, never change it.")]
		public int CosmeticId;
	}
}
