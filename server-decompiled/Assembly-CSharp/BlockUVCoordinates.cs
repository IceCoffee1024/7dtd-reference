using System;
using UnityEngine;

// Token: 0x02000AFD RID: 2813
public class BlockUVCoordinates
{
	// Token: 0x060053CA RID: 21450 RVA: 0x00200D40 File Offset: 0x001FEF40
	public BlockUVCoordinates(Rect topUvCoordinates, Rect sideUvCoordinates, Rect bottomUvCoordinates)
	{
		this.BlockFaceUvCoordinates[0] = topUvCoordinates;
		this.BlockFaceUvCoordinates[1] = bottomUvCoordinates;
		this.BlockFaceUvCoordinates[2] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[4] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[3] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[5] = sideUvCoordinates;
	}

	// Token: 0x060053CB RID: 21451 RVA: 0x00200DB0 File Offset: 0x001FEFB0
	public BlockUVCoordinates(Rect topUvCoordinates, Rect bottomUvCoordinates, Rect northUvCoordinates, Rect southUvCoordinates, Rect westUvCoordinates, Rect eastUvCoordinates)
	{
		this.BlockFaceUvCoordinates[0] = topUvCoordinates;
		this.BlockFaceUvCoordinates[1] = bottomUvCoordinates;
		this.BlockFaceUvCoordinates[2] = northUvCoordinates;
		this.BlockFaceUvCoordinates[4] = southUvCoordinates;
		this.BlockFaceUvCoordinates[3] = westUvCoordinates;
		this.BlockFaceUvCoordinates[5] = eastUvCoordinates;
	}

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x060053CC RID: 21452 RVA: 0x00200E20 File Offset: 0x001FF020
	public Rect[] BlockFaceUvCoordinates
	{
		get
		{
			return this.m_BlockFaceUvCoordinates;
		}
	}

	// Token: 0x0400418F RID: 16783
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Rect[] m_BlockFaceUvCoordinates = new Rect[6];
}
