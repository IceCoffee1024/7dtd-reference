using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009AC RID: 2476
[Preserve]
public class ObjectiveOpenWindow : BaseObjective
{
	// Token: 0x06004835 RID: 18485 RVA: 0x001C63F9 File Offset: 0x001C45F9
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveOpenWindow_keyword", false, null);
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x001C640D File Offset: 0x001C460D
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.ID);
		this.StatusText = "";
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x001C6438 File Offset: 0x001C4638
	public override void AddHooks()
	{
		QuestEventManager.Current.WindowChanged += this.Current_WindowOpened;
	}

	// Token: 0x06004838 RID: 18488 RVA: 0x001C6450 File Offset: 0x001C4650
	public override void RemoveHooks()
	{
		QuestEventManager.Current.WindowChanged -= this.Current_WindowOpened;
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x001C6468 File Offset: 0x001C4668
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_WindowOpened(string windowName)
	{
		if (windowName.EqualsCaseInsensitive(this.ID) && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x001C6494 File Offset: 0x001C4694
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x0600483B RID: 18491 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x0600483C RID: 18492 RVA: 0x001C64CC File Offset: 0x001C46CC
	public override BaseObjective Clone()
	{
		ObjectiveOpenWindow objectiveOpenWindow = new ObjectiveOpenWindow();
		this.CopyValues(objectiveOpenWindow);
		return objectiveOpenWindow;
	}

	// Token: 0x0600483D RID: 18493 RVA: 0x001C64E7 File Offset: 0x001C46E7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("window_name", this.ID);
	}
}
