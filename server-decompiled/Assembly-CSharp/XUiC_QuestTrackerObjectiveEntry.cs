using System;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x02001012 RID: 4114
[Preserve]
public class XUiC_QuestTrackerObjectiveEntry : XUiController
{
	// Token: 0x17000F38 RID: 3896
	// (get) Token: 0x0600813A RID: 33082 RVA: 0x0031182D File Offset: 0x0030FA2D
	// (set) Token: 0x0600813B RID: 33083 RVA: 0x00311835 File Offset: 0x0030FA35
	public XUiC_QuestTrackerObjectiveList Owner { get; set; }

	// Token: 0x17000F39 RID: 3897
	// (get) Token: 0x0600813C RID: 33084 RVA: 0x0031183E File Offset: 0x0030FA3E
	// (set) Token: 0x0600813D RID: 33085 RVA: 0x00311848 File Offset: 0x0030FA48
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

	// Token: 0x17000F3A RID: 3898
	// (get) Token: 0x0600813E RID: 33086 RVA: 0x003118CD File Offset: 0x0030FACD
	// (set) Token: 0x0600813F RID: 33087 RVA: 0x003118D8 File Offset: 0x0030FAD8
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

	// Token: 0x06008140 RID: 33088 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Objective_ValueChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x17000F3B RID: 3899
	// (get) Token: 0x06008141 RID: 33089 RVA: 0x00311945 File Offset: 0x0030FB45
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

	// Token: 0x06008142 RID: 33090 RVA: 0x00311970 File Offset: 0x0030FB70
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

	// Token: 0x06008143 RID: 33091 RVA: 0x00311E56 File Offset: 0x00310056
	[PublicizedFrom(EAccessModifier.Internal)]
	public void ClearObjective()
	{
		this.QuestObjective = null;
		this.ChallengeObjective = null;
	}

	// Token: 0x06008144 RID: 33092 RVA: 0x00311E66 File Offset: 0x00310066
	public override void Update(float _dt)
	{
		if (this.questObjective != null && this.questObjective.UpdateUI)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x04005FE4 RID: 24548
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseObjective questObjective;

	// Token: 0x04005FE5 RID: 24549
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseChallengeObjective challengeObjective;

	// Token: 0x04005FE6 RID: 24550
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBool;

	// Token: 0x04005FE7 RID: 24551
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005FE9 RID: 24553
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> objectiveOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");
}
