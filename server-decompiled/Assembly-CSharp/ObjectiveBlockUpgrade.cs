using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000990 RID: 2448
[Preserve]
public class ObjectiveBlockUpgrade : BaseObjective
{
	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x06004736 RID: 18230 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004737 RID: 18231 RVA: 0x001C1394 File Offset: 0x001BF594
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockUpgrade_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004738 RID: 18232 RVA: 0x001C13F8 File Offset: 0x001BF5F8
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004739 RID: 18233 RVA: 0x001C1437 File Offset: 0x001BF637
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
	}

	// Token: 0x0600473A RID: 18234 RVA: 0x001C1465 File Offset: 0x001BF665
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
	}

	// Token: 0x0600473B RID: 18235 RVA: 0x001C1480 File Offset: 0x001BF680
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockUpgrade(string blockname, Vector3i blockPos)
	{
		if (base.Complete)
		{
			return;
		}
		bool flag = false;
		if (this.ID == null || this.ID == "")
		{
			flag = true;
		}
		else
		{
			if (this.ID.EqualsCaseInsensitive(blockname))
			{
				flag = true;
			}
			if (blockname.Contains(":") && this.ID.EqualsCaseInsensitive(blockname.Substring(0, blockname.IndexOf(':'))))
			{
				flag = true;
			}
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

	// Token: 0x0600473C RID: 18236 RVA: 0x001C1558 File Offset: 0x001BF758
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

	// Token: 0x0600473D RID: 18237 RVA: 0x001C15B8 File Offset: 0x001BF7B8
	public override BaseObjective Clone()
	{
		ObjectiveBlockUpgrade objectiveBlockUpgrade = new ObjectiveBlockUpgrade();
		this.CopyValues(objectiveBlockUpgrade);
		return objectiveBlockUpgrade;
	}

	// Token: 0x0600473E RID: 18238 RVA: 0x001C15D3 File Offset: 0x001BF7D3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("block_id", string.IsNullOrEmpty(this.ID) ? "Any Block" : this.ID);
		parameters.Add("count", this.neededCount);
	}

	// Token: 0x04003964 RID: 14692
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003965 RID: 14693
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
