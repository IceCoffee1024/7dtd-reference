using System;
using Webserver.UrlHandlers;

namespace Webserver
{
	// Token: 0x02001AF5 RID: 6901
	public static class WebServer
	{
		// Token: 0x0600CFE8 RID: 53224 RVA: 0x004B7A3C File Offset: 0x004B5C3C
		public static void Init()
		{
			ModEvents.GameStartDone.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartDoneData>(WebServer.GameStartDone));
			ModEvents.WorldShuttingDown.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SWorldShuttingDownData>(WebServer.WorldShuttingDown));
		}

		// Token: 0x0600CFE9 RID: 53225 RVA: 0x004B7A6A File Offset: 0x004B5C6A
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

		// Token: 0x0600CFEA RID: 53226 RVA: 0x004B7A9F File Offset: 0x004B5C9F
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

		// Token: 0x04009D57 RID: 40279
		[PublicizedFrom(EAccessModifier.Private)]
		public static Web webInstance;
	}
}
