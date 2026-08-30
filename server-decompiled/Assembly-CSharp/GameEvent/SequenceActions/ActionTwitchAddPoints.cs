using System;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019FC RID: 6652
	[Preserve]
	public class ActionTwitchAddPoints : ActionBaseClientAction
	{
		// Token: 0x0600CAB5 RID: 51893 RVA: 0x004A4828 File Offset: 0x004A2A28
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

		// Token: 0x0600CAB6 RID: 51894 RVA: 0x004A49C7 File Offset: 0x004A2BC7
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

		// Token: 0x0600CAB7 RID: 51895 RVA: 0x004A4A00 File Offset: 0x004A2C00
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchAddPoints.PropAmount, ref this.amountText);
			properties.ParseEnum<TwitchAction.PointTypes>(ActionTwitchAddPoints.PropPointType, ref this.pointType);
			properties.ParseEnum<ActionTwitchAddPoints.RecipientTypes>(ActionTwitchAddPoints.PropRecipientType, ref this.recipientType);
			properties.ParseBool(ActionTwitchAddPoints.PropRequesterOnly, ref this.requesterOnly);
			properties.ParseString(ActionTwitchAddPoints.PropAwardText, ref this.awardText);
		}

		// Token: 0x0600CAB8 RID: 51896 RVA: 0x004A4A6C File Offset: 0x004A2C6C
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

		// Token: 0x040099E9 RID: 39401
		[PublicizedFrom(EAccessModifier.Protected)]
		public string amountText;

		// Token: 0x040099EA RID: 39402
		[PublicizedFrom(EAccessModifier.Protected)]
		public string viewer;

		// Token: 0x040099EB RID: 39403
		[PublicizedFrom(EAccessModifier.Protected)]
		public int amount;

		// Token: 0x040099EC RID: 39404
		[PublicizedFrom(EAccessModifier.Protected)]
		public TwitchAction.PointTypes pointType;

		// Token: 0x040099ED RID: 39405
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool requesterOnly = true;

		// Token: 0x040099EE RID: 39406
		[PublicizedFrom(EAccessModifier.Protected)]
		public string awardText = "";

		// Token: 0x040099EF RID: 39407
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTwitchAddPoints.RecipientTypes recipientType;

		// Token: 0x040099F0 RID: 39408
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAmount = "amount";

		// Token: 0x040099F1 RID: 39409
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPointType = "point_type";

		// Token: 0x040099F2 RID: 39410
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRecipientType = "recipient_type";

		// Token: 0x040099F3 RID: 39411
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRequesterOnly = "requester_only";

		// Token: 0x040099F4 RID: 39412
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAwardText = "award_text";

		// Token: 0x020019FD RID: 6653
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum RecipientTypes
		{
			// Token: 0x040099F6 RID: 39414
			Requester,
			// Token: 0x040099F7 RID: 39415
			All,
			// Token: 0x040099F8 RID: 39416
			Random
		}
	}
}
