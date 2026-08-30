using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F1E RID: 3870
[Preserve]
public class XUiC_DewCollectorStack : XUiC_RequiredItemStack
{
	// Token: 0x17000E30 RID: 3632
	// (get) Token: 0x0600796A RID: 31082 RVA: 0x002DC508 File Offset: 0x002DA708
	// (set) Token: 0x0600796B RID: 31083 RVA: 0x002DC510 File Offset: 0x002DA710
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

	// Token: 0x17000E31 RID: 3633
	// (get) Token: 0x0600796C RID: 31084 RVA: 0x002DC535 File Offset: 0x002DA735
	// (set) Token: 0x0600796D RID: 31085 RVA: 0x002DC53D File Offset: 0x002DA73D
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

	// Token: 0x0600796E RID: 31086 RVA: 0x002DC550 File Offset: 0x002DA750
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("fillIcon");
		if (childById != null)
		{
			this.fillIcon = childById.ViewComponent as XUiV_Sprite;
		}
	}

	// Token: 0x0600796F RID: 31087 RVA: 0x002DC584 File Offset: 0x002DA784
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

	// Token: 0x06007970 RID: 31088 RVA: 0x002DC630 File Offset: 0x002DA830
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

	// Token: 0x06007971 RID: 31089 RVA: 0x002DC742 File Offset: 0x002DA942
	public void SetStandardFillColor(string colorString)
	{
		this.standardFillColorString = colorString;
		this.standardFillColor = StringParsers.ParseColor32(this.standardFillColorString);
	}

	// Token: 0x06007972 RID: 31090 RVA: 0x002DC75C File Offset: 0x002DA95C
	public void SetModdedFillColor(string colorString)
	{
		this.moddedFillColorString = colorString;
		this.moddedFillColor = StringParsers.ParseColor32(this.moddedFillColorString);
	}

	// Token: 0x06007973 RID: 31091 RVA: 0x002DC778 File Offset: 0x002DA978
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

	// Token: 0x06007974 RID: 31092 RVA: 0x002DC7E6 File Offset: 0x002DA9E6
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

	// Token: 0x06007975 RID: 31093 RVA: 0x002DC804 File Offset: 0x002DAA04
	public void SetSprite(string sprite)
	{
		this.fillIcon.SpriteName = sprite;
	}

	// Token: 0x040059BD RID: 22973
	public bool IsCurrentStack;

	// Token: 0x040059BE RID: 22974
	public bool IsBlocked;

	// Token: 0x040059BF RID: 22975
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isModded;

	// Token: 0x040059C0 RID: 22976
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite fillIcon;

	// Token: 0x040059C1 RID: 22977
	[PublicizedFrom(EAccessModifier.Protected)]
	public TileEntityCollector tileEntity;

	// Token: 0x040059C2 RID: 22978
	[PublicizedFrom(EAccessModifier.Private)]
	public string standardFillColorString = "202,190,33,255";

	// Token: 0x040059C3 RID: 22979
	[PublicizedFrom(EAccessModifier.Private)]
	public Color standardFillColor = new Color32(202, 190, 33, byte.MaxValue);

	// Token: 0x040059C4 RID: 22980
	[PublicizedFrom(EAccessModifier.Private)]
	public string moddedFillColorString = "0,173,216,255";

	// Token: 0x040059C5 RID: 22981
	[PublicizedFrom(EAccessModifier.Private)]
	public Color moddedFillColor = new Color32(0, 173, 216, byte.MaxValue);

	// Token: 0x040059C6 RID: 22982
	[PublicizedFrom(EAccessModifier.Private)]
	public Color disabledColor = new Color32(64, 64, 64, byte.MaxValue);

	// Token: 0x040059C7 RID: 22983
	[PublicizedFrom(EAccessModifier.Private)]
	public Color blockedColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	// Token: 0x040059C8 RID: 22984
	[PublicizedFrom(EAccessModifier.Private)]
	public Color currentFillColor;

	// Token: 0x040059C9 RID: 22985
	[PublicizedFrom(EAccessModifier.Private)]
	public float fillAmount = -1f;

	// Token: 0x040059CA RID: 22986
	public float MaxFill = 15f;
}
