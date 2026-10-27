using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200196C RID: 6508
	[Preserve]
	public class ChallengeObjectiveTime : BaseChallengeObjective
	{
		// Token: 0x17001971 RID: 6513
		// (get) Token: 0x0600C88F RID: 51343 RVA: 0x00495D87 File Offset: 0x00493F87
		// (set) Token: 0x0600C890 RID: 51344 RVA: 0x00495D8F File Offset: 0x00493F8F
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

		// Token: 0x17001972 RID: 6514
		// (get) Token: 0x0600C891 RID: 51345 RVA: 0x0004AC0D File Offset: 0x00048E0D
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Time;
			}
		}

		// Token: 0x17001973 RID: 6515
		// (get) Token: 0x0600C892 RID: 51346 RVA: 0x00495DA0 File Offset: 0x00493FA0
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

		// Token: 0x17001974 RID: 6516
		// (get) Token: 0x0600C893 RID: 51347 RVA: 0x00495E00 File Offset: 0x00494000
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

		// Token: 0x17001975 RID: 6517
		// (get) Token: 0x0600C894 RID: 51348 RVA: 0x00495E71 File Offset: 0x00494071
		public override float FillAmount
		{
			get
			{
				return this.currentTime / this.maxTime;
			}
		}

		// Token: 0x17001976 RID: 6518
		// (get) Token: 0x0600C895 RID: 51349 RVA: 0x00495E80 File Offset: 0x00494080
		public override bool NeedsConstantUIUpdate
		{
			get
			{
				return !base.Complete;
			}
		}

		// Token: 0x0600C896 RID: 51350 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C897 RID: 51351 RVA: 0x00495E8B File Offset: 0x0049408B
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		}

		// Token: 0x0600C898 RID: 51352 RVA: 0x00495E98 File Offset: 0x00494098
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		}

		// Token: 0x0600C899 RID: 51353 RVA: 0x00495EA8 File Offset: 0x004940A8
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

		// Token: 0x0600C89A RID: 51354 RVA: 0x00495F5F File Offset: 0x0049415F
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("max_time"))
			{
				this.maxTime = StringParsers.ParseFloat(e.GetAttribute("max_time"), 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600C89B RID: 51355 RVA: 0x00495F9C File Offset: 0x0049419C
		public override void Read(byte _currentVersion, PooledBinaryReader _br)
		{
			base.Read(_currentVersion, _br);
			this.currentTime = _br.ReadSingle();
		}

		// Token: 0x0600C89C RID: 51356 RVA: 0x00495FB2 File Offset: 0x004941B2
		public override void Write(PooledBinaryWriter _bw)
		{
			base.Write(_bw);
			_bw.Write(this.currentTime);
		}

		// Token: 0x0600C89D RID: 51357 RVA: 0x00495FC8 File Offset: 0x004941C8
		public override void CopyValues(BaseChallengeObjective obj, BaseChallengeObjective objFromClass)
		{
			base.CopyValues(obj, objFromClass);
			ChallengeObjectiveTime challengeObjectiveTime = obj as ChallengeObjectiveTime;
			if (challengeObjectiveTime != null)
			{
				this.currentTime = challengeObjectiveTime.currentTime;
			}
		}

		// Token: 0x0600C89E RID: 51358 RVA: 0x00495FF4 File Offset: 0x004941F4
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

		// Token: 0x0400971D RID: 38685
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime;

		// Token: 0x0400971E RID: 38686
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxTime;

		// Token: 0x0400971F RID: 38687
		[PublicizedFrom(EAccessModifier.Protected)]
		public float nextCheck;

		// Token: 0x04009720 RID: 38688
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isInvalid = true;

		// Token: 0x04009721 RID: 38689
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal player;
	}
}
