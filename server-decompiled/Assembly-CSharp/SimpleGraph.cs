using System;
using UnityEngine;

// Token: 0x020014D4 RID: 5332
public class SimpleGraph
{
	// Token: 0x0600A6B3 RID: 42675 RVA: 0x003E1210 File Offset: 0x003DF410
	public void Init(int _texW, int _texH, float _maxValue, float[] _lines = null)
	{
		this.maxValue = _maxValue;
		if (_lines != null)
		{
			this.lines = _lines;
		}
		this.texture = new Texture2D(_texW, _texH, TextureFormat.RGBA32, false);
		this.texture.filterMode = FilterMode.Point;
		this.texture.name = "SimpleGraph";
		for (int i = 0; i < this.texture.height; i++)
		{
			for (int j = 0; j < this.texture.width; j++)
			{
				this.texture.SetPixel(j, i, default(Color));
			}
		}
	}

	// Token: 0x0600A6B4 RID: 42676 RVA: 0x003E129D File Offset: 0x003DF49D
	public void Cleanup()
	{
		global::UnityEngine.Object.Destroy(this.texture);
	}

	// Token: 0x0600A6B5 RID: 42677 RVA: 0x003E12AC File Offset: 0x003DF4AC
	public void Update(float _value, Color _color)
	{
		int height = this.texture.height;
		int num = (int)(_value * (float)height * 1f / this.maxValue);
		if (num >= height)
		{
			num = height;
			_color = Color.red;
		}
		for (int i = 0; i <= num; i++)
		{
			this.texture.SetPixel(this.curGraphXPos, i, _color);
		}
		for (int j = num + 1; j < height; j++)
		{
			this.texture.SetPixel(this.curGraphXPos, j, Color.clear);
		}
		for (int k = 0; k < height; k++)
		{
			this.texture.SetPixel(this.curGraphXPos + 1, k, new Color(1f, 1f, 1f, 0.5f));
		}
		for (int l = 0; l < this.lines.Length; l++)
		{
			this.texture.SetPixel(this.curGraphXPos, (int)((float)height * this.lines[l]), new Color(1f, 1f, 1f, 0.7f));
			this.texture.SetPixel(this.curGraphXPos, (int)((float)height * this.lines[l]) - 1, new Color(1f, 1f, 1f, 0.7f));
		}
		this.texture.Apply(false);
		this.curGraphXPos++;
		this.curGraphXPos %= this.texture.width - 1;
	}

	// Token: 0x04007C32 RID: 31794
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxValue = 1f;

	// Token: 0x04007C33 RID: 31795
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] lines = new float[] { 1f, 0.5f, 0.33333334f, 0.25f, 0.2f, 0.16666667f, 0f };

	// Token: 0x04007C34 RID: 31796
	public Texture2D texture;

	// Token: 0x04007C35 RID: 31797
	[PublicizedFrom(EAccessModifier.Private)]
	public int curGraphXPos;
}
