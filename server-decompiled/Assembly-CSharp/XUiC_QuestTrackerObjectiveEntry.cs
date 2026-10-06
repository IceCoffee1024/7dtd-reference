using System;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x02001002 RID: 4098
[Preserve]
public class XUiC_QuestTrackerObjectiveEntry : XUiController
{
	// Token: 0x17000F2D RID: 3885
	// (get) Token: 0x060080EB RID: 33003 RVA: 0x0030FD11 File Offset: 0x0030DF11
	// (set) Token: 0x060080EC RID: 33004 RVA: 0x0030FD19 File Offset: 0x0030DF19
	public XUiC_QuestTrackerObjectiveList Owner { get; set; }

	// Token: 0x17000F2E RID: 3886
	// (get) Token: 0x060080ED RID: 33005 RVA: 0x0030FD22 File Offset: 0x0030DF22
	// (set) Token: 0x060080EE RID: 33006 RVA: 0x0030FD2C File Offset: 0x0030DF2C
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

	// Token: 0x17000F2F RID: 3887
	// (get) Token: 0x060080EF RID: 33007 RVA: 0x0030FDB1 File Offset: 0x0030DFB1
	// (set) Token: 0x060080F0 RID: 33008 RVA: 0x0030FDBC File Offset: 0x0030DFBC
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

	// Token: 0x060080F1 RID: 33009 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Objective_ValueChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x17000F30 RID: 3888
	// (get) Token: 0x060080F2 RID: 33010 RVA: 0x0030FE29 File Offset: 0x0030E029
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

	// Token: 0x060080F3 RID: 33011 RVA: 0x0030FE54 File Offset: 0x0030E054
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

	// Token: 0x060080F4 RID: 33012 RVA: 0x0031033A File Offset: 0x0030E53A
	[PublicizedFrom(EAccessModifier.Internal)]
	public void ClearObjective()
	{
		this.QuestObjective = null;
		this.ChallengeObjective = null;
	}

	// Token: 0x060080F5 RID: 33013 RVA: 0x0031034A File Offset: 0x0030E54A
	public override void Update(float _dt)
	{
		if (this.questObjective != null && this.questObjective.UpdateUI)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x04005FA9 RID: 24489
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseObjective questObjective;

	// Token: 0x04005FAA RID: 24490
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseChallengeObjective challengeObjective;

	// Token: 0x04005FAB RID: 24491
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBool;

	// Token: 0x04005FAC RID: 24492
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005FAE RID: 24494
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");
}
