using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BC3 RID: 7107
	public interface IPlayerReporting
	{
		// Token: 0x0600D46E RID: 54382
		void Init(IPlatform _owner);

		// Token: 0x0600D46F RID: 54383
		IList<IPlayerReporting.PlayerReportCategory> ReportCategories();

		// Token: 0x0600D470 RID: 54384
		void ReportPlayer(PlatformUserIdentifierAbs _reportedUserCross, IPlayerReporting.PlayerReportCategory _reportCategory, string _message, Action<bool> _reportCompleteCallback);

		// Token: 0x0600D471 RID: 54385
		IPlayerReporting.PlayerReportCategory GetPlayerReportCategoryMapping(EnumReportCategory _reportCategory);

		// Token: 0x02001BC4 RID: 7108
		public abstract class PlayerReportCategory
		{
			// Token: 0x0600D472 RID: 54386
			public abstract override string ToString();

			// Token: 0x0600D473 RID: 54387 RVA: 0x0000640C File Offset: 0x0000460C
			[PublicizedFrom(EAccessModifier.Protected)]
			public PlayerReportCategory()
			{
			}
		}
	}
}
