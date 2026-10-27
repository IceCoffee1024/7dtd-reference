using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020010D0 RID: 4304
[Preserve]
public class XUiC_UnlockByList : XUiController
{
	// Token: 0x17001023 RID: 4131
	// (get) Token: 0x060088D9 RID: 35033 RVA: 0x003465C4 File Offset: 0x003447C4
	// (set) Token: 0x060088DA RID: 35034 RVA: 0x003465CC File Offset: 0x003447CC
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

	// Token: 0x060088DB RID: 35035 RVA: 0x003465DC File Offset: 0x003447DC
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

	// Token: 0x060088DC RID: 35036 RVA: 0x0034661C File Offset: 0x0034481C
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

	// Token: 0x040065FD RID: 26109
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x040065FE RID: 26110
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> unlockByEntries = new List<XUiController>();
}
