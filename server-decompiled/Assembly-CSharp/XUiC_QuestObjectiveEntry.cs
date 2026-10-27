using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001002 RID: 4098
[Preserve]
public class XUiC_QuestObjectiveEntry : XUiController
{
	// Token: 0x17000F21 RID: 3873
	// (get) Token: 0x060080BA RID: 32954 RVA: 0x0030E5C0 File Offset: 0x0030C7C0
	// (set) Token: 0x060080BB RID: 32955 RVA: 0x0030E5C8 File Offset: 0x0030C7C8
	public XUiC_QuestObjectiveList Owner { get; set; }

	// Token: 0x17000F22 RID: 3874
	// (get) Token: 0x060080BC RID: 32956 RVA: 0x0030E5D1 File Offset: 0x0030C7D1
	// (set) Token: 0x060080BD RID: 32957 RVA: 0x0030E5DC File Offset: 0x0030C7DC
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

	// Token: 0x060080BE RID: 32958 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Objective_ValueChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x060080BF RID: 32959 RVA: 0x0030E63B File Offset: 0x0030C83B
	public void SetIsTracker()
	{
		this.isTracker = true;
		this.completeColor = Utils.ColorToHex(StringParsers.ParseColor(this.completeColor));
		this.incompleteColor = Utils.ColorToHex(StringParsers.ParseColor(this.incompleteColor));
	}

	// Token: 0x17000F23 RID: 3875
	// (get) Token: 0x060080C0 RID: 32960 RVA: 0x0030E67A File Offset: 0x0030C87A
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

	// Token: 0x060080C1 RID: 32961 RVA: 0x0030E6A4 File Offset: 0x0030C8A4
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

	// Token: 0x060080C2 RID: 32962 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x060080C3 RID: 32963 RVA: 0x0030EA38 File Offset: 0x0030CC38
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

	// Token: 0x060080C4 RID: 32964 RVA: 0x0030EAA0 File Offset: 0x0030CCA0
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

	// Token: 0x04005F72 RID: 24434
	[PublicizedFrom(EAccessModifier.Private)]
	public string notstartedIconName = "";

	// Token: 0x04005F73 RID: 24435
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeIconName = "";

	// Token: 0x04005F74 RID: 24436
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteIconName = "";

	// Token: 0x04005F75 RID: 24437
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeColor = "0,255,0,255";

	// Token: 0x04005F76 RID: 24438
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteColor = "255,0,0,255";

	// Token: 0x04005F77 RID: 24439
	[PublicizedFrom(EAccessModifier.Private)]
	public string warningColor = "255,255,0,255";

	// Token: 0x04005F78 RID: 24440
	[PublicizedFrom(EAccessModifier.Private)]
	public string inactiveColor = "160,160,160,255";

	// Token: 0x04005F79 RID: 24441
	[PublicizedFrom(EAccessModifier.Private)]
	public string originalColor = "255,255,255,255";

	// Token: 0x04005F7A RID: 24442
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseObjective objective;

	// Token: 0x04005F7B RID: 24443
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracker;

	// Token: 0x04005F7C RID: 24444
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005F7E RID: 24446
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");

	// Token: 0x04005F7F RID: 24447
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
