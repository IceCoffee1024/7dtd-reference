using System;
using UnityEngine;

// Token: 0x02000B0F RID: 2831
public class BlockUVCoordinates
{
	// Token: 0x060053E3 RID: 21475 RVA: 0x00203FA0 File Offset: 0x002021A0
	public BlockUVCoordinates(Rect topUvCoordinates, Rect sideUvCoordinates, Rect bottomUvCoordinates)
	{
		this.BlockFaceUvCoordinates[0] = topUvCoordinates;
		this.BlockFaceUvCoordinates[1] = bottomUvCoordinates;
		this.BlockFaceUvCoordinates[2] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[4] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[3] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[5] = sideUvCoordinates;
	}

	// Token: 0x060053E4 RID: 21476 RVA: 0x00204010 File Offset: 0x00202210
	public BlockUVCoordinates(Rect topUvCoordinates, Rect bottomUvCoordinates, Rect northUvCoordinates, Rect southUvCoordinates, Rect westUvCoordinates, Rect eastUvCoordinates)
	{
		this.BlockFaceUvCoordinates[0] = topUvCoordinates;
		this.BlockFaceUvCoordinates[1] = bottomUvCoordinates;
		this.BlockFaceUvCoordinates[2] = northUvCoordinates;
		this.BlockFaceUvCoordinates[4] = southUvCoordinates;
		this.BlockFaceUvCoordinates[3] = westUvCoordinates;
		this.BlockFaceUvCoordinates[5] = eastUvCoordinates;
	}

	// Token: 0x17000930 RID: 2352
	// (get) Token: 0x060053E5 RID: 21477 RVA: 0x00204080 File Offset: 0x00202280
	public Rect[] BlockFaceUvCoordinates
	{
		get
		{
			return this.m_BlockFaceUvCoordinates;
		}
	}

	// Token: 0x040041CC RID: 16844
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Rect[] m_BlockFaceUvCoordinates = new Rect[6];
}
