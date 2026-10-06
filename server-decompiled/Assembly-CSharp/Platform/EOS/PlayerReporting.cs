using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Reports;

namespace Platform.EOS
{
	// Token: 0x02001D30 RID: 7472
	public class PlayerReporting : IPlayerReporting
	{
		// Token: 0x17001C18 RID: 7192
		// (get) Token: 0x0600DDA6 RID: 56742 RVA: 0x004F0200 File Offset: 0x004EE400
		public ReportsInterface reportsInterface
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return ((Api)this.owner.Api).PlatformInterface.GetReportsInterface();
			}
		}

		// Token: 0x17001C19 RID: 7193
		// (get) Token: 0x0600DDA7 RID: 56743 RVA: 0x004F021C File Offset: 0x004EE41C
		public ProductUserId localProductUserId
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return ((UserIdentifierEos)this.owner.User.PlatformUserId).ProductUserId;
			}
		}

		// Token: 0x0600DDA8 RID: 56744 RVA: 0x004F0238 File Offset: 0x004EE438
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DDA9 RID: 56745 RVA: 0x004F0244 File Offset: 0x004EE444
		public IList<IPlayerReporting.PlayerReportCategory> ReportCategories()
		{
			if (this.reportCategories != null)
			{
				return this.reportCategories.list;
			}
			this.reportCategories = new DictionaryList<PlayerReportsCategory, IPlayerReporting.PlayerReportCategory>();
			foreach (PlayerReportsCategory playerReportsCategory in EnumUtils.Values<PlayerReportsCategory>())
			{
				if (playerReportsCategory != PlayerReportsCategory.Invalid)
				{
					this.reportCategories.Add(playerReportsCategory, new PlayerReporting.PlayerReportCategoryEos(playerReportsCategory, Localization.Get("xuiCategoryPlayerReport" + playerReportsCategory.ToStringCached<PlayerReportsCategory>(), false, null)));
				}
			}
			return this.reportCategories.list;
		}

		// Token: 0x0600DDAA RID: 56746 RVA: 0x004F02E0 File Offset: 0x004EE4E0
		public void ReportPlayer(PlatformUserIdentifierAbs _reportedUserCross, IPlayerReporting.PlayerReportCategory _reportCategory, string _message, Action<bool> _reportCompleteCallback)
		{
			if (_message != null && _message.Length > 256)
			{
				Log.Out("[EOS-Report] Long message, might get truncated");
			}
			EosHelpers.AssertMainThread("PRep.Send");
			SendPlayerBehaviorReportOptions sendPlayerBehaviorReportOptions = new SendPlayerBehaviorReportOptions
			{
				ReporterUserId = this.localProductUserId,
				ReportedUserId = ((UserIdentifierEos)_reportedUserCross).ProductUserId,
				Category = ((PlayerReporting.PlayerReportCategoryEos)_reportCategory).Category,
				Message = _message
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.reportsInterface.SendPlayerBehaviorReport(ref sendPlayerBehaviorReportOptions, null, delegate(ref SendPlayerBehaviorReportCompleteCallbackInfo _callbackData)
				{
					if (_callbackData.ResultCode != Result.Success)
					{
						Log.Error("[EOS-Report] Reporting player failed: " + _callbackData.ResultCode.ToStringCached<Result>());
						_reportCompleteCallback(false);
						return;
					}
					Log.Out("[EOS-Report] Sent player report");
					_reportCompleteCallback(true);
				});
			}
		}

		// Token: 0x0600DDAB RID: 56747 RVA: 0x004F03B0 File Offset: 0x004EE5B0
		public IPlayerReporting.PlayerReportCategory GetPlayerReportCategoryMapping(EnumReportCategory _reportCategory)
		{
			PlayerReportsCategory playerReportsCategory;
			if (_reportCategory != EnumReportCategory.Cheating)
			{
				if (_reportCategory != EnumReportCategory.VerbalAbuse)
				{
					playerReportsCategory = PlayerReportsCategory.Other;
				}
				else
				{
					playerReportsCategory = PlayerReportsCategory.VerbalAbuse;
				}
			}
			else
			{
				playerReportsCategory = PlayerReportsCategory.Cheating;
			}
			PlayerReportsCategory playerReportsCategory2 = playerReportsCategory;
			IPlayerReporting.PlayerReportCategory playerReportCategory;
			if (!this.reportCategories.dict.TryGetValue(playerReportsCategory2, out playerReportCategory))
			{
				return null;
			}
			return playerReportCategory;
		}

		// Token: 0x0400A741 RID: 42817
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A742 RID: 42818
		[PublicizedFrom(EAccessModifier.Private)]
		public DictionaryList<PlayerReportsCategory, IPlayerReporting.PlayerReportCategory> reportCategories;

		// Token: 0x02001D31 RID: 7473
		[PublicizedFrom(EAccessModifier.Private)]
		public class PlayerReportCategoryEos : IPlayerReporting.PlayerReportCategory
		{
			// Token: 0x0600DDAD RID: 56749 RVA: 0x004F03EB File Offset: 0x004EE5EB
			public PlayerReportCategoryEos(PlayerReportsCategory _category, string _displayString)
			{
				this.Category = _category;
				this.displayString = _displayString;
			}

			// Token: 0x0600DDAE RID: 56750 RVA: 0x004F0401 File Offset: 0x004EE601
			public override string ToString()
			{
				return this.displayString;
			}

			// Token: 0x0400A743 RID: 42819
			public readonly PlayerReportsCategory Category;

			// Token: 0x0400A744 RID: 42820
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly string displayString;
		}
	}
}
