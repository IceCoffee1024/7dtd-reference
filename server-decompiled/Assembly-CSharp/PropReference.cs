using System;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class PropReference : MonoBehaviour
{
	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0004C777 File Offset: 0x0004A977
	// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0004C78A File Offset: 0x0004A98A
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

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0004C7A4 File Offset: 0x0004A9A4
	// (set) Token: 0x06000C41 RID: 3137 RVA: 0x0004C7DF File Offset: 0x0004A9DF
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

	// Token: 0x06000C42 RID: 3138 RVA: 0x0004C810 File Offset: 0x0004AA10
	public override string ToString()
	{
		return this.PropRef.ToString();
	}

	// Token: 0x04000B11 RID: 2833
	public int ChunkX;

	// Token: 0x04000B12 RID: 2834
	public int ChunkZ;

	// Token: 0x04000B13 RID: 2835
	public int PropId;
}
