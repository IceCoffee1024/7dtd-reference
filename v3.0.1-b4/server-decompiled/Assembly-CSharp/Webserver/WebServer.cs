using System;
using Webserver.UrlHandlers;

namespace Webserver
{
	// Token: 0x02001AA2 RID: 6818
	public static class WebServer
	{
		// Token: 0x0600CD07 RID: 52487 RVA: 0x004AD17C File Offset: 0x004AB37C
		public static void Init()
		{
			ModEvents.GameStartDone.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartDoneData>(WebServer.GameStartDone));
			ModEvents.WorldShuttingDown.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SWorldShuttingDownData>(WebServer.WorldShuttingDown));
		}

		// Token: 0x0600CD08 RID: 52488 RVA: 0x004AD1AA File Offset: 0x004AB3AA
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

		// Token: 0x0600CD09 RID: 52489 RVA: 0x004AD1DF File Offset: 0x004AB3DF
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

		// Token: 0x04009B60 RID: 39776
		[PublicizedFrom(EAccessModifier.Private)]
		public static Web webInstance;
	}
}
