using System;
using InControl;
using Platform.Shared;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CA4 RID: 7332
	public class Utils : IUtils
	{
		// Token: 0x0600D980 RID: 55680 RVA: 0x004E1076 File Offset: 0x004DF276
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += delegate
			{
				this.ResolveSteamBranchName();
			};
		}

		// Token: 0x0600D981 RID: 55681 RVA: 0x004E109C File Offset: 0x004DF29C
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResolveSteamBranchName()
		{
			if (this.owner.Api.ClientApiStatus != EApiStatus.Ok)
			{
				return;
			}
			string text;
			if (SteamApps.GetCurrentBetaName(out text, 128) && !string.IsNullOrEmpty(text))
			{
				this.branchName = text;
				return;
			}
			this.branchName = "public";
		}

		// Token: 0x0600D982 RID: 55682 RVA: 0x004E10E5 File Offset: 0x004DF2E5
		public bool OpenBrowser(string _url)
		{
			if (global::Utils.IsValidWebUrl(ref _url))
			{
				SteamFriends.ActivateGameOverlayToWebPage(_url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
			}
			return true;
		}

		// Token: 0x0600D983 RID: 55683 RVA: 0x004E10F8 File Offset: 0x004DF2F8
		public string GetPlatformLanguage()
		{
			if (GameManager.IsDedicatedServer)
			{
				return "english";
			}
			if (this.owner.Api.ClientApiStatus != EApiStatus.Ok)
			{
				Log.Warning("[Steam] Unable to get platform language, Steam not initialized");
				return "english";
			}
			string text = SteamUtils.GetSteamUILanguage().ToLower();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return "english";
		}

		// Token: 0x0600D984 RID: 55684 RVA: 0x004E1150 File Offset: 0x004DF350
		public string GetAppLanguage()
		{
			if (GameManager.IsDedicatedServer)
			{
				return "english";
			}
			if (this.owner.Api.ClientApiStatus != EApiStatus.Ok)
			{
				Log.Warning("[Steam] Unable to get app language, Steam not initialized");
				return "english";
			}
			string text = SteamApps.GetCurrentGameLanguage().ToLower();
			if (text == "latam")
			{
				text = "spanish";
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return "english";
		}

		// Token: 0x0600D985 RID: 55685 RVA: 0x004E11B9 File Offset: 0x004DF3B9
		public string GetCountry()
		{
			if (GameManager.IsDedicatedServer)
			{
				return "??";
			}
			if (this.owner.Api.ClientApiStatus == EApiStatus.Ok)
			{
				return SteamUtils.GetIPCountry();
			}
			Log.Warning("[Steam] Unable to get country, Steam not initialized");
			return "??";
		}

		// Token: 0x0600D986 RID: 55686 RVA: 0x004E11EF File Offset: 0x004DF3EF
		public string GetStoreBranchName()
		{
			return this.branchName;
		}

		// Token: 0x0600D987 RID: 55687 RVA: 0x004E11F7 File Offset: 0x004DF3F7
		public void ClearTempFiles()
		{
			Platform.Shared.Utils.TryDeleteTempCacheContents();
		}

		// Token: 0x0600D988 RID: 55688 RVA: 0x004E11FE File Offset: 0x004DF3FE
		public string GetTempFileName(string prefix = "", string suffix = "")
		{
			return Platform.Shared.Utils.GetRandomTempCacheFileName(prefix, suffix);
		}

		// Token: 0x0600D989 RID: 55689 RVA: 0x004E1208 File Offset: 0x004DF408
		public bool? IsFamilyShare()
		{
			bool? flag;
			try
			{
				flag = new bool?(SteamApps.BIsSubscribedFromFamilySharing());
			}
			catch
			{
				flag = null;
			}
			return flag;
		}

		// Token: 0x0600D98A RID: 55690 RVA: 0x000027FC File Offset: 0x000009FC
		public void ControllerDisconnected(InputDevice inputDevice)
		{
		}

		// Token: 0x0600D98B RID: 55691 RVA: 0x004E1240 File Offset: 0x004DF440
		public string GetCrossplayPlayerIcon(EPlayGroup _playGroup, bool _fetchGenericIcons, EPlatformIdentifier _nativePlatform = EPlatformIdentifier.None)
		{
			switch (_playGroup)
			{
			case EPlayGroup.Standalone:
				if (_fetchGenericIcons)
				{
					return "ui_platform_pc";
				}
				break;
			case EPlayGroup.XBS:
				if (_fetchGenericIcons)
				{
					return "ui_platform_console";
				}
				break;
			case EPlayGroup.PS5:
				if (_fetchGenericIcons)
				{
					return "ui_platform_console";
				}
				break;
			}
			return string.Empty;
		}

		// Token: 0x0400A4FA RID: 42234
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A4FB RID: 42235
		[PublicizedFrom(EAccessModifier.Private)]
		public string branchName = "none";
	}
}
