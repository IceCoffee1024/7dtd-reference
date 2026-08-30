using System;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x02000FDA RID: 4058
[Preserve]
public class XUiC_QuestTrackerObjectiveEntry : XUiController
{
	// Token: 0x17000ECD RID: 3789
	// (get) Token: 0x06007FEA RID: 32746 RVA: 0x003084F5 File Offset: 0x003066F5
	// (set) Token: 0x06007FEB RID: 32747 RVA: 0x003084FD File Offset: 0x003066FD
	public XUiC_QuestTrackerObjectiveList Owner { get; set; }

	// Token: 0x17000ECE RID: 3790
	// (get) Token: 0x06007FEC RID: 32748 RVA: 0x00308506 File Offset: 0x00306706
	// (set) Token: 0x06007FED RID: 32749 RVA: 0x00308510 File Offset: 0x00306710
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

	// Token: 0x17000ECF RID: 3791
	// (get) Token: 0x06007FEE RID: 32750 RVA: 0x00308595 File Offset: 0x00306795
	// (set) Token: 0x06007FEF RID: 32751 RVA: 0x003085A0 File Offset: 0x003067A0
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

	// Token: 0x06007FF0 RID: 32752 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Objective_ValueChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x17000ED0 RID: 3792
	// (get) Token: 0x06007FF1 RID: 32753 RVA: 0x0030860D File Offset: 0x0030680D
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

	// Token: 0x06007FF2 RID: 32754 RVA: 0x00308638 File Offset: 0x00306838
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

	// Token: 0x06007FF3 RID: 32755 RVA: 0x00308B1E File Offset: 0x00306D1E
	[PublicizedFrom(EAccessModifier.Internal)]
	public void ClearObjective()
	{
		this.QuestObjective = null;
		this.ChallengeObjective = null;
	}

	// Token: 0x06007FF4 RID: 32756 RVA: 0x00308B2E File Offset: 0x00306D2E
	public override void Update(float _dt)
	{
		if (this.questObjective != null && this.questObjective.UpdateUI)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x04005EB9 RID: 24249
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseObjective questObjective;

	// Token: 0x04005EBA RID: 24250
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseChallengeObjective challengeObjective;

	// Token: 0x04005EBB RID: 24251
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBool;

	// Token: 0x04005EBC RID: 24252
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005EBE RID: 24254
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");
}
