using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200098B RID: 2443
[Preserve]
public class ObjectiveAssemble : BaseObjective
{
	// Token: 0x06004701 RID: 18177 RVA: 0x001C0911 File Offset: 0x001BEB11
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
	}

	// Token: 0x06004702 RID: 18178 RVA: 0x001C0949 File Offset: 0x001BEB49
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x06004703 RID: 18179 RVA: 0x001C0972 File Offset: 0x001BEB72
	public override void AddHooks()
	{
		QuestEventManager.Current.AssembleItem += this.Current_AssembleItem;
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x001C098A File Offset: 0x001BEB8A
	public override void RemoveHooks()
	{
		QuestEventManager.Current.AssembleItem -= this.Current_AssembleItem;
	}

	// Token: 0x06004705 RID: 18181 RVA: 0x001C09A2 File Offset: 0x001BEBA2
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_AssembleItem(ItemStack stack)
	{
		if (stack.itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004706 RID: 18182 RVA: 0x001C09D8 File Offset: 0x001BEBD8
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

	// Token: 0x06004707 RID: 18183 RVA: 0x001C0A18 File Offset: 0x001BEC18
	public override BaseObjective Clone()
	{
		ObjectiveAssemble objectiveAssemble = new ObjectiveAssemble();
		this.CopyValues(objectiveAssemble);
		return objectiveAssemble;
	}

	// Token: 0x06004708 RID: 18184 RVA: 0x001C0A33 File Offset: 0x001BEC33
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
	}

	// Token: 0x04003951 RID: 14673
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003952 RID: 14674
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003953 RID: 14675
	[PublicizedFrom(EAccessModifier.Private)]
	public bool assembled;
}
