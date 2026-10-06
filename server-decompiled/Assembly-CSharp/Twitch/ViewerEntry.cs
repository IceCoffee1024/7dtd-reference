using System;
using UnityEngine;

namespace Twitch
{
	// Token: 0x0200188E RID: 6286
	public class ViewerEntry
	{
		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x0600C23A RID: 49722 RVA: 0x0047AABE File Offset: 0x00478CBE
		public float CombinedPoints
		{
			get
			{
				return this.SpecialPoints + this.StandardPoints;
			}
		}

		// Token: 0x0600C23B RID: 49723 RVA: 0x0047AAD0 File Offset: 0x00478CD0
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

		// Token: 0x04009235 RID: 37429
		public float SpecialPoints;

		// Token: 0x04009236 RID: 37430
		public float StandardPoints;

		// Token: 0x04009237 RID: 37431
		public int BitCredits;

		// Token: 0x04009238 RID: 37432
		public int UserID = -1;

		// Token: 0x04009239 RID: 37433
		public string UserColor = "FFFFFF";

		// Token: 0x0400923A RID: 37434
		public float LastAction = -1f;

		// Token: 0x0400923B RID: 37435
		public float addPointsUntil;

		// Token: 0x0400923C RID: 37436
		public bool IsActive;

		// Token: 0x0400923D RID: 37437
		public bool IsSub;
	}
}
