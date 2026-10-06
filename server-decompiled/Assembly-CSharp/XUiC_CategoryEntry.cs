using System;
using UnityEngine.Scripting;

// Token: 0x02000EE2 RID: 3810
[Preserve]
public class XUiC_CategoryEntry : XUiController
{
	// Token: 0x17000E19 RID: 3609
	// (get) Token: 0x060076DF RID: 30431 RVA: 0x002CD500 File Offset: 0x002CB700
	// (set) Token: 0x060076E0 RID: 30432 RVA: 0x002CD508 File Offset: 0x002CB708
	public XUiC_CategoryList CategoryList { get; set; }

	// Token: 0x17000E1A RID: 3610
	// (get) Token: 0x060076E1 RID: 30433 RVA: 0x002CD511 File Offset: 0x002CB711
	// (set) Token: 0x060076E2 RID: 30434 RVA: 0x002CD519 File Offset: 0x002CB719
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

	// Token: 0x17000E1B RID: 3611
	// (get) Token: 0x060076E3 RID: 30435 RVA: 0x002CD529 File Offset: 0x002CB729
	// (set) Token: 0x060076E4 RID: 30436 RVA: 0x002CD531 File Offset: 0x002CB731
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

	// Token: 0x17000E1C RID: 3612
	// (get) Token: 0x060076E5 RID: 30437 RVA: 0x002CD541 File Offset: 0x002CB741
	// (set) Token: 0x060076E6 RID: 30438 RVA: 0x002CD549 File Offset: 0x002CB749
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

	// Token: 0x17000E1D RID: 3613
	// (get) Token: 0x060076E7 RID: 30439 RVA: 0x002CD559 File Offset: 0x002CB759
	// (set) Token: 0x060076E8 RID: 30440 RVA: 0x002CD561 File Offset: 0x002CB761
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

	// Token: 0x060076E9 RID: 30441 RVA: 0x002CD57B File Offset: 0x002CB77B
	public override void Init()
	{
		base.Init();
		this.button = (XUiV_Button)base.ViewComponent;
		base.OnPress += this.XUiC_CategoryEntry_OnPress;
		this.IsDirty = true;
	}

	// Token: 0x060076EA RID: 30442 RVA: 0x002CD5B0 File Offset: 0x002CB7B0
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

	// Token: 0x060076EB RID: 30443 RVA: 0x002CD60F File Offset: 0x002CB80F
	public void PlayButtonClickSound()
	{
		this.button.PlayClickSound();
	}

	// Token: 0x060076EC RID: 30444 RVA: 0x002CD61C File Offset: 0x002CB81C
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

	// Token: 0x060076ED RID: 30445 RVA: 0x002CD653 File Offset: 0x002CB853
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

	// Token: 0x060076EE RID: 30446 RVA: 0x002CD690 File Offset: 0x002CB890
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

	// Token: 0x0400580E RID: 22542
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryName = "";

	// Token: 0x0400580F RID: 22543
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryDisplayName = "";

	// Token: 0x04005810 RID: 22544
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName = "";

	// Token: 0x04005811 RID: 22545
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x04005812 RID: 22546
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button button;
}
