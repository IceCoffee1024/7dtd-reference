using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009A2 RID: 2466
[Preserve]
public class ObjectiveOpenWindow : BaseObjective
{
	// Token: 0x06004812 RID: 18450 RVA: 0x001C534D File Offset: 0x001C354D
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveOpenWindow_keyword", false, null);
	}

	// Token: 0x06004813 RID: 18451 RVA: 0x001C5361 File Offset: 0x001C3561
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.ID);
		this.StatusText = "";
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x001C538C File Offset: 0x001C358C
	public override void AddHooks()
	{
		QuestEventManager.Current.WindowChanged += this.Current_WindowOpened;
	}

	// Token: 0x06004815 RID: 18453 RVA: 0x001C53A4 File Offset: 0x001C35A4
	public override void RemoveHooks()
	{
		QuestEventManager.Current.WindowChanged -= this.Current_WindowOpened;
	}

	// Token: 0x06004816 RID: 18454 RVA: 0x001C53BC File Offset: 0x001C35BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_WindowOpened(string windowName)
	{
		if (windowName.EqualsCaseInsensitive(this.ID) && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004817 RID: 18455 RVA: 0x001C53E8 File Offset: 0x001C35E8
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x06004818 RID: 18456 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x06004819 RID: 18457 RVA: 0x001C5420 File Offset: 0x001C3620
	public override BaseObjective Clone()
	{
		ObjectiveOpenWindow objectiveOpenWindow = new ObjectiveOpenWindow();
		this.CopyValues(objectiveOpenWindow);
		return objectiveOpenWindow;
	}

	// Token: 0x0600481A RID: 18458 RVA: 0x001C543B File Offset: 0x001C363B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("window_name", this.ID);
	}
}
