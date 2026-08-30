using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000043 RID: 67
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Examples/Item Database")]
public class InvDatabase : MonoBehaviour
{
	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000176 RID: 374 RVA: 0x0000F498 File Offset: 0x0000D698
	public static InvDatabase[] list
	{
		get
		{
			if (InvDatabase.mIsDirty)
			{
				InvDatabase.mIsDirty = false;
				InvDatabase.mList = NGUITools.FindActive<InvDatabase>();
			}
			return InvDatabase.mList;
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000F4B6 File Offset: 0x0000D6B6
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		InvDatabase.mIsDirty = true;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000F4B6 File Offset: 0x0000D6B6
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		InvDatabase.mIsDirty = true;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000F4C0 File Offset: 0x0000D6C0
	[PublicizedFrom(EAccessModifier.Private)]
	public InvBaseItem GetItem(int id16)
	{
		int i = 0;
		int count = this.items.Count;
		while (i < count)
		{
			InvBaseItem invBaseItem = this.items[i];
			if (invBaseItem.id16 == id16)
			{
				return invBaseItem;
			}
			i++;
		}
		return null;
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000F500 File Offset: 0x0000D700
	[PublicizedFrom(EAccessModifier.Private)]
	public static InvDatabase GetDatabase(int dbID)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			if (invDatabase.databaseID == dbID)
			{
				return invDatabase;
			}
			i++;
		}
		return null;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000F538 File Offset: 0x0000D738
	public static InvBaseItem FindByID(int id32)
	{
		InvDatabase database = InvDatabase.GetDatabase(id32 >> 16);
		if (!(database != null))
		{
			return null;
		}
		return database.GetItem(id32 & 65535);
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000F568 File Offset: 0x0000D768
	public static InvBaseItem FindByName(string exact)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			int j = 0;
			int count = invDatabase.items.Count;
			while (j < count)
			{
				InvBaseItem invBaseItem = invDatabase.items[j];
				if (invBaseItem.name == exact)
				{
					return invBaseItem;
				}
				j++;
			}
			i++;
		}
		return null;
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000F5CC File Offset: 0x0000D7CC
	public static int FindItemID(InvBaseItem item)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			if (invDatabase.items.Contains(item))
			{
				return (invDatabase.databaseID << 16) | item.id16;
			}
			i++;
		}
		return -1;
	}

	// Token: 0x0400021D RID: 541
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static InvDatabase[] mList;

	// Token: 0x0400021E RID: 542
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool mIsDirty = true;

	// Token: 0x0400021F RID: 543
	public int databaseID;

	// Token: 0x04000220 RID: 544
	public List<InvBaseItem> items = new List<InvBaseItem>();

	// Token: 0x04000221 RID: 545
	public global::UnityEngine.Object iconAtlas;
}
