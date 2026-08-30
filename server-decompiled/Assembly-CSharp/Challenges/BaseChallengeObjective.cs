using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018F7 RID: 6391
	[Preserve]
	public class BaseChallengeObjective
	{
		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x0600C4FB RID: 50427 RVA: 0x00489B6A File Offset: 0x00487D6A
		// (set) Token: 0x0600C4FC RID: 50428 RVA: 0x00489B72 File Offset: 0x00487D72
		public byte CurrentFileVersion { get; set; }

		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x0600C4FD RID: 50429 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Invalid;
			}
		}

		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x0600C4FE RID: 50430 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual ChallengeClass.UINavTypes NavType
		{
			get
			{
				return ChallengeClass.UINavTypes.None;
			}
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x0600C4FF RID: 50431 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool NeedsConstantUIUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x0600C500 RID: 50432 RVA: 0x00489B7B File Offset: 0x00487D7B
		// (set) Token: 0x0600C501 RID: 50433 RVA: 0x00489B83 File Offset: 0x00487D83
		public bool Complete
		{
			get
			{
				return this.complete;
			}
			set
			{
				if (this.complete != value)
				{
					this.complete = value;
					this.HandleValueChanged();
				}
			}
		}

		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x0600C502 RID: 50434 RVA: 0x00489B9B File Offset: 0x00487D9B
		// (set) Token: 0x0600C503 RID: 50435 RVA: 0x00489BA3 File Offset: 0x00487DA3
		public int Current
		{
			get
			{
				return this.current;
			}
			set
			{
				if (this.current != value)
				{
					this.current = value;
					this.HandleValueChanged();
				}
			}
		}

		// Token: 0x0600C504 RID: 50436 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void BaseInit()
		{
		}

		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x0600C505 RID: 50437 RVA: 0x00489BBB File Offset: 0x00487DBB
		public EntityPlayerLocal Player
		{
			get
			{
				return this.Owner.Owner.Player;
			}
		}

		// Token: 0x0600C506 RID: 50438 RVA: 0x00489BCD File Offset: 0x00487DCD
		public void ResetComplete()
		{
			this.Complete = false;
			this.current = 0;
		}

		// Token: 0x14000118 RID: 280
		// (add) Token: 0x0600C507 RID: 50439 RVA: 0x00489BE0 File Offset: 0x00487DE0
		// (remove) Token: 0x0600C508 RID: 50440 RVA: 0x00489C18 File Offset: 0x00487E18
		public event ObjectiveValueChanged ValueChanged;

		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x0600C509 RID: 50441 RVA: 0x00489C4D File Offset: 0x00487E4D
		public string ObjectiveText
		{
			get
			{
				return string.Format("{0} {1}", this.DescriptionText, this.StatusText);
			}
		}

		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x0600C50A RID: 50442 RVA: 0x00032147 File Offset: 0x00030347
		public virtual string DescriptionText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x0600C50B RID: 50443 RVA: 0x00489C65 File Offset: 0x00487E65
		public virtual string StatusText
		{
			get
			{
				if (this.Owner != null && this.Owner.AutoCompleted)
				{
					return "--";
				}
				return string.Format("{0}/{1}", this.current, this.MaxCount);
			}
		}

		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x0600C50C RID: 50444 RVA: 0x00489CA2 File Offset: 0x00487EA2
		public virtual float FillAmount
		{
			get
			{
				return (float)this.current / (float)this.MaxCount;
			}
		}

		// Token: 0x0600C50D RID: 50445 RVA: 0x00489CB3 File Offset: 0x00487EB3
		[PublicizedFrom(EAccessModifier.Protected)]
		public void HandleValueChanged()
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged();
			}
		}

		// Token: 0x0600C50E RID: 50446 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init()
		{
		}

		// Token: 0x0600C50F RID: 50447 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleOnCreated()
		{
		}

		// Token: 0x0600C510 RID: 50448 RVA: 0x00489CC8 File Offset: 0x00487EC8
		public virtual bool HandleCheckStatus()
		{
			this.Complete = this.CheckObjectiveComplete(false);
			return this.Complete;
		}

		// Token: 0x0600C511 RID: 50449 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UpdateStatus()
		{
		}

		// Token: 0x0600C512 RID: 50450 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleAddHooks()
		{
		}

		// Token: 0x0600C513 RID: 50451 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleRemoveHooks()
		{
		}

		// Token: 0x0600C514 RID: 50452 RVA: 0x00489CDD File Offset: 0x00487EDD
		public virtual void HandleTrackingStarted()
		{
			this.IsTracking = true;
		}

		// Token: 0x0600C515 RID: 50453 RVA: 0x00489CE8 File Offset: 0x00487EE8
		public virtual void CopyValues(BaseChallengeObjective obj, BaseChallengeObjective objFromClass)
		{
			this.current = obj.current;
			this.MaxCount = objFromClass.MaxCount;
			this.ShowRequirements = objFromClass.ShowRequirements;
			this.Biome = objFromClass.Biome;
			this.complete = this.Current >= this.MaxCount;
		}

		// Token: 0x0600C516 RID: 50454 RVA: 0x00489D3C File Offset: 0x00487F3C
		public virtual void HandleTrackingEnded()
		{
			this.IsTracking = false;
		}

		// Token: 0x0600C517 RID: 50455 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleUpdatingCurrent()
		{
		}

		// Token: 0x0600C518 RID: 50456 RVA: 0x00489D45 File Offset: 0x00487F45
		public void HandleAutoComplete()
		{
			this.current = this.MaxCount;
			this.Complete = true;
		}

		// Token: 0x0600C519 RID: 50457 RVA: 0x00489D5C File Offset: 0x00487F5C
		public static BaseChallengeObjective ReadObjective(byte _currentVersion, ChallengeObjectiveType _type, BinaryReader _br)
		{
			BaseChallengeObjective baseChallengeObjective = null;
			switch (_type)
			{
			case ChallengeObjectiveType.BlockPlace:
				baseChallengeObjective = new ChallengeObjectiveBlockPlace();
				break;
			case ChallengeObjectiveType.BlockUpgrade:
				baseChallengeObjective = new ChallengeObjectiveBlockUpgrade();
				break;
			case ChallengeObjectiveType.Bloodmoon:
				baseChallengeObjective = new ChallengeObjectiveBloodmoon();
				break;
			case ChallengeObjectiveType.Craft:
				baseChallengeObjective = new ChallengeObjectiveCraft();
				break;
			case ChallengeObjectiveType.CureDebuff:
				baseChallengeObjective = new ChallengeObjectiveCureDebuff();
				break;
			case ChallengeObjectiveType.EnterBiome:
				baseChallengeObjective = new ChallengeObjectiveEnterBiome();
				break;
			case ChallengeObjectiveType.Gather:
				baseChallengeObjective = new ChallengeObjectiveGather();
				break;
			case ChallengeObjectiveType.GatherIngredient:
				baseChallengeObjective = new ChallengeObjectiveGatherIngredient();
				break;
			case ChallengeObjectiveType.Harvest:
				baseChallengeObjective = new ChallengeObjectiveHarvest();
				break;
			case ChallengeObjectiveType.Hold:
				baseChallengeObjective = new ChallengeObjectiveHold();
				break;
			case ChallengeObjectiveType.Kill:
				baseChallengeObjective = new ChallengeObjectiveKill();
				break;
			case ChallengeObjectiveType.QuestComplete:
				baseChallengeObjective = new ChallengeObjectiveQuestComplete();
				break;
			case ChallengeObjectiveType.Scrap:
				baseChallengeObjective = new ChallengeObjectiveScrap();
				break;
			case ChallengeObjectiveType.Survive:
				baseChallengeObjective = new ChallengeObjectiveSurvive();
				break;
			case ChallengeObjectiveType.Trader:
				baseChallengeObjective = new ChallengeObjectiveTrader();
				break;
			case ChallengeObjectiveType.Wear:
				baseChallengeObjective = new ChallengeObjectiveWear();
				break;
			case ChallengeObjectiveType.Use:
				baseChallengeObjective = new ChallengeObjectiveUseItem();
				break;
			case ChallengeObjectiveType.ChallengeComplete:
				baseChallengeObjective = new ChallengeObjectiveChallengeComplete();
				break;
			case ChallengeObjectiveType.MeetTrader:
				baseChallengeObjective = new ChallengeObjectiveMeetTrader();
				break;
			case ChallengeObjectiveType.KillByTag:
				baseChallengeObjective = new ChallengeObjectiveKillByTag();
				break;
			case ChallengeObjectiveType.ChallengeStatAwarded:
				baseChallengeObjective = new ChallengeObjectiveChallengeStatAwarded();
				break;
			case ChallengeObjectiveType.SpendSkillPoint:
				baseChallengeObjective = new ChallengeObjectiveSpendSkillPoint();
				break;
			case ChallengeObjectiveType.Twitch:
				baseChallengeObjective = new ChallengeObjectiveTwitch();
				break;
			case ChallengeObjectiveType.Time:
				baseChallengeObjective = new ChallengeObjectiveTime();
				break;
			case ChallengeObjectiveType.GatherByTag:
				baseChallengeObjective = new ChallengeObjectiveGatherByTag();
				break;
			case ChallengeObjectiveType.LootContainer:
				baseChallengeObjective = new ChallengeObjectiveLootContainer();
				break;
			}
			if (baseChallengeObjective != null)
			{
				baseChallengeObjective.Read(_currentVersion, _br);
			}
			return baseChallengeObjective;
		}

		// Token: 0x0600C51A RID: 50458 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual Recipe GetRecipeItem()
		{
			return null;
		}

		// Token: 0x0600C51B RID: 50459 RVA: 0x00489EDC File Offset: 0x004880DC
		public virtual Recipe[] GetRecipeItems()
		{
			if (this.Owner.NeedsPreRequisites)
			{
				Recipe recipeFromRequirements = this.Owner.GetRecipeFromRequirements();
				if (recipeFromRequirements != null)
				{
					return new Recipe[] { recipeFromRequirements };
				}
			}
			return null;
		}

		// Token: 0x0600C51C RID: 50460 RVA: 0x00489F11 File Offset: 0x00488111
		public virtual void Read(byte _currentVersion, BinaryReader _br)
		{
			this.current = _br.ReadInt32();
		}

		// Token: 0x0600C51D RID: 50461 RVA: 0x00489F1F File Offset: 0x0048811F
		public void WriteObjective(BinaryWriter _bw)
		{
			_bw.Write((byte)this.ObjectiveType);
			this.Write(_bw);
		}

		// Token: 0x0600C51E RID: 50462 RVA: 0x00489F34 File Offset: 0x00488134
		public virtual void Write(BinaryWriter _bw)
		{
			_bw.Write(this.current);
		}

		// Token: 0x0600C51F RID: 50463 RVA: 0x00489F44 File Offset: 0x00488144
		public virtual bool CheckObjectiveComplete(bool handleComplete = true)
		{
			this.HandleUpdatingCurrent();
			if (this.Current >= this.MaxCount)
			{
				this.Current = this.MaxCount;
				this.Complete = true;
				if (handleComplete)
				{
					this.Owner.HandleComplete(true, false);
				}
				return true;
			}
			if (handleComplete)
			{
				this.Owner.HandleComplete(true, false);
			}
			this.Complete = false;
			return false;
		}

		// Token: 0x0600C520 RID: 50464 RVA: 0x00489FA4 File Offset: 0x004881A4
		public virtual bool CheckBaseRequirements()
		{
			return this.Owner.ChallengeGroup.IsActive && (this.Biome != "" && (this.Owner.Owner.Player.biomeStandingOn == null || this.Owner.Owner.Player.biomeStandingOn.m_sBiomeName != this.Biome));
		}

		// Token: 0x0600C521 RID: 50465 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual BaseChallengeObjective Clone()
		{
			return null;
		}

		// Token: 0x0600C522 RID: 50466 RVA: 0x0048A018 File Offset: 0x00488218
		public virtual void ParseElement(XElement e)
		{
			if (e.HasAttribute("count"))
			{
				this.MaxCount = StringParsers.ParseSInt32(e.GetAttribute("count"), 0, -1, NumberStyles.Integer);
			}
			if (e.HasAttribute("show_requirements"))
			{
				this.ShowRequirements = StringParsers.ParseBool(e.GetAttribute("show_requirements"), 0, -1, true);
			}
			if (e.HasAttribute("biome"))
			{
				this.Biome = e.GetAttribute("biome");
			}
		}

		// Token: 0x0600C523 RID: 50467 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void Update(float deltaTime)
		{
		}

		// Token: 0x0600C524 RID: 50468 RVA: 0x0048A0AD File Offset: 0x004882AD
		public void HandleUpdate(float deltaTime)
		{
			if (this.Owner.IsActive)
			{
				this.Update(deltaTime);
			}
		}

		// Token: 0x0600C525 RID: 50469 RVA: 0x0048A0C3 File Offset: 0x004882C3
		public virtual void CompleteObjective(bool handleComplete = true)
		{
			this.Current = this.MaxCount;
			this.CheckObjectiveComplete(handleComplete);
		}

		// Token: 0x04009511 RID: 38161
		public static byte FileVersion = 1;

		// Token: 0x04009513 RID: 38163
		public int MaxCount = 1;

		// Token: 0x04009514 RID: 38164
		public bool IsRequirement;

		// Token: 0x04009515 RID: 38165
		public Challenge Owner;

		// Token: 0x04009516 RID: 38166
		public ChallengeClass OwnerClass;

		// Token: 0x04009517 RID: 38167
		public string Biome = "";

		// Token: 0x04009518 RID: 38168
		public bool ShowRequirements = true;

		// Token: 0x04009519 RID: 38169
		[PublicizedFrom(EAccessModifier.Private)]
		public bool complete;

		// Token: 0x0400951A RID: 38170
		public bool IsTracking;

		// Token: 0x0400951B RID: 38171
		[PublicizedFrom(EAccessModifier.Protected)]
		public int current;
	}
}
