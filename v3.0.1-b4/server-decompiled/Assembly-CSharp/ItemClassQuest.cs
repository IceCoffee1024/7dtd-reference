using System;
using UnityEngine.Scripting;

// Token: 0x020005F7 RID: 1527
[Preserve]
public class ItemClassQuest : ItemClass
{
	// Token: 0x060031BD RID: 12733 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanDrop(ItemValue _iv = null)
	{
		return false;
	}

	// Token: 0x060031BE RID: 12734 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanStack()
	{
		return false;
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool KeepOnDeath()
	{
		return true;
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x00143503 File Offset: 0x00141703
	public new static void Cleanup()
	{
		ItemClassQuest.questItemList = null;
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanPlaceInContainer()
	{
		return false;
	}

	// Token: 0x060031C2 RID: 12738 RVA: 0x0014350B File Offset: 0x0014170B
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

	// Token: 0x04002739 RID: 10041
	public static ItemClassQuest[] questItemList = new ItemClassQuest[100];
}
