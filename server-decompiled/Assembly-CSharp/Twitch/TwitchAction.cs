using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001820 RID: 6176
	public class TwitchAction
	{
		// Token: 0x0600BF52 RID: 48978 RVA: 0x004686B5 File Offset: 0x004668B5
		public bool HasModifiedPrice()
		{
			return this.DefaultCost != this.ModifiedCost;
		}

		// Token: 0x0600BF53 RID: 48979 RVA: 0x004686C8 File Offset: 0x004668C8
		public bool HasExtraConditions()
		{
			return ((!this.SingleDayUse && !this.RandomDaily) || this.AllowedDay != -1) && !this.OnCooldown && this.OnlyUsableByType == TwitchAction.OnlyUsableTypes.Everyone;
		}

		// Token: 0x0600BF54 RID: 48980 RVA: 0x004686F8 File Offset: 0x004668F8
		public static int GetAdjustedBitPriceCeil(int price)
		{
			int num = TwitchAction.bitPrices.Find((int p) => p >= price);
			if (num == 0)
			{
				return TwitchAction.bitPrices[TwitchAction.bitPrices.Count - 1];
			}
			return num;
		}

		// Token: 0x0600BF55 RID: 48981 RVA: 0x00468744 File Offset: 0x00466944
		public static int GetAdjustedBitPriceFloor(int price)
		{
			return TwitchAction.bitPrices.FindLast((int p) => p <= price);
		}

		// Token: 0x0600BF56 RID: 48982 RVA: 0x00468774 File Offset: 0x00466974
		public static int GetAdjustedBitPriceFloorNoZero(int price)
		{
			return Mathf.Max(TwitchAction.GetAdjustedBitPriceFloor(price), TwitchAction.bitPrices[0]);
		}

		// Token: 0x0600BF57 RID: 48983 RVA: 0x0046878C File Offset: 0x0046698C
		public int GetModifiedDiscountCost()
		{
			return TwitchAction.GetAdjustedBitPriceFloorNoZero((int)((float)this.ModifiedCost * TwitchManager.Current.BitPriceMultiplier));
		}

		// Token: 0x0600BF58 RID: 48984 RVA: 0x004687A8 File Offset: 0x004669A8
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

		// Token: 0x0600BF59 RID: 48985 RVA: 0x00468814 File Offset: 0x00466A14
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

		// Token: 0x0600BF5A RID: 48986 RVA: 0x00468882 File Offset: 0x00466A82
		public void ResetToDefaultCost()
		{
			if (this.PointType == TwitchAction.PointTypes.Bits)
			{
				this.ModifiedCost = TwitchAction.GetAdjustedBitPriceFloorNoZero(this.DefaultCost);
				return;
			}
			this.ModifiedCost = this.DefaultCost;
		}

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x0600BF5B RID: 48987 RVA: 0x004688AB File Offset: 0x00466AAB
		public bool CanUse
		{
			get
			{
				return this.Enabled && this.CheckAllowed();
			}
		}

		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x0600BF5C RID: 48988 RVA: 0x004688BD File Offset: 0x00466ABD
		public bool RandomDaily
		{
			get
			{
				return this.RandomGroup != "";
			}
		}

		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x0600BF5D RID: 48989 RVA: 0x004688CF File Offset: 0x00466ACF
		public bool SpecialOnly
		{
			get
			{
				return this.PointType == TwitchAction.PointTypes.SP;
			}
		}

		// Token: 0x0600BF5E RID: 48990 RVA: 0x004688DC File Offset: 0x00466ADC
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

		// Token: 0x0600BF5F RID: 48991 RVA: 0x00468940 File Offset: 0x00466B40
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

		// Token: 0x0600BF60 RID: 48992 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600BF61 RID: 48993 RVA: 0x0046899D File Offset: 0x00466B9D
		public virtual TwitchActionEntry SetupActionEntry()
		{
			return new TwitchActionEntry();
		}

		// Token: 0x0600BF62 RID: 48994 RVA: 0x004689A4 File Offset: 0x00466BA4
		public bool IsInPreset(TwitchActionPreset preset)
		{
			return ((!preset.IsEmpty && this.PresetNames != null && this.PresetNames.Contains(preset.Name)) || preset.AddedActions.Contains(this.Name)) && !preset.RemovedActions.Contains(this.Name);
		}

		// Token: 0x0600BF63 RID: 48995 RVA: 0x004689FD File Offset: 0x00466BFD
		public bool IsInPresetForList(TwitchActionPreset preset)
		{
			return (!preset.IsEmpty && this.PresetNames != null && this.PresetNames.Contains(preset.Name)) || preset.AddedActions.Contains(this.Name);
		}

		// Token: 0x0600BF64 RID: 48996 RVA: 0x00468A35 File Offset: 0x00466C35
		public bool IsInPresetDefault(TwitchActionPreset preset)
		{
			return !preset.IsEmpty && this.PresetNames != null && this.PresetNames.Contains(preset.Name);
		}

		// Token: 0x0600BF65 RID: 48997 RVA: 0x00468A5C File Offset: 0x00466C5C
		public bool CanPerformAction(EntityPlayer target, TwitchActionEntry entry)
		{
			if (entry.Target == null)
			{
				entry.Target = target;
			}
			return this.OnPerformAction(target, entry);
		}

		// Token: 0x0600BF66 RID: 48998 RVA: 0x00468A80 File Offset: 0x00466C80
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

		// Token: 0x0600BF67 RID: 48999 RVA: 0x00468B9C File Offset: 0x00466D9C
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

		// Token: 0x0600BF68 RID: 49000 RVA: 0x00469174 File Offset: 0x00467374
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

		// Token: 0x0600BF69 RID: 49001 RVA: 0x004691E1 File Offset: 0x004673E1
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

		// Token: 0x0600BF6A RID: 49002 RVA: 0x00469210 File Offset: 0x00467410
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

		// Token: 0x0600BF6B RID: 49003 RVA: 0x00469268 File Offset: 0x00467468
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

		// Token: 0x0600BF6C RID: 49004 RVA: 0x0046960F File Offset: 0x0046780F
		public void UpdateModifiedCooldown(float modifier)
		{
			this.ModifiedCooldown = modifier * this.Cooldown;
		}

		// Token: 0x0600BF6D RID: 49005 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool OnPerformAction(EntityPlayer Target, TwitchActionEntry entry)
		{
			return false;
		}

		// Token: 0x0600BF6E RID: 49006 RVA: 0x0046961F File Offset: 0x0046781F
		public void AddCooldownAddition(TwitchActionCooldownAddition newCooldown)
		{
			if (this.CooldownAdditions == null)
			{
				this.CooldownAdditions = new List<TwitchActionCooldownAddition>();
			}
			this.CooldownAdditions.Add(newCooldown);
		}

		// Token: 0x0600BF6F RID: 49007 RVA: 0x00469640 File Offset: 0x00467840
		public void ResetCooldown(float currentUnityTime)
		{
			this.lastUse = currentUnityTime - this.ModifiedCooldown;
			this.tempCooldown = 0f;
			this.tempCooldownSet = 0f;
		}

		// Token: 0x0600BF70 RID: 49008 RVA: 0x00469666 File Offset: 0x00467866
		public void SetCooldown(float currentUnityTime, float newCooldownTime)
		{
			this.lastUse = currentUnityTime - this.ModifiedCooldown;
			this.tempCooldown = newCooldownTime;
			this.tempCooldownSet = currentUnityTime;
		}

		// Token: 0x04008F34 RID: 36660
		public string Name = "";

		// Token: 0x04008F35 RID: 36661
		public string Title = "";

		// Token: 0x04008F36 RID: 36662
		public string BaseCommand = "";

		// Token: 0x04008F37 RID: 36663
		public string Command = "";

		// Token: 0x04008F38 RID: 36664
		public string EventName = "";

		// Token: 0x04008F39 RID: 36665
		public List<string> CategoryNames = new List<string>();

		// Token: 0x04008F3A RID: 36666
		public int DefaultCost = 5;

		// Token: 0x04008F3B RID: 36667
		public int ModifiedCost = 5;

		// Token: 0x04008F3C RID: 36668
		public int CurrentCost = 5;

		// Token: 0x04008F3D RID: 36669
		public int StartGameStage;

		// Token: 0x04008F3E RID: 36670
		public string Description;

		// Token: 0x04008F3F RID: 36671
		public float OriginalCooldown;

		// Token: 0x04008F40 RID: 36672
		public float Cooldown;

		// Token: 0x04008F41 RID: 36673
		public bool IsPositive;

		// Token: 0x04008F42 RID: 36674
		public bool AddsToCooldown;

		// Token: 0x04008F43 RID: 36675
		public int CooldownAddAmount = -1;

		// Token: 0x04008F44 RID: 36676
		public bool CooldownBlocked;

		// Token: 0x04008F45 RID: 36677
		public bool WaitingBlocked;

		// Token: 0x04008F46 RID: 36678
		public bool OnCooldown;

		// Token: 0x04008F47 RID: 36679
		public bool Enabled = true;

		// Token: 0x04008F48 RID: 36680
		public bool SingleDayUse;

		// Token: 0x04008F49 RID: 36681
		public bool DelayNotify;

		// Token: 0x04008F4A RID: 36682
		public bool TwitchNotify = true;

		// Token: 0x04008F4B RID: 36683
		public bool PlayBitSound = true;

		// Token: 0x04008F4C RID: 36684
		public string RandomGroup = "";

		// Token: 0x04008F4D RID: 36685
		public string Replaces = "";

		// Token: 0x04008F4E RID: 36686
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUse;

		// Token: 0x04008F4F RID: 36687
		public int AllowedDay = -1;

		// Token: 0x04008F50 RID: 36688
		public int groupIndex;

		// Token: 0x04008F51 RID: 36689
		public float tempCooldownSet;

		// Token: 0x04008F52 RID: 36690
		public float tempCooldown;

		// Token: 0x04008F53 RID: 36691
		public bool IgnoreCooldown;

		// Token: 0x04008F54 RID: 36692
		public bool IgnoreDiscount;

		// Token: 0x04008F55 RID: 36693
		public float ModifiedCooldown;

		// Token: 0x04008F56 RID: 36694
		public List<TwitchActionCooldownAddition> CooldownAdditions;

		// Token: 0x04008F57 RID: 36695
		public bool ShowInActionList = true;

		// Token: 0x04008F58 RID: 36696
		public List<TwitchAction.SpecialRequirements> SpecialRequirementList;

		// Token: 0x04008F59 RID: 36697
		public List<BaseTwitchRequirement> TwitchRequirements;

		// Token: 0x04008F5A RID: 36698
		public bool HideOnDisable;

		// Token: 0x04008F5B RID: 36699
		public TwitchAction.OnlyUsableTypes OnlyUsableByType;

		// Token: 0x04008F5C RID: 36700
		public string[] OnlyUsableBy;

		// Token: 0x04008F5D RID: 36701
		public bool OriginalEnabled = true;

		// Token: 0x04008F5E RID: 36702
		public TwitchAction.PointTypes PointType;

		// Token: 0x04008F5F RID: 36703
		public TwitchActionManager.ActionCategory MainCategory;

		// Token: 0x04008F60 RID: 36704
		public TwitchActionManager.ActionCategory DisplayCategory;

		// Token: 0x04008F61 RID: 36705
		public List<string> PresetNames;

		// Token: 0x04008F62 RID: 36706
		public bool OnlyShowInPreset;

		// Token: 0x04008F63 RID: 36707
		public float VoteCooldownAddition;

		// Token: 0x04008F64 RID: 36708
		public bool StreamerOnly;

		// Token: 0x04008F65 RID: 36709
		public TwitchAction.RespawnCountTypes RespawnCountType;

		// Token: 0x04008F66 RID: 36710
		public int MinRespawnCount = -1;

		// Token: 0x04008F67 RID: 36711
		public int MaxRespawnCount = -1;

		// Token: 0x04008F68 RID: 36712
		public int RespawnThreshold;

		// Token: 0x04008F69 RID: 36713
		public DynamicProperties Properties;

		// Token: 0x04008F6A RID: 36714
		public static string PropCommand = "command";

		// Token: 0x04008F6B RID: 36715
		public static string PropCommandKey = "command_key";

		// Token: 0x04008F6C RID: 36716
		public static string PropTitle = "title";

		// Token: 0x04008F6D RID: 36717
		public static string PropTitleKey = "title_key";

		// Token: 0x04008F6E RID: 36718
		public static string PropCategory = "category";

		// Token: 0x04008F6F RID: 36719
		public static string PropDisplayCategory = "display_category";

		// Token: 0x04008F70 RID: 36720
		public static string PropEventName = "event_name";

		// Token: 0x04008F71 RID: 36721
		public static string PropDefaultCost = "default_cost";

		// Token: 0x04008F72 RID: 36722
		public static string PropDescription = "description";

		// Token: 0x04008F73 RID: 36723
		public static string PropDescriptionKey = "description_key";

		// Token: 0x04008F74 RID: 36724
		public static string PropStartGameStage = "start_gamestage";

		// Token: 0x04008F75 RID: 36725
		public static string PropCooldown = "cooldown";

		// Token: 0x04008F76 RID: 36726
		public static string PropIsPositive = "is_positive";

		// Token: 0x04008F77 RID: 36727
		public static string PropSpecialOnly = "special_only";

		// Token: 0x04008F78 RID: 36728
		public static string PropAddCooldown = "add_cooldown";

		// Token: 0x04008F79 RID: 36729
		public static string PropCooldownAddAmount = "cooldown_add_amount";

		// Token: 0x04008F7A RID: 36730
		public static string PropCooldownBlocked = "cooldown_blocked";

		// Token: 0x04008F7B RID: 36731
		public static string PropWaitingBlocked = "waiting_blocked";

		// Token: 0x04008F7C RID: 36732
		public static string PropEnabled = "enabled";

		// Token: 0x04008F7D RID: 36733
		public static string PropSingleDayUse = "single_day";

		// Token: 0x04008F7E RID: 36734
		public static string PropRandomGroup = "random_group";

		// Token: 0x04008F7F RID: 36735
		public static string PropReplaces = "replaces";

		// Token: 0x04008F80 RID: 36736
		public static string PropShowInActionList = "show_in_action_list";

		// Token: 0x04008F81 RID: 36737
		public static string PropPlayBitSound = "play_bit_sound";

		// Token: 0x04008F82 RID: 36738
		public static string PropSpecialRequirement = "special_requirement";

		// Token: 0x04008F83 RID: 36739
		public static string PropTwitchNotify = "twitch_notify";

		// Token: 0x04008F84 RID: 36740
		public static string PropDelayNotify = "delay_notify";

		// Token: 0x04008F85 RID: 36741
		public static string PropHideOnDisable = "hide_on_disable";

		// Token: 0x04008F86 RID: 36742
		public static string PropOnlyUsableByType = "only_usable_type";

		// Token: 0x04008F87 RID: 36743
		public static string PropOnlyUsableBy = "only_usable_by";

		// Token: 0x04008F88 RID: 36744
		public static string PropIgnoreCooldown = "ignore_cooldown";

		// Token: 0x04008F89 RID: 36745
		public static string PropIgnoreDiscount = "ignore_discount";

		// Token: 0x04008F8A RID: 36746
		public static string PropPointTypes = "point_type";

		// Token: 0x04008F8B RID: 36747
		public static string PropPresets = "presets";

		// Token: 0x04008F8C RID: 36748
		public static string PropOnlyShowInPreset = "only_show_in_preset";

		// Token: 0x04008F8D RID: 36749
		public static string PropVoteCooldownAddition = "vote_cooldown_add";

		// Token: 0x04008F8E RID: 36750
		public static string PropStreamerOnly = "streamer_only";

		// Token: 0x04008F8F RID: 36751
		public static string PropMinRespawnCount = "min_respawn_count";

		// Token: 0x04008F90 RID: 36752
		public static string PropMaxRespawnCount = "max_respawn_count";

		// Token: 0x04008F91 RID: 36753
		public static string PropRespawnCountType = "respawn_count_type";

		// Token: 0x04008F92 RID: 36754
		public static string PropRespawnThreshold = "respawn_threshold";

		// Token: 0x04008F93 RID: 36755
		public static HashSet<string> ExtendsExcludes = new HashSet<string>
		{
			TwitchAction.PropShowInActionList,
			TwitchAction.PropCommandKey,
			TwitchAction.PropCommand,
			TwitchAction.PropTitleKey,
			TwitchAction.PropDescriptionKey
		};

		// Token: 0x04008F94 RID: 36756
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

		// Token: 0x02001821 RID: 6177
		public enum SpecialRequirements
		{
			// Token: 0x04008F96 RID: 36758
			None,
			// Token: 0x04008F97 RID: 36759
			HasSpawnedEntities,
			// Token: 0x04008F98 RID: 36760
			NoSpawnedEntities,
			// Token: 0x04008F99 RID: 36761
			Bloodmoon,
			// Token: 0x04008F9A RID: 36762
			NotBloodmoon,
			// Token: 0x04008F9B RID: 36763
			NotBloodmoonDay,
			// Token: 0x04008F9C RID: 36764
			EarlyDay,
			// Token: 0x04008F9D RID: 36765
			Daytime,
			// Token: 0x04008F9E RID: 36766
			Night,
			// Token: 0x04008F9F RID: 36767
			NotAllDaySetting,
			// Token: 0x04008FA0 RID: 36768
			IsCooldown,
			// Token: 0x04008FA1 RID: 36769
			InLandClaim,
			// Token: 0x04008FA2 RID: 36770
			NotInLandClaim,
			// Token: 0x04008FA3 RID: 36771
			Safe,
			// Token: 0x04008FA4 RID: 36772
			NotSafe,
			// Token: 0x04008FA5 RID: 36773
			NoFullProgression,
			// Token: 0x04008FA6 RID: 36774
			NotOnVehicle,
			// Token: 0x04008FA7 RID: 36775
			NotInTrader,
			// Token: 0x04008FA8 RID: 36776
			Encumbrance,
			// Token: 0x04008FA9 RID: 36777
			WeatherGracePeriod,
			// Token: 0x04008FAA RID: 36778
			NotOnQuest,
			// Token: 0x04008FAB RID: 36779
			OnQuest
		}

		// Token: 0x02001822 RID: 6178
		public enum OnlyUsableTypes
		{
			// Token: 0x04008FAD RID: 36781
			Everyone,
			// Token: 0x04008FAE RID: 36782
			Broadcaster,
			// Token: 0x04008FAF RID: 36783
			Mods,
			// Token: 0x04008FB0 RID: 36784
			VIPs,
			// Token: 0x04008FB1 RID: 36785
			Subs,
			// Token: 0x04008FB2 RID: 36786
			Name
		}

		// Token: 0x02001823 RID: 6179
		public enum PointTypes
		{
			// Token: 0x04008FB4 RID: 36788
			PP,
			// Token: 0x04008FB5 RID: 36789
			SP,
			// Token: 0x04008FB6 RID: 36790
			Bits
		}

		// Token: 0x02001824 RID: 6180
		public enum RespawnCountTypes
		{
			// Token: 0x04008FB8 RID: 36792
			None,
			// Token: 0x04008FB9 RID: 36793
			Both,
			// Token: 0x04008FBA RID: 36794
			BlocksOnly,
			// Token: 0x04008FBB RID: 36795
			SpawnsOnly
		}
	}
}
