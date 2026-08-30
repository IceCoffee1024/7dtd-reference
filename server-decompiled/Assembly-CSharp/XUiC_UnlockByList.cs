using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020010AF RID: 4271
[Preserve]
public class XUiC_UnlockByList : XUiController
{
	// Token: 0x17000FE3 RID: 4067
	// (get) Token: 0x06008837 RID: 34871 RVA: 0x00340728 File Offset: 0x0033E928
	// (set) Token: 0x06008838 RID: 34872 RVA: 0x00340730 File Offset: 0x0033E930
	public Recipe Recipe
	{
		get
		{
			return this.recipe;
		}
		set
		{
			this.recipe = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06008839 RID: 34873 RVA: 0x00340740 File Offset: 0x0033E940
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_UnlockByEntry>(null);
		XUiController[] array = childrenByType;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				this.unlockByEntries.Add(array[i]);
			}
		}
	}

	// Token: 0x0600883A RID: 34874 RVA: 0x00340780 File Offset: 0x0033E980
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			if (this.recipe != null)
			{
				ItemClass forId = ItemClass.GetForId(this.recipe.itemValueType);
				RecipeUnlockData[] array;
				if (forId.IsBlock())
				{
					array = forId.GetBlock().UnlockedBy;
				}
				else
				{
					array = forId.UnlockedBy;
				}
				int num = 0;
				int count = this.unlockByEntries.Count;
				Progression progression = this.xui.playerUI.entityPlayer.Progression;
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						XUiC_UnlockByEntry xuiC_UnlockByEntry = this.unlockByEntries[i] as XUiC_UnlockByEntry;
						if (xuiC_UnlockByEntry != null)
						{
							xuiC_UnlockByEntry.UnlockData = array[i];
							xuiC_UnlockByEntry.Recipe = this.recipe;
							num++;
						}
					}
				}
				for (int j = num; j < count; j++)
				{
					XUiC_UnlockByEntry xuiC_UnlockByEntry2 = this.unlockByEntries[j] as XUiC_UnlockByEntry;
					if (xuiC_UnlockByEntry2 != null)
					{
						xuiC_UnlockByEntry2.UnlockData = null;
					}
				}
			}
			else
			{
				int count2 = this.unlockByEntries.Count;
				for (int k = 0; k < count2; k++)
				{
					XUiC_UnlockByEntry xuiC_UnlockByEntry3 = this.unlockByEntries[k] as XUiC_UnlockByEntry;
					if (xuiC_UnlockByEntry3 != null)
					{
						xuiC_UnlockByEntry3.UnlockData = null;
					}
				}
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x0400657C RID: 25980
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x0400657D RID: 25981
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> unlockByEntries = new List<XUiController>();
}
