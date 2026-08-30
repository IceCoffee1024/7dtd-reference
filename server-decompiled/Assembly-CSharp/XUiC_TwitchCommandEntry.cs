using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02001098 RID: 4248
[Preserve]
public class XUiC_TwitchCommandEntry : XUiController
{
	// Token: 0x17000FC9 RID: 4041
	// (get) Token: 0x06008737 RID: 34615 RVA: 0x00338998 File Offset: 0x00336B98
	// (set) Token: 0x06008738 RID: 34616 RVA: 0x003389A0 File Offset: 0x00336BA0
	public XUiC_TwitchWindow Owner { get; set; }

	// Token: 0x17000FCA RID: 4042
	// (get) Token: 0x06008739 RID: 34617 RVA: 0x003389A9 File Offset: 0x00336BA9
	// (set) Token: 0x0600873A RID: 34618 RVA: 0x003389B1 File Offset: 0x00336BB1
	public TwitchAction Action
	{
		get
		{
			return this.action;
		}
		set
		{
			this.action = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x0600873B RID: 34619 RVA: 0x003389C4 File Offset: 0x00336BC4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.action != null;
		if (bindingName == "hascommand")
		{
			value = flag.ToString();
			return true;
		}
		if (bindingName == "commandname")
		{
			value = (flag ? this.action.Command : "");
			return true;
		}
		if (bindingName == "commandcost")
		{
			if (flag)
			{
				if (this.isReady)
				{
					if (this.Owner != null)
					{
						switch (this.action.PointType)
						{
						case TwitchAction.PointTypes.PP:
							value = string.Format("{0} {1}", this.action.CurrentCost, this.Owner.lblPointsPP);
							break;
						case TwitchAction.PointTypes.SP:
							value = string.Format("{0} {1}", this.action.CurrentCost, this.Owner.lblPointsSP);
							break;
						case TwitchAction.PointTypes.Bits:
							value = "* ";
							break;
						}
					}
					else
					{
						value = "";
					}
				}
				else
				{
					value = "--";
				}
			}
			else
			{
				value = "";
			}
			return true;
		}
		if (bindingName == "commandcolor")
		{
			if (flag)
			{
				if (this.isReady)
				{
					if (this.action.IsPositive)
					{
						value = this.positiveColor;
					}
					else
					{
						value = this.negativeColor;
					}
				}
				else
				{
					value = this.disabledColor;
				}
			}
			return true;
		}
		if (bindingName == "costcolor")
		{
			if (flag)
			{
				if (this.isReady)
				{
					switch (this.action.PointType)
					{
					case TwitchAction.PointTypes.PP:
						value = this.defaultCostColor;
						break;
					case TwitchAction.PointTypes.SP:
						value = this.specialCostColor;
						break;
					case TwitchAction.PointTypes.Bits:
						value = this.bitCostColor;
						break;
					}
				}
				else
				{
					value = this.disabledColor;
				}
			}
			return true;
		}
		if (!(bindingName == "commandtextwidth"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		if (this.action != null && this.isBool)
		{
			value = "150";
		}
		else
		{
			value = "150";
		}
		return true;
	}

	// Token: 0x0600873C RID: 34620 RVA: 0x00338BC0 File Offset: 0x00336DC0
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "positive_color")
		{
			this.positiveColor = value;
			return true;
		}
		if (name == "negative_color")
		{
			this.negativeColor = value;
			return true;
		}
		if (name == "disabled_color")
		{
			this.disabledColor = value;
			return true;
		}
		if (name == "default_cost_color")
		{
			this.defaultCostColor = value;
			return true;
		}
		if (!(name == "special_cost_color"))
		{
			return base.ParseAttribute(name, value);
		}
		this.specialCostColor = value;
		return true;
	}

	// Token: 0x0600873D RID: 34621 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x0600873E RID: 34622 RVA: 0x00338C48 File Offset: 0x00336E48
	public override void Update(float _dt)
	{
		if (this.Action != null)
		{
			this.IsDirty = true;
			this.isReady = this.action.IsReady(this.Owner.twitchManager);
		}
		else
		{
			this.isReady = false;
		}
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x04006477 RID: 25719
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchAction action;

	// Token: 0x04006478 RID: 25720
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBool;

	// Token: 0x04006479 RID: 25721
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x0400647A RID: 25722
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x0400647B RID: 25723
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor = "80,80,80";

	// Token: 0x0400647C RID: 25724
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultCostColor = "255,255,255,255";

	// Token: 0x0400647D RID: 25725
	[PublicizedFrom(EAccessModifier.Private)]
	public string specialCostColor = "0,125,125,255";

	// Token: 0x0400647E RID: 25726
	[PublicizedFrom(EAccessModifier.Private)]
	public string bitCostColor = "145,70,255,255";

	// Token: 0x04006480 RID: 25728
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReady;

	// Token: 0x04006481 RID: 25729
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");
}
