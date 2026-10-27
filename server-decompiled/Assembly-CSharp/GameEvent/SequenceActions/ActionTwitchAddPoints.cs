using System;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A59 RID: 6745
	[Preserve]
	public class ActionTwitchAddPoints : ActionBaseClientAction
	{
		// Token: 0x0600CD21 RID: 52513 RVA: 0x004AC7F0 File Offset: 0x004AA9F0
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null && base.Owner.Requester == entityPlayerLocal)
			{
				this.amount = GameEventManager.GetIntValue(entityPlayerLocal, this.amountText, 0);
				if (this.recipientType == ActionTwitchAddPoints.RecipientTypes.All)
				{
					switch (this.pointType)
					{
					case TwitchAction.PointTypes.PP:
						TwitchManager.Current.ViewerData.AddPointsAll(this.amount, 0, true);
						break;
					case TwitchAction.PointTypes.SP:
						TwitchManager.Current.ViewerData.AddPointsAll(0, this.amount, true);
						break;
					case TwitchAction.PointTypes.Bits:
						Debug.LogWarning("TwitchAddPoints: Cannot add Bit Credit to all.");
						break;
					}
					TwitchManager.Current.SendChannelMessage(Localization.Get(this.awardText, false, null), true);
					return;
				}
				this.viewer = ((this.recipientType == ActionTwitchAddPoints.RecipientTypes.Requester) ? base.Owner.ExtraData : TwitchManager.Current.ViewerData.GetRandomActiveViewer());
				if (this.viewer == "")
				{
					return;
				}
				switch (this.pointType)
				{
				case TwitchAction.PointTypes.PP:
					TwitchManager.Current.ViewerData.AddPoints(this.viewer, this.amount, false, false);
					break;
				case TwitchAction.PointTypes.SP:
					TwitchManager.Current.ViewerData.AddPoints(this.viewer, this.amount, true, false);
					break;
				case TwitchAction.PointTypes.Bits:
					TwitchManager.Current.ViewerData.AddCredit(this.viewer, this.amount, false);
					break;
				}
				if (this.awardText != "")
				{
					TwitchManager.Current.SendChannelMessage(base.GetTextWithElements(Localization.Get(this.awardText, false, null)), true);
				}
			}
		}

		// Token: 0x0600CD22 RID: 52514 RVA: 0x004AC98F File Offset: 0x004AAB8F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string ParseTextElement(string element)
		{
			if (element == "amount")
			{
				return this.amount.ToString();
			}
			if (!(element == "viewer"))
			{
				return base.ParseTextElement(element);
			}
			return this.viewer;
		}

		// Token: 0x0600CD23 RID: 52515 RVA: 0x004AC9C8 File Offset: 0x004AABC8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchAddPoints.PropAmount, ref this.amountText);
			properties.ParseEnum<TwitchAction.PointTypes>(ActionTwitchAddPoints.PropPointType, ref this.pointType);
			properties.ParseEnum<ActionTwitchAddPoints.RecipientTypes>(ActionTwitchAddPoints.PropRecipientType, ref this.recipientType);
			properties.ParseBool(ActionTwitchAddPoints.PropRequesterOnly, ref this.requesterOnly);
			properties.ParseString(ActionTwitchAddPoints.PropAwardText, ref this.awardText);
		}

		// Token: 0x0600CD24 RID: 52516 RVA: 0x004ACA34 File Offset: 0x004AAC34
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchAddPoints
			{
				amountText = this.amountText,
				pointType = this.pointType,
				recipientType = this.recipientType,
				requesterOnly = this.requesterOnly,
				awardText = this.awardText
			};
		}

		// Token: 0x04009B98 RID: 39832
		[PublicizedFrom(EAccessModifier.Protected)]
		public string amountText;

		// Token: 0x04009B99 RID: 39833
		[PublicizedFrom(EAccessModifier.Protected)]
		public string viewer;

		// Token: 0x04009B9A RID: 39834
		[PublicizedFrom(EAccessModifier.Protected)]
		public int amount;

		// Token: 0x04009B9B RID: 39835
		[PublicizedFrom(EAccessModifier.Protected)]
		public TwitchAction.PointTypes pointType;

		// Token: 0x04009B9C RID: 39836
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool requesterOnly = true;

		// Token: 0x04009B9D RID: 39837
		[PublicizedFrom(EAccessModifier.Protected)]
		public string awardText = "";

		// Token: 0x04009B9E RID: 39838
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTwitchAddPoints.RecipientTypes recipientType;

		// Token: 0x04009B9F RID: 39839
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAmount = "amount";

		// Token: 0x04009BA0 RID: 39840
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPointType = "point_type";

		// Token: 0x04009BA1 RID: 39841
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRecipientType = "recipient_type";

		// Token: 0x04009BA2 RID: 39842
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRequesterOnly = "requester_only";

		// Token: 0x04009BA3 RID: 39843
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAwardText = "award_text";

		// Token: 0x02001A5A RID: 6746
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum RecipientTypes
		{
			// Token: 0x04009BA5 RID: 39845
			Requester,
			// Token: 0x04009BA6 RID: 39846
			All,
			// Token: 0x04009BA7 RID: 39847
			Random
		}
	}
}
