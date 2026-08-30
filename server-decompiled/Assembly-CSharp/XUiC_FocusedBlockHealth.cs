using System;
using UnityEngine.Scripting;

// Token: 0x02000F1E RID: 3870
[Preserve]
public class XUiC_FocusedBlockHealth : XUiController
{
	// Token: 0x17000E30 RID: 3632
	// (get) Token: 0x0600798C RID: 31116 RVA: 0x002DD806 File Offset: 0x002DBA06
	// (set) Token: 0x0600798D RID: 31117 RVA: 0x002DD80E File Offset: 0x002DBA0E
	public string Text
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.text;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value != this.text)
			{
				this.text = value ?? "";
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000E31 RID: 3633
	// (get) Token: 0x0600798E RID: 31118 RVA: 0x002DD835 File Offset: 0x002DBA35
	// (set) Token: 0x0600798F RID: 31119 RVA: 0x002DD83D File Offset: 0x002DBA3D
	public float Fill
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.fill;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value != this.fill)
			{
				this.fill = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x06007990 RID: 31120 RVA: 0x002DD856 File Offset: 0x002DBA56
	public override void Init()
	{
		base.Init();
		XUiC_FocusedBlockHealth.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007991 RID: 31121 RVA: 0x0008A091 File Offset: 0x00088291
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007992 RID: 31122 RVA: 0x002DD86E File Offset: 0x002DBA6E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "text")
		{
			_value = this.text;
			return true;
		}
		if (!(_bindingName == "fill"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.fill.ToCultureInvariantString();
		return true;
	}

	// Token: 0x06007993 RID: 31123 RVA: 0x002DD8B0 File Offset: 0x002DBAB0
	public static void SetData(LocalPlayerUI _playerUi, string _text, float _fill)
	{
		if (_playerUi != null && _playerUi.xui != null)
		{
			XUiController xuiController = _playerUi.xui.FindWindowGroupByName(XUiC_FocusedBlockHealth.ID);
			XUiC_FocusedBlockHealth xuiC_FocusedBlockHealth = ((xuiController != null) ? xuiController.GetChildByType<XUiC_FocusedBlockHealth>() : null);
			if (xuiC_FocusedBlockHealth == null)
			{
				return;
			}
			xuiC_FocusedBlockHealth.Text = _text;
			xuiC_FocusedBlockHealth.Fill = _fill;
			if (_text == null)
			{
				_playerUi.windowManager.Close(XUiC_FocusedBlockHealth.ID);
				return;
			}
			_playerUi.windowManager.Open(XUiC_FocusedBlockHealth.ID, false);
		}
	}

	// Token: 0x06007994 RID: 31124 RVA: 0x002DD928 File Offset: 0x002DBB28
	public static bool IsWindowOpen(LocalPlayerUI _playerUi)
	{
		return _playerUi.windowManager.IsWindowOpen(XUiC_FocusedBlockHealth.ID);
	}

	// Token: 0x040059C1 RID: 22977
	public static string ID = "";

	// Token: 0x040059C2 RID: 22978
	[PublicizedFrom(EAccessModifier.Private)]
	public string text = "";

	// Token: 0x040059C3 RID: 22979
	[PublicizedFrom(EAccessModifier.Private)]
	public float fill;
}
