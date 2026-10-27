using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BF4 RID: 7156
	public interface IPlayerReporting
	{
		// Token: 0x0600D582 RID: 54658
		void Init(IPlatform _owner);

		// Token: 0x0600D583 RID: 54659
		IList<IPlayerReporting.PlayerReportCategory> ReportCategories();

		// Token: 0x0600D584 RID: 54660
		void ReportPlayer(PlatformUserIdentifierAbs _reportedUserCross, IPlayerReporting.PlayerReportCategory _reportCategory, string _message, Action<bool> _reportCompleteCallback);

		// Token: 0x0600D585 RID: 54661
		IPlayerReporting.PlayerReportCategory GetPlayerReportCategoryMapping(EnumReportCategory _reportCategory);

		// Token: 0x02001BF5 RID: 7157
		public abstract class PlayerReportCategory
		{
			// Token: 0x0600D586 RID: 54662
			public abstract override string ToString();

			// Token: 0x0600D587 RID: 54663 RVA: 0x0000640C File Offset: 0x0000460C
			[PublicizedFrom(EAccessModifier.Protected)]
			public PlayerReportCategory()
			{
			}
		}
	}
}
