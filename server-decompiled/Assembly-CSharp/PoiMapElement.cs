using System;
using System.Collections.Generic;

// Token: 0x02000C14 RID: 3092
public class PoiMapElement
{
	// Token: 0x06005DCB RID: 24011 RVA: 0x00248EC0 File Offset: 0x002470C0
	public PoiMapElement(uint _color, string _name, BlockValue _blockValue, BlockValue _blockBelow, int _iSO, int _ypos, int _yposFill, int _iST)
	{
		this.m_sModelName = _name;
		this.m_uColorId = _color;
		this.m_YPos = _ypos;
		this.m_YPosFill = _yposFill;
		this.m_BlockValue = _blockValue;
		this.m_BlockBelow = _blockBelow;
	}

	// Token: 0x06005DCC RID: 24012 RVA: 0x00248F16 File Offset: 0x00247116
	public PoiMapDecal GetDecal(int _index)
	{
		if (_index >= 0 && _index < this.decals.Count)
		{
			return this.decals[_index];
		}
		return null;
	}

	// Token: 0x06005DCD RID: 24013 RVA: 0x00248F38 File Offset: 0x00247138
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

	// Token: 0x06005DCE RID: 24014 RVA: 0x00248F7C File Offset: 0x0024717C
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

	// Token: 0x040048CD RID: 18637
	public uint m_uColorId;

	// Token: 0x040048CE RID: 18638
	public string m_sModelName;

	// Token: 0x040048CF RID: 18639
	public BlockValue m_BlockValue;

	// Token: 0x040048D0 RID: 18640
	public BlockValue m_BlockBelow;

	// Token: 0x040048D1 RID: 18641
	public int m_YPos;

	// Token: 0x040048D2 RID: 18642
	public int m_YPosFill;

	// Token: 0x040048D3 RID: 18643
	public List<PoiMapDecal> decals = new List<PoiMapDecal>();

	// Token: 0x040048D4 RID: 18644
	public List<PoiMapBlock> blocksOnTop = new List<PoiMapBlock>();
}
