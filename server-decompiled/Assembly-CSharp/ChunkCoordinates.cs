using System;
using UnityEngine;

// Token: 0x02000B1E RID: 2846
public class ChunkCoordinates
{
	// Token: 0x0600560A RID: 22026 RVA: 0x0020E4AC File Offset: 0x0020C6AC
	public ChunkCoordinates(int _x, int _y, int _z)
	{
		this.position = new Vector3i(_x, _y, _z);
	}

	// Token: 0x0600560B RID: 22027 RVA: 0x0020E4C2 File Offset: 0x0020C6C2
	public ChunkCoordinates(ChunkCoordinates _cc)
	{
		this.position = _cc.position;
	}

	// Token: 0x0600560C RID: 22028 RVA: 0x0020E4D6 File Offset: 0x0020C6D6
	public override bool Equals(object _obj)
	{
		return _obj is ChunkCoordinates && this.position.Equals(((ChunkCoordinates)_obj).position);
	}

	// Token: 0x0600560D RID: 22029 RVA: 0x0020E4F8 File Offset: 0x0020C6F8
	public override int GetHashCode()
	{
		return this.position.x + this.position.z << 8 + this.position.y << 16;
	}

	// Token: 0x0600560E RID: 22030 RVA: 0x0020E528 File Offset: 0x0020C728
	public float getDistance(int _x, int _y, int _z)
	{
		int num = this.position.x - _x;
		int num2 = this.position.y - _y;
		int num3 = this.position.z - _z;
		return Mathf.Sqrt((float)(num * num + num2 * num2 + num3 * num3));
	}

	// Token: 0x0600560F RID: 22031 RVA: 0x0020E570 File Offset: 0x0020C770
	public float getDistanceSquared(int _x, int _y, int _z)
	{
		int num = this.position.x - _x;
		int num2 = this.position.y - _y;
		int num3 = this.position.z - _z;
		return (float)(num * num + num2 * num2 + num3 * num3);
	}

	// Token: 0x04004287 RID: 17031
	public Vector3i position;
}
