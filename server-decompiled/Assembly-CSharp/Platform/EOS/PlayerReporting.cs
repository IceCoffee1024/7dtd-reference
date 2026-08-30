using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Reports;

namespace Platform.EOS
{
	// Token: 0x02001CFE RID: 7422
	public class PlayerReporting : IPlayerReporting
	{
		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x0600DC2B RID: 56363 RVA: 0x004EB6B8 File Offset: 0x004E98B8
		public ReportsInterface reportsInterface
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return ((Api)this.owner.Api).PlatformInterface.GetReportsInterface();
			}
		}

		// Token: 0x17001B73 RID: 7027
		// (get) Token: 0x0600DC2C RID: 56364 RVA: 0x004EB6D4 File Offset: 0x004E98D4
		public ProductUserId localProductUserId
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return ((UserIdentifierEos)this.owner.User.PlatformUserId).ProductUserId;
			}
		}

		// Token: 0x0600DC2D RID: 56365 RVA: 0x004EB6F0 File Offset: 0x004E98F0
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DC2E RID: 56366 RVA: 0x004EB6FC File Offset: 0x004E98FC
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

		// Token: 0x0600DC2F RID: 56367 RVA: 0x004EB798 File Offset: 0x004E9998
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

		// Token: 0x0600DC30 RID: 56368 RVA: 0x004EB868 File Offset: 0x004E9A68
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

		// Token: 0x0400A66C RID: 42604
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A66D RID: 42605
		[PublicizedFrom(EAccessModifier.Private)]
		public DictionaryList<PlayerReportsCategory, IPlayerReporting.PlayerReportCategory> reportCategories;

		// Token: 0x02001CFF RID: 7423
		[PublicizedFrom(EAccessModifier.Private)]
		public class PlayerReportCategoryEos : IPlayerReporting.PlayerReportCategory
		{
			// Token: 0x0600DC32 RID: 56370 RVA: 0x004EB8A3 File Offset: 0x004E9AA3
			public PlayerReportCategoryEos(PlayerReportsCategory _category, string _displayString)
			{
				this.Category = _category;
				this.displayString = _displayString;
			}

			// Token: 0x0600DC33 RID: 56371 RVA: 0x004EB8B9 File Offset: 0x004E9AB9
			public override string ToString()
			{
				return this.displayString;
			}

			// Token: 0x0400A66E RID: 42606
			public readonly PlayerReportsCategory Category;

			// Token: 0x0400A66F RID: 42607
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly string displayString;
		}
	}
}
