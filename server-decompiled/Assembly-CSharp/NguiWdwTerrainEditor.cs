using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000569 RID: 1385
public class NguiWdwTerrainEditor : MonoBehaviour, INGuiButtonOnClick
{
	// Token: 0x06002CEE RID: 11502 RVA: 0x0011EE68 File Offset: 0x0011D068
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.nguiWindowManager = base.GetComponentInParent<NGUIWindowManager>();
		this.gm = (GameManager)global::UnityEngine.Object.FindAnyObjectByType(typeof(GameManager));
		if (!GameModeEditWorld.TypeName.Equals(GamePrefs.GetString(EnumGamePrefs.GameMode)))
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.thisWindow = base.transform;
		this.toolGrid = this.thisWindow.Find("Toolbox/ToolGrid");
		this.sizeVal = this.thisWindow.Find("Toolbox/Sliders/1_Size/Value").GetComponent<UILabel>();
		this.falloffVal = this.thisWindow.Find("Toolbox/Sliders/2_Falloff/Value").GetComponent<UILabel>();
		this.strengthVal = this.thisWindow.Find("Toolbox/Sliders/3_Strength/Value").GetComponent<UILabel>();
		this.anchor = this.thisWindow.GetComponent<UIAnchor>();
		this.tools = new List<IBlockTool>();
		this.brush = new BlockTools.Brush(BlockTools.Brush.BrushShape.Sphere, 1, 10, 80);
		this.toolButtonPrefab = this.thisWindow.Find("Toolbox/ToolButton").gameObject;
		this.toolButtons = new List<Transform>();
		this.tools.Add(new BlockToolTerrainAdjust(this.brush, this));
		this.tools.Add(new BlockToolTerrainSmoothing(this.brush, this));
		this.tools.Add(new BlockToolTerrainPaint(this.brush, this));
		GameObject gameObject = Resources.Load("Prefabs/prefabTerrainBrush") as GameObject;
		if (gameObject == null)
		{
			return;
		}
		this.projectorParent = global::UnityEngine.Object.Instantiate<GameObject>(gameObject).transform;
		for (int i = 0; i < this.tools.Count; i++)
		{
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(this.toolButtonPrefab);
			gameObject2.SetActive(true);
			gameObject2.name = this.tools[i].ToString() + " Button";
			Transform transform = gameObject2.transform;
			transform.parent = this.toolGrid;
			transform.localPosition = Vector3.zero;
			transform.localScale = Vector3.one;
			transform.GetComponent<NGuiButtonOnClickHandler>().OnClickDelegate = this;
			this.toolButtons.Add(transform);
		}
		this.toolGrid.GetComponent<UIGrid>().repositionNow = true;
		this.gm.SetActiveBlockTool(this.tools[0]);
		this.panel = base.GetComponent<UIPanel>();
	}

	// Token: 0x06002CEF RID: 11503 RVA: 0x0011F0B1 File Offset: 0x0011D2B1
	public void InGameMenuOpen(bool _isOpen)
	{
		if (_isOpen)
		{
			this.anchor.pixelOffset = new Vector2(150f, -7f);
			return;
		}
		this.anchor.pixelOffset = new Vector2(0f, -7f);
	}

	// Token: 0x06002CF0 RID: 11504 RVA: 0x0011F0EC File Offset: 0x0011D2EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		if (this.projectorParent != null)
		{
			this.projectorParent.gameObject.SetActive(true);
		}
		if (this.gm != null)
		{
			this.gm.SetActiveBlockTool(this.tools[0]);
		}
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x0011F13D File Offset: 0x0011D33D
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDisable()
	{
		if (this.projectorParent != null)
		{
			this.projectorParent.gameObject.SetActive(false);
		}
		if (this.gm != null)
		{
			this.gm.SetActiveBlockTool(null);
		}
	}

	// Token: 0x06002CF2 RID: 11506 RVA: 0x0011F178 File Offset: 0x0011D378
	public void OnSizeChange()
	{
		float value = UIProgressBar.current.value;
		this.size = (int)(value * 32f);
		this.brush.Falloff = this.size;
		this.brush.Size = (int)((float)this.size * this.hardness);
		this.sizeVal.text = this.size.ToString();
	}

	// Token: 0x06002CF3 RID: 11507 RVA: 0x0011F1E0 File Offset: 0x0011D3E0
	public void OnFalloffChange()
	{
		float value = UIProgressBar.current.value;
		this.hardness = value;
		this.brush.Falloff = this.size;
		this.brush.Size = (int)((float)this.size * this.hardness);
		this.falloffVal.text = this.hardness.ToCultureInvariantString();
	}

	// Token: 0x06002CF4 RID: 11508 RVA: 0x0011F240 File Offset: 0x0011D440
	public void OnStrengthChange()
	{
		float value = UIProgressBar.current.value;
		this.flow = value;
		this.brush.Strength = (int)(this.flow * 127f);
		this.strengthVal.text = this.flow.ToCultureInvariantString();
	}

	// Token: 0x06002CF5 RID: 11509 RVA: 0x0011F28D File Offset: 0x0011D48D
	public void HideWindow(bool _hide)
	{
		if (_hide)
		{
			this.panel.alpha = 0f;
			return;
		}
		this.panel.alpha = 1f;
	}

	// Token: 0x06002CF6 RID: 11510 RVA: 0x0011F2B4 File Offset: 0x0011D4B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (this.projectorParent != null)
		{
			this.projectorParent.parent = null;
			this.projectorParent.position = this.lastPosition.ToVector3();
			this.projectorParent.localScale = Vector3.one;
			this.projectorParent.Find("Size").transform.localScale = Vector3.one * (float)(this.brush.Size * 2);
			this.projectorParent.Find("Falloff").transform.localScale = Vector3.one * (float)(this.brush.Falloff * 2);
		}
		this.InGameMenuOpen(this.nguiWindowManager.WindowManager.IsWindowOpen(XUiC_InGameMenuWindow.ID));
	}

	// Token: 0x06002CF7 RID: 11511 RVA: 0x0011F384 File Offset: 0x0011D584
	public void NGuiButtonOnClick(Transform _t)
	{
		for (int i = 0; i < this.toolButtons.Count; i++)
		{
			if (_t == this.toolButtons[i])
			{
				this.gm.SetActiveBlockTool(this.tools[i]);
				Log.Out(this.tools[i].ToString());
			}
			this.toolButtons[i].Find("Highlight").gameObject.SetActive(_t == this.toolButtons[i]);
		}
	}

	// Token: 0x04002346 RID: 9030
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameManager gm;

	// Token: 0x04002347 RID: 9031
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<IBlockTool> tools;

	// Token: 0x04002348 RID: 9032
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Transform> toolButtons;

	// Token: 0x04002349 RID: 9033
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BlockTools.Brush brush;

	// Token: 0x0400234A RID: 9034
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject toolButtonPrefab;

	// Token: 0x0400234B RID: 9035
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform thisWindow;

	// Token: 0x0400234C RID: 9036
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform toolGrid;

	// Token: 0x0400234D RID: 9037
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform projectorParent;

	// Token: 0x0400234E RID: 9038
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Projector brushSizeProjector;

	// Token: 0x0400234F RID: 9039
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Projector brushFalloffPojector;

	// Token: 0x04002350 RID: 9040
	public Vector3i lastPosition;

	// Token: 0x04002351 RID: 9041
	public Vector3 lastDirection;

	// Token: 0x04002352 RID: 9042
	public Texture2D[] buttonTextures;

	// Token: 0x04002353 RID: 9043
	public string[] buttonNames;

	// Token: 0x04002354 RID: 9044
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel sizeVal;

	// Token: 0x04002355 RID: 9045
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel falloffVal;

	// Token: 0x04002356 RID: 9046
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel strengthVal;

	// Token: 0x04002357 RID: 9047
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIAnchor anchor;

	// Token: 0x04002358 RID: 9048
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x04002359 RID: 9049
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager nguiWindowManager;

	// Token: 0x0400235A RID: 9050
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int size;

	// Token: 0x0400235B RID: 9051
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float hardness;

	// Token: 0x0400235C RID: 9052
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float flow;
}
