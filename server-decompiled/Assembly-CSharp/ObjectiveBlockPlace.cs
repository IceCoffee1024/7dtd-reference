using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200098F RID: 2447
[Preserve]
public class ObjectiveBlockPlace : BaseObjective
{
	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x0600472C RID: 18220 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600472D RID: 18221 RVA: 0x001C1140 File Offset: 0x001BF340
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockPlace_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600472E RID: 18222 RVA: 0x001C11A4 File Offset: 0x001BF3A4
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x0600472F RID: 18223 RVA: 0x001C11E3 File Offset: 0x001BF3E3
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
		QuestEventManager.Current.BlockPlace += this.Current_BlockPlace;
	}

	// Token: 0x06004730 RID: 18224 RVA: 0x001C1211 File Offset: 0x001BF411
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
	}

	// Token: 0x06004731 RID: 18225 RVA: 0x001C122C File Offset: 0x001BF42C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockPlace(string blockname, Vector3i blockPos)
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

	// Token: 0x06004732 RID: 18226 RVA: 0x001C12D8 File Offset: 0x001BF4D8
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

	// Token: 0x06004733 RID: 18227 RVA: 0x001C1338 File Offset: 0x001BF538
	public override BaseObjective Clone()
	{
		ObjectiveBlockPlace objectiveBlockPlace = new ObjectiveBlockPlace();
		this.CopyValues(objectiveBlockPlace);
		return objectiveBlockPlace;
	}

	// Token: 0x06004734 RID: 18228 RVA: 0x001C1353 File Offset: 0x001BF553
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("block_id", this.ID);
		parameters.Add("count", this.neededCount);
	}

	// Token: 0x04003962 RID: 14690
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003963 RID: 14691
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
