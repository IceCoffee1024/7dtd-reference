using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001071 RID: 4209
[Preserve]
public class XUiC_ToolTip : XUiController
{
	// Token: 0x17000F72 RID: 3954
	// (get) Token: 0x06008597 RID: 34199 RVA: 0x0032F9CA File Offset: 0x0032DBCA
	// (set) Token: 0x06008598 RID: 34200 RVA: 0x0032F9D4 File Offset: 0x0032DBD4
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

	// Token: 0x06008599 RID: 34201 RVA: 0x0032FA92 File Offset: 0x0032DC92
	public override void Init()
	{
		base.Init();
		this.xui.ToolTipWindow = this;
		this.border = (XUiV_Sprite)base.GetChildById("sprBackgroundBorder").ViewComponent;
	}

	// Token: 0x0600859A RID: 34202 RVA: 0x0032FAC1 File Offset: 0x0032DCC1
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.ToolTipWindow = null;
	}

	// Token: 0x0600859B RID: 34203 RVA: 0x0032FAD8 File Offset: 0x0032DCD8
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

	// Token: 0x0600859C RID: 34204 RVA: 0x0032FBDC File Offset: 0x0032DDDC
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

	// Token: 0x0600859D RID: 34205 RVA: 0x0032FC78 File Offset: 0x0032DE78
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

	// Token: 0x0400632E RID: 25390
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite border;

	// Token: 0x0400632F RID: 25391
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltip = "";

	// Token: 0x04006330 RID: 25392
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltipOld = "";

	// Token: 0x04006331 RID: 25393
	public float ShowDelaySec = 0.3f;

	// Token: 0x04006332 RID: 25394
	public Vector2i PositionOffset = new Vector2i(0, -36);

	// Token: 0x04006333 RID: 25395
	[PublicizedFrom(EAccessModifier.Private)]
	public float showDelay;

	// Token: 0x04006334 RID: 25396
	[PublicizedFrom(EAccessModifier.Private)]
	public bool show;
}
