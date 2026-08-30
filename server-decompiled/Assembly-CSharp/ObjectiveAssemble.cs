using System;
using UnityEngine.Scripting;

// Token: 0x02000974 RID: 2420
[Preserve]
public class ObjectiveAssemble : BaseObjective
{
	// Token: 0x060046D8 RID: 18136 RVA: 0x001BDB95 File Offset: 0x001BBD95
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x001BDBCD File Offset: 0x001BBDCD
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x060046DA RID: 18138 RVA: 0x001BDBF6 File Offset: 0x001BBDF6
	public override void AddHooks()
	{
		QuestEventManager.Current.AssembleItem += this.Current_AssembleItem;
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x001BDC0E File Offset: 0x001BBE0E
	public override void RemoveHooks()
	{
		QuestEventManager.Current.AssembleItem -= this.Current_AssembleItem;
	}

	// Token: 0x060046DC RID: 18140 RVA: 0x001BDC26 File Offset: 0x001BBE26
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_AssembleItem(ItemStack stack)
	{
		if (stack.itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x060046DD RID: 18141 RVA: 0x001BDC5C File Offset: 0x001BBE5C
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

	// Token: 0x060046DE RID: 18142 RVA: 0x001BDC9C File Offset: 0x001BBE9C
	public override BaseObjective Clone()
	{
		ObjectiveAssemble objectiveAssemble = new ObjectiveAssemble();
		this.CopyValues(objectiveAssemble);
		return objectiveAssemble;
	}

	// Token: 0x040038C4 RID: 14532
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x040038C5 RID: 14533
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x040038C6 RID: 14534
	[PublicizedFrom(EAccessModifier.Private)]
	public bool assembled;
}
