using System;
using UnityEngine.Scripting;

// Token: 0x02000607 RID: 1543
[Preserve]
public class ItemClassQuest : ItemClass
{
	// Token: 0x0600325D RID: 12893 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanDrop(ItemValue _iv = null)
	{
		return false;
	}

	// Token: 0x0600325E RID: 12894 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanStack()
	{
		return false;
	}

	// Token: 0x0600325F RID: 12895 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool KeepOnDeath()
	{
		return true;
	}

	// Token: 0x06003260 RID: 12896 RVA: 0x00146913 File Offset: 0x00144B13
	public new static void Cleanup()
	{
		ItemClassQuest.questItemList = null;
	}

	// Token: 0x06003261 RID: 12897 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanPlaceInContainer()
	{
		return false;
	}

	// Token: 0x06003262 RID: 12898 RVA: 0x0014691B File Offset: 0x00144B1B
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

	// Token: 0x040027AE RID: 10158
	public static ItemClassQuest[] questItemList = new ItemClassQuest[100];
}
