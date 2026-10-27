using System;
using System.Collections.Generic;

// Token: 0x02000C1F RID: 3103
public class PoiMapElement
{
	// Token: 0x06005DF0 RID: 24048 RVA: 0x0024A3E8 File Offset: 0x002485E8
	public PoiMapElement(uint _color, string _name, BlockValue _blockValue, BlockValue _blockBelow, int _iSO, int _ypos, int _yposFill, int _iST)
	{
		this.m_sModelName = _name;
		this.m_uColorId = _color;
		this.m_YPos = _ypos;
		this.m_YPosFill = _yposFill;
		this.m_BlockValue = _blockValue;
		this.m_BlockBelow = _blockBelow;
	}

	// Token: 0x06005DF1 RID: 24049 RVA: 0x0024A43E File Offset: 0x0024863E
	public PoiMapDecal GetDecal(int _index)
	{
		if (_index >= 0 && _index < this.decals.Count)
		{
			return this.decals[_index];
		}
		return null;
	}

	// Token: 0x06005DF2 RID: 24050 RVA: 0x0024A460 File Offset: 0x00248660
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

	// Token: 0x06005DF3 RID: 24051 RVA: 0x0024A4A4 File Offset: 0x002486A4
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

	// Token: 0x040048F2 RID: 18674
	public uint m_uColorId;

	// Token: 0x040048F3 RID: 18675
	public string m_sModelName;

	// Token: 0x040048F4 RID: 18676
	public BlockValue m_BlockValue;

	// Token: 0x040048F5 RID: 18677
	public BlockValue m_BlockBelow;

	// Token: 0x040048F6 RID: 18678
	public int m_YPos;

	// Token: 0x040048F7 RID: 18679
	public int m_YPosFill;

	// Token: 0x040048F8 RID: 18680
	public List<PoiMapDecal> decals = new List<PoiMapDecal>();

	// Token: 0x040048F9 RID: 18681
	public List<PoiMapBlock> blocksOnTop = new List<PoiMapBlock>();
}
