using System;
using System.Text;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CB1 RID: 7345
	public class Api : IPlatformApi
	{
		// Token: 0x17001B79 RID: 7033
		// (get) Token: 0x0600D9C1 RID: 55745 RVA: 0x004E0BA2 File Offset: 0x004DEDA2
		// (set) Token: 0x0600D9C2 RID: 55746 RVA: 0x004E0BAA File Offset: 0x004DEDAA
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x14000134 RID: 308
		// (add) Token: 0x0600D9C3 RID: 55747 RVA: 0x004E0BB4 File Offset: 0x004DEDB4
		// (remove) Token: 0x0600D9C4 RID: 55748 RVA: 0x004E0C10 File Offset: 0x004DEE10
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

		// Token: 0x0600D9C5 RID: 55749 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D9C6 RID: 55750 RVA: 0x004E0C5C File Offset: 0x004DEE5C
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

		// Token: 0x0600D9C7 RID: 55751 RVA: 0x00020119 File Offset: 0x0001E319
		public bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600D9C8 RID: 55752 RVA: 0x004E0D30 File Offset: 0x004DEF30
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

		// Token: 0x0600D9C9 RID: 55753 RVA: 0x004E0D4C File Offset: 0x004DEF4C
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

		// Token: 0x0600D9CA RID: 55754 RVA: 0x004E0D9F File Offset: 0x004DEF9F
		public void Destroy()
		{
			if (this.ClientApiStatus == EApiStatus.Ok)
			{
				SteamAPI.Shutdown();
			}
		}

		// Token: 0x0600D9CB RID: 55755 RVA: 0x004E0DAE File Offset: 0x004DEFAE
		[PublicizedFrom(EAccessModifier.Private)]
		public void ExceptionThrown(int _severity, StringBuilder _message)
		{
			Log.Error("[Steamworks.NET] " + ((_severity == 0) ? "Info: " : "Warning: ") + ": " + ((_message != null) ? _message.ToString() : null));
		}

		// Token: 0x0600D9CC RID: 55756 RVA: 0x00041064 File Offset: 0x0003F264
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0400A52A RID: 42282
		[PublicizedFrom(EAccessModifier.Private)]
		public Action clientApiInitialized;

		// Token: 0x0400A52B RID: 42283
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch tickDurationStopwatch = new MicroStopwatch(false);
	}
}
