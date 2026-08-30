using System;
using UnityEngine.Scripting;

// Token: 0x02000979 RID: 2425
[Preserve]
public class ObjectiveBlockUpgrade : BaseObjective
{
	// Token: 0x1700077F RID: 1919
	// (get) Token: 0x06004708 RID: 18184 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004709 RID: 18185 RVA: 0x001BE56C File Offset: 0x001BC76C
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockUpgrade_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600470A RID: 18186 RVA: 0x001BE5D0 File Offset: 0x001BC7D0
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x0600470B RID: 18187 RVA: 0x001BE60F File Offset: 0x001BC80F
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
	}

	// Token: 0x0600470C RID: 18188 RVA: 0x001BE63D File Offset: 0x001BC83D
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
	}

	// Token: 0x0600470D RID: 18189 RVA: 0x001BE658 File Offset: 0x001BC858
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

	// Token: 0x0600470E RID: 18190 RVA: 0x001BE730 File Offset: 0x001BC930
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

	// Token: 0x0600470F RID: 18191 RVA: 0x001BE790 File Offset: 0x001BC990
	public override BaseObjective Clone()
	{
		ObjectiveBlockUpgrade objectiveBlockUpgrade = new ObjectiveBlockUpgrade();
		this.CopyValues(objectiveBlockUpgrade);
		return objectiveBlockUpgrade;
	}

	// Token: 0x040038D7 RID: 14551
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x040038D8 RID: 14552
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
