using System;
using UnityEngine.Scripting;

// Token: 0x02000985 RID: 2437
[Preserve]
public class ObjectiveAssemble : BaseObjective
{
	// Token: 0x0600475E RID: 18270 RVA: 0x001C0389 File Offset: 0x001BE589
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
	}

	// Token: 0x0600475F RID: 18271 RVA: 0x001C03C1 File Offset: 0x001BE5C1
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x06004760 RID: 18272 RVA: 0x001C03EA File Offset: 0x001BE5EA
	public override void AddHooks()
	{
		QuestEventManager.Current.AssembleItem += this.Current_AssembleItem;
	}

	// Token: 0x06004761 RID: 18273 RVA: 0x001C0402 File Offset: 0x001BE602
	public override void RemoveHooks()
	{
		QuestEventManager.Current.AssembleItem -= this.Current_AssembleItem;
	}

	// Token: 0x06004762 RID: 18274 RVA: 0x001C041A File Offset: 0x001BE61A
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_AssembleItem(ItemStack stack)
	{
		if (stack.itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004763 RID: 18275 RVA: 0x001C0450 File Offset: 0x001BE650
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x06004764 RID: 18276 RVA: 0x001C0490 File Offset: 0x001BE690
	public override BaseObjective Clone()
	{
		ObjectiveAssemble objectiveAssemble = new ObjectiveAssemble();
		this.CopyValues(objectiveAssemble);
		return objectiveAssemble;
	}

	// Token: 0x04003955 RID: 14677
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003956 RID: 14678
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003957 RID: 14679
	[PublicizedFrom(EAccessModifier.Private)]
	public bool assembled;
}
