using System;
using UnityEngine.Scripting;

// Token: 0x0200098A RID: 2442
[Preserve]
public class ObjectiveBlockUpgrade : BaseObjective
{
	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x0600478E RID: 18318 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600478F RID: 18319 RVA: 0x001C0D60 File Offset: 0x001BEF60
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockUpgrade_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004790 RID: 18320 RVA: 0x001C0DC4 File Offset: 0x001BEFC4
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004791 RID: 18321 RVA: 0x001C0E03 File Offset: 0x001BF003
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
	}

	// Token: 0x06004792 RID: 18322 RVA: 0x001C0E31 File Offset: 0x001BF031
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
	}

	// Token: 0x06004793 RID: 18323 RVA: 0x001C0E4C File Offset: 0x001BF04C
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

	// Token: 0x06004794 RID: 18324 RVA: 0x001C0F24 File Offset: 0x001BF124
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

	// Token: 0x06004795 RID: 18325 RVA: 0x001C0F84 File Offset: 0x001BF184
	public override BaseObjective Clone()
	{
		ObjectiveBlockUpgrade objectiveBlockUpgrade = new ObjectiveBlockUpgrade();
		this.CopyValues(objectiveBlockUpgrade);
		return objectiveBlockUpgrade;
	}

	// Token: 0x04003968 RID: 14696
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003969 RID: 14697
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
