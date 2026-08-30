using System;
using UnityEngine.Scripting;

// Token: 0x02000959 RID: 2393
[Preserve]
public class QuestActionShowMessageWindow : BaseQuestAction
{
	// Token: 0x06004614 RID: 17940 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004615 RID: 17941 RVA: 0x001BBB1D File Offset: 0x001B9D1D
	public override void PerformAction(Quest ownerQuest)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
	}

	// Token: 0x06004616 RID: 17942 RVA: 0x001BBB54 File Offset: 0x001B9D54
	public override BaseQuestAction Clone()
	{
		QuestActionShowMessageWindow questActionShowMessageWindow = new QuestActionShowMessageWindow();
		base.CopyValues(questActionShowMessageWindow);
		questActionShowMessageWindow.message = this.message;
		questActionShowMessageWindow.title = this.title;
		return questActionShowMessageWindow;
	}

	// Token: 0x06004617 RID: 17943 RVA: 0x001BBB87 File Offset: 0x001B9D87
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionShowMessageWindow.PropMessage, ref this.message);
		properties.ParseString(QuestActionShowMessageWindow.PropTitle, ref this.title);
	}

	// Token: 0x0400383F RID: 14399
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropMessage = "message";

	// Token: 0x04003840 RID: 14400
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTitle = "title";

	// Token: 0x04003841 RID: 14401
	[PublicizedFrom(EAccessModifier.Private)]
	public string message = "";

	// Token: 0x04003842 RID: 14402
	[PublicizedFrom(EAccessModifier.Private)]
	public string title = "";
}
