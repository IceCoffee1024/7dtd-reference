using System;
using UnityEngine.Scripting;

// Token: 0x02000E6F RID: 3695
[Preserve]
public class XUiC_CaptionedTextBox : XUiController
{
	// Token: 0x17000CFF RID: 3327
	// (get) Token: 0x06007200 RID: 29184 RVA: 0x002BA284 File Offset: 0x002B8484
	// (set) Token: 0x06007201 RID: 29185 RVA: 0x002BA291 File Offset: 0x002B8491
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

	// Token: 0x140000A6 RID: 166
	// (add) Token: 0x06007202 RID: 29186 RVA: 0x002BA2A0 File Offset: 0x002B84A0
	// (remove) Token: 0x06007203 RID: 29187 RVA: 0x002BA2D8 File Offset: 0x002B84D8
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000A7 RID: 167
	// (add) Token: 0x06007204 RID: 29188 RVA: 0x002BA310 File Offset: 0x002B8510
	// (remove) Token: 0x06007205 RID: 29189 RVA: 0x002BA348 File Offset: 0x002B8548
	public event XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x06007206 RID: 29190 RVA: 0x002BA37D File Offset: 0x002B857D
	public override void Init()
	{
		base.Init();
		this.CustomAttributes.EntryModified += this.OnCustomAttributeModified;
		this.textInput.UIInputController.OnHover += this.Event_OnHover;
	}

	// Token: 0x06007207 RID: 29191 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCustomAttributeModified(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007208 RID: 29192 RVA: 0x0008A715 File Offset: 0x00088915
	[XuiBindEvent("OnInteraction", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAnyEvent(XUiController _sender, EXUiControllerInteractionType _type)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007209 RID: 29193 RVA: 0x0028DC1B File Offset: 0x0028BE1B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600720A RID: 29194 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnHover(XUiController _sender, bool _isOver)
	{
		this.IsDirty = true;
	}

	// Token: 0x0600720B RID: 29195 RVA: 0x002BA3B8 File Offset: 0x002B85B8
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

	// Token: 0x0600720C RID: 29196 RVA: 0x002BA3CA File Offset: 0x002B85CA
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

	// Token: 0x0600720D RID: 29197 RVA: 0x002BA3F3 File Offset: 0x002B85F3
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

	// Token: 0x0600720E RID: 29198 RVA: 0x002BA407 File Offset: 0x002B8607
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

	// Token: 0x17000D00 RID: 3328
	// (get) Token: 0x0600720F RID: 29199 RVA: 0x002BA43E File Offset: 0x002B863E
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

	// Token: 0x17000D01 RID: 3329
	// (get) Token: 0x06007210 RID: 29200 RVA: 0x002BA451 File Offset: 0x002B8651
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

	// Token: 0x17000D02 RID: 3330
	// (get) Token: 0x06007211 RID: 29201 RVA: 0x002BA46E File Offset: 0x002B866E
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

	// Token: 0x17000D03 RID: 3331
	// (get) Token: 0x06007212 RID: 29202 RVA: 0x002BA487 File Offset: 0x002B8687
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

	// Token: 0x17000D04 RID: 3332
	// (get) Token: 0x06007213 RID: 29203 RVA: 0x002BA15E File Offset: 0x002B835E
	[XuiXmlBinding("box_attributes")]
	public ObservableDictionary<string, object> BindingCustomAttributes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomAttributes;
		}
	}

	// Token: 0x040055D6 RID: 21974
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput textInput;
}
