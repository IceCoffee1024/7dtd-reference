using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001096 RID: 4246
[Preserve]
public class XUiC_UnlockByList : XUiController
{
	// Token: 0x17000FAA RID: 4010
	// (get) Token: 0x06008745 RID: 34629 RVA: 0x0033BF00 File Offset: 0x0033A100
	// (set) Token: 0x06008746 RID: 34630 RVA: 0x0033BF08 File Offset: 0x0033A108
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

	// Token: 0x06008747 RID: 34631 RVA: 0x0033BF18 File Offset: 0x0033A118
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

	// Token: 0x06008748 RID: 34632 RVA: 0x0033BF58 File Offset: 0x0033A158
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

	// Token: 0x040064B5 RID: 25781
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x040064B6 RID: 25782
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> unlockByEntries = new List<XUiController>();
}
