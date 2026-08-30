using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B91 RID: 7057
	public interface IPlayerReporting
	{
		// Token: 0x0600D2F6 RID: 54006
		void Init(IPlatform _owner);

		// Token: 0x0600D2F7 RID: 54007
		IList<IPlayerReporting.PlayerReportCategory> ReportCategories();

		// Token: 0x0600D2F8 RID: 54008
		void ReportPlayer(PlatformUserIdentifierAbs _reportedUserCross, IPlayerReporting.PlayerReportCategory _reportCategory, string _message, Action<bool> _reportCompleteCallback);

		// Token: 0x0600D2F9 RID: 54009
		IPlayerReporting.PlayerReportCategory GetPlayerReportCategoryMapping(EnumReportCategory _reportCategory);

		// Token: 0x02001B92 RID: 7058
		public abstract class PlayerReportCategory
		{
			// Token: 0x0600D2FA RID: 54010
			public abstract override string ToString();

			// Token: 0x0600D2FB RID: 54011 RVA: 0x0000640C File Offset: 0x0000460C
			[PublicizedFrom(EAccessModifier.Protected)]
			public PlayerReportCategory()
			{
			}
		}
	}
}
