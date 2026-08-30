using System;
using UnityEngine.Scripting;

// Token: 0x02000977 RID: 2423
[Preserve]
public class ObjectiveBlockPickup : BaseObjective
{
	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x060046F6 RID: 18166 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060046F7 RID: 18167 RVA: 0x001BE134 File Offset: 0x001BC334
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockPickup_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060046F8 RID: 18168 RVA: 0x001BE198 File Offset: 0x001BC398
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x060046F9 RID: 18169 RVA: 0x001BE1D7 File Offset: 0x001BC3D7
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockPickup += this.Current_BlockPickup;
	}

	// Token: 0x060046FA RID: 18170 RVA: 0x001BE1EF File Offset: 0x001BC3EF
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockPickup -= this.Current_BlockPickup;
	}

	// Token: 0x060046FB RID: 18171 RVA: 0x001BE208 File Offset: 0x001BC408
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

	// Token: 0x060046FC RID: 18172 RVA: 0x001BE2B4 File Offset: 0x001BC4B4
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

	// Token: 0x060046FD RID: 18173 RVA: 0x001BE314 File Offset: 0x001BC514
	public override BaseObjective Clone()
	{
		ObjectiveBlockPickup objectiveBlockPickup = new ObjectiveBlockPickup();
		this.CopyValues(objectiveBlockPickup);
		return objectiveBlockPickup;
	}

	// Token: 0x040038D3 RID: 14547
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x040038D4 RID: 14548
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
