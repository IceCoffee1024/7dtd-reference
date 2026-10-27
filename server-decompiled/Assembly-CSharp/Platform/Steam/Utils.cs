using System;
using InControl;
using Platform.Shared;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001D07 RID: 7431
	public class Utils : IUtils
	{
		// Token: 0x0600DC0E RID: 56334 RVA: 0x004E975E File Offset: 0x004E795E
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += delegate
			{
				this.ResolveSteamBranchName();
			};
		}

		// Token: 0x0600DC0F RID: 56335 RVA: 0x004E9784 File Offset: 0x004E7984
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

		// Token: 0x0600DC10 RID: 56336 RVA: 0x004E97CD File Offset: 0x004E79CD
		public bool OpenBrowser(string _url)
		{
			if (global::Utils.IsValidWebUrl(ref _url))
			{
				SteamFriends.ActivateGameOverlayToWebPage(_url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
			}
			return true;
		}

		// Token: 0x0600DC11 RID: 56337 RVA: 0x004E97E0 File Offset: 0x004E79E0
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

		// Token: 0x0600DC12 RID: 56338 RVA: 0x004E9838 File Offset: 0x004E7A38
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

		// Token: 0x0600DC13 RID: 56339 RVA: 0x004E98A1 File Offset: 0x004E7AA1
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

		// Token: 0x0600DC14 RID: 56340 RVA: 0x004E98D7 File Offset: 0x004E7AD7
		public string GetStoreBranchName()
		{
			return this.branchName;
		}

		// Token: 0x0600DC15 RID: 56341 RVA: 0x004E98DF File Offset: 0x004E7ADF
		public void ClearTempFiles()
		{
			Platform.Shared.Utils.TryDeleteTempCacheContents();
		}

		// Token: 0x0600DC16 RID: 56342 RVA: 0x004E98E6 File Offset: 0x004E7AE6
		public string GetTempFileName(string prefix = "", string suffix = "")
		{
			return Platform.Shared.Utils.GetRandomTempCacheFileName(prefix, suffix);
		}

		// Token: 0x0600DC17 RID: 56343 RVA: 0x004E98F0 File Offset: 0x004E7AF0
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

		// Token: 0x0600DC18 RID: 56344 RVA: 0x000027FC File Offset: 0x000009FC
		public void ControllerDisconnected(InputDevice inputDevice)
		{
		}

		// Token: 0x0600DC19 RID: 56345 RVA: 0x004E9928 File Offset: 0x004E7B28
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

		// Token: 0x0400A6B8 RID: 42680
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A6B9 RID: 42681
		[PublicizedFrom(EAccessModifier.Private)]
		public string branchName = "none";
	}
}
