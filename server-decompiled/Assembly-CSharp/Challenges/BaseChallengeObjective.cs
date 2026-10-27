using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001954 RID: 6484
	[Preserve]
	public class BaseChallengeObjective
	{
		// Token: 0x17001936 RID: 6454
		// (get) Token: 0x0600C767 RID: 51047 RVA: 0x00491A4E File Offset: 0x0048FC4E
		// (set) Token: 0x0600C768 RID: 51048 RVA: 0x00491A56 File Offset: 0x0048FC56
		public byte CurrentFileVersion { get; set; }

		// Token: 0x17001937 RID: 6455
		// (get) Token: 0x0600C769 RID: 51049 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Invalid;
			}
		}

		// Token: 0x17001938 RID: 6456
		// (get) Token: 0x0600C76A RID: 51050 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual ChallengeClass.UINavTypes NavType
		{
			get
			{
				return ChallengeClass.UINavTypes.None;
			}
		}

		// Token: 0x17001939 RID: 6457
		// (get) Token: 0x0600C76B RID: 51051 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool NeedsConstantUIUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700193A RID: 6458
		// (get) Token: 0x0600C76C RID: 51052 RVA: 0x00491A5F File Offset: 0x0048FC5F
		// (set) Token: 0x0600C76D RID: 51053 RVA: 0x00491A67 File Offset: 0x0048FC67
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

		// Token: 0x1700193B RID: 6459
		// (get) Token: 0x0600C76E RID: 51054 RVA: 0x00491A7F File Offset: 0x0048FC7F
		// (set) Token: 0x0600C76F RID: 51055 RVA: 0x00491A87 File Offset: 0x0048FC87
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

		// Token: 0x0600C770 RID: 51056 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void BaseInit()
		{
		}

		// Token: 0x1700193C RID: 6460
		// (get) Token: 0x0600C771 RID: 51057 RVA: 0x00491A9F File Offset: 0x0048FC9F
		public EntityPlayerLocal Player
		{
			get
			{
				return this.Owner.Owner.Player;
			}
		}

		// Token: 0x0600C772 RID: 51058 RVA: 0x00491AB1 File Offset: 0x0048FCB1
		public void ResetComplete()
		{
			this.Complete = false;
			this.current = 0;
		}

		// Token: 0x14000117 RID: 279
		// (add) Token: 0x0600C773 RID: 51059 RVA: 0x00491AC4 File Offset: 0x0048FCC4
		// (remove) Token: 0x0600C774 RID: 51060 RVA: 0x00491AFC File Offset: 0x0048FCFC
		public event ObjectiveValueChanged ValueChanged;

		// Token: 0x1700193D RID: 6461
		// (get) Token: 0x0600C775 RID: 51061 RVA: 0x00491B31 File Offset: 0x0048FD31
		public string ObjectiveText
		{
			get
			{
				return string.Format("{0} {1}", this.DescriptionText, this.StatusText);
			}
		}

		// Token: 0x1700193E RID: 6462
		// (get) Token: 0x0600C776 RID: 51062 RVA: 0x000321FF File Offset: 0x000303FF
		public virtual string DescriptionText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700193F RID: 6463
		// (get) Token: 0x0600C777 RID: 51063 RVA: 0x00491B49 File Offset: 0x0048FD49
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

		// Token: 0x17001940 RID: 6464
		// (get) Token: 0x0600C778 RID: 51064 RVA: 0x00491B86 File Offset: 0x0048FD86
		public virtual float FillAmount
		{
			get
			{
				return (float)this.current / (float)this.MaxCount;
			}
		}

		// Token: 0x0600C779 RID: 51065 RVA: 0x00491B97 File Offset: 0x0048FD97
		[PublicizedFrom(EAccessModifier.Protected)]
		public void HandleValueChanged()
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged();
			}
		}

		// Token: 0x0600C77A RID: 51066 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init()
		{
		}

		// Token: 0x0600C77B RID: 51067 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleOnCreated()
		{
		}

		// Token: 0x0600C77C RID: 51068 RVA: 0x00491BAC File Offset: 0x0048FDAC
		public virtual bool HandleCheckStatus()
		{
			this.Complete = this.CheckObjectiveComplete(false);
			return this.Complete;
		}

		// Token: 0x0600C77D RID: 51069 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UpdateStatus()
		{
		}

		// Token: 0x0600C77E RID: 51070 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleAddHooks()
		{
		}

		// Token: 0x0600C77F RID: 51071 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleRemoveHooks()
		{
		}

		// Token: 0x0600C780 RID: 51072 RVA: 0x00491BC1 File Offset: 0x0048FDC1
		public virtual void HandleTrackingStarted()
		{
			this.IsTracking = true;
		}

		// Token: 0x0600C781 RID: 51073 RVA: 0x00491BCC File Offset: 0x0048FDCC
		public virtual void CopyValues(BaseChallengeObjective obj, BaseChallengeObjective objFromClass)
		{
			this.current = obj.current;
			this.MaxCount = objFromClass.MaxCount;
			this.ShowRequirements = objFromClass.ShowRequirements;
			this.Biome = objFromClass.Biome;
			this.complete = this.Current >= this.MaxCount;
		}

		// Token: 0x0600C782 RID: 51074 RVA: 0x00491C20 File Offset: 0x0048FE20
		public virtual void HandleTrackingEnded()
		{
			this.IsTracking = false;
		}

		// Token: 0x0600C783 RID: 51075 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleUpdatingCurrent()
		{
		}

		// Token: 0x0600C784 RID: 51076 RVA: 0x00491C29 File Offset: 0x0048FE29
		public void HandleAutoComplete()
		{
			this.current = this.MaxCount;
			this.Complete = true;
		}

		// Token: 0x0600C785 RID: 51077 RVA: 0x00491C40 File Offset: 0x0048FE40
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

		// Token: 0x0600C786 RID: 51078 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual Recipe GetRecipeItem()
		{
			return null;
		}

		// Token: 0x0600C787 RID: 51079 RVA: 0x00491DC0 File Offset: 0x0048FFC0
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

		// Token: 0x0600C788 RID: 51080 RVA: 0x00491DF5 File Offset: 0x0048FFF5
		public virtual void Read(byte _currentVersion, PooledBinaryReader _br)
		{
			this.current = _br.ReadInt32();
		}

		// Token: 0x0600C789 RID: 51081 RVA: 0x00491E03 File Offset: 0x00490003
		public void WriteObjective(PooledBinaryWriter _bw)
		{
			_bw.Write((byte)this.ObjectiveType);
			this.Write(_bw);
		}

		// Token: 0x0600C78A RID: 51082 RVA: 0x00491E18 File Offset: 0x00490018
		public virtual void Write(PooledBinaryWriter _bw)
		{
			_bw.Write(this.current);
		}

		// Token: 0x0600C78B RID: 51083 RVA: 0x00491E28 File Offset: 0x00490028
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

		// Token: 0x0600C78C RID: 51084 RVA: 0x00491E88 File Offset: 0x00490088
		public virtual bool CheckBaseRequirements()
		{
			return this.Owner.ChallengeGroup.IsActive && (this.Biome != "" && (this.Owner.Owner.Player.biomeStandingOn == null || this.Owner.Owner.Player.biomeStandingOn.m_sBiomeName != this.Biome));
		}

		// Token: 0x0600C78D RID: 51085 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual BaseChallengeObjective Clone()
		{
			return null;
		}

		// Token: 0x0600C78E RID: 51086 RVA: 0x00491EFC File Offset: 0x004900FC
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

		// Token: 0x0600C78F RID: 51087 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void Update(float deltaTime)
		{
		}

		// Token: 0x0600C790 RID: 51088 RVA: 0x00491F91 File Offset: 0x00490191
		public void HandleUpdate(float deltaTime)
		{
			if (this.Owner.IsActive)
			{
				this.Update(deltaTime);
			}
		}

		// Token: 0x0600C791 RID: 51089 RVA: 0x00491FA7 File Offset: 0x004901A7
		public virtual void CompleteObjective(bool handleComplete = true)
		{
			this.Current = this.MaxCount;
			this.CheckObjectiveComplete(handleComplete);
		}

		// Token: 0x040096C0 RID: 38592
		public static byte FileVersion = 1;

		// Token: 0x040096C2 RID: 38594
		public int MaxCount = 1;

		// Token: 0x040096C3 RID: 38595
		public bool IsRequirement;

		// Token: 0x040096C4 RID: 38596
		public Challenge Owner;

		// Token: 0x040096C5 RID: 38597
		public ChallengeClass OwnerClass;

		// Token: 0x040096C6 RID: 38598
		public string Biome = "";

		// Token: 0x040096C7 RID: 38599
		public bool ShowRequirements = true;

		// Token: 0x040096C8 RID: 38600
		[PublicizedFrom(EAccessModifier.Private)]
		public bool complete;

		// Token: 0x040096C9 RID: 38601
		public bool IsTracking;

		// Token: 0x040096CA RID: 38602
		[PublicizedFrom(EAccessModifier.Protected)]
		public int current;
	}
}
