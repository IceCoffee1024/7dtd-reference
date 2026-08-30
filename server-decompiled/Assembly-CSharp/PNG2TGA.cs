using System;
using System.IO;
using UnityEngine;

// Token: 0x0200147B RID: 5243
public class PNG2TGA
{
	// Token: 0x0600A476 RID: 42102 RVA: 0x003D48E4 File Offset: 0x003D2AE4
	[PublicizedFrom(EAccessModifier.Private)]
	public PNG2TGA.Vector2Int IndexToVector2Int(int index)
	{
		return new PNG2TGA.Vector2Int(index % this._width, index / this._width);
	}

	// Token: 0x0600A477 RID: 42103 RVA: 0x003D48FB File Offset: 0x003D2AFB
	[PublicizedFrom(EAccessModifier.Private)]
	public int XYToIndex(int x, int y)
	{
		return x + y * this._width;
	}

	// Token: 0x0600A478 RID: 42104 RVA: 0x003D4907 File Offset: 0x003D2B07
	[PublicizedFrom(EAccessModifier.Private)]
	public int Vector2IntToIndex(PNG2TGA.Vector2Int pos)
	{
		return this.XYToIndex(pos.x, pos.y);
	}

	// Token: 0x0600A479 RID: 42105 RVA: 0x003D491C File Offset: 0x003D2B1C
	public void Process(Texture2D texture)
	{
		this._texture = texture;
		Color[] pixels = this._texture.GetPixels(0);
		int[] array = new int[pixels.Length];
		this._width = this._texture.width;
		this._height = this._texture.height;
		bool flag = false;
		for (int i = 0; i < pixels.Length; i++)
		{
			if (pixels[i].a == 0f)
			{
				pixels[i].r = (pixels[i].g = (pixels[i].b = 0f));
				array[i] = 0;
			}
			else
			{
				array[i] = 1;
				flag = true;
			}
		}
		if (flag)
		{
			bool flag2 = true;
			int num = 1;
			while (flag2)
			{
				flag2 = false;
				for (int j = 0; j < this._height; j++)
				{
					for (int k = 0; k < this._width; k++)
					{
						if (array[this.XYToIndex(k, j)] == 0)
						{
							flag2 = true;
							if ((k > 0 && array[this.XYToIndex(k - 1, j)] == num) || (k < this._width - 1 && array[this.XYToIndex(k + 1, j)] == num) || (j > 0 && array[this.XYToIndex(k, j - 1)] == num) || (j < this._height - 1 && array[this.XYToIndex(k, j + 1)] == num))
							{
								array[this.XYToIndex(k, j)] = num + 1;
							}
						}
					}
				}
				num++;
			}
			for (int l = 2; l < num; l++)
			{
				for (int m = 0; m < this._height; m++)
				{
					for (int n = 0; n < this._width; n++)
					{
						if (array[this.XYToIndex(n, m)] == l)
						{
							float num2 = 0f;
							Color color = Color.black;
							if (n > 0 && array[this.XYToIndex(n - 1, m)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n - 1, m)];
							}
							if (n < this._width - 1 && array[this.XYToIndex(n + 1, m)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n + 1, m)];
							}
							if (m > 0 && array[this.XYToIndex(n, m - 1)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n, m - 1)];
							}
							if (m < this._height - 1 && array[this.XYToIndex(n, m + 1)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n, m + 1)];
							}
							color *= 8f / num2;
							color.a = 0f;
							pixels[this.XYToIndex(n, m)] = color;
						}
					}
				}
			}
		}
		this._texture.SetPixels(pixels, 0);
		this._data = new byte[pixels.Length * 4];
		int num3 = pixels.Length;
		Mathf.Max(num3 / 200, 1);
		for (int num4 = 0; num4 < num3; num4++)
		{
			this._data[4 * num4] = (byte)(pixels[num4].b * 255f);
			this._data[4 * num4 + 1] = (byte)(pixels[num4].g * 255f);
			this._data[4 * num4 + 2] = (byte)(pixels[num4].r * 255f);
			this._data[4 * num4 + 3] = (byte)(pixels[num4].a * 255f);
		}
		this._texture.Apply();
	}

	// Token: 0x0600A47A RID: 42106 RVA: 0x003D4D54 File Offset: 0x003D2F54
	public void Process2(Color[] pixels, int _width, int _height)
	{
		this.gwidth = _width;
		this.gheight = _height;
		int[] array = new int[pixels.Length];
		bool flag = false;
		for (int i = 0; i < pixels.Length; i++)
		{
			array[i] = 1;
			flag = true;
		}
		if (flag)
		{
			bool flag2 = true;
			int num = 1;
			while (flag2)
			{
				flag2 = false;
				for (int j = 0; j < this._height; j++)
				{
					for (int k = 0; k < this._width; k++)
					{
						if (array[this.XYToIndex(k, j)] == 0)
						{
							flag2 = true;
							if ((k > 0 && array[this.XYToIndex(k - 1, j)] == num) || (k < this._width - 1 && array[this.XYToIndex(k + 1, j)] == num) || (j > 0 && array[this.XYToIndex(k, j - 1)] == num) || (j < this._height - 1 && array[this.XYToIndex(k, j + 1)] == num))
							{
								array[this.XYToIndex(k, j)] = num + 1;
							}
						}
					}
				}
				num++;
			}
			for (int l = 2; l < num; l++)
			{
				for (int m = 0; m < this._height; m++)
				{
					for (int n = 0; n < this._width; n++)
					{
						if (array[this.XYToIndex(n, m)] == l)
						{
							float num2 = 0f;
							Color color = Color.black;
							if (n > 0 && array[this.XYToIndex(n - 1, m)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n - 1, m)];
							}
							if (n < this._width - 1 && array[this.XYToIndex(n + 1, m)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n + 1, m)];
							}
							if (m > 0 && array[this.XYToIndex(n, m - 1)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n, m - 1)];
							}
							if (m < this._height - 1 && array[this.XYToIndex(n, m + 1)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n, m + 1)];
							}
							color *= 8f / num2;
							color.a = 0f;
							pixels[this.XYToIndex(n, m)] = color;
						}
					}
				}
			}
		}
		this._data = new byte[pixels.Length * 4];
		int num3 = pixels.Length;
		Mathf.Max(num3 / 200, 1);
		for (int num4 = 0; num4 < num3; num4++)
		{
			this._data[4 * num4] = (byte)(pixels[num4].b * 255f);
			this._data[4 * num4 + 1] = (byte)(pixels[num4].g * 255f);
			this._data[4 * num4 + 2] = (byte)(pixels[num4].r * 255f);
			this._data[4 * num4 + 3] = (byte)(pixels[num4].a * 255f);
		}
	}

	// Token: 0x0600A47B RID: 42107 RVA: 0x003D50F8 File Offset: 0x003D32F8
	public void Process3(Color32[] pixels, int _width, int _height)
	{
		this.gwidth = _width;
		this.gheight = _height;
		int[] array = new int[pixels.Length];
		bool flag = false;
		for (int i = 0; i < pixels.Length; i++)
		{
			array[i] = 1;
			flag = true;
		}
		if (flag)
		{
			bool flag2 = true;
			int num = 1;
			while (flag2)
			{
				flag2 = false;
				for (int j = 0; j < this._height; j++)
				{
					for (int k = 0; k < this._width; k++)
					{
						if (array[this.XYToIndex(k, j)] == 0)
						{
							flag2 = true;
							if ((k > 0 && array[this.XYToIndex(k - 1, j)] == num) || (k < this._width - 1 && array[this.XYToIndex(k + 1, j)] == num) || (j > 0 && array[this.XYToIndex(k, j - 1)] == num) || (j < this._height - 1 && array[this.XYToIndex(k, j + 1)] == num))
							{
								array[this.XYToIndex(k, j)] = num + 1;
							}
						}
					}
				}
				num++;
			}
			for (int l = 2; l < num; l++)
			{
				for (int m = 0; m < this._height; m++)
				{
					for (int n = 0; n < this._width; n++)
					{
						if (array[this.XYToIndex(n, m)] == l)
						{
							float num2 = 0f;
							Color color = Color.black;
							if (n > 0 && array[this.XYToIndex(n - 1, m)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n - 1, m)];
							}
							if (n < this._width - 1 && array[this.XYToIndex(n + 1, m)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n + 1, m)];
							}
							if (m > 0 && array[this.XYToIndex(n, m - 1)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n, m - 1)];
							}
							if (m < this._height - 1 && array[this.XYToIndex(n, m + 1)] == l - 1)
							{
								num2 += 1f;
								color += 0.125f * pixels[this.XYToIndex(n, m + 1)];
							}
							color *= 8f / num2;
							color.a = 0f;
							pixels[this.XYToIndex(n, m)] = color;
						}
					}
				}
			}
		}
		this._data = new byte[pixels.Length * 4];
		int num3 = pixels.Length;
		Mathf.Max(num3 / 200, 1);
		for (int num4 = 0; num4 < num3; num4++)
		{
			this._data[4 * num4] = pixels[num4].b;
			this._data[4 * num4 + 1] = pixels[num4].g;
			this._data[4 * num4 + 2] = pixels[num4].r;
			this._data[4 * num4 + 3] = pixels[num4].a;
		}
	}

	// Token: 0x0600A47C RID: 42108 RVA: 0x003D5490 File Offset: 0x003D3690
	public bool Save(string path)
	{
		try
		{
			Stream stream = SdFile.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read);
			int width = this._texture.width;
			int height = this._texture.height;
			PNG2TGA.header[12] = (byte)(width % 256);
			PNG2TGA.header[13] = (byte)(width / 256);
			PNG2TGA.header[14] = (byte)(height % 256);
			PNG2TGA.header[15] = (byte)(height / 256);
			stream.Write(PNG2TGA.header, 0, PNG2TGA.header.Length);
			stream.Write(this._data, 0, this._data.Length);
			stream.Write(PNG2TGA.footer, 0, PNG2TGA.footer.Length);
			stream.Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
		return false;
	}

	// Token: 0x0600A47D RID: 42109 RVA: 0x003D555C File Offset: 0x003D375C
	public bool Save2(string path)
	{
		try
		{
			Stream stream = SdFile.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read);
			PNG2TGA.header[12] = (byte)(this.gwidth % 256);
			PNG2TGA.header[13] = (byte)(this.gwidth / 256);
			PNG2TGA.header[14] = (byte)(this.gheight % 256);
			PNG2TGA.header[15] = (byte)(this.gheight / 256);
			stream.Write(PNG2TGA.header, 0, PNG2TGA.header.Length);
			stream.Write(this._data, 0, this._data.Length);
			stream.Write(PNG2TGA.footer, 0, PNG2TGA.footer.Length);
			stream.Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
		return false;
	}

	// Token: 0x04007AE2 RID: 31458
	[PublicizedFrom(EAccessModifier.Private)]
	public static PNG2TGA _window;

	// Token: 0x04007AE3 RID: 31459
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D _texture;

	// Token: 0x04007AE4 RID: 31460
	[PublicizedFrom(EAccessModifier.Private)]
	public int _width;

	// Token: 0x04007AE5 RID: 31461
	[PublicizedFrom(EAccessModifier.Private)]
	public int _height;

	// Token: 0x04007AE6 RID: 31462
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] _data;

	// Token: 0x04007AE7 RID: 31463
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] header = new byte[]
	{
		0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 32, 8
	};

	// Token: 0x04007AE8 RID: 31464
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] footer = new byte[]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 84, 82,
		85, 69, 86, 73, 83, 73, 79, 78, 45, 88,
		70, 73, 76, 69, 46, 0
	};

	// Token: 0x04007AE9 RID: 31465
	[PublicizedFrom(EAccessModifier.Private)]
	public int gwidth;

	// Token: 0x04007AEA RID: 31466
	[PublicizedFrom(EAccessModifier.Private)]
	public int gheight;

	// Token: 0x0200147C RID: 5244
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Vector2Int
	{
		// Token: 0x0600A480 RID: 42112 RVA: 0x003D5654 File Offset: 0x003D3854
		public Vector2Int(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x0600A481 RID: 42113 RVA: 0x003D5664 File Offset: 0x003D3864
		public static PNG2TGA.Vector2Int operator +(PNG2TGA.Vector2Int a, PNG2TGA.Vector2Int b)
		{
			return new PNG2TGA.Vector2Int(a.x + b.x, a.y + b.y);
		}

		// Token: 0x0600A482 RID: 42114 RVA: 0x003D5685 File Offset: 0x003D3885
		public static PNG2TGA.Vector2Int operator -(PNG2TGA.Vector2Int a, PNG2TGA.Vector2Int b)
		{
			return new PNG2TGA.Vector2Int(a.x - b.x, a.y - b.y);
		}

		// Token: 0x04007AEB RID: 31467
		public int x;

		// Token: 0x04007AEC RID: 31468
		public int y;
	}
}
