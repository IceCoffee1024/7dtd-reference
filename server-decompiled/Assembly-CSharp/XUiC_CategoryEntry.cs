using System;
using UnityEngine.Scripting;

// Token: 0x02000ED3 RID: 3795
[Preserve]
public class XUiC_CategoryEntry : XUiController
{
	// Token: 0x17000DFC RID: 3580
	// (get) Token: 0x060076E4 RID: 30436 RVA: 0x002CA128 File Offset: 0x002C8328
	// (set) Token: 0x060076E5 RID: 30437 RVA: 0x002CA130 File Offset: 0x002C8330
	public XUiC_CategoryList CategoryList { get; set; }

	// Token: 0x17000DFD RID: 3581
	// (get) Token: 0x060076E6 RID: 30438 RVA: 0x002CA139 File Offset: 0x002C8339
	// (set) Token: 0x060076E7 RID: 30439 RVA: 0x002CA141 File Offset: 0x002C8341
	public string CategoryName
	{
		get
		{
			return this.categoryName;
		}
		set
		{
			this.categoryName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DFE RID: 3582
	// (get) Token: 0x060076E8 RID: 30440 RVA: 0x002CA151 File Offset: 0x002C8351
	// (set) Token: 0x060076E9 RID: 30441 RVA: 0x002CA159 File Offset: 0x002C8359
	public string CategoryDisplayName
	{
		get
		{
			return this.categoryDisplayName;
		}
		set
		{
			this.categoryDisplayName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DFF RID: 3583
	// (get) Token: 0x060076EA RID: 30442 RVA: 0x002CA169 File Offset: 0x002C8369
	// (set) Token: 0x060076EB RID: 30443 RVA: 0x002CA171 File Offset: 0x002C8371
	public string SpriteName
	{
		get
		{
			return this.spriteName;
		}
		set
		{
			this.spriteName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E00 RID: 3584
	// (get) Token: 0x060076EC RID: 30444 RVA: 0x002CA181 File Offset: 0x002C8381
	// (set) Token: 0x060076ED RID: 30445 RVA: 0x002CA189 File Offset: 0x002C8389
	public new bool Selected
	{
		get
		{
			return this.selected;
		}
		set
		{
			this.selected = value;
			this.button.Selected = this.selected;
		}
	}

	// Token: 0x060076EE RID: 30446 RVA: 0x002CA1A3 File Offset: 0x002C83A3
	public override void Init()
	{
		base.Init();
		this.button = (XUiV_Button)base.ViewComponent;
		base.OnPress += this.XUiC_CategoryEntry_OnPress;
		this.IsDirty = true;
	}

	// Token: 0x060076EF RID: 30447 RVA: 0x002CA1D8 File Offset: 0x002C83D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_CategoryEntry_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.spriteName != string.Empty)
		{
			if (this.CategoryList.CurrentCategory == this && this.CategoryList.AllowUnselect)
			{
				this.CategoryList.CurrentCategory = null;
			}
			else
			{
				this.CategoryList.CurrentCategory = this;
			}
			this.CategoryList.HandleCategoryChanged();
		}
	}

	// Token: 0x060076F0 RID: 30448 RVA: 0x002CA237 File Offset: 0x002C8437
	public void PlayButtonClickSound()
	{
		this.button.PlayClickSound();
	}

	// Token: 0x060076F1 RID: 30449 RVA: 0x002CA244 File Offset: 0x002C8444
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			base.ViewComponent.IsNavigatable = !string.IsNullOrEmpty(this.SpriteName);
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x060076F2 RID: 30450 RVA: 0x002CA27B File Offset: 0x002C847B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "categoryicon")
		{
			value = this.spriteName;
			return true;
		}
		if (!(bindingName == "categorydisplayname"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = this.categoryDisplayName;
		return true;
	}

	// Token: 0x060076F3 RID: 30451 RVA: 0x002CA2B8 File Offset: 0x002C84B8
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "categoryname")
		{
			if (!string.IsNullOrEmpty(_value))
			{
				this.CategoryName = _value;
			}
			return true;
		}
		if (_name == "spritename")
		{
			if (!string.IsNullOrEmpty(_value))
			{
				this.SpriteName = _value;
			}
			return true;
		}
		if (_name == "displayname")
		{
			if (!string.IsNullOrEmpty(_value))
			{
				this.CategoryDisplayName = _value;
			}
			return true;
		}
		if (!(_name == "displayname_key"))
		{
			return base.ParseAttribute(_name, _value);
		}
		if (!string.IsNullOrEmpty(_value))
		{
			this.CategoryDisplayName = Localization.Get(_value, false, null);
		}
		return true;
	}

	// Token: 0x040057D6 RID: 22486
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryName = "";

	// Token: 0x040057D7 RID: 22487
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryDisplayName = "";

	// Token: 0x040057D8 RID: 22488
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName = "";

	// Token: 0x040057D9 RID: 22489
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x040057DA RID: 22490
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button button;
}
