using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018D7 RID: 6359
	[Preserve]
	public class ChallengeObjectiveChallengeStatAwarded : BaseChallengeObjective
	{
		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x0600C3D4 RID: 50132 RVA: 0x00484989 File Offset: 0x00482B89
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.ChallengeStatAwarded;
			}
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x0600C3D5 RID: 50133 RVA: 0x0048498D File Offset: 0x00482B8D
		public override string DescriptionText
		{
			get
			{
				return Localization.Get(this.statText, false, null);
			}
		}

		// Token: 0x0600C3D6 RID: 50134 RVA: 0x0048499C File Offset: 0x00482B9C
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

		// Token: 0x0600C3D7 RID: 50135 RVA: 0x00484A2A File Offset: 0x00482C2A
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ChallengeAwardCredit -= this.Current_ChallengeAwardCredit;
		}

		// Token: 0x0600C3D8 RID: 50136 RVA: 0x00484A44 File Offset: 0x00482C44
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

		// Token: 0x0600C3D9 RID: 50137 RVA: 0x00484A98 File Offset: 0x00482C98
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

		// Token: 0x0600C3DA RID: 50138 RVA: 0x00484AE5 File Offset: 0x00482CE5
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C3DB RID: 50139 RVA: 0x00484B14 File Offset: 0x00482D14
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

		// Token: 0x0600C3DC RID: 50140 RVA: 0x00484C30 File Offset: 0x00482E30
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

		// Token: 0x04009404 RID: 37892
		[PublicizedFrom(EAccessModifier.Private)]
		public string challengeStat = "";

		// Token: 0x04009405 RID: 37893
		[PublicizedFrom(EAccessModifier.Private)]
		public string statText = "";

		// Token: 0x04009406 RID: 37894
		[PublicizedFrom(EAccessModifier.Protected)]
		public string trackerIndexName;

		// Token: 0x04009407 RID: 37895
		[PublicizedFrom(EAccessModifier.Protected)]
		public string trackerNavObjectName;

		// Token: 0x04009408 RID: 37896
		[PublicizedFrom(EAccessModifier.Protected)]
		public float trackDistance = 20f;

		// Token: 0x04009409 RID: 37897
		public TrackingEntry trackingEntry;
	}
}
