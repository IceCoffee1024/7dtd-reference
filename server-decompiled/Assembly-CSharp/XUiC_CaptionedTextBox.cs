using System;
using UnityEngine.Scripting;

// Token: 0x02000E5A RID: 3674
[Preserve]
public class XUiC_CaptionedTextBox : XUiController
{
	// Token: 0x17000CE6 RID: 3302
	// (get) Token: 0x06007160 RID: 29024 RVA: 0x002B6D60 File Offset: 0x002B4F60
	// (set) Token: 0x06007161 RID: 29025 RVA: 0x002B6D6D File Offset: 0x002B4F6D
	public string Text
	{
		get
		{
			return this.textInput.Text;
		}
		set
		{
			this.textInput.Text = value;
		}
	}

	// Token: 0x140000A5 RID: 165
	// (add) Token: 0x06007162 RID: 29026 RVA: 0x002B6D7C File Offset: 0x002B4F7C
	// (remove) Token: 0x06007163 RID: 29027 RVA: 0x002B6DB4 File Offset: 0x002B4FB4
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000A6 RID: 166
	// (add) Token: 0x06007164 RID: 29028 RVA: 0x002B6DEC File Offset: 0x002B4FEC
	// (remove) Token: 0x06007165 RID: 29029 RVA: 0x002B6E24 File Offset: 0x002B5024
	public event XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x06007166 RID: 29030 RVA: 0x002B6E59 File Offset: 0x002B5059
	public override void Init()
	{
		base.Init();
		this.CustomAttributes.EntryModified += this.OnCustomAttributeModified;
		this.textInput.UIInputController.OnHover += this.Event_OnHover;
	}

	// Token: 0x06007167 RID: 29031 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCustomAttributeModified(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007168 RID: 29032 RVA: 0x0008A069 File Offset: 0x00088269
	[XuiBindEvent("OnInteraction", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAnyEvent(XUiController _sender, EXUiControllerInteractionType _type)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007169 RID: 29033 RVA: 0x0028AD37 File Offset: 0x00288F37
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600716A RID: 29034 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnHover(XUiController _sender, bool _isOver)
	{
		this.IsDirty = true;
	}

	// Token: 0x0600716B RID: 29035 RVA: 0x002B6E94 File Offset: 0x002B5094
	[XuiBindEvent("OnInputSelectedHandler", "textInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnInputSelected(XUiController _sender, bool _selected)
	{
		if (!_selected)
		{
			this.switchToButtonIfEmpty();
		}
		this.IsDirty = true;
	}

	// Token: 0x0600716C RID: 29036 RVA: 0x002B6EA6 File Offset: 0x002B50A6
	[XuiBindEvent("OnChangeHandler", "textInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		XUiEvent_InputOnChangedEventHandler onChangeHandler = this.OnChangeHandler;
		if (onChangeHandler != null)
		{
			onChangeHandler(_sender, _text, _changeFromCode);
		}
		if (!this.textInput.IsSelected)
		{
			this.switchToButtonIfEmpty();
		}
	}

	// Token: 0x0600716D RID: 29037 RVA: 0x002B6ECF File Offset: 0x002B50CF
	[XuiBindEvent("OnSubmitHandler", "textInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnSubmitChanged(XUiController _sender, string _text)
	{
		XUiEvent_InputOnSubmitEventHandler onSubmitHandler = this.OnSubmitHandler;
		if (onSubmitHandler == null)
		{
			return;
		}
		onSubmitHandler(_sender, _text);
	}

	// Token: 0x0600716E RID: 29038 RVA: 0x002B6EE3 File Offset: 0x002B50E3
	[PublicizedFrom(EAccessModifier.Private)]
	public void switchToButtonIfEmpty()
	{
		if (!string.IsNullOrWhiteSpace(this.textInput.Text))
		{
			return;
		}
		this.textInput.Text = "";
		ThreadManager.RunTaskAfterFrames(delegate
		{
			this.IsDirty = true;
		}, 1);
	}

	// Token: 0x17000CE7 RID: 3303
	// (get) Token: 0x0600716F RID: 29039 RVA: 0x002B6F1A File Offset: 0x002B511A
	[XuiXmlBinding("box_enabled")]
	public bool BindingEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiView viewComponent = this.viewComponent;
			return viewComponent == null || viewComponent.Enabled;
		}
	}

	// Token: 0x17000CE8 RID: 3304
	// (get) Token: 0x06007170 RID: 29040 RVA: 0x002B6F2D File Offset: 0x002B512D
	[XuiXmlBinding("box_hovered")]
	public bool BindingHovered
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_TextInput xuiC_TextInput = this.textInput;
			return xuiC_TextInput != null && xuiC_TextInput.UIInputController.ViewComponent.IsHovered;
		}
	}

	// Token: 0x17000CE9 RID: 3305
	// (get) Token: 0x06007171 RID: 29041 RVA: 0x002B6F4A File Offset: 0x002B514A
	[XuiXmlBinding("box_empty")]
	public bool BindingEmpty
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_TextInput xuiC_TextInput = this.textInput;
			return string.IsNullOrEmpty((xuiC_TextInput != null) ? xuiC_TextInput.Text : null);
		}
	}

	// Token: 0x17000CEA RID: 3306
	// (get) Token: 0x06007172 RID: 29042 RVA: 0x002B6F63 File Offset: 0x002B5163
	[XuiXmlBinding("box_selected")]
	public bool BindingSelected
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_TextInput xuiC_TextInput = this.textInput;
			return xuiC_TextInput != null && xuiC_TextInput.IsSelected;
		}
	}

	// Token: 0x17000CEB RID: 3307
	// (get) Token: 0x06007173 RID: 29043 RVA: 0x002B6C3A File Offset: 0x002B4E3A
	[XuiXmlBinding("box_attributes")]
	public ObservableDictionary<string, object> BindingCustomAttributes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomAttributes;
		}
	}

	// Token: 0x04005532 RID: 21810
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput textInput;
}
