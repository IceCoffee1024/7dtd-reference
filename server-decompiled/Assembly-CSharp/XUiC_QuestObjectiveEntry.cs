using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FE3 RID: 4067
[Preserve]
public class XUiC_QuestObjectiveEntry : XUiController
{
	// Token: 0x17000EEF RID: 3823
	// (get) Token: 0x0600805C RID: 32860 RVA: 0x00309ACC File Offset: 0x00307CCC
	// (set) Token: 0x0600805D RID: 32861 RVA: 0x00309AD4 File Offset: 0x00307CD4
	public XUiC_QuestObjectiveList Owner { get; set; }

	// Token: 0x17000EF0 RID: 3824
	// (get) Token: 0x0600805E RID: 32862 RVA: 0x00309ADD File Offset: 0x00307CDD
	// (set) Token: 0x0600805F RID: 32863 RVA: 0x00309AE8 File Offset: 0x00307CE8
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

	// Token: 0x06008060 RID: 32864 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Objective_ValueChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x06008061 RID: 32865 RVA: 0x00309B47 File Offset: 0x00307D47
	public void SetIsTracker()
	{
		this.isTracker = true;
		this.completeColor = Utils.ColorToHex(StringParsers.ParseColor(this.completeColor));
		this.incompleteColor = Utils.ColorToHex(StringParsers.ParseColor(this.incompleteColor));
	}

	// Token: 0x17000EF1 RID: 3825
	// (get) Token: 0x06008062 RID: 32866 RVA: 0x00309B86 File Offset: 0x00307D86
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

	// Token: 0x06008063 RID: 32867 RVA: 0x00309BB0 File Offset: 0x00307DB0
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

	// Token: 0x06008064 RID: 32868 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06008065 RID: 32869 RVA: 0x00309F44 File Offset: 0x00308144
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

	// Token: 0x06008066 RID: 32870 RVA: 0x00309FAC File Offset: 0x003081AC
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

	// Token: 0x04005F0E RID: 24334
	[PublicizedFrom(EAccessModifier.Private)]
	public string notstartedIconName = "";

	// Token: 0x04005F0F RID: 24335
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeIconName = "";

	// Token: 0x04005F10 RID: 24336
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteIconName = "";

	// Token: 0x04005F11 RID: 24337
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeColor = "0,255,0,255";

	// Token: 0x04005F12 RID: 24338
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteColor = "255,0,0,255";

	// Token: 0x04005F13 RID: 24339
	[PublicizedFrom(EAccessModifier.Private)]
	public string warningColor = "255,255,0,255";

	// Token: 0x04005F14 RID: 24340
	[PublicizedFrom(EAccessModifier.Private)]
	public string inactiveColor = "160,160,160,255";

	// Token: 0x04005F15 RID: 24341
	[PublicizedFrom(EAccessModifier.Private)]
	public string originalColor = "255,255,255,255";

	// Token: 0x04005F16 RID: 24342
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseObjective objective;

	// Token: 0x04005F17 RID: 24343
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracker;

	// Token: 0x04005F18 RID: 24344
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005F1A RID: 24346
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");

	// Token: 0x04005F1B RID: 24347
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
