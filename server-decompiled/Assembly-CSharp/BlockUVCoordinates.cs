using System;
using UnityEngine;

// Token: 0x02000B1A RID: 2842
public class BlockUVCoordinates
{
	// Token: 0x06005408 RID: 21512 RVA: 0x00205524 File Offset: 0x00203724
	public BlockUVCoordinates(Rect topUvCoordinates, Rect sideUvCoordinates, Rect bottomUvCoordinates)
	{
		this.BlockFaceUvCoordinates[0] = topUvCoordinates;
		this.BlockFaceUvCoordinates[1] = bottomUvCoordinates;
		this.BlockFaceUvCoordinates[2] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[4] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[3] = sideUvCoordinates;
		this.BlockFaceUvCoordinates[5] = sideUvCoordinates;
	}

	// Token: 0x06005409 RID: 21513 RVA: 0x00205594 File Offset: 0x00203794
	public BlockUVCoordinates(Rect topUvCoordinates, Rect bottomUvCoordinates, Rect northUvCoordinates, Rect southUvCoordinates, Rect westUvCoordinates, Rect eastUvCoordinates)
	{
		this.BlockFaceUvCoordinates[0] = topUvCoordinates;
		this.BlockFaceUvCoordinates[1] = bottomUvCoordinates;
		this.BlockFaceUvCoordinates[2] = northUvCoordinates;
		this.BlockFaceUvCoordinates[4] = southUvCoordinates;
		this.BlockFaceUvCoordinates[3] = westUvCoordinates;
		this.BlockFaceUvCoordinates[5] = eastUvCoordinates;
	}

	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x0600540A RID: 21514 RVA: 0x00205604 File Offset: 0x00203804
	public Rect[] BlockFaceUvCoordinates
	{
		get
		{
			return this.m_BlockFaceUvCoordinates;
		}
	}

	// Token: 0x040041F1 RID: 16881
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Rect[] m_BlockFaceUvCoordinates = new Rect[6];
}
