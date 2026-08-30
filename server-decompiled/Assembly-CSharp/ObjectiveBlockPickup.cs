using System;
using UnityEngine.Scripting;

// Token: 0x02000988 RID: 2440
[Preserve]
public class ObjectiveBlockPickup : BaseObjective
{
	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x0600477C RID: 18300 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600477D RID: 18301 RVA: 0x001C0928 File Offset: 0x001BEB28
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockPickup_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600477E RID: 18302 RVA: 0x001C098C File Offset: 0x001BEB8C
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x0600477F RID: 18303 RVA: 0x001C09CB File Offset: 0x001BEBCB
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockPickup += this.Current_BlockPickup;
	}

	// Token: 0x06004780 RID: 18304 RVA: 0x001C09E3 File Offset: 0x001BEBE3
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockPickup -= this.Current_BlockPickup;
	}

	// Token: 0x06004781 RID: 18305 RVA: 0x001C09FC File Offset: 0x001BEBFC
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

	// Token: 0x06004782 RID: 18306 RVA: 0x001C0AA8 File Offset: 0x001BECA8
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

	// Token: 0x06004783 RID: 18307 RVA: 0x001C0B08 File Offset: 0x001BED08
	public override BaseObjective Clone()
	{
		ObjectiveBlockPickup objectiveBlockPickup = new ObjectiveBlockPickup();
		this.CopyValues(objectiveBlockPickup);
		return objectiveBlockPickup;
	}

	// Token: 0x04003964 RID: 14692
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003965 RID: 14693
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
