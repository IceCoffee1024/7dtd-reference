using System;
using UnityEngine;

// Token: 0x020014A2 RID: 5282
public class SimpleGraph
{
	// Token: 0x0600A5AD RID: 42413 RVA: 0x003DA180 File Offset: 0x003D8380
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

	// Token: 0x0600A5AE RID: 42414 RVA: 0x003DA20D File Offset: 0x003D840D
	public void Cleanup()
	{
		global::UnityEngine.Object.Destroy(this.texture);
	}

	// Token: 0x0600A5AF RID: 42415 RVA: 0x003DA21C File Offset: 0x003D841C
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

	// Token: 0x04007B76 RID: 31606
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxValue = 1f;

	// Token: 0x04007B77 RID: 31607
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] lines = new float[] { 1f, 0.5f, 0.33333334f, 0.25f, 0.2f, 0.16666667f, 0f };

	// Token: 0x04007B78 RID: 31608
	public Texture2D texture;

	// Token: 0x04007B79 RID: 31609
	[PublicizedFrom(EAccessModifier.Private)]
	public int curGraphXPos;
}
