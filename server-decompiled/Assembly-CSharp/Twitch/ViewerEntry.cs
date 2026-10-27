using System;
using UnityEngine;

namespace Twitch
{
	// Token: 0x020018BF RID: 6335
	public class ViewerEntry
	{
		// Token: 0x17001888 RID: 6280
		// (get) Token: 0x0600C34E RID: 49998 RVA: 0x0047E46A File Offset: 0x0047C66A
		public float CombinedPoints
		{
			get
			{
				return this.SpecialPoints + this.StandardPoints;
			}
		}

		// Token: 0x0600C34F RID: 49999 RVA: 0x0047E47C File Offset: 0x0047C67C
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

		// Token: 0x0400931E RID: 37662
		public float SpecialPoints;

		// Token: 0x0400931F RID: 37663
		public float StandardPoints;

		// Token: 0x04009320 RID: 37664
		public int BitCredits;

		// Token: 0x04009321 RID: 37665
		public int UserID = -1;

		// Token: 0x04009322 RID: 37666
		public string UserColor = "FFFFFF";

		// Token: 0x04009323 RID: 37667
		public float LastAction = -1f;

		// Token: 0x04009324 RID: 37668
		public float addPointsUntil;

		// Token: 0x04009325 RID: 37669
		public bool IsActive;

		// Token: 0x04009326 RID: 37670
		public bool IsSub;
	}
}
