using System;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class PropReference : MonoBehaviour
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0004C77B File Offset: 0x0004A97B
	// (set) Token: 0x06000C36 RID: 3126 RVA: 0x0004C78E File Offset: 0x0004A98E
	public Vector2i ChunkPos
	{
		get
		{
			return new Vector2i(this.ChunkX, this.ChunkZ);
		}
		set
		{
			this.ChunkX = value.x;
			this.ChunkZ = value.y;
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0004C7A8 File Offset: 0x0004A9A8
	// (set) Token: 0x06000C38 RID: 3128 RVA: 0x0004C7E3 File Offset: 0x0004A9E3
	public PropRef PropRef
	{
		get
		{
			return new PropRef
			{
				ChunkPos = new Vector2i(this.ChunkX, this.ChunkZ),
				PropId = this.PropId
			};
		}
		set
		{
			this.ChunkX = value.ChunkPos.x;
			this.ChunkZ = value.ChunkPos.y;
			this.PropId = value.PropId;
		}
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x0004C814 File Offset: 0x0004AA14
	public override string ToString()
	{
		return this.PropRef.ToString();
	}

	// Token: 0x04000B0A RID: 2826
	public int ChunkX;

	// Token: 0x04000B0B RID: 2827
	public int ChunkZ;

	// Token: 0x04000B0C RID: 2828
	public int PropId;
}
