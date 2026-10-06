using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F2D RID: 3885
[Preserve]
public class XUiC_DewCollectorStack : XUiC_RequiredItemStack
{
	// Token: 0x17000E4D RID: 3661
	// (get) Token: 0x06007965 RID: 31077 RVA: 0x002DF8A4 File Offset: 0x002DDAA4
	// (set) Token: 0x06007966 RID: 31078 RVA: 0x002DF8AC File Offset: 0x002DDAAC
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

	// Token: 0x17000E4E RID: 3662
	// (get) Token: 0x06007967 RID: 31079 RVA: 0x002DF8D1 File Offset: 0x002DDAD1
	// (set) Token: 0x06007968 RID: 31080 RVA: 0x002DF8D9 File Offset: 0x002DDAD9
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

	// Token: 0x06007969 RID: 31081 RVA: 0x002DF8EC File Offset: 0x002DDAEC
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("fillIcon");
		if (childById != null)
		{
			this.fillIcon = childById.ViewComponent as XUiV_Sprite;
		}
	}

	// Token: 0x0600796A RID: 31082 RVA: 0x002DF920 File Offset: 0x002DDB20
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

	// Token: 0x0600796B RID: 31083 RVA: 0x002DF9CC File Offset: 0x002DDBCC
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

	// Token: 0x0600796C RID: 31084 RVA: 0x002DFADE File Offset: 0x002DDCDE
	public void SetStandardFillColor(string colorString)
	{
		this.standardFillColorString = colorString;
		this.standardFillColor = StringParsers.ParseColor32(this.standardFillColorString);
	}

	// Token: 0x0600796D RID: 31085 RVA: 0x002DFAF8 File Offset: 0x002DDCF8
	public void SetModdedFillColor(string colorString)
	{
		this.moddedFillColorString = colorString;
		this.moddedFillColor = StringParsers.ParseColor32(this.moddedFillColorString);
	}

	// Token: 0x0600796E RID: 31086 RVA: 0x002DFB14 File Offset: 0x002DDD14
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

	// Token: 0x0600796F RID: 31087 RVA: 0x002DFB82 File Offset: 0x002DDD82
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

	// Token: 0x06007970 RID: 31088 RVA: 0x002DFBA0 File Offset: 0x002DDDA0
	public void SetSprite(string sprite)
	{
		this.fillIcon.SpriteName = sprite;
	}

	// Token: 0x040059F5 RID: 23029
	public bool IsCurrentStack;

	// Token: 0x040059F6 RID: 23030
	public bool IsBlocked;

	// Token: 0x040059F7 RID: 23031
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isModded;

	// Token: 0x040059F8 RID: 23032
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite fillIcon;

	// Token: 0x040059F9 RID: 23033
	[PublicizedFrom(EAccessModifier.Protected)]
	public TileEntityCollector tileEntity;

	// Token: 0x040059FA RID: 23034
	[PublicizedFrom(EAccessModifier.Private)]
	public string standardFillColorString = "202,190,33,255";

	// Token: 0x040059FB RID: 23035
	[PublicizedFrom(EAccessModifier.Private)]
	public Color standardFillColor = new Color32(202, 190, 33, byte.MaxValue);

	// Token: 0x040059FC RID: 23036
	[PublicizedFrom(EAccessModifier.Private)]
	public string moddedFillColorString = "0,173,216,255";

	// Token: 0x040059FD RID: 23037
	[PublicizedFrom(EAccessModifier.Private)]
	public Color moddedFillColor = new Color32(0, 173, 216, byte.MaxValue);

	// Token: 0x040059FE RID: 23038
	[PublicizedFrom(EAccessModifier.Private)]
	public Color disabledColor = new Color32(64, 64, 64, byte.MaxValue);

	// Token: 0x040059FF RID: 23039
	[PublicizedFrom(EAccessModifier.Private)]
	public Color blockedColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	// Token: 0x04005A00 RID: 23040
	[PublicizedFrom(EAccessModifier.Private)]
	public Color currentFillColor;

	// Token: 0x04005A01 RID: 23041
	[PublicizedFrom(EAccessModifier.Private)]
	public float fillAmount = -1f;

	// Token: 0x04005A02 RID: 23042
	public float MaxFill = 15f;
}
