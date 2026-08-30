using System;
using UnityEngine.Scripting;

// Token: 0x02000989 RID: 2441
[Preserve]
public class ObjectiveBlockPlace : BaseObjective
{
	// Token: 0x17000790 RID: 1936
	// (get) Token: 0x06004785 RID: 18309 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004786 RID: 18310 RVA: 0x001C0B38 File Offset: 0x001BED38
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockPlace_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004787 RID: 18311 RVA: 0x001C0B9C File Offset: 0x001BED9C
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004788 RID: 18312 RVA: 0x001C0BDB File Offset: 0x001BEDDB
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
		QuestEventManager.Current.BlockPlace += this.Current_BlockPlace;
	}

	// Token: 0x06004789 RID: 18313 RVA: 0x001C0C09 File Offset: 0x001BEE09
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
	}

	// Token: 0x0600478A RID: 18314 RVA: 0x001C0C24 File Offset: 0x001BEE24
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

	// Token: 0x0600478B RID: 18315 RVA: 0x001C0CD0 File Offset: 0x001BEED0
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

	// Token: 0x0600478C RID: 18316 RVA: 0x001C0D30 File Offset: 0x001BEF30
	public override BaseObjective Clone()
	{
		ObjectiveBlockPlace objectiveBlockPlace = new ObjectiveBlockPlace();
		this.CopyValues(objectiveBlockPlace);
		return objectiveBlockPlace;
	}

	// Token: 0x04003966 RID: 14694
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003967 RID: 14695
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
