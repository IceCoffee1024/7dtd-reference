using System;
using UnityEngine.Scripting;

// Token: 0x02000E7F RID: 3711
[Preserve]
public class XUiC_SimpleButton : XUiController
{
	// Token: 0x140000B4 RID: 180
	// (add) Token: 0x06007385 RID: 29573 RVA: 0x002BBF48 File Offset: 0x002BA148
	// (remove) Token: 0x06007386 RID: 29574 RVA: 0x002BBF80 File Offset: 0x002BA180
	public event XUiEvent_OnPressEventHandler OnPressed;

	// Token: 0x140000B5 RID: 181
	// (add) Token: 0x06007387 RID: 29575 RVA: 0x002BBFB8 File Offset: 0x002BA1B8
	// (remove) Token: 0x06007388 RID: 29576 RVA: 0x002BBFF0 File Offset: 0x002BA1F0
	public new event XUiEvent_OnHoverEventHandler OnHovered;

	// Token: 0x17000D75 RID: 3445
	// (get) Token: 0x06007389 RID: 29577 RVA: 0x002BC025 File Offset: 0x002BA225
	// (set) Token: 0x0600738A RID: 29578 RVA: 0x002BC038 File Offset: 0x002BA238
	public string Text
	{
		get
		{
			XUiV_Label xuiV_Label = this.label;
			if (xuiV_Label == null)
			{
				return null;
			}
			return xuiV_Label.Text;
		}
		set
		{
			if (this.label != null)
			{
				this.label.Text = value;
			}
		}
	}

	// Token: 0x17000D76 RID: 3446
	// (get) Token: 0x0600738B RID: 29579 RVA: 0x002BC04E File Offset: 0x002BA24E
	// (set) Token: 0x0600738C RID: 29580 RVA: 0x002BC05B File Offset: 0x002BA25B
	public string Tooltip
	{
		get
		{
			return this.button.ToolTip;
		}
		set
		{
			if (this.button.ToolTip != value)
			{
				this.button.ToolTip = value;
			}
		}
	}

	// Token: 0x17000D77 RID: 3447
	// (get) Token: 0x0600738D RID: 29581 RVA: 0x002BC07C File Offset: 0x002BA27C
	// (set) Token: 0x0600738E RID: 29582 RVA: 0x002BC089 File Offset: 0x002BA289
	public string DisabledToolTip
	{
		get
		{
			return this.button.DisabledToolTip;
		}
		set
		{
			if (this.button.DisabledToolTip != value)
			{
				this.button.DisabledToolTip = value;
			}
		}
	}

	// Token: 0x17000D78 RID: 3448
	// (get) Token: 0x0600738F RID: 29583 RVA: 0x002BC0AA File Offset: 0x002BA2AA
	public XUiV_Button Button
	{
		get
		{
			return this.button;
		}
	}

	// Token: 0x17000D79 RID: 3449
	// (get) Token: 0x06007390 RID: 29584 RVA: 0x002BC0B2 File Offset: 0x002BA2B2
	// (set) Token: 0x06007391 RID: 29585 RVA: 0x002BC0BC File Offset: 0x002BA2BC
	[XuiXmlAttribute("button_enabled", false)]
	[XuiXmlBinding("enabled")]
	public bool Enabled
	{
		get
		{
			return this.isEnabled;
		}
		set
		{
			if (value == this.isEnabled && (this.button == null || value == this.button.Enabled))
			{
				return;
			}
			this.isEnabled = value;
			if (this.button != null)
			{
				this.button.Enabled = value;
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D7A RID: 3450
	// (get) Token: 0x06007392 RID: 29586 RVA: 0x002BC10B File Offset: 0x002BA30B
	[XuiXmlBinding("selected")]
	public bool ButtonSelected
	{
		get
		{
			XUiV_Button xuiV_Button = this.Button;
			return xuiV_Button != null && xuiV_Button.Selected;
		}
	}

	// Token: 0x17000D7B RID: 3451
	// (get) Token: 0x06007393 RID: 29587 RVA: 0x002BC11E File Offset: 0x002BA31E
	[XuiXmlBinding("hovered")]
	public bool ButtonHovered
	{
		get
		{
			return this.isOver && this.isEnabled;
		}
	}

	// Token: 0x06007394 RID: 29588 RVA: 0x002BC130 File Offset: 0x002BA330
	public override void Init()
	{
		base.Init();
		XUiV_Button xuiV_Button = base.ViewComponent as XUiV_Button;
		if (xuiV_Button != null)
		{
			this.button = xuiV_Button;
		}
		else
		{
			XUiController childById = base.GetChildById("clickable");
			XUiV_Button xuiV_Button2 = ((childById != null) ? childById.ViewComponent : null) as XUiV_Button;
			if (xuiV_Button2 == null)
			{
				throw new Exception(string.Concat(new string[]
				{
					"SimpleButton requires a Button view (itself or as a child named 'clickable'). WindowGroup: '",
					base.WindowGroup.Id,
					"', element: '",
					base.ViewComponent.ID,
					"'"
				}));
			}
			this.button = xuiV_Button2;
		}
		this.button.Controller.OnPress += this.Btn_OnPress;
		this.button.Controller.OnHover += this.Btn_OnHover;
	}

	// Token: 0x06007395 RID: 29589 RVA: 0x002BC201 File Offset: 0x002BA401
	[PublicizedFrom(EAccessModifier.Private)]
	public void Btn_OnHover(XUiController _sender, bool _isOver)
	{
		this.isOver = _isOver;
		this.IsDirty = true;
		XUiEvent_OnHoverEventHandler onHovered = this.OnHovered;
		if (onHovered == null)
		{
			return;
		}
		onHovered(this, _isOver);
	}

	// Token: 0x06007396 RID: 29590 RVA: 0x002BC223 File Offset: 0x002BA423
	[PublicizedFrom(EAccessModifier.Private)]
	public void Btn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (!this.isEnabled)
		{
			return;
		}
		XUiEvent_OnPressEventHandler onPressed = this.OnPressed;
		if (onPressed != null)
		{
			onPressed(this, _mouseButton);
		}
		this.IsDirty = true;
	}

	// Token: 0x06007397 RID: 29591 RVA: 0x0008A091 File Offset: 0x00088291
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x040055D7 RID: 21975
	[XuiBindComponent("btnLabel", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x040055D8 RID: 21976
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button button;

	// Token: 0x040055D9 RID: 21977
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEnabled = true;

	// Token: 0x040055DA RID: 21978
	public string Tag;

	// Token: 0x040055DB RID: 21979
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;
}
