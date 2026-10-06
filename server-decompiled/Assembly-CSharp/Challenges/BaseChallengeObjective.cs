using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001923 RID: 6435
	[Preserve]
	public class BaseChallengeObjective
	{
		// Token: 0x170018EE RID: 6382
		// (get) Token: 0x0600C653 RID: 50771 RVA: 0x0048E05A File Offset: 0x0048C25A
		// (set) Token: 0x0600C654 RID: 50772 RVA: 0x0048E062 File Offset: 0x0048C262
		public byte CurrentFileVersion { get; set; }

		// Token: 0x170018EF RID: 6383
		// (get) Token: 0x0600C655 RID: 50773 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Invalid;
			}
		}

		// Token: 0x170018F0 RID: 6384
		// (get) Token: 0x0600C656 RID: 50774 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual ChallengeClass.UINavTypes NavType
		{
			get
			{
				return ChallengeClass.UINavTypes.None;
			}
		}

		// Token: 0x170018F1 RID: 6385
		// (get) Token: 0x0600C657 RID: 50775 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool NeedsConstantUIUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170018F2 RID: 6386
		// (get) Token: 0x0600C658 RID: 50776 RVA: 0x0048E06B File Offset: 0x0048C26B
		// (set) Token: 0x0600C659 RID: 50777 RVA: 0x0048E073 File Offset: 0x0048C273
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

		// Token: 0x170018F3 RID: 6387
		// (get) Token: 0x0600C65A RID: 50778 RVA: 0x0048E08B File Offset: 0x0048C28B
		// (set) Token: 0x0600C65B RID: 50779 RVA: 0x0048E093 File Offset: 0x0048C293
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

		// Token: 0x0600C65C RID: 50780 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void BaseInit()
		{
		}

		// Token: 0x170018F4 RID: 6388
		// (get) Token: 0x0600C65D RID: 50781 RVA: 0x0048E0AB File Offset: 0x0048C2AB
		public EntityPlayerLocal Player
		{
			get
			{
				return this.Owner.Owner.Player;
			}
		}

		// Token: 0x0600C65E RID: 50782 RVA: 0x0048E0BD File Offset: 0x0048C2BD
		public void ResetComplete()
		{
			this.Complete = false;
			this.current = 0;
		}

		// Token: 0x14000117 RID: 279
		// (add) Token: 0x0600C65F RID: 50783 RVA: 0x0048E0D0 File Offset: 0x0048C2D0
		// (remove) Token: 0x0600C660 RID: 50784 RVA: 0x0048E108 File Offset: 0x0048C308
		public event ObjectiveValueChanged ValueChanged;

		// Token: 0x170018F5 RID: 6389
		// (get) Token: 0x0600C661 RID: 50785 RVA: 0x0048E13D File Offset: 0x0048C33D
		public string ObjectiveText
		{
			get
			{
				return string.Format("{0} {1}", this.DescriptionText, this.StatusText);
			}
		}

		// Token: 0x170018F6 RID: 6390
		// (get) Token: 0x0600C662 RID: 50786 RVA: 0x0003223F File Offset: 0x0003043F
		public virtual string DescriptionText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170018F7 RID: 6391
		// (get) Token: 0x0600C663 RID: 50787 RVA: 0x0048E155 File Offset: 0x0048C355
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

		// Token: 0x170018F8 RID: 6392
		// (get) Token: 0x0600C664 RID: 50788 RVA: 0x0048E192 File Offset: 0x0048C392
		public virtual float FillAmount
		{
			get
			{
				return (float)this.current / (float)this.MaxCount;
			}
		}

		// Token: 0x0600C665 RID: 50789 RVA: 0x0048E1A3 File Offset: 0x0048C3A3
		[PublicizedFrom(EAccessModifier.Protected)]
		public void HandleValueChanged()
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged();
			}
		}

		// Token: 0x0600C666 RID: 50790 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init()
		{
		}

		// Token: 0x0600C667 RID: 50791 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleOnCreated()
		{
		}

		// Token: 0x0600C668 RID: 50792 RVA: 0x0048E1B8 File Offset: 0x0048C3B8
		public virtual bool HandleCheckStatus()
		{
			this.Complete = this.CheckObjectiveComplete(false);
			return this.Complete;
		}

		// Token: 0x0600C669 RID: 50793 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UpdateStatus()
		{
		}

		// Token: 0x0600C66A RID: 50794 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleAddHooks()
		{
		}

		// Token: 0x0600C66B RID: 50795 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleRemoveHooks()
		{
		}

		// Token: 0x0600C66C RID: 50796 RVA: 0x0048E1CD File Offset: 0x0048C3CD
		public virtual void HandleTrackingStarted()
		{
			this.IsTracking = true;
		}

		// Token: 0x0600C66D RID: 50797 RVA: 0x0048E1D8 File Offset: 0x0048C3D8
		public virtual void CopyValues(BaseChallengeObjective obj, BaseChallengeObjective objFromClass)
		{
			this.current = obj.current;
			this.MaxCount = objFromClass.MaxCount;
			this.ShowRequirements = objFromClass.ShowRequirements;
			this.Biome = objFromClass.Biome;
			this.complete = this.Current >= this.MaxCount;
		}

		// Token: 0x0600C66E RID: 50798 RVA: 0x0048E22C File Offset: 0x0048C42C
		public virtual void HandleTrackingEnded()
		{
			this.IsTracking = false;
		}

		// Token: 0x0600C66F RID: 50799 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleUpdatingCurrent()
		{
		}

		// Token: 0x0600C670 RID: 50800 RVA: 0x0048E235 File Offset: 0x0048C435
		public void HandleAutoComplete()
		{
			this.current = this.MaxCount;
			this.Complete = true;
		}

		// Token: 0x0600C671 RID: 50801 RVA: 0x0048E24C File Offset: 0x0048C44C
		public static BaseChallengeObjective ReadObjective(byte _currentVersion, ChallengeObjectiveType _type, PooledBinaryReader _br)
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

		// Token: 0x0600C672 RID: 50802 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual Recipe GetRecipeItem()
		{
			return null;
		}

		// Token: 0x0600C673 RID: 50803 RVA: 0x0048E3CC File Offset: 0x0048C5CC
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

		// Token: 0x0600C674 RID: 50804 RVA: 0x0048E401 File Offset: 0x0048C601
		public virtual void Read(byte _currentVersion, PooledBinaryReader _br)
		{
			this.current = _br.ReadInt32();
		}

		// Token: 0x0600C675 RID: 50805 RVA: 0x0048E40F File Offset: 0x0048C60F
		public void WriteObjective(PooledBinaryWriter _bw)
		{
			_bw.Write((byte)this.ObjectiveType);
			this.Write(_bw);
		}

		// Token: 0x0600C676 RID: 50806 RVA: 0x0048E424 File Offset: 0x0048C624
		public virtual void Write(PooledBinaryWriter _bw)
		{
			_bw.Write(this.current);
		}

		// Token: 0x0600C677 RID: 50807 RVA: 0x0048E434 File Offset: 0x0048C634
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

		// Token: 0x0600C678 RID: 50808 RVA: 0x0048E494 File Offset: 0x0048C694
		public virtual bool CheckBaseRequirements()
		{
			return this.Owner.ChallengeGroup.IsActive && (this.Biome != "" && (this.Owner.Owner.Player.biomeStandingOn == null || this.Owner.Owner.Player.biomeStandingOn.m_sBiomeName != this.Biome));
		}

		// Token: 0x0600C679 RID: 50809 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual BaseChallengeObjective Clone()
		{
			return null;
		}

		// Token: 0x0600C67A RID: 50810 RVA: 0x0048E508 File Offset: 0x0048C708
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

		// Token: 0x0600C67B RID: 50811 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void Update(float deltaTime)
		{
		}

		// Token: 0x0600C67C RID: 50812 RVA: 0x0048E59D File Offset: 0x0048C79D
		public void HandleUpdate(float deltaTime)
		{
			if (this.Owner.IsActive)
			{
				this.Update(deltaTime);
			}
		}

		// Token: 0x0600C67D RID: 50813 RVA: 0x0048E5B3 File Offset: 0x0048C7B3
		public virtual void CompleteObjective(bool handleComplete = true)
		{
			this.Current = this.MaxCount;
			this.CheckObjectiveComplete(handleComplete);
		}

		// Token: 0x040095D7 RID: 38359
		public static byte FileVersion = 1;

		// Token: 0x040095D9 RID: 38361
		public int MaxCount = 1;

		// Token: 0x040095DA RID: 38362
		public bool IsRequirement;

		// Token: 0x040095DB RID: 38363
		public Challenge Owner;

		// Token: 0x040095DC RID: 38364
		public ChallengeClass OwnerClass;

		// Token: 0x040095DD RID: 38365
		public string Biome = "";

		// Token: 0x040095DE RID: 38366
		public bool ShowRequirements = true;

		// Token: 0x040095DF RID: 38367
		[PublicizedFrom(EAccessModifier.Private)]
		public bool complete;

		// Token: 0x040095E0 RID: 38368
		public bool IsTracking;

		// Token: 0x040095E1 RID: 38369
		[PublicizedFrom(EAccessModifier.Protected)]
		public int current;
	}
}
