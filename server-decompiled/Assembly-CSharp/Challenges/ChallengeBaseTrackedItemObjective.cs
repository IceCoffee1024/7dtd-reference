using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018F8 RID: 6392
	[Preserve]
	public class ChallengeBaseTrackedItemObjective : BaseChallengeObjective
	{
		// Token: 0x0600C528 RID: 50472 RVA: 0x0048A102 File Offset: 0x00488302
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
		}

		// Token: 0x0600C529 RID: 50473 RVA: 0x0048A128 File Offset: 0x00488328
		public void SetupItem(string itemID)
		{
			this.itemClassID = itemID;
		}

		// Token: 0x0600C52A RID: 50474 RVA: 0x0048A134 File Offset: 0x00488334
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

		// Token: 0x0600C52B RID: 50475 RVA: 0x0048A1F0 File Offset: 0x004883F0
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

		// Token: 0x0600C52C RID: 50476 RVA: 0x0048A23D File Offset: 0x0048843D
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
			if (this.trackingEntry != null)
			{
				this.trackingEntry.RemoveHooks();
				this.Owner.RemoveTrackingEntry(this.trackingEntry);
			}
		}

		// Token: 0x0600C52D RID: 50477 RVA: 0x0048A26C File Offset: 0x0048846C
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

		// Token: 0x0600C52E RID: 50478 RVA: 0x0048A334 File Offset: 0x00488534
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

		// Token: 0x0600C52F RID: 50479 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public override BaseChallengeObjective Clone()
		{
			return null;
		}

		// Token: 0x0600C530 RID: 50480 RVA: 0x0048A383 File Offset: 0x00488583
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true, false);
			}
		}

		// Token: 0x0400951D RID: 38173
		[PublicizedFrom(EAccessModifier.Protected)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x0400951E RID: 38174
		[PublicizedFrom(EAccessModifier.Protected)]
		public ItemClass expectedItemClass;

		// Token: 0x0400951F RID: 38175
		[PublicizedFrom(EAccessModifier.Protected)]
		public string itemClassID = "";

		// Token: 0x04009520 RID: 38176
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideTrackerIndexName;

		// Token: 0x04009521 RID: 38177
		[PublicizedFrom(EAccessModifier.Protected)]
		public float trackDistance = 20f;

		// Token: 0x04009522 RID: 38178
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool disableTracking;

		// Token: 0x04009523 RID: 38179
		public TrackingEntry trackingEntry;
	}
}
