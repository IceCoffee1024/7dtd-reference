using System;

// Token: 0x0200124E RID: 4686
public class GUIWindowNGUI : GUIWindow
{
	// Token: 0x06009543 RID: 38211 RVA: 0x00387BEB File Offset: 0x00385DEB
	public GUIWindowNGUI(EnumNGUIWindow _nguiEnum)
		: base(_nguiEnum.ToStringCached<EnumNGUIWindow>())
	{
		this.nguiEnum = _nguiEnum;
	}

	// Token: 0x06009544 RID: 38212 RVA: 0x00387C00 File Offset: 0x00385E00
	public override void OnOpen()
	{
		this.playerUI.nguiWindowManager.Show(this.nguiEnum, true);
	}

	// Token: 0x06009545 RID: 38213 RVA: 0x00387C19 File Offset: 0x00385E19
	public override void OnClose()
	{
		base.OnClose();
		this.playerUI.nguiWindowManager.Show(this.nguiEnum, false);
	}

	// Token: 0x04006FD2 RID: 28626
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumNGUIWindow nguiEnum;
}
