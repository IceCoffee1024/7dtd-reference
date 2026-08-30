using System;
using UnityEngine.Scripting;

// Token: 0x02000978 RID: 2424
[Preserve]
public class ObjectiveBlockPlace : BaseObjective
{
	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x060046FF RID: 18175 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004700 RID: 18176 RVA: 0x001BE344 File Offset: 0x001BC544
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockPlace_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004701 RID: 18177 RVA: 0x001BE3A8 File Offset: 0x001BC5A8
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004702 RID: 18178 RVA: 0x001BE3E7 File Offset: 0x001BC5E7
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
		QuestEventManager.Current.BlockPlace += this.Current_BlockPlace;
	}

	// Token: 0x06004703 RID: 18179 RVA: 0x001BE415 File Offset: 0x001BC615
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x001BE430 File Offset: 0x001BC630
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

	// Token: 0x06004705 RID: 18181 RVA: 0x001BE4DC File Offset: 0x001BC6DC
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

	// Token: 0x06004706 RID: 18182 RVA: 0x001BE53C File Offset: 0x001BC73C
	public override BaseObjective Clone()
	{
		ObjectiveBlockPlace objectiveBlockPlace = new ObjectiveBlockPlace();
		this.CopyValues(objectiveBlockPlace);
		return objectiveBlockPlace;
	}

	// Token: 0x040038D5 RID: 14549
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x040038D6 RID: 14550
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
