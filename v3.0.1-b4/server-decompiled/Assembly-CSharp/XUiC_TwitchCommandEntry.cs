using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200107F RID: 4223
[Preserve]
public class XUiC_TwitchCommandEntry : XUiController
{
	// Token: 0x17000F90 RID: 3984
	// (get) Token: 0x06008645 RID: 34373 RVA: 0x00334170 File Offset: 0x00332370
	// (set) Token: 0x06008646 RID: 34374 RVA: 0x00334178 File Offset: 0x00332378
	public XUiC_TwitchWindow Owner { get; set; }

	// Token: 0x17000F91 RID: 3985
	// (get) Token: 0x06008647 RID: 34375 RVA: 0x00334181 File Offset: 0x00332381
	// (set) Token: 0x06008648 RID: 34376 RVA: 0x00334189 File Offset: 0x00332389
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

	// Token: 0x06008649 RID: 34377 RVA: 0x0033419C File Offset: 0x0033239C
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

	// Token: 0x0600864A RID: 34378 RVA: 0x00334398 File Offset: 0x00332598
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

	// Token: 0x0600864B RID: 34379 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x0600864C RID: 34380 RVA: 0x00334420 File Offset: 0x00332620
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

	// Token: 0x040063B0 RID: 25520
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchAction action;

	// Token: 0x040063B1 RID: 25521
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBool;

	// Token: 0x040063B2 RID: 25522
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x040063B3 RID: 25523
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x040063B4 RID: 25524
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor = "80,80,80";

	// Token: 0x040063B5 RID: 25525
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultCostColor = "255,255,255,255";

	// Token: 0x040063B6 RID: 25526
	[PublicizedFrom(EAccessModifier.Private)]
	public string specialCostColor = "0,125,125,255";

	// Token: 0x040063B7 RID: 25527
	[PublicizedFrom(EAccessModifier.Private)]
	public string bitCostColor = "145,70,255,255";

	// Token: 0x040063B9 RID: 25529
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReady;

	// Token: 0x040063BA RID: 25530
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");
}
