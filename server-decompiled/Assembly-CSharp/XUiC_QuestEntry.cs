using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FDD RID: 4061
[Preserve]
public class XUiC_QuestEntry : XUiController
{
	// Token: 0x17000EE8 RID: 3816
	// (get) Token: 0x06008021 RID: 32801 RVA: 0x003084F5 File Offset: 0x003066F5
	// (set) Token: 0x06008022 RID: 32802 RVA: 0x00308500 File Offset: 0x00306700
	public Quest Quest
	{
		get
		{
			return this.quest;
		}
		set
		{
			this.quest = value;
			this.questClass = ((value != null) ? QuestClass.GetQuest(this.quest.ID) : null);
			this.IsDirty = true;
			base.ViewComponent.Enabled = value != null;
			this.viewComponent.IsNavigatable = (base.ViewComponent.IsSnappable = value != null);
		}
	}

	// Token: 0x17000EE9 RID: 3817
	// (get) Token: 0x06008023 RID: 32803 RVA: 0x00308563 File Offset: 0x00306763
	// (set) Token: 0x06008024 RID: 32804 RVA: 0x0030856B File Offset: 0x0030676B
	public XUiC_QuestWindowGroup QuestUIHandler { get; set; }

	// Token: 0x17000EEA RID: 3818
	// (get) Token: 0x06008025 RID: 32805 RVA: 0x00308574 File Offset: 0x00306774
	// (set) Token: 0x06008026 RID: 32806 RVA: 0x0030857C File Offset: 0x0030677C
	public bool Tracked { get; set; }

	// Token: 0x17000EEB RID: 3819
	// (get) Token: 0x06008027 RID: 32807 RVA: 0x00308585 File Offset: 0x00306785
	// (set) Token: 0x06008028 RID: 32808 RVA: 0x0030858D File Offset: 0x0030678D
	public SharedQuestEntry SharedQuestEntry { get; set; }

	// Token: 0x06008029 RID: 32809 RVA: 0x00308598 File Offset: 0x00306798
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.quest != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 783488098U)
		{
			if (num <= 112224632U)
			{
				if (num != 33281100U)
				{
					if (num == 112224632U)
					{
						if (bindingName == "questicon")
						{
							value = "";
							if (flag)
							{
								if (this.quest.CurrentState == Quest.QuestState.Failed)
								{
									value = this.failedIcon;
								}
								else if (this.quest.CurrentState == Quest.QuestState.Completed)
								{
									value = this.completeIcon;
								}
								else if (this.quest.CurrentState == Quest.QuestState.InProgress || this.quest.CurrentState == Quest.QuestState.ReadyForTurnIn)
								{
									if (this.quest.CurrentPhase == this.questClass.HighestPhase && this.questClass.CompletionType == QuestClass.CompletionTypes.TurnIn)
									{
										value = this.finishedIcon;
									}
									else if (this.quest.QuestClass.AddsToTierComplete && !this.xui.playerUI.entityPlayer.QuestJournal.CanAddProgression)
									{
										value = this.questlimitedIcon;
									}
									else if (this.quest.SharedOwnerID == -1)
									{
										value = this.questClass.Icon;
									}
									else
									{
										value = this.sharedIcon;
									}
								}
								else if (!this.quest.QuestClass.AddsToTierComplete || this.xui.playerUI.entityPlayer.QuestJournal.CanAddProgression)
								{
									value = this.questClass.Icon;
								}
								else
								{
									value = this.questlimitedIcon;
								}
							}
							return true;
						}
					}
				}
				else if (bindingName == "istracking")
				{
					value = (flag ? this.quest.Tracked.ToString() : "false");
					return true;
				}
			}
			else if (num != 765459171U)
			{
				if (num == 783488098U)
				{
					if (bindingName == "distance")
					{
						if (flag && (this.quest.Active || this.SharedQuestEntry != null) && this.quest.HasPosition)
						{
							Vector3 position = this.quest.Position;
							Vector3 position2 = this.xui.playerUI.entityPlayer.GetPosition();
							position.y = 0f;
							position2.y = 0f;
							float num2 = (position - position2).magnitude;
							float num3 = num2;
							string text = "m";
							if (num2 >= 1000f)
							{
								num2 /= 1000f;
								text = "km";
							}
							if (this.quest.MapObject is MapObjectTreasureChest)
							{
								float num4 = (float)(this.quest.MapObject as MapObjectTreasureChest).DefaultRadius;
								if (num3 < num4)
								{
									num4 = EffectManager.GetValue(PassiveEffects.TreasureRadius, null, num4, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
									num4 = Mathf.Clamp(num4, 0f, 13f);
									if (num3 < num4)
									{
										value = this.zerodistanceFormatter.Format(text);
									}
								}
								else
								{
									value = this.distanceFormatter.Format(num2, text);
								}
							}
							else
							{
								value = this.distanceFormatter.Format(num2, text);
							}
						}
						else
						{
							value = "";
						}
						return true;
					}
				}
			}
			else if (bindingName == "rowstatecolor")
			{
				value = (this.Selected ? "255,255,255,255" : (this.IsHovered ? this.hoverColor : this.rowColor));
				return true;
			}
		}
		else if (num <= 2730462270U)
		{
			if (num != 1656712805U)
			{
				if (num == 2730462270U)
				{
					if (bindingName == "questname")
					{
						value = (flag ? this.questClass.Name : "");
						return true;
					}
				}
			}
			else if (bindingName == "rowstatesprite")
			{
				value = (this.Selected ? "ui_game_select_row" : "menu_empty");
				return true;
			}
		}
		else if (num != 3106195591U)
		{
			if (num == 3644377122U)
			{
				if (bindingName == "textstatecolor")
				{
					value = "255,255,255,255";
					if (flag)
					{
						Quest.QuestState currentState = this.quest.CurrentState;
						if (currentState <= Quest.QuestState.ReadyForTurnIn)
						{
							value = this.enabledColor;
						}
						else
						{
							value = this.disabledColor;
						}
					}
					return true;
				}
			}
		}
		else if (bindingName == "iconcolor")
		{
			value = "255,255,255,255";
			if (flag)
			{
				switch (this.quest.CurrentState)
				{
				case Quest.QuestState.NotStarted:
				case Quest.QuestState.InProgress:
				case Quest.QuestState.ReadyForTurnIn:
					if (this.quest.CurrentPhase == this.questClass.HighestPhase && this.questClass.CompletionType == QuestClass.CompletionTypes.TurnIn)
					{
						value = this.finishedColor;
					}
					else if (this.quest.QuestClass.AddsToTierComplete && !this.xui.playerUI.entityPlayer.QuestJournal.CanAddProgression)
					{
						value = this.failedColor;
					}
					else if (this.quest.SharedOwnerID == -1)
					{
						value = this.enabledColor;
					}
					else
					{
						value = this.sharedColor;
					}
					break;
				case Quest.QuestState.Completed:
					value = this.completeColor;
					break;
				case Quest.QuestState.Failed:
					value = this.failedColor;
					break;
				}
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600802A RID: 32810 RVA: 0x002DD73C File Offset: 0x002DB93C
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x0600802B RID: 32811 RVA: 0x00308AF9 File Offset: 0x00306CF9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		base.OnHovered(_isOver);
		if (this.Quest == null)
		{
			this.IsHovered = false;
			return;
		}
		if (this.IsHovered != _isOver)
		{
			this.IsHovered = _isOver;
			base.RefreshBindings();
		}
	}

	// Token: 0x0600802C RID: 32812 RVA: 0x002C422D File Offset: 0x002C242D
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x0600802D RID: 32813 RVA: 0x0008A715 File Offset: 0x00088915
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x0600802E RID: 32814 RVA: 0x00308B28 File Offset: 0x00306D28
	public override bool ParseAttribute(string name, string value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
		if (num <= 2218008692U)
		{
			if (num <= 1285243605U)
			{
				if (num != 185729116U)
				{
					if (num != 1048851580U)
					{
						if (num == 1285243605U)
						{
							if (name == "finished_icon")
							{
								this.finishedIcon = value;
								return true;
							}
						}
					}
					else if (name == "failed_icon")
					{
						this.failedIcon = value;
						return true;
					}
				}
				else if (name == "shared_icon")
				{
					this.sharedIcon = value;
					return true;
				}
			}
			else if (num != 1553164900U)
			{
				if (num != 1627114004U)
				{
					if (num == 2218008692U)
					{
						if (name == "shared_color")
						{
							this.sharedColor = value;
							return true;
						}
					}
				}
				else if (name == "failed_color")
				{
					this.failedColor = value;
					return true;
				}
			}
			else if (name == "quest_limited_icon")
			{
				this.questlimitedIcon = value;
				return true;
			}
		}
		else if (num <= 3152862043U)
		{
			if (num != 2531019123U)
			{
				if (num != 2911778486U)
				{
					if (num == 3152862043U)
					{
						if (name == "finished_color")
						{
							this.finishedColor = value;
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
			else if (name == "row_color")
			{
				this.rowColor = value;
				return true;
			}
		}
		else if (num <= 3868148786U)
		{
			if (num != 3387915097U)
			{
				if (num == 3868148786U)
				{
					if (name == "enabled_color")
					{
						this.enabledColor = value;
						return true;
					}
				}
			}
			else if (name == "hover_color")
			{
				this.hoverColor = value;
				return true;
			}
		}
		else if (num != 4076031121U)
		{
			if (num == 4270887654U)
			{
				if (name == "complete_icon")
				{
					this.completeIcon = value;
					return true;
				}
			}
		}
		else if (name == "disabled_color")
		{
			this.disabledColor = value;
			return true;
		}
		return base.ParseAttribute(name, value);
	}

	// Token: 0x04005ED8 RID: 24280
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04005ED9 RID: 24281
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04005EDA RID: 24282
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedColor;

	// Token: 0x04005EDB RID: 24283
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeColor;

	// Token: 0x04005EDC RID: 24284
	[PublicizedFrom(EAccessModifier.Private)]
	public string finishedColor;

	// Token: 0x04005EDD RID: 24285
	[PublicizedFrom(EAccessModifier.Private)]
	public string sharedColor;

	// Token: 0x04005EDE RID: 24286
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedIcon;

	// Token: 0x04005EDF RID: 24287
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeIcon;

	// Token: 0x04005EE0 RID: 24288
	[PublicizedFrom(EAccessModifier.Private)]
	public string finishedIcon;

	// Token: 0x04005EE1 RID: 24289
	[PublicizedFrom(EAccessModifier.Private)]
	public string sharedIcon;

	// Token: 0x04005EE2 RID: 24290
	[PublicizedFrom(EAccessModifier.Private)]
	public string questlimitedIcon = "ui_game_symbol_quest_limited";

	// Token: 0x04005EE3 RID: 24291
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04005EE4 RID: 24292
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04005EE5 RID: 24293
	public new bool Selected;

	// Token: 0x04005EE6 RID: 24294
	public bool IsHovered;

	// Token: 0x04005EE7 RID: 24295
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005EE8 RID: 24296
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005EEC RID: 24300
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float, string> distanceFormatter = new CachedStringFormatter<float, string>((float _f, string _s) => _f.ToCultureInvariantString("0.0") + " " + _s);

	// Token: 0x04005EED RID: 24301
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> zerodistanceFormatter = new CachedStringFormatter<string>((string _s) => "0 " + _s);
}
