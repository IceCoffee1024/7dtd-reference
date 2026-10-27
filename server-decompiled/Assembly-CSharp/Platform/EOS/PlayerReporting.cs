using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Reports;

namespace Platform.EOS
{
	// Token: 0x02001D61 RID: 7521
	public class PlayerReporting : IPlayerReporting
	{
		// Token: 0x17001C60 RID: 7264
		// (get) Token: 0x0600DEBA RID: 57018 RVA: 0x004F3DA0 File Offset: 0x004F1FA0
		public ReportsInterface reportsInterface
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return ((Api)this.owner.Api).PlatformInterface.GetReportsInterface();
			}
		}

		// Token: 0x17001C61 RID: 7265
		// (get) Token: 0x0600DEBB RID: 57019 RVA: 0x004F3DBC File Offset: 0x004F1FBC
		public ProductUserId localProductUserId
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return ((UserIdentifierEos)this.owner.User.PlatformUserId).ProductUserId;
			}
		}

		// Token: 0x0600DEBC RID: 57020 RVA: 0x004F3DD8 File Offset: 0x004F1FD8
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DEBD RID: 57021 RVA: 0x004F3DE4 File Offset: 0x004F1FE4
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

		// Token: 0x0600DEBE RID: 57022 RVA: 0x004F3E80 File Offset: 0x004F2080
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

		// Token: 0x0600DEBF RID: 57023 RVA: 0x004F3F50 File Offset: 0x004F2150
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

		// Token: 0x0400A82A RID: 43050
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A82B RID: 43051
		[PublicizedFrom(EAccessModifier.Private)]
		public DictionaryList<PlayerReportsCategory, IPlayerReporting.PlayerReportCategory> reportCategories;

		// Token: 0x02001D62 RID: 7522
		[PublicizedFrom(EAccessModifier.Private)]
		public class PlayerReportCategoryEos : IPlayerReporting.PlayerReportCategory
		{
			// Token: 0x0600DEC1 RID: 57025 RVA: 0x004F3F8B File Offset: 0x004F218B
			public PlayerReportCategoryEos(PlayerReportsCategory _category, string _displayString)
			{
				this.Category = _category;
				this.displayString = _displayString;
			}

			// Token: 0x0600DEC2 RID: 57026 RVA: 0x004F3FA1 File Offset: 0x004F21A1
			public override string ToString()
			{
				return this.displayString;
			}

			// Token: 0x0400A82C RID: 43052
			public readonly PlayerReportsCategory Category;

			// Token: 0x0400A82D RID: 43053
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly string displayString;
		}
	}
}
