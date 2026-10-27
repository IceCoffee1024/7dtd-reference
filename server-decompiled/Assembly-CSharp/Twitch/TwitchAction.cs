using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001851 RID: 6225
	public class TwitchAction
	{
		// Token: 0x0600C066 RID: 49254 RVA: 0x0046C061 File Offset: 0x0046A261
		public bool HasModifiedPrice()
		{
			return this.DefaultCost != this.ModifiedCost;
		}

		// Token: 0x0600C067 RID: 49255 RVA: 0x0046C074 File Offset: 0x0046A274
		public bool HasExtraConditions()
		{
			return ((!this.SingleDayUse && !this.RandomDaily) || this.AllowedDay != -1) && !this.OnCooldown && this.OnlyUsableByType == TwitchAction.OnlyUsableTypes.Everyone;
		}

		// Token: 0x0600C068 RID: 49256 RVA: 0x0046C0A4 File Offset: 0x0046A2A4
		public static int GetAdjustedBitPriceCeil(int price)
		{
			int num = TwitchAction.bitPrices.Find((int p) => p >= price);
			if (num == 0)
			{
				return TwitchAction.bitPrices[TwitchAction.bitPrices.Count - 1];
			}
			return num;
		}

		// Token: 0x0600C069 RID: 49257 RVA: 0x0046C0F0 File Offset: 0x0046A2F0
		public static int GetAdjustedBitPriceFloor(int price)
		{
			return TwitchAction.bitPrices.FindLast((int p) => p <= price);
		}

		// Token: 0x0600C06A RID: 49258 RVA: 0x0046C120 File Offset: 0x0046A320
		public static int GetAdjustedBitPriceFloorNoZero(int price)
		{
			return Mathf.Max(TwitchAction.GetAdjustedBitPriceFloor(price), TwitchAction.bitPrices[0]);
		}

		// Token: 0x0600C06B RID: 49259 RVA: 0x0046C138 File Offset: 0x0046A338
		public int GetModifiedDiscountCost()
		{
			return TwitchAction.GetAdjustedBitPriceFloorNoZero((int)((float)this.ModifiedCost * TwitchManager.Current.BitPriceMultiplier));
		}

		// Token: 0x0600C06C RID: 49260 RVA: 0x0046C154 File Offset: 0x0046A354
		public void DecreaseCost()
		{
			if (this.PointType == TwitchAction.PointTypes.Bits)
			{
				this.ModifiedCost = TwitchAction.bitPrices[(int)MathUtils.Clamp((float)(TwitchAction.bitPrices.IndexOf(this.ModifiedCost) - 1), 0f, (float)(TwitchAction.bitPrices.Count - 1))];
				return;
			}
			if (this.ModifiedCost > 25)
			{
				this.ModifiedCost -= 25;
			}
		}

		// Token: 0x0600C06D RID: 49261 RVA: 0x0046C1C0 File Offset: 0x0046A3C0
		public void IncreaseCost()
		{
			if (this.PointType == TwitchAction.PointTypes.Bits)
			{
				this.ModifiedCost = TwitchAction.bitPrices[(int)MathUtils.Clamp((float)(TwitchAction.bitPrices.IndexOf(this.ModifiedCost) + 1), 0f, (float)(TwitchAction.bitPrices.Count - 1))];
				return;
			}
			if (this.ModifiedCost < 2000)
			{
				this.ModifiedCost += 25;
			}
		}

		// Token: 0x0600C06E RID: 49262 RVA: 0x0046C22E File Offset: 0x0046A42E
		public void ResetToDefaultCost()
		{
			if (this.PointType == TwitchAction.PointTypes.Bits)
			{
				this.ModifiedCost = TwitchAction.GetAdjustedBitPriceFloorNoZero(this.DefaultCost);
				return;
			}
			this.ModifiedCost = this.DefaultCost;
		}

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x0600C06F RID: 49263 RVA: 0x0046C257 File Offset: 0x0046A457
		public bool CanUse
		{
			get
			{
				return this.Enabled && this.CheckAllowed();
			}
		}

		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x0600C070 RID: 49264 RVA: 0x0046C269 File Offset: 0x0046A469
		public bool RandomDaily
		{
			get
			{
				return this.RandomGroup != "";
			}
		}

		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x0600C071 RID: 49265 RVA: 0x0046C27B File Offset: 0x0046A47B
		public bool SpecialOnly
		{
			get
			{
				return this.PointType == TwitchAction.PointTypes.SP;
			}
		}

		// Token: 0x0600C072 RID: 49266 RVA: 0x0046C288 File Offset: 0x0046A488
		public bool CheckUsable(TwitchIRCClient.TwitchChatMessage message)
		{
			switch (this.OnlyUsableByType)
			{
			case TwitchAction.OnlyUsableTypes.Broadcaster:
				return message.isBroadcaster;
			case TwitchAction.OnlyUsableTypes.Mods:
				return message.isMod;
			case TwitchAction.OnlyUsableTypes.VIPs:
				return message.isVIP;
			case TwitchAction.OnlyUsableTypes.Subs:
				return message.isSub;
			case TwitchAction.OnlyUsableTypes.Name:
				return this.OnlyUsableBy.ContainsCaseInsensitive(message.UserName);
			default:
				return true;
			}
		}

		// Token: 0x0600C073 RID: 49267 RVA: 0x0046C2EC File Offset: 0x0046A4EC
		public void Init()
		{
			if (this.CategoryNames.Count > 0)
			{
				this.MainCategory = TwitchActionManager.Current.GetCategory(this.CategoryNames[this.CategoryNames.Count - 1]);
				if (this.DisplayCategory == null)
				{
					this.DisplayCategory = this.MainCategory;
				}
			}
			this.OnInit();
		}

		// Token: 0x0600C074 RID: 49268 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C075 RID: 49269 RVA: 0x0046C349 File Offset: 0x0046A549
		public virtual TwitchActionEntry SetupActionEntry()
		{
			return new TwitchActionEntry();
		}

		// Token: 0x0600C076 RID: 49270 RVA: 0x0046C350 File Offset: 0x0046A550
		public bool IsInPreset(TwitchActionPreset preset)
		{
			return ((!preset.IsEmpty && this.PresetNames != null && this.PresetNames.Contains(preset.Name)) || preset.AddedActions.Contains(this.Name)) && !preset.RemovedActions.Contains(this.Name);
		}

		// Token: 0x0600C077 RID: 49271 RVA: 0x0046C3A9 File Offset: 0x0046A5A9
		public bool IsInPresetForList(TwitchActionPreset preset)
		{
			return (!preset.IsEmpty && this.PresetNames != null && this.PresetNames.Contains(preset.Name)) || preset.AddedActions.Contains(this.Name);
		}

		// Token: 0x0600C078 RID: 49272 RVA: 0x0046C3E1 File Offset: 0x0046A5E1
		public bool IsInPresetDefault(TwitchActionPreset preset)
		{
			return !preset.IsEmpty && this.PresetNames != null && this.PresetNames.Contains(preset.Name);
		}

		// Token: 0x0600C079 RID: 49273 RVA: 0x0046C408 File Offset: 0x0046A608
		public bool CanPerformAction(EntityPlayer target, TwitchActionEntry entry)
		{
			if (entry.Target == null)
			{
				entry.Target = target;
			}
			return this.OnPerformAction(target, entry);
		}

		// Token: 0x0600C07A RID: 49274 RVA: 0x0046C42C File Offset: 0x0046A62C
		public void SetQueued()
		{
			this.lastUse = Time.time;
			if (this.CooldownAdditions != null)
			{
				float actionCooldownModifier = TwitchManager.Current.ActionCooldownModifier;
				for (int i = 0; i < this.CooldownAdditions.Count; i++)
				{
					TwitchActionCooldownAddition twitchActionCooldownAddition = this.CooldownAdditions[i];
					if (twitchActionCooldownAddition.IsAction && TwitchActionManager.TwitchActions.ContainsKey(twitchActionCooldownAddition.ActionName))
					{
						TwitchAction twitchAction = TwitchActionManager.TwitchActions[twitchActionCooldownAddition.ActionName];
						twitchAction.tempCooldown = twitchActionCooldownAddition.CooldownTime * actionCooldownModifier;
						twitchAction.tempCooldownSet = Time.time;
					}
					else if (!twitchActionCooldownAddition.IsAction && TwitchActionManager.TwitchVotes.ContainsKey(twitchActionCooldownAddition.ActionName))
					{
						TwitchVote twitchVote = TwitchActionManager.TwitchVotes[twitchActionCooldownAddition.ActionName];
						twitchVote.tempCooldown = twitchActionCooldownAddition.CooldownTime * actionCooldownModifier;
						twitchVote.tempCooldownSet = Time.time;
					}
				}
			}
			if (this.VoteCooldownAddition != 0f)
			{
				TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
				votingManager.VoteStartDelayTimeRemaining = Math.Max(this.VoteCooldownAddition, votingManager.VoteStartDelayTimeRemaining);
			}
			if (this.SingleDayUse)
			{
				this.AllowedDay = -1;
			}
		}

		// Token: 0x0600C07B RID: 49275 RVA: 0x0046C548 File Offset: 0x0046A748
		public virtual bool ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			properties.ParseString(TwitchAction.PropTitle, ref this.Title);
			if (properties.Values.ContainsKey(TwitchAction.PropTitleKey))
			{
				this.Title = Localization.Get(properties.Values[TwitchAction.PropTitleKey], false, null);
			}
			if (properties.Values.ContainsKey(TwitchAction.PropCommand))
			{
				this.BaseCommand = (this.Command = properties.Values[TwitchAction.PropCommand].ToLower());
				if (!Regex.IsMatch(this.Command, "^#[a-zA-Z0-9]+(_[a-zA-Z0-9]+)*$"))
				{
					return false;
				}
			}
			if (properties.Values.ContainsKey(TwitchAction.PropCommandKey))
			{
				this.Command = Localization.Get(properties.Values[TwitchAction.PropCommandKey], false, null).ToLower();
				if (Localization.LocalizationChecks)
				{
					if (this.Command.StartsWith("l_"))
					{
						this.Command = this.Command.Substring(3).Insert(0, "#l_");
					}
					else if (this.Command.StartsWith("ul_"))
					{
						this.Command = this.Command.Substring(3).Insert(0, "#ul_");
					}
					else if (this.Command.StartsWith("le_"))
					{
						this.Command = this.Command.Substring(3).Insert(0, "#le_");
					}
				}
				if (!Regex.IsMatch(this.Command, "^#[\\p{L}\\p{N}]+([-_][\\p{L}\\p{N}]+)*$"))
				{
					return false;
				}
			}
			if (properties.Values.ContainsKey(TwitchAction.PropCategory))
			{
				this.CategoryNames.AddRange(properties.Values[TwitchAction.PropCategory].Split(',', StringSplitOptions.None));
			}
			if (properties.Values.ContainsKey(TwitchAction.PropDisplayCategory))
			{
				this.DisplayCategory = TwitchActionManager.Current.GetCategory(properties.Values[TwitchAction.PropDisplayCategory]);
			}
			properties.ParseString(TwitchAction.PropEventName, ref this.EventName);
			properties.ParseString(TwitchAction.PropDescription, ref this.Description);
			if (properties.Values.ContainsKey(TwitchAction.PropDescriptionKey))
			{
				this.Description = Localization.Get(properties.Values[TwitchAction.PropDescriptionKey], false, null);
			}
			properties.ParseInt(TwitchAction.PropDefaultCost, ref this.DefaultCost);
			properties.ParseInt(TwitchAction.PropStartGameStage, ref this.StartGameStage);
			properties.ParseBool(TwitchAction.PropIsPositive, ref this.IsPositive);
			bool flag = false;
			properties.ParseBool(TwitchAction.PropSpecialOnly, ref flag);
			if (flag)
			{
				this.PointType = TwitchAction.PointTypes.SP;
			}
			properties.ParseBool(TwitchAction.PropAddCooldown, ref this.AddsToCooldown);
			properties.ParseInt(TwitchAction.PropCooldownAddAmount, ref this.CooldownAddAmount);
			properties.ParseBool(TwitchAction.PropCooldownBlocked, ref this.CooldownBlocked);
			properties.ParseBool(TwitchAction.PropWaitingBlocked, ref this.WaitingBlocked);
			if (properties.Values.ContainsKey(TwitchAction.PropCooldown))
			{
				this.OriginalCooldown = (this.Cooldown = StringParsers.ParseFloat(properties.Values[TwitchAction.PropCooldown], 0, -1, NumberStyles.Any));
				this.lastUse = Time.time - this.Cooldown;
				this.ModifiedCooldown = this.Cooldown;
			}
			properties.ParseBool(TwitchAction.PropEnabled, ref this.Enabled);
			this.OriginalEnabled = this.Enabled;
			properties.ParseBool(TwitchAction.PropSingleDayUse, ref this.SingleDayUse);
			properties.ParseString(TwitchAction.PropRandomGroup, ref this.RandomGroup);
			properties.ParseBool(TwitchAction.PropShowInActionList, ref this.ShowInActionList);
			if (properties.Values.ContainsKey(TwitchAction.PropSpecialRequirement))
			{
				string[] array = this.Properties.Values[TwitchAction.PropSpecialRequirement].Split(',', StringSplitOptions.None);
				this.SpecialRequirementList = new List<TwitchAction.SpecialRequirements>();
				foreach (string text in array)
				{
					TwitchAction.SpecialRequirements specialRequirements = TwitchAction.SpecialRequirements.None;
					if (Enum.TryParse<TwitchAction.SpecialRequirements>(text, true, out specialRequirements))
					{
						this.SpecialRequirementList.Add(specialRequirements);
					}
					else
					{
						Log.Error("TwitchAction " + this.Title + " has unknown SpecialRequirement " + text);
					}
				}
			}
			properties.ParseString(TwitchAction.PropReplaces, ref this.Replaces);
			properties.ParseBool(TwitchAction.PropTwitchNotify, ref this.TwitchNotify);
			properties.ParseBool(TwitchAction.PropDelayNotify, ref this.DelayNotify);
			properties.ParseBool(TwitchAction.PropPlayBitSound, ref this.PlayBitSound);
			properties.ParseBool(TwitchAction.PropHideOnDisable, ref this.HideOnDisable);
			properties.ParseBool(TwitchAction.PropIgnoreCooldown, ref this.IgnoreCooldown);
			properties.ParseBool(TwitchAction.PropIgnoreDiscount, ref this.IgnoreDiscount);
			properties.ParseBool(TwitchAction.PropStreamerOnly, ref this.StreamerOnly);
			properties.ParseEnum<TwitchAction.OnlyUsableTypes>(TwitchAction.PropOnlyUsableByType, ref this.OnlyUsableByType);
			string text2 = "";
			properties.ParseString(TwitchAction.PropOnlyUsableBy, ref text2);
			if (text2 != "")
			{
				this.OnlyUsableBy = text2.Split(',', StringSplitOptions.None);
			}
			properties.ParseEnum<TwitchAction.PointTypes>(TwitchAction.PropPointTypes, ref this.PointType);
			this.ResetToDefaultCost();
			this.UpdateCost(1f);
			if (this.CooldownAddAmount == -1)
			{
				this.CooldownAddAmount = this.DefaultCost;
			}
			properties.ParseFloat(TwitchAction.PropVoteCooldownAddition, ref this.VoteCooldownAddition);
			if (properties.Values.ContainsKey(TwitchAction.PropPresets))
			{
				this.PresetNames = new List<string>();
				this.PresetNames.AddRange(properties.Values[TwitchAction.PropPresets].Split(',', StringSplitOptions.None));
			}
			properties.ParseBool(TwitchAction.PropOnlyShowInPreset, ref this.OnlyShowInPreset);
			if (properties.Contains(TwitchAction.PropMinRespawnCount) || properties.Contains(TwitchAction.PropMaxRespawnCount))
			{
				properties.ParseInt(TwitchAction.PropMinRespawnCount, ref this.MinRespawnCount);
				properties.ParseInt(TwitchAction.PropMaxRespawnCount, ref this.MaxRespawnCount);
				this.RespawnCountType = TwitchAction.RespawnCountTypes.Both;
			}
			else
			{
				this.RespawnCountType = TwitchAction.RespawnCountTypes.None;
			}
			properties.ParseEnum<TwitchAction.RespawnCountTypes>(TwitchAction.PropRespawnCountType, ref this.RespawnCountType);
			properties.ParseInt(TwitchAction.PropRespawnThreshold, ref this.RespawnThreshold);
			return true;
		}

		// Token: 0x0600C07C RID: 49276 RVA: 0x0046CB20 File Offset: 0x0046AD20
		public bool UpdateCost(float bitPriceModifier = 1f)
		{
			int currentCost = this.CurrentCost;
			if (this.PointType == TwitchAction.PointTypes.Bits)
			{
				if (!this.IgnoreDiscount && bitPriceModifier != 1f)
				{
					this.CurrentCost = TwitchAction.GetAdjustedBitPriceFloorNoZero((int)((float)this.ModifiedCost * bitPriceModifier));
				}
				else
				{
					this.CurrentCost = TwitchAction.GetAdjustedBitPriceFloorNoZero(this.ModifiedCost);
				}
			}
			else
			{
				this.CurrentCost = this.ModifiedCost;
			}
			return currentCost != this.CurrentCost;
		}

		// Token: 0x0600C07D RID: 49277 RVA: 0x0046CB8D File Offset: 0x0046AD8D
		public void AddRequirement(BaseTwitchRequirement requirement)
		{
			if (this.TwitchRequirements == null)
			{
				this.TwitchRequirements = new List<BaseTwitchRequirement>();
			}
			requirement.OwnerAction = this;
			requirement.OwnerType = BaseTwitchRequirement.OwnerTypes.Action;
			this.TwitchRequirements.Add(requirement);
		}

		// Token: 0x0600C07E RID: 49278 RVA: 0x0046CBBC File Offset: 0x0046ADBC
		public bool CheckAllowed()
		{
			if (this.TwitchRequirements != null)
			{
				EntityPlayerLocal localPlayer = TwitchManager.Current.LocalPlayer;
				for (int i = 0; i < this.TwitchRequirements.Count; i++)
				{
					BaseTwitchRequirement baseTwitchRequirement = this.TwitchRequirements[i];
					if (baseTwitchRequirement.HideAction && !baseTwitchRequirement.CanPerform(localPlayer))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600C07F RID: 49279 RVA: 0x0046CC14 File Offset: 0x0046AE14
		public virtual bool IsReady(TwitchManager twitchManager)
		{
			if (this.TwitchRequirements != null)
			{
				EntityPlayerLocal localPlayer = twitchManager.LocalPlayer;
				for (int i = 0; i < this.TwitchRequirements.Count; i++)
				{
					BaseTwitchRequirement baseTwitchRequirement = this.TwitchRequirements[i];
					if (!baseTwitchRequirement.HideAction && !baseTwitchRequirement.CanPerform(localPlayer))
					{
						return false;
					}
				}
			}
			if (this.SpecialRequirementList != null)
			{
				for (int j = 0; j < this.SpecialRequirementList.Count; j++)
				{
					switch (this.SpecialRequirementList[j])
					{
					case TwitchAction.SpecialRequirements.HasSpawnedEntities:
						if (twitchManager.actionSpawnLiveList.Count == 0)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.NoSpawnedEntities:
						if (twitchManager.actionSpawnLiveList.Count > 0)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.Bloodmoon:
						if (!twitchManager.isBMActive)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.NotBloodmoon:
						if (twitchManager.isBMActive)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.NotBloodmoonDay:
						if (SkyManager.IsBloodMoonVisible())
						{
							return false;
						}
						if (GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime) == GameStats.GetInt(EnumGameStats.BloodMoonDay))
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.EarlyDay:
					{
						int num = GameUtils.WorldTimeToHours(GameManager.Instance.World.worldTime);
						if ((float)num > SkyManager.GetDuskTime() - 5f || (float)num < SkyManager.GetDawnTime())
						{
							return false;
						}
						break;
					}
					case TwitchAction.SpecialRequirements.Daytime:
					{
						int num2 = GameUtils.WorldTimeToHours(GameManager.Instance.World.worldTime);
						if ((float)num2 > SkyManager.GetDuskTime() || (float)num2 < SkyManager.GetDawnTime())
						{
							return false;
						}
						break;
					}
					case TwitchAction.SpecialRequirements.Night:
						GameUtils.WorldTimeToHours(GameManager.Instance.World.worldTime);
						if (!SkyManager.IsDark())
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.NotAllDaySetting:
						if (SkyManager.isAllTimeDay || SkyManager.isAllTimeDay)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.IsCooldown:
						if (!TwitchManager.HasInstance || !twitchManager.IsReady)
						{
							return false;
						}
						if (twitchManager.CurrentCooldownPreset.CooldownType != CooldownPreset.CooldownTypes.Fill)
						{
							return false;
						}
						if (twitchManager.CooldownType != TwitchManager.CooldownTypes.MaxReached)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.InLandClaim:
						if (!twitchManager.LocalPlayerInLandClaim)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.NotInLandClaim:
						if (twitchManager.LocalPlayerInLandClaim)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.Safe:
						if (!twitchManager.LocalPlayer.TwitchSafe)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.NotSafe:
						if (twitchManager.LocalPlayer.TwitchSafe)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.NoFullProgression:
						if (!twitchManager.IsReady)
						{
							return false;
						}
						if (!twitchManager.UseProgression || twitchManager.OverrideProgession)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.NotOnVehicle:
						if (!twitchManager.IsReady)
						{
							return false;
						}
						if (twitchManager.LocalPlayer.AttachedToEntity != null)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.NotInTrader:
						if (twitchManager.LocalPlayer.IsInTrader)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.Encumbrance:
						if ((int)EffectManager.GetValue(PassiveEffects.CarryCapacity, null, 0f, twitchManager.LocalPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) <= 30)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.WeatherGracePeriod:
						if (GameManager.Instance.World.GetWorldTime() <= 30000UL)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.NotOnQuest:
						if (QuestEventManager.Current.QuestBounds.width != 0f)
						{
							return false;
						}
						break;
					case TwitchAction.SpecialRequirements.OnQuest:
						if (QuestEventManager.Current.QuestBounds.width == 0f)
						{
							return false;
						}
						break;
					}
				}
			}
			bool flag = (!this.SingleDayUse && !this.RandomDaily) || this.AllowedDay == GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
			if (this.tempCooldown > 0f && twitchManager.CurrentUnityTime - this.tempCooldownSet < this.tempCooldown)
			{
				return false;
			}
			this.tempCooldown = 0f;
			this.tempCooldownSet = 0f;
			return twitchManager.CurrentUnityTime - this.lastUse >= this.ModifiedCooldown && flag;
		}

		// Token: 0x0600C080 RID: 49280 RVA: 0x0046CFBB File Offset: 0x0046B1BB
		public void UpdateModifiedCooldown(float modifier)
		{
			this.ModifiedCooldown = modifier * this.Cooldown;
		}

		// Token: 0x0600C081 RID: 49281 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool OnPerformAction(EntityPlayer Target, TwitchActionEntry entry)
		{
			return false;
		}

		// Token: 0x0600C082 RID: 49282 RVA: 0x0046CFCB File Offset: 0x0046B1CB
		public void AddCooldownAddition(TwitchActionCooldownAddition newCooldown)
		{
			if (this.CooldownAdditions == null)
			{
				this.CooldownAdditions = new List<TwitchActionCooldownAddition>();
			}
			this.CooldownAdditions.Add(newCooldown);
		}

		// Token: 0x0600C083 RID: 49283 RVA: 0x0046CFEC File Offset: 0x0046B1EC
		public void ResetCooldown(float currentUnityTime)
		{
			this.lastUse = currentUnityTime - this.ModifiedCooldown;
			this.tempCooldown = 0f;
			this.tempCooldownSet = 0f;
		}

		// Token: 0x0600C084 RID: 49284 RVA: 0x0046D012 File Offset: 0x0046B212
		public void SetCooldown(float currentUnityTime, float newCooldownTime)
		{
			this.lastUse = currentUnityTime - this.ModifiedCooldown;
			this.tempCooldown = newCooldownTime;
			this.tempCooldownSet = currentUnityTime;
		}

		// Token: 0x0400901D RID: 36893
		public string Name = "";

		// Token: 0x0400901E RID: 36894
		public string Title = "";

		// Token: 0x0400901F RID: 36895
		public string BaseCommand = "";

		// Token: 0x04009020 RID: 36896
		public string Command = "";

		// Token: 0x04009021 RID: 36897
		public string EventName = "";

		// Token: 0x04009022 RID: 36898
		public List<string> CategoryNames = new List<string>();

		// Token: 0x04009023 RID: 36899
		public int DefaultCost = 5;

		// Token: 0x04009024 RID: 36900
		public int ModifiedCost = 5;

		// Token: 0x04009025 RID: 36901
		public int CurrentCost = 5;

		// Token: 0x04009026 RID: 36902
		public int StartGameStage;

		// Token: 0x04009027 RID: 36903
		public string Description;

		// Token: 0x04009028 RID: 36904
		public float OriginalCooldown;

		// Token: 0x04009029 RID: 36905
		public float Cooldown;

		// Token: 0x0400902A RID: 36906
		public bool IsPositive;

		// Token: 0x0400902B RID: 36907
		public bool AddsToCooldown;

		// Token: 0x0400902C RID: 36908
		public int CooldownAddAmount = -1;

		// Token: 0x0400902D RID: 36909
		public bool CooldownBlocked;

		// Token: 0x0400902E RID: 36910
		public bool WaitingBlocked;

		// Token: 0x0400902F RID: 36911
		public bool OnCooldown;

		// Token: 0x04009030 RID: 36912
		public bool Enabled = true;

		// Token: 0x04009031 RID: 36913
		public bool SingleDayUse;

		// Token: 0x04009032 RID: 36914
		public bool DelayNotify;

		// Token: 0x04009033 RID: 36915
		public bool TwitchNotify = true;

		// Token: 0x04009034 RID: 36916
		public bool PlayBitSound = true;

		// Token: 0x04009035 RID: 36917
		public string RandomGroup = "";

		// Token: 0x04009036 RID: 36918
		public string Replaces = "";

		// Token: 0x04009037 RID: 36919
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUse;

		// Token: 0x04009038 RID: 36920
		public int AllowedDay = -1;

		// Token: 0x04009039 RID: 36921
		public int groupIndex;

		// Token: 0x0400903A RID: 36922
		public float tempCooldownSet;

		// Token: 0x0400903B RID: 36923
		public float tempCooldown;

		// Token: 0x0400903C RID: 36924
		public bool IgnoreCooldown;

		// Token: 0x0400903D RID: 36925
		public bool IgnoreDiscount;

		// Token: 0x0400903E RID: 36926
		public float ModifiedCooldown;

		// Token: 0x0400903F RID: 36927
		public List<TwitchActionCooldownAddition> CooldownAdditions;

		// Token: 0x04009040 RID: 36928
		public bool ShowInActionList = true;

		// Token: 0x04009041 RID: 36929
		public List<TwitchAction.SpecialRequirements> SpecialRequirementList;

		// Token: 0x04009042 RID: 36930
		public List<BaseTwitchRequirement> TwitchRequirements;

		// Token: 0x04009043 RID: 36931
		public bool HideOnDisable;

		// Token: 0x04009044 RID: 36932
		public TwitchAction.OnlyUsableTypes OnlyUsableByType;

		// Token: 0x04009045 RID: 36933
		public string[] OnlyUsableBy;

		// Token: 0x04009046 RID: 36934
		public bool OriginalEnabled = true;

		// Token: 0x04009047 RID: 36935
		public TwitchAction.PointTypes PointType;

		// Token: 0x04009048 RID: 36936
		public TwitchActionManager.ActionCategory MainCategory;

		// Token: 0x04009049 RID: 36937
		public TwitchActionManager.ActionCategory DisplayCategory;

		// Token: 0x0400904A RID: 36938
		public List<string> PresetNames;

		// Token: 0x0400904B RID: 36939
		public bool OnlyShowInPreset;

		// Token: 0x0400904C RID: 36940
		public float VoteCooldownAddition;

		// Token: 0x0400904D RID: 36941
		public bool StreamerOnly;

		// Token: 0x0400904E RID: 36942
		public TwitchAction.RespawnCountTypes RespawnCountType;

		// Token: 0x0400904F RID: 36943
		public int MinRespawnCount = -1;

		// Token: 0x04009050 RID: 36944
		public int MaxRespawnCount = -1;

		// Token: 0x04009051 RID: 36945
		public int RespawnThreshold;

		// Token: 0x04009052 RID: 36946
		public DynamicProperties Properties;

		// Token: 0x04009053 RID: 36947
		public static string PropCommand = "command";

		// Token: 0x04009054 RID: 36948
		public static string PropCommandKey = "command_key";

		// Token: 0x04009055 RID: 36949
		public static string PropTitle = "title";

		// Token: 0x04009056 RID: 36950
		public static string PropTitleKey = "title_key";

		// Token: 0x04009057 RID: 36951
		public static string PropCategory = "category";

		// Token: 0x04009058 RID: 36952
		public static string PropDisplayCategory = "display_category";

		// Token: 0x04009059 RID: 36953
		public static string PropEventName = "event_name";

		// Token: 0x0400905A RID: 36954
		public static string PropDefaultCost = "default_cost";

		// Token: 0x0400905B RID: 36955
		public static string PropDescription = "description";

		// Token: 0x0400905C RID: 36956
		public static string PropDescriptionKey = "description_key";

		// Token: 0x0400905D RID: 36957
		public static string PropStartGameStage = "start_gamestage";

		// Token: 0x0400905E RID: 36958
		public static string PropCooldown = "cooldown";

		// Token: 0x0400905F RID: 36959
		public static string PropIsPositive = "is_positive";

		// Token: 0x04009060 RID: 36960
		public static string PropSpecialOnly = "special_only";

		// Token: 0x04009061 RID: 36961
		public static string PropAddCooldown = "add_cooldown";

		// Token: 0x04009062 RID: 36962
		public static string PropCooldownAddAmount = "cooldown_add_amount";

		// Token: 0x04009063 RID: 36963
		public static string PropCooldownBlocked = "cooldown_blocked";

		// Token: 0x04009064 RID: 36964
		public static string PropWaitingBlocked = "waiting_blocked";

		// Token: 0x04009065 RID: 36965
		public static string PropEnabled = "enabled";

		// Token: 0x04009066 RID: 36966
		public static string PropSingleDayUse = "single_day";

		// Token: 0x04009067 RID: 36967
		public static string PropRandomGroup = "random_group";

		// Token: 0x04009068 RID: 36968
		public static string PropReplaces = "replaces";

		// Token: 0x04009069 RID: 36969
		public static string PropShowInActionList = "show_in_action_list";

		// Token: 0x0400906A RID: 36970
		public static string PropPlayBitSound = "play_bit_sound";

		// Token: 0x0400906B RID: 36971
		public static string PropSpecialRequirement = "special_requirement";

		// Token: 0x0400906C RID: 36972
		public static string PropTwitchNotify = "twitch_notify";

		// Token: 0x0400906D RID: 36973
		public static string PropDelayNotify = "delay_notify";

		// Token: 0x0400906E RID: 36974
		public static string PropHideOnDisable = "hide_on_disable";

		// Token: 0x0400906F RID: 36975
		public static string PropOnlyUsableByType = "only_usable_type";

		// Token: 0x04009070 RID: 36976
		public static string PropOnlyUsableBy = "only_usable_by";

		// Token: 0x04009071 RID: 36977
		public static string PropIgnoreCooldown = "ignore_cooldown";

		// Token: 0x04009072 RID: 36978
		public static string PropIgnoreDiscount = "ignore_discount";

		// Token: 0x04009073 RID: 36979
		public static string PropPointTypes = "point_type";

		// Token: 0x04009074 RID: 36980
		public static string PropPresets = "presets";

		// Token: 0x04009075 RID: 36981
		public static string PropOnlyShowInPreset = "only_show_in_preset";

		// Token: 0x04009076 RID: 36982
		public static string PropVoteCooldownAddition = "vote_cooldown_add";

		// Token: 0x04009077 RID: 36983
		public static string PropStreamerOnly = "streamer_only";

		// Token: 0x04009078 RID: 36984
		public static string PropMinRespawnCount = "min_respawn_count";

		// Token: 0x04009079 RID: 36985
		public static string PropMaxRespawnCount = "max_respawn_count";

		// Token: 0x0400907A RID: 36986
		public static string PropRespawnCountType = "respawn_count_type";

		// Token: 0x0400907B RID: 36987
		public static string PropRespawnThreshold = "respawn_threshold";

		// Token: 0x0400907C RID: 36988
		public static HashSet<string> ExtendsExcludes = new HashSet<string>
		{
			TwitchAction.PropShowInActionList,
			TwitchAction.PropCommandKey,
			TwitchAction.PropCommand,
			TwitchAction.PropTitleKey,
			TwitchAction.PropDescriptionKey
		};

		// Token: 0x0400907D RID: 36989
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<int> bitPrices = new List<int>
		{
			10, 15, 20, 25, 30, 35, 40, 45, 50, 55,
			60, 65, 70, 75, 80, 85, 90, 95, 100, 105,
			110, 115, 120, 125, 130, 135, 140, 145, 150, 155,
			160, 165, 170, 175, 180, 185, 190, 195, 200, 250,
			300, 350, 400, 450, 500, 550, 600, 650, 700, 750,
			800, 850, 900, 950, 1000, 1100, 1200, 1250, 1300, 1400,
			1500, 1600, 1700, 1750, 1800, 1900, 2000, 2250, 2500, 2750,
			3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500,
			8000, 8500, 9000, 9500, 10000
		};

		// Token: 0x02001852 RID: 6226
		public enum SpecialRequirements
		{
			// Token: 0x0400907F RID: 36991
			None,
			// Token: 0x04009080 RID: 36992
			HasSpawnedEntities,
			// Token: 0x04009081 RID: 36993
			NoSpawnedEntities,
			// Token: 0x04009082 RID: 36994
			Bloodmoon,
			// Token: 0x04009083 RID: 36995
			NotBloodmoon,
			// Token: 0x04009084 RID: 36996
			NotBloodmoonDay,
			// Token: 0x04009085 RID: 36997
			EarlyDay,
			// Token: 0x04009086 RID: 36998
			Daytime,
			// Token: 0x04009087 RID: 36999
			Night,
			// Token: 0x04009088 RID: 37000
			NotAllDaySetting,
			// Token: 0x04009089 RID: 37001
			IsCooldown,
			// Token: 0x0400908A RID: 37002
			InLandClaim,
			// Token: 0x0400908B RID: 37003
			NotInLandClaim,
			// Token: 0x0400908C RID: 37004
			Safe,
			// Token: 0x0400908D RID: 37005
			NotSafe,
			// Token: 0x0400908E RID: 37006
			NoFullProgression,
			// Token: 0x0400908F RID: 37007
			NotOnVehicle,
			// Token: 0x04009090 RID: 37008
			NotInTrader,
			// Token: 0x04009091 RID: 37009
			Encumbrance,
			// Token: 0x04009092 RID: 37010
			WeatherGracePeriod,
			// Token: 0x04009093 RID: 37011
			NotOnQuest,
			// Token: 0x04009094 RID: 37012
			OnQuest
		}

		// Token: 0x02001853 RID: 6227
		public enum OnlyUsableTypes
		{
			// Token: 0x04009096 RID: 37014
			Everyone,
			// Token: 0x04009097 RID: 37015
			Broadcaster,
			// Token: 0x04009098 RID: 37016
			Mods,
			// Token: 0x04009099 RID: 37017
			VIPs,
			// Token: 0x0400909A RID: 37018
			Subs,
			// Token: 0x0400909B RID: 37019
			Name
		}

		// Token: 0x02001854 RID: 6228
		public enum PointTypes
		{
			// Token: 0x0400909D RID: 37021
			PP,
			// Token: 0x0400909E RID: 37022
			SP,
			// Token: 0x0400909F RID: 37023
			Bits
		}

		// Token: 0x02001855 RID: 6229
		public enum RespawnCountTypes
		{
			// Token: 0x040090A1 RID: 37025
			None,
			// Token: 0x040090A2 RID: 37026
			Both,
			// Token: 0x040090A3 RID: 37027
			BlocksOnly,
			// Token: 0x040090A4 RID: 37028
			SpawnsOnly
		}
	}
}
