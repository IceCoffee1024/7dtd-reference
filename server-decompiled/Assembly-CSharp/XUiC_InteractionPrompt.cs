using System;
using UnityEngine.Scripting;

// Token: 0x02000F5B RID: 3931
[Preserve]
public class XUiC_InteractionPrompt : XUiController
{
	// Token: 0x17000E76 RID: 3702
	// (get) Token: 0x06007B03 RID: 31491 RVA: 0x002E9695 File Offset: 0x002E7895
	// (set) Token: 0x06007B04 RID: 31492 RVA: 0x002E969D File Offset: 0x002E789D
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

	// Token: 0x06007B05 RID: 31493 RVA: 0x002E96BB File Offset: 0x002E78BB
	public override void Init()
	{
		base.Init();
		XUiC_InteractionPrompt.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007B06 RID: 31494 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007B07 RID: 31495 RVA: 0x002E96D3 File Offset: 0x002E78D3
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

	// Token: 0x06007B08 RID: 31496 RVA: 0x002E96F4 File Offset: 0x002E78F4
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

	// Token: 0x04005B4A RID: 23370
	public static string ID = "";

	// Token: 0x04005B4B RID: 23371
	[PublicizedFrom(EAccessModifier.Private)]
	public string text;
}
