using System;

namespace SharpEXR.ColorSpace
{
	// Token: 0x02001729 RID: 5929
	public struct tMat3x3
	{
		// Token: 0x0600B940 RID: 47424 RVA: 0x004456F2 File Offset: 0x004438F2
		public void SetCol(int colIdx, tVec3 vec)
		{
			this[0, colIdx] = vec.X;
			this[1, colIdx] = vec.Y;
			this[2, colIdx] = vec.Z;
		}

		// Token: 0x0600B941 RID: 47425 RVA: 0x00445720 File Offset: 0x00443920
		public bool Invert(out tMat3x3 result)
		{
			result = default(tMat3x3);
			float num = this[1, 1] * this[2, 2] - this[1, 2] * this[2, 1];
			float num2 = this[1, 2] * this[2, 0] - this[1, 0] * this[2, 2];
			float num3 = this[1, 0] * this[2, 1] - this[1, 1] * this[2, 0];
			float num4 = this[0, 0] * num + this[0, 1] * num2 + this[0, 2] * num3;
			if (num4 > -1E-06f && num4 < 1E-06f)
			{
				return false;
			}
			float num5 = 1f / num4;
			result[0, 0] = num5 * num;
			result[0, 1] = num5 * (this[2, 1] * this[0, 2] - this[2, 2] * this[0, 1]);
			result[0, 2] = num5 * (this[0, 1] * this[1, 2] - this[0, 2] * this[1, 1]);
			result[1, 0] = num5 * num2;
			result[1, 1] = num5 * (this[2, 2] * this[0, 0] - this[2, 0] * this[0, 2]);
			result[1, 2] = num5 * (this[0, 2] * this[1, 0] - this[0, 0] * this[1, 2]);
			result[2, 0] = num5 * num3;
			result[2, 1] = num5 * (this[2, 0] * this[0, 1] - this[2, 1] * this[0, 0]);
			result[2, 2] = num5 * (this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0]);
			return true;
		}

		// Token: 0x0600B942 RID: 47426 RVA: 0x00445918 File Offset: 0x00443B18
		public static tVec3 operator *(tMat3x3 mat, tVec3 vec)
		{
			return new tVec3(mat[0, 0] * vec.X + mat[0, 1] * vec.Y + mat[0, 2] * vec.Z, mat[1, 0] * vec.X + mat[1, 1] * vec.Y + mat[1, 2] * vec.Z, mat[2, 0] * vec.X + mat[2, 1] * vec.Y + mat[2, 2] * vec.Z);
		}

		// Token: 0x1700173D RID: 5949
		public float this[int row, int col]
		{
			get
			{
				if (row != 0)
				{
					if (row != 1)
					{
						if (col == 1)
						{
							return this.M21;
						}
						if (col != 2)
						{
							return this.M20;
						}
						return this.M22;
					}
					else
					{
						if (col == 1)
						{
							return this.M11;
						}
						if (col != 2)
						{
							return this.M10;
						}
						return this.M12;
					}
				}
				else
				{
					if (col == 1)
					{
						return this.M01;
					}
					if (col != 2)
					{
						return this.M00;
					}
					return this.M02;
				}
			}
			set
			{
				if (row != 0)
				{
					if (row != 1)
					{
						if (col == 1)
						{
							this.M21 = value;
							return;
						}
						if (col != 2)
						{
							this.M20 = value;
							return;
						}
						this.M22 = value;
						return;
					}
					else
					{
						if (col == 1)
						{
							this.M11 = value;
							return;
						}
						if (col != 2)
						{
							this.M10 = value;
							return;
						}
						this.M12 = value;
						return;
					}
				}
				else
				{
					if (col == 1)
					{
						this.M01 = value;
						return;
					}
					if (col != 2)
					{
						this.M00 = value;
						return;
					}
					this.M02 = value;
					return;
				}
			}
		}

		// Token: 0x04008A33 RID: 35379
		public float M00;

		// Token: 0x04008A34 RID: 35380
		public float M01;

		// Token: 0x04008A35 RID: 35381
		public float M02;

		// Token: 0x04008A36 RID: 35382
		public float M10;

		// Token: 0x04008A37 RID: 35383
		public float M11;

		// Token: 0x04008A38 RID: 35384
		public float M12;

		// Token: 0x04008A39 RID: 35385
		public float M20;

		// Token: 0x04008A3A RID: 35386
		public float M21;

		// Token: 0x04008A3B RID: 35387
		public float M22;
	}
}
