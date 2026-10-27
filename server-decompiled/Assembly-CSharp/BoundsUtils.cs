using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020013DE RID: 5086
public class BoundsUtils
{
	// Token: 0x06009FB6 RID: 40886 RVA: 0x003C4BFF File Offset: 0x003C2DFF
	public static Bounds BoundsForMinMax(float mnx, float mny, float mnz, float mxx, float mxy, float mxz)
	{
		return BoundsUtils.BoundsForMinMax(new Vector3(mnx, mny, mnz), new Vector3(mxx, mxy, mxz));
	}

	// Token: 0x06009FB7 RID: 40887 RVA: 0x003C4C18 File Offset: 0x003C2E18
	public static Bounds BoundsForMinMax(Vector3 _v1, Vector3 _v2)
	{
		Vector3 vector = _v2 - _v1;
		return new Bounds(_v1 + vector / 2f, vector);
	}

	// Token: 0x06009FB8 RID: 40888 RVA: 0x003C4C44 File Offset: 0x003C2E44
	public static Bounds ExpandBounds(Bounds bounds, float x, float y, float z)
	{
		bounds.Expand(new Vector3(x, y, z));
		return bounds;
	}

	// Token: 0x06009FB9 RID: 40889 RVA: 0x003C4C56 File Offset: 0x003C2E56
	public static Bounds ContractBounds(Bounds bounds, float x, float y, float z)
	{
		return BoundsUtils.ExpandBounds(bounds, -x, -y, -z);
	}

	// Token: 0x06009FBA RID: 40890 RVA: 0x003C4C64 File Offset: 0x003C2E64
	public static float ClipBoundsMoveY(Vector3 bmins, Vector3 bmaxs, float move, Bounds collider)
	{
		Vector3 min = collider.min;
		Vector3 max = collider.max;
		if (move != 0f && bmaxs.x > min.x && bmins.x < max.x && bmaxs.z > min.z && bmins.z < max.z)
		{
			if (move > 0f && min.y >= bmaxs.y)
			{
				move = MathUtils.Clamp(min.y - bmaxs.y, 0f, move);
			}
			else if (move < 0f && max.y <= bmins.y)
			{
				move = MathUtils.Clamp(max.y - bmins.y, move, 0f);
			}
			else if (move < 0f)
			{
				float num = max.y - bmins.y;
				if (num < 0.2f)
				{
					move = num;
				}
			}
			if (Math.Abs(move) < 0.0001f)
			{
				move = 0f;
			}
		}
		return move;
	}

	// Token: 0x06009FBB RID: 40891 RVA: 0x003C4D6C File Offset: 0x003C2F6C
	public static float ClipBoundsMoveX(Vector3 bmins, Vector3 bmaxs, float move, Bounds collider)
	{
		Vector3 min = collider.min;
		Vector3 max = collider.max;
		if (move != 0f && bmaxs.y > min.y && bmins.y < max.y && bmaxs.z > min.z && bmins.z < max.z)
		{
			if (move > 0f && min.x >= bmaxs.x)
			{
				move = MathUtils.Clamp(min.x - bmaxs.x, 0f, move);
			}
			else if (move < 0f && max.x <= bmins.x)
			{
				move = MathUtils.Clamp(max.x - bmins.x, move, 0f);
			}
			if (Math.Abs(move) < 0.0001f)
			{
				move = 0f;
			}
		}
		return move;
	}

	// Token: 0x06009FBC RID: 40892 RVA: 0x003C4E4C File Offset: 0x003C304C
	public static float ClipBoundsMoveZ(Vector3 bmins, Vector3 bmaxs, float move, Bounds collider)
	{
		Vector3 min = collider.min;
		Vector3 max = collider.max;
		if (move != 0f && bmaxs.x > min.x && bmins.x < max.x && bmaxs.y > min.y && bmins.y < max.y)
		{
			if (move > 0f && min.z >= bmaxs.z)
			{
				move = MathUtils.Clamp(min.z - bmaxs.z, 0f, move);
			}
			else if (move < 0f && max.z <= bmins.z)
			{
				move = MathUtils.Clamp(max.z - bmins.z, move, 0f);
			}
			if (Math.Abs(move) < 0.0001f)
			{
				move = 0f;
			}
		}
		return move;
	}

	// Token: 0x06009FBD RID: 40893 RVA: 0x003C4F2C File Offset: 0x003C312C
	public static Vector3 ClipBoundsMove(Bounds bounds, Vector3 move, IList<Bounds> colliderList, int numColliders)
	{
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		move.y = BoundsUtils.ClipBoundsMoveY(min, max, move.y, colliderList, numColliders);
		min.y += move.y;
		max.y += move.y;
		move.x = BoundsUtils.ClipBoundsMoveX(min, max, move.x, colliderList, numColliders);
		min.x += move.x;
		max.x += move.x;
		move.z = BoundsUtils.ClipBoundsMoveZ(min, max, move.z, colliderList, numColliders);
		return move;
	}

	// Token: 0x06009FBE RID: 40894 RVA: 0x003C4FD0 File Offset: 0x003C31D0
	public static float ClipBoundsMoveY(Vector3 bmins, Vector3 bmaxs, float move, IList<Bounds> colliderList, int numColliders)
	{
		if (move != 0f)
		{
			for (int i = 0; i < numColliders; i++)
			{
				Bounds bounds = colliderList[i];
				Vector3 min = bounds.min;
				Vector3 max = bounds.max;
				if (bmaxs.x > min.x + 0f && bmins.x < max.x - 0f && bmaxs.z > min.z + 0f && bmins.z < max.z - 0f)
				{
					if (move > 0f && min.y >= bmaxs.y + 0f)
					{
						move = MathUtils.Clamp(min.y - bmaxs.y, 0f, move);
					}
					else if (move < 0f && max.y <= bmins.y - 0f)
					{
						move = MathUtils.Clamp(max.y - bmins.y, move, 0f);
					}
					else if (move < 0f)
					{
						float num = max.y - bmins.y;
						if (num < 0.2f)
						{
							move = num;
						}
					}
					if (Math.Abs(move) < 0.0001f)
					{
						move = 0f;
						break;
					}
				}
			}
		}
		return move;
	}

	// Token: 0x06009FBF RID: 40895 RVA: 0x003C511C File Offset: 0x003C331C
	public static float ClipBoundsMoveX(Vector3 bmins, Vector3 bmaxs, float move, IList<Bounds> colliderList, int numColliders)
	{
		if (move != 0f)
		{
			for (int i = 0; i < numColliders; i++)
			{
				Bounds bounds = colliderList[i];
				Vector3 min = bounds.min;
				Vector3 max = bounds.max;
				if (bmaxs.y > min.y + 0f && bmins.y < max.y - 0f && bmaxs.z > min.z + 0f && bmins.z < max.z - 0f)
				{
					if (move > 0f && min.x >= bmaxs.x + 0f)
					{
						move = MathUtils.Clamp(min.x - bmaxs.x, 0f, move);
					}
					else if (move < 0f && max.x <= bmins.x - 0f)
					{
						move = MathUtils.Clamp(max.x - bmins.x, move, 0f);
					}
					if (Math.Abs(move) < 0.0001f)
					{
						move = 0f;
						break;
					}
				}
			}
		}
		return move;
	}

	// Token: 0x06009FC0 RID: 40896 RVA: 0x003C5240 File Offset: 0x003C3440
	public static float ClipBoundsMoveZ(Vector3 bmins, Vector3 bmaxs, float move, IList<Bounds> colliderList, int numColliders)
	{
		if (move != 0f)
		{
			for (int i = 0; i < numColliders; i++)
			{
				Bounds bounds = colliderList[i];
				Vector3 min = bounds.min;
				Vector3 max = bounds.max;
				if (bmaxs.x > min.x + 0f && bmins.x < max.x - 0f && bmaxs.y > min.y + 0f && bmins.y < max.y - 0f)
				{
					if (move > 0f && min.z >= bmaxs.z + 0f)
					{
						move = MathUtils.Clamp(min.z - bmaxs.z, 0f, move);
					}
					else if (move < 0f && max.z <= bmins.z - 0f)
					{
						move = MathUtils.Clamp(max.z - bmins.z, move, 0f);
					}
					if (Math.Abs(move) < 0.0001f)
					{
						move = 0f;
						break;
					}
				}
			}
		}
		return move;
	}

	// Token: 0x06009FC1 RID: 40897 RVA: 0x003C5364 File Offset: 0x003C3564
	public static bool Intersects(Bounds bounds, Vector3 min1, Vector3 max1)
	{
		Vector3 min2 = bounds.min;
		Vector3 max2 = bounds.max;
		return min2.x <= max1.x && max2.x >= min1.x && min2.y <= max1.y && max2.y >= min1.y && min2.z <= max1.z && max2.z >= min1.z;
	}

	// Token: 0x06009FC2 RID: 40898 RVA: 0x003C53DC File Offset: 0x003C35DC
	public static Bounds ExpandDirectional(Bounds bounds, Vector3 move)
	{
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		if (move.x < 0f)
		{
			min.x += move.x;
		}
		else
		{
			max.x += move.x;
		}
		if (move.y < 0f)
		{
			min.y += move.y;
		}
		else
		{
			max.y += move.y;
		}
		if (move.z < 0f)
		{
			min.z += move.z;
		}
		else
		{
			max.z += move.z;
		}
		bounds.SetMinMax(min, max);
		return bounds;
	}

	// Token: 0x06009FC3 RID: 40899 RVA: 0x003C5498 File Offset: 0x003C3698
	public static void WriteBounds(BinaryWriter _bw, Bounds bounds)
	{
		_bw.Write(bounds.min.x);
		_bw.Write(bounds.min.y);
		_bw.Write(bounds.min.z);
		_bw.Write(bounds.max.x);
		_bw.Write(bounds.max.y);
		_bw.Write(bounds.max.z);
	}

	// Token: 0x06009FC4 RID: 40900 RVA: 0x003C5514 File Offset: 0x003C3714
	public static Bounds ReadBounds(BinaryReader _br)
	{
		Bounds bounds = default(Bounds);
		bounds.SetMinMax(new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle()), new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle()));
		return bounds;
	}

	// Token: 0x04007902 RID: 30978
	[PublicizedFrom(EAccessModifier.Private)]
	public const float kClipEpsilon = 0f;

	// Token: 0x04007903 RID: 30979
	[PublicizedFrom(EAccessModifier.Private)]
	public const float kMinMoveClamp = 0.0001f;
}
