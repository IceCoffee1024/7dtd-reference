using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020013AC RID: 5036
public class BoundsUtils
{
	// Token: 0x06009EB3 RID: 40627 RVA: 0x003BDC5F File Offset: 0x003BBE5F
	public static Bounds BoundsForMinMax(float mnx, float mny, float mnz, float mxx, float mxy, float mxz)
	{
		return BoundsUtils.BoundsForMinMax(new Vector3(mnx, mny, mnz), new Vector3(mxx, mxy, mxz));
	}

	// Token: 0x06009EB4 RID: 40628 RVA: 0x003BDC78 File Offset: 0x003BBE78
	public static Bounds BoundsForMinMax(Vector3 _v1, Vector3 _v2)
	{
		Vector3 vector = _v2 - _v1;
		return new Bounds(_v1 + vector / 2f, vector);
	}

	// Token: 0x06009EB5 RID: 40629 RVA: 0x003BDCA4 File Offset: 0x003BBEA4
	public static Bounds ExpandBounds(Bounds bounds, float x, float y, float z)
	{
		bounds.Expand(new Vector3(x, y, z));
		return bounds;
	}

	// Token: 0x06009EB6 RID: 40630 RVA: 0x003BDCB6 File Offset: 0x003BBEB6
	public static Bounds ContractBounds(Bounds bounds, float x, float y, float z)
	{
		return BoundsUtils.ExpandBounds(bounds, -x, -y, -z);
	}

	// Token: 0x06009EB7 RID: 40631 RVA: 0x003BDCC4 File Offset: 0x003BBEC4
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

	// Token: 0x06009EB8 RID: 40632 RVA: 0x003BDDCC File Offset: 0x003BBFCC
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

	// Token: 0x06009EB9 RID: 40633 RVA: 0x003BDEAC File Offset: 0x003BC0AC
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

	// Token: 0x06009EBA RID: 40634 RVA: 0x003BDF8C File Offset: 0x003BC18C
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

	// Token: 0x06009EBB RID: 40635 RVA: 0x003BE030 File Offset: 0x003BC230
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

	// Token: 0x06009EBC RID: 40636 RVA: 0x003BE17C File Offset: 0x003BC37C
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

	// Token: 0x06009EBD RID: 40637 RVA: 0x003BE2A0 File Offset: 0x003BC4A0
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

	// Token: 0x06009EBE RID: 40638 RVA: 0x003BE3C4 File Offset: 0x003BC5C4
	public static bool Intersects(Bounds bounds, Vector3 min1, Vector3 max1)
	{
		Vector3 min2 = bounds.min;
		Vector3 max2 = bounds.max;
		return min2.x <= max1.x && max2.x >= min1.x && min2.y <= max1.y && max2.y >= min1.y && min2.z <= max1.z && max2.z >= min1.z;
	}

	// Token: 0x06009EBF RID: 40639 RVA: 0x003BE43C File Offset: 0x003BC63C
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

	// Token: 0x06009EC0 RID: 40640 RVA: 0x003BE4F8 File Offset: 0x003BC6F8
	public static void WriteBounds(BinaryWriter _bw, Bounds bounds)
	{
		_bw.Write(bounds.min.x);
		_bw.Write(bounds.min.y);
		_bw.Write(bounds.min.z);
		_bw.Write(bounds.max.x);
		_bw.Write(bounds.max.y);
		_bw.Write(bounds.max.z);
	}

	// Token: 0x06009EC1 RID: 40641 RVA: 0x003BE574 File Offset: 0x003BC774
	public static Bounds ReadBounds(BinaryReader _br)
	{
		Bounds bounds = default(Bounds);
		bounds.SetMinMax(new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle()), new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle()));
		return bounds;
	}

	// Token: 0x04007846 RID: 30790
	[PublicizedFrom(EAccessModifier.Private)]
	public const float kClipEpsilon = 0f;

	// Token: 0x04007847 RID: 30791
	[PublicizedFrom(EAccessModifier.Private)]
	public const float kMinMoveClamp = 0.0001f;
}
