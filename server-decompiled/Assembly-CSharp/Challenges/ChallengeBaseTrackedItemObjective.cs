using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018D2 RID: 6354
	[Preserve]
	public class ChallengeBaseTrackedItemObjective : BaseChallengeObjective
	{
		// Token: 0x0600C3A1 RID: 50081 RVA: 0x00483DBE File Offset: 0x00481FBE
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C3A2 RID: 50082 RVA: 0x00483DE4 File Offset: 0x00481FE4
		public void SetupItem(string itemID)
		{
			this.itemClassID = itemID;
		}

		// Token: 0x0600C3A3 RID: 50083 RVA: 0x00483DF0 File Offset: 0x00481FF0
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

		// Token: 0x0600C3A4 RID: 50084 RVA: 0x00483EAC File Offset: 0x004820AC
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

		// Token: 0x0600C3A5 RID: 50085 RVA: 0x00483EF9 File Offset: 0x004820F9
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C3A6 RID: 50086 RVA: 0x00483F28 File Offset: 0x00482128
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

		// Token: 0x0600C3A7 RID: 50087 RVA: 0x00483FF0 File Offset: 0x004821F0
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

		// Token: 0x0600C3A8 RID: 50088 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public override BaseChallengeObjective Clone()
		{
			return null;
		}

		// Token: 0x0600C3A9 RID: 50089 RVA: 0x0048403F File Offset: 0x0048223F
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true);
			}
		}

		// Token: 0x040093F4 RID: 37876
		[PublicizedFrom(EAccessModifier.Protected)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x040093F5 RID: 37877
		[PublicizedFrom(EAccessModifier.Protected)]
		public ItemClass expectedItemClass;

		// Token: 0x040093F6 RID: 37878
		[PublicizedFrom(EAccessModifier.Protected)]
		public string itemClassID = "";

		// Token: 0x040093F7 RID: 37879
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideTrackerIndexName;

		// Token: 0x040093F8 RID: 37880
		[PublicizedFrom(EAccessModifier.Protected)]
		public float trackDistance = 20f;

		// Token: 0x040093F9 RID: 37881
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool disableTracking;

		// Token: 0x040093FA RID: 37882
		public TrackingEntry trackingEntry;
	}
}
