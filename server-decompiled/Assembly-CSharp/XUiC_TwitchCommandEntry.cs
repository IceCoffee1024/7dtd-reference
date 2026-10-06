using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A9 RID: 4265
[Preserve]
public class XUiC_TwitchCommandEntry : XUiController
{
	// Token: 0x17000FFE RID: 4094
	// (get) Token: 0x06008786 RID: 34694 RVA: 0x0033C7B0 File Offset: 0x0033A9B0
	// (set) Token: 0x06008787 RID: 34695 RVA: 0x0033C7B8 File Offset: 0x0033A9B8
	public XUiC_TwitchWindow Owner { get; set; }

	// Token: 0x17000FFF RID: 4095
	// (get) Token: 0x06008788 RID: 34696 RVA: 0x0033C7C1 File Offset: 0x0033A9C1
	// (set) Token: 0x06008789 RID: 34697 RVA: 0x0033C7C9 File Offset: 0x0033A9C9
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

	// Token: 0x0600878A RID: 34698 RVA: 0x0033C7DC File Offset: 0x0033A9DC
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

	// Token: 0x0600878B RID: 34699 RVA: 0x0033C9D8 File Offset: 0x0033ABD8
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

	// Token: 0x0600878C RID: 34700 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x0600878D RID: 34701 RVA: 0x0033CA60 File Offset: 0x0033AC60
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

	// Token: 0x040064B9 RID: 25785
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchAction action;

	// Token: 0x040064BA RID: 25786
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBool;

	// Token: 0x040064BB RID: 25787
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x040064BC RID: 25788
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x040064BD RID: 25789
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor = "80,80,80";

	// Token: 0x040064BE RID: 25790
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultCostColor = "255,255,255,255";

	// Token: 0x040064BF RID: 25791
	[PublicizedFrom(EAccessModifier.Private)]
	public string specialCostColor = "0,125,125,255";

	// Token: 0x040064C0 RID: 25792
	[PublicizedFrom(EAccessModifier.Private)]
	public string bitCostColor = "145,70,255,255";

	// Token: 0x040064C2 RID: 25794
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReady;

	// Token: 0x040064C3 RID: 25795
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");
}
