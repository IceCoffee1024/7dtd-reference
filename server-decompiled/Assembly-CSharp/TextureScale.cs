using System;
using System.Threading;
using UnityEngine;

// Token: 0x020014FC RID: 5372
public class TextureScale
{
	// Token: 0x0600A818 RID: 43032 RVA: 0x003E94FA File Offset: 0x003E76FA
	public static void Point(Texture2D tex, int newWidth, int newHeight)
	{
		TextureScale.ThreadedScale(tex, newWidth, newHeight, false);
	}

	// Token: 0x0600A819 RID: 43033 RVA: 0x003E9505 File Offset: 0x003E7705
	public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
	{
		TextureScale.ThreadedScale(tex, newWidth, newHeight, true);
	}

	// Token: 0x0600A81A RID: 43034 RVA: 0x003E9510 File Offset: 0x003E7710
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

	// Token: 0x0600A81B RID: 43035 RVA: 0x003E96D0 File Offset: 0x003E78D0
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

	// Token: 0x0600A81C RID: 43036 RVA: 0x003E97F4 File Offset: 0x003E79F4
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

	// Token: 0x0600A81D RID: 43037 RVA: 0x003E9890 File Offset: 0x003E7A90
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color ColorLerpUnclamped(Color c1, Color c2, float value)
	{
		return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
	}

	// Token: 0x04007CCF RID: 31951
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color[] texColors;

	// Token: 0x04007CD0 RID: 31952
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color[] newColors;

	// Token: 0x04007CD1 RID: 31953
	[PublicizedFrom(EAccessModifier.Private)]
	public static int w;

	// Token: 0x04007CD2 RID: 31954
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ratioX;

	// Token: 0x04007CD3 RID: 31955
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ratioY;

	// Token: 0x04007CD4 RID: 31956
	[PublicizedFrom(EAccessModifier.Private)]
	public static int w2;

	// Token: 0x04007CD5 RID: 31957
	[PublicizedFrom(EAccessModifier.Private)]
	public static int finishCount;

	// Token: 0x04007CD6 RID: 31958
	[PublicizedFrom(EAccessModifier.Private)]
	public static Mutex mutex;

	// Token: 0x020014FD RID: 5373
	public class ThreadData
	{
		// Token: 0x0600A81F RID: 43039 RVA: 0x003E98FA File Offset: 0x003E7AFA
		public ThreadData(int s, int e)
		{
			this.start = s;
			this.end = e;
		}

		// Token: 0x04007CD7 RID: 31959
		public int start;

		// Token: 0x04007CD8 RID: 31960
		public int end;
	}
}
