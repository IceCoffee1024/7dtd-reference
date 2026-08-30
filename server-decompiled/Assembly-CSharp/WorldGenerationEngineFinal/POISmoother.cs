using System;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001718 RID: 5912
	public class POISmoother
	{
		// Token: 0x0600B815 RID: 47125 RVA: 0x0044507D File Offset: 0x0044327D
		public POISmoother(WorldBuilder _worldBuilder)
		{
			this.worldBuilder = _worldBuilder;
		}

		// Token: 0x0600B816 RID: 47126 RVA: 0x0044508C File Offset: 0x0044328C
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

		// Token: 0x04008989 RID: 35209
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;
	}
}
