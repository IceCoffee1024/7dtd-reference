using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000564 RID: 1380
public class NguiWdwTerrainEditor : MonoBehaviour, INGuiButtonOnClick
{
	// Token: 0x06002CC4 RID: 11460 RVA: 0x0011DDF4 File Offset: 0x0011BFF4
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

	// Token: 0x06002CC5 RID: 11461 RVA: 0x0011E03D File Offset: 0x0011C23D
	public void InGameMenuOpen(bool _isOpen)
	{
		if (_isOpen)
		{
			this.anchor.pixelOffset = new Vector2(150f, -7f);
			return;
		}
		this.anchor.pixelOffset = new Vector2(0f, -7f);
	}

	// Token: 0x06002CC6 RID: 11462 RVA: 0x0011E078 File Offset: 0x0011C278
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

	// Token: 0x06002CC7 RID: 11463 RVA: 0x0011E0C9 File Offset: 0x0011C2C9
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

	// Token: 0x06002CC8 RID: 11464 RVA: 0x0011E104 File Offset: 0x0011C304
	public void OnSizeChange()
	{
		float value = UIProgressBar.current.value;
		this.size = (int)(value * 32f);
		this.brush.Falloff = this.size;
		this.brush.Size = (int)((float)this.size * this.hardness);
		this.sizeVal.text = this.size.ToString();
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x0011E16C File Offset: 0x0011C36C
	public void OnFalloffChange()
	{
		float value = UIProgressBar.current.value;
		this.hardness = value;
		this.brush.Falloff = this.size;
		this.brush.Size = (int)((float)this.size * this.hardness);
		this.falloffVal.text = this.hardness.ToCultureInvariantString();
	}

	// Token: 0x06002CCA RID: 11466 RVA: 0x0011E1CC File Offset: 0x0011C3CC
	public void OnStrengthChange()
	{
		float value = UIProgressBar.current.value;
		this.flow = value;
		this.brush.Strength = (int)(this.flow * 127f);
		this.strengthVal.text = this.flow.ToCultureInvariantString();
	}

	// Token: 0x06002CCB RID: 11467 RVA: 0x0011E219 File Offset: 0x0011C419
	public void HideWindow(bool _hide)
	{
		if (_hide)
		{
			this.panel.alpha = 0f;
			return;
		}
		this.panel.alpha = 1f;
	}

	// Token: 0x06002CCC RID: 11468 RVA: 0x0011E240 File Offset: 0x0011C440
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

	// Token: 0x06002CCD RID: 11469 RVA: 0x0011E310 File Offset: 0x0011C510
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

	// Token: 0x04002314 RID: 8980
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameManager gm;

	// Token: 0x04002315 RID: 8981
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<IBlockTool> tools;

	// Token: 0x04002316 RID: 8982
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Transform> toolButtons;

	// Token: 0x04002317 RID: 8983
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BlockTools.Brush brush;

	// Token: 0x04002318 RID: 8984
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject toolButtonPrefab;

	// Token: 0x04002319 RID: 8985
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform thisWindow;

	// Token: 0x0400231A RID: 8986
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform toolGrid;

	// Token: 0x0400231B RID: 8987
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform projectorParent;

	// Token: 0x0400231C RID: 8988
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Projector brushSizeProjector;

	// Token: 0x0400231D RID: 8989
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Projector brushFalloffPojector;

	// Token: 0x0400231E RID: 8990
	public Vector3i lastPosition;

	// Token: 0x0400231F RID: 8991
	public Vector3 lastDirection;

	// Token: 0x04002320 RID: 8992
	public Texture2D[] buttonTextures;

	// Token: 0x04002321 RID: 8993
	public string[] buttonNames;

	// Token: 0x04002322 RID: 8994
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel sizeVal;

	// Token: 0x04002323 RID: 8995
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel falloffVal;

	// Token: 0x04002324 RID: 8996
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel strengthVal;

	// Token: 0x04002325 RID: 8997
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIAnchor anchor;

	// Token: 0x04002326 RID: 8998
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x04002327 RID: 8999
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager nguiWindowManager;

	// Token: 0x04002328 RID: 9000
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int size;

	// Token: 0x04002329 RID: 9001
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float hardness;

	// Token: 0x0400232A RID: 9002
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float flow;
}
