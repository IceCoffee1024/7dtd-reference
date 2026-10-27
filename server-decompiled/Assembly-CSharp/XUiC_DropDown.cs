using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E9B RID: 3739
[Preserve]
public class XUiC_DropDown : XUiController
{
	// Token: 0x140000AB RID: 171
	// (add) Token: 0x06007356 RID: 29526 RVA: 0x002C1A34 File Offset: 0x002BFC34
	// (remove) Token: 0x06007357 RID: 29527 RVA: 0x002C1A6C File Offset: 0x002BFC6C
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000AC RID: 172
	// (add) Token: 0x06007358 RID: 29528 RVA: 0x002C1AA4 File Offset: 0x002BFCA4
	// (remove) Token: 0x06007359 RID: 29529 RVA: 0x002C1ADC File Offset: 0x002BFCDC
	public event XUiC_DropDown.XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x17000D79 RID: 3449
	// (get) Token: 0x0600735A RID: 29530 RVA: 0x002C1B11 File Offset: 0x002BFD11
	public int PageLength
	{
		get
		{
			XUiC_DropDown.Entry[] array = this.listEntryControllers;
			if (array == null)
			{
				return 1;
			}
			return array.Length;
		}
	}

	// Token: 0x17000D7A RID: 3450
	// (get) Token: 0x0600735B RID: 29531 RVA: 0x002C1B21 File Offset: 0x002BFD21
	// (set) Token: 0x0600735C RID: 29532 RVA: 0x002C1B29 File Offset: 0x002BFD29
	[XuiXmlBinding("dropdown_open")]
	public bool DropdownOpen
	{
		get
		{
			return this.dropdownOpen;
		}
		set
		{
			if (value == this.dropdownOpen)
			{
				return;
			}
			this.dropdownOpen = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D7B RID: 3451
	// (get) Token: 0x0600735D RID: 29533 RVA: 0x002C1B43 File Offset: 0x002BFD43
	// (set) Token: 0x0600735E RID: 29534 RVA: 0x002C1B4C File Offset: 0x002BFD4C
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			int num = Mathf.Clamp(value, 0, this.LastPage);
			if (num == this.page)
			{
				return;
			}
			this.page = num;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D7C RID: 3452
	// (get) Token: 0x0600735F RID: 29535 RVA: 0x002C1B7F File Offset: 0x002BFD7F
	public int LastPage
	{
		get
		{
			return Math.Max(0, Mathf.CeilToInt((float)this.filteredEntries.Count / (float)this.PageLength) - 1);
		}
	}

	// Token: 0x17000D7D RID: 3453
	// (get) Token: 0x06007360 RID: 29536 RVA: 0x002C1BA2 File Offset: 0x002BFDA2
	public int EntryCount
	{
		get
		{
			return this.filteredEntries.Count;
		}
	}

	// Token: 0x17000D7E RID: 3454
	// (get) Token: 0x06007361 RID: 29537 RVA: 0x002C1BAF File Offset: 0x002BFDAF
	// (set) Token: 0x06007362 RID: 29538 RVA: 0x002C1BBC File Offset: 0x002BFDBC
	public string Text
	{
		get
		{
			return this.TextInput.Text;
		}
		set
		{
			if (value == this.TextInput.Text)
			{
				return;
			}
			this.TextInput.Text = value;
			this.UpdateFilteredList();
		}
	}

	// Token: 0x17000D7F RID: 3455
	// (get) Token: 0x06007363 RID: 29539 RVA: 0x002C1BE4 File Offset: 0x002BFDE4
	// (set) Token: 0x06007364 RID: 29540 RVA: 0x002C1BEC File Offset: 0x002BFDEC
	[XuiXmlAttribute("dropdown_hovercolor", false)]
	public Color DropdownHoverColor
	{
		get
		{
			return this.dropdownHoverColor;
		}
		set
		{
			if (value == this.dropdownHoverColor)
			{
				return;
			}
			this.dropdownHoverColor = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D80 RID: 3456
	// (get) Token: 0x06007365 RID: 29541 RVA: 0x002C1C0B File Offset: 0x002BFE0B
	// (set) Token: 0x06007366 RID: 29542 RVA: 0x002C1C13 File Offset: 0x002BFE13
	[XuiXmlAttribute("dropdown_textcolor", false)]
	public Color DropdownTextColor
	{
		get
		{
			return this.dropdownTextColor;
		}
		set
		{
			if (value == this.dropdownTextColor)
			{
				return;
			}
			this.dropdownTextColor = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D81 RID: 3457
	// (get) Token: 0x06007367 RID: 29543 RVA: 0x002C1C32 File Offset: 0x002BFE32
	[XuiXmlBinding("valid_input")]
	public bool ValidInput
	{
		get
		{
			return this.AllowCustomValue || (this.TextInput != null && this.AllEntries.Contains(this.Text));
		}
	}

	// Token: 0x17000D82 RID: 3458
	// (get) Token: 0x06007368 RID: 29544 RVA: 0x002C1C59 File Offset: 0x002BFE59
	// (set) Token: 0x06007369 RID: 29545 RVA: 0x002C1C61 File Offset: 0x002BFE61
	[XuiXmlAttribute("thumbareasize", false)]
	public float ThumbAreaSize
	{
		get
		{
			return this.thumbAreaSize;
		}
		set
		{
			if (Mathf.Approximately(this.thumbAreaSize, value))
			{
				return;
			}
			this.thumbAreaSize = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D83 RID: 3459
	// (get) Token: 0x0600736A RID: 29546 RVA: 0x002C1C80 File Offset: 0x002BFE80
	// (set) Token: 0x0600736B RID: 29547 RVA: 0x002C1C88 File Offset: 0x002BFE88
	[XuiXmlAttribute("sortentries", false)]
	public bool SortEntries
	{
		get
		{
			return this.sortEntries;
		}
		set
		{
			if (value == this.sortEntries)
			{
				return;
			}
			this.sortEntries = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D84 RID: 3460
	// (get) Token: 0x0600736C RID: 29548 RVA: 0x002C1CA2 File Offset: 0x002BFEA2
	// (set) Token: 0x0600736D RID: 29549 RVA: 0x002C1CAA File Offset: 0x002BFEAA
	[XuiXmlAttribute("clearonopen", false)]
	public bool ClearOnOpen
	{
		get
		{
			return this.clearOnOpen;
		}
		set
		{
			if (value == this.clearOnOpen)
			{
				return;
			}
			this.clearOnOpen = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D85 RID: 3461
	// (get) Token: 0x0600736E RID: 29550 RVA: 0x002C1CC4 File Offset: 0x002BFEC4
	// (set) Token: 0x0600736F RID: 29551 RVA: 0x002C1CCC File Offset: 0x002BFECC
	[XuiXmlAttribute("allowcustomvalue", false)]
	public bool AllowCustomValue
	{
		get
		{
			return this.allowCustomValue;
		}
		set
		{
			if (value == this.allowCustomValue)
			{
				return;
			}
			this.allowCustomValue = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D86 RID: 3462
	// (get) Token: 0x06007370 RID: 29552 RVA: 0x002C1CE6 File Offset: 0x002BFEE6
	[XuiXmlBinding("thumb_size")]
	public int ThumbSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.RoundToInt(this.ThumbAreaSize / (float)(this.LastPage + 1));
		}
	}

	// Token: 0x17000D87 RID: 3463
	// (get) Token: 0x06007371 RID: 29553 RVA: 0x002C1CFD File Offset: 0x002BFEFD
	[XuiXmlBinding("thumb_position")]
	public int ThumbPosition
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.RoundToInt((float)this.Page / (float)(this.LastPage + 1) * this.ThumbAreaSize);
		}
	}

	// Token: 0x06007372 RID: 29554 RVA: 0x002C1D1C File Offset: 0x002BFF1C
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("pageUp");
		if (childById != null)
		{
			childById.OnPress += this.handlePageUpPress;
		}
		XUiController childById2 = base.GetChildById("pageDown");
		if (childById2 != null)
		{
			childById2.OnPress += this.handlePageDownPress;
		}
		this.handlePageDownAction = new Func<bool>(this.handlePageDown);
		this.handlePageUpAction = new Func<bool>(this.handlePageUp);
		for (int i = 0; i < this.listEntryControllers.Length; i++)
		{
			if (this.listEntryControllers[i] != null)
			{
				this.listEntryControllers[i].Owner = this;
			}
			else
			{
				Log.Warning("[XUi] DropDown elements do not have the correct controller set (should be \"XUiC_DropDown+Entry\")");
			}
		}
		XUiController childById3 = base.GetChildById("btnDropdown");
		if (childById3 != null)
		{
			childById3.OnPress += this.BtnDropdown_OnPress;
		}
	}

	// Token: 0x06007373 RID: 29555 RVA: 0x002C1DED File Offset: 0x002BFFED
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDropdown_OnPress(XUiController _sender, int _mouseButton)
	{
		this.DropdownOpen = !this.DropdownOpen;
	}

	// Token: 0x06007374 RID: 29556 RVA: 0x002C1DFE File Offset: 0x002BFFFE
	[XuiBindEvent("OnInputSelectedHandler", "TextInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnInputSelected(XUiController _sender, bool _selected)
	{
		if (_selected)
		{
			this.DropdownOpen = true;
			return;
		}
		ThreadManager.StartCoroutine(this.closeDropdownLater());
	}

	// Token: 0x06007375 RID: 29557 RVA: 0x002C1E17 File Offset: 0x002C0017
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeDropdownLater()
	{
		while (this.xui.playerUI.playerInput.GUIActions.LeftClick.IsPressed)
		{
			yield return null;
		}
		yield return null;
		if (!this.TextInput.IsSelected)
		{
			this.DropdownOpen = false;
		}
		yield break;
	}

	// Token: 0x06007376 RID: 29558 RVA: 0x002C1E26 File Offset: 0x002C0026
	[XuiBindEvent("OnSubmitHandler", "TextInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnInputSubmit(XUiController _sender, string _text)
	{
		this.DropdownOpen = false;
		this.TextInput.SetSelected(false, false);
		this.OnInputChanged(_sender, _text, false);
		this.sendSubmitEvent();
	}

	// Token: 0x06007377 RID: 29559 RVA: 0x002C1E4B File Offset: 0x002C004B
	[XuiBindEvent("OnChangeHandler", "TextInput")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.UpdateFilteredList();
		base.RefreshBindings();
		this.sendChangedEvent(_changeFromCode);
	}

	// Token: 0x06007378 RID: 29560 RVA: 0x002C1E60 File Offset: 0x002C0060
	[PublicizedFrom(EAccessModifier.Private)]
	public void sendChangedEvent(bool _changeFromCode)
	{
		XUiC_DropDown.XUiEvent_InputOnChangedEventHandler onChangeHandler = this.OnChangeHandler;
		if (onChangeHandler == null)
		{
			return;
		}
		onChangeHandler(this, this.Text, this.ValidInput, _changeFromCode);
	}

	// Token: 0x06007379 RID: 29561 RVA: 0x002C1E80 File Offset: 0x002C0080
	[PublicizedFrom(EAccessModifier.Private)]
	public void sendSubmitEvent()
	{
		XUiEvent_InputOnSubmitEventHandler onSubmitHandler = this.OnSubmitHandler;
		if (onSubmitHandler == null)
		{
			return;
		}
		onSubmitHandler(this, this.Text);
	}

	// Token: 0x0600737A RID: 29562 RVA: 0x002C1E9C File Offset: 0x002C009C
	public void UpdateFilteredList()
	{
		XUiC_TextInput textInput = this.TextInput;
		string text = ((textInput != null) ? textInput.Text : null);
		this.filteredEntries.Clear();
		if (!string.IsNullOrEmpty(text))
		{
			using (List<string>.Enumerator enumerator = this.AllEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text2 = enumerator.Current;
					if (text2.ContainsCaseInsensitive(text))
					{
						this.filteredEntries.Add(text2);
					}
				}
				goto IL_007B;
			}
		}
		this.filteredEntries.AddRange(this.AllEntries);
		IL_007B:
		if (this.SortEntries)
		{
			this.filteredEntries.Sort();
		}
		this.Page = 0;
		this.IsDirty = true;
	}

	// Token: 0x0600737B RID: 29563 RVA: 0x002C1F58 File Offset: 0x002C0158
	[XuiBindEvent("OnScroll", null)]
	[XuiBindEvent("OnScroll", "listEntryControllers")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			this.handlePageDown();
			return;
		}
		this.handlePageUp();
	}

	// Token: 0x0600737C RID: 29564 RVA: 0x002C1F71 File Offset: 0x002C0171
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePageDownPress(XUiController _sender, int _mouseButton)
	{
		this.handlePageDown();
	}

	// Token: 0x0600737D RID: 29565 RVA: 0x002C1F7A File Offset: 0x002C017A
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePageUpPress(XUiController _sender, int _mouseButton)
	{
		this.handlePageUp();
	}

	// Token: 0x0600737E RID: 29566 RVA: 0x002C1F84 File Offset: 0x002C0184
	[PublicizedFrom(EAccessModifier.Private)]
	public bool handlePageDown()
	{
		if (!this.TextInput.IsSelected)
		{
			this.TextInput.SetSelected(true, false);
		}
		if (this.page > 0)
		{
			int num = this.Page;
			this.Page = num - 1;
			return true;
		}
		return false;
	}

	// Token: 0x0600737F RID: 29567 RVA: 0x002C1FC8 File Offset: 0x002C01C8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool handlePageUp()
	{
		if (!this.TextInput.IsSelected)
		{
			this.TextInput.SetSelected(true, false);
		}
		if ((this.page + 1) * this.PageLength < this.filteredEntries.Count)
		{
			int num = this.Page;
			this.Page = num + 1;
			return true;
		}
		return false;
	}

	// Token: 0x06007380 RID: 29568 RVA: 0x002C2020 File Offset: 0x002C0220
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCurrentPageContents()
	{
		for (int i = 0; i < this.PageLength; i++)
		{
			int num = i + this.PageLength * this.page;
			this.listEntryControllers[i].Text = ((num < this.filteredEntries.Count) ? this.filteredEntries[num] : null);
		}
	}

	// Token: 0x06007381 RID: 29569 RVA: 0x002C2078 File Offset: 0x002C0278
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			if (this.page > this.LastPage)
			{
				this.Page = this.LastPage;
			}
			this.updateCurrentPageContents();
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
		if (base.ViewComponent.IsVisible)
		{
			XUi.HandlePaging(this.xui, this.handlePageUpAction, this.handlePageDownAction, false);
		}
	}

	// Token: 0x06007382 RID: 29570 RVA: 0x002C20E6 File Offset: 0x002C02E6
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.ClearOnOpen)
		{
			this.Text = string.Empty;
		}
		this.DropdownOpen = false;
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x04005686 RID: 22150
	public readonly List<string> AllEntries = new List<string>();

	// Token: 0x04005687 RID: 22151
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> filteredEntries = new List<string>();

	// Token: 0x04005688 RID: 22152
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DropDown.Entry[] listEntryControllers;

	// Token: 0x04005689 RID: 22153
	[XuiBindComponent("input", false)]
	public readonly XUiC_TextInput TextInput;

	// Token: 0x0400568A RID: 22154
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageDownAction;

	// Token: 0x0400568B RID: 22155
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageUpAction;

	// Token: 0x0400568C RID: 22156
	[PublicizedFrom(EAccessModifier.Private)]
	public float thumbAreaSize;

	// Token: 0x0400568D RID: 22157
	[PublicizedFrom(EAccessModifier.Private)]
	public Color dropdownHoverColor;

	// Token: 0x0400568E RID: 22158
	[PublicizedFrom(EAccessModifier.Private)]
	public Color dropdownTextColor;

	// Token: 0x0400568F RID: 22159
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sortEntries;

	// Token: 0x04005690 RID: 22160
	[PublicizedFrom(EAccessModifier.Private)]
	public bool clearOnOpen;

	// Token: 0x04005691 RID: 22161
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowCustomValue = true;

	// Token: 0x04005692 RID: 22162
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005693 RID: 22163
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dropdownOpen;

	// Token: 0x02000E9C RID: 3740
	[Preserve]
	public class Entry : XUiController
	{
		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06007384 RID: 29572 RVA: 0x002C213A File Offset: 0x002C033A
		// (set) Token: 0x06007385 RID: 29573 RVA: 0x002C214B File Offset: 0x002C034B
		[XuiXmlBinding("name")]
		public string Text
		{
			get
			{
				return this.text ?? "";
			}
			set
			{
				if (value == this.text)
				{
					return;
				}
				this.text = value;
				this.IsDirty = true;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06007386 RID: 29574 RVA: 0x002C216A File Offset: 0x002C036A
		[XuiXmlBinding("textcolor")]
		public Color TextColor
		{
			get
			{
				if (this.Owner == null)
				{
					return new Color(100f, 100f, 100f);
				}
				if (!this.hovered)
				{
					return this.Owner.DropdownTextColor;
				}
				return this.Owner.DropdownHoverColor;
			}
		}

		// Token: 0x06007387 RID: 29575 RVA: 0x002C21A8 File Offset: 0x002C03A8
		public override void OnOpen()
		{
			base.OnOpen();
			this.hovered = false;
			this.IsDirty = true;
		}

		// Token: 0x06007388 RID: 29576 RVA: 0x002C21BE File Offset: 0x002C03BE
		[XuiBindEvent("OnPress", null)]
		[PublicizedFrom(EAccessModifier.Protected)]
		public void OnPressed(XUiController _sender, int _mouseButton)
		{
			this.Owner.Text = this.text;
			this.Owner.DropdownOpen = false;
			this.Owner.sendChangedEvent(false);
			this.Owner.sendSubmitEvent();
		}

		// Token: 0x06007389 RID: 29577 RVA: 0x002C21F4 File Offset: 0x002C03F4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnHovered(bool _isOver)
		{
			base.OnHovered(_isOver);
			this.hovered = _isOver;
			this.IsDirty = true;
		}

		// Token: 0x0600738A RID: 29578 RVA: 0x002926AB File Offset: 0x002908AB
		public override void Update(float _dt)
		{
			base.Update(_dt);
			base.handleDirtyUpdateDefault();
		}

		// Token: 0x04005694 RID: 22164
		public XUiC_DropDown Owner;

		// Token: 0x04005695 RID: 22165
		[PublicizedFrom(EAccessModifier.Private)]
		public string text;

		// Token: 0x04005696 RID: 22166
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hovered;
	}

	// Token: 0x02000E9D RID: 3741
	// (Invoke) Token: 0x0600738D RID: 29581
	public delegate void XUiEvent_InputOnChangedEventHandler(XUiController _sender, string _text, bool _validInput, bool _changeFromCode);
}
