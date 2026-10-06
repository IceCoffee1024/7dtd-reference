using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E90 RID: 3728
[Preserve]
public class XUiC_DropDown : XUiController
{
	// Token: 0x140000AB RID: 171
	// (add) Token: 0x06007331 RID: 29489 RVA: 0x002C04C8 File Offset: 0x002BE6C8
	// (remove) Token: 0x06007332 RID: 29490 RVA: 0x002C0500 File Offset: 0x002BE700
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000AC RID: 172
	// (add) Token: 0x06007333 RID: 29491 RVA: 0x002C0538 File Offset: 0x002BE738
	// (remove) Token: 0x06007334 RID: 29492 RVA: 0x002C0570 File Offset: 0x002BE770
	public event XUiC_DropDown.XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x17000D73 RID: 3443
	// (get) Token: 0x06007335 RID: 29493 RVA: 0x002C05A5 File Offset: 0x002BE7A5
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

	// Token: 0x17000D74 RID: 3444
	// (get) Token: 0x06007336 RID: 29494 RVA: 0x002C05B5 File Offset: 0x002BE7B5
	// (set) Token: 0x06007337 RID: 29495 RVA: 0x002C05BD File Offset: 0x002BE7BD
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

	// Token: 0x17000D75 RID: 3445
	// (get) Token: 0x06007338 RID: 29496 RVA: 0x002C05D7 File Offset: 0x002BE7D7
	// (set) Token: 0x06007339 RID: 29497 RVA: 0x002C05E0 File Offset: 0x002BE7E0
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

	// Token: 0x17000D76 RID: 3446
	// (get) Token: 0x0600733A RID: 29498 RVA: 0x002C0613 File Offset: 0x002BE813
	public int LastPage
	{
		get
		{
			return Math.Max(0, Mathf.CeilToInt((float)this.filteredEntries.Count / (float)this.PageLength) - 1);
		}
	}

	// Token: 0x17000D77 RID: 3447
	// (get) Token: 0x0600733B RID: 29499 RVA: 0x002C0636 File Offset: 0x002BE836
	public int EntryCount
	{
		get
		{
			return this.filteredEntries.Count;
		}
	}

	// Token: 0x17000D78 RID: 3448
	// (get) Token: 0x0600733C RID: 29500 RVA: 0x002C0643 File Offset: 0x002BE843
	// (set) Token: 0x0600733D RID: 29501 RVA: 0x002C0650 File Offset: 0x002BE850
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

	// Token: 0x17000D79 RID: 3449
	// (get) Token: 0x0600733E RID: 29502 RVA: 0x002C0678 File Offset: 0x002BE878
	// (set) Token: 0x0600733F RID: 29503 RVA: 0x002C0680 File Offset: 0x002BE880
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

	// Token: 0x17000D7A RID: 3450
	// (get) Token: 0x06007340 RID: 29504 RVA: 0x002C069F File Offset: 0x002BE89F
	// (set) Token: 0x06007341 RID: 29505 RVA: 0x002C06A7 File Offset: 0x002BE8A7
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

	// Token: 0x17000D7B RID: 3451
	// (get) Token: 0x06007342 RID: 29506 RVA: 0x002C06C6 File Offset: 0x002BE8C6
	[XuiXmlBinding("valid_input")]
	public bool ValidInput
	{
		get
		{
			return this.AllowCustomValue || (this.TextInput != null && this.AllEntries.Contains(this.Text));
		}
	}

	// Token: 0x17000D7C RID: 3452
	// (get) Token: 0x06007343 RID: 29507 RVA: 0x002C06ED File Offset: 0x002BE8ED
	// (set) Token: 0x06007344 RID: 29508 RVA: 0x002C06F5 File Offset: 0x002BE8F5
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

	// Token: 0x17000D7D RID: 3453
	// (get) Token: 0x06007345 RID: 29509 RVA: 0x002C0714 File Offset: 0x002BE914
	// (set) Token: 0x06007346 RID: 29510 RVA: 0x002C071C File Offset: 0x002BE91C
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

	// Token: 0x17000D7E RID: 3454
	// (get) Token: 0x06007347 RID: 29511 RVA: 0x002C0736 File Offset: 0x002BE936
	// (set) Token: 0x06007348 RID: 29512 RVA: 0x002C073E File Offset: 0x002BE93E
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

	// Token: 0x17000D7F RID: 3455
	// (get) Token: 0x06007349 RID: 29513 RVA: 0x002C0758 File Offset: 0x002BE958
	// (set) Token: 0x0600734A RID: 29514 RVA: 0x002C0760 File Offset: 0x002BE960
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

	// Token: 0x17000D80 RID: 3456
	// (get) Token: 0x0600734B RID: 29515 RVA: 0x002C077A File Offset: 0x002BE97A
	[XuiXmlBinding("thumb_size")]
	public int ThumbSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.RoundToInt(this.ThumbAreaSize / (float)(this.LastPage + 1));
		}
	}

	// Token: 0x17000D81 RID: 3457
	// (get) Token: 0x0600734C RID: 29516 RVA: 0x002C0791 File Offset: 0x002BE991
	[XuiXmlBinding("thumb_position")]
	public int ThumbPosition
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.RoundToInt((float)this.Page / (float)(this.LastPage + 1) * this.ThumbAreaSize);
		}
	}

	// Token: 0x0600734D RID: 29517 RVA: 0x002C07B0 File Offset: 0x002BE9B0
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

	// Token: 0x0600734E RID: 29518 RVA: 0x002C0881 File Offset: 0x002BEA81
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDropdown_OnPress(XUiController _sender, int _mouseButton)
	{
		this.DropdownOpen = !this.DropdownOpen;
	}

	// Token: 0x0600734F RID: 29519 RVA: 0x002C0892 File Offset: 0x002BEA92
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

	// Token: 0x06007350 RID: 29520 RVA: 0x002C08AB File Offset: 0x002BEAAB
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

	// Token: 0x06007351 RID: 29521 RVA: 0x002C08BA File Offset: 0x002BEABA
	[XuiBindEvent("OnSubmitHandler", "TextInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnInputSubmit(XUiController _sender, string _text)
	{
		this.DropdownOpen = false;
		this.TextInput.SetSelected(false, false);
		this.OnInputChanged(_sender, _text, false);
		this.sendSubmitEvent();
	}

	// Token: 0x06007352 RID: 29522 RVA: 0x002C08DF File Offset: 0x002BEADF
	[XuiBindEvent("OnChangeHandler", "TextInput")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.UpdateFilteredList();
		base.RefreshBindings();
		this.sendChangedEvent(_changeFromCode);
	}

	// Token: 0x06007353 RID: 29523 RVA: 0x002C08F4 File Offset: 0x002BEAF4
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

	// Token: 0x06007354 RID: 29524 RVA: 0x002C0914 File Offset: 0x002BEB14
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

	// Token: 0x06007355 RID: 29525 RVA: 0x002C0930 File Offset: 0x002BEB30
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

	// Token: 0x06007356 RID: 29526 RVA: 0x002C09EC File Offset: 0x002BEBEC
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

	// Token: 0x06007357 RID: 29527 RVA: 0x002C0A05 File Offset: 0x002BEC05
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePageDownPress(XUiController _sender, int _mouseButton)
	{
		this.handlePageDown();
	}

	// Token: 0x06007358 RID: 29528 RVA: 0x002C0A0E File Offset: 0x002BEC0E
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePageUpPress(XUiController _sender, int _mouseButton)
	{
		this.handlePageUp();
	}

	// Token: 0x06007359 RID: 29529 RVA: 0x002C0A18 File Offset: 0x002BEC18
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

	// Token: 0x0600735A RID: 29530 RVA: 0x002C0A5C File Offset: 0x002BEC5C
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

	// Token: 0x0600735B RID: 29531 RVA: 0x002C0AB4 File Offset: 0x002BECB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCurrentPageContents()
	{
		for (int i = 0; i < this.PageLength; i++)
		{
			int num = i + this.PageLength * this.page;
			this.listEntryControllers[i].Text = ((num < this.filteredEntries.Count) ? this.filteredEntries[num] : null);
		}
	}

	// Token: 0x0600735C RID: 29532 RVA: 0x002C0B0C File Offset: 0x002BED0C
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

	// Token: 0x0600735D RID: 29533 RVA: 0x002C0B7A File Offset: 0x002BED7A
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

	// Token: 0x04005661 RID: 22113
	public readonly List<string> AllEntries = new List<string>();

	// Token: 0x04005662 RID: 22114
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> filteredEntries = new List<string>();

	// Token: 0x04005663 RID: 22115
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DropDown.Entry[] listEntryControllers;

	// Token: 0x04005664 RID: 22116
	[XuiBindComponent("input", false)]
	public readonly XUiC_TextInput TextInput;

	// Token: 0x04005665 RID: 22117
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageDownAction;

	// Token: 0x04005666 RID: 22118
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageUpAction;

	// Token: 0x04005667 RID: 22119
	[PublicizedFrom(EAccessModifier.Private)]
	public float thumbAreaSize;

	// Token: 0x04005668 RID: 22120
	[PublicizedFrom(EAccessModifier.Private)]
	public Color dropdownHoverColor;

	// Token: 0x04005669 RID: 22121
	[PublicizedFrom(EAccessModifier.Private)]
	public Color dropdownTextColor;

	// Token: 0x0400566A RID: 22122
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sortEntries;

	// Token: 0x0400566B RID: 22123
	[PublicizedFrom(EAccessModifier.Private)]
	public bool clearOnOpen;

	// Token: 0x0400566C RID: 22124
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowCustomValue = true;

	// Token: 0x0400566D RID: 22125
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400566E RID: 22126
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dropdownOpen;

	// Token: 0x02000E91 RID: 3729
	[Preserve]
	public class Entry : XUiController
	{
		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x0600735F RID: 29535 RVA: 0x002C0BCE File Offset: 0x002BEDCE
		// (set) Token: 0x06007360 RID: 29536 RVA: 0x002C0BDF File Offset: 0x002BEDDF
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

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06007361 RID: 29537 RVA: 0x002C0BFE File Offset: 0x002BEDFE
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

		// Token: 0x06007362 RID: 29538 RVA: 0x002C0C3C File Offset: 0x002BEE3C
		public override void OnOpen()
		{
			base.OnOpen();
			this.hovered = false;
			this.IsDirty = true;
		}

		// Token: 0x06007363 RID: 29539 RVA: 0x002C0C52 File Offset: 0x002BEE52
		[XuiBindEvent("OnPress", null)]
		[PublicizedFrom(EAccessModifier.Protected)]
		public void OnPressed(XUiController _sender, int _mouseButton)
		{
			this.Owner.Text = this.text;
			this.Owner.DropdownOpen = false;
			this.Owner.sendChangedEvent(false);
			this.Owner.sendSubmitEvent();
		}

		// Token: 0x06007364 RID: 29540 RVA: 0x002C0C88 File Offset: 0x002BEE88
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnHovered(bool _isOver)
		{
			base.OnHovered(_isOver);
			this.hovered = _isOver;
			this.IsDirty = true;
		}

		// Token: 0x06007365 RID: 29541 RVA: 0x00291183 File Offset: 0x0028F383
		public override void Update(float _dt)
		{
			base.Update(_dt);
			base.handleDirtyUpdateDefault();
		}

		// Token: 0x0400566F RID: 22127
		public XUiC_DropDown Owner;

		// Token: 0x04005670 RID: 22128
		[PublicizedFrom(EAccessModifier.Private)]
		public string text;

		// Token: 0x04005671 RID: 22129
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hovered;
	}

	// Token: 0x02000E92 RID: 3730
	// (Invoke) Token: 0x06007368 RID: 29544
	public delegate void XUiEvent_InputOnChangedEventHandler(XUiController _sender, string _text, bool _validInput, bool _changeFromCode);
}
