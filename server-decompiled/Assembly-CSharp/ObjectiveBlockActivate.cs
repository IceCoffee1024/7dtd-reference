using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200098D RID: 2445
[Preserve]
public class ObjectiveBlockActivate : BaseObjective
{
	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x06004718 RID: 18200 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x06004719 RID: 18201 RVA: 0x001C0DC4 File Offset: 0x001BEFC4
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockActivate_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
	}

	// Token: 0x0600471A RID: 18202 RVA: 0x001C0E17 File Offset: 0x001BF017
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.localizedName);
	}

	// Token: 0x0600471B RID: 18203 RVA: 0x000027FC File Offset: 0x000009FC
	public override void AddHooks()
	{
	}

	// Token: 0x0600471C RID: 18204 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveHooks()
	{
	}

	// Token: 0x0600471D RID: 18205 RVA: 0x001C0E30 File Offset: 0x001BF030
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockActivate(string blockName)
	{
		if (base.Complete)
		{
			return;
		}
		if ((this.ID == null || this.ID == "" || this.ID.EqualsCaseInsensitive(blockName)) && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x0600471E RID: 18206 RVA: 0x001C0E88 File Offset: 0x001BF088
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x0600471F RID: 18207 RVA: 0x001C0EC0 File Offset: 0x001BF0C0
	public override BaseObjective Clone()
	{
		ObjectiveBlockActivate objectiveBlockActivate = new ObjectiveBlockActivate();
		this.CopyValues(objectiveBlockActivate);
		return objectiveBlockActivate;
	}

	// Token: 0x06004720 RID: 18208 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
	}

	// Token: 0x0400395F RID: 14687
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";
}
