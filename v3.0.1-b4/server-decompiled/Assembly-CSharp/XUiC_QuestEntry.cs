using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FC4 RID: 4036
[Preserve]
public class XUiC_QuestEntry : XUiController
{
	// Token: 0x17000EAF RID: 3759
	// (get) Token: 0x06007F2F RID: 32559 RVA: 0x00303D21 File Offset: 0x00301F21
	// (set) Token: 0x06007F30 RID: 32560 RVA: 0x00303D2C File Offset: 0x00301F2C
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

	// Token: 0x17000EB0 RID: 3760
	// (get) Token: 0x06007F31 RID: 32561 RVA: 0x00303D8F File Offset: 0x00301F8F
	// (set) Token: 0x06007F32 RID: 32562 RVA: 0x00303D97 File Offset: 0x00301F97
	public XUiC_QuestWindowGroup QuestUIHandler { get; set; }

	// Token: 0x17000EB1 RID: 3761
	// (get) Token: 0x06007F33 RID: 32563 RVA: 0x00303DA0 File Offset: 0x00301FA0
	// (set) Token: 0x06007F34 RID: 32564 RVA: 0x00303DA8 File Offset: 0x00301FA8
	public bool Tracked { get; set; }

	// Token: 0x17000EB2 RID: 3762
	// (get) Token: 0x06007F35 RID: 32565 RVA: 0x00303DB1 File Offset: 0x00301FB1
	// (set) Token: 0x06007F36 RID: 32566 RVA: 0x00303DB9 File Offset: 0x00301FB9
	public SharedQuestEntry SharedQuestEntry { get; set; }

	// Token: 0x06007F37 RID: 32567 RVA: 0x00303DC4 File Offset: 0x00301FC4
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

	// Token: 0x06007F38 RID: 32568 RVA: 0x002D97B0 File Offset: 0x002D79B0
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06007F39 RID: 32569 RVA: 0x00304325 File Offset: 0x00302525
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

	// Token: 0x06007F3A RID: 32570 RVA: 0x002C0D09 File Offset: 0x002BEF09
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06007F3B RID: 32571 RVA: 0x0008A069 File Offset: 0x00088269
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007F3C RID: 32572 RVA: 0x00304354 File Offset: 0x00302554
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

	// Token: 0x04005E11 RID: 24081
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04005E12 RID: 24082
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04005E13 RID: 24083
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedColor;

	// Token: 0x04005E14 RID: 24084
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeColor;

	// Token: 0x04005E15 RID: 24085
	[PublicizedFrom(EAccessModifier.Private)]
	public string finishedColor;

	// Token: 0x04005E16 RID: 24086
	[PublicizedFrom(EAccessModifier.Private)]
	public string sharedColor;

	// Token: 0x04005E17 RID: 24087
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedIcon;

	// Token: 0x04005E18 RID: 24088
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeIcon;

	// Token: 0x04005E19 RID: 24089
	[PublicizedFrom(EAccessModifier.Private)]
	public string finishedIcon;

	// Token: 0x04005E1A RID: 24090
	[PublicizedFrom(EAccessModifier.Private)]
	public string sharedIcon;

	// Token: 0x04005E1B RID: 24091
	[PublicizedFrom(EAccessModifier.Private)]
	public string questlimitedIcon = "ui_game_symbol_quest_limited";

	// Token: 0x04005E1C RID: 24092
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04005E1D RID: 24093
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04005E1E RID: 24094
	public new bool Selected;

	// Token: 0x04005E1F RID: 24095
	public bool IsHovered;

	// Token: 0x04005E20 RID: 24096
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005E21 RID: 24097
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005E25 RID: 24101
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float, string> distanceFormatter = new CachedStringFormatter<float, string>((float _f, string _s) => _f.ToCultureInvariantString("0.0") + " " + _s);

	// Token: 0x04005E26 RID: 24102
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> zerodistanceFormatter = new CachedStringFormatter<string>((string _s) => "0 " + _s);
}
