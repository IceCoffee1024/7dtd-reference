using System;
using UnityEngine.Scripting;

// Token: 0x02000EE9 RID: 3817
[Preserve]
public class XUiC_CamPositionAdd : XUiController
{
	// Token: 0x060076DF RID: 30431 RVA: 0x002CE28C File Offset: 0x002CC48C
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

	// Token: 0x060076E0 RID: 30432 RVA: 0x002CE3AB File Offset: 0x002CC5AB
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x060076E1 RID: 30433 RVA: 0x002CE3C5 File Offset: 0x002CC5C5
	[PublicizedFrom(EAccessModifier.Private)]
	public void Name_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.name = _text;
		base.RefreshBindings();
	}

	// Token: 0x060076E2 RID: 30434 RVA: 0x002CE3D4 File Offset: 0x002CC5D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Comment_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.comment = _text;
		base.RefreshBindings();
	}

	// Token: 0x060076E3 RID: 30435 RVA: 0x002CE3E3 File Offset: 0x002CC5E3
	[PublicizedFrom(EAccessModifier.Private)]
	public void Inputs_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.add();
	}

	// Token: 0x060076E4 RID: 30436 RVA: 0x002CE3EB File Offset: 0x002CC5EB
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

	// Token: 0x060076E5 RID: 30437 RVA: 0x002CE41D File Offset: 0x002CC61D
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

	// Token: 0x060076E6 RID: 30438 RVA: 0x002CE451 File Offset: 0x002CC651
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validateInput()
	{
		return this.name.Trim().Length > 0;
	}

	// Token: 0x060076E7 RID: 30439 RVA: 0x002CE468 File Offset: 0x002CC668
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

	// Token: 0x060076E8 RID: 30440 RVA: 0x002CE49C File Offset: 0x002CC69C
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

	// Token: 0x060076E9 RID: 30441 RVA: 0x002CE50E File Offset: 0x002CC70E
	public override void OnClose()
	{
		base.OnClose();
		this.modalWindowGroupEscCloseableBefore = null;
	}

	// Token: 0x060076EA RID: 30442 RVA: 0x002CE522 File Offset: 0x002CC722
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x060076EB RID: 30443 RVA: 0x002CE534 File Offset: 0x002CC734
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

	// Token: 0x04005829 RID: 22569
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CamPositionsList parentListWindow;

	// Token: 0x0400582A RID: 22570
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtName;

	// Token: 0x0400582B RID: 22571
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtComment;

	// Token: 0x0400582C RID: 22572
	[PublicizedFrom(EAccessModifier.Private)]
	public bool? modalWindowGroupEscCloseableBefore;

	// Token: 0x0400582D RID: 22573
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = string.Empty;

	// Token: 0x0400582E RID: 22574
	[PublicizedFrom(EAccessModifier.Private)]
	public string comment = string.Empty;
}
