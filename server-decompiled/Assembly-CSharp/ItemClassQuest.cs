using System;
using UnityEngine.Scripting;

// Token: 0x020005FE RID: 1534
[Preserve]
public class ItemClassQuest : ItemClass
{
	// Token: 0x0600320C RID: 12812 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanDrop(ItemValue _iv = null)
	{
		return false;
	}

	// Token: 0x0600320D RID: 12813 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanStack()
	{
		return false;
	}

	// Token: 0x0600320E RID: 12814 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool KeepOnDeath()
	{
		return true;
	}

	// Token: 0x0600320F RID: 12815 RVA: 0x00145347 File Offset: 0x00143547
	public new static void Cleanup()
	{
		ItemClassQuest.questItemList = null;
	}

	// Token: 0x06003210 RID: 12816 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanPlaceInContainer()
	{
		return false;
	}

	// Token: 0x06003211 RID: 12817 RVA: 0x0014534F File Offset: 0x0014354F
	public static ItemClassQuest GetItemQuestById(ushort _questTypeID)
	{
		if (ItemClassQuest.questItemList == null)
		{
			return null;
		}
		if (_questTypeID < 0 || (int)_questTypeID >= ItemClassQuest.questItemList.Length)
		{
			return null;
		}
		return ItemClassQuest.questItemList[(int)_questTypeID];
	}

	// Token: 0x0400279C RID: 10140
	public static ItemClassQuest[] questItemList = new ItemClassQuest[100];
}
