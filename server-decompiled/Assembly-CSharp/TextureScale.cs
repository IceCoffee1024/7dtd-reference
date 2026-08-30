using System;
using System.Threading;
using UnityEngine;

// Token: 0x020014AD RID: 5293
public class TextureScale
{
	// Token: 0x0600A610 RID: 42512 RVA: 0x003DCF62 File Offset: 0x003DB162
	public static void Point(Texture2D tex, int newWidth, int newHeight)
	{
		TextureScale.ThreadedScale(tex, newWidth, newHeight, false);
	}

	// Token: 0x0600A611 RID: 42513 RVA: 0x003DCF6D File Offset: 0x003DB16D
	public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
	{
		TextureScale.ThreadedScale(tex, newWidth, newHeight, true);
	}

	// Token: 0x0600A612 RID: 42514 RVA: 0x003DCF78 File Offset: 0x003DB178
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
	{
		TextureScale.texColors = tex.GetPixels();
		TextureScale.newColors = new Color[newWidth * newHeight];
		if (useBilinear)
		{
			TextureScale.ratioX = 1f / ((float)newWidth / (float)(tex.width - 1));
			TextureScale.ratioY = 1f / ((float)newHeight / (float)(tex.height - 1));
		}
		else
		{
			TextureScale.ratioX = (float)tex.width / (float)newWidth;
			TextureScale.ratioY = (float)tex.height / (float)newHeight;
		}
		TextureScale.w = tex.width;
		TextureScale.w2 = newWidth;
		int num = Mathf.Min(SystemInfo.processorCount, newHeight);
		int num2 = newHeight / num;
		TextureScale.finishCount = 0;
		if (TextureScale.mutex == null)
		{
			TextureScale.mutex = new Mutex(false);
		}
		if (num > 1)
		{
			int i;
			TextureScale.ThreadData threadData;
			for (i = 0; i < num - 1; i++)
			{
				threadData = new TextureScale.ThreadData(num2 * i, num2 * (i + 1));
				new Thread(useBilinear ? new ParameterizedThreadStart(TextureScale.BilinearScale) : new ParameterizedThreadStart(TextureScale.PointScale))
				{
					Name = "TextureScale_" + i.ToString()
				}.Start(threadData);
			}
			threadData = new TextureScale.ThreadData(num2 * i, newHeight);
			if (useBilinear)
			{
				TextureScale.BilinearScale(threadData);
			}
			else
			{
				TextureScale.PointScale(threadData);
			}
			while (TextureScale.finishCount < num)
			{
				Thread.Sleep(1);
			}
		}
		else
		{
			TextureScale.ThreadData threadData2 = new TextureScale.ThreadData(0, newHeight);
			if (useBilinear)
			{
				TextureScale.BilinearScale(threadData2);
			}
			else
			{
				TextureScale.PointScale(threadData2);
			}
		}
		if (!tex.Reinitialize(newWidth, newHeight))
		{
			Log.Warning(string.Concat(new string[]
			{
				"Resized image format: ",
				tex.format.ToString(),
				" (",
				tex.name,
				")"
			}));
		}
		tex.SetPixels(TextureScale.newColors);
		tex.Apply();
	}

	// Token: 0x0600A613 RID: 42515 RVA: 0x003DD138 File Offset: 0x003DB338
	[PublicizedFrom(EAccessModifier.Private)]
	public static void BilinearScale(object obj)
	{
		TextureScale.ThreadData threadData = (TextureScale.ThreadData)obj;
		for (int i = threadData.start; i < threadData.end; i++)
		{
			int num = (int)Mathf.Floor((float)i * TextureScale.ratioY);
			int num2 = num * TextureScale.w;
			int num3 = (num + 1) * TextureScale.w;
			int num4 = i * TextureScale.w2;
			for (int j = 0; j < TextureScale.w2; j++)
			{
				int num5 = (int)Mathf.Floor((float)j * TextureScale.ratioX);
				float num6 = (float)j * TextureScale.ratioX - (float)num5;
				TextureScale.newColors[num4 + j] = TextureScale.ColorLerpUnclamped(TextureScale.ColorLerpUnclamped(TextureScale.texColors[num2 + num5], TextureScale.texColors[num2 + num5 + 1], num6), TextureScale.ColorLerpUnclamped(TextureScale.texColors[num3 + num5], TextureScale.texColors[num3 + num5 + 1], num6), (float)i * TextureScale.ratioY - (float)num);
			}
		}
		TextureScale.mutex.WaitOne();
		TextureScale.finishCount++;
		TextureScale.mutex.ReleaseMutex();
	}

	// Token: 0x0600A614 RID: 42516 RVA: 0x003DD25C File Offset: 0x003DB45C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PointScale(object obj)
	{
		TextureScale.ThreadData threadData = (TextureScale.ThreadData)obj;
		for (int i = threadData.start; i < threadData.end; i++)
		{
			int num = (int)(TextureScale.ratioY * (float)i) * TextureScale.w;
			int num2 = i * TextureScale.w2;
			for (int j = 0; j < TextureScale.w2; j++)
			{
				TextureScale.newColors[num2 + j] = TextureScale.texColors[(int)((float)num + TextureScale.ratioX * (float)j)];
			}
		}
		TextureScale.mutex.WaitOne();
		TextureScale.finishCount++;
		TextureScale.mutex.ReleaseMutex();
	}

	// Token: 0x0600A615 RID: 42517 RVA: 0x003DD2F8 File Offset: 0x003DB4F8
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color ColorLerpUnclamped(Color c1, Color c2, float value)
	{
		return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
	}

	// Token: 0x04007B32 RID: 31538
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color[] texColors;

	// Token: 0x04007B33 RID: 31539
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color[] newColors;

	// Token: 0x04007B34 RID: 31540
	[PublicizedFrom(EAccessModifier.Private)]
	public static int w;

	// Token: 0x04007B35 RID: 31541
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ratioX;

	// Token: 0x04007B36 RID: 31542
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ratioY;

	// Token: 0x04007B37 RID: 31543
	[PublicizedFrom(EAccessModifier.Private)]
	public static int w2;

	// Token: 0x04007B38 RID: 31544
	[PublicizedFrom(EAccessModifier.Private)]
	public static int finishCount;

	// Token: 0x04007B39 RID: 31545
	[PublicizedFrom(EAccessModifier.Private)]
	public static Mutex mutex;

	// Token: 0x020014AE RID: 5294
	public class ThreadData
	{
		// Token: 0x0600A617 RID: 42519 RVA: 0x003DD362 File Offset: 0x003DB562
		public ThreadData(int s, int e)
		{
			this.start = s;
			this.end = e;
		}

		// Token: 0x04007B3A RID: 31546
		public int start;

		// Token: 0x04007B3B RID: 31547
		public int end;
	}
}
