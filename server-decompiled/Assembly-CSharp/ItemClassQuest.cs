using System;
using UnityEngine.Scripting;

// Token: 0x02000600 RID: 1536
[Preserve]
public class ItemClassQuest : ItemClass
{
	// Token: 0x0600324F RID: 12879 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanDrop(ItemValue _iv = null)
	{
		return false;
	}

	// Token: 0x06003250 RID: 12880 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanStack()
	{
		return false;
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool KeepOnDeath()
	{
		return true;
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x00145D1F File Offset: 0x00143F1F
	public new static void Cleanup()
	{
		ItemClassQuest.questItemList = null;
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanPlaceInContainer()
	{
		return false;
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x00145D27 File Offset: 0x00143F27
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

	// Token: 0x04002793 RID: 10131
	public static ItemClassQuest[] questItemList = new ItemClassQuest[100];
}
