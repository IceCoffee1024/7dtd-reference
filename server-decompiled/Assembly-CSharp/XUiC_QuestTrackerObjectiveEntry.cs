using System;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x02000FF3 RID: 4083
[Preserve]
public class XUiC_QuestTrackerObjectiveEntry : XUiController
{
	// Token: 0x17000F06 RID: 3846
	// (get) Token: 0x060080DC RID: 32988 RVA: 0x0030CD29 File Offset: 0x0030AF29
	// (set) Token: 0x060080DD RID: 32989 RVA: 0x0030CD31 File Offset: 0x0030AF31
	public XUiC_QuestTrackerObjectiveList Owner { get; set; }

	// Token: 0x17000F07 RID: 3847
	// (get) Token: 0x060080DE RID: 32990 RVA: 0x0030CD3A File Offset: 0x0030AF3A
	// (set) Token: 0x060080DF RID: 32991 RVA: 0x0030CD44 File Offset: 0x0030AF44
	public BaseObjective QuestObjective
	{
		get
		{
			return this.questObjective;
		}
		set
		{
			if (this.questObjective != null)
			{
				this.questObjective.ValueChanged -= this.Objective_ValueChanged;
			}
			this.questObjective = value;
			if (this.questObjective != null)
			{
				this.ChallengeObjective = null;
				this.questObjective.ValueChanged += this.Objective_ValueChanged;
			}
			this.IsDirty = true;
			this.isBool = this.questObjective != null && this.questObjective.ObjectiveValueType == BaseObjective.ObjectiveValueTypes.Boolean;
			base.RefreshBindings();
		}
	}

	// Token: 0x17000F08 RID: 3848
	// (get) Token: 0x060080E0 RID: 32992 RVA: 0x0030CDC9 File Offset: 0x0030AFC9
	// (set) Token: 0x060080E1 RID: 32993 RVA: 0x0030CDD4 File Offset: 0x0030AFD4
	public BaseChallengeObjective ChallengeObjective
	{
		get
		{
			return this.challengeObjective;
		}
		set
		{
			if (this.challengeObjective != null)
			{
				this.challengeObjective.ValueChanged -= this.Objective_ValueChanged;
			}
			this.challengeObjective = value;
			if (this.challengeObjective != null)
			{
				this.QuestObjective = null;
				this.challengeObjective.ValueChanged += this.Objective_ValueChanged;
			}
			this.IsDirty = true;
			this.isBool = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x060080E2 RID: 32994 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Objective_ValueChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x17000F09 RID: 3849
	// (get) Token: 0x060080E3 RID: 32995 RVA: 0x0030CE41 File Offset: 0x0030B041
	public static string OptionalKeyword
	{
		get
		{
			if (XUiC_QuestTrackerObjectiveEntry.optionalKeyword == "")
			{
				XUiC_QuestTrackerObjectiveEntry.optionalKeyword = Localization.Get("optional", false, null);
			}
			return XUiC_QuestTrackerObjectiveEntry.optionalKeyword;
		}
	}

	// Token: 0x060080E4 RID: 32996 RVA: 0x0030CE6C File Offset: 0x0030B06C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.questObjective != null;
		bool flag2 = this.challengeObjective != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2750710735U)
		{
			if (num <= 612643325U)
			{
				if (num != 195861468U)
				{
					if (num == 612643325U)
					{
						if (bindingName == "objectivestate")
						{
							if (flag)
							{
								if (this.isBool)
								{
									value = "";
								}
								else
								{
									value = this.questObjective.StatusText;
								}
							}
							else if (flag2)
							{
								value = this.challengeObjective.StatusText;
							}
							else
							{
								value = "";
							}
							return true;
						}
					}
				}
				else if (bindingName == "objectiveshowicon")
				{
					value = (flag ? this.isBool.ToString() : "false");
					return true;
				}
			}
			else if (num != 1160085873U)
			{
				if (num != 1556015422U)
				{
					if (num == 2750710735U)
					{
						if (bindingName == "objectivecompletehexcolor")
						{
							if (flag && this.questObjective.OwnerQuest != null && this.questObjective.OwnerQuest.Active)
							{
								value = (this.questObjective.Complete ? this.Owner.completeHexColor : this.Owner.incompleteHexColor);
								switch (this.questObjective.ObjectiveState)
								{
								case BaseObjective.ObjectiveStates.InProgress:
									value = this.Owner.incompleteHexColor;
									break;
								case BaseObjective.ObjectiveStates.Warning:
									value = this.Owner.warningHexColor;
									break;
								case BaseObjective.ObjectiveStates.Complete:
									value = this.Owner.completeHexColor;
									break;
								case BaseObjective.ObjectiveStates.Failed:
									value = this.Owner.incompleteHexColor;
									break;
								}
							}
							else if (flag2)
							{
								value = (this.challengeObjective.Complete ? this.Owner.completeHexColor : this.Owner.incompleteHexColor);
							}
							else
							{
								value = "FFFFFF";
							}
							return true;
						}
					}
				}
				else if (bindingName == "hasobjective")
				{
					value = (flag || flag2).ToString();
					return true;
				}
			}
			else if (bindingName == "objectivetextwidth")
			{
				if (flag && this.isBool)
				{
					value = "280";
				}
				else
				{
					value = "300";
				}
				return true;
			}
		}
		else if (num <= 3091161906U)
		{
			if (num != 3046379438U)
			{
				if (num == 3091161906U)
				{
					if (bindingName == "objectivedescription")
					{
						if (flag)
						{
							value = this.questObjective.Description;
						}
						else if (flag2)
						{
							value = this.challengeObjective.DescriptionText;
						}
						else
						{
							value = "";
						}
						return true;
					}
				}
			}
			else if (bindingName == "objectivecompletecolor")
			{
				value = "255,255,255,255";
				if (flag && this.questObjective.OwnerQuest != null && this.questObjective.OwnerQuest.Active)
				{
					switch (this.questObjective.ObjectiveState)
					{
					case BaseObjective.ObjectiveStates.InProgress:
						value = this.Owner.incompleteColor;
						break;
					case BaseObjective.ObjectiveStates.Warning:
						value = this.Owner.warningColor;
						break;
					case BaseObjective.ObjectiveStates.Complete:
						value = this.Owner.completeColor;
						break;
					case BaseObjective.ObjectiveStates.Failed:
						value = this.Owner.incompleteColor;
						break;
					}
				}
				else if (flag2)
				{
					value = (this.challengeObjective.Complete ? this.Owner.completeColor : this.Owner.incompleteColor);
				}
				return true;
			}
		}
		else if (num != 3186090687U)
		{
			if (num != 3220755286U)
			{
				if (num == 3422073972U)
				{
					if (bindingName == "objectiveoptional")
					{
						value = (flag ? (this.questObjective.Optional ? this.objectiveOptionalFormatter.Format(XUiC_QuestTrackerObjectiveEntry.OptionalKeyword) : "") : "");
						return true;
					}
				}
			}
			else if (bindingName == "objectivecompletesprite")
			{
				if (flag && this.questObjective.OwnerQuest != null && this.questObjective.OwnerQuest.Active && this.isBool)
				{
					value = (this.questObjective.Complete ? this.Owner.completeIconName : this.Owner.incompleteIconName);
				}
				else
				{
					value = "";
				}
				return true;
			}
		}
		else if (bindingName == "objectivephasehexcolor")
		{
			if (flag && this.questObjective.OwnerQuest != null && this.questObjective.OwnerQuest.Active)
			{
				value = ((this.questObjective.Phase == this.questObjective.OwnerQuest.CurrentPhase || this.questObjective.Phase == 0) ? this.Owner.activeHexColor : this.Owner.inactiveHexColor);
			}
			else
			{
				value = "FFFFFF";
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060080E5 RID: 32997 RVA: 0x0030D352 File Offset: 0x0030B552
	[PublicizedFrom(EAccessModifier.Internal)]
	public void ClearObjective()
	{
		this.QuestObjective = null;
		this.ChallengeObjective = null;
	}

	// Token: 0x060080E6 RID: 32998 RVA: 0x0030D362 File Offset: 0x0030B562
	public override void Update(float _dt)
	{
		if (this.questObjective != null && this.questObjective.UpdateUI)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x04005F80 RID: 24448
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseObjective questObjective;

	// Token: 0x04005F81 RID: 24449
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseChallengeObjective challengeObjective;

	// Token: 0x04005F82 RID: 24450
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBool;

	// Token: 0x04005F83 RID: 24451
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005F85 RID: 24453
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");
}
