using System;
using UnityEngine;

// Token: 0x020013FB RID: 5115
public class GUIFPS : MonoBehaviour
{
	// Token: 0x170012BB RID: 4795
	// (get) Token: 0x0600A088 RID: 41096 RVA: 0x003C3DD7 File Offset: 0x003C1FD7
	// (set) Token: 0x0600A089 RID: 41097 RVA: 0x003C3DDF File Offset: 0x003C1FDF
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

	// Token: 0x170012BC RID: 4796
	// (get) Token: 0x0600A08A RID: 41098 RVA: 0x003C3E1C File Offset: 0x003C201C
	// (set) Token: 0x0600A08B RID: 41099 RVA: 0x003C3E24 File Offset: 0x003C2024
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

	// Token: 0x0600A08C RID: 41100 RVA: 0x003C3E7D File Offset: 0x003C207D
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.windowManager = base.GetComponentInParent<GUIWindowManager>();
		GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
	}

	// Token: 0x0600A08D RID: 41101 RVA: 0x003C3E9C File Offset: 0x003C209C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
	}

	// Token: 0x0600A08E RID: 41102 RVA: 0x003C3EAF File Offset: 0x003C20AF
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _obj)
	{
		if (_obj == EnumGamePrefs.OptionsUiFpsScaling)
		{
			this.lastResolution = Vector2i.zero;
		}
	}

	// Token: 0x0600A08F RID: 41103 RVA: 0x003C3EC4 File Offset: 0x003C20C4
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

	// Token: 0x0600A090 RID: 41104 RVA: 0x003C3F18 File Offset: 0x003C2118
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

	// Token: 0x0600A091 RID: 41105 RVA: 0x003C404B File Offset: 0x003C224B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnApplicationQuit()
	{
		if (this.texture != null)
		{
			global::UnityEngine.Object.Destroy(this.texture);
		}
	}

	// Token: 0x0600A092 RID: 41106 RVA: 0x003C4066 File Offset: 0x003C2266
	[PublicizedFrom(EAccessModifier.Private)]
	public void initFpsGraph()
	{
		this.texture = GUIFPS.createGUITexture();
		this.guiFpsGraphTexture = base.gameObject.AddMissingComponent<UITexture>();
		this.guiFpsGraphTexture.mainTexture = this.texture;
		this.guiFpsGraphTexture.enabled = false;
	}

	// Token: 0x0600A093 RID: 41107 RVA: 0x003C40A4 File Offset: 0x003C22A4
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

	// Token: 0x0600A094 RID: 41108 RVA: 0x003C4100 File Offset: 0x003C2300
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

	// Token: 0x040078B3 RID: 30899
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bEnabled;

	// Token: 0x040078B4 RID: 30900
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public FPS fps = new FPS(0.5f);

	// Token: 0x040078B5 RID: 30901
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string format;

	// Token: 0x040078B6 RID: 30902
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int BaseTextSize = 13;

	// Token: 0x040078B7 RID: 30903
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bShowGraph;

	// Token: 0x040078B8 RID: 30904
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D texture;

	// Token: 0x040078B9 RID: 30905
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int curGraphXPos;

	// Token: 0x040078BA RID: 30906
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UITexture guiFpsGraphTexture;

	// Token: 0x040078BB RID: 30907
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public long lastTotalMemory;

	// Token: 0x040078BC RID: 30908
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int gcSpikeCounter;

	// Token: 0x040078BD RID: 30909
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cBarHeight = 2500f;

	// Token: 0x040078BE RID: 30910
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2i lastResolution;

	// Token: 0x040078BF RID: 30911
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIStyle boxStyle;

	// Token: 0x040078C0 RID: 30912
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int boxAreaHeight;

	// Token: 0x040078C1 RID: 30913
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int boxAreaWidth;

	// Token: 0x040078C2 RID: 30914
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager windowManager;
}
