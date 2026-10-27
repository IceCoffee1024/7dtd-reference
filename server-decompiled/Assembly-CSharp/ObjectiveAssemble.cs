using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000995 RID: 2453
[Preserve]
public class ObjectiveAssemble : BaseObjective
{
	// Token: 0x06004724 RID: 18212 RVA: 0x001C1995 File Offset: 0x001BFB95
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
	}

	// Token: 0x06004725 RID: 18213 RVA: 0x001C19CD File Offset: 0x001BFBCD
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x06004726 RID: 18214 RVA: 0x001C19F6 File Offset: 0x001BFBF6
	public override void AddHooks()
	{
		QuestEventManager.Current.AssembleItem += this.Current_AssembleItem;
	}

	// Token: 0x06004727 RID: 18215 RVA: 0x001C1A0E File Offset: 0x001BFC0E
	public override void RemoveHooks()
	{
		QuestEventManager.Current.AssembleItem -= this.Current_AssembleItem;
	}

	// Token: 0x06004728 RID: 18216 RVA: 0x001C1A26 File Offset: 0x001BFC26
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_AssembleItem(ItemStack stack)
	{
		if (stack.itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004729 RID: 18217 RVA: 0x001C1A5C File Offset: 0x001BFC5C
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

	// Token: 0x0600472A RID: 18218 RVA: 0x001C1A9C File Offset: 0x001BFC9C
	public override BaseObjective Clone()
	{
		ObjectiveAssemble objectiveAssemble = new ObjectiveAssemble();
		this.CopyValues(objectiveAssemble);
		return objectiveAssemble;
	}

	// Token: 0x0600472B RID: 18219 RVA: 0x001C1AB7 File Offset: 0x001BFCB7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
	}

	// Token: 0x04003971 RID: 14705
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003972 RID: 14706
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003973 RID: 14707
	[PublicizedFrom(EAccessModifier.Private)]
	public bool assembled;
}
