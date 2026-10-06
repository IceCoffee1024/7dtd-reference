using System;

namespace SharpEXR.ColorSpace
{
	// Token: 0x0200170A RID: 5898
	public struct tMat3x3
	{
		// Token: 0x0600B86C RID: 47212 RVA: 0x00442C4E File Offset: 0x00440E4E
		public void SetCol(int colIdx, tVec3 vec)
		{
			this[0, colIdx] = vec.X;
			this[1, colIdx] = vec.Y;
			this[2, colIdx] = vec.Z;
		}

		// Token: 0x0600B86D RID: 47213 RVA: 0x00442C7C File Offset: 0x00440E7C
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

		// Token: 0x0600B86E RID: 47214 RVA: 0x00442E74 File Offset: 0x00441074
		public static tVec3 operator *(tMat3x3 mat, tVec3 vec)
		{
			return new tVec3(mat[0, 0] * vec.X + mat[0, 1] * vec.Y + mat[0, 2] * vec.Z, mat[1, 0] * vec.X + mat[1, 1] * vec.Y + mat[1, 2] * vec.Z, mat[2, 0] * vec.X + mat[2, 1] * vec.Y + mat[2, 2] * vec.Z);
		}

		// Token: 0x170016F6 RID: 5878
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

		// Token: 0x04008980 RID: 35200
		public float M00;

		// Token: 0x04008981 RID: 35201
		public float M01;

		// Token: 0x04008982 RID: 35202
		public float M02;

		// Token: 0x04008983 RID: 35203
		public float M10;

		// Token: 0x04008984 RID: 35204
		public float M11;

		// Token: 0x04008985 RID: 35205
		public float M12;

		// Token: 0x04008986 RID: 35206
		public float M20;

		// Token: 0x04008987 RID: 35207
		public float M21;

		// Token: 0x04008988 RID: 35208
		public float M22;
	}
}
