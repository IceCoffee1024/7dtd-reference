using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020013CC RID: 5068
public class BoundsUtils
{
	// Token: 0x06009F5E RID: 40798 RVA: 0x003C27FB File Offset: 0x003C09FB
	public static Bounds BoundsForMinMax(float mnx, float mny, float mnz, float mxx, float mxy, float mxz)
	{
		return BoundsUtils.BoundsForMinMax(new Vector3(mnx, mny, mnz), new Vector3(mxx, mxy, mxz));
	}

	// Token: 0x06009F5F RID: 40799 RVA: 0x003C2814 File Offset: 0x003C0A14
	public static Bounds BoundsForMinMax(Vector3 _v1, Vector3 _v2)
	{
		Vector3 vector = _v2 - _v1;
		return new Bounds(_v1 + vector / 2f, vector);
	}

	// Token: 0x06009F60 RID: 40800 RVA: 0x003C2840 File Offset: 0x003C0A40
	public static Bounds ExpandBounds(Bounds bounds, float x, float y, float z)
	{
		bounds.Expand(new Vector3(x, y, z));
		return bounds;
	}

	// Token: 0x06009F61 RID: 40801 RVA: 0x003C2852 File Offset: 0x003C0A52
	public static Bounds ContractBounds(Bounds bounds, float x, float y, float z)
	{
		return BoundsUtils.ExpandBounds(bounds, -x, -y, -z);
	}

	// Token: 0x06009F62 RID: 40802 RVA: 0x003C2860 File Offset: 0x003C0A60
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

	// Token: 0x06009F63 RID: 40803 RVA: 0x003C2968 File Offset: 0x003C0B68
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

	// Token: 0x06009F64 RID: 40804 RVA: 0x003C2A48 File Offset: 0x003C0C48
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

	// Token: 0x06009F65 RID: 40805 RVA: 0x003C2B28 File Offset: 0x003C0D28
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

	// Token: 0x06009F66 RID: 40806 RVA: 0x003C2BCC File Offset: 0x003C0DCC
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

	// Token: 0x06009F67 RID: 40807 RVA: 0x003C2D18 File Offset: 0x003C0F18
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

	// Token: 0x06009F68 RID: 40808 RVA: 0x003C2E3C File Offset: 0x003C103C
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

	// Token: 0x06009F69 RID: 40809 RVA: 0x003C2F60 File Offset: 0x003C1160
	public static bool Intersects(Bounds bounds, Vector3 min1, Vector3 max1)
	{
		Vector3 min2 = bounds.min;
		Vector3 max2 = bounds.max;
		return min2.x <= max1.x && max2.x >= min1.x && min2.y <= max1.y && max2.y >= min1.y && min2.z <= max1.z && max2.z >= min1.z;
	}

	// Token: 0x06009F6A RID: 40810 RVA: 0x003C2FD8 File Offset: 0x003C11D8
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

	// Token: 0x06009F6B RID: 40811 RVA: 0x003C3094 File Offset: 0x003C1294
	public static void WriteBounds(BinaryWriter _bw, Bounds bounds)
	{
		_bw.Write(bounds.min.x);
		_bw.Write(bounds.min.y);
		_bw.Write(bounds.min.z);
		_bw.Write(bounds.max.x);
		_bw.Write(bounds.max.y);
		_bw.Write(bounds.max.z);
	}

	// Token: 0x06009F6C RID: 40812 RVA: 0x003C3110 File Offset: 0x003C1310
	public static Bounds ReadBounds(BinaryReader _br)
	{
		Bounds bounds = default(Bounds);
		bounds.SetMinMax(new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle()), new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle()));
		return bounds;
	}

	// Token: 0x040078C1 RID: 30913
	[PublicizedFrom(EAccessModifier.Private)]
	public const float kClipEpsilon = 0f;

	// Token: 0x040078C2 RID: 30914
	[PublicizedFrom(EAccessModifier.Private)]
	public const float kMinMoveClamp = 0.0001f;
}
