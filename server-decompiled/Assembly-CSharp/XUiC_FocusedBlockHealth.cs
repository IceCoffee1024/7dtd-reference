using System;
using UnityEngine.Scripting;

// Token: 0x02000F53 RID: 3923
[Preserve]
public class XUiC_FocusedBlockHealth : XUiController
{
	// Token: 0x17000E72 RID: 3698
	// (get) Token: 0x06007A85 RID: 31365 RVA: 0x002E641A File Offset: 0x002E461A
	// (set) Token: 0x06007A86 RID: 31366 RVA: 0x002E6422 File Offset: 0x002E4622
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

	// Token: 0x17000E73 RID: 3699
	// (get) Token: 0x06007A87 RID: 31367 RVA: 0x002E6449 File Offset: 0x002E4649
	// (set) Token: 0x06007A88 RID: 31368 RVA: 0x002E6451 File Offset: 0x002E4651
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

	// Token: 0x06007A89 RID: 31369 RVA: 0x002E646A File Offset: 0x002E466A
	public override void Init()
	{
		base.Init();
		XUiC_FocusedBlockHealth.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007A8A RID: 31370 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007A8B RID: 31371 RVA: 0x002E6482 File Offset: 0x002E4682
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

	// Token: 0x06007A8C RID: 31372 RVA: 0x002E64C4 File Offset: 0x002E46C4
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

	// Token: 0x06007A8D RID: 31373 RVA: 0x002E653C File Offset: 0x002E473C
	public static bool IsWindowOpen(LocalPlayerUI _playerUi)
	{
		return _playerUi.windowManager.IsWindowOpen(XUiC_FocusedBlockHealth.ID);
	}

	// Token: 0x04005ADF RID: 23263
	public static string ID = "";

	// Token: 0x04005AE0 RID: 23264
	[PublicizedFrom(EAccessModifier.Private)]
	public string text = "";

	// Token: 0x04005AE1 RID: 23265
	[PublicizedFrom(EAccessModifier.Private)]
	public float fill;
}
