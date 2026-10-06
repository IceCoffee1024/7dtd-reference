using System;
using Webserver;
using Webserver.UrlHandlers;

namespace MapRendering
{
	// Token: 0x02001ADD RID: 6877
	public static class MapRendering
	{
		// Token: 0x0600CF83 RID: 53123 RVA: 0x004B33F4 File Offset: 0x004B15F4
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

		// Token: 0x0600CF84 RID: 53124 RVA: 0x004B3467 File Offset: 0x004B1667
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GameStartDone(ref ModEvents.SGameStartDoneData _data)
		{
			if (!MapRenderer.Enabled)
			{
				return;
			}
			MapRenderer instance = MapRenderer.Instance;
		}

		// Token: 0x0600CF85 RID: 53125 RVA: 0x004B3477 File Offset: 0x004B1677
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GameShutdown(ref ModEvents.SGameShutdownData _data)
		{
			MapRenderer.Shutdown();
		}

		// Token: 0x0600CF86 RID: 53126 RVA: 0x004B347E File Offset: 0x004B167E
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CalcChunkColorsDone(ref ModEvents.SCalcChunkColorsDoneData _data)
		{
			MapRenderer.RenderSingleChunk(_data.Chunk);
		}

		// Token: 0x04009CF2 RID: 40178
		[PublicizedFrom(EAccessModifier.Private)]
		public const string mapTilesBaseUrl = "/map/";
	}
}
