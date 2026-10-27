using System;
using UnityEngine;

// Token: 0x02000B3B RID: 2875
public class ChunkCoordinates
{
	// Token: 0x06005648 RID: 22088 RVA: 0x00212C98 File Offset: 0x00210E98
	public ChunkCoordinates(int _x, int _y, int _z)
	{
		this.position = new Vector3i(_x, _y, _z);
	}

	// Token: 0x06005649 RID: 22089 RVA: 0x00212CAE File Offset: 0x00210EAE
	public ChunkCoordinates(ChunkCoordinates _cc)
	{
		this.position = _cc.position;
	}

	// Token: 0x0600564A RID: 22090 RVA: 0x00212CC2 File Offset: 0x00210EC2
	public override bool Equals(object _obj)
	{
		return _obj is ChunkCoordinates && this.position.Equals(((ChunkCoordinates)_obj).position);
	}

	// Token: 0x0600564B RID: 22091 RVA: 0x00212CE4 File Offset: 0x00210EE4
	public override int GetHashCode()
	{
		return this.position.x + this.position.z << 8 + this.position.y << 16;
	}

	// Token: 0x0600564C RID: 22092 RVA: 0x00212D14 File Offset: 0x00210F14
	public float getDistance(int _x, int _y, int _z)
	{
		int num = this.position.x - _x;
		int num2 = this.position.y - _y;
		int num3 = this.position.z - _z;
		return Mathf.Sqrt((float)(num * num + num2 * num2 + num3 * num3));
	}

	// Token: 0x0600564D RID: 22093 RVA: 0x00212D5C File Offset: 0x00210F5C
	public float getDistanceSquared(int _x, int _y, int _z)
	{
		int num = this.position.x - _x;
		int num2 = this.position.y - _y;
		int num3 = this.position.z - _z;
		return (float)(num * num + num2 * num2 + num3 * num3);
	}

	// Token: 0x040042E9 RID: 17129
	public Vector3i position;
}
