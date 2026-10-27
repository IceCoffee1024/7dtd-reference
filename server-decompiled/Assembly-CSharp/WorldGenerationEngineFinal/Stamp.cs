using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001782 RID: 6018
	public class Stamp
	{
		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x0600BAC3 RID: 47811 RVA: 0x0044E444 File Offset: 0x0044C644
		public int imageHeight
		{
			get
			{
				return this.stamp.height;
			}
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x0600BAC4 RID: 47812 RVA: 0x0044E451 File Offset: 0x0044C651
		public int imageWidth
		{
			get
			{
				return this.stamp.width;
			}
		}

		// Token: 0x0600BAC5 RID: 47813 RVA: 0x0044E460 File Offset: 0x0044C660
		public Stamp(WorldBuilder _worldBuilder, RawStamp _stamp, TranslationData _transData, Color32 _customColor = default(Color32), float _alphaCutoff = 0.1f, bool _isWater = false, string stampName = "")
		{
			this.worldBuilder = _worldBuilder;
			this.stamp = _stamp;
			this.transform = _transData;
			this.scale = this.transform.scale;
			this.customColor = _customColor;
			this.alphaCutoff = _alphaCutoff;
			this.isWater = _isWater;
			this.Name = stampName;
			this.alpha = 1f;
			this.additive = false;
			int rotation = this.transform.rotation;
			int num = (int)((float)_stamp.width * this.scale * 1.4f);
			int num2 = (int)((float)_stamp.height * this.scale * 1.4f);
			int num3 = this.transform.x - num / 2;
			int num4 = this.transform.x + num / 2;
			int num5 = this.transform.y - num2 / 2;
			int num6 = this.transform.y + num2 / 2;
			int x = this.transform.x;
			int y = this.transform.y;
			Vector2i rotatedPoint = this.getRotatedPoint(num3, num5, x, y, rotation);
			Vector2i rotatedPoint2 = this.getRotatedPoint(num4, num5, x, y, rotation);
			Vector2i rotatedPoint3 = this.getRotatedPoint(num3, num6, x, y, rotation);
			Vector2i rotatedPoint4 = this.getRotatedPoint(num4, num6, x, y, rotation);
			Vector2 vector = new Vector2((float)Mathf.Min(Mathf.Min(rotatedPoint.x, rotatedPoint2.x), Mathf.Min(rotatedPoint3.x, rotatedPoint4.x)), (float)Mathf.Min(Mathf.Min(rotatedPoint.y, rotatedPoint2.y), Mathf.Min(rotatedPoint3.y, rotatedPoint4.y)));
			Vector2 vector2 = new Vector2((float)Mathf.Max(Mathf.Max(rotatedPoint.x, rotatedPoint2.x), Mathf.Max(rotatedPoint3.x, rotatedPoint4.x)), (float)Mathf.Max(Mathf.Max(rotatedPoint.y, rotatedPoint2.y), Mathf.Max(rotatedPoint3.y, rotatedPoint4.y)));
			this.Area = new Rect(vector, vector2 - vector);
			if (this.isWater)
			{
				if (this.worldBuilder.waterRects == null)
				{
					this.worldBuilder.waterRects = new List<Rect>();
				}
				this.worldBuilder.waterRects.Add(this.Area);
			}
		}

		// Token: 0x0600BAC6 RID: 47814 RVA: 0x0044E6BC File Offset: 0x0044C8BC
		[PublicizedFrom(EAccessModifier.Private)]
		public Color[] rotateColorArray(Color[] src, float angle, int width, int height)
		{
			Color[] array = new Color[width * height];
			double num = Math.Sin(0.017453292519943295 * (double)angle);
			double num2 = Math.Cos(0.017453292519943295 * (double)angle);
			int num3 = width / 2;
			int num4 = height / 2;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					float num5 = (float)(num2 * (double)(j - num3) + num * (double)(i - num4) + (double)num3);
					float num6 = (float)(-(float)num * (double)(j - num3) + num2 * (double)(i - num4) + (double)num4);
					int num7 = (int)num5;
					int num8 = (int)num6;
					num5 -= (float)num7;
					num6 -= (float)num8;
					if (num7 >= 0 && num7 < width && num8 >= 0 && num8 < height)
					{
						Color color = src[num8 * width + num7];
						Color color2 = color;
						Color color3 = color;
						Color color4 = color;
						if (num7 + 1 < width)
						{
							color2 = src[num8 * width + num7 + 1];
						}
						if (num8 + 1 < height)
						{
							color3 = src[(num8 + 1) * width + num7];
						}
						if (num7 + 1 < width && num8 + 1 < height)
						{
							color4 = src[(num8 + 1) * width + num7 + 1];
						}
						array[i * width + j] = this.QuadLerpColor(color, color2, color4, color3, num5, num6);
					}
				}
			}
			return array;
		}

		// Token: 0x0600BAC7 RID: 47815 RVA: 0x0044E818 File Offset: 0x0044CA18
		[PublicizedFrom(EAccessModifier.Private)]
		public Color QuadLerpColor(Color selfVal, Color rightVal, Color upRightVal, Color upVal, float horizontalPerc, float verticalPerc)
		{
			return Color.Lerp(Color.Lerp(selfVal, rightVal, horizontalPerc), Color.Lerp(upVal, upRightVal, horizontalPerc), verticalPerc);
		}

		// Token: 0x0600BAC8 RID: 47816 RVA: 0x0044E834 File Offset: 0x0044CA34
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2i getRotatedPoint(int x, int y, int cx, int cy, int angle)
		{
			double num = Math.Cos((double)angle);
			double num2 = Math.Sin((double)angle);
			return new Vector2i(Mathf.RoundToInt((float)((double)(x - cx) * num - (double)(y - cy) * num2 + (double)cx)), Mathf.RoundToInt((float)((double)(x - cx) * num2 + (double)(y - cy) * num + (double)cy)));
		}

		// Token: 0x04008B68 RID: 35688
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008B69 RID: 35689
		public TranslationData transform;

		// Token: 0x04008B6A RID: 35690
		public float alpha;

		// Token: 0x04008B6B RID: 35691
		public bool additive;

		// Token: 0x04008B6C RID: 35692
		public float scale;

		// Token: 0x04008B6D RID: 35693
		public Color32 customColor;

		// Token: 0x04008B6E RID: 35694
		public float alphaCutoff;

		// Token: 0x04008B6F RID: 35695
		public Rect Area;

		// Token: 0x04008B70 RID: 35696
		public bool isWater;

		// Token: 0x04008B71 RID: 35697
		public string Name = "";

		// Token: 0x04008B72 RID: 35698
		public RawStamp stamp;

		// Token: 0x04008B73 RID: 35699
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cOneByoneScale = 1.4f;
	}
}
