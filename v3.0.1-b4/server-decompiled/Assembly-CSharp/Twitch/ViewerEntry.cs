using System;
using UnityEngine;

namespace Twitch
{
	// Token: 0x0200183C RID: 6204
	public class ViewerEntry
	{
		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x0600BF5D RID: 48989 RVA: 0x004707E6 File Offset: 0x0046E9E6
		public float CombinedPoints
		{
			get
			{
				return this.SpecialPoints + this.StandardPoints;
			}
		}

		// Token: 0x0600BF5E RID: 48990 RVA: 0x004707F8 File Offset: 0x0046E9F8
		public void RemovePoints(float usedPoints, TwitchAction.PointTypes pointType, TwitchActionEntry entry)
		{
			if (pointType == TwitchAction.PointTypes.SP)
			{
				this.SpecialPoints -= usedPoints;
				entry.SpecialPointsUsed = (int)usedPoints;
				return;
			}
			if (pointType == TwitchAction.PointTypes.PP)
			{
				float num = Mathf.Min(usedPoints, this.StandardPoints);
				entry.StandardPointsUsed = (int)num;
				this.StandardPoints -= num;
				num = usedPoints - num;
				if (num > 0f)
				{
					this.SpecialPoints -= num;
					entry.SpecialPointsUsed = (int)num;
					return;
				}
			}
			else if (pointType == TwitchAction.PointTypes.Bits)
			{
				int num2 = Utils.FastMin((int)usedPoints, (ExtensionManager.Version == "2.0.1") ? this.BitCredits : TwitchAction.GetAdjustedBitPriceFloor(this.BitCredits));
				this.BitCredits -= num2;
				entry.CreditsUsed = num2;
				entry.BitsUsed = (int)usedPoints;
				TwitchLeaderboardStats leaderboardStats = TwitchManager.LeaderboardStats;
				int num3 = ((ExtensionManager.Version == "2.0.1") ? (entry.BitsUsed - num2) : TwitchAction.GetAdjustedBitPriceCeil(entry.BitsUsed - num2));
				if (num3 > 0)
				{
					leaderboardStats.TotalBits += num3;
					leaderboardStats.CheckMostBitsSpent(leaderboardStats.AddBitsUsed(entry.UserName, this.UserColor, num3));
				}
			}
		}

		// Token: 0x04009053 RID: 36947
		public float SpecialPoints;

		// Token: 0x04009054 RID: 36948
		public float StandardPoints;

		// Token: 0x04009055 RID: 36949
		public int BitCredits;

		// Token: 0x04009056 RID: 36950
		public int UserID = -1;

		// Token: 0x04009057 RID: 36951
		public string UserColor = "FFFFFF";

		// Token: 0x04009058 RID: 36952
		public float LastAction = -1f;

		// Token: 0x04009059 RID: 36953
		public float addPointsUntil;

		// Token: 0x0400905A RID: 36954
		public bool IsActive;

		// Token: 0x0400905B RID: 36955
		public bool IsSub;
	}
}
