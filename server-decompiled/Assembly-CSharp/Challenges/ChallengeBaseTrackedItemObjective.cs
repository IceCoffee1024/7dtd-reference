using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001955 RID: 6485
	[Preserve]
	public class ChallengeBaseTrackedItemObjective : BaseChallengeObjective
	{
		// Token: 0x0600C794 RID: 51092 RVA: 0x00491FE6 File Offset: 0x004901E6
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C795 RID: 51093 RVA: 0x0049200C File Offset: 0x0049020C
		public void SetupItem(string itemID)
		{
			this.itemClassID = itemID;
		}

		// Token: 0x0600C796 RID: 51094 RVA: 0x00492018 File Offset: 0x00490218
		public override void HandleAddHooks()
		{
			if (this.expectedItemClass != null)
			{
				string text = ((this.overrideTrackerIndexName != null) ? this.overrideTrackerIndexName : this.expectedItemClass.TrackerIndexName);
				if (text != null && this.trackingEntry == null && !this.disableTracking)
				{
					this.trackingEntry = new TrackingEntry
					{
						TrackedItem = this.expectedItemClass,
						Owner = this,
						blockIndexName = text,
						navObjectName = ((this.expectedItemClass.TrackerNavObject != null) ? this.expectedItemClass.TrackerNavObject : "quest_resource"),
						trackDistance = this.trackDistance
					};
					this.trackingEntry.TrackingHelper = this.Owner.GetTrackingHelper();
				}
			}
			base.HandleAddHooks();
		}

		// Token: 0x0600C797 RID: 51095 RVA: 0x004920D4 File Offset: 0x004902D4
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

		// Token: 0x0600C798 RID: 51096 RVA: 0x00492121 File Offset: 0x00490321
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C799 RID: 51097 RVA: 0x00492150 File Offset: 0x00490350
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemClassID = e.GetAttribute("item");
			}
			if (e.HasAttribute("override_tracker_index"))
			{
				this.overrideTrackerIndexName = e.GetAttribute("override_tracker_index");
			}
			if (e.HasAttribute("track_distance"))
			{
				this.trackDistance = StringParsers.ParseFloat(e.GetAttribute("track_distance"), 0, -1, NumberStyles.Any);
			}
			if (e.HasAttribute("disable_tracking"))
			{
				this.disableTracking = StringParsers.ParseBool(e.GetAttribute("disable_tracking"), 0, -1, true);
			}
		}

		// Token: 0x0600C79A RID: 51098 RVA: 0x00492218 File Offset: 0x00490418
		public override void CopyValues(BaseChallengeObjective obj, BaseChallengeObjective objFromClass)
		{
			base.CopyValues(obj, objFromClass);
			ChallengeBaseTrackedItemObjective challengeBaseTrackedItemObjective = objFromClass as ChallengeBaseTrackedItemObjective;
			if (challengeBaseTrackedItemObjective != null)
			{
				this.itemClassID = challengeBaseTrackedItemObjective.itemClassID;
				this.overrideTrackerIndexName = challengeBaseTrackedItemObjective.overrideTrackerIndexName;
				this.trackDistance = challengeBaseTrackedItemObjective.trackDistance;
				this.disableTracking = challengeBaseTrackedItemObjective.disableTracking;
			}
		}

		// Token: 0x0600C79B RID: 51099 RVA: 0x000200DA File Offset: 0x0001E2DA
		public override BaseChallengeObjective Clone()
		{
			return null;
		}

		// Token: 0x0600C79C RID: 51100 RVA: 0x00492267 File Offset: 0x00490467
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true, false);
			}
		}

		// Token: 0x040096CC RID: 38604
		[PublicizedFrom(EAccessModifier.Protected)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x040096CD RID: 38605
		[PublicizedFrom(EAccessModifier.Protected)]
		public ItemClass expectedItemClass;

		// Token: 0x040096CE RID: 38606
		[PublicizedFrom(EAccessModifier.Protected)]
		public string itemClassID = "";

		// Token: 0x040096CF RID: 38607
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideTrackerIndexName;

		// Token: 0x040096D0 RID: 38608
		[PublicizedFrom(EAccessModifier.Protected)]
		public float trackDistance = 20f;

		// Token: 0x040096D1 RID: 38609
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool disableTracking;

		// Token: 0x040096D2 RID: 38610
		public TrackingEntry trackingEntry;
	}
}
