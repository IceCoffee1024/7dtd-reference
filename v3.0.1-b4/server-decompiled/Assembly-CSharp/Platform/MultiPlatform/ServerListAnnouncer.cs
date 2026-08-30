using System;
using System.Collections.Generic;
using System.Threading;

namespace Platform.MultiPlatform
{
	// Token: 0x02001C98 RID: 7320
	public class ServerListAnnouncer : IMasterServerAnnouncer
	{
		// Token: 0x0600D8EB RID: 55531 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D8EC RID: 55532 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x0600D8ED RID: 55533 RVA: 0x004DC980 File Offset: 0x004DAB80
		public bool GameServerInitialized
		{
			get
			{
				IMasterServerAnnouncer serverListAnnouncer = PlatformManager.NativePlatform.ServerListAnnouncer;
				if (serverListAnnouncer == null || serverListAnnouncer.GameServerInitialized)
				{
					IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
					bool? flag;
					if (crossplatformPlatform == null)
					{
						flag = null;
					}
					else
					{
						IMasterServerAnnouncer serverListAnnouncer2 = crossplatformPlatform.ServerListAnnouncer;
						flag = ((serverListAnnouncer2 != null) ? new bool?(serverListAnnouncer2.GameServerInitialized) : null);
					}
					return flag ?? true;
				}
				return false;
			}
		}

		// Token: 0x0600D8EE RID: 55534 RVA: 0x004DC9EC File Offset: 0x004DABEC
		public string GetServerPorts()
		{
			string text = "";
			IMasterServerAnnouncer serverListAnnouncer = PlatformManager.NativePlatform.ServerListAnnouncer;
			string text2 = ((serverListAnnouncer != null) ? serverListAnnouncer.GetServerPorts() : null);
			if (!string.IsNullOrEmpty(text2))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ", ";
				}
				text += text2;
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			string text3;
			if (crossplatformPlatform == null)
			{
				text3 = null;
			}
			else
			{
				IMasterServerAnnouncer serverListAnnouncer2 = crossplatformPlatform.ServerListAnnouncer;
				text3 = ((serverListAnnouncer2 != null) ? serverListAnnouncer2.GetServerPorts() : null);
			}
			string text4 = text3;
			if (!string.IsNullOrEmpty(text4))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ", ";
				}
				text += text4;
			}
			foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.ServerPlatforms)
			{
				if (keyValuePair.Value.AsServerOnly)
				{
					IMasterServerAnnouncer serverListAnnouncer3 = keyValuePair.Value.ServerListAnnouncer;
					string text5 = ((serverListAnnouncer3 != null) ? serverListAnnouncer3.GetServerPorts() : null);
					if (!string.IsNullOrEmpty(text5))
					{
						if (!string.IsNullOrEmpty(text))
						{
							text += ", ";
						}
						text += text5;
					}
				}
			}
			return text;
		}

		// Token: 0x0600D8EF RID: 55535 RVA: 0x004DCB04 File Offset: 0x004DAD04
		public void AdvertiseServer(Action _onServerRegistered)
		{
			if (PlatformManager.NativePlatform.ServerListAnnouncer != null)
			{
				Interlocked.Increment(ref this.platformsAdvertising);
				PlatformManager.NativePlatform.ServerListAnnouncer.AdvertiseServer(new Action(this.serverRegisteredCallback));
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (((crossplatformPlatform != null) ? crossplatformPlatform.ServerListAnnouncer : null) != null)
			{
				Interlocked.Increment(ref this.platformsAdvertising);
				IPlatform crossplatformPlatform2 = PlatformManager.CrossplatformPlatform;
				if (crossplatformPlatform2 != null)
				{
					IMasterServerAnnouncer serverListAnnouncer = crossplatformPlatform2.ServerListAnnouncer;
					if (serverListAnnouncer != null)
					{
						serverListAnnouncer.AdvertiseServer(new Action(this.serverRegisteredCallback));
					}
				}
			}
			foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.ServerPlatforms)
			{
				if (keyValuePair.Value.AsServerOnly && keyValuePair.Value.ServerListAnnouncer != null)
				{
					Interlocked.Increment(ref this.platformsAdvertising);
					keyValuePair.Value.ServerListAnnouncer.AdvertiseServer(new Action(this.serverRegisteredCallback));
				}
			}
			if (this.platformsAdvertising == 0)
			{
				_onServerRegistered();
				this.onServerRegistered = null;
				return;
			}
			this.onServerRegistered = _onServerRegistered;
		}

		// Token: 0x0600D8F0 RID: 55536 RVA: 0x004DCC24 File Offset: 0x004DAE24
		[PublicizedFrom(EAccessModifier.Private)]
		public void serverRegisteredCallback()
		{
			if (Interlocked.Decrement(ref this.platformsAdvertising) == 0)
			{
				Action action = this.onServerRegistered;
				if (action != null)
				{
					action();
				}
				this.onServerRegistered = null;
			}
		}

		// Token: 0x0600D8F1 RID: 55537 RVA: 0x004DCC4C File Offset: 0x004DAE4C
		public void StopServer()
		{
			foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.ServerPlatforms)
			{
				if (keyValuePair.Value.AsServerOnly)
				{
					IMasterServerAnnouncer serverListAnnouncer = keyValuePair.Value.ServerListAnnouncer;
					if (serverListAnnouncer != null)
					{
						serverListAnnouncer.StopServer();
					}
				}
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform != null)
			{
				IMasterServerAnnouncer serverListAnnouncer2 = crossplatformPlatform.ServerListAnnouncer;
				if (serverListAnnouncer2 != null)
				{
					serverListAnnouncer2.StopServer();
				}
			}
			IMasterServerAnnouncer serverListAnnouncer3 = PlatformManager.NativePlatform.ServerListAnnouncer;
			if (serverListAnnouncer3 == null)
			{
				return;
			}
			serverListAnnouncer3.StopServer();
		}

		// Token: 0x0400A432 RID: 42034
		[PublicizedFrom(EAccessModifier.Private)]
		public int platformsAdvertising;

		// Token: 0x0400A433 RID: 42035
		[PublicizedFrom(EAccessModifier.Private)]
		public Action onServerRegistered;
	}
}
