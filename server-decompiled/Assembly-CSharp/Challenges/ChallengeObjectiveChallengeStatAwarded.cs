using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200195A RID: 6490
	[Preserve]
	public class ChallengeObjectiveChallengeStatAwarded : BaseChallengeObjective
	{
		// Token: 0x1700194A RID: 6474
		// (get) Token: 0x0600C7C7 RID: 51143 RVA: 0x00492BB5 File Offset: 0x00490DB5
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.ChallengeStatAwarded;
			}
		}

		// Token: 0x1700194B RID: 6475
		// (get) Token: 0x0600C7C8 RID: 51144 RVA: 0x00492BB9 File Offset: 0x00490DB9
		public override string DescriptionText
		{
			get
			{
				return Localization.Get(this.statText, false, null);
			}
		}

		// Token: 0x0600C7C9 RID: 51145 RVA: 0x00492BC8 File Offset: 0x00490DC8
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

		// Token: 0x0600C7CA RID: 51146 RVA: 0x00492C56 File Offset: 0x00490E56
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ChallengeAwardCredit -= this.Current_ChallengeAwardCredit;
		}

		// Token: 0x0600C7CB RID: 51147 RVA: 0x00492C70 File Offset: 0x00490E70
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

		// Token: 0x0600C7CC RID: 51148 RVA: 0x00492CC4 File Offset: 0x00490EC4
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

		// Token: 0x0600C7CD RID: 51149 RVA: 0x00492D11 File Offset: 0x00490F11
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C7CE RID: 51150 RVA: 0x00492D40 File Offset: 0x00490F40
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

		// Token: 0x0600C7CF RID: 51151 RVA: 0x00492E5C File Offset: 0x0049105C
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

		// Token: 0x040096DC RID: 38620
		[PublicizedFrom(EAccessModifier.Private)]
		public string challengeStat = "";

		// Token: 0x040096DD RID: 38621
		[PublicizedFrom(EAccessModifier.Private)]
		public string statText = "";

		// Token: 0x040096DE RID: 38622
		[PublicizedFrom(EAccessModifier.Protected)]
		public string trackerIndexName;

		// Token: 0x040096DF RID: 38623
		[PublicizedFrom(EAccessModifier.Protected)]
		public string trackerNavObjectName;

		// Token: 0x040096E0 RID: 38624
		[PublicizedFrom(EAccessModifier.Protected)]
		public float trackDistance = 20f;

		// Token: 0x040096E1 RID: 38625
		public TrackingEntry trackingEntry;
	}
}
