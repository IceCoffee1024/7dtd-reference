using System;
using System.Threading;
using UnityEngine;

// Token: 0x020014EA RID: 5354
public class TextureScale
{
	// Token: 0x0600A7C0 RID: 42944 RVA: 0x003E70F6 File Offset: 0x003E52F6
	public static void Point(Texture2D tex, int newWidth, int newHeight)
	{
		TextureScale.ThreadedScale(tex, newWidth, newHeight, false);
	}

	// Token: 0x0600A7C1 RID: 42945 RVA: 0x003E7101 File Offset: 0x003E5301
	public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
	{
		TextureScale.ThreadedScale(tex, newWidth, newHeight, true);
	}

	// Token: 0x0600A7C2 RID: 42946 RVA: 0x003E710C File Offset: 0x003E530C
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

	// Token: 0x0600A7C3 RID: 42947 RVA: 0x003E72CC File Offset: 0x003E54CC
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

	// Token: 0x0600A7C4 RID: 42948 RVA: 0x003E73F0 File Offset: 0x003E55F0
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

	// Token: 0x0600A7C5 RID: 42949 RVA: 0x003E748C File Offset: 0x003E568C
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color ColorLerpUnclamped(Color c1, Color c2, float value)
	{
		return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
	}

	// Token: 0x04007C8E RID: 31886
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color[] texColors;

	// Token: 0x04007C8F RID: 31887
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color[] newColors;

	// Token: 0x04007C90 RID: 31888
	[PublicizedFrom(EAccessModifier.Private)]
	public static int w;

	// Token: 0x04007C91 RID: 31889
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ratioX;

	// Token: 0x04007C92 RID: 31890
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ratioY;

	// Token: 0x04007C93 RID: 31891
	[PublicizedFrom(EAccessModifier.Private)]
	public static int w2;

	// Token: 0x04007C94 RID: 31892
	[PublicizedFrom(EAccessModifier.Private)]
	public static int finishCount;

	// Token: 0x04007C95 RID: 31893
	[PublicizedFrom(EAccessModifier.Private)]
	public static Mutex mutex;

	// Token: 0x020014EB RID: 5355
	public class ThreadData
	{
		// Token: 0x0600A7C7 RID: 42951 RVA: 0x003E74F6 File Offset: 0x003E56F6
		public ThreadData(int s, int e)
		{
			this.start = s;
			this.end = e;
		}

		// Token: 0x04007C96 RID: 31894
		public int start;

		// Token: 0x04007C97 RID: 31895
		public int end;
	}
}
