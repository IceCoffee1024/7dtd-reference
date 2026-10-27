using System;
using UnityEngine.Scripting;

// Token: 0x02000F6A RID: 3946
[Preserve]
public class XUiC_InteractionPrompt : XUiController
{
	// Token: 0x17000E80 RID: 3712
	// (get) Token: 0x06007B46 RID: 31558 RVA: 0x002EAF91 File Offset: 0x002E9191
	// (set) Token: 0x06007B47 RID: 31559 RVA: 0x002EAF99 File Offset: 0x002E9199
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

	// Token: 0x06007B48 RID: 31560 RVA: 0x002EAFB7 File Offset: 0x002E91B7
	public override void Init()
	{
		base.Init();
		XUiC_InteractionPrompt.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007B49 RID: 31561 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007B4A RID: 31562 RVA: 0x002EAFCF File Offset: 0x002E91CF
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

	// Token: 0x06007B4B RID: 31563 RVA: 0x002EAFF0 File Offset: 0x002E91F0
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

	// Token: 0x04005B7D RID: 23421
	public static string ID = "";

	// Token: 0x04005B7E RID: 23422
	[PublicizedFrom(EAccessModifier.Private)]
	public string text;
}
