using System;
using UnityEngine;

// Token: 0x02000B0C RID: 2828
public class ChunkCoordinates
{
	// Token: 0x06005582 RID: 21890 RVA: 0x0020B790 File Offset: 0x00209990
	public ChunkCoordinates(int _x, int _y, int _z)
	{
		this.position = new Vector3i(_x, _y, _z);
	}

	// Token: 0x06005583 RID: 21891 RVA: 0x0020B7A6 File Offset: 0x002099A6
	public ChunkCoordinates(ChunkCoordinates _cc)
	{
		this.position = _cc.position;
	}

	// Token: 0x06005584 RID: 21892 RVA: 0x0020B7BA File Offset: 0x002099BA
	public override bool Equals(object _obj)
	{
		return _obj is ChunkCoordinates && this.position.Equals(((ChunkCoordinates)_obj).position);
	}

	// Token: 0x06005585 RID: 21893 RVA: 0x0020B7DC File Offset: 0x002099DC
	public override int GetHashCode()
	{
		return this.position.x + this.position.z << 8 + this.position.y << 16;
	}

	// Token: 0x06005586 RID: 21894 RVA: 0x0020B80C File Offset: 0x00209A0C
	public float getDistance(int _x, int _y, int _z)
	{
		int num = this.position.x - _x;
		int num2 = this.position.y - _y;
		int num3 = this.position.z - _z;
		return Mathf.Sqrt((float)(num * num + num2 * num2 + num3 * num3));
	}

	// Token: 0x06005587 RID: 21895 RVA: 0x0020B854 File Offset: 0x00209A54
	public float getDistanceSquared(int _x, int _y, int _z)
	{
		int num = this.position.x - _x;
		int num2 = this.position.y - _y;
		int num3 = this.position.z - _z;
		return (float)(num * num + num2 * num2 + num3 * num3);
	}

	// Token: 0x040041EC RID: 16876
	public Vector3i position;
}
