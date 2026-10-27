using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200099A RID: 2458
[Preserve]
public class ObjectiveBlockUpgrade : BaseObjective
{
	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x06004759 RID: 18265 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600475A RID: 18266 RVA: 0x001C2418 File Offset: 0x001C0618
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockUpgrade_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x001C247C File Offset: 0x001C067C
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x001C24BB File Offset: 0x001C06BB
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
	}

	// Token: 0x0600475D RID: 18269 RVA: 0x001C24E9 File Offset: 0x001C06E9
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
	}

	// Token: 0x0600475E RID: 18270 RVA: 0x001C2504 File Offset: 0x001C0704
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

	// Token: 0x0600475F RID: 18271 RVA: 0x001C25DC File Offset: 0x001C07DC
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

	// Token: 0x06004760 RID: 18272 RVA: 0x001C263C File Offset: 0x001C083C
	public override BaseObjective Clone()
	{
		ObjectiveBlockUpgrade objectiveBlockUpgrade = new ObjectiveBlockUpgrade();
		this.CopyValues(objectiveBlockUpgrade);
		return objectiveBlockUpgrade;
	}

	// Token: 0x06004761 RID: 18273 RVA: 0x001C2657 File Offset: 0x001C0857
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("block_id", string.IsNullOrEmpty(this.ID) ? "Any Block" : this.ID);
		parameters.Add("count", this.neededCount);
	}

	// Token: 0x04003984 RID: 14724
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003985 RID: 14725
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
