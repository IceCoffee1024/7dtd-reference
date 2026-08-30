using System;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001862 RID: 6242
	public class ViewerEntry
	{
		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x0600C0E2 RID: 49378 RVA: 0x00476582 File Offset: 0x00474782
		public float CombinedPoints
		{
			get
			{
				return this.SpecialPoints + this.StandardPoints;
			}
		}

		// Token: 0x0600C0E3 RID: 49379 RVA: 0x00476594 File Offset: 0x00474794
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

		// Token: 0x0400916F RID: 37231
		public float SpecialPoints;

		// Token: 0x04009170 RID: 37232
		public float StandardPoints;

		// Token: 0x04009171 RID: 37233
		public int BitCredits;

		// Token: 0x04009172 RID: 37234
		public int UserID = -1;

		// Token: 0x04009173 RID: 37235
		public string UserColor = "FFFFFF";

		// Token: 0x04009174 RID: 37236
		public float LastAction = -1f;

		// Token: 0x04009175 RID: 37237
		public float addPointsUntil;

		// Token: 0x04009176 RID: 37238
		public bool IsActive;

		// Token: 0x04009177 RID: 37239
		public bool IsSub;
	}
}
