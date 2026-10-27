using System;
using UnityEngine.Scripting;

// Token: 0x02000DD1 RID: 3537
[Preserve]
public class XUiC_OptionsUsername : XUiC_OptionsDialogBase
{
	// Token: 0x06006B91 RID: 27537 RVA: 0x002A4E4B File Offset: 0x002A304B
	public override void Init()
	{
		base.Init();
		XUiC_OptionsUsername.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006B92 RID: 27538 RVA: 0x002A4E63 File Offset: 0x002A3063
	[XuiBindEvent("OnSubmitHandler", "txtUsername")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtUsername_OnSubmitHandler(XUiController _sender, string _text)
	{
		base.Event_ApplyOnPress(_sender, -1);
	}

	// Token: 0x06006B93 RID: 27539 RVA: 0x002A4E6D File Offset: 0x002A306D
	[XuiBindEvent("OnChangeHandler", "txtUsername")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtUsername_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (_changeFromCode)
		{
			return;
		}
		base.SetChanged();
	}

	// Token: 0x06006B94 RID: 27540 RVA: 0x002A4E79 File Offset: 0x002A3079
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doSaveChangesInternal()
	{
		base.doSaveChangesInternal();
		GamePrefs.Set(EnumGamePrefs.PlayerName, this.txtUsername.Text);
	}

	// Token: 0x06006B95 RID: 27541 RVA: 0x002A4E93 File Offset: 0x002A3093
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtUsername.Text = GamePrefs.GetString(EnumGamePrefs.PlayerName);
	}

	// Token: 0x17000BE1 RID: 3041
	// (get) Token: 0x06006B96 RID: 27542 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x04005262 RID: 21090
	public static string ID = "";

	// Token: 0x04005263 RID: 21091
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtUsername;
}
