using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001924 RID: 6436
	[Preserve]
	public class ChallengeBaseTrackedItemObjective : BaseChallengeObjective
	{
		// Token: 0x0600C680 RID: 50816 RVA: 0x0048E5F2 File Offset: 0x0048C7F2
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C681 RID: 50817 RVA: 0x0048E618 File Offset: 0x0048C818
		public void SetupItem(string itemID)
		{
			this.itemClassID = itemID;
		}

		// Token: 0x0600C682 RID: 50818 RVA: 0x0048E624 File Offset: 0x0048C824
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

		// Token: 0x0600C683 RID: 50819 RVA: 0x0048E6E0 File Offset: 0x0048C8E0
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

		// Token: 0x0600C684 RID: 50820 RVA: 0x0048E72D File Offset: 0x0048C92D
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C685 RID: 50821 RVA: 0x0048E75C File Offset: 0x0048C95C
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

		// Token: 0x0600C686 RID: 50822 RVA: 0x0048E824 File Offset: 0x0048CA24
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

		// Token: 0x0600C687 RID: 50823 RVA: 0x000200DA File Offset: 0x0001E2DA
		public override BaseChallengeObjective Clone()
		{
			return null;
		}

		// Token: 0x0600C688 RID: 50824 RVA: 0x0048E873 File Offset: 0x0048CA73
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true, false);
			}
		}

		// Token: 0x040095E3 RID: 38371
		[PublicizedFrom(EAccessModifier.Protected)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x040095E4 RID: 38372
		[PublicizedFrom(EAccessModifier.Protected)]
		public ItemClass expectedItemClass;

		// Token: 0x040095E5 RID: 38373
		[PublicizedFrom(EAccessModifier.Protected)]
		public string itemClassID = "";

		// Token: 0x040095E6 RID: 38374
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideTrackerIndexName;

		// Token: 0x040095E7 RID: 38375
		[PublicizedFrom(EAccessModifier.Protected)]
		public float trackDistance = 20f;

		// Token: 0x040095E8 RID: 38376
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool disableTracking;

		// Token: 0x040095E9 RID: 38377
		public TrackingEntry trackingEntry;
	}
}
