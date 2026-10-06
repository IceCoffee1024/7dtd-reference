using System;
using InControl;
using Platform.Shared;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CD6 RID: 7382
	public class Utils : IUtils
	{
		// Token: 0x0600DAFA RID: 56058 RVA: 0x004E5BBE File Offset: 0x004E3DBE
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += delegate
			{
				this.ResolveSteamBranchName();
			};
		}

		// Token: 0x0600DAFB RID: 56059 RVA: 0x004E5BE4 File Offset: 0x004E3DE4
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

		// Token: 0x0600DAFC RID: 56060 RVA: 0x004E5C2D File Offset: 0x004E3E2D
		public bool OpenBrowser(string _url)
		{
			if (global::Utils.IsValidWebUrl(ref _url))
			{
				SteamFriends.ActivateGameOverlayToWebPage(_url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
			}
			return true;
		}

		// Token: 0x0600DAFD RID: 56061 RVA: 0x004E5C40 File Offset: 0x004E3E40
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

		// Token: 0x0600DAFE RID: 56062 RVA: 0x004E5C98 File Offset: 0x004E3E98
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

		// Token: 0x0600DAFF RID: 56063 RVA: 0x004E5D01 File Offset: 0x004E3F01
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

		// Token: 0x0600DB00 RID: 56064 RVA: 0x004E5D37 File Offset: 0x004E3F37
		public string GetStoreBranchName()
		{
			return this.branchName;
		}

		// Token: 0x0600DB01 RID: 56065 RVA: 0x004E5D3F File Offset: 0x004E3F3F
		public void ClearTempFiles()
		{
			Platform.Shared.Utils.TryDeleteTempCacheContents();
		}

		// Token: 0x0600DB02 RID: 56066 RVA: 0x004E5D46 File Offset: 0x004E3F46
		public string GetTempFileName(string prefix = "", string suffix = "")
		{
			return Platform.Shared.Utils.GetRandomTempCacheFileName(prefix, suffix);
		}

		// Token: 0x0600DB03 RID: 56067 RVA: 0x004E5D50 File Offset: 0x004E3F50
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

		// Token: 0x0600DB04 RID: 56068 RVA: 0x000027FC File Offset: 0x000009FC
		public void ControllerDisconnected(InputDevice inputDevice)
		{
		}

		// Token: 0x0600DB05 RID: 56069 RVA: 0x004E5D88 File Offset: 0x004E3F88
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

		// Token: 0x0400A5CF RID: 42447
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A5D0 RID: 42448
		[PublicizedFrom(EAccessModifier.Private)]
		public string branchName = "none";
	}
}
