using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000998 RID: 2456
[Preserve]
public class ObjectiveBlockPickup : BaseObjective
{
	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x06004745 RID: 18245 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004746 RID: 18246 RVA: 0x001C1F74 File Offset: 0x001C0174
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockPickup_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		this.neededCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004747 RID: 18247 RVA: 0x001C1FD8 File Offset: 0x001C01D8
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004748 RID: 18248 RVA: 0x001C2017 File Offset: 0x001C0217
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockPickup += this.Current_BlockPickup;
	}

	// Token: 0x06004749 RID: 18249 RVA: 0x001C202F File Offset: 0x001C022F
	public override void RemoveHooks()
	{
		QuestEventManager.Current.BlockPickup -= this.Current_BlockPickup;
	}

	// Token: 0x0600474A RID: 18250 RVA: 0x001C2048 File Offset: 0x001C0248
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

	// Token: 0x0600474B RID: 18251 RVA: 0x001C20F4 File Offset: 0x001C02F4
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

	// Token: 0x0600474C RID: 18252 RVA: 0x001C2154 File Offset: 0x001C0354
	public override BaseObjective Clone()
	{
		ObjectiveBlockPickup objectiveBlockPickup = new ObjectiveBlockPickup();
		this.CopyValues(objectiveBlockPickup);
		return objectiveBlockPickup;
	}

	// Token: 0x0600474D RID: 18253 RVA: 0x001C216F File Offset: 0x001C036F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("block_id", string.IsNullOrEmpty(this.ID) ? "Any Block" : this.ID);
		parameters.Add("count", this.neededCount);
	}

	// Token: 0x04003980 RID: 14720
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003981 RID: 14721
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount;
}
