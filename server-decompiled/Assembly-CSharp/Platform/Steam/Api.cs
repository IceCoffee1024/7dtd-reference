using System;
using System.Text;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C58 RID: 7256
	public class Api : IPlatformApi
	{
		// Token: 0x17001A61 RID: 6753
		// (get) Token: 0x0600D6B8 RID: 54968 RVA: 0x004D5C42 File Offset: 0x004D3E42
		// (set) Token: 0x0600D6B9 RID: 54969 RVA: 0x004D5C4A File Offset: 0x004D3E4A
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x14000134 RID: 308
		// (add) Token: 0x0600D6BA RID: 54970 RVA: 0x004D5C54 File Offset: 0x004D3E54
		// (remove) Token: 0x0600D6BB RID: 54971 RVA: 0x004D5CB0 File Offset: 0x004D3EB0
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

		// Token: 0x0600D6BC RID: 54972 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D6BD RID: 54973 RVA: 0x004D5CFC File Offset: 0x004D3EFC
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

		// Token: 0x0600D6BE RID: 54974 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600D6BF RID: 54975 RVA: 0x004D5DD0 File Offset: 0x004D3FD0
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

		// Token: 0x0600D6C0 RID: 54976 RVA: 0x004D5DEC File Offset: 0x004D3FEC
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

		// Token: 0x0600D6C1 RID: 54977 RVA: 0x004D5E3F File Offset: 0x004D403F
		public void Destroy()
		{
			if (this.ClientApiStatus == EApiStatus.Ok)
			{
				SteamAPI.Shutdown();
			}
		}

		// Token: 0x0600D6C2 RID: 54978 RVA: 0x004D5E4E File Offset: 0x004D404E
		[PublicizedFrom(EAccessModifier.Private)]
		public void ExceptionThrown(int _severity, StringBuilder _message)
		{
			Log.Error("[Steamworks.NET] " + ((_severity == 0) ? "Info: " : "Warning: ") + ": " + ((_message != null) ? _message.ToString() : null));
		}

		// Token: 0x0600D6C3 RID: 54979 RVA: 0x00040D24 File Offset: 0x0003EF24
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0400A324 RID: 41764
		[PublicizedFrom(EAccessModifier.Private)]
		public Action clientApiInitialized;

		// Token: 0x0400A325 RID: 41765
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch tickDurationStopwatch = new MicroStopwatch(false);
	}
}
