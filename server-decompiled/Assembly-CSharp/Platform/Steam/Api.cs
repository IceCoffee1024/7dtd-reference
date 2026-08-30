using System;
using System.Text;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C7F RID: 7295
	public class Api : IPlatformApi
	{
		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x0600D848 RID: 55368 RVA: 0x004DC066 File Offset: 0x004DA266
		// (set) Token: 0x0600D849 RID: 55369 RVA: 0x004DC06E File Offset: 0x004DA26E
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x14000135 RID: 309
		// (add) Token: 0x0600D84A RID: 55370 RVA: 0x004DC078 File Offset: 0x004DA278
		// (remove) Token: 0x0600D84B RID: 55371 RVA: 0x004DC0D4 File Offset: 0x004DA2D4
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

		// Token: 0x0600D84C RID: 55372 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D84D RID: 55373 RVA: 0x004DC120 File Offset: 0x004DA320
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

		// Token: 0x0600D84E RID: 55374 RVA: 0x00020021 File Offset: 0x0001E221
		public bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600D84F RID: 55375 RVA: 0x004DC1F4 File Offset: 0x004DA3F4
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

		// Token: 0x0600D850 RID: 55376 RVA: 0x004DC210 File Offset: 0x004DA410
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

		// Token: 0x0600D851 RID: 55377 RVA: 0x004DC263 File Offset: 0x004DA463
		public void Destroy()
		{
			if (this.ClientApiStatus == EApiStatus.Ok)
			{
				SteamAPI.Shutdown();
			}
		}

		// Token: 0x0600D852 RID: 55378 RVA: 0x004DC272 File Offset: 0x004DA472
		[PublicizedFrom(EAccessModifier.Private)]
		public void ExceptionThrown(int _severity, StringBuilder _message)
		{
			Log.Error("[Steamworks.NET] " + ((_severity == 0) ? "Info: " : "Warning: ") + ": " + ((_message != null) ? _message.ToString() : null));
		}

		// Token: 0x0600D853 RID: 55379 RVA: 0x00040F84 File Offset: 0x0003F184
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0400A455 RID: 42069
		[PublicizedFrom(EAccessModifier.Private)]
		public Action clientApiInitialized;

		// Token: 0x0400A456 RID: 42070
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch tickDurationStopwatch = new MicroStopwatch(false);
	}
}
