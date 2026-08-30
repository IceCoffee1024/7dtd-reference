using System;
using Webserver;
using Webserver.UrlHandlers;

namespace MapRendering
{
	// Token: 0x02001AB1 RID: 6833
	public static class MapRendering
	{
		// Token: 0x0600CE2B RID: 52779 RVA: 0x004AEF28 File Offset: 0x004AD128
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

		// Token: 0x0600CE2C RID: 52780 RVA: 0x004AEF9B File Offset: 0x004AD19B
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GameStartDone(ref ModEvents.SGameStartDoneData _data)
		{
			if (!MapRenderer.Enabled)
			{
				return;
			}
			MapRenderer instance = MapRenderer.Instance;
		}

		// Token: 0x0600CE2D RID: 52781 RVA: 0x004AEFAB File Offset: 0x004AD1AB
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GameShutdown(ref ModEvents.SGameShutdownData _data)
		{
			MapRenderer.Shutdown();
		}

		// Token: 0x0600CE2E RID: 52782 RVA: 0x004AEFB2 File Offset: 0x004AD1B2
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CalcChunkColorsDone(ref ModEvents.SCalcChunkColorsDoneData _data)
		{
			MapRenderer.RenderSingleChunk(_data.Chunk);
		}

		// Token: 0x04009C2C RID: 39980
		[PublicizedFrom(EAccessModifier.Private)]
		public const string mapTilesBaseUrl = "/map/";
	}
}
