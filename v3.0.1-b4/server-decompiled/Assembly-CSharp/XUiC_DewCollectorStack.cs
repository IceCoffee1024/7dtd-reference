using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F07 RID: 3847
[Preserve]
public class XUiC_DewCollectorStack : XUiC_RequiredItemStack
{
	// Token: 0x17000E16 RID: 3606
	// (get) Token: 0x060078B2 RID: 30898 RVA: 0x002D857C File Offset: 0x002D677C
	// (set) Token: 0x060078B3 RID: 30899 RVA: 0x002D8584 File Offset: 0x002D6784
	public bool IsModded
	{
		get
		{
			return this.isModded;
		}
		set
		{
			this.isModded = value;
			this.currentFillColor = (this.isModded ? this.moddedFillColor : this.standardFillColor);
		}
	}

	// Token: 0x17000E17 RID: 3607
	// (get) Token: 0x060078B4 RID: 30900 RVA: 0x002D85A9 File Offset: 0x002D67A9
	// (set) Token: 0x060078B5 RID: 30901 RVA: 0x002D85B1 File Offset: 0x002D67B1
	public float FillAmount
	{
		get
		{
			return this.fillAmount;
		}
		set
		{
			if (this.fillAmount != value)
			{
				this.fillAmount = value;
			}
		}
	}

	// Token: 0x060078B6 RID: 30902 RVA: 0x002D85C4 File Offset: 0x002D67C4
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("fillIcon");
		if (childById != null)
		{
			this.fillIcon = childById.ViewComponent as XUiV_Sprite;
		}
	}

	// Token: 0x060078B7 RID: 30903 RVA: 0x002D85F8 File Offset: 0x002D67F8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsBlocked)
		{
			this.fillIcon.Color = this.blockedColor;
			return;
		}
		if (this.IsCurrentStack)
		{
			float num = Mathf.PingPong(Time.time, 0.5f);
			this.fillIcon.Color = Color.Lerp(Color.grey, this.currentFillColor, num * 4f);
		}
		else if (this.fillIcon.Color != this.disabledColor)
		{
			this.fillIcon.Color = this.disabledColor;
		}
		base.ViewComponent.IsNavigatable = !base.ItemStack.IsEmpty();
	}

	// Token: 0x060078B8 RID: 30904 RVA: 0x002D86A4 File Offset: 0x002D68A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "hasfill")
		{
			_value = (this.IsCurrentStack ? (this.FillAmount != -1f).ToString() : "false");
			return true;
		}
		if (_bindingName == "waterfill")
		{
			_value = (this.IsCurrentStack ? (this.FillAmount / this.MaxFill).ToString() : "0");
			return true;
		}
		if (_bindingName == "showitem")
		{
			_value = (!this.itemStack.IsEmpty()).ToString();
			return true;
		}
		if (_bindingName == "fillcolor")
		{
			if (this.IsBlocked)
			{
				_value = "255,0,0,255";
			}
			else if (this.isModded)
			{
				_value = this.moddedFillColorString;
			}
			else
			{
				_value = this.standardFillColorString;
			}
			return true;
		}
		if (!(_bindingName == "showempty"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.itemStack.IsEmpty().ToString();
		return true;
	}

	// Token: 0x060078B9 RID: 30905 RVA: 0x002D87B6 File Offset: 0x002D69B6
	public void SetStandardFillColor(string colorString)
	{
		this.standardFillColorString = colorString;
		this.standardFillColor = StringParsers.ParseColor32(this.standardFillColorString);
	}

	// Token: 0x060078BA RID: 30906 RVA: 0x002D87D0 File Offset: 0x002D69D0
	public void SetModdedFillColor(string colorString)
	{
		this.moddedFillColorString = colorString;
		this.moddedFillColor = StringParsers.ParseColor32(this.moddedFillColorString);
	}

	// Token: 0x060078BB RID: 30907 RVA: 0x002D87EC File Offset: 0x002D69EC
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "standard_fill_color"))
			{
				if (!(name == "modded_fill_color"))
				{
					return false;
				}
				this.moddedFillColorString = value;
				this.moddedFillColor = StringParsers.ParseColor32(this.moddedFillColorString);
			}
			else
			{
				this.standardFillColorString = value;
				this.standardFillColor = StringParsers.ParseColor32(this.standardFillColorString);
			}
			return true;
		}
		return flag;
	}

	// Token: 0x060078BC RID: 30908 RVA: 0x002D885A File Offset: 0x002D6A5A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		if (this.itemStack.IsEmpty())
		{
			base.OnHovered(false);
			return;
		}
		base.OnHovered(_isOver);
	}

	// Token: 0x060078BD RID: 30909 RVA: 0x002D8878 File Offset: 0x002D6A78
	public void SetSprite(string sprite)
	{
		this.fillIcon.SpriteName = sprite;
	}

	// Token: 0x0400590B RID: 22795
	public bool IsCurrentStack;

	// Token: 0x0400590C RID: 22796
	public bool IsBlocked;

	// Token: 0x0400590D RID: 22797
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isModded;

	// Token: 0x0400590E RID: 22798
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite fillIcon;

	// Token: 0x0400590F RID: 22799
	[PublicizedFrom(EAccessModifier.Protected)]
	public TileEntityCollector tileEntity;

	// Token: 0x04005910 RID: 22800
	[PublicizedFrom(EAccessModifier.Private)]
	public string standardFillColorString = "202,190,33,255";

	// Token: 0x04005911 RID: 22801
	[PublicizedFrom(EAccessModifier.Private)]
	public Color standardFillColor = new Color32(202, 190, 33, byte.MaxValue);

	// Token: 0x04005912 RID: 22802
	[PublicizedFrom(EAccessModifier.Private)]
	public string moddedFillColorString = "0,173,216,255";

	// Token: 0x04005913 RID: 22803
	[PublicizedFrom(EAccessModifier.Private)]
	public Color moddedFillColor = new Color32(0, 173, 216, byte.MaxValue);

	// Token: 0x04005914 RID: 22804
	[PublicizedFrom(EAccessModifier.Private)]
	public Color disabledColor = new Color32(64, 64, 64, byte.MaxValue);

	// Token: 0x04005915 RID: 22805
	[PublicizedFrom(EAccessModifier.Private)]
	public Color blockedColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	// Token: 0x04005916 RID: 22806
	[PublicizedFrom(EAccessModifier.Private)]
	public Color currentFillColor;

	// Token: 0x04005917 RID: 22807
	[PublicizedFrom(EAccessModifier.Private)]
	public float fillAmount = -1f;

	// Token: 0x04005918 RID: 22808
	public float MaxFill = 15f;
}
