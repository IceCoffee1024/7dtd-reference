using System;
using UnityEngine.Scripting;

// Token: 0x02000EBA RID: 3770
[Preserve]
public class XUiC_CamPositionAdd : XUiController
{
	// Token: 0x0600761E RID: 30238 RVA: 0x002C64D4 File Offset: 0x002C46D4
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

	// Token: 0x0600761F RID: 30239 RVA: 0x002C65F3 File Offset: 0x002C47F3
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x06007620 RID: 30240 RVA: 0x002C660D File Offset: 0x002C480D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Name_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.name = _text;
		base.RefreshBindings();
	}

	// Token: 0x06007621 RID: 30241 RVA: 0x002C661C File Offset: 0x002C481C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Comment_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.comment = _text;
		base.RefreshBindings();
	}

	// Token: 0x06007622 RID: 30242 RVA: 0x002C662B File Offset: 0x002C482B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Inputs_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.add();
	}

	// Token: 0x06007623 RID: 30243 RVA: 0x002C6633 File Offset: 0x002C4833
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

	// Token: 0x06007624 RID: 30244 RVA: 0x002C6665 File Offset: 0x002C4865
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

	// Token: 0x06007625 RID: 30245 RVA: 0x002C6699 File Offset: 0x002C4899
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validateInput()
	{
		return this.name.Trim().Length > 0;
	}

	// Token: 0x06007626 RID: 30246 RVA: 0x002C66B0 File Offset: 0x002C48B0
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

	// Token: 0x06007627 RID: 30247 RVA: 0x002C66E4 File Offset: 0x002C48E4
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

	// Token: 0x06007628 RID: 30248 RVA: 0x002C6756 File Offset: 0x002C4956
	public override void OnClose()
	{
		base.OnClose();
		this.modalWindowGroupEscCloseableBefore = null;
	}

	// Token: 0x06007629 RID: 30249 RVA: 0x002C676A File Offset: 0x002C496A
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x0600762A RID: 30250 RVA: 0x002C677C File Offset: 0x002C497C
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

	// Token: 0x04005727 RID: 22311
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CamPositionsList parentListWindow;

	// Token: 0x04005728 RID: 22312
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtName;

	// Token: 0x04005729 RID: 22313
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtComment;

	// Token: 0x0400572A RID: 22314
	[PublicizedFrom(EAccessModifier.Private)]
	public bool? modalWindowGroupEscCloseableBefore;

	// Token: 0x0400572B RID: 22315
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = string.Empty;

	// Token: 0x0400572C RID: 22316
	[PublicizedFrom(EAccessModifier.Private)]
	public string comment = string.Empty;
}
