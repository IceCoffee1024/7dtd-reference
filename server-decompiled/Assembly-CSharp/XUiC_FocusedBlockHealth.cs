using System;
using UnityEngine.Scripting;

// Token: 0x02000F44 RID: 3908
[Preserve]
public class XUiC_FocusedBlockHealth : XUiController
{
	// Token: 0x17000E68 RID: 3688
	// (get) Token: 0x06007A42 RID: 31298 RVA: 0x002E4B2A File Offset: 0x002E2D2A
	// (set) Token: 0x06007A43 RID: 31299 RVA: 0x002E4B32 File Offset: 0x002E2D32
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

	// Token: 0x17000E69 RID: 3689
	// (get) Token: 0x06007A44 RID: 31300 RVA: 0x002E4B59 File Offset: 0x002E2D59
	// (set) Token: 0x06007A45 RID: 31301 RVA: 0x002E4B61 File Offset: 0x002E2D61
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

	// Token: 0x06007A46 RID: 31302 RVA: 0x002E4B7A File Offset: 0x002E2D7A
	public override void Init()
	{
		base.Init();
		XUiC_FocusedBlockHealth.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007A47 RID: 31303 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007A48 RID: 31304 RVA: 0x002E4B92 File Offset: 0x002E2D92
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

	// Token: 0x06007A49 RID: 31305 RVA: 0x002E4BD4 File Offset: 0x002E2DD4
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

	// Token: 0x06007A4A RID: 31306 RVA: 0x002E4C4C File Offset: 0x002E2E4C
	public static bool IsWindowOpen(LocalPlayerUI _playerUi)
	{
		return _playerUi.windowManager.IsWindowOpen(XUiC_FocusedBlockHealth.ID);
	}

	// Token: 0x04005AAC RID: 23212
	public static string ID = "";

	// Token: 0x04005AAD RID: 23213
	[PublicizedFrom(EAccessModifier.Private)]
	public string text = "";

	// Token: 0x04005AAE RID: 23214
	[PublicizedFrom(EAccessModifier.Private)]
	public float fill;
}
