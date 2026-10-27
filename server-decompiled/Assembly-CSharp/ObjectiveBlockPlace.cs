using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000999 RID: 2457
[Preserve]
public class ObjectiveBlockPlace : BaseObjective
{
	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x0600474F RID: 18255 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004750 RID: 18256 RVA: 0x001C21C4 File Offset: 0x001C03C4
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockPlace_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004751 RID: 18257 RVA: 0x001C2228 File Offset: 0x001C0428
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004752 RID: 18258 RVA: 0x001C2267 File Offset: 0x001C0467
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
		QuestEventManager.Current.BlockPlace += this.Current_BlockPlace;
	}

	// Token: 0x06004753 RID: 18259 RVA: 0x001C2295 File Offset: 0x001C0495
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
	}

	// Token: 0x06004754 RID: 18260 RVA: 0x001C22B0 File Offset: 0x001C04B0
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

	// Token: 0x06004755 RID: 18261 RVA: 0x001C235C File Offset: 0x001C055C
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

	// Token: 0x06004756 RID: 18262 RVA: 0x001C23BC File Offset: 0x001C05BC
	public override BaseObjective Clone()
	{
		ObjectiveBlockPlace objectiveBlockPlace = new ObjectiveBlockPlace();
		this.CopyValues(objectiveBlockPlace);
		return objectiveBlockPlace;
	}

	// Token: 0x06004757 RID: 18263 RVA: 0x001C23D7 File Offset: 0x001C05D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("block_id", this.ID);
		parameters.Add("count", this.neededCount);
	}

	// Token: 0x04003982 RID: 14722
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003983 RID: 14723
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
