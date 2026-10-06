using System;
using UnityEngine.Scripting;

// Token: 0x02000E7E RID: 3710
[Preserve]
public class XUiC_CaptionedTextBox : XUiController
{
	// Token: 0x17000D1A RID: 3354
	// (get) Token: 0x060071F8 RID: 29176 RVA: 0x002BD634 File Offset: 0x002BB834
	// (set) Token: 0x060071F9 RID: 29177 RVA: 0x002BD641 File Offset: 0x002BB841
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
	// (add) Token: 0x060071FA RID: 29178 RVA: 0x002BD650 File Offset: 0x002BB850
	// (remove) Token: 0x060071FB RID: 29179 RVA: 0x002BD688 File Offset: 0x002BB888
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000A6 RID: 166
	// (add) Token: 0x060071FC RID: 29180 RVA: 0x002BD6C0 File Offset: 0x002BB8C0
	// (remove) Token: 0x060071FD RID: 29181 RVA: 0x002BD6F8 File Offset: 0x002BB8F8
	public event XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x060071FE RID: 29182 RVA: 0x002BD72D File Offset: 0x002BB92D
	public override void Init()
	{
		base.Init();
		this.CustomAttributes.EntryModified += this.OnCustomAttributeModified;
		this.textInput.UIInputController.OnHover += this.Event_OnHover;
	}

	// Token: 0x060071FF RID: 29183 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCustomAttributeModified(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007200 RID: 29184 RVA: 0x0008A995 File Offset: 0x00088B95
	[XuiBindEvent("OnInteraction", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAnyEvent(XUiController _sender, EXUiControllerInteractionType _type)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007201 RID: 29185 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06007202 RID: 29186 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnHover(XUiController _sender, bool _isOver)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007203 RID: 29187 RVA: 0x002BD768 File Offset: 0x002BB968
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

	// Token: 0x06007204 RID: 29188 RVA: 0x002BD77A File Offset: 0x002BB97A
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

	// Token: 0x06007205 RID: 29189 RVA: 0x002BD7A3 File Offset: 0x002BB9A3
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

	// Token: 0x06007206 RID: 29190 RVA: 0x002BD7B7 File Offset: 0x002BB9B7
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

	// Token: 0x17000D1B RID: 3355
	// (get) Token: 0x06007207 RID: 29191 RVA: 0x002BD7EE File Offset: 0x002BB9EE
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

	// Token: 0x17000D1C RID: 3356
	// (get) Token: 0x06007208 RID: 29192 RVA: 0x002BD801 File Offset: 0x002BBA01
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

	// Token: 0x17000D1D RID: 3357
	// (get) Token: 0x06007209 RID: 29193 RVA: 0x002BD81E File Offset: 0x002BBA1E
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

	// Token: 0x17000D1E RID: 3358
	// (get) Token: 0x0600720A RID: 29194 RVA: 0x002BD837 File Offset: 0x002BBA37
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

	// Token: 0x17000D1F RID: 3359
	// (get) Token: 0x0600720B RID: 29195 RVA: 0x002BD50E File Offset: 0x002BB70E
	[XuiXmlBinding("box_attributes")]
	public ObservableDictionary<string, object> BindingCustomAttributes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomAttributes;
		}
	}

	// Token: 0x0400560D RID: 22029
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput textInput;
}
