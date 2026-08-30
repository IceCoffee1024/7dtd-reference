using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200190F RID: 6415
	[Preserve]
	public class ChallengeObjectiveTime : BaseChallengeObjective
	{
		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x0600C623 RID: 50723 RVA: 0x0048DEA7 File Offset: 0x0048C0A7
		// (set) Token: 0x0600C624 RID: 50724 RVA: 0x0048DEAF File Offset: 0x0048C0AF
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

		// Token: 0x17001888 RID: 6280
		// (get) Token: 0x0600C625 RID: 50725 RVA: 0x0004AC4A File Offset: 0x00048E4A
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Time;
			}
		}

		// Token: 0x17001889 RID: 6281
		// (get) Token: 0x0600C626 RID: 50726 RVA: 0x0048DEC0 File Offset: 0x0048C0C0
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

		// Token: 0x1700188A RID: 6282
		// (get) Token: 0x0600C627 RID: 50727 RVA: 0x0048DF20 File Offset: 0x0048C120
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

		// Token: 0x1700188B RID: 6283
		// (get) Token: 0x0600C628 RID: 50728 RVA: 0x0048DF91 File Offset: 0x0048C191
		public override float FillAmount
		{
			get
			{
				return this.currentTime / this.maxTime;
			}
		}

		// Token: 0x1700188C RID: 6284
		// (get) Token: 0x0600C629 RID: 50729 RVA: 0x0048DFA0 File Offset: 0x0048C1A0
		public override bool NeedsConstantUIUpdate
		{
			get
			{
				return !base.Complete;
			}
		}

		// Token: 0x0600C62A RID: 50730 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C62B RID: 50731 RVA: 0x0048DFAB File Offset: 0x0048C1AB
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		}

		// Token: 0x0600C62C RID: 50732 RVA: 0x0048DFB8 File Offset: 0x0048C1B8
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		}

		// Token: 0x0600C62D RID: 50733 RVA: 0x0048DFC8 File Offset: 0x0048C1C8
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

		// Token: 0x0600C62E RID: 50734 RVA: 0x0048E07F File Offset: 0x0048C27F
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("max_time"))
			{
				this.maxTime = StringParsers.ParseFloat(e.GetAttribute("max_time"), 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600C62F RID: 50735 RVA: 0x0048E0BC File Offset: 0x0048C2BC
		public override void Read(byte _currentVersion, BinaryReader _br)
		{
			base.Read(_currentVersion, _br);
			this.currentTime = _br.ReadSingle();
		}

		// Token: 0x0600C630 RID: 50736 RVA: 0x0048E0D2 File Offset: 0x0048C2D2
		public override void Write(BinaryWriter _bw)
		{
			base.Write(_bw);
			_bw.Write(this.currentTime);
		}

		// Token: 0x0600C631 RID: 50737 RVA: 0x0048E0E8 File Offset: 0x0048C2E8
		public override void CopyValues(BaseChallengeObjective obj, BaseChallengeObjective objFromClass)
		{
			base.CopyValues(obj, objFromClass);
			ChallengeObjectiveTime challengeObjectiveTime = obj as ChallengeObjectiveTime;
			if (challengeObjectiveTime != null)
			{
				this.currentTime = challengeObjectiveTime.currentTime;
			}
		}

		// Token: 0x0600C632 RID: 50738 RVA: 0x0048E114 File Offset: 0x0048C314
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

		// Token: 0x0400956E RID: 38254
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime;

		// Token: 0x0400956F RID: 38255
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxTime;

		// Token: 0x04009570 RID: 38256
		[PublicizedFrom(EAccessModifier.Protected)]
		public float nextCheck;

		// Token: 0x04009571 RID: 38257
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isInvalid = true;

		// Token: 0x04009572 RID: 38258
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal player;
	}
}
