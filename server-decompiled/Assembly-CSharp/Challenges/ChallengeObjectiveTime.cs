using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200193B RID: 6459
	[Preserve]
	public class ChallengeObjectiveTime : BaseChallengeObjective
	{
		// Token: 0x17001929 RID: 6441
		// (get) Token: 0x0600C77B RID: 51067 RVA: 0x00492393 File Offset: 0x00490593
		// (set) Token: 0x0600C77C RID: 51068 RVA: 0x0049239B File Offset: 0x0049059B
		public float CurrentTime
		{
			get
			{
				return this.currentTime;
			}
			set
			{
				this.currentTime = value;
				base.HandleValueChanged();
			}
		}

		// Token: 0x1700192A RID: 6442
		// (get) Token: 0x0600C77D RID: 51069 RVA: 0x0004AC4D File Offset: 0x00048E4D
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Time;
			}
		}

		// Token: 0x1700192B RID: 6443
		// (get) Token: 0x0600C77E RID: 51070 RVA: 0x004923AC File Offset: 0x004905AC
		public override string DescriptionText
		{
			get
			{
				if (this.Biome == "")
				{
					return Localization.Get("challengeObjectiveTime", false, null) + ":";
				}
				return string.Format(Localization.Get("challengeObjectiveTimeInBiome", false, null), Localization.Get("biome_" + this.Biome, false, null));
			}
		}

		// Token: 0x1700192C RID: 6444
		// (get) Token: 0x0600C77F RID: 51071 RVA: 0x0049240C File Offset: 0x0049060C
		public override string StatusText
		{
			get
			{
				if (this.currentTime == 0f)
				{
					return string.Format("{0}{1}", this.currentTime, Localization.Get("timeAbbreviationSeconds", false, null)) + " / " + XUiM_PlayerBuffs.GetTimeString(this.maxTime);
				}
				return XUiM_PlayerBuffs.GetTimeString(this.currentTime) + "/ " + XUiM_PlayerBuffs.GetTimeString(this.maxTime);
			}
		}

		// Token: 0x1700192D RID: 6445
		// (get) Token: 0x0600C780 RID: 51072 RVA: 0x0049247D File Offset: 0x0049067D
		public override float FillAmount
		{
			get
			{
				return this.currentTime / this.maxTime;
			}
		}

		// Token: 0x1700192E RID: 6446
		// (get) Token: 0x0600C781 RID: 51073 RVA: 0x0049248C File Offset: 0x0049068C
		public override bool NeedsConstantUIUpdate
		{
			get
			{
				return !base.Complete;
			}
		}

		// Token: 0x0600C782 RID: 51074 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C783 RID: 51075 RVA: 0x00492497 File Offset: 0x00490697
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		}

		// Token: 0x0600C784 RID: 51076 RVA: 0x004924A4 File Offset: 0x004906A4
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		}

		// Token: 0x0600C785 RID: 51077 RVA: 0x004924B4 File Offset: 0x004906B4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void Update(float deltaTime)
		{
			this.nextCheck -= deltaTime;
			if (this.nextCheck <= 0f)
			{
				this.isInvalid = this.CheckBaseRequirements();
				this.nextCheck = 2f;
			}
			if (this.player == null)
			{
				this.player = this.Owner.Owner.Player;
			}
			if (this.isInvalid || this.player == null || this.player.IsDead())
			{
				return;
			}
			this.CurrentTime += deltaTime;
			if (this.currentTime >= this.maxTime)
			{
				base.Current = this.MaxCount;
				this.CheckObjectiveComplete(true);
			}
		}

		// Token: 0x0600C786 RID: 51078 RVA: 0x0049256B File Offset: 0x0049076B
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("max_time"))
			{
				this.maxTime = StringParsers.ParseFloat(e.GetAttribute("max_time"), 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600C787 RID: 51079 RVA: 0x004925A8 File Offset: 0x004907A8
		public override void Read(byte _currentVersion, PooledBinaryReader _br)
		{
			base.Read(_currentVersion, _br);
			this.currentTime = _br.ReadSingle();
		}

		// Token: 0x0600C788 RID: 51080 RVA: 0x004925BE File Offset: 0x004907BE
		public override void Write(PooledBinaryWriter _bw)
		{
			base.Write(_bw);
			_bw.Write(this.currentTime);
		}

		// Token: 0x0600C789 RID: 51081 RVA: 0x004925D4 File Offset: 0x004907D4
		public override void CopyValues(BaseChallengeObjective obj, BaseChallengeObjective objFromClass)
		{
			base.CopyValues(obj, objFromClass);
			ChallengeObjectiveTime challengeObjectiveTime = obj as ChallengeObjectiveTime;
			if (challengeObjectiveTime != null)
			{
				this.currentTime = challengeObjectiveTime.currentTime;
			}
		}

		// Token: 0x0600C78A RID: 51082 RVA: 0x00492600 File Offset: 0x00490800
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveTime
			{
				Biome = this.Biome,
				maxTime = this.maxTime,
				currentTime = this.currentTime,
				nextCheck = this.nextCheck,
				isInvalid = this.isInvalid
			};
		}

		// Token: 0x04009634 RID: 38452
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime;

		// Token: 0x04009635 RID: 38453
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxTime;

		// Token: 0x04009636 RID: 38454
		[PublicizedFrom(EAccessModifier.Protected)]
		public float nextCheck;

		// Token: 0x04009637 RID: 38455
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isInvalid = true;

		// Token: 0x04009638 RID: 38456
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal player;
	}
}
