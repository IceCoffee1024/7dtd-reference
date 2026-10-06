using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200098E RID: 2446
[Preserve]
public class ObjectiveBlockPickup : BaseObjective
{
	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x06004722 RID: 18210 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004723 RID: 18211 RVA: 0x001C0EF0 File Offset: 0x001BF0F0
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockPickup_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004724 RID: 18212 RVA: 0x001C0F54 File Offset: 0x001BF154
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004725 RID: 18213 RVA: 0x001C0F93 File Offset: 0x001BF193
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockPickup += this.Current_BlockPickup;
	}

	// Token: 0x06004726 RID: 18214 RVA: 0x001C0FAB File Offset: 0x001BF1AB
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockPickup -= this.Current_BlockPickup;
	}

	// Token: 0x06004727 RID: 18215 RVA: 0x001C0FC4 File Offset: 0x001BF1C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockPickup(string blockname, Vector3i blockPos)
	{
		if (base.Complete)
		{
			return;
		}
		bool flag = false;
		if (this.ID == null || this.ID == "" || this.ID.EqualsCaseInsensitive(blockname))
		{
			flag = true;
		}
		if (!flag && this.ID != null && this.ID != "")
		{
			Block blockByName = Block.GetBlockByName(this.ID, true);
			if (blockByName != null && blockByName.SelectAlternates && blockByName.ContainsAlternateBlock(blockname))
			{
				flag = true;
			}
		}
		if (flag && base.OwnerQuest.CheckRequirements())
		{
			byte currentValue = base.CurrentValue;
			base.CurrentValue = currentValue + 1;
			this.Refresh();
		}
	}

	// Token: 0x06004728 RID: 18216 RVA: 0x001C1070 File Offset: 0x001BF270
	public override void Refresh()
	{
		if ((int)base.CurrentValue > this.neededCount)
		{
			base.CurrentValue = (byte)this.neededCount;
		}
		if (base.Complete)
		{
			return;
		}
		base.Complete = (int)base.CurrentValue >= this.neededCount;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x06004729 RID: 18217 RVA: 0x001C10D0 File Offset: 0x001BF2D0
	public override BaseObjective Clone()
	{
		ObjectiveBlockPickup objectiveBlockPickup = new ObjectiveBlockPickup();
		this.CopyValues(objectiveBlockPickup);
		return objectiveBlockPickup;
	}

	// Token: 0x0600472A RID: 18218 RVA: 0x001C10EB File Offset: 0x001BF2EB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("block_id", string.IsNullOrEmpty(this.ID) ? "Any Block" : this.ID);
		parameters.Add("count", this.neededCount);
	}

	// Token: 0x04003960 RID: 14688
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003961 RID: 14689
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
