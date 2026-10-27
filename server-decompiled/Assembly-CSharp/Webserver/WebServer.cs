using System;
using Webserver.UrlHandlers;

namespace Webserver
{
	// Token: 0x02001B26 RID: 6950
	public static class WebServer
	{
		// Token: 0x0600D0FC RID: 53500 RVA: 0x004BB538 File Offset: 0x004B9738
		public static void Init()
		{
			ModEvents.GameStartDone.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartDoneData>(WebServer.GameStartDone));
			ModEvents.WorldShuttingDown.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SWorldShuttingDownData>(WebServer.WorldShuttingDown));
		}

		// Token: 0x0600D0FD RID: 53501 RVA: 0x004BB566 File Offset: 0x004B9766
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

		// Token: 0x0600D0FE RID: 53502 RVA: 0x004BB59B File Offset: 0x004B979B
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

		// Token: 0x04009E40 RID: 40512
		[PublicizedFrom(EAccessModifier.Private)]
		public static Web webInstance;
	}
}
