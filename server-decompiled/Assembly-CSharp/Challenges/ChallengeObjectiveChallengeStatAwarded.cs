using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018FD RID: 6397
	[Preserve]
	public class ChallengeObjectiveChallengeStatAwarded : BaseChallengeObjective
	{
		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x0600C55B RID: 50523 RVA: 0x0048ACD1 File Offset: 0x00488ED1
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.ChallengeStatAwarded;
			}
		}

		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x0600C55C RID: 50524 RVA: 0x0048ACD5 File Offset: 0x00488ED5
		public override string DescriptionText
		{
			get
			{
				return Localization.Get(this.statText, false, null);
			}
		}

		// Token: 0x0600C55D RID: 50525 RVA: 0x0048ACE4 File Offset: 0x00488EE4
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ChallengeAwardCredit += this.Current_ChallengeAwardCredit;
			if (this.trackerIndexName != null && this.trackingEntry == null)
			{
				this.trackingEntry = new TrackingEntry
				{
					Owner = this,
					blockIndexName = this.trackerIndexName,
					navObjectName = ((this.trackerNavObjectName != null) ? this.trackerNavObjectName : "quest_resource"),
					trackDistance = this.trackDistance
				};
				this.trackingEntry.TrackingHelper = this.Owner.GetTrackingHelper();
			}
		}

		// Token: 0x0600C55E RID: 50526 RVA: 0x0048AD72 File Offset: 0x00488F72
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ChallengeAwardCredit -= this.Current_ChallengeAwardCredit;
		}

		// Token: 0x0600C55F RID: 50527 RVA: 0x0048AD8C File Offset: 0x00488F8C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_ChallengeAwardCredit(string stat, int awardCount)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.challengeStat.EqualsCaseInsensitive(stat))
			{
				base.Current += awardCount;
				if (base.Current >= this.MaxCount)
				{
					base.Current = this.MaxCount;
					this.CheckObjectiveComplete(true);
				}
			}
		}

		// Token: 0x0600C560 RID: 50528 RVA: 0x0048ADE0 File Offset: 0x00488FE0
		public override void HandleTrackingStarted()
		{
			base.HandleTrackingStarted();
			if (this.trackingEntry != null)
			{
				this.Owner.AddTrackingEntry(this.trackingEntry);
				this.trackingEntry.TrackingHelper = this.Owner.TrackingHandler;
				this.trackingEntry.AddHooks();
			}
		}

		// Token: 0x0600C561 RID: 50529 RVA: 0x0048AE2D File Offset: 0x0048902D
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C562 RID: 50530 RVA: 0x0048AE5C File Offset: 0x0048905C
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("challenge_stat"))
			{
				this.challengeStat = e.GetAttribute("challenge_stat");
			}
			if (e.HasAttribute("stat_text_key"))
			{
				this.statText = Localization.Get(e.GetAttribute("stat_text_key"), false, null);
			}
			else if (e.HasAttribute("stat_text"))
			{
				this.statText = e.GetAttribute("stat_text");
			}
			if (e.HasAttribute("tracker_index"))
			{
				this.trackerIndexName = e.GetAttribute("tracker_index");
			}
			if (e.HasAttribute("tracker_nav_object"))
			{
				this.trackerNavObjectName = e.GetAttribute("tracker_nav_object");
			}
			if (e.HasAttribute("track_distance"))
			{
				this.trackDistance = StringParsers.ParseFloat(e.GetAttribute("track_distance"), 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600C563 RID: 50531 RVA: 0x0048AF78 File Offset: 0x00489178
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveChallengeStatAwarded
			{
				challengeStat = this.challengeStat,
				statText = this.statText,
				trackerIndexName = this.trackerIndexName,
				trackerNavObjectName = this.trackerNavObjectName,
				trackDistance = this.trackDistance
			};
		}

		// Token: 0x0400952D RID: 38189
		[PublicizedFrom(EAccessModifier.Private)]
		public string challengeStat = "";

		// Token: 0x0400952E RID: 38190
		[PublicizedFrom(EAccessModifier.Private)]
		public string statText = "";

		// Token: 0x0400952F RID: 38191
		[PublicizedFrom(EAccessModifier.Protected)]
		public string trackerIndexName;

		// Token: 0x04009530 RID: 38192
		[PublicizedFrom(EAccessModifier.Protected)]
		public string trackerNavObjectName;

		// Token: 0x04009531 RID: 38193
		[PublicizedFrom(EAccessModifier.Protected)]
		public float trackDistance = 20f;

		// Token: 0x04009532 RID: 38194
		public TrackingEntry trackingEntry;
	}
}
