using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018E9 RID: 6377
	[Preserve]
	public class ChallengeObjectiveTime : BaseChallengeObjective
	{
		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x0600C49C RID: 50332 RVA: 0x00487B5B File Offset: 0x00485D5B
		// (set) Token: 0x0600C49D RID: 50333 RVA: 0x00487B63 File Offset: 0x00485D63
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

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x0600C49E RID: 50334 RVA: 0x0004A832 File Offset: 0x00048A32
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Time;
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x0600C49F RID: 50335 RVA: 0x00487B74 File Offset: 0x00485D74
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

		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x0600C4A0 RID: 50336 RVA: 0x00487BD4 File Offset: 0x00485DD4
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

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x0600C4A1 RID: 50337 RVA: 0x00487C45 File Offset: 0x00485E45
		public override float FillAmount
		{
			get
			{
				return this.currentTime / this.maxTime;
			}
		}

		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x0600C4A2 RID: 50338 RVA: 0x00487C54 File Offset: 0x00485E54
		public override bool NeedsConstantUIUpdate
		{
			get
			{
				return !base.Complete;
			}
		}

		// Token: 0x0600C4A3 RID: 50339 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C4A4 RID: 50340 RVA: 0x00487C5F File Offset: 0x00485E5F
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		}

		// Token: 0x0600C4A5 RID: 50341 RVA: 0x00487C6C File Offset: 0x00485E6C
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		}

		// Token: 0x0600C4A6 RID: 50342 RVA: 0x00487C7C File Offset: 0x00485E7C
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

		// Token: 0x0600C4A7 RID: 50343 RVA: 0x00487D33 File Offset: 0x00485F33
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("max_time"))
			{
				this.maxTime = StringParsers.ParseFloat(e.GetAttribute("max_time"), 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600C4A8 RID: 50344 RVA: 0x00487D70 File Offset: 0x00485F70
		public override void Read(byte _currentVersion, BinaryReader _br)
		{
			base.Read(_currentVersion, _br);
			this.currentTime = _br.ReadSingle();
		}

		// Token: 0x0600C4A9 RID: 50345 RVA: 0x00487D86 File Offset: 0x00485F86
		public override void Write(BinaryWriter _bw)
		{
			base.Write(_bw);
			_bw.Write(this.currentTime);
		}

		// Token: 0x0600C4AA RID: 50346 RVA: 0x00487D9C File Offset: 0x00485F9C
		public override void CopyValues(BaseChallengeObjective obj, BaseChallengeObjective objFromClass)
		{
			base.CopyValues(obj, objFromClass);
			ChallengeObjectiveTime challengeObjectiveTime = obj as ChallengeObjectiveTime;
			if (challengeObjectiveTime != null)
			{
				this.currentTime = challengeObjectiveTime.currentTime;
			}
		}

		// Token: 0x0600C4AB RID: 50347 RVA: 0x00487DC8 File Offset: 0x00485FC8
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

		// Token: 0x04009445 RID: 37957
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime;

		// Token: 0x04009446 RID: 37958
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxTime;

		// Token: 0x04009447 RID: 37959
		[PublicizedFrom(EAccessModifier.Protected)]
		public float nextCheck;

		// Token: 0x04009448 RID: 37960
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isInvalid = true;

		// Token: 0x04009449 RID: 37961
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal player;
	}
}
