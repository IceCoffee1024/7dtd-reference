using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003A RID: 58
[AddComponentMenu("NGUI/Examples/Equip Random Item")]
public class EquipRandomItem : MonoBehaviour
{
	// Token: 0x06000156 RID: 342 RVA: 0x0000EA50 File Offset: 0x0000CC50
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnClick()
	{
		if (this.equipment == null)
		{
			return;
		}
		List<InvBaseItem> items = InvDatabase.list[0].items;
		if (items.Count == 0)
		{
			return;
		}
		int num = 12;
		int num2 = global::UnityEngine.Random.Range(0, items.Count);
		InvBaseItem invBaseItem = items[num2];
		InvGameItem invGameItem = new InvGameItem(num2, invBaseItem);
		invGameItem.quality = (InvGameItem.Quality)global::UnityEngine.Random.Range(0, num);
		invGameItem.itemLevel = NGUITools.RandomRange(invBaseItem.minItemLevel, invBaseItem.maxItemLevel);
		this.equipment.Equip(invGameItem);
	}

	// Token: 0x040001E9 RID: 489
	public InvEquipment equipment;
}
