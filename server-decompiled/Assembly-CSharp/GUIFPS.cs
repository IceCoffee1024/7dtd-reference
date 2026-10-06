using System;
using UnityEngine;

// Token: 0x02001436 RID: 5174
public class GUIFPS : MonoBehaviour
{
	// Token: 0x17001339 RID: 4921
	// (get) Token: 0x0600A230 RID: 41520 RVA: 0x003CDCB7 File Offset: 0x003CBEB7
	// (set) Token: 0x0600A231 RID: 41521 RVA: 0x003CDCBF File Offset: 0x003CBEBF
	public bool Enabled
	{
		get
		{
			return this.bEnabled;
		}
		set
		{
			if (this.bEnabled == value)
			{
				return;
			}
			this.bEnabled = value;
			if (!value && this.guiFpsGraphTexture != null && this.guiFpsGraphTexture.enabled)
			{
				this.guiFpsGraphTexture.enabled = false;
			}
		}
	}

	// Token: 0x1700133A RID: 4922
	// (get) Token: 0x0600A232 RID: 41522 RVA: 0x003CDCFC File Offset: 0x003CBEFC
	// (set) Token: 0x0600A233 RID: 41523 RVA: 0x003CDD04 File Offset: 0x003CBF04
	public bool ShowGraph
	{
		get
		{
			return this.bShowGraph;
		}
		set
		{
			if (this.bShowGraph == value)
			{
				return;
			}
			this.bShowGraph = value;
			if (value && this.guiFpsGraphTexture == null)
			{
				this.initFpsGraph();
			}
			if (this.guiFpsGraphTexture.enabled != this.bShowGraph)
			{
				this.guiFpsGraphTexture.enabled = this.bShowGraph;
			}
		}
	}

	// Token: 0x0600A234 RID: 41524 RVA: 0x003CDD5D File Offset: 0x003CBF5D
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.windowManager = base.GetComponentInParent<GUIWindowManager>();
		GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
	}

	// Token: 0x0600A235 RID: 41525 RVA: 0x003CDD7C File Offset: 0x003CBF7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
	}

	// Token: 0x0600A236 RID: 41526 RVA: 0x003CDD8F File Offset: 0x003CBF8F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _obj)
	{
		if (_obj == EnumGamePrefs.OptionsUiFpsScaling)
		{
			this.lastResolution = Vector2i.zero;
		}
	}

	// Token: 0x0600A237 RID: 41527 RVA: 0x003CDDA4 File Offset: 0x003CBFA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (this.fps.Update())
		{
			this.format = string.Format("{0:F1} FPS", this.fps.Counter);
		}
		if (!this.bEnabled)
		{
			return;
		}
		if (this.bShowGraph)
		{
			this.updateFPSGraph();
		}
	}

	// Token: 0x0600A238 RID: 41528 RVA: 0x003CDDF8 File Offset: 0x003CBFF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		if (!this.Enabled || !this.windowManager.IsHUDEnabled())
		{
			return;
		}
		Vector2i vector2i = new Vector2i(Screen.width, Screen.height);
		if (this.lastResolution != vector2i)
		{
			float num = GamePrefs.GetFloat(EnumGamePrefs.OptionsUiFpsScaling) * 13f;
			this.lastResolution = vector2i;
			this.boxStyle = new GUIStyle(GUI.skin.box);
			int num2;
			if (vector2i.y > 1200)
			{
				num2 = Mathf.RoundToInt((float)vector2i.y / (1200f / num));
			}
			else
			{
				num2 = Mathf.RoundToInt(num);
			}
			this.boxStyle.fontSize = num2;
			this.boxAreaHeight = num2 + 10;
			this.boxAreaWidth = num2 * 7;
		}
		if (this.fps.Counter < 30f)
		{
			GUI.color = Color.yellow;
		}
		else if (this.fps.Counter < 10f)
		{
			GUI.color = Color.red;
		}
		else
		{
			GUI.color = Color.green;
		}
		GUI.Box(new Rect(14f, (float)(Screen.height / 2 + 40), (float)this.boxAreaWidth, (float)this.boxAreaHeight), this.format, this.boxStyle);
	}

	// Token: 0x0600A239 RID: 41529 RVA: 0x003CDF2B File Offset: 0x003CC12B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnApplicationQuit()
	{
		if (this.texture != null)
		{
			global::UnityEngine.Object.Destroy(this.texture);
		}
	}

	// Token: 0x0600A23A RID: 41530 RVA: 0x003CDF46 File Offset: 0x003CC146
	[PublicizedFrom(EAccessModifier.Private)]
	public void initFpsGraph()
	{
		this.texture = GUIFPS.createGUITexture();
		this.guiFpsGraphTexture = base.gameObject.AddMissingComponent<UITexture>();
		this.guiFpsGraphTexture.mainTexture = this.texture;
		this.guiFpsGraphTexture.enabled = false;
	}

	// Token: 0x0600A23B RID: 41531 RVA: 0x003CDF84 File Offset: 0x003CC184
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D createGUITexture()
	{
		Texture2D texture2D = new Texture2D(1024, 256, TextureFormat.RGBA32, false);
		for (int i = 0; i < texture2D.height; i++)
		{
			for (int j = 0; j < texture2D.width; j++)
			{
				texture2D.SetPixel(j, i, default(Color));
			}
		}
		texture2D.filterMode = FilterMode.Point;
		return texture2D;
	}

	// Token: 0x0600A23C RID: 41532 RVA: 0x003CDFE0 File Offset: 0x003CC1E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateFPSGraph()
	{
		long totalMemory = GC.GetTotalMemory(false);
		if (totalMemory < this.lastTotalMemory)
		{
			this.gcSpikeCounter = 3;
		}
		this.lastTotalMemory = totalMemory;
		int height = this.texture.height;
		int num = (int)Math.Min((float)height, Time.deltaTime * 2500f);
		float num2 = 1f / Time.deltaTime;
		Color color;
		if (num2 > 20f)
		{
			if (num2 <= 40f)
			{
				color = new Color(1f, 1f, 0f, 0.5f);
			}
			else
			{
				color = new Color(0f, 1f, 0f, 0.5f);
			}
		}
		else if (num2 <= 10f)
		{
			color = new Color(1f, 0f, 0f, 0.5f);
		}
		else
		{
			color = new Color(1f, 0.5f, 0f, 0.5f);
		}
		Color color2 = color;
		int num3 = this.gcSpikeCounter;
		this.gcSpikeCounter = num3 - 1;
		if (num3 > 0)
		{
			color2 = Color.magenta;
		}
		for (int i = 0; i <= num; i++)
		{
			this.texture.SetPixel(this.curGraphXPos, i, color2);
		}
		for (int j = num + 1; j < height; j++)
		{
			this.texture.SetPixel(this.curGraphXPos, j, new Color(0f, 0f, 0f, 0f));
		}
		for (int k = 0; k < height; k++)
		{
			this.texture.SetPixel(this.curGraphXPos + 1, k, new Color(0f, 0f, 0f, 0f));
		}
		for (int l = 10; l <= 60; l += 10)
		{
			this.texture.SetPixel(this.curGraphXPos, (int)(2500f / (float)l), new Color(1f, 1f, 1f, 0.5f));
			this.texture.SetPixel(this.curGraphXPos, (int)(2500f / (float)l) - 1, new Color(1f, 1f, 1f, 0.5f));
		}
		this.texture.Apply(false);
		this.curGraphXPos++;
		this.curGraphXPos %= this.texture.width - 1;
	}

	// Token: 0x04007A07 RID: 31239
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bEnabled;

	// Token: 0x04007A08 RID: 31240
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public FPS fps = new FPS(0.5f);

	// Token: 0x04007A09 RID: 31241
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string format;

	// Token: 0x04007A0A RID: 31242
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int BaseTextSize = 13;

	// Token: 0x04007A0B RID: 31243
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bShowGraph;

	// Token: 0x04007A0C RID: 31244
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D texture;

	// Token: 0x04007A0D RID: 31245
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int curGraphXPos;

	// Token: 0x04007A0E RID: 31246
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UITexture guiFpsGraphTexture;

	// Token: 0x04007A0F RID: 31247
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public long lastTotalMemory;

	// Token: 0x04007A10 RID: 31248
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int gcSpikeCounter;

	// Token: 0x04007A11 RID: 31249
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cBarHeight = 2500f;

	// Token: 0x04007A12 RID: 31250
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2i lastResolution;

	// Token: 0x04007A13 RID: 31251
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIStyle boxStyle;

	// Token: 0x04007A14 RID: 31252
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int boxAreaHeight;

	// Token: 0x04007A15 RID: 31253
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int boxAreaWidth;

	// Token: 0x04007A16 RID: 31254
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager windowManager;
}
