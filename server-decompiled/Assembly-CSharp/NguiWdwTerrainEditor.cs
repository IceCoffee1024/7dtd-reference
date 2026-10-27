using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000576 RID: 1398
public class NguiWdwTerrainEditor : MonoBehaviour, INGuiButtonOnClick
{
	// Token: 0x06002CEF RID: 11503 RVA: 0x001202AC File Offset: 0x0011E4AC
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

	// Token: 0x06002CF0 RID: 11504 RVA: 0x001204F5 File Offset: 0x0011E6F5
	public void InGameMenuOpen(bool _isOpen)
	{
		if (_isOpen)
		{
			this.anchor.pixelOffset = new Vector2(150f, -7f);
			return;
		}
		this.anchor.pixelOffset = new Vector2(0f, -7f);
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x00120530 File Offset: 0x0011E730
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

	// Token: 0x06002CF2 RID: 11506 RVA: 0x00120581 File Offset: 0x0011E781
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

	// Token: 0x06002CF3 RID: 11507 RVA: 0x001205BC File Offset: 0x0011E7BC
	public void OnSizeChange()
	{
		float value = UIProgressBar.current.value;
		this.size = (int)(value * 32f);
		this.brush.Falloff = this.size;
		this.brush.Size = (int)((float)this.size * this.hardness);
		this.sizeVal.text = this.size.ToString();
	}

	// Token: 0x06002CF4 RID: 11508 RVA: 0x00120624 File Offset: 0x0011E824
	public void OnFalloffChange()
	{
		float value = UIProgressBar.current.value;
		this.hardness = value;
		this.brush.Falloff = this.size;
		this.brush.Size = (int)((float)this.size * this.hardness);
		this.falloffVal.text = this.hardness.ToCultureInvariantString();
	}

	// Token: 0x06002CF5 RID: 11509 RVA: 0x00120684 File Offset: 0x0011E884
	public void OnStrengthChange()
	{
		float value = UIProgressBar.current.value;
		this.flow = value;
		this.brush.Strength = (int)(this.flow * 127f);
		this.strengthVal.text = this.flow.ToCultureInvariantString();
	}

	// Token: 0x06002CF6 RID: 11510 RVA: 0x001206D1 File Offset: 0x0011E8D1
	public void HideWindow(bool _hide)
	{
		if (_hide)
		{
			this.panel.alpha = 0f;
			return;
		}
		this.panel.alpha = 1f;
	}

	// Token: 0x06002CF7 RID: 11511 RVA: 0x001206F8 File Offset: 0x0011E8F8
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

	// Token: 0x06002CF8 RID: 11512 RVA: 0x001207C8 File Offset: 0x0011E9C8
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

	// Token: 0x0400236B RID: 9067
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameManager gm;

	// Token: 0x0400236C RID: 9068
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<IBlockTool> tools;

	// Token: 0x0400236D RID: 9069
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Transform> toolButtons;

	// Token: 0x0400236E RID: 9070
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BlockTools.Brush brush;

	// Token: 0x0400236F RID: 9071
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject toolButtonPrefab;

	// Token: 0x04002370 RID: 9072
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform thisWindow;

	// Token: 0x04002371 RID: 9073
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform toolGrid;

	// Token: 0x04002372 RID: 9074
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform projectorParent;

	// Token: 0x04002373 RID: 9075
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Projector brushSizeProjector;

	// Token: 0x04002374 RID: 9076
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Projector brushFalloffPojector;

	// Token: 0x04002375 RID: 9077
	public Vector3i lastPosition;

	// Token: 0x04002376 RID: 9078
	public Vector3 lastDirection;

	// Token: 0x04002377 RID: 9079
	public Texture2D[] buttonTextures;

	// Token: 0x04002378 RID: 9080
	public string[] buttonNames;

	// Token: 0x04002379 RID: 9081
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel sizeVal;

	// Token: 0x0400237A RID: 9082
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel falloffVal;

	// Token: 0x0400237B RID: 9083
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel strengthVal;

	// Token: 0x0400237C RID: 9084
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIAnchor anchor;

	// Token: 0x0400237D RID: 9085
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x0400237E RID: 9086
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager nguiWindowManager;

	// Token: 0x0400237F RID: 9087
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int size;

	// Token: 0x04002380 RID: 9088
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float hardness;

	// Token: 0x04002381 RID: 9089
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float flow;
}
