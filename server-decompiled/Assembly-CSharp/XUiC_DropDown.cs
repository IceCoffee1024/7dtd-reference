using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E81 RID: 3713
[Preserve]
public class XUiC_DropDown : XUiController
{
	// Token: 0x140000AC RID: 172
	// (add) Token: 0x06007339 RID: 29497 RVA: 0x002BD118 File Offset: 0x002BB318
	// (remove) Token: 0x0600733A RID: 29498 RVA: 0x002BD150 File Offset: 0x002BB350
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000AD RID: 173
	// (add) Token: 0x0600733B RID: 29499 RVA: 0x002BD188 File Offset: 0x002BB388
	// (remove) Token: 0x0600733C RID: 29500 RVA: 0x002BD1C0 File Offset: 0x002BB3C0
	public event XUiC_DropDown.XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x17000D58 RID: 3416
	// (get) Token: 0x0600733D RID: 29501 RVA: 0x002BD1F5 File Offset: 0x002BB3F5
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

	// Token: 0x17000D59 RID: 3417
	// (get) Token: 0x0600733E RID: 29502 RVA: 0x002BD205 File Offset: 0x002BB405
	// (set) Token: 0x0600733F RID: 29503 RVA: 0x002BD20D File Offset: 0x002BB40D
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

	// Token: 0x17000D5A RID: 3418
	// (get) Token: 0x06007340 RID: 29504 RVA: 0x002BD227 File Offset: 0x002BB427
	// (set) Token: 0x06007341 RID: 29505 RVA: 0x002BD230 File Offset: 0x002BB430
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

	// Token: 0x17000D5B RID: 3419
	// (get) Token: 0x06007342 RID: 29506 RVA: 0x002BD263 File Offset: 0x002BB463
	public int LastPage
	{
		get
		{
			return Math.Max(0, Mathf.CeilToInt((float)this.filteredEntries.Count / (float)this.PageLength) - 1);
		}
	}

	// Token: 0x17000D5C RID: 3420
	// (get) Token: 0x06007343 RID: 29507 RVA: 0x002BD286 File Offset: 0x002BB486
	public int EntryCount
	{
		get
		{
			return this.filteredEntries.Count;
		}
	}

	// Token: 0x17000D5D RID: 3421
	// (get) Token: 0x06007344 RID: 29508 RVA: 0x002BD293 File Offset: 0x002BB493
	// (set) Token: 0x06007345 RID: 29509 RVA: 0x002BD2A0 File Offset: 0x002BB4A0
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

	// Token: 0x17000D5E RID: 3422
	// (get) Token: 0x06007346 RID: 29510 RVA: 0x002BD2C8 File Offset: 0x002BB4C8
	// (set) Token: 0x06007347 RID: 29511 RVA: 0x002BD2D0 File Offset: 0x002BB4D0
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

	// Token: 0x17000D5F RID: 3423
	// (get) Token: 0x06007348 RID: 29512 RVA: 0x002BD2EF File Offset: 0x002BB4EF
	// (set) Token: 0x06007349 RID: 29513 RVA: 0x002BD2F7 File Offset: 0x002BB4F7
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

	// Token: 0x17000D60 RID: 3424
	// (get) Token: 0x0600734A RID: 29514 RVA: 0x002BD316 File Offset: 0x002BB516
	[XuiXmlBinding("valid_input")]
	public bool ValidInput
	{
		get
		{
			return this.AllowCustomValue || (this.TextInput != null && this.AllEntries.Contains(this.Text));
		}
	}

	// Token: 0x17000D61 RID: 3425
	// (get) Token: 0x0600734B RID: 29515 RVA: 0x002BD33D File Offset: 0x002BB53D
	// (set) Token: 0x0600734C RID: 29516 RVA: 0x002BD345 File Offset: 0x002BB545
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

	// Token: 0x17000D62 RID: 3426
	// (get) Token: 0x0600734D RID: 29517 RVA: 0x002BD364 File Offset: 0x002BB564
	// (set) Token: 0x0600734E RID: 29518 RVA: 0x002BD36C File Offset: 0x002BB56C
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

	// Token: 0x17000D63 RID: 3427
	// (get) Token: 0x0600734F RID: 29519 RVA: 0x002BD386 File Offset: 0x002BB586
	// (set) Token: 0x06007350 RID: 29520 RVA: 0x002BD38E File Offset: 0x002BB58E
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

	// Token: 0x17000D64 RID: 3428
	// (get) Token: 0x06007351 RID: 29521 RVA: 0x002BD3A8 File Offset: 0x002BB5A8
	// (set) Token: 0x06007352 RID: 29522 RVA: 0x002BD3B0 File Offset: 0x002BB5B0
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

	// Token: 0x17000D65 RID: 3429
	// (get) Token: 0x06007353 RID: 29523 RVA: 0x002BD3CA File Offset: 0x002BB5CA
	[XuiXmlBinding("thumb_size")]
	public int ThumbSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.RoundToInt(this.ThumbAreaSize / (float)(this.LastPage + 1));
		}
	}

	// Token: 0x17000D66 RID: 3430
	// (get) Token: 0x06007354 RID: 29524 RVA: 0x002BD3E1 File Offset: 0x002BB5E1
	[XuiXmlBinding("thumb_position")]
	public int ThumbPosition
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.RoundToInt((float)this.Page / (float)(this.LastPage + 1) * this.ThumbAreaSize);
		}
	}

	// Token: 0x06007355 RID: 29525 RVA: 0x002BD400 File Offset: 0x002BB600
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

	// Token: 0x06007356 RID: 29526 RVA: 0x002BD4D1 File Offset: 0x002BB6D1
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDropdown_OnPress(XUiController _sender, int _mouseButton)
	{
		this.DropdownOpen = !this.DropdownOpen;
	}

	// Token: 0x06007357 RID: 29527 RVA: 0x002BD4E2 File Offset: 0x002BB6E2
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

	// Token: 0x06007358 RID: 29528 RVA: 0x002BD4FB File Offset: 0x002BB6FB
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

	// Token: 0x06007359 RID: 29529 RVA: 0x002BD50A File Offset: 0x002BB70A
	[XuiBindEvent("OnSubmitHandler", "TextInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnInputSubmit(XUiController _sender, string _text)
	{
		this.DropdownOpen = false;
		this.TextInput.SetSelected(false, false);
		this.OnInputChanged(_sender, _text, false);
		this.sendSubmitEvent();
	}

	// Token: 0x0600735A RID: 29530 RVA: 0x002BD52F File Offset: 0x002BB72F
	[XuiBindEvent("OnChangeHandler", "TextInput")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.UpdateFilteredList();
		base.RefreshBindings();
		this.sendChangedEvent(_changeFromCode);
	}

	// Token: 0x0600735B RID: 29531 RVA: 0x002BD544 File Offset: 0x002BB744
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

	// Token: 0x0600735C RID: 29532 RVA: 0x002BD564 File Offset: 0x002BB764
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

	// Token: 0x0600735D RID: 29533 RVA: 0x002BD580 File Offset: 0x002BB780
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

	// Token: 0x0600735E RID: 29534 RVA: 0x002BD63C File Offset: 0x002BB83C
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

	// Token: 0x0600735F RID: 29535 RVA: 0x002BD655 File Offset: 0x002BB855
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePageDownPress(XUiController _sender, int _mouseButton)
	{
		this.handlePageDown();
	}

	// Token: 0x06007360 RID: 29536 RVA: 0x002BD65E File Offset: 0x002BB85E
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePageUpPress(XUiController _sender, int _mouseButton)
	{
		this.handlePageUp();
	}

	// Token: 0x06007361 RID: 29537 RVA: 0x002BD668 File Offset: 0x002BB868
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

	// Token: 0x06007362 RID: 29538 RVA: 0x002BD6AC File Offset: 0x002BB8AC
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

	// Token: 0x06007363 RID: 29539 RVA: 0x002BD704 File Offset: 0x002BB904
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCurrentPageContents()
	{
		for (int i = 0; i < this.PageLength; i++)
		{
			int num = i + this.PageLength * this.page;
			this.listEntryControllers[i].Text = ((num < this.filteredEntries.Count) ? this.filteredEntries[num] : null);
		}
	}

	// Token: 0x06007364 RID: 29540 RVA: 0x002BD75C File Offset: 0x002BB95C
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

	// Token: 0x06007365 RID: 29541 RVA: 0x002BD7CA File Offset: 0x002BB9CA
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

	// Token: 0x0400562A RID: 22058
	public readonly List<string> AllEntries = new List<string>();

	// Token: 0x0400562B RID: 22059
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> filteredEntries = new List<string>();

	// Token: 0x0400562C RID: 22060
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DropDown.Entry[] listEntryControllers;

	// Token: 0x0400562D RID: 22061
	[XuiBindComponent("input", false)]
	public readonly XUiC_TextInput TextInput;

	// Token: 0x0400562E RID: 22062
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageDownAction;

	// Token: 0x0400562F RID: 22063
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageUpAction;

	// Token: 0x04005630 RID: 22064
	[PublicizedFrom(EAccessModifier.Private)]
	public float thumbAreaSize;

	// Token: 0x04005631 RID: 22065
	[PublicizedFrom(EAccessModifier.Private)]
	public Color dropdownHoverColor;

	// Token: 0x04005632 RID: 22066
	[PublicizedFrom(EAccessModifier.Private)]
	public Color dropdownTextColor;

	// Token: 0x04005633 RID: 22067
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sortEntries;

	// Token: 0x04005634 RID: 22068
	[PublicizedFrom(EAccessModifier.Private)]
	public bool clearOnOpen;

	// Token: 0x04005635 RID: 22069
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowCustomValue = true;

	// Token: 0x04005636 RID: 22070
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005637 RID: 22071
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dropdownOpen;

	// Token: 0x02000E82 RID: 3714
	[Preserve]
	public class Entry : XUiController
	{
		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06007367 RID: 29543 RVA: 0x002BD81E File Offset: 0x002BBA1E
		// (set) Token: 0x06007368 RID: 29544 RVA: 0x002BD82F File Offset: 0x002BBA2F
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

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06007369 RID: 29545 RVA: 0x002BD84E File Offset: 0x002BBA4E
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

		// Token: 0x0600736A RID: 29546 RVA: 0x002BD88C File Offset: 0x002BBA8C
		public override void OnOpen()
		{
			base.OnOpen();
			this.hovered = false;
			this.IsDirty = true;
		}

		// Token: 0x0600736B RID: 29547 RVA: 0x002BD8A2 File Offset: 0x002BBAA2
		[XuiBindEvent("OnPress", null)]
		[PublicizedFrom(EAccessModifier.Protected)]
		public void OnPressed(XUiController _sender, int _mouseButton)
		{
			this.Owner.Text = this.text;
			this.Owner.DropdownOpen = false;
			this.Owner.sendChangedEvent(false);
			this.Owner.sendSubmitEvent();
		}

		// Token: 0x0600736C RID: 29548 RVA: 0x002BD8D8 File Offset: 0x002BBAD8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnHovered(bool _isOver)
		{
			base.OnHovered(_isOver);
			this.hovered = _isOver;
			this.IsDirty = true;
		}

		// Token: 0x0600736D RID: 29549 RVA: 0x0028DC1B File Offset: 0x0028BE1B
		public override void Update(float _dt)
		{
			base.Update(_dt);
			base.handleDirtyUpdateDefault();
		}

		// Token: 0x04005638 RID: 22072
		public XUiC_DropDown Owner;

		// Token: 0x04005639 RID: 22073
		[PublicizedFrom(EAccessModifier.Private)]
		public string text;

		// Token: 0x0400563A RID: 22074
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hovered;
	}

	// Token: 0x02000E83 RID: 3715
	// (Invoke) Token: 0x06007370 RID: 29552
	public delegate void XUiEvent_InputOnChangedEventHandler(XUiController _sender, string _text, bool _validInput, bool _changeFromCode);
}
