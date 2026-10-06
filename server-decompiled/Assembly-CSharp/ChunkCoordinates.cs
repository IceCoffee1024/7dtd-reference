using System;
using UnityEngine;

// Token: 0x02000B30 RID: 2864
public class ChunkCoordinates
{
	// Token: 0x06005623 RID: 22051 RVA: 0x0021171C File Offset: 0x0020F91C
	public ChunkCoordinates(int _x, int _y, int _z)
	{
		this.position = new Vector3i(_x, _y, _z);
	}

	// Token: 0x06005624 RID: 22052 RVA: 0x00211732 File Offset: 0x0020F932
	public ChunkCoordinates(ChunkCoordinates _cc)
	{
		this.position = _cc.position;
	}

	// Token: 0x06005625 RID: 22053 RVA: 0x00211746 File Offset: 0x0020F946
	public override bool Equals(object _obj)
	{
		return _obj is ChunkCoordinates && this.position.Equals(((ChunkCoordinates)_obj).position);
	}

	// Token: 0x06005626 RID: 22054 RVA: 0x00211768 File Offset: 0x0020F968
	public override int GetHashCode()
	{
		return this.position.x + this.position.z << 8 + this.position.y << 16;
	}

	// Token: 0x06005627 RID: 22055 RVA: 0x00211798 File Offset: 0x0020F998
	public float getDistance(int _x, int _y, int _z)
	{
		int num = this.position.x - _x;
		int num2 = this.position.y - _y;
		int num3 = this.position.z - _z;
		return Mathf.Sqrt((float)(num * num + num2 * num2 + num3 * num3));
	}

	// Token: 0x06005628 RID: 22056 RVA: 0x002117E0 File Offset: 0x0020F9E0
	public float getDistanceSquared(int _x, int _y, int _z)
	{
		int num = this.position.x - _x;
		int num2 = this.position.y - _y;
		int num3 = this.position.z - _z;
		return (float)(num * num + num2 * num2 + num3 * num3);
	}

	// Token: 0x040042C4 RID: 17092
	public Vector3i position;
}
