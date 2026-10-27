using System;
using UnityEngine.Scripting;

// Token: 0x02000EED RID: 3821
[Preserve]
public class XUiC_CategoryEntry : XUiController
{
	// Token: 0x17000E1D RID: 3613
	// (get) Token: 0x06007704 RID: 30468 RVA: 0x002CE9BC File Offset: 0x002CCBBC
	// (set) Token: 0x06007705 RID: 30469 RVA: 0x002CE9C4 File Offset: 0x002CCBC4
	public XUiC_CategoryList CategoryList { get; set; }

	// Token: 0x17000E1E RID: 3614
	// (get) Token: 0x06007706 RID: 30470 RVA: 0x002CE9CD File Offset: 0x002CCBCD
	// (set) Token: 0x06007707 RID: 30471 RVA: 0x002CE9D5 File Offset: 0x002CCBD5
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

	// Token: 0x17000E1F RID: 3615
	// (get) Token: 0x06007708 RID: 30472 RVA: 0x002CE9E5 File Offset: 0x002CCBE5
	// (set) Token: 0x06007709 RID: 30473 RVA: 0x002CE9ED File Offset: 0x002CCBED
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

	// Token: 0x17000E20 RID: 3616
	// (get) Token: 0x0600770A RID: 30474 RVA: 0x002CE9FD File Offset: 0x002CCBFD
	// (set) Token: 0x0600770B RID: 30475 RVA: 0x002CEA05 File Offset: 0x002CCC05
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

	// Token: 0x17000E21 RID: 3617
	// (get) Token: 0x0600770C RID: 30476 RVA: 0x002CEA15 File Offset: 0x002CCC15
	// (set) Token: 0x0600770D RID: 30477 RVA: 0x002CEA1D File Offset: 0x002CCC1D
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

	// Token: 0x0600770E RID: 30478 RVA: 0x002CEA37 File Offset: 0x002CCC37
	public override void Init()
	{
		base.Init();
		this.button = (XUiV_Button)base.ViewComponent;
		base.OnPress += this.XUiC_CategoryEntry_OnPress;
		this.IsDirty = true;
	}

	// Token: 0x0600770F RID: 30479 RVA: 0x002CEA6C File Offset: 0x002CCC6C
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

	// Token: 0x06007710 RID: 30480 RVA: 0x002CEACB File Offset: 0x002CCCCB
	public void PlayButtonClickSound()
	{
		this.button.PlayClickSound();
	}

	// Token: 0x06007711 RID: 30481 RVA: 0x002CEAD8 File Offset: 0x002CCCD8
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

	// Token: 0x06007712 RID: 30482 RVA: 0x002CEB0F File Offset: 0x002CCD0F
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

	// Token: 0x06007713 RID: 30483 RVA: 0x002CEB4C File Offset: 0x002CCD4C
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

	// Token: 0x04005833 RID: 22579
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryName = "";

	// Token: 0x04005834 RID: 22580
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryDisplayName = "";

	// Token: 0x04005835 RID: 22581
	[PublicizedFrom(EAccessModifier.Private)]
	public string spriteName = "";

	// Token: 0x04005836 RID: 22582
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x04005837 RID: 22583
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button button;
}
