using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Twitch
{
	// Token: 0x020017CE RID: 6094
	public class TwitchAction
	{
		// Token: 0x0600BC75 RID: 48245 RVA: 0x0045E3DD File Offset: 0x0045C5DD
		public bool HasModifiedPrice()
		{
			return this.DefaultCost != this.ModifiedCost;
		}

		// Token: 0x0600BC76 RID: 48246 RVA: 0x0045E3F0 File Offset: 0x0045C5F0
		public bool HasExtraConditions()
		{
			return ((!this.SingleDayUse && !this.RandomDaily) || this.AllowedDay != -1) && !this.OnCooldown && this.OnlyUsableByType == TwitchAction.OnlyUsableTypes.Everyone;
		}

		// Token: 0x0600BC77 RID: 48247 RVA: 0x0045E420 File Offset: 0x0045C620
		public static int GetAdjustedBitPriceCeil(int price)
		{
			int num = TwitchAction.bitPrices.Find((int p) => p >= price);
			if (num == 0)
			{
				return TwitchAction.bitPrices[TwitchAction.bitPrices.Count - 1];
			}
			return num;
		}

		// Token: 0x0600BC78 RID: 48248 RVA: 0x0045E46C File Offset: 0x0045C66C
		public static int GetAdjustedBitPriceFloor(int price)
		{
			return TwitchAction.bitPrices.FindLast((int p) => p <= price);
		}

		// Token: 0x0600BC79 RID: 48249 RVA: 0x0045E49C File Offset: 0x0045C69C
		public static int GetAdjustedBitPriceFloorNoZero(int price)
		{
			return Mathf.Max(TwitchAction.GetAdjustedBitPriceFloor(price), TwitchAction.bitPrices[0]);
		}

		// Token: 0x0600BC7A RID: 48250 RVA: 0x0045E4B4 File Offset: 0x0045C6B4
		public int GetModifiedDiscountCost()
		{
			return TwitchAction.GetAdjustedBitPriceFloorNoZero((int)((float)this.ModifiedCost * TwitchManager.Current.BitPriceMultiplier));
		}

		// Token: 0x0600BC7B RID: 48251 RVA: 0x0045E4D0 File Offset: 0x0045C6D0
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

		// Token: 0x0600BC7C RID: 48252 RVA: 0x0045E53C File Offset: 0x0045C73C
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

		// Token: 0x0600BC7D RID: 48253 RVA: 0x0045E5AA File Offset: 0x0045C7AA
		public void ResetToDefaultCost()
		{
			if (this.PointType == TwitchAction.PointTypes.Bits)
			{
				this.ModifiedCost = TwitchAction.GetAdjustedBitPriceFloorNoZero(this.DefaultCost);
				return;
			}
			this.ModifiedCost = this.DefaultCost;
		}

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x0600BC7E RID: 48254 RVA: 0x0045E5D3 File Offset: 0x0045C7D3
		public bool CanUse
		{
			get
			{
				return this.Enabled && this.CheckAllowed();
			}
		}

		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x0600BC7F RID: 48255 RVA: 0x0045E5E5 File Offset: 0x0045C7E5
		public bool RandomDaily
		{
			get
			{
				return this.RandomGroup != "";
			}
		}

		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x0600BC80 RID: 48256 RVA: 0x0045E5F7 File Offset: 0x0045C7F7
		public bool SpecialOnly
		{
			get
			{
				return this.PointType == TwitchAction.PointTypes.SP;
			}
		}

		// Token: 0x0600BC81 RID: 48257 RVA: 0x0045E604 File Offset: 0x0045C804
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

		// Token: 0x0600BC82 RID: 48258 RVA: 0x0045E668 File Offset: 0x0045C868
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

		// Token: 0x0600BC83 RID: 48259 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600BC84 RID: 48260 RVA: 0x0045E6C5 File Offset: 0x0045C8C5
		public virtual TwitchActionEntry SetupActionEntry()
		{
			return new TwitchActionEntry();
		}

		// Token: 0x0600BC85 RID: 48261 RVA: 0x0045E6CC File Offset: 0x0045C8CC
		public bool IsInPreset(TwitchActionPreset preset)
		{
			return ((!preset.IsEmpty && this.PresetNames != null && this.PresetNames.Contains(preset.Name)) || preset.AddedActions.Contains(this.Name)) && !preset.RemovedActions.Contains(this.Name);
		}

		// Token: 0x0600BC86 RID: 48262 RVA: 0x0045E725 File Offset: 0x0045C925
		public bool IsInPresetForList(TwitchActionPreset preset)
		{
			return (!preset.IsEmpty && this.PresetNames != null && this.PresetNames.Contains(preset.Name)) || preset.AddedActions.Contains(this.Name);
		}

		// Token: 0x0600BC87 RID: 48263 RVA: 0x0045E75D File Offset: 0x0045C95D
		public bool IsInPresetDefault(TwitchActionPreset preset)
		{
			return !preset.IsEmpty && this.PresetNames != null && this.PresetNames.Contains(preset.Name);
		}

		// Token: 0x0600BC88 RID: 48264 RVA: 0x0045E784 File Offset: 0x0045C984
		public bool CanPerformAction(EntityPlayer target, TwitchActionEntry entry)
		{
			if (entry.Target == null)
			{
				entry.Target = target;
			}
			return this.OnPerformAction(target, entry);
		}

		// Token: 0x0600BC89 RID: 48265 RVA: 0x0045E7A8 File Offset: 0x0045C9A8
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

		// Token: 0x0600BC8A RID: 48266 RVA: 0x0045E8C4 File Offset: 0x0045CAC4
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

		// Token: 0x0600BC8B RID: 48267 RVA: 0x0045EE9C File Offset: 0x0045D09C
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

		// Token: 0x0600BC8C RID: 48268 RVA: 0x0045EF09 File Offset: 0x0045D109
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

		// Token: 0x0600BC8D RID: 48269 RVA: 0x0045EF38 File Offset: 0x0045D138
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

		// Token: 0x0600BC8E RID: 48270 RVA: 0x0045EF90 File Offset: 0x0045D190
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

		// Token: 0x0600BC8F RID: 48271 RVA: 0x0045F337 File Offset: 0x0045D537
		public void UpdateModifiedCooldown(float modifier)
		{
			this.ModifiedCooldown = modifier * this.Cooldown;
		}

		// Token: 0x0600BC90 RID: 48272 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool OnPerformAction(EntityPlayer Target, TwitchActionEntry entry)
		{
			return false;
		}

		// Token: 0x0600BC91 RID: 48273 RVA: 0x0045F347 File Offset: 0x0045D547
		public void AddCooldownAddition(TwitchActionCooldownAddition newCooldown)
		{
			if (this.CooldownAdditions == null)
			{
				this.CooldownAdditions = new List<TwitchActionCooldownAddition>();
			}
			this.CooldownAdditions.Add(newCooldown);
		}

		// Token: 0x0600BC92 RID: 48274 RVA: 0x0045F368 File Offset: 0x0045D568
		public void ResetCooldown(float currentUnityTime)
		{
			this.lastUse = currentUnityTime - this.ModifiedCooldown;
			this.tempCooldown = 0f;
			this.tempCooldownSet = 0f;
		}

		// Token: 0x0600BC93 RID: 48275 RVA: 0x0045F38E File Offset: 0x0045D58E
		public void SetCooldown(float currentUnityTime, float newCooldownTime)
		{
			this.lastUse = currentUnityTime - this.ModifiedCooldown;
			this.tempCooldown = newCooldownTime;
			this.tempCooldownSet = currentUnityTime;
		}

		// Token: 0x04008D52 RID: 36178
		public string Name = "";

		// Token: 0x04008D53 RID: 36179
		public string Title = "";

		// Token: 0x04008D54 RID: 36180
		public string BaseCommand = "";

		// Token: 0x04008D55 RID: 36181
		public string Command = "";

		// Token: 0x04008D56 RID: 36182
		public string EventName = "";

		// Token: 0x04008D57 RID: 36183
		public List<string> CategoryNames = new List<string>();

		// Token: 0x04008D58 RID: 36184
		public int DefaultCost = 5;

		// Token: 0x04008D59 RID: 36185
		public int ModifiedCost = 5;

		// Token: 0x04008D5A RID: 36186
		public int CurrentCost = 5;

		// Token: 0x04008D5B RID: 36187
		public int StartGameStage;

		// Token: 0x04008D5C RID: 36188
		public string Description;

		// Token: 0x04008D5D RID: 36189
		public float OriginalCooldown;

		// Token: 0x04008D5E RID: 36190
		public float Cooldown;

		// Token: 0x04008D5F RID: 36191
		public bool IsPositive;

		// Token: 0x04008D60 RID: 36192
		public bool AddsToCooldown;

		// Token: 0x04008D61 RID: 36193
		public int CooldownAddAmount = -1;

		// Token: 0x04008D62 RID: 36194
		public bool CooldownBlocked;

		// Token: 0x04008D63 RID: 36195
		public bool WaitingBlocked;

		// Token: 0x04008D64 RID: 36196
		public bool OnCooldown;

		// Token: 0x04008D65 RID: 36197
		public bool Enabled = true;

		// Token: 0x04008D66 RID: 36198
		public bool SingleDayUse;

		// Token: 0x04008D67 RID: 36199
		public bool DelayNotify;

		// Token: 0x04008D68 RID: 36200
		public bool TwitchNotify = true;

		// Token: 0x04008D69 RID: 36201
		public bool PlayBitSound = true;

		// Token: 0x04008D6A RID: 36202
		public string RandomGroup = "";

		// Token: 0x04008D6B RID: 36203
		public string Replaces = "";

		// Token: 0x04008D6C RID: 36204
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUse;

		// Token: 0x04008D6D RID: 36205
		public int AllowedDay = -1;

		// Token: 0x04008D6E RID: 36206
		public int groupIndex;

		// Token: 0x04008D6F RID: 36207
		public float tempCooldownSet;

		// Token: 0x04008D70 RID: 36208
		public float tempCooldown;

		// Token: 0x04008D71 RID: 36209
		public bool IgnoreCooldown;

		// Token: 0x04008D72 RID: 36210
		public bool IgnoreDiscount;

		// Token: 0x04008D73 RID: 36211
		public float ModifiedCooldown;

		// Token: 0x04008D74 RID: 36212
		public List<TwitchActionCooldownAddition> CooldownAdditions;

		// Token: 0x04008D75 RID: 36213
		public bool ShowInActionList = true;

		// Token: 0x04008D76 RID: 36214
		public List<TwitchAction.SpecialRequirements> SpecialRequirementList;

		// Token: 0x04008D77 RID: 36215
		public List<BaseTwitchRequirement> TwitchRequirements;

		// Token: 0x04008D78 RID: 36216
		public bool HideOnDisable;

		// Token: 0x04008D79 RID: 36217
		public TwitchAction.OnlyUsableTypes OnlyUsableByType;

		// Token: 0x04008D7A RID: 36218
		public string[] OnlyUsableBy;

		// Token: 0x04008D7B RID: 36219
		public bool OriginalEnabled = true;

		// Token: 0x04008D7C RID: 36220
		public TwitchAction.PointTypes PointType;

		// Token: 0x04008D7D RID: 36221
		public TwitchActionManager.ActionCategory MainCategory;

		// Token: 0x04008D7E RID: 36222
		public TwitchActionManager.ActionCategory DisplayCategory;

		// Token: 0x04008D7F RID: 36223
		public List<string> PresetNames;

		// Token: 0x04008D80 RID: 36224
		public bool OnlyShowInPreset;

		// Token: 0x04008D81 RID: 36225
		public float VoteCooldownAddition;

		// Token: 0x04008D82 RID: 36226
		public bool StreamerOnly;

		// Token: 0x04008D83 RID: 36227
		public TwitchAction.RespawnCountTypes RespawnCountType;

		// Token: 0x04008D84 RID: 36228
		public int MinRespawnCount = -1;

		// Token: 0x04008D85 RID: 36229
		public int MaxRespawnCount = -1;

		// Token: 0x04008D86 RID: 36230
		public int RespawnThreshold;

		// Token: 0x04008D87 RID: 36231
		public DynamicProperties Properties;

		// Token: 0x04008D88 RID: 36232
		public static string PropCommand = "command";

		// Token: 0x04008D89 RID: 36233
		public static string PropCommandKey = "command_key";

		// Token: 0x04008D8A RID: 36234
		public static string PropTitle = "title";

		// Token: 0x04008D8B RID: 36235
		public static string PropTitleKey = "title_key";

		// Token: 0x04008D8C RID: 36236
		public static string PropCategory = "category";

		// Token: 0x04008D8D RID: 36237
		public static string PropDisplayCategory = "display_category";

		// Token: 0x04008D8E RID: 36238
		public static string PropEventName = "event_name";

		// Token: 0x04008D8F RID: 36239
		public static string PropDefaultCost = "default_cost";

		// Token: 0x04008D90 RID: 36240
		public static string PropDescription = "description";

		// Token: 0x04008D91 RID: 36241
		public static string PropDescriptionKey = "description_key";

		// Token: 0x04008D92 RID: 36242
		public static string PropStartGameStage = "start_gamestage";

		// Token: 0x04008D93 RID: 36243
		public static string PropCooldown = "cooldown";

		// Token: 0x04008D94 RID: 36244
		public static string PropIsPositive = "is_positive";

		// Token: 0x04008D95 RID: 36245
		public static string PropSpecialOnly = "special_only";

		// Token: 0x04008D96 RID: 36246
		public static string PropAddCooldown = "add_cooldown";

		// Token: 0x04008D97 RID: 36247
		public static string PropCooldownAddAmount = "cooldown_add_amount";

		// Token: 0x04008D98 RID: 36248
		public static string PropCooldownBlocked = "cooldown_blocked";

		// Token: 0x04008D99 RID: 36249
		public static string PropWaitingBlocked = "waiting_blocked";

		// Token: 0x04008D9A RID: 36250
		public static string PropEnabled = "enabled";

		// Token: 0x04008D9B RID: 36251
		public static string PropSingleDayUse = "single_day";

		// Token: 0x04008D9C RID: 36252
		public static string PropRandomGroup = "random_group";

		// Token: 0x04008D9D RID: 36253
		public static string PropReplaces = "replaces";

		// Token: 0x04008D9E RID: 36254
		public static string PropShowInActionList = "show_in_action_list";

		// Token: 0x04008D9F RID: 36255
		public static string PropPlayBitSound = "play_bit_sound";

		// Token: 0x04008DA0 RID: 36256
		public static string PropSpecialRequirement = "special_requirement";

		// Token: 0x04008DA1 RID: 36257
		public static string PropTwitchNotify = "twitch_notify";

		// Token: 0x04008DA2 RID: 36258
		public static string PropDelayNotify = "delay_notify";

		// Token: 0x04008DA3 RID: 36259
		public static string PropHideOnDisable = "hide_on_disable";

		// Token: 0x04008DA4 RID: 36260
		public static string PropOnlyUsableByType = "only_usable_type";

		// Token: 0x04008DA5 RID: 36261
		public static string PropOnlyUsableBy = "only_usable_by";

		// Token: 0x04008DA6 RID: 36262
		public static string PropIgnoreCooldown = "ignore_cooldown";

		// Token: 0x04008DA7 RID: 36263
		public static string PropIgnoreDiscount = "ignore_discount";

		// Token: 0x04008DA8 RID: 36264
		public static string PropPointTypes = "point_type";

		// Token: 0x04008DA9 RID: 36265
		public static string PropPresets = "presets";

		// Token: 0x04008DAA RID: 36266
		public static string PropOnlyShowInPreset = "only_show_in_preset";

		// Token: 0x04008DAB RID: 36267
		public static string PropVoteCooldownAddition = "vote_cooldown_add";

		// Token: 0x04008DAC RID: 36268
		public static string PropStreamerOnly = "streamer_only";

		// Token: 0x04008DAD RID: 36269
		public static string PropMinRespawnCount = "min_respawn_count";

		// Token: 0x04008DAE RID: 36270
		public static string PropMaxRespawnCount = "max_respawn_count";

		// Token: 0x04008DAF RID: 36271
		public static string PropRespawnCountType = "respawn_count_type";

		// Token: 0x04008DB0 RID: 36272
		public static string PropRespawnThreshold = "respawn_threshold";

		// Token: 0x04008DB1 RID: 36273
		public static HashSet<string> ExtendsExcludes = new HashSet<string>
		{
			TwitchAction.PropShowInActionList,
			TwitchAction.PropCommandKey,
			TwitchAction.PropCommand,
			TwitchAction.PropTitleKey,
			TwitchAction.PropDescriptionKey
		};

		// Token: 0x04008DB2 RID: 36274
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

		// Token: 0x020017CF RID: 6095
		public enum SpecialRequirements
		{
			// Token: 0x04008DB4 RID: 36276
			None,
			// Token: 0x04008DB5 RID: 36277
			HasSpawnedEntities,
			// Token: 0x04008DB6 RID: 36278
			NoSpawnedEntities,
			// Token: 0x04008DB7 RID: 36279
			Bloodmoon,
			// Token: 0x04008DB8 RID: 36280
			NotBloodmoon,
			// Token: 0x04008DB9 RID: 36281
			NotBloodmoonDay,
			// Token: 0x04008DBA RID: 36282
			EarlyDay,
			// Token: 0x04008DBB RID: 36283
			Daytime,
			// Token: 0x04008DBC RID: 36284
			Night,
			// Token: 0x04008DBD RID: 36285
			NotAllDaySetting,
			// Token: 0x04008DBE RID: 36286
			IsCooldown,
			// Token: 0x04008DBF RID: 36287
			InLandClaim,
			// Token: 0x04008DC0 RID: 36288
			NotInLandClaim,
			// Token: 0x04008DC1 RID: 36289
			Safe,
			// Token: 0x04008DC2 RID: 36290
			NotSafe,
			// Token: 0x04008DC3 RID: 36291
			NoFullProgression,
			// Token: 0x04008DC4 RID: 36292
			NotOnVehicle,
			// Token: 0x04008DC5 RID: 36293
			NotInTrader,
			// Token: 0x04008DC6 RID: 36294
			Encumbrance,
			// Token: 0x04008DC7 RID: 36295
			WeatherGracePeriod,
			// Token: 0x04008DC8 RID: 36296
			NotOnQuest,
			// Token: 0x04008DC9 RID: 36297
			OnQuest
		}

		// Token: 0x020017D0 RID: 6096
		public enum OnlyUsableTypes
		{
			// Token: 0x04008DCB RID: 36299
			Everyone,
			// Token: 0x04008DCC RID: 36300
			Broadcaster,
			// Token: 0x04008DCD RID: 36301
			Mods,
			// Token: 0x04008DCE RID: 36302
			VIPs,
			// Token: 0x04008DCF RID: 36303
			Subs,
			// Token: 0x04008DD0 RID: 36304
			Name
		}

		// Token: 0x020017D1 RID: 6097
		public enum PointTypes
		{
			// Token: 0x04008DD2 RID: 36306
			PP,
			// Token: 0x04008DD3 RID: 36307
			SP,
			// Token: 0x04008DD4 RID: 36308
			Bits
		}

		// Token: 0x020017D2 RID: 6098
		public enum RespawnCountTypes
		{
			// Token: 0x04008DD6 RID: 36310
			None,
			// Token: 0x04008DD7 RID: 36311
			Both,
			// Token: 0x04008DD8 RID: 36312
			BlocksOnly,
			// Token: 0x04008DD9 RID: 36313
			SpawnsOnly
		}
	}
}
