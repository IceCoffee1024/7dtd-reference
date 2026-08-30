using System;
using UnityEngine.Scripting;

// Token: 0x02000F35 RID: 3893
[Preserve]
public class XUiC_InteractionPrompt : XUiController
{
	// Token: 0x17000E3E RID: 3646
	// (get) Token: 0x06007A4D RID: 31309 RVA: 0x002E2361 File Offset: 0x002E0561
	// (set) Token: 0x06007A4E RID: 31310 RVA: 0x002E2369 File Offset: 0x002E0569
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

	// Token: 0x06007A4F RID: 31311 RVA: 0x002E2387 File Offset: 0x002E0587
	public override void Init()
	{
		base.Init();
		XUiC_InteractionPrompt.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007A50 RID: 31312 RVA: 0x0008A091 File Offset: 0x00088291
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007A51 RID: 31313 RVA: 0x002E239F File Offset: 0x002E059F
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

	// Token: 0x06007A52 RID: 31314 RVA: 0x002E23C0 File Offset: 0x002E05C0
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

	// Token: 0x04005A5F RID: 23135
	public static string ID = "";

	// Token: 0x04005A60 RID: 23136
	[PublicizedFrom(EAccessModifier.Private)]
	public string text;
}
