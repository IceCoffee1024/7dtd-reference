using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FCA RID: 4042
[Preserve]
public class XUiC_QuestObjectiveEntry : XUiController
{
	// Token: 0x17000EB6 RID: 3766
	// (get) Token: 0x06007F6A RID: 32618 RVA: 0x003052F8 File Offset: 0x003034F8
	// (set) Token: 0x06007F6B RID: 32619 RVA: 0x00305300 File Offset: 0x00303500
	public XUiC_QuestObjectiveList Owner { get; set; }

	// Token: 0x17000EB7 RID: 3767
	// (get) Token: 0x06007F6C RID: 32620 RVA: 0x00305309 File Offset: 0x00303509
	// (set) Token: 0x06007F6D RID: 32621 RVA: 0x00305314 File Offset: 0x00303514
	public BaseObjective Objective
	{
		get
		{
			return this.objective;
		}
		set
		{
			if (this.objective != null)
			{
				this.objective.ValueChanged -= this.Objective_ValueChanged;
			}
			this.objective = value;
			if (this.objective != null)
			{
				this.objective.ValueChanged += this.Objective_ValueChanged;
			}
			this.IsDirty = true;
			base.RefreshBindings();
		}
	}

	// Token: 0x06007F6E RID: 32622 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Objective_ValueChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x06007F6F RID: 32623 RVA: 0x00305373 File Offset: 0x00303573
	public void SetIsTracker()
	{
		this.isTracker = true;
		this.completeColor = Utils.ColorToHex(StringParsers.ParseColor(this.completeColor));
		this.incompleteColor = Utils.ColorToHex(StringParsers.ParseColor(this.incompleteColor));
	}

	// Token: 0x17000EB8 RID: 3768
	// (get) Token: 0x06007F70 RID: 32624 RVA: 0x003053B2 File Offset: 0x003035B2
	public static string OptionalKeyword
	{
		get
		{
			if (XUiC_QuestObjectiveEntry.optionalKeyword == "")
			{
				XUiC_QuestObjectiveEntry.optionalKeyword = Localization.Get("optional", false, null);
			}
			return XUiC_QuestObjectiveEntry.optionalKeyword;
		}
	}

	// Token: 0x06007F71 RID: 32625 RVA: 0x003053DC File Offset: 0x003035DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.objective != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 3046379438U)
		{
			if (num <= 1556015422U)
			{
				if (num != 612643325U)
				{
					if (num == 1556015422U)
					{
						if (bindingName == "hasobjective")
						{
							value = flag.ToString();
							return true;
						}
					}
				}
				else if (bindingName == "objectivestate")
				{
					value = (flag ? this.objective.StatusText : "");
					return true;
				}
			}
			else if (num != 1883252318U)
			{
				if (num == 3046379438U)
				{
					if (bindingName == "objectivecompletecolor")
					{
						if (this.objective != null)
						{
							if (this.objective.OwnerQuest.CurrentState == Quest.QuestState.Completed)
							{
								value = this.completeColor;
								return true;
							}
							switch (this.objective.ObjectiveState)
							{
							case BaseObjective.ObjectiveStates.NotStarted:
								value = this.originalColor;
								break;
							case BaseObjective.ObjectiveStates.InProgress:
								value = this.incompleteColor;
								break;
							case BaseObjective.ObjectiveStates.Warning:
								value = this.warningColor;
								break;
							case BaseObjective.ObjectiveStates.Complete:
								value = this.completeColor;
								break;
							case BaseObjective.ObjectiveStates.Failed:
								value = this.incompleteColor;
								break;
							}
						}
						else
						{
							value = this.completeColor;
						}
						return true;
					}
				}
			}
			else if (bindingName == "objectivephasecolor")
			{
				if (this.objective != null)
				{
					if (this.objective.OwnerQuest.CurrentState == Quest.QuestState.NotStarted)
					{
						value = this.originalColor;
					}
					else
					{
						value = (((this.objective.Phase == 0 || this.objective.Phase == this.objective.OwnerQuest.CurrentPhase) && this.objective.OwnerQuest.CurrentState == Quest.QuestState.InProgress) ? this.originalColor : this.inactiveColor);
					}
				}
				else
				{
					value = this.originalColor;
				}
				return true;
			}
		}
		else if (num <= 3186090687U)
		{
			if (num != 3091161906U)
			{
				if (num == 3186090687U)
				{
					if (bindingName == "objectivephasehexcolor")
					{
						if (this.objective != null)
						{
							if (this.objective.OwnerQuest.CurrentState == Quest.QuestState.NotStarted)
							{
								value = this.Owner.activeHexColor;
							}
							else
							{
								value = ((this.objective.Phase == this.objective.OwnerQuest.CurrentPhase) ? this.Owner.activeHexColor : this.Owner.inactiveHexColor);
							}
						}
						else
						{
							value = "FFFFFF";
						}
						return true;
					}
				}
			}
			else if (bindingName == "objectivedescription")
			{
				value = (flag ? this.objective.Description : "");
				return true;
			}
		}
		else if (num != 3220755286U)
		{
			if (num == 3422073972U)
			{
				if (bindingName == "objectiveoptional")
				{
					value = (flag ? (this.objective.Optional ? this.objectiveOptionalFormatter.Format(XUiC_QuestObjectiveEntry.OptionalKeyword) : "") : "");
					return true;
				}
			}
		}
		else if (bindingName == "objectivecompletesprite")
		{
			if (this.objective != null)
			{
				Quest ownerQuest = this.objective.OwnerQuest;
				if (ownerQuest.CurrentState == Quest.QuestState.Completed)
				{
					value = this.completeIconName;
					return true;
				}
				if (ownerQuest.CurrentState == Quest.QuestState.NotStarted || this.objective.ObjectiveState == BaseObjective.ObjectiveStates.NotStarted)
				{
					value = this.notstartedIconName;
				}
				else
				{
					value = (this.objective.Complete ? this.completeIconName : this.incompleteIconName);
				}
			}
			else
			{
				value = "";
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06007F72 RID: 32626 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007F73 RID: 32627 RVA: 0x00305770 File Offset: 0x00303970
	public override void Update(float _dt)
	{
		if (this.objective != null && this.objective.OwnerQuest.CurrentState == Quest.QuestState.InProgress && this.objective.UpdateUI && Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 0.1f;
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x06007F74 RID: 32628 RVA: 0x003057D8 File Offset: 0x003039D8
	public override bool ParseAttribute(string name, string value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
		if (num <= 2036604981U)
		{
			if (num <= 466249841U)
			{
				if (num != 215154731U)
				{
					if (num == 466249841U)
					{
						if (name == "incomplete_color")
						{
							this.incompleteColor = value;
							return true;
						}
					}
				}
				else if (name == "incomplete_icon")
				{
					this.incompleteIconName = value;
					return true;
				}
			}
			else if (num != 1516116007U)
			{
				if (num == 2036604981U)
				{
					if (name == "notstarted_icon")
					{
						this.notstartedIconName = value;
						return true;
					}
				}
			}
			else if (name == "warning_color")
			{
				this.warningColor = value;
				return true;
			}
		}
		else if (num <= 3169435800U)
		{
			if (num != 2911778486U)
			{
				if (num == 3169435800U)
				{
					if (name == "inactive_color")
					{
						this.inactiveColor = value;
						return true;
					}
				}
			}
			else if (name == "complete_color")
			{
				this.completeColor = value;
				return true;
			}
		}
		else if (num != 3495370150U)
		{
			if (num == 4270887654U)
			{
				if (name == "complete_icon")
				{
					this.completeIconName = value;
					return true;
				}
			}
		}
		else if (name == "original_color")
		{
			this.originalColor = value;
			return true;
		}
		return base.ParseAttribute(name, value);
	}

	// Token: 0x04005E47 RID: 24135
	[PublicizedFrom(EAccessModifier.Private)]
	public string notstartedIconName = "";

	// Token: 0x04005E48 RID: 24136
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeIconName = "";

	// Token: 0x04005E49 RID: 24137
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteIconName = "";

	// Token: 0x04005E4A RID: 24138
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeColor = "0,255,0,255";

	// Token: 0x04005E4B RID: 24139
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteColor = "255,0,0,255";

	// Token: 0x04005E4C RID: 24140
	[PublicizedFrom(EAccessModifier.Private)]
	public string warningColor = "255,255,0,255";

	// Token: 0x04005E4D RID: 24141
	[PublicizedFrom(EAccessModifier.Private)]
	public string inactiveColor = "160,160,160,255";

	// Token: 0x04005E4E RID: 24142
	[PublicizedFrom(EAccessModifier.Private)]
	public string originalColor = "255,255,255,255";

	// Token: 0x04005E4F RID: 24143
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseObjective objective;

	// Token: 0x04005E50 RID: 24144
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracker;

	// Token: 0x04005E51 RID: 24145
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005E53 RID: 24147
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");

	// Token: 0x04005E54 RID: 24148
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
