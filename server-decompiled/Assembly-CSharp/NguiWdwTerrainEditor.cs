using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200056F RID: 1391
public class NguiWdwTerrainEditor : MonoBehaviour, INGuiButtonOnClick
{
	// Token: 0x06002CE4 RID: 11492 RVA: 0x0011FCEC File Offset: 0x0011DEEC
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

	// Token: 0x06002CE5 RID: 11493 RVA: 0x0011FF35 File Offset: 0x0011E135
	public void InGameMenuOpen(bool _isOpen)
	{
		if (_isOpen)
		{
			this.anchor.pixelOffset = new Vector2(150f, -7f);
			return;
		}
		this.anchor.pixelOffset = new Vector2(0f, -7f);
	}

	// Token: 0x06002CE6 RID: 11494 RVA: 0x0011FF70 File Offset: 0x0011E170
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

	// Token: 0x06002CE7 RID: 11495 RVA: 0x0011FFC1 File Offset: 0x0011E1C1
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

	// Token: 0x06002CE8 RID: 11496 RVA: 0x0011FFFC File Offset: 0x0011E1FC
	public void OnSizeChange()
	{
		float value = UIProgressBar.current.value;
		this.size = (int)(value * 32f);
		this.brush.Falloff = this.size;
		this.brush.Size = (int)((float)this.size * this.hardness);
		this.sizeVal.text = this.size.ToString();
	}

	// Token: 0x06002CE9 RID: 11497 RVA: 0x00120064 File Offset: 0x0011E264
	public void OnFalloffChange()
	{
		float value = UIProgressBar.current.value;
		this.hardness = value;
		this.brush.Falloff = this.size;
		this.brush.Size = (int)((float)this.size * this.hardness);
		this.falloffVal.text = this.hardness.ToCultureInvariantString();
	}

	// Token: 0x06002CEA RID: 11498 RVA: 0x001200C4 File Offset: 0x0011E2C4
	public void OnStrengthChange()
	{
		float value = UIProgressBar.current.value;
		this.flow = value;
		this.brush.Strength = (int)(this.flow * 127f);
		this.strengthVal.text = this.flow.ToCultureInvariantString();
	}

	// Token: 0x06002CEB RID: 11499 RVA: 0x00120111 File Offset: 0x0011E311
	public void HideWindow(bool _hide)
	{
		if (_hide)
		{
			this.panel.alpha = 0f;
			return;
		}
		this.panel.alpha = 1f;
	}

	// Token: 0x06002CEC RID: 11500 RVA: 0x00120138 File Offset: 0x0011E338
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

	// Token: 0x06002CED RID: 11501 RVA: 0x00120208 File Offset: 0x0011E408
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

	// Token: 0x04002350 RID: 9040
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameManager gm;

	// Token: 0x04002351 RID: 9041
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<IBlockTool> tools;

	// Token: 0x04002352 RID: 9042
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Transform> toolButtons;

	// Token: 0x04002353 RID: 9043
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BlockTools.Brush brush;

	// Token: 0x04002354 RID: 9044
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject toolButtonPrefab;

	// Token: 0x04002355 RID: 9045
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform thisWindow;

	// Token: 0x04002356 RID: 9046
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform toolGrid;

	// Token: 0x04002357 RID: 9047
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform projectorParent;

	// Token: 0x04002358 RID: 9048
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Projector brushSizeProjector;

	// Token: 0x04002359 RID: 9049
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Projector brushFalloffPojector;

	// Token: 0x0400235A RID: 9050
	public Vector3i lastPosition;

	// Token: 0x0400235B RID: 9051
	public Vector3 lastDirection;

	// Token: 0x0400235C RID: 9052
	public Texture2D[] buttonTextures;

	// Token: 0x0400235D RID: 9053
	public string[] buttonNames;

	// Token: 0x0400235E RID: 9054
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel sizeVal;

	// Token: 0x0400235F RID: 9055
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel falloffVal;

	// Token: 0x04002360 RID: 9056
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel strengthVal;

	// Token: 0x04002361 RID: 9057
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIAnchor anchor;

	// Token: 0x04002362 RID: 9058
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x04002363 RID: 9059
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager nguiWindowManager;

	// Token: 0x04002364 RID: 9060
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int size;

	// Token: 0x04002365 RID: 9061
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float hardness;

	// Token: 0x04002366 RID: 9062
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float flow;
}
