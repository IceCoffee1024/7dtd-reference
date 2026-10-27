using System;
using System.Text;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CE2 RID: 7394
	public class Api : IPlatformApi
	{
		// Token: 0x17001BC1 RID: 7105
		// (get) Token: 0x0600DAD5 RID: 56021 RVA: 0x004E46F2 File Offset: 0x004E28F2
		// (set) Token: 0x0600DAD6 RID: 56022 RVA: 0x004E46FA File Offset: 0x004E28FA
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x14000134 RID: 308
		// (add) Token: 0x0600DAD7 RID: 56023 RVA: 0x004E4704 File Offset: 0x004E2904
		// (remove) Token: 0x0600DAD8 RID: 56024 RVA: 0x004E4760 File Offset: 0x004E2960
		public event Action ClientApiInitialized
		{
			add
			{
				lock (this)
				{
					this.clientApiInitialized = (Action)Delegate.Combine(this.clientApiInitialized, value);
					if (this.ClientApiStatus == EApiStatus.Ok)
					{
						value();
					}
				}
			}
			remove
			{
				lock (this)
				{
					this.clientApiInitialized = (Action)Delegate.Remove(this.clientApiInitialized, value);
				}
			}
		}

		// Token: 0x0600DAD9 RID: 56025 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DADA RID: 56026 RVA: 0x004E47AC File Offset: 0x004E29AC
		public bool InitClientApis()
		{
			if (!Packsize.Test())
			{
				Log.Out("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
				this.ClientApiStatus = EApiStatus.PermanentError;
				return false;
			}
			if (!DllCheck.Test())
			{
				Log.Out("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
				this.ClientApiStatus = EApiStatus.PermanentError;
				return false;
			}
			try
			{
				if (!SteamAPI.Init())
				{
					Log.Out("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");
					this.ClientApiStatus = EApiStatus.TemporaryError;
					return false;
				}
			}
			catch (DllNotFoundException ex)
			{
				string text = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
				DllNotFoundException ex2 = ex;
				Log.Out(text + ((ex2 != null) ? ex2.ToString() : null));
				this.ClientApiStatus = EApiStatus.PermanentError;
				return false;
			}
			Log.Out("[Steamworks.NET] SteamAPI_Init() ok");
			SteamClient.SetWarningMessageHook(new SteamAPIWarningMessageHook_t(this.ExceptionThrown));
			SteamUtils.SetOverlayNotificationPosition(ENotificationPosition.k_EPositionTopRight);
			this.ClientApiStatus = EApiStatus.Ok;
			Action action = this.clientApiInitialized;
			if (action != null)
			{
				action();
			}
			return true;
		}

		// Token: 0x0600DADB RID: 56027 RVA: 0x00020119 File Offset: 0x0001E319
		public bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600DADC RID: 56028 RVA: 0x004E4880 File Offset: 0x004E2A80
		public void ServerApiLoaded()
		{
			if (this.ClientApiStatus != EApiStatus.Ok)
			{
				Action action = this.clientApiInitialized;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x0600DADD RID: 56029 RVA: 0x004E489C File Offset: 0x004E2A9C
		public void Update()
		{
			if (this.ClientApiStatus == EApiStatus.Ok)
			{
				this.tickDurationStopwatch.Restart();
				SteamAPI.RunCallbacks();
				long num = this.tickDurationStopwatch.ElapsedMicroseconds / 1000L;
				if (num > 25L)
				{
					Log.Warning(string.Format("[Steam] Tick took exceptionally long: {0} ms", num));
				}
			}
		}

		// Token: 0x0600DADE RID: 56030 RVA: 0x004E48EF File Offset: 0x004E2AEF
		public void Destroy()
		{
			if (this.ClientApiStatus == EApiStatus.Ok)
			{
				SteamAPI.Shutdown();
			}
		}

		// Token: 0x0600DADF RID: 56031 RVA: 0x004E48FE File Offset: 0x004E2AFE
		[PublicizedFrom(EAccessModifier.Private)]
		public void ExceptionThrown(int _severity, StringBuilder _message)
		{
			Log.Error("[Steamworks.NET] " + ((_severity == 0) ? "Info: " : "Warning: ") + ": " + ((_message != null) ? _message.ToString() : null));
		}

		// Token: 0x0600DAE0 RID: 56032 RVA: 0x00041024 File Offset: 0x0003F224
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0400A613 RID: 42515
		[PublicizedFrom(EAccessModifier.Private)]
		public Action clientApiInitialized;

		// Token: 0x0400A614 RID: 42516
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch tickDurationStopwatch = new MicroStopwatch(false);
	}
}
