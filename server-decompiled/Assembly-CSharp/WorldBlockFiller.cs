using System;

// Token: 0x02000C4E RID: 3150
public class WorldBlockFiller
{
	// Token: 0x06005FF7 RID: 24567 RVA: 0x0025EB04 File Offset: 0x0025CD04
	public WorldBlockFiller(int iBiomeColorId, WorldBiomeProviderFromImage _biomeProvider, GameRandom _rand, WorldBiomes _rules)
	{
		this.m_RandomGenerator = _rand;
		this.m_iChunkDimension = 65536;
		this.m_BlocksToFill = new byte[this.m_iChunkDimension];
		this.m_iThisBiomeColorId = iBiomeColorId;
		this.m_GenRules = _rules;
	}

	// Token: 0x06005FF8 RID: 24568 RVA: 0x0025EB40 File Offset: 0x0025CD40
	public void resetBlockInformation()
	{
		for (int i = 0; i < this.m_iChunkDimension; i++)
		{
			this.m_BlocksToFill[i] = byte.MaxValue;
		}
		this.m_iMaxX = 0;
		this.m_iMinX = 16;
		this.m_iMaxY = 0;
		this.m_iMinY = 256;
		this.m_iMaxZ = 0;
		this.m_iMinZ = 16;
		this.m_iFillCount = 0;
		this.m_iAreaCount = 0;
	}

	// Token: 0x06005FF9 RID: 24569 RVA: 0x0025EBAC File Offset: 0x0025CDAC
	public void setBlockToFill(int x, int y, int z, byte top)
	{
		this.m_iMaxX = ((x > this.m_iMaxX) ? x : this.m_iMaxX);
		this.m_iMaxY = ((x > this.m_iMaxY) ? x : this.m_iMaxY);
		this.m_iMaxZ = ((x > this.m_iMaxZ) ? x : this.m_iMaxZ);
		this.m_iMinX = ((x < this.m_iMinX) ? x : this.m_iMinX);
		this.m_iMinY = ((x < this.m_iMinY) ? x : this.m_iMinY);
		this.m_iMinZ = ((x < this.m_iMinZ) ? x : this.m_iMinZ);
		this.setBlockArrayValue(x, y, z, top);
		this.m_iFillCount++;
		if (y == 0)
		{
			this.m_iAreaCount++;
		}
	}

	// Token: 0x06005FFA RID: 24570 RVA: 0x0025EC74 File Offset: 0x0025CE74
	public void fillChunk(Chunk c)
	{
		if (this.m_iAreaCount == 0)
		{
			return;
		}
		BiomeDefinition biomeDefinition = null;
		this.m_GenRules.GetBiomeMap().TryGetValue((uint)this.m_iThisBiomeColorId, out biomeDefinition);
		if (biomeDefinition != null)
		{
			int num = this.m_iAreaCount;
			int num2 = -1;
			for (int i = 0; i < biomeDefinition.m_DecoBlocks.Count; i++)
			{
				BiomeBlockDecoration biomeBlockDecoration = biomeDefinition.m_DecoBlocks[i];
				this.fillLevel(c, biomeBlockDecoration, num2, ref num);
			}
			num = this.m_iAreaCount;
			for (int j = 0; j < biomeDefinition.m_Layers.Count; j++)
			{
				num2 = biomeDefinition.m_Layers[j].m_Depth;
			}
		}
	}

	// Token: 0x06005FFB RID: 24571 RVA: 0x0025ED18 File Offset: 0x0025CF18
	[PublicizedFrom(EAccessModifier.Private)]
	public void fillLevel(Chunk c, BiomeBlockDecoration bb, int iLayerDepth, ref int iAvailableCount)
	{
		double num = (double)bb.prob;
		double num2 = (double)bb.clusterProb;
		int num3 = (int)((double)this.m_iAreaCount * num);
		int num4 = this.m_RandomGenerator.RandomRange(this.m_iMinX, this.m_iMaxX + 1);
		int num5 = this.m_RandomGenerator.RandomRange(this.m_iMinZ, this.m_iMaxZ + 1);
		byte b = this.getBlockArrayValue(num4, 0, num5);
		if (b == 255)
		{
			return;
		}
		while (iAvailableCount >= 0 && num3 >= 0)
		{
			bool flag = false;
			if (this.getBlockArrayValue(num4, (int)(b + 1), num5) == 255)
			{
				int num6 = 0;
				while (!flag)
				{
					if (num6 >= 9)
					{
						break;
					}
					int num7 = Math.Max(0, num4 - num6);
					while (!flag && num7 < Math.Min(16, num4 + num6))
					{
						int num8 = Math.Max(0, num5 - num6);
						while (!flag && num8 < Math.Min(16, num5 + num6))
						{
							b = this.getBlockArrayValue(num7, 0, num8);
							if (b != 255)
							{
								flag = true;
								num4 = num7;
								num5 = num8;
							}
							num8++;
						}
						num7++;
					}
					num6++;
				}
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				Log.Error("did not find spot to place decoration");
				return;
			}
			int num9 = this.setDecorationBlock(c, num4, (int)b, iLayerDepth, num5, num2, bb.blockValues[0]);
			iAvailableCount -= num9;
			num3 -= num9;
			do
			{
				num4 = this.m_RandomGenerator.RandomRange(this.m_iMinX, this.m_iMaxX + 1);
				num5 = this.m_RandomGenerator.RandomRange(this.m_iMinZ, this.m_iMaxZ + 1);
				b = this.getBlockArrayValue(num4, 0, num5);
			}
			while (b == 255);
		}
	}

	// Token: 0x06005FFC RID: 24572 RVA: 0x0025EEC4 File Offset: 0x0025D0C4
	[PublicizedFrom(EAccessModifier.Private)]
	public int setDecorationBlock(Chunk c, int x, int y, int d, int z, double probability, BlockValue blockValue)
	{
		int num = 1;
		int num2 = ((d >= 0) ? this.m_RandomGenerator.RandomRange(0, d) : d);
		if (num2 >= y)
		{
			return 0;
		}
		if (probability > 0.0)
		{
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					int num3 = ((d > 1) ? (-1) : 0);
					while (num3 <= ((d > 1) ? 1 : 0) && y + num3 - num2 > 0 && y + num3 - num2 < y)
					{
						if (i + x >= 0 && i + x < 16 && j + z >= 0 && j + z < 16 && this.m_RandomGenerator.RandomDouble < probability)
						{
							c.SetBlockRaw(i + x, y + num3 - num2, j + z, blockValue);
							this.setBlockArrayValue(i + x, y + num3 - num2, j + z, byte.MaxValue);
							num++;
						}
						num3++;
					}
				}
			}
		}
		c.SetBlockRaw(x, y - num2, z, blockValue);
		this.setBlockArrayValue(x, y - num2, z, byte.MaxValue);
		return num;
	}

	// Token: 0x06005FFD RID: 24573 RVA: 0x0025EFD8 File Offset: 0x0025D1D8
	[PublicizedFrom(EAccessModifier.Private)]
	public byte getBlockArrayValue(int x, int y, int z)
	{
		return this.m_BlocksToFill[((x << 4) + z << 8) + y];
	}

	// Token: 0x06005FFE RID: 24574 RVA: 0x0025EFEA File Offset: 0x0025D1EA
	[PublicizedFrom(EAccessModifier.Private)]
	public void setBlockArrayValue(int x, int y, int z, byte value)
	{
		this.m_BlocksToFill[((x << 4) + z << 8) + y] = value;
	}

	// Token: 0x04004B23 RID: 19235
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iMaxX;

	// Token: 0x04004B24 RID: 19236
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iMinX;

	// Token: 0x04004B25 RID: 19237
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iMaxY;

	// Token: 0x04004B26 RID: 19238
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iMinY;

	// Token: 0x04004B27 RID: 19239
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iMaxZ;

	// Token: 0x04004B28 RID: 19240
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iMinZ;

	// Token: 0x04004B29 RID: 19241
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_BlocksToFill;

	// Token: 0x04004B2A RID: 19242
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom m_RandomGenerator;

	// Token: 0x04004B2B RID: 19243
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iChunkDimension;

	// Token: 0x04004B2C RID: 19244
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iFillCount;

	// Token: 0x04004B2D RID: 19245
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iAreaCount;

	// Token: 0x04004B2E RID: 19246
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iThisBiomeColorId;

	// Token: 0x04004B2F RID: 19247
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBiomes m_GenRules;
}
