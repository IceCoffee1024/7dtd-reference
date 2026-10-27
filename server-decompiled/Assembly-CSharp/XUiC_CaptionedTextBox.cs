using System;
using UnityEngine.Scripting;

// Token: 0x02000E89 RID: 3721
[Preserve]
public class XUiC_CaptionedTextBox : XUiController
{
	// Token: 0x17000D20 RID: 3360
	// (get) Token: 0x0600721D RID: 29213 RVA: 0x002BEBA0 File Offset: 0x002BCDA0
	// (set) Token: 0x0600721E RID: 29214 RVA: 0x002BEBAD File Offset: 0x002BCDAD
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
	// (add) Token: 0x0600721F RID: 29215 RVA: 0x002BEBBC File Offset: 0x002BCDBC
	// (remove) Token: 0x06007220 RID: 29216 RVA: 0x002BEBF4 File Offset: 0x002BCDF4
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000A6 RID: 166
	// (add) Token: 0x06007221 RID: 29217 RVA: 0x002BEC2C File Offset: 0x002BCE2C
	// (remove) Token: 0x06007222 RID: 29218 RVA: 0x002BEC64 File Offset: 0x002BCE64
	public event XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x06007223 RID: 29219 RVA: 0x002BEC99 File Offset: 0x002BCE99
	public override void Init()
	{
		base.Init();
		this.CustomAttributes.EntryModified += this.OnCustomAttributeModified;
		this.textInput.UIInputController.OnHover += this.Event_OnHover;
	}

	// Token: 0x06007224 RID: 29220 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCustomAttributeModified(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007225 RID: 29221 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[XuiBindEvent("OnInteraction", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAnyEvent(XUiController _sender, EXUiControllerInteractionType _type)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007226 RID: 29222 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06007227 RID: 29223 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnHover(XUiController _sender, bool _isOver)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007228 RID: 29224 RVA: 0x002BECD4 File Offset: 0x002BCED4
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

	// Token: 0x06007229 RID: 29225 RVA: 0x002BECE6 File Offset: 0x002BCEE6
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

	// Token: 0x0600722A RID: 29226 RVA: 0x002BED0F File Offset: 0x002BCF0F
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

	// Token: 0x0600722B RID: 29227 RVA: 0x002BED23 File Offset: 0x002BCF23
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

	// Token: 0x17000D21 RID: 3361
	// (get) Token: 0x0600722C RID: 29228 RVA: 0x002BED5A File Offset: 0x002BCF5A
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

	// Token: 0x17000D22 RID: 3362
	// (get) Token: 0x0600722D RID: 29229 RVA: 0x002BED6D File Offset: 0x002BCF6D
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

	// Token: 0x17000D23 RID: 3363
	// (get) Token: 0x0600722E RID: 29230 RVA: 0x002BED8A File Offset: 0x002BCF8A
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

	// Token: 0x17000D24 RID: 3364
	// (get) Token: 0x0600722F RID: 29231 RVA: 0x002BEDA3 File Offset: 0x002BCFA3
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

	// Token: 0x17000D25 RID: 3365
	// (get) Token: 0x06007230 RID: 29232 RVA: 0x002BEA7A File Offset: 0x002BCC7A
	[XuiXmlBinding("box_attributes")]
	public ObservableDictionary<string, object> BindingCustomAttributes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomAttributes;
		}
	}

	// Token: 0x04005632 RID: 22066
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput textInput;
}
