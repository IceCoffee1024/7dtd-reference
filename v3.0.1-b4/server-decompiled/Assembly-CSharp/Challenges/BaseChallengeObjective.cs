using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018D1 RID: 6353
	[Preserve]
	public class BaseChallengeObjective
	{
		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x0600C374 RID: 50036 RVA: 0x0048382A File Offset: 0x00481A2A
		// (set) Token: 0x0600C375 RID: 50037 RVA: 0x00483832 File Offset: 0x00481A32
		public byte CurrentFileVersion { get; set; }

		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x0600C376 RID: 50038 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Invalid;
			}
		}

		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x0600C377 RID: 50039 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual ChallengeClass.UINavTypes NavType
		{
			get
			{
				return ChallengeClass.UINavTypes.None;
			}
		}

		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x0600C378 RID: 50040 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool NeedsConstantUIUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x0600C379 RID: 50041 RVA: 0x0048383B File Offset: 0x00481A3B
		// (set) Token: 0x0600C37A RID: 50042 RVA: 0x00483843 File Offset: 0x00481A43
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

		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x0600C37B RID: 50043 RVA: 0x0048385B File Offset: 0x00481A5B
		// (set) Token: 0x0600C37C RID: 50044 RVA: 0x00483863 File Offset: 0x00481A63
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

		// Token: 0x0600C37D RID: 50045 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void BaseInit()
		{
		}

		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x0600C37E RID: 50046 RVA: 0x0048387B File Offset: 0x00481A7B
		public EntityPlayerLocal Player
		{
			get
			{
				return this.Owner.Owner.Player;
			}
		}

		// Token: 0x0600C37F RID: 50047 RVA: 0x0048388D File Offset: 0x00481A8D
		public void ResetComplete()
		{
			this.Complete = false;
			this.current = 0;
		}

		// Token: 0x14000117 RID: 279
		// (add) Token: 0x0600C380 RID: 50048 RVA: 0x004838A0 File Offset: 0x00481AA0
		// (remove) Token: 0x0600C381 RID: 50049 RVA: 0x004838D8 File Offset: 0x00481AD8
		public event ObjectiveValueChanged ValueChanged;

		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x0600C382 RID: 50050 RVA: 0x0048390D File Offset: 0x00481B0D
		public string ObjectiveText
		{
			get
			{
				return string.Format("{0} {1}", this.DescriptionText, this.StatusText);
			}
		}

		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x0600C383 RID: 50051 RVA: 0x00031E8B File Offset: 0x0003008B
		public virtual string DescriptionText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x0600C384 RID: 50052 RVA: 0x00483925 File Offset: 0x00481B25
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

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x0600C385 RID: 50053 RVA: 0x00483962 File Offset: 0x00481B62
		public virtual float FillAmount
		{
			get
			{
				return (float)this.current / (float)this.MaxCount;
			}
		}

		// Token: 0x0600C386 RID: 50054 RVA: 0x00483973 File Offset: 0x00481B73
		[PublicizedFrom(EAccessModifier.Protected)]
		public void HandleValueChanged()
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged();
			}
		}

		// Token: 0x0600C387 RID: 50055 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init()
		{
		}

		// Token: 0x0600C388 RID: 50056 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleOnCreated()
		{
		}

		// Token: 0x0600C389 RID: 50057 RVA: 0x00483988 File Offset: 0x00481B88
		public virtual bool HandleCheckStatus()
		{
			this.Complete = this.CheckObjectiveComplete(false);
			return this.Complete;
		}

		// Token: 0x0600C38A RID: 50058 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UpdateStatus()
		{
		}

		// Token: 0x0600C38B RID: 50059 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleAddHooks()
		{
		}

		// Token: 0x0600C38C RID: 50060 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void HandleRemoveHooks()
		{
		}

		// Token: 0x0600C38D RID: 50061 RVA: 0x0048399D File Offset: 0x00481B9D
		public virtual void HandleTrackingStarted()
		{
			this.IsTracking = true;
		}

		// Token: 0x0600C38E RID: 50062 RVA: 0x004839A8 File Offset: 0x00481BA8
		public virtual void CopyValues(BaseChallengeObjective obj, BaseChallengeObjective objFromClass)
		{
			this.current = obj.current;
			this.MaxCount = objFromClass.MaxCount;
			this.ShowRequirements = objFromClass.ShowRequirements;
			this.Biome = objFromClass.Biome;
			this.complete = this.Current >= this.MaxCount;
		}

		// Token: 0x0600C38F RID: 50063 RVA: 0x004839FC File Offset: 0x00481BFC
		public virtual void HandleTrackingEnded()
		{
			this.IsTracking = false;
		}

		// Token: 0x0600C390 RID: 50064 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleUpdatingCurrent()
		{
		}

		// Token: 0x0600C391 RID: 50065 RVA: 0x00483A05 File Offset: 0x00481C05
		public void HandleAutoComplete()
		{
			this.current = this.MaxCount;
			this.Complete = true;
		}

		// Token: 0x0600C392 RID: 50066 RVA: 0x00483A1C File Offset: 0x00481C1C
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

		// Token: 0x0600C393 RID: 50067 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual Recipe GetRecipeItem()
		{
			return null;
		}

		// Token: 0x0600C394 RID: 50068 RVA: 0x00483B9C File Offset: 0x00481D9C
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

		// Token: 0x0600C395 RID: 50069 RVA: 0x00483BD1 File Offset: 0x00481DD1
		public virtual void Read(byte _currentVersion, BinaryReader _br)
		{
			this.current = _br.ReadInt32();
		}

		// Token: 0x0600C396 RID: 50070 RVA: 0x00483BDF File Offset: 0x00481DDF
		public void WriteObjective(BinaryWriter _bw)
		{
			_bw.Write((byte)this.ObjectiveType);
			this.Write(_bw);
		}

		// Token: 0x0600C397 RID: 50071 RVA: 0x00483BF4 File Offset: 0x00481DF4
		public virtual void Write(BinaryWriter _bw)
		{
			_bw.Write(this.current);
		}

		// Token: 0x0600C398 RID: 50072 RVA: 0x00483C04 File Offset: 0x00481E04
		public virtual bool CheckObjectiveComplete(bool handleComplete = true)
		{
			this.HandleUpdatingCurrent();
			if (this.Current >= this.MaxCount)
			{
				this.Current = this.MaxCount;
				this.Complete = true;
				if (handleComplete)
				{
					this.Owner.HandleComplete(true);
				}
				return true;
			}
			if (handleComplete)
			{
				this.Owner.HandleComplete(true);
			}
			this.Complete = false;
			return false;
		}

		// Token: 0x0600C399 RID: 50073 RVA: 0x00483C60 File Offset: 0x00481E60
		public virtual bool CheckBaseRequirements()
		{
			return this.Owner.ChallengeGroup.IsActive && (this.Biome != "" && (this.Owner.Owner.Player.biomeStandingOn == null || this.Owner.Owner.Player.biomeStandingOn.m_sBiomeName != this.Biome));
		}

		// Token: 0x0600C39A RID: 50074 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual BaseChallengeObjective Clone()
		{
			return null;
		}

		// Token: 0x0600C39B RID: 50075 RVA: 0x00483CD4 File Offset: 0x00481ED4
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

		// Token: 0x0600C39C RID: 50076 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void Update(float deltaTime)
		{
		}

		// Token: 0x0600C39D RID: 50077 RVA: 0x00483D69 File Offset: 0x00481F69
		public void HandleUpdate(float deltaTime)
		{
			if (this.Owner.IsActive)
			{
				this.Update(deltaTime);
			}
		}

		// Token: 0x0600C39E RID: 50078 RVA: 0x00483D7F File Offset: 0x00481F7F
		public virtual void CompleteObjective(bool handleComplete = true)
		{
			this.Current = this.MaxCount;
			this.CheckObjectiveComplete(handleComplete);
		}

		// Token: 0x040093E8 RID: 37864
		public static byte FileVersion = 1;

		// Token: 0x040093EA RID: 37866
		public int MaxCount = 1;

		// Token: 0x040093EB RID: 37867
		public bool IsRequirement;

		// Token: 0x040093EC RID: 37868
		public Challenge Owner;

		// Token: 0x040093ED RID: 37869
		public ChallengeClass OwnerClass;

		// Token: 0x040093EE RID: 37870
		public string Biome = "";

		// Token: 0x040093EF RID: 37871
		public bool ShowRequirements = true;

		// Token: 0x040093F0 RID: 37872
		[PublicizedFrom(EAccessModifier.Private)]
		public bool complete;

		// Token: 0x040093F1 RID: 37873
		public bool IsTracking;

		// Token: 0x040093F2 RID: 37874
		[PublicizedFrom(EAccessModifier.Protected)]
		public int current;
	}
}
