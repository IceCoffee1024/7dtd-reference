using System;
using Webserver;
using Webserver.UrlHandlers;

namespace MapRendering
{
	// Token: 0x02001B0E RID: 6926
	public static class MapRendering
	{
		// Token: 0x0600D097 RID: 53399 RVA: 0x004B6EF0 File Offset: 0x004B50F0
		public static void Init()
		{
			ModEvents.GameStartDone.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartDoneData>(MapRendering.GameStartDone));
			ModEvents.GameShutdown.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameShutdownData>(MapRendering.GameShutdown));
			ModEvents.CalcChunkColorsDone.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SCalcChunkColorsDoneData>(MapRendering.CalcChunkColorsDone));
			Web.ServerInitialized += delegate(Web _web)
			{
				if (!MapRenderer.Enabled)
				{
					return;
				}
				_web.RegisterPathHandler("/map/", new StaticHandler(GameIO.GetSaveGameDir() + "/map", MapRenderer.GetTileCache(), false, "web.map"));
				_web.OpenApiHelpers.RegisterCustomSpec(typeof(MapRendering).Assembly, "MapTileHandler", "/map/");
			};
		}

		// Token: 0x0600D098 RID: 53400 RVA: 0x004B6F63 File Offset: 0x004B5163
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GameStartDone(ref ModEvents.SGameStartDoneData _data)
		{
			if (!MapRenderer.Enabled)
			{
				return;
			}
			MapRenderer instance = MapRenderer.Instance;
		}

		// Token: 0x0600D099 RID: 53401 RVA: 0x004B6F73 File Offset: 0x004B5173
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GameShutdown(ref ModEvents.SGameShutdownData _data)
		{
			MapRenderer.Shutdown();
		}

		// Token: 0x0600D09A RID: 53402 RVA: 0x004B6F7A File Offset: 0x004B517A
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CalcChunkColorsDone(ref ModEvents.SCalcChunkColorsDoneData _data)
		{
			MapRenderer.RenderSingleChunk(_data.Chunk);
		}

		// Token: 0x04009DDB RID: 40411
		[PublicizedFrom(EAccessModifier.Private)]
		public const string mapTilesBaseUrl = "/map/";
	}
}
