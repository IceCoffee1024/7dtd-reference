using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E6C RID: 3692
[Preserve]
public class XUiC_DropDown : XUiController
{
	// Token: 0x140000AB RID: 171
	// (add) Token: 0x06007299 RID: 29337 RVA: 0x002B9BF4 File Offset: 0x002B7DF4
	// (remove) Token: 0x0600729A RID: 29338 RVA: 0x002B9C2C File Offset: 0x002B7E2C
	public event XUiEvent_InputOnSubmitEventHandler OnSubmitHandler;

	// Token: 0x140000AC RID: 172
	// (add) Token: 0x0600729B RID: 29339 RVA: 0x002B9C64 File Offset: 0x002B7E64
	// (remove) Token: 0x0600729C RID: 29340 RVA: 0x002B9C9C File Offset: 0x002B7E9C
	public event XUiC_DropDown.XUiEvent_InputOnChangedEventHandler OnChangeHandler;

	// Token: 0x17000D3F RID: 3391
	// (get) Token: 0x0600729D RID: 29341 RVA: 0x002B9CD1 File Offset: 0x002B7ED1
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

	// Token: 0x17000D40 RID: 3392
	// (get) Token: 0x0600729E RID: 29342 RVA: 0x002B9CE1 File Offset: 0x002B7EE1
	// (set) Token: 0x0600729F RID: 29343 RVA: 0x002B9CE9 File Offset: 0x002B7EE9
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

	// Token: 0x17000D41 RID: 3393
	// (get) Token: 0x060072A0 RID: 29344 RVA: 0x002B9D03 File Offset: 0x002B7F03
	// (set) Token: 0x060072A1 RID: 29345 RVA: 0x002B9D0C File Offset: 0x002B7F0C
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

	// Token: 0x17000D42 RID: 3394
	// (get) Token: 0x060072A2 RID: 29346 RVA: 0x002B9D3F File Offset: 0x002B7F3F
	public int LastPage
	{
		get
		{
			return Math.Max(0, Mathf.CeilToInt((float)this.filteredEntries.Count / (float)this.PageLength) - 1);
		}
	}

	// Token: 0x17000D43 RID: 3395
	// (get) Token: 0x060072A3 RID: 29347 RVA: 0x002B9D62 File Offset: 0x002B7F62
	public int EntryCount
	{
		get
		{
			return this.filteredEntries.Count;
		}
	}

	// Token: 0x17000D44 RID: 3396
	// (get) Token: 0x060072A4 RID: 29348 RVA: 0x002B9D6F File Offset: 0x002B7F6F
	// (set) Token: 0x060072A5 RID: 29349 RVA: 0x002B9D7C File Offset: 0x002B7F7C
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

	// Token: 0x17000D45 RID: 3397
	// (get) Token: 0x060072A6 RID: 29350 RVA: 0x002B9DA4 File Offset: 0x002B7FA4
	// (set) Token: 0x060072A7 RID: 29351 RVA: 0x002B9DAC File Offset: 0x002B7FAC
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

	// Token: 0x17000D46 RID: 3398
	// (get) Token: 0x060072A8 RID: 29352 RVA: 0x002B9DCB File Offset: 0x002B7FCB
	// (set) Token: 0x060072A9 RID: 29353 RVA: 0x002B9DD3 File Offset: 0x002B7FD3
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

	// Token: 0x17000D47 RID: 3399
	// (get) Token: 0x060072AA RID: 29354 RVA: 0x002B9DF2 File Offset: 0x002B7FF2
	[XuiXmlBinding("valid_input")]
	public bool ValidInput
	{
		get
		{
			return this.AllowCustomValue || (this.TextInput != null && this.AllEntries.Contains(this.Text));
		}
	}

	// Token: 0x17000D48 RID: 3400
	// (get) Token: 0x060072AB RID: 29355 RVA: 0x002B9E19 File Offset: 0x002B8019
	// (set) Token: 0x060072AC RID: 29356 RVA: 0x002B9E21 File Offset: 0x002B8021
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

	// Token: 0x17000D49 RID: 3401
	// (get) Token: 0x060072AD RID: 29357 RVA: 0x002B9E40 File Offset: 0x002B8040
	// (set) Token: 0x060072AE RID: 29358 RVA: 0x002B9E48 File Offset: 0x002B8048
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

	// Token: 0x17000D4A RID: 3402
	// (get) Token: 0x060072AF RID: 29359 RVA: 0x002B9E62 File Offset: 0x002B8062
	// (set) Token: 0x060072B0 RID: 29360 RVA: 0x002B9E6A File Offset: 0x002B806A
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

	// Token: 0x17000D4B RID: 3403
	// (get) Token: 0x060072B1 RID: 29361 RVA: 0x002B9E84 File Offset: 0x002B8084
	// (set) Token: 0x060072B2 RID: 29362 RVA: 0x002B9E8C File Offset: 0x002B808C
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

	// Token: 0x17000D4C RID: 3404
	// (get) Token: 0x060072B3 RID: 29363 RVA: 0x002B9EA6 File Offset: 0x002B80A6
	[XuiXmlBinding("thumb_size")]
	public int ThumbSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.RoundToInt(this.ThumbAreaSize / (float)(this.LastPage + 1));
		}
	}

	// Token: 0x17000D4D RID: 3405
	// (get) Token: 0x060072B4 RID: 29364 RVA: 0x002B9EBD File Offset: 0x002B80BD
	[XuiXmlBinding("thumb_position")]
	public int ThumbPosition
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.RoundToInt((float)this.Page / (float)(this.LastPage + 1) * this.ThumbAreaSize);
		}
	}

	// Token: 0x060072B5 RID: 29365 RVA: 0x002B9EDC File Offset: 0x002B80DC
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

	// Token: 0x060072B6 RID: 29366 RVA: 0x002B9FAD File Offset: 0x002B81AD
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDropdown_OnPress(XUiController _sender, int _mouseButton)
	{
		this.DropdownOpen = !this.DropdownOpen;
	}

	// Token: 0x060072B7 RID: 29367 RVA: 0x002B9FBE File Offset: 0x002B81BE
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

	// Token: 0x060072B8 RID: 29368 RVA: 0x002B9FD7 File Offset: 0x002B81D7
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

	// Token: 0x060072B9 RID: 29369 RVA: 0x002B9FE6 File Offset: 0x002B81E6
	[XuiBindEvent("OnSubmitHandler", "TextInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnInputSubmit(XUiController _sender, string _text)
	{
		this.DropdownOpen = false;
		this.TextInput.SetSelected(false, false);
		this.OnInputChanged(_sender, _text, false);
		this.sendSubmitEvent();
	}

	// Token: 0x060072BA RID: 29370 RVA: 0x002BA00B File Offset: 0x002B820B
	[XuiBindEvent("OnChangeHandler", "TextInput")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.UpdateFilteredList();
		base.RefreshBindings();
		this.sendChangedEvent(_changeFromCode);
	}

	// Token: 0x060072BB RID: 29371 RVA: 0x002BA020 File Offset: 0x002B8220
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

	// Token: 0x060072BC RID: 29372 RVA: 0x002BA040 File Offset: 0x002B8240
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

	// Token: 0x060072BD RID: 29373 RVA: 0x002BA05C File Offset: 0x002B825C
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

	// Token: 0x060072BE RID: 29374 RVA: 0x002BA118 File Offset: 0x002B8318
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

	// Token: 0x060072BF RID: 29375 RVA: 0x002BA131 File Offset: 0x002B8331
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePageDownPress(XUiController _sender, int _mouseButton)
	{
		this.handlePageDown();
	}

	// Token: 0x060072C0 RID: 29376 RVA: 0x002BA13A File Offset: 0x002B833A
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePageUpPress(XUiController _sender, int _mouseButton)
	{
		this.handlePageUp();
	}

	// Token: 0x060072C1 RID: 29377 RVA: 0x002BA144 File Offset: 0x002B8344
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

	// Token: 0x060072C2 RID: 29378 RVA: 0x002BA188 File Offset: 0x002B8388
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

	// Token: 0x060072C3 RID: 29379 RVA: 0x002BA1E0 File Offset: 0x002B83E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCurrentPageContents()
	{
		for (int i = 0; i < this.PageLength; i++)
		{
			int num = i + this.PageLength * this.page;
			this.listEntryControllers[i].Text = ((num < this.filteredEntries.Count) ? this.filteredEntries[num] : null);
		}
	}

	// Token: 0x060072C4 RID: 29380 RVA: 0x002BA238 File Offset: 0x002B8438
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

	// Token: 0x060072C5 RID: 29381 RVA: 0x002BA2A6 File Offset: 0x002B84A6
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

	// Token: 0x04005586 RID: 21894
	public readonly List<string> AllEntries = new List<string>();

	// Token: 0x04005587 RID: 21895
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> filteredEntries = new List<string>();

	// Token: 0x04005588 RID: 21896
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DropDown.Entry[] listEntryControllers;

	// Token: 0x04005589 RID: 21897
	[XuiBindComponent("input", false)]
	public readonly XUiC_TextInput TextInput;

	// Token: 0x0400558A RID: 21898
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageDownAction;

	// Token: 0x0400558B RID: 21899
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageUpAction;

	// Token: 0x0400558C RID: 21900
	[PublicizedFrom(EAccessModifier.Private)]
	public float thumbAreaSize;

	// Token: 0x0400558D RID: 21901
	[PublicizedFrom(EAccessModifier.Private)]
	public Color dropdownHoverColor;

	// Token: 0x0400558E RID: 21902
	[PublicizedFrom(EAccessModifier.Private)]
	public Color dropdownTextColor;

	// Token: 0x0400558F RID: 21903
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sortEntries;

	// Token: 0x04005590 RID: 21904
	[PublicizedFrom(EAccessModifier.Private)]
	public bool clearOnOpen;

	// Token: 0x04005591 RID: 21905
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowCustomValue = true;

	// Token: 0x04005592 RID: 21906
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005593 RID: 21907
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dropdownOpen;

	// Token: 0x02000E6D RID: 3693
	[Preserve]
	public class Entry : XUiController
	{
		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x060072C7 RID: 29383 RVA: 0x002BA2FA File Offset: 0x002B84FA
		// (set) Token: 0x060072C8 RID: 29384 RVA: 0x002BA30B File Offset: 0x002B850B
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

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x060072C9 RID: 29385 RVA: 0x002BA32A File Offset: 0x002B852A
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

		// Token: 0x060072CA RID: 29386 RVA: 0x002BA368 File Offset: 0x002B8568
		public override void OnOpen()
		{
			base.OnOpen();
			this.hovered = false;
			this.IsDirty = true;
		}

		// Token: 0x060072CB RID: 29387 RVA: 0x002BA37E File Offset: 0x002B857E
		[XuiBindEvent("OnPress", null)]
		[PublicizedFrom(EAccessModifier.Protected)]
		public void OnPressed(XUiController _sender, int _mouseButton)
		{
			this.Owner.Text = this.text;
			this.Owner.DropdownOpen = false;
			this.Owner.sendChangedEvent(false);
			this.Owner.sendSubmitEvent();
		}

		// Token: 0x060072CC RID: 29388 RVA: 0x002BA3B4 File Offset: 0x002B85B4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnHovered(bool _isOver)
		{
			base.OnHovered(_isOver);
			this.hovered = _isOver;
			this.IsDirty = true;
		}

		// Token: 0x060072CD RID: 29389 RVA: 0x0028AD37 File Offset: 0x00288F37
		public override void Update(float _dt)
		{
			base.Update(_dt);
			base.handleDirtyUpdateDefault();
		}

		// Token: 0x04005594 RID: 21908
		public XUiC_DropDown Owner;

		// Token: 0x04005595 RID: 21909
		[PublicizedFrom(EAccessModifier.Private)]
		public string text;

		// Token: 0x04005596 RID: 21910
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hovered;
	}

	// Token: 0x02000E6E RID: 3694
	// (Invoke) Token: 0x060072D0 RID: 29392
	public delegate void XUiEvent_InputOnChangedEventHandler(XUiController _sender, string _text, bool _validInput, bool _changeFromCode);
}
