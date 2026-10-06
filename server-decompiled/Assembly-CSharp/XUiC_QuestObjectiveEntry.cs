using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FF2 RID: 4082
[Preserve]
public class XUiC_QuestObjectiveEntry : XUiController
{
	// Token: 0x17000F16 RID: 3862
	// (get) Token: 0x0600806B RID: 32875 RVA: 0x0030CAA4 File Offset: 0x0030ACA4
	// (set) Token: 0x0600806C RID: 32876 RVA: 0x0030CAAC File Offset: 0x0030ACAC
	public XUiC_QuestObjectiveList Owner { get; set; }

	// Token: 0x17000F17 RID: 3863
	// (get) Token: 0x0600806D RID: 32877 RVA: 0x0030CAB5 File Offset: 0x0030ACB5
	// (set) Token: 0x0600806E RID: 32878 RVA: 0x0030CAC0 File Offset: 0x0030ACC0
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

	// Token: 0x0600806F RID: 32879 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Objective_ValueChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x06008070 RID: 32880 RVA: 0x0030CB1F File Offset: 0x0030AD1F
	public void SetIsTracker()
	{
		this.isTracker = true;
		this.completeColor = Utils.ColorToHex(StringParsers.ParseColor(this.completeColor));
		this.incompleteColor = Utils.ColorToHex(StringParsers.ParseColor(this.incompleteColor));
	}

	// Token: 0x17000F18 RID: 3864
	// (get) Token: 0x06008071 RID: 32881 RVA: 0x0030CB5E File Offset: 0x0030AD5E
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

	// Token: 0x06008072 RID: 32882 RVA: 0x0030CB88 File Offset: 0x0030AD88
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

	// Token: 0x06008073 RID: 32883 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06008074 RID: 32884 RVA: 0x0030CF1C File Offset: 0x0030B11C
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

	// Token: 0x06008075 RID: 32885 RVA: 0x0030CF84 File Offset: 0x0030B184
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

	// Token: 0x04005F37 RID: 24375
	[PublicizedFrom(EAccessModifier.Private)]
	public string notstartedIconName = "";

	// Token: 0x04005F38 RID: 24376
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeIconName = "";

	// Token: 0x04005F39 RID: 24377
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteIconName = "";

	// Token: 0x04005F3A RID: 24378
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeColor = "0,255,0,255";

	// Token: 0x04005F3B RID: 24379
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteColor = "255,0,0,255";

	// Token: 0x04005F3C RID: 24380
	[PublicizedFrom(EAccessModifier.Private)]
	public string warningColor = "255,255,0,255";

	// Token: 0x04005F3D RID: 24381
	[PublicizedFrom(EAccessModifier.Private)]
	public string inactiveColor = "160,160,160,255";

	// Token: 0x04005F3E RID: 24382
	[PublicizedFrom(EAccessModifier.Private)]
	public string originalColor = "255,255,255,255";

	// Token: 0x04005F3F RID: 24383
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseObjective objective;

	// Token: 0x04005F40 RID: 24384
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracker;

	// Token: 0x04005F41 RID: 24385
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005F43 RID: 24387
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");

	// Token: 0x04005F44 RID: 24388
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
