using System;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D5 RID: 6613
	[Preserve]
	public class ActionTwitchAddPoints : ActionBaseClientAction
	{
		// Token: 0x0600C92C RID: 51500 RVA: 0x0049E434 File Offset: 0x0049C634
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

		// Token: 0x0600C92D RID: 51501 RVA: 0x0049E5D3 File Offset: 0x0049C7D3
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

		// Token: 0x0600C92E RID: 51502 RVA: 0x0049E60C File Offset: 0x0049C80C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchAddPoints.PropAmount, ref this.amountText);
			properties.ParseEnum<TwitchAction.PointTypes>(ActionTwitchAddPoints.PropPointType, ref this.pointType);
			properties.ParseEnum<ActionTwitchAddPoints.RecipientTypes>(ActionTwitchAddPoints.PropRecipientType, ref this.recipientType);
			properties.ParseBool(ActionTwitchAddPoints.PropRequesterOnly, ref this.requesterOnly);
			properties.ParseString(ActionTwitchAddPoints.PropAwardText, ref this.awardText);
		}

		// Token: 0x0600C92F RID: 51503 RVA: 0x0049E678 File Offset: 0x0049C878
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

		// Token: 0x040098B8 RID: 39096
		[PublicizedFrom(EAccessModifier.Protected)]
		public string amountText;

		// Token: 0x040098B9 RID: 39097
		[PublicizedFrom(EAccessModifier.Protected)]
		public string viewer;

		// Token: 0x040098BA RID: 39098
		[PublicizedFrom(EAccessModifier.Protected)]
		public int amount;

		// Token: 0x040098BB RID: 39099
		[PublicizedFrom(EAccessModifier.Protected)]
		public TwitchAction.PointTypes pointType;

		// Token: 0x040098BC RID: 39100
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool requesterOnly = true;

		// Token: 0x040098BD RID: 39101
		[PublicizedFrom(EAccessModifier.Protected)]
		public string awardText = "";

		// Token: 0x040098BE RID: 39102
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTwitchAddPoints.RecipientTypes recipientType;

		// Token: 0x040098BF RID: 39103
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAmount = "amount";

		// Token: 0x040098C0 RID: 39104
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPointType = "point_type";

		// Token: 0x040098C1 RID: 39105
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRecipientType = "recipient_type";

		// Token: 0x040098C2 RID: 39106
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRequesterOnly = "requester_only";

		// Token: 0x040098C3 RID: 39107
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAwardText = "award_text";

		// Token: 0x020019D6 RID: 6614
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum RecipientTypes
		{
			// Token: 0x040098C5 RID: 39109
			Requester,
			// Token: 0x040098C6 RID: 39110
			All,
			// Token: 0x040098C7 RID: 39111
			Random
		}
	}
}
