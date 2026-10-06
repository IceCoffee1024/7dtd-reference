using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001929 RID: 6441
	[Preserve]
	public class ChallengeObjectiveChallengeStatAwarded : BaseChallengeObjective
	{
		// Token: 0x17001902 RID: 6402
		// (get) Token: 0x0600C6B3 RID: 50867 RVA: 0x0048F1C1 File Offset: 0x0048D3C1
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.ChallengeStatAwarded;
			}
		}

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x0600C6B4 RID: 50868 RVA: 0x0048F1C5 File Offset: 0x0048D3C5
		public override string DescriptionText
		{
			get
			{
				return Localization.Get(this.statText, false, null);
			}
		}

		// Token: 0x0600C6B5 RID: 50869 RVA: 0x0048F1D4 File Offset: 0x0048D3D4
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

		// Token: 0x0600C6B6 RID: 50870 RVA: 0x0048F262 File Offset: 0x0048D462
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ChallengeAwardCredit -= this.Current_ChallengeAwardCredit;
		}

		// Token: 0x0600C6B7 RID: 50871 RVA: 0x0048F27C File Offset: 0x0048D47C
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

		// Token: 0x0600C6B8 RID: 50872 RVA: 0x0048F2D0 File Offset: 0x0048D4D0
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

		// Token: 0x0600C6B9 RID: 50873 RVA: 0x0048F31D File Offset: 0x0048D51D
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C6BA RID: 50874 RVA: 0x0048F34C File Offset: 0x0048D54C
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

		// Token: 0x0600C6BB RID: 50875 RVA: 0x0048F468 File Offset: 0x0048D668
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

		// Token: 0x040095F3 RID: 38387
		[PublicizedFrom(EAccessModifier.Private)]
		public string challengeStat = "";

		// Token: 0x040095F4 RID: 38388
		[PublicizedFrom(EAccessModifier.Private)]
		public string statText = "";

		// Token: 0x040095F5 RID: 38389
		[PublicizedFrom(EAccessModifier.Protected)]
		public string trackerIndexName;

		// Token: 0x040095F6 RID: 38390
		[PublicizedFrom(EAccessModifier.Protected)]
		public string trackerNavObjectName;

		// Token: 0x040095F7 RID: 38391
		[PublicizedFrom(EAccessModifier.Protected)]
		public float trackDistance = 20f;

		// Token: 0x040095F8 RID: 38392
		public TrackingEntry trackingEntry;
	}
}
