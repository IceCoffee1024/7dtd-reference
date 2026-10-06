using System;
using UnityEngine.Scripting;

// Token: 0x02000970 RID: 2416
[Preserve]
public class QuestActionShowMessageWindow : BaseQuestAction
{
	// Token: 0x0600463B RID: 17979 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x0600463C RID: 17980 RVA: 0x001BE819 File Offset: 0x001BCA19
	public override void PerformAction(Quest ownerQuest)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
	}

	// Token: 0x0600463D RID: 17981 RVA: 0x001BE850 File Offset: 0x001BCA50
	public override BaseQuestAction Clone()
	{
		QuestActionShowMessageWindow questActionShowMessageWindow = new QuestActionShowMessageWindow();
		base.CopyValues(questActionShowMessageWindow);
		questActionShowMessageWindow.message = this.message;
		questActionShowMessageWindow.title = this.title;
		return questActionShowMessageWindow;
	}

	// Token: 0x0600463E RID: 17982 RVA: 0x001BE883 File Offset: 0x001BCA83
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionShowMessageWindow.PropMessage, ref this.message);
		properties.ParseString(QuestActionShowMessageWindow.PropTitle, ref this.title);
	}

	// Token: 0x040038CC RID: 14540
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropMessage = "message";

	// Token: 0x040038CD RID: 14541
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTitle = "title";

	// Token: 0x040038CE RID: 14542
	[PublicizedFrom(EAccessModifier.Private)]
	public string message = "";

	// Token: 0x040038CF RID: 14543
	[PublicizedFrom(EAccessModifier.Private)]
	public string title = "";
}
