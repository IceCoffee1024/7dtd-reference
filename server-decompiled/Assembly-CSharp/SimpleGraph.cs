using System;
using UnityEngine;

// Token: 0x02001487 RID: 5255
public class SimpleGraph
{
	// Token: 0x0600A4B0 RID: 42160 RVA: 0x003D4E3C File Offset: 0x003D303C
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

	// Token: 0x0600A4B1 RID: 42161 RVA: 0x003D4EC9 File Offset: 0x003D30C9
	public void Cleanup()
	{
		global::UnityEngine.Object.Destroy(this.texture);
	}

	// Token: 0x0600A4B2 RID: 42162 RVA: 0x003D4ED8 File Offset: 0x003D30D8
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

	// Token: 0x04007A9D RID: 31389
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxValue = 1f;

	// Token: 0x04007A9E RID: 31390
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] lines = new float[] { 1f, 0.5f, 0.33333334f, 0.25f, 0.2f, 0.16666667f, 0f };

	// Token: 0x04007A9F RID: 31391
	public Texture2D texture;

	// Token: 0x04007AA0 RID: 31392
	[PublicizedFrom(EAccessModifier.Private)]
	public int curGraphXPos;
}
