using System;
using UnityEngine;

// Token: 0x02001416 RID: 5142
public class GUIFPS : MonoBehaviour
{
	// Token: 0x170012F4 RID: 4852
	// (get) Token: 0x0600A185 RID: 41349 RVA: 0x003C911B File Offset: 0x003C731B
	// (set) Token: 0x0600A186 RID: 41350 RVA: 0x003C9123 File Offset: 0x003C7323
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

	// Token: 0x170012F5 RID: 4853
	// (get) Token: 0x0600A187 RID: 41351 RVA: 0x003C9160 File Offset: 0x003C7360
	// (set) Token: 0x0600A188 RID: 41352 RVA: 0x003C9168 File Offset: 0x003C7368
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

	// Token: 0x0600A189 RID: 41353 RVA: 0x003C91C1 File Offset: 0x003C73C1
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.windowManager = base.GetComponentInParent<GUIWindowManager>();
		GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
	}

	// Token: 0x0600A18A RID: 41354 RVA: 0x003C91E0 File Offset: 0x003C73E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
	}

	// Token: 0x0600A18B RID: 41355 RVA: 0x003C91F3 File Offset: 0x003C73F3
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _obj)
	{
		if (_obj == EnumGamePrefs.OptionsUiFpsScaling)
		{
			this.lastResolution = Vector2i.zero;
		}
	}

	// Token: 0x0600A18C RID: 41356 RVA: 0x003C9208 File Offset: 0x003C7408
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

	// Token: 0x0600A18D RID: 41357 RVA: 0x003C925C File Offset: 0x003C745C
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

	// Token: 0x0600A18E RID: 41358 RVA: 0x003C938F File Offset: 0x003C758F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnApplicationQuit()
	{
		if (this.texture != null)
		{
			global::UnityEngine.Object.Destroy(this.texture);
		}
	}

	// Token: 0x0600A18F RID: 41359 RVA: 0x003C93AA File Offset: 0x003C75AA
	[PublicizedFrom(EAccessModifier.Private)]
	public void initFpsGraph()
	{
		this.texture = GUIFPS.createGUITexture();
		this.guiFpsGraphTexture = base.gameObject.AddMissingComponent<UITexture>();
		this.guiFpsGraphTexture.mainTexture = this.texture;
		this.guiFpsGraphTexture.enabled = false;
	}

	// Token: 0x0600A190 RID: 41360 RVA: 0x003C93E8 File Offset: 0x003C75E8
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

	// Token: 0x0600A191 RID: 41361 RVA: 0x003C9444 File Offset: 0x003C7644
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

	// Token: 0x0400798C RID: 31116
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bEnabled;

	// Token: 0x0400798D RID: 31117
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public FPS fps = new FPS(0.5f);

	// Token: 0x0400798E RID: 31118
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string format;

	// Token: 0x0400798F RID: 31119
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int BaseTextSize = 13;

	// Token: 0x04007990 RID: 31120
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bShowGraph;

	// Token: 0x04007991 RID: 31121
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D texture;

	// Token: 0x04007992 RID: 31122
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int curGraphXPos;

	// Token: 0x04007993 RID: 31123
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UITexture guiFpsGraphTexture;

	// Token: 0x04007994 RID: 31124
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public long lastTotalMemory;

	// Token: 0x04007995 RID: 31125
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int gcSpikeCounter;

	// Token: 0x04007996 RID: 31126
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cBarHeight = 2500f;

	// Token: 0x04007997 RID: 31127
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2i lastResolution;

	// Token: 0x04007998 RID: 31128
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIStyle boxStyle;

	// Token: 0x04007999 RID: 31129
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int boxAreaHeight;

	// Token: 0x0400799A RID: 31130
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int boxAreaWidth;

	// Token: 0x0400799B RID: 31131
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager windowManager;
}
