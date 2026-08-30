using System;
using Webserver.UrlHandlers;

namespace Webserver
{
	// Token: 0x02001AC9 RID: 6857
	public static class WebServer
	{
		// Token: 0x0600CE90 RID: 52880 RVA: 0x004B3570 File Offset: 0x004B1770
		public static void Init()
		{
			ModEvents.GameStartDone.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartDoneData>(WebServer.GameStartDone));
			ModEvents.WorldShuttingDown.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SWorldShuttingDownData>(WebServer.WorldShuttingDown));
		}

		// Token: 0x0600CE91 RID: 52881 RVA: 0x004B359E File Offset: 0x004B179E
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GameStartDone(ref ModEvents.SGameStartDoneData _data)
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return;
			}
			WebServer.webInstance = new Web();
			LogBuffer.Init();
			if (ItemIconHandler.Instance != null)
			{
				ThreadManager.StartCoroutine(ItemIconHandler.Instance.LoadIcons());
			}
		}

		// Token: 0x0600CE92 RID: 52882 RVA: 0x004B35D3 File Offset: 0x004B17D3
		[PublicizedFrom(EAccessModifier.Private)]
		public static void WorldShuttingDown(ref ModEvents.SWorldShuttingDownData _data)
		{
			Web web = WebServer.webInstance;
			if (web == null)
			{
				return;
			}
			web.Disconnect();
		}

		// Token: 0x04009C91 RID: 40081
		[PublicizedFrom(EAccessModifier.Private)]
		public static Web webInstance;
	}
}
