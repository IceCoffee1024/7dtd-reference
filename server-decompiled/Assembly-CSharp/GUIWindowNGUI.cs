using System;

// Token: 0x02001208 RID: 4616
public class GUIWindowNGUI : GUIWindow
{
	// Token: 0x0600935A RID: 37722 RVA: 0x0037BCCB File Offset: 0x00379ECB
	public GUIWindowNGUI(EnumNGUIWindow _nguiEnum)
		: base(_nguiEnum.ToStringCached<EnumNGUIWindow>())
	{
		this.nguiEnum = _nguiEnum;
	}

	// Token: 0x0600935B RID: 37723 RVA: 0x0037BCE0 File Offset: 0x00379EE0
	public override void OnOpen()
	{
		this.playerUI.nguiWindowManager.Show(this.nguiEnum, true);
	}

	// Token: 0x0600935C RID: 37724 RVA: 0x0037BCF9 File Offset: 0x00379EF9
	public override void OnClose()
	{
		base.OnClose();
		this.playerUI.nguiWindowManager.Show(this.nguiEnum, false);
	}

	// Token: 0x04006E4E RID: 28238
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumNGUIWindow nguiEnum;
}
