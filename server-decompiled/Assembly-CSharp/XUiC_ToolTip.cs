using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010AB RID: 4267
[Preserve]
public class XUiC_ToolTip : XUiController
{
	// Token: 0x17000FEB RID: 4075
	// (get) Token: 0x0600872B RID: 34603 RVA: 0x0033A08D File Offset: 0x0033828D
	// (set) Token: 0x0600872C RID: 34604 RVA: 0x0033A098 File Offset: 0x00338298
	public string ToolTip
	{
		get
		{
			return this.tooltip;
		}
		set
		{
			if (this.tooltip == value)
			{
				return;
			}
			this.tooltipOld = this.tooltip;
			if (value == null)
			{
				this.tooltip = "";
			}
			else if (value.Length > 0 && value[value.Length - 1] == '\n')
			{
				this.tooltip = value.Substring(0, value.Length - 1);
			}
			else
			{
				this.tooltip = value;
			}
			if (this.tooltip == "")
			{
				this.show = false;
				return;
			}
			base.ViewComponent.Position = this.xui.GetMouseXUiPosition() + this.PositionOffset;
			this.showDelay = Time.unscaledTime + this.ShowDelaySec;
		}
	}

	// Token: 0x0600872D RID: 34605 RVA: 0x0033A156 File Offset: 0x00338356
	public override void Init()
	{
		base.Init();
		this.xui.ToolTipWindow = this;
		this.border = (XUiV_Sprite)base.GetChildById("sprBackgroundBorder").ViewComponent;
	}

	// Token: 0x0600872E RID: 34606 RVA: 0x0033A185 File Offset: 0x00338385
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.ToolTipWindow = null;
	}

	// Token: 0x0600872F RID: 34607 RVA: 0x0033A19C File Offset: 0x0033839C
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		if (this.tooltip != "")
		{
			if (Time.unscaledTime > this.showDelay)
			{
				this.show = true;
			}
			Vector2i vector2i = this.xui.GetXUiScreenSize() / 2;
			if ((base.ViewComponent.Position + this.border.Size).x > vector2i.x)
			{
				base.ViewComponent.Position -= new Vector2i(this.border.Size.x, 0);
			}
			if ((base.ViewComponent.Position - this.border.Size).y < -vector2i.y)
			{
				base.ViewComponent.Position += new Vector2i(20, 20 + this.border.Size.y);
			}
		}
		base.Update(_dt);
	}

	// Token: 0x06008730 RID: 34608 RVA: 0x0033A2A0 File Offset: 0x003384A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "screen_width")
		{
			_value = this.xui.GetXUiScreenSize().x.ToString();
			return true;
		}
		if (_bindingName == "text")
		{
			_value = ((this.tooltip.Length > 0) ? this.tooltip : this.tooltipOld);
			return true;
		}
		if (!(_bindingName == "show"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = (this.show && GameManager.Instance.isAnyCursorWindowOpen(null)).ToString();
		return true;
	}

	// Token: 0x06008731 RID: 34609 RVA: 0x0033A33C File Offset: 0x0033853C
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "show_delay")
		{
			this.ShowDelaySec = (float)StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		if (!(_name == "position_offset"))
		{
			return base.ParseAttribute(_name, _value);
		}
		this.PositionOffset = StringParsers.ParseVector2i(_value, ',');
		return true;
	}

	// Token: 0x04006476 RID: 25718
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite border;

	// Token: 0x04006477 RID: 25719
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltip = "";

	// Token: 0x04006478 RID: 25720
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltipOld = "";

	// Token: 0x04006479 RID: 25721
	public float ShowDelaySec = 0.3f;

	// Token: 0x0400647A RID: 25722
	public Vector2i PositionOffset = new Vector2i(0, -36);

	// Token: 0x0400647B RID: 25723
	[PublicizedFrom(EAccessModifier.Private)]
	public float showDelay;

	// Token: 0x0400647C RID: 25724
	[PublicizedFrom(EAccessModifier.Private)]
	public bool show;
}
