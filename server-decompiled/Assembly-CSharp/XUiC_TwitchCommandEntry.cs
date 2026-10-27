using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010B9 RID: 4281
[Preserve]
public class XUiC_TwitchCommandEntry : XUiController
{
	// Token: 0x17001009 RID: 4105
	// (get) Token: 0x060087D9 RID: 34777 RVA: 0x0033E834 File Offset: 0x0033CA34
	// (set) Token: 0x060087DA RID: 34778 RVA: 0x0033E83C File Offset: 0x0033CA3C
	public XUiC_TwitchWindow Owner { get; set; }

	// Token: 0x1700100A RID: 4106
	// (get) Token: 0x060087DB RID: 34779 RVA: 0x0033E845 File Offset: 0x0033CA45
	// (set) Token: 0x060087DC RID: 34780 RVA: 0x0033E84D File Offset: 0x0033CA4D
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

	// Token: 0x060087DD RID: 34781 RVA: 0x0033E860 File Offset: 0x0033CA60
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

	// Token: 0x060087DE RID: 34782 RVA: 0x0033EA5C File Offset: 0x0033CC5C
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

	// Token: 0x060087DF RID: 34783 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x060087E0 RID: 34784 RVA: 0x0033EAE4 File Offset: 0x0033CCE4
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

	// Token: 0x040064F8 RID: 25848
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchAction action;

	// Token: 0x040064F9 RID: 25849
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBool;

	// Token: 0x040064FA RID: 25850
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x040064FB RID: 25851
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x040064FC RID: 25852
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor = "80,80,80";

	// Token: 0x040064FD RID: 25853
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultCostColor = "255,255,255,255";

	// Token: 0x040064FE RID: 25854
	[PublicizedFrom(EAccessModifier.Private)]
	public string specialCostColor = "0,125,125,255";

	// Token: 0x040064FF RID: 25855
	[PublicizedFrom(EAccessModifier.Private)]
	public string bitCostColor = "145,70,255,255";

	// Token: 0x04006501 RID: 25857
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReady;

	// Token: 0x04006502 RID: 25858
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");
}
