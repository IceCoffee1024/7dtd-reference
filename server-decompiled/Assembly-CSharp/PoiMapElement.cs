using System;
using System.Collections.Generic;

// Token: 0x02000BF0 RID: 3056
public class PoiMapElement
{
	// Token: 0x06005D28 RID: 23848 RVA: 0x00242E20 File Offset: 0x00241020
	public PoiMapElement(uint _color, string _name, BlockValue _blockValue, BlockValue _blockBelow, int _iSO, int _ypos, int _yposFill, int _iST)
	{
		this.m_sModelName = _name;
		this.m_uColorId = _color;
		this.m_YPos = _ypos;
		this.m_YPosFill = _yposFill;
		this.m_BlockValue = _blockValue;
		this.m_BlockBelow = _blockBelow;
	}

	// Token: 0x06005D29 RID: 23849 RVA: 0x00242E76 File Offset: 0x00241076
	public PoiMapDecal GetDecal(int _index)
	{
		if (_index >= 0 && _index < this.decals.Count)
		{
			return this.decals[_index];
		}
		return null;
	}

	// Token: 0x06005D2A RID: 23850 RVA: 0x00242E98 File Offset: 0x00241098
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

	// Token: 0x06005D2B RID: 23851 RVA: 0x00242EDC File Offset: 0x002410DC
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

	// Token: 0x040047F2 RID: 18418
	public uint m_uColorId;

	// Token: 0x040047F3 RID: 18419
	public string m_sModelName;

	// Token: 0x040047F4 RID: 18420
	public BlockValue m_BlockValue;

	// Token: 0x040047F5 RID: 18421
	public BlockValue m_BlockBelow;

	// Token: 0x040047F6 RID: 18422
	public int m_YPos;

	// Token: 0x040047F7 RID: 18423
	public int m_YPosFill;

	// Token: 0x040047F8 RID: 18424
	public List<PoiMapDecal> decals = new List<PoiMapDecal>();

	// Token: 0x040047F9 RID: 18425
	public List<PoiMapBlock> blocksOnTop = new List<PoiMapBlock>();
}
