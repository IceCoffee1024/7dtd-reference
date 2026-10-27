using System;
using UnityEngine;

// Token: 0x0200136C RID: 4972
public class FurCombPainter
{
	// Token: 0x06009C88 RID: 40072 RVA: 0x003B1778 File Offset: 0x003AF978
	public void Paint(Texture2D texture, Vector2 uv, Vector3 tangentDirection, BrushSettings brushSettings, FurCombPainter.PaintMode paintMode)
	{
		if (this.texturePaintComputeShader == null)
		{
			this.texturePaintComputeShader = Resources.Load<ComputeShader>("Shaders/TexturePaintCompute");
		}
		if (this.texturePaintComputeShader == null)
		{
			Debug.LogError("TexturePaintCompute.compute not found in Resources folder");
			return;
		}
		int num = this.texturePaintComputeShader.FindKernel("ChannelWrite");
		int num2 = Mathf.FloorToInt(uv.x * (float)texture.width);
		int num3 = Mathf.FloorToInt(uv.y * (float)texture.height);
		RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);
		renderTexture.enableRandomWrite = true;
		renderTexture.useMipMap = texture.mipmapCount > 1;
		renderTexture.Create();
		Graphics.CopyTexture(texture, renderTexture);
		Vector3 vector = tangentDirection * 0.5f + Vector3.one * 0.5f;
		this.texturePaintComputeShader.SetInts("PaintPosition", new int[] { num2, num3 });
		this.texturePaintComputeShader.SetInt("BrushRadius", (int)brushSettings.Size);
		this.texturePaintComputeShader.SetFloat("BrushStrength", brushSettings.Strength);
		this.texturePaintComputeShader.SetFloat("BrushFalloff", brushSettings.Falloff);
		this.texturePaintComputeShader.SetVector("BrushColor", brushSettings.Color);
		switch (paintMode)
		{
		case FurCombPainter.PaintMode.Albedo:
			this.texturePaintComputeShader.SetVector("BrushValue", new Vector4(brushSettings.Color.r, brushSettings.Color.g, brushSettings.Color.b, brushSettings.Color.a));
			this.texturePaintComputeShader.SetVector("ChannelMask", new Vector4(1f, 1f, 1f, 0f));
			this.texturePaintComputeShader.SetTexture(num, "Result", renderTexture);
			this.texturePaintComputeShader.Dispatch(num, texture.width / 8, texture.height / 8, 1);
			break;
		case FurCombPainter.PaintMode.Direction:
			this.texturePaintComputeShader.SetVector("BrushValue", new Vector4(vector.x, vector.y, 0f, 0f));
			this.texturePaintComputeShader.SetVector("ChannelMask", new Vector4(1f, 1f, 0f, 0f));
			this.texturePaintComputeShader.SetTexture(num, "Result", renderTexture);
			this.texturePaintComputeShader.Dispatch(num, texture.width / 8, texture.height / 8, 1);
			break;
		case FurCombPainter.PaintMode.Matting:
			this.texturePaintComputeShader.SetVector("ChannelMask", new Vector4(0f, 0f, 0f, 1f));
			this.texturePaintComputeShader.SetVector("BrushValue", new Vector4(0f, 0f, 0f, brushSettings.Matting));
			this.texturePaintComputeShader.SetTexture(num, "Result", renderTexture);
			this.texturePaintComputeShader.Dispatch(num, texture.width / 8, texture.height / 8, 1);
			break;
		case FurCombPainter.PaintMode.DirectionAndMatting:
			this.texturePaintComputeShader.SetVector("ChannelMask", new Vector4(1f, 1f, 0f, 1f));
			this.texturePaintComputeShader.SetVector("BrushValue", new Vector4(vector.x, vector.y, 0f, brushSettings.Matting));
			this.texturePaintComputeShader.SetTexture(num, "Result", renderTexture);
			this.texturePaintComputeShader.Dispatch(num, texture.width / 8, texture.height / 8, 1);
			break;
		case FurCombPainter.PaintMode.Length:
			this.texturePaintComputeShader.SetVector("ChannelMask", new Vector4(0f, 0f, 1f, 0f));
			this.texturePaintComputeShader.SetVector("BrushValue", new Vector4(0f, 0f, brushSettings.Length, 0f));
			this.texturePaintComputeShader.SetTexture(num, "Result", renderTexture);
			this.texturePaintComputeShader.Dispatch(num, texture.width / 8, texture.height / 8, 1);
			break;
		case FurCombPainter.PaintMode.Roughness:
			this.texturePaintComputeShader.SetVector("ChannelMask", new Vector4(1f, 0f, 0f, 0f));
			this.texturePaintComputeShader.SetVector("BrushValue", new Vector4(brushSettings.Roughness, 0f, 0f, 0f));
			this.texturePaintComputeShader.SetTexture(num, "Result", renderTexture);
			this.texturePaintComputeShader.Dispatch(num, texture.width / 8, texture.height / 8, 1);
			break;
		case FurCombPainter.PaintMode.Metallic:
			this.texturePaintComputeShader.SetVector("ChannelMask", new Vector4(0f, 1f, 0f, 0f));
			this.texturePaintComputeShader.SetVector("BrushValue", new Vector4(0f, brushSettings.Metallic, 0f, 0f));
			this.texturePaintComputeShader.SetTexture(num, "Result", renderTexture);
			this.texturePaintComputeShader.Dispatch(num, texture.width / 8, texture.height / 8, 1);
			break;
		case FurCombPainter.PaintMode.Occlusion:
			this.texturePaintComputeShader.SetVector("ChannelMask", new Vector4(0f, 0f, 1f, 0f));
			this.texturePaintComputeShader.SetVector("BrushValue", new Vector4(0f, 0f, brushSettings.Occlusion, 0f));
			this.texturePaintComputeShader.SetTexture(num, "Result", renderTexture);
			this.texturePaintComputeShader.Dispatch(num, texture.width / 8, texture.height / 8, 1);
			break;
		}
		RenderTexture.active = renderTexture;
		texture.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
		texture.Apply();
		RenderTexture.active = null;
		renderTexture.Release();
	}

	// Token: 0x040075F2 RID: 30194
	public ComputeShader texturePaintComputeShader;

	// Token: 0x0200136D RID: 4973
	public enum PaintMode
	{
		// Token: 0x040075F4 RID: 30196
		Albedo,
		// Token: 0x040075F5 RID: 30197
		Direction,
		// Token: 0x040075F6 RID: 30198
		Matting,
		// Token: 0x040075F7 RID: 30199
		DirectionAndMatting,
		// Token: 0x040075F8 RID: 30200
		Length,
		// Token: 0x040075F9 RID: 30201
		Roughness,
		// Token: 0x040075FA RID: 30202
		Metallic,
		// Token: 0x040075FB RID: 30203
		Occlusion
	}
}
