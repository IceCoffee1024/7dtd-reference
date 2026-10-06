using System;
using UnityEngine.Scripting;

// Token: 0x020009FF RID: 2559
[Preserve]
public class RewardShowMessageWindow : BaseReward
{
	// Token: 0x06004BE7 RID: 19431 RVA: 0x001D911D File Offset: 0x001D731D
	public RewardShowMessageWindow()
	{
		base.HiddenReward = true;
	}

	// Token: 0x06004BE8 RID: 19432 RVA: 0x001D9142 File Offset: 0x001D7342
	public override void SetupReward()
	{
		base.HiddenReward = true;
	}

	// Token: 0x06004BE9 RID: 19433 RVA: 0x001D914B File Offset: 0x001D734B
	public override void GiveReward(EntityPlayer player)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
	}

	// Token: 0x06004BEA RID: 19434 RVA: 0x001D9180 File Offset: 0x001D7380
	public override BaseReward Clone()
	{
		RewardShowMessageWindow rewardShowMessageWindow = new RewardShowMessageWindow();
		base.CopyValues(rewardShowMessageWindow);
		rewardShowMessageWindow.title = this.title;
		rewardShowMessageWindow.message = this.message;
		return rewardShowMessageWindow;
	}

	// Token: 0x06004BEB RID: 19435 RVA: 0x001D91B3 File Offset: 0x001D73B3
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(RewardShowMessageWindow.PropMessage, ref this.message);
		properties.ParseString(RewardShowMessageWindow.PropTitle, ref this.title);
	}

	// Token: 0x04003BDA RID: 15322
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropMessage = "message";

	// Token: 0x04003BDB RID: 15323
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTitle = "title";

	// Token: 0x04003BDC RID: 15324
	[PublicizedFrom(EAccessModifier.Private)]
	public string message = "";

	// Token: 0x04003BDD RID: 15325
	[PublicizedFrom(EAccessModifier.Private)]
	public string title = "";
}
