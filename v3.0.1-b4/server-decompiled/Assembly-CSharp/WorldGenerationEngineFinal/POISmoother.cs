using System;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016F2 RID: 5874
	public class POISmoother
	{
		// Token: 0x0600B692 RID: 46738 RVA: 0x0043F305 File Offset: 0x0043D505
		public POISmoother(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x0600B693 RID: 46739 RVA: 0x0043F314 File Offset: 0x0043D514
		public void SmoothStreetTiles()
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			int streetTileMapWidth = this.worldBuilder.StreetTileMapWidth;
			int streetTileMapWidth2 = this.worldBuilder.StreetTileMapWidth;
			int num = 0;
			int num2 = streetTileMapWidth * streetTileMapWidth2;
			for (int i = 0; i < streetTileMapWidth2; i++)
			{
				for (int j = 0; j < streetTileMapWidth; j++)
				{
					num++;
					StreetTile streetTile = this.worldBuilder.StreetTileMap[j + i * streetTileMapWidth];
					streetTile.SmoothTownshipTerrain();
					streetTile.UpdateValidity();
				}
				this.worldBuilder.SetTaskMessage(string.Format(this.worldBuilder.messageSmoothingStreetTiles, Mathf.RoundToInt((float)num / (float)num2 * 100f)));
			}
			Log.Out("POISmoother SmoothStreetTiles in {0}", new object[] { (float)microStopwatch.ElapsedMilliseconds * 0.001f });
		}

		// Token: 0x0400886D RID: 34925
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;
	}
}
