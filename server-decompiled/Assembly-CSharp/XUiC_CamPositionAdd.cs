using System;
using UnityEngine.Scripting;

// Token: 0x02000EDE RID: 3806
[Preserve]
public class XUiC_CamPositionAdd : XUiController
{
	// Token: 0x060076BA RID: 30394 RVA: 0x002CCDD0 File Offset: 0x002CAFD0
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

	// Token: 0x060076BB RID: 30395 RVA: 0x002CCEEF File Offset: 0x002CB0EF
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x060076BC RID: 30396 RVA: 0x002CCF09 File Offset: 0x002CB109
	[PublicizedFrom(EAccessModifier.Private)]
	public void Name_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.name = _text;
		base.RefreshBindings();
	}

	// Token: 0x060076BD RID: 30397 RVA: 0x002CCF18 File Offset: 0x002CB118
	[PublicizedFrom(EAccessModifier.Private)]
	public void Comment_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.comment = _text;
		base.RefreshBindings();
	}

	// Token: 0x060076BE RID: 30398 RVA: 0x002CCF27 File Offset: 0x002CB127
	[PublicizedFrom(EAccessModifier.Private)]
	public void Inputs_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.add();
	}

	// Token: 0x060076BF RID: 30399 RVA: 0x002CCF2F File Offset: 0x002CB12F
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

	// Token: 0x060076C0 RID: 30400 RVA: 0x002CCF61 File Offset: 0x002CB161
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

	// Token: 0x060076C1 RID: 30401 RVA: 0x002CCF95 File Offset: 0x002CB195
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validateInput()
	{
		return this.name.Trim().Length > 0;
	}

	// Token: 0x060076C2 RID: 30402 RVA: 0x002CCFAC File Offset: 0x002CB1AC
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

	// Token: 0x060076C3 RID: 30403 RVA: 0x002CCFE0 File Offset: 0x002CB1E0
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

	// Token: 0x060076C4 RID: 30404 RVA: 0x002CD052 File Offset: 0x002CB252
	public override void OnClose()
	{
		base.OnClose();
		this.modalWindowGroupEscCloseableBefore = null;
	}

	// Token: 0x060076C5 RID: 30405 RVA: 0x002CD066 File Offset: 0x002CB266
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x060076C6 RID: 30406 RVA: 0x002CD078 File Offset: 0x002CB278
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

	// Token: 0x04005804 RID: 22532
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CamPositionsList parentListWindow;

	// Token: 0x04005805 RID: 22533
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtName;

	// Token: 0x04005806 RID: 22534
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtComment;

	// Token: 0x04005807 RID: 22535
	[PublicizedFrom(EAccessModifier.Private)]
	public bool? modalWindowGroupEscCloseableBefore;

	// Token: 0x04005808 RID: 22536
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = string.Empty;

	// Token: 0x04005809 RID: 22537
	[PublicizedFrom(EAccessModifier.Private)]
	public string comment = string.Empty;
}
