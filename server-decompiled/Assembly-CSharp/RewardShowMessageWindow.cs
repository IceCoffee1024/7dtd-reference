using System;
using UnityEngine.Scripting;

// Token: 0x020009F5 RID: 2549
[Preserve]
public class RewardShowMessageWindow : BaseReward
{
	// Token: 0x06004C18 RID: 19480 RVA: 0x001D7E6D File Offset: 0x001D606D
	public RewardShowMessageWindow()
	{
		base.HiddenReward = true;
	}

	// Token: 0x06004C19 RID: 19481 RVA: 0x001D7E92 File Offset: 0x001D6092
	public override void SetupReward()
	{
		base.HiddenReward = true;
	}

	// Token: 0x06004C1A RID: 19482 RVA: 0x001D7E9B File Offset: 0x001D609B
	public override void GiveReward(EntityPlayer player)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
	}

	// Token: 0x06004C1B RID: 19483 RVA: 0x001D7ED0 File Offset: 0x001D60D0
	public override BaseReward Clone()
	{
		RewardShowMessageWindow rewardShowMessageWindow = new RewardShowMessageWindow();
		base.CopyValues(rewardShowMessageWindow);
		rewardShowMessageWindow.title = this.title;
		rewardShowMessageWindow.message = this.message;
		return rewardShowMessageWindow;
	}

	// Token: 0x06004C1C RID: 19484 RVA: 0x001D7F03 File Offset: 0x001D6103
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(RewardShowMessageWindow.PropMessage, ref this.message);
		properties.ParseString(RewardShowMessageWindow.PropTitle, ref this.title);
	}

	// Token: 0x04003BCD RID: 15309
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropMessage = "message";

	// Token: 0x04003BCE RID: 15310
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTitle = "title";

	// Token: 0x04003BCF RID: 15311
	[PublicizedFrom(EAccessModifier.Private)]
	public string message = "";

	// Token: 0x04003BD0 RID: 15312
	[PublicizedFrom(EAccessModifier.Private)]
	public string title = "";
}
