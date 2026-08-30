using System;
using UnityEngine.Scripting;

// Token: 0x02000F4C RID: 3916
[Preserve]
public class XUiC_InteractionPrompt : XUiController
{
	// Token: 0x17000E59 RID: 3673
	// (get) Token: 0x06007B06 RID: 31494 RVA: 0x002E6315 File Offset: 0x002E4515
	// (set) Token: 0x06007B07 RID: 31495 RVA: 0x002E631D File Offset: 0x002E451D
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
				this.text = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x06007B08 RID: 31496 RVA: 0x002E633B File Offset: 0x002E453B
	public override void Init()
	{
		base.Init();
		XUiC_InteractionPrompt.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007B09 RID: 31497 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007B0A RID: 31498 RVA: 0x002E6353 File Offset: 0x002E4553
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "text")
		{
			_value = this.text;
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06007B0B RID: 31499 RVA: 0x002E6374 File Offset: 0x002E4574
	public static void SetText(LocalPlayerUI _playerUi, string _text)
	{
		if (_playerUi != null && _playerUi.xui != null)
		{
			XUiController xuiController = _playerUi.xui.FindWindowGroupByName(XUiC_InteractionPrompt.ID);
			XUiC_InteractionPrompt xuiC_InteractionPrompt = ((xuiController != null) ? xuiController.GetChildByType<XUiC_InteractionPrompt>() : null);
			if (xuiC_InteractionPrompt == null)
			{
				return;
			}
			xuiC_InteractionPrompt.Text = _text;
			if (string.IsNullOrEmpty(_text))
			{
				_playerUi.windowManager.Close(XUiC_InteractionPrompt.ID);
				return;
			}
			_playerUi.windowManager.Open(XUiC_InteractionPrompt.ID, false);
		}
	}

	// Token: 0x04005B12 RID: 23314
	public static string ID = "";

	// Token: 0x04005B13 RID: 23315
	[PublicizedFrom(EAccessModifier.Private)]
	public string text;
}
