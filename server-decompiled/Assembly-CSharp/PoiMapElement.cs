using System;
using System.Collections.Generic;

// Token: 0x02000C02 RID: 3074
public class PoiMapElement
{
	// Token: 0x06005DB2 RID: 23986 RVA: 0x00245BC4 File Offset: 0x00243DC4
	public PoiMapElement(uint _color, string _name, BlockValue _blockValue, BlockValue _blockBelow, int _iSO, int _ypos, int _yposFill, int _iST)
	{
		this.m_sModelName = _name;
		this.m_uColorId = _color;
		this.m_YPos = _ypos;
		this.m_YPosFill = _yposFill;
		this.m_BlockValue = _blockValue;
		this.m_BlockBelow = _blockBelow;
	}

	// Token: 0x06005DB3 RID: 23987 RVA: 0x00245C1A File Offset: 0x00243E1A
	public PoiMapDecal GetDecal(int _index)
	{
		if (_index >= 0 && _index < this.decals.Count)
		{
			return this.decals[_index];
		}
		return null;
	}

	// Token: 0x06005DB4 RID: 23988 RVA: 0x00245C3C File Offset: 0x00243E3C
	public PoiMapDecal GetRandomDecal(GameRandom _random)
	{
		for (int i = 0; i < this.decals.Count; i++)
		{
			PoiMapDecal poiMapDecal = this.decals[i];
			if (_random.RandomFloat < poiMapDecal.m_Prob)
			{
				return poiMapDecal;
			}
		}
		return null;
	}

	// Token: 0x06005DB5 RID: 23989 RVA: 0x00245C80 File Offset: 0x00243E80
	public PoiMapBlock GetRandomBlockOnTop(GameRandom _random)
	{
		for (int i = 0; i < this.blocksOnTop.Count; i++)
		{
			PoiMapBlock poiMapBlock = this.blocksOnTop[i];
			if (_random.RandomFloat < poiMapBlock.m_Prob)
			{
				return poiMapBlock;
			}
		}
		return null;
	}

	// Token: 0x0400488E RID: 18574
	public uint m_uColorId;

	// Token: 0x0400488F RID: 18575
	public string m_sModelName;

	// Token: 0x04004890 RID: 18576
	public BlockValue m_BlockValue;

	// Token: 0x04004891 RID: 18577
	public BlockValue m_BlockBelow;

	// Token: 0x04004892 RID: 18578
	public int m_YPos;

	// Token: 0x04004893 RID: 18579
	public int m_YPosFill;

	// Token: 0x04004894 RID: 18580
	public List<PoiMapDecal> decals = new List<PoiMapDecal>();

	// Token: 0x04004895 RID: 18581
	public List<PoiMapBlock> blocksOnTop = new List<PoiMapBlock>();
}
