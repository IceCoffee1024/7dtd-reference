using System;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class PropReference : MonoBehaviour
{
	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0004C35F File Offset: 0x0004A55F
	// (set) Token: 0x06000C34 RID: 3124 RVA: 0x0004C372 File Offset: 0x0004A572
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
	// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0004C38C File Offset: 0x0004A58C
	// (set) Token: 0x06000C36 RID: 3126 RVA: 0x0004C3C7 File Offset: 0x0004A5C7
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

	// Token: 0x06000C37 RID: 3127 RVA: 0x0004C3F8 File Offset: 0x0004A5F8
	public override string ToString()
	{
		return this.PropRef.ToString();
	}

	// Token: 0x04000B04 RID: 2820
	public int ChunkX;

	// Token: 0x04000B05 RID: 2821
	public int ChunkZ;

	// Token: 0x04000B06 RID: 2822
	public int PropId;
}
