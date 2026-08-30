using System;
using UnityEngine.Scripting;

// Token: 0x02000ECF RID: 3791
[Preserve]
public class XUiC_CamPositionAdd : XUiController
{
	// Token: 0x060076BF RID: 30399 RVA: 0x002C99F8 File Offset: 0x002C7BF8
	public override void Init()
	{
		base.Init();
		this.parentListWindow = base.GetParentByType<XUiC_CamPositionsList>();
		XUiController childById = base.GetChildById("txtName");
		this.txtName = ((childById != null) ? childById.GetChildByType<XUiC_TextInput>() : null);
		XUiController childById2 = base.GetChildById("txtComment");
		this.txtComment = ((childById2 != null) ? childById2.GetChildByType<XUiC_TextInput>() : null);
		if (this.txtName != null)
		{
			this.txtName.OnChangeHandler += this.Name_OnChangeHandler;
			this.txtName.OnSubmitHandler += this.Inputs_OnSubmitHandler;
			this.txtName.SelectOnTab = this.txtComment;
		}
		if (this.txtComment != null)
		{
			this.txtComment.OnChangeHandler += this.Comment_OnChangeHandler;
			this.txtComment.OnSubmitHandler += this.Inputs_OnSubmitHandler;
			this.txtComment.SelectOnTab = this.txtName;
		}
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnAdd") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += delegate(XUiController _, int _)
			{
				this.add();
			};
		}
		base.OnVisiblity += this.OnVisibilityChanged;
	}

	// Token: 0x060076C0 RID: 30400 RVA: 0x002C9B17 File Offset: 0x002C7D17
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x060076C1 RID: 30401 RVA: 0x002C9B31 File Offset: 0x002C7D31
	[PublicizedFrom(EAccessModifier.Private)]
	public void Name_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.name = _text;
		base.RefreshBindings();
	}

	// Token: 0x060076C2 RID: 30402 RVA: 0x002C9B40 File Offset: 0x002C7D40
	[PublicizedFrom(EAccessModifier.Private)]
	public void Comment_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.comment = _text;
		base.RefreshBindings();
	}

	// Token: 0x060076C3 RID: 30403 RVA: 0x002C9B4F File Offset: 0x002C7D4F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Inputs_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.add();
	}

	// Token: 0x060076C4 RID: 30404 RVA: 0x002C9B57 File Offset: 0x002C7D57
	[PublicizedFrom(EAccessModifier.Private)]
	public void clear()
	{
		if (this.txtName != null)
		{
			this.txtName.Text = string.Empty;
		}
		if (this.txtComment != null)
		{
			this.txtComment.Text = string.Empty;
		}
	}

	// Token: 0x060076C5 RID: 30405 RVA: 0x002C9B89 File Offset: 0x002C7D89
	[PublicizedFrom(EAccessModifier.Private)]
	public void add()
	{
		if (!this.validateInput())
		{
			return;
		}
		this.parentListWindow.Add(this.name, this.comment);
		this.parentListWindow.ShowAddCamPositionWindow = false;
		this.clear();
	}

	// Token: 0x060076C6 RID: 30406 RVA: 0x002C9BBD File Offset: 0x002C7DBD
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validateInput()
	{
		return this.name.Trim().Length > 0;
	}

	// Token: 0x060076C7 RID: 30407 RVA: 0x002C9BD4 File Offset: 0x002C7DD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "valid_input")
		{
			_value = this.validateInput().ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x060076C8 RID: 30408 RVA: 0x002C9C08 File Offset: 0x002C7E08
	public void OnVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		GUIWindow modalWindow = this.xui.playerUI.windowManager.GetModalWindow();
		if (modalWindow == null)
		{
			return;
		}
		if (!_visibleInScene)
		{
			if (this.modalWindowGroupEscCloseableBefore != null)
			{
				modalWindow.isEscClosable = this.modalWindowGroupEscCloseableBefore.Value;
			}
			return;
		}
		this.modalWindowGroupEscCloseableBefore = new bool?(modalWindow.isEscClosable);
		modalWindow.isEscClosable = false;
		XUiC_TextInput xuiC_TextInput = this.txtName;
		if (xuiC_TextInput == null)
		{
			return;
		}
		xuiC_TextInput.SelectOrVirtualKeyboard(false);
	}

	// Token: 0x060076C9 RID: 30409 RVA: 0x002C9C7A File Offset: 0x002C7E7A
	public override void OnClose()
	{
		base.OnClose();
		this.modalWindowGroupEscCloseableBefore = null;
	}

	// Token: 0x060076CA RID: 30410 RVA: 0x002C9C8E File Offset: 0x002C7E8E
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x060076CB RID: 30411 RVA: 0x002C9CA0 File Offset: 0x002C7EA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		if (!this.viewComponent.IsVisible)
		{
			return;
		}
		if (this.xui.playerUI.playerInput.GUIActions.Cancel.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Cancel.WasPressed)
		{
			this.parentListWindow.ShowAddCamPositionWindow = false;
		}
	}

	// Token: 0x040057CC RID: 22476
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CamPositionsList parentListWindow;

	// Token: 0x040057CD RID: 22477
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtName;

	// Token: 0x040057CE RID: 22478
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtComment;

	// Token: 0x040057CF RID: 22479
	[PublicizedFrom(EAccessModifier.Private)]
	public bool? modalWindowGroupEscCloseableBefore;

	// Token: 0x040057D0 RID: 22480
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = string.Empty;

	// Token: 0x040057D1 RID: 22481
	[PublicizedFrom(EAccessModifier.Private)]
	public string comment = string.Empty;
}
