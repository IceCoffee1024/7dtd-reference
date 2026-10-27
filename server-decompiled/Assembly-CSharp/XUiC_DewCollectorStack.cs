using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F3C RID: 3900
[Preserve]
public class XUiC_DewCollectorStack : XUiC_RequiredItemStack
{
	// Token: 0x17000E57 RID: 3671
	// (get) Token: 0x060079A8 RID: 31144 RVA: 0x002E1194 File Offset: 0x002DF394
	// (set) Token: 0x060079A9 RID: 31145 RVA: 0x002E119C File Offset: 0x002DF39C
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

	// Token: 0x17000E58 RID: 3672
	// (get) Token: 0x060079AA RID: 31146 RVA: 0x002E11C1 File Offset: 0x002DF3C1
	// (set) Token: 0x060079AB RID: 31147 RVA: 0x002E11C9 File Offset: 0x002DF3C9
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

	// Token: 0x060079AC RID: 31148 RVA: 0x002E11DC File Offset: 0x002DF3DC
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("fillIcon");
		if (childById != null)
		{
			this.fillIcon = childById.ViewComponent as XUiV_Sprite;
		}
	}

	// Token: 0x060079AD RID: 31149 RVA: 0x002E1210 File Offset: 0x002DF410
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

	// Token: 0x060079AE RID: 31150 RVA: 0x002E12BC File Offset: 0x002DF4BC
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

	// Token: 0x060079AF RID: 31151 RVA: 0x002E13CE File Offset: 0x002DF5CE
	public void SetStandardFillColor(string colorString)
	{
		this.standardFillColorString = colorString;
		this.standardFillColor = StringParsers.ParseColor32(this.standardFillColorString);
	}

	// Token: 0x060079B0 RID: 31152 RVA: 0x002E13E8 File Offset: 0x002DF5E8
	public void SetModdedFillColor(string colorString)
	{
		this.moddedFillColorString = colorString;
		this.moddedFillColor = StringParsers.ParseColor32(this.moddedFillColorString);
	}

	// Token: 0x060079B1 RID: 31153 RVA: 0x002E1404 File Offset: 0x002DF604
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

	// Token: 0x060079B2 RID: 31154 RVA: 0x002E1472 File Offset: 0x002DF672
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

	// Token: 0x060079B3 RID: 31155 RVA: 0x002E1490 File Offset: 0x002DF690
	public void SetSprite(string sprite)
	{
		this.fillIcon.SpriteName = sprite;
	}

	// Token: 0x04005A28 RID: 23080
	public bool IsCurrentStack;

	// Token: 0x04005A29 RID: 23081
	public bool IsBlocked;

	// Token: 0x04005A2A RID: 23082
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isModded;

	// Token: 0x04005A2B RID: 23083
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite fillIcon;

	// Token: 0x04005A2C RID: 23084
	[PublicizedFrom(EAccessModifier.Protected)]
	public TileEntityCollector tileEntity;

	// Token: 0x04005A2D RID: 23085
	[PublicizedFrom(EAccessModifier.Private)]
	public string standardFillColorString = "202,190,33,255";

	// Token: 0x04005A2E RID: 23086
	[PublicizedFrom(EAccessModifier.Private)]
	public Color standardFillColor = new Color32(202, 190, 33, byte.MaxValue);

	// Token: 0x04005A2F RID: 23087
	[PublicizedFrom(EAccessModifier.Private)]
	public string moddedFillColorString = "0,173,216,255";

	// Token: 0x04005A30 RID: 23088
	[PublicizedFrom(EAccessModifier.Private)]
	public Color moddedFillColor = new Color32(0, 173, 216, byte.MaxValue);

	// Token: 0x04005A31 RID: 23089
	[PublicizedFrom(EAccessModifier.Private)]
	public Color disabledColor = new Color32(64, 64, 64, byte.MaxValue);

	// Token: 0x04005A32 RID: 23090
	[PublicizedFrom(EAccessModifier.Private)]
	public Color blockedColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	// Token: 0x04005A33 RID: 23091
	[PublicizedFrom(EAccessModifier.Private)]
	public Color currentFillColor;

	// Token: 0x04005A34 RID: 23092
	[PublicizedFrom(EAccessModifier.Private)]
	public float fillAmount = -1f;

	// Token: 0x04005A35 RID: 23093
	public float MaxFill = 15f;
}
