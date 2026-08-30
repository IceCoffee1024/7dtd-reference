using System;
using UnityEngine;

// Token: 0x02000AEB RID: 2795
public class BlockUVCoordinates
{
	// Token: 0x06005342 RID: 21314 RVA: 0x001FE0D0 File Offset: 0x001FC2D0
	public BlockUVCoordinates(Rect topUvCoordinates, Rect sideUvCoordinates, Rect bottomUvCoordinates)
	{
		this.BlockFaceUvCoordinates[0] = topUvCoordinates;
		this.BlockFaceUvCoordinates[1] = bottomUvCoordinates;
		this.BlockFaceUvCoordinates[2] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[4] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[3] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[5] = sideUvCoordinates;
	}

	// Token: 0x06005343 RID: 21315 RVA: 0x001FE140 File Offset: 0x001FC340
	public BlockUVCoordinates(Rect topUvCoordinates, Rect bottomUvCoordinates, Rect northUvCoordinates, Rect southUvCoordinates, Rect westUvCoordinates, Rect eastUvCoordinates)
	{
		this.BlockFaceUvCoordinates[0] = topUvCoordinates;
		this.BlockFaceUvCoordinates[1] = bottomUvCoordinates;
		this.BlockFaceUvCoordinates[2] = northUvCoordinates;
		this.BlockFaceUvCoordinates[4] = southUvCoordinates;
		this.BlockFaceUvCoordinates[3] = westUvCoordinates;
		this.BlockFaceUvCoordinates[5] = eastUvCoordinates;
	}

	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x06005344 RID: 21316 RVA: 0x001FE1B0 File Offset: 0x001FC3B0
	public Rect[] BlockFaceUvCoordinates
	{
		get
		{
			return this.m_BlockFaceUvCoordinates;
		}
	}

	// Token: 0x040040F9 RID: 16633
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Rect[] m_BlockFaceUvCoordinates = new Rect[6];
}
