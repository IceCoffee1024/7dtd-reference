using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B6A RID: 7018
	public interface IPlayerReporting
	{
		// Token: 0x0600D16B RID: 53611
		void Init(IPlatform _owner);

		// Token: 0x0600D16C RID: 53612
		IList<IPlayerReporting.PlayerReportCategory> ReportCategories();

		// Token: 0x0600D16D RID: 53613
		void ReportPlayer(PlatformUserIdentifierAbs _reportedUserCross, IPlayerReporting.PlayerReportCategory _reportCategory, string _message, Action<bool> _reportCompleteCallback);

		// Token: 0x0600D16E RID: 53614
		IPlayerReporting.PlayerReportCategory GetPlayerReportCategoryMapping(EnumReportCategory _reportCategory);

		// Token: 0x02001B6B RID: 7019
		public abstract class PlayerReportCategory
		{
			// Token: 0x0600D16F RID: 53615
			public abstract override string ToString();

			// Token: 0x0600D170 RID: 53616 RVA: 0x0000640C File Offset: 0x0000460C
			[PublicizedFrom(EAccessModifier.Protected)]
			public PlayerReportCategory()
			{
			}
		}
	}
}
