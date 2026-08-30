using System;
using UnityEngine.Scripting;

// Token: 0x02000F35 RID: 3893
[Preserve]
public class XUiC_FocusedBlockHealth : XUiController
{
	// Token: 0x17000E4B RID: 3659
	// (get) Token: 0x06007A45 RID: 31301 RVA: 0x002E17BA File Offset: 0x002DF9BA
	// (set) Token: 0x06007A46 RID: 31302 RVA: 0x002E17C2 File Offset: 0x002DF9C2
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

	// Token: 0x17000E4C RID: 3660
	// (get) Token: 0x06007A47 RID: 31303 RVA: 0x002E17E9 File Offset: 0x002DF9E9
	// (set) Token: 0x06007A48 RID: 31304 RVA: 0x002E17F1 File Offset: 0x002DF9F1
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

	// Token: 0x06007A49 RID: 31305 RVA: 0x002E180A File Offset: 0x002DFA0A
	public override void Init()
	{
		base.Init();
		XUiC_FocusedBlockHealth.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007A4A RID: 31306 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007A4B RID: 31307 RVA: 0x002E1822 File Offset: 0x002DFA22
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

	// Token: 0x06007A4C RID: 31308 RVA: 0x002E1864 File Offset: 0x002DFA64
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

	// Token: 0x06007A4D RID: 31309 RVA: 0x002E18DC File Offset: 0x002DFADC
	public static bool IsWindowOpen(LocalPlayerUI _playerUi)
	{
		return _playerUi.windowManager.IsWindowOpen(XUiC_FocusedBlockHealth.ID);
	}

	// Token: 0x04005A74 RID: 23156
	public static string ID = "";

	// Token: 0x04005A75 RID: 23157
	[PublicizedFrom(EAccessModifier.Private)]
	public string text = "";

	// Token: 0x04005A76 RID: 23158
	[PublicizedFrom(EAccessModifier.Private)]
	public float fill;
}
