using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A52 RID: 2642
public class SelectionBox : MonoBehaviour
{
	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x06004FD4 RID: 20436 RVA: 0x001E5FDC File Offset: 0x001E41DC
	public SelectionCategory Category
	{
		get
		{
			return this.ownerCategory;
		}
	}

	// Token: 0x17000891 RID: 2193
	// (get) Token: 0x06004FD5 RID: 20437 RVA: 0x001E5FE4 File Offset: 0x001E41E4
	// (set) Token: 0x06004FD6 RID: 20438 RVA: 0x001E5FEC File Offset: 0x001E41EC
	public new string name
	{
		get
		{
			return this.boxName;
		}
		set
		{
			if (this.boxName == value)
			{
				return;
			}
			this.boxName = value;
			base.name = value;
		}
	}

	// Token: 0x17000892 RID: 2194
	// (get) Token: 0x06004FD7 RID: 20439 RVA: 0x001E600B File Offset: 0x001E420B
	// (set) Token: 0x06004FD8 RID: 20440 RVA: 0x001E6013 File Offset: 0x001E4213
	public object UserData
	{
		get
		{
			return this.userData;
		}
		set
		{
			this.userData = value;
			ISelectionBoxCallback boxCallbacks = this.ownerCategory.BoxCallbacks;
			if (boxCallbacks == null)
			{
				return;
			}
			boxCallbacks.OnSelectionBoxUserDataChanged(this);
		}
	}

	// Token: 0x17000893 RID: 2195
	// (get) Token: 0x06004FD9 RID: 20441 RVA: 0x001E6032 File Offset: 0x001E4232
	public Vector3i Size
	{
		get
		{
			return this.size;
		}
	}

	// Token: 0x17000894 RID: 2196
	// (get) Token: 0x06004FDA RID: 20442 RVA: 0x001E603A File Offset: 0x001E423A
	// (set) Token: 0x06004FDB RID: 20443 RVA: 0x001E6042 File Offset: 0x001E4242
	public Vector3i Position
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000895 RID: 2197
	// (get) Token: 0x06004FDC RID: 20444 RVA: 0x001E604B File Offset: 0x001E424B
	// (set) Token: 0x06004FDD RID: 20445 RVA: 0x001E6053 File Offset: 0x001E4253
	public float FacingDirection
	{
		get
		{
			return this.facingDirection;
		}
		set
		{
			if (Mathf.Approximately(this.facingDirection, value))
			{
				return;
			}
			this.facingDirection = value;
		}
	}

	// Token: 0x17000896 RID: 2198
	// (get) Token: 0x06004FDE RID: 20446 RVA: 0x001E606B File Offset: 0x001E426B
	public bool IsActive
	{
		get
		{
			return SelectionBoxManager.Instance.Selection == this;
		}
	}

	// Token: 0x06004FDF RID: 20447 RVA: 0x001E6080 File Offset: 0x001E4280
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		GameObject gameObject = new GameObject("Box");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = this.bounds.size;
		this.m_MeshFilter = gameObject.AddComponent<MeshFilter>();
		this.m_MeshRenderer = gameObject.AddComponent<MeshRenderer>();
		for (int i = 0; i < this.materialsArr.Length; i++)
		{
			this.materialsArr[i] = new Material(Resources.Load<Shader>("Shaders/SelectionBox"));
			this.materialsArr[i].renderQueue = -1;
		}
		this.m_MeshRenderer.materials = this.materialsArr;
		for (int j = 0; j < this.subMeshTriangles.Length; j++)
		{
			this.subMeshTriangles[j] = new List<int>();
		}
		this.ResetAllFacesColor();
	}

	// Token: 0x06004FE0 RID: 20448 RVA: 0x001E6156 File Offset: 0x001E4356
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.camPostRender));
	}

	// Token: 0x06004FE1 RID: 20449 RVA: 0x001E6178 File Offset: 0x001E4378
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.camPostRender));
		Utils.CleanupMaterials<Material[]>(this.materialsArr);
		Utils.CleanupMaterialsOfRenderer(this.m_MeshRenderer);
		if (this.frame != null)
		{
			global::UnityEngine.Object.Destroy(this.frame);
		}
	}

	// Token: 0x06004FE2 RID: 20450 RVA: 0x001E61D4 File Offset: 0x001E43D4
	public void SetOwner(SelectionCategory _selectionCategory)
	{
		this.ownerCategory = _selectionCategory;
	}

	// Token: 0x06004FE3 RID: 20451 RVA: 0x001E61E0 File Offset: 0x001E43E0
	public void SetAllFacesColor(Color _c, bool useAlphaMultiplier = true)
	{
		if (useAlphaMultiplier)
		{
			_c.a *= SelectionBoxManager.Instance.AlphaMultiplier;
		}
		if (this.m_MeshRenderer != null)
		{
			Material[] materials = this.m_MeshRenderer.materials;
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].color = _c;
			}
		}
		this.curColor = _c;
	}

	// Token: 0x06004FE4 RID: 20452 RVA: 0x001E6240 File Offset: 0x001E4440
	public void ResetAllFacesColor()
	{
		this.SetAllFacesColor(this.curColor, false);
	}

	// Token: 0x06004FE5 RID: 20453 RVA: 0x001E624F File Offset: 0x001E444F
	public void SetFaceColor(BlockFace _face, Color _c)
	{
		this.m_MeshRenderer.materials[(int)_face].color = _c;
		this.faceColorsSet[(int)_face] = true;
	}

	// Token: 0x06004FE6 RID: 20454 RVA: 0x001E6270 File Offset: 0x001E4470
	public void SetCaption(string _text)
	{
		if (this.captionMesh == null)
		{
			GameObject gameObject = new GameObject("Caption");
			gameObject.transform.parent = base.transform;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
			gameObject.transform.localPosition = new Vector3(0f, this.bounds.size.y + 0.1f, 0f);
			this.captionMesh = gameObject.AddMissingComponent<TextMesh>();
			this.captionMesh.alignment = TextAlignment.Center;
			this.captionMesh.anchor = TextAnchor.MiddleCenter;
			this.captionMesh.fontSize = 20;
			this.captionMesh.color = Color.green;
			gameObject.SetActive(true);
		}
		this.captionMesh.text = _text;
	}

	// Token: 0x06004FE7 RID: 20455 RVA: 0x001E636D File Offset: 0x001E456D
	public void SetCaptionVisibility(bool _visible)
	{
		if (this.captionMesh == null)
		{
			return;
		}
		this.captionMesh.gameObject.SetActive(_visible);
	}

	// Token: 0x06004FE8 RID: 20456 RVA: 0x001E6390 File Offset: 0x001E4590
	public void SetPositionAndSize(Vector3i _pos, Vector3i _size)
	{
		base.transform.localPosition = _pos + new Vector3((float)_size.x * 0.5f, -0.1f, (float)_size.z * 0.5f) - Origin.position;
		this.bounds = BoundsUtils.BoundsForMinMax(_pos, _pos + _size);
		this.bounds.size = this.bounds.size + new Vector3(0.16f, 0.16f, 0.16f);
		Transform boxTransform = this.GetBoxTransform();
		if (boxTransform != null)
		{
			boxTransform.localScale = this.bounds.size;
		}
		if (this.size != _size)
		{
			this.BuildFrame();
			if (this.captionMesh != null)
			{
				this.captionMesh.transform.localPosition = new Vector3(0f, this.bounds.size.y + 0.1f, 0f);
			}
			this.UpdateSizeMeshes(_size);
		}
		this.Position = _pos;
		this.size = _size;
	}

	// Token: 0x06004FE9 RID: 20457 RVA: 0x001E64B5 File Offset: 0x001E46B5
	public void SetVisible(bool _visible)
	{
		if (base.gameObject.activeSelf == _visible)
		{
			return;
		}
		base.gameObject.SetActive(_visible);
		if (_visible)
		{
			this.ResetAllFacesColor();
			this.SetFrameActive(false);
		}
	}

	// Token: 0x06004FEA RID: 20458 RVA: 0x001E64E2 File Offset: 0x001E46E2
	public Vector3i GetScale()
	{
		return new Vector3i(this.bounds.size);
	}

	// Token: 0x06004FEB RID: 20459 RVA: 0x001E64F4 File Offset: 0x001E46F4
	public Transform GetBoxTransform()
	{
		return base.transform.Find("Box");
	}

	// Token: 0x06004FEC RID: 20460 RVA: 0x001E6506 File Offset: 0x001E4706
	public void SetFrameActive(bool _active)
	{
		this.SetFrameColor(_active ? SelectionBox.activeFrameColor : SelectionBox.inActiveFrameColor);
	}

	// Token: 0x06004FED RID: 20461 RVA: 0x001E651D File Offset: 0x001E471D
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFrameColor(Color _color)
	{
		this.curFrameColor = _color;
		if (this.frame != null)
		{
			this.frame.GetComponent<MeshRenderer>().material.SetColor(SelectionBox.colorShaderProperty, _color);
		}
	}

	// Token: 0x06004FEE RID: 20462 RVA: 0x001E6550 File Offset: 0x001E4750
	public void ShowThroughWalls(bool _bShow)
	{
		if (this.curShowingThroughWalls == _bShow)
		{
			return;
		}
		this.curShowingThroughWalls = _bShow;
		int num = (_bShow ? 8 : 4);
		if (this.frame != null)
		{
			this.frame.GetComponent<MeshRenderer>().material.SetInt(SelectionBox.zTestShaderProperty, num);
		}
		Material[] materials = this.m_MeshRenderer.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetInt(SelectionBox.zTestShaderProperty, num);
		}
	}

	// Token: 0x06004FEF RID: 20463 RVA: 0x001E65C7 File Offset: 0x001E47C7
	public void EnableCollider(string _tag, int _layer)
	{
		this.collLayer = _layer;
		this.collTag = _tag;
	}

	// Token: 0x06004FF0 RID: 20464 RVA: 0x001E65D7 File Offset: 0x001E47D7
	public void HighlightAxis(Vector3i _hightlightedAxis)
	{
		this.hightlightedAxis = _hightlightedAxis;
	}

	// Token: 0x06004FF1 RID: 20465 RVA: 0x001E65E0 File Offset: 0x001E47E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (!this.bCreated && this.createMesh())
		{
			this.bCreated = true;
		}
	}

	// Token: 0x06004FF2 RID: 20466 RVA: 0x001E65FC File Offset: 0x001E47FC
	public void SetSizeVisibility(bool _visible)
	{
		if (_visible && this.sizeMeshes == null)
		{
			this.sizeMeshes = new TextMesh[SelectionBox.SizeTextMeshDefs.Length];
			for (int i = 0; i < this.sizeMeshes.Length; i++)
			{
				SelectionBox.SizeTextMeshDefinition sizeTextMeshDefinition = SelectionBox.SizeTextMeshDefs[i];
				GameObject gameObject = new GameObject("Size_" + sizeTextMeshDefinition.Name);
				gameObject.transform.parent = base.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.Scale(this.bounds.size, sizeTextMeshDefinition.Position);
				gameObject.transform.rotation = Quaternion.Euler(sizeTextMeshDefinition.Rotation);
				TextMesh textMesh = gameObject.AddMissingComponent<TextMesh>();
				this.sizeMeshes[i] = textMesh;
				textMesh.alignment = TextAlignment.Center;
				textMesh.anchor = TextAnchor.MiddleCenter;
				textMesh.characterSize = 0.1f;
				textMesh.fontSize = 20;
				textMesh.color = Color.green;
				textMesh.text = sizeTextMeshDefinition.Name;
				gameObject.SetActive(true);
			}
			this.UpdateSizeMeshes(this.size);
		}
		if (this.sizeMeshes == null)
		{
			return;
		}
		TextMesh[] array = this.sizeMeshes;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].gameObject.SetActive(_visible);
		}
	}

	// Token: 0x06004FF3 RID: 20467 RVA: 0x001E6748 File Offset: 0x001E4948
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateSizeMeshes(Vector3i _newSize)
	{
		if (this.sizeMeshes == null)
		{
			return;
		}
		for (int i = 0; i < this.sizeMeshes.Length; i++)
		{
			SelectionBox.SizeTextMeshDefinition sizeTextMeshDefinition = SelectionBox.SizeTextMeshDefs[i];
			this.sizeMeshes[i].text = string.Format("{0}{1} {2}{3} {4}{5}", new object[]
			{
				sizeTextMeshDefinition.Arrows[0],
				_newSize.x,
				sizeTextMeshDefinition.Arrows[1],
				_newSize.y,
				sizeTextMeshDefinition.Arrows[2],
				_newSize.z
			});
			this.sizeMeshes[i].transform.localPosition = Vector3.Scale(this.bounds.size, sizeTextMeshDefinition.Position);
		}
	}

	// Token: 0x06004FF4 RID: 20468 RVA: 0x001E6820 File Offset: 0x001E4A20
	public bool WorldPosWithinBox(Vector3i _pos)
	{
		Vector3i position = this.Position;
		Vector3i vector3i = position + this.size;
		return _pos.x >= position.x && _pos.x < vector3i.x && _pos.y >= position.y && _pos.y < vector3i.y && _pos.z >= position.z && _pos.z < vector3i.z;
	}

	// Token: 0x06004FF5 RID: 20469 RVA: 0x001E6898 File Offset: 0x001E4A98
	[PublicizedFrom(EAccessModifier.Private)]
	public void camPostRender(Camera _cam)
	{
		if (_cam != Camera.main)
		{
			return;
		}
		Vector3 position = GameManager.Instance.World.GetPrimaryPlayer().position;
		Vector3 center = this.bounds.center;
		float sqrMagnitude = (position - center).sqrMagnitude;
		bool isActive = this.IsActive;
		if (!isActive && sqrMagnitude > 62500f)
		{
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		GUIUtils.SetupLines(_cam, 3f);
		if (this.bDrawDirection && (isActive || this.bAlwaysDrawDirection))
		{
			Vector3 vector = base.transform.position + new Vector3(0f, this.bounds.size.y, 0f);
			float num = Mathf.Min(15f, 4f * Mathf.Max(this.bounds.size.x, this.bounds.size.z));
			float num2 = Mathf.Clamp((_cam.transform.position - vector).magnitude / 10f, 1f, num);
			Vector3 vector2 = Quaternion.AngleAxis(this.FacingDirection, Vector3.up) * Vector3.forward;
			vector += vector2 * 0.5f * Math.Min(this.bounds.size.x - 1f, this.bounds.size.z - 1f);
			GUIUtils.DrawTriangleWide(vector, vector2, Vector3.up, num2 * 0.5f, Color.black);
		}
		if (!isActive)
		{
			return;
		}
		if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 0)
		{
			return;
		}
		Vector3 vector3 = base.transform.position + new Vector3(0f, this.bounds.size.y * 0.5f, 0f);
		this.AxisOrigin = vector3;
		float num3 = Mathf.Max(1f, (_cam.transform.position - vector3).magnitude / 10f);
		Color color = new Color(0.3f, 0.05f, 0.05f);
		Color color2 = new Color(1f, 0.6f, 0.6f);
		Color color3 = new Color(0.05f, 0.2f, 0.05f);
		Color color4 = new Color(0f, 0.7f, 0f);
		Color color5 = new Color(0.6f, 1f, 0.6f);
		Color color6 = new Color(0.05f, 0.05f, 0.3f);
		Color color7 = new Color(0.4f, 0.4f, 1f);
		Color color8 = new Color(0.7f, 0.7f, 1f);
		this.Axises.Clear();
		this.AxisesI.Clear();
		if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 1)
		{
			this.Axises.Add(vector3 + num3 * Vector3.right);
			this.Axises.Add(vector3 + num3 * Vector3.up);
			this.Axises.Add(vector3 + num3 * Vector3.forward);
			this.AxisesI.Add(Vector3i.right);
			this.AxisesI.Add(Vector3i.up);
			this.AxisesI.Add(Vector3i.forward);
			GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[0], color, (this.hightlightedAxis.x != 0) ? color2 : Color.red);
			GUIUtils.DrawTriangleWide(this.Axises[0], (this.Axises[0] - this.AxisOrigin).normalized, Vector3.up, num3 * 0.125f, (this.hightlightedAxis.x != 0) ? Color.yellow : Color.red);
			GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[1], color3, (this.hightlightedAxis.y != 0) ? color5 : color4);
			GUIUtils.DrawTriangleWide(this.Axises[1], (this.Axises[1] - this.AxisOrigin).normalized, Vector3.right, num3 * 0.125f, (this.hightlightedAxis.y != 0) ? Color.yellow : color4);
			GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[2], color6, (this.hightlightedAxis.z != 0) ? color8 : color7);
			GUIUtils.DrawTriangleWide(this.Axises[2], (this.Axises[2] - this.AxisOrigin).normalized, Vector3.up, num3 * 0.125f, (this.hightlightedAxis.z != 0) ? Color.yellow : color7);
			return;
		}
		if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) != 2)
		{
			if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 3)
			{
				if (!this.ownerCategory.BoxCallbacks.OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities.CanMirror))
				{
					return;
				}
				this.Axises.Add(vector3 + num3 * Vector3.right);
				this.Axises.Add(vector3 - num3 * Vector3.right);
				this.Axises.Add(vector3 + num3 * Vector3.up);
				this.Axises.Add(vector3 - num3 * Vector3.up);
				this.Axises.Add(vector3 + num3 * Vector3.forward);
				this.Axises.Add(vector3 - num3 * Vector3.forward);
				this.AxisesI.Add(Vector3i.right);
				this.AxisesI.Add(Vector3i.left);
				this.AxisesI.Add(Vector3i.up);
				this.AxisesI.Add(Vector3i.down);
				this.AxisesI.Add(Vector3i.forward);
				this.AxisesI.Add(Vector3i.back);
				GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[0], color, (this.hightlightedAxis.x > 0) ? color2 : Color.red);
				GUIUtils.DrawLineWide(this.Axises[0] - Vector3.up * 0.2f, this.Axises[0] + Vector3.up * 0.2f, (this.hightlightedAxis.x > 0) ? Color.yellow : Color.red);
				GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[1], color, (this.hightlightedAxis.x < 0) ? color2 : Color.red);
				GUIUtils.DrawLineWide(this.Axises[1] - Vector3.up * 0.2f, this.Axises[1] + Vector3.up * 0.2f, (this.hightlightedAxis.x < 0) ? Color.yellow : Color.red);
				GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[2], color3, (this.hightlightedAxis.y > 0) ? color5 : color4);
				GUIUtils.DrawLineWide(this.Axises[2] - Vector3.right * 0.2f, this.Axises[2] + Vector3.right * 0.2f, (this.hightlightedAxis.y > 0) ? Color.yellow : color4);
				GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[3], color3, (this.hightlightedAxis.y < 0) ? color5 : color4);
				GUIUtils.DrawLineWide(this.Axises[3] - Vector3.right * 0.2f, this.Axises[3] + Vector3.right * 0.2f, (this.hightlightedAxis.y < 0) ? Color.yellow : color4);
				GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[4], color6, (this.hightlightedAxis.z > 0) ? color8 : color7);
				GUIUtils.DrawLineWide(this.Axises[4] - Vector3.right * 0.2f, this.Axises[4] + Vector3.right * 0.2f, (this.hightlightedAxis.z > 0) ? Color.yellow : color7);
				GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[5], color6, (this.hightlightedAxis.z < 0) ? color8 : color7);
				GUIUtils.DrawLineWide(this.Axises[5] - Vector3.right * 0.2f, this.Axises[5] + Vector3.right * 0.2f, (this.hightlightedAxis.z < 0) ? Color.yellow : color7);
			}
			return;
		}
		if (!this.ownerCategory.BoxCallbacks.OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities.CanResize))
		{
			return;
		}
		this.Axises.Add(vector3 + num3 * Vector3.right);
		this.Axises.Add(vector3 - num3 * Vector3.right);
		this.Axises.Add(vector3 + num3 * Vector3.up);
		this.Axises.Add(vector3 - num3 * Vector3.up);
		this.Axises.Add(vector3 + num3 * Vector3.forward);
		this.Axises.Add(vector3 - num3 * Vector3.forward);
		this.Axises.Add(vector3);
		this.AxisesI.Add(Vector3i.right);
		this.AxisesI.Add(Vector3i.left);
		this.AxisesI.Add(Vector3i.up);
		this.AxisesI.Add(Vector3i.down);
		this.AxisesI.Add(Vector3i.forward);
		this.AxisesI.Add(Vector3i.back);
		this.AxisesI.Add(Vector3i.one);
		GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[0], color, (this.hightlightedAxis.x > 0) ? color2 : Color.red);
		GUIUtils.DrawRectWide(this.Axises[0], (this.Axises[0] - this.AxisOrigin).normalized, Vector3.up, num3 * 0.125f, (this.hightlightedAxis.x > 0) ? Color.yellow : Color.red);
		GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[1], color, (this.hightlightedAxis.x < 0) ? color2 : Color.red);
		GUIUtils.DrawRectWide(this.Axises[1], (this.Axises[1] - this.AxisOrigin).normalized, Vector3.up, num3 * 0.125f, (this.hightlightedAxis.x < 0) ? Color.yellow : Color.red);
		GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[2], color3, (this.hightlightedAxis.y > 0) ? color5 : color4);
		GUIUtils.DrawRectWide(this.Axises[2], (this.Axises[2] - this.AxisOrigin).normalized, Vector3.right, num3 * 0.125f, (this.hightlightedAxis.y > 0) ? Color.yellow : color4);
		GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[3], color3, (this.hightlightedAxis.y < 0) ? color5 : color4);
		GUIUtils.DrawRectWide(this.Axises[3], (this.Axises[3] - this.AxisOrigin).normalized, Vector3.right, num3 * 0.125f, (this.hightlightedAxis.y < 0) ? Color.yellow : color4);
		GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[4], color6, (this.hightlightedAxis.z > 0) ? color8 : color7);
		GUIUtils.DrawRectWide(this.Axises[4], (this.Axises[4] - this.AxisOrigin).normalized, Vector3.up, num3 * 0.125f, (this.hightlightedAxis.z > 0) ? Color.yellow : color7);
		GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[5], color6, (this.hightlightedAxis.z < 0) ? color8 : color7);
		GUIUtils.DrawRectWide(this.Axises[5], (this.Axises[5] - this.AxisOrigin).normalized, Vector3.up, num3 * 0.125f, (this.hightlightedAxis.z < 0) ? Color.yellow : color7);
	}

	// Token: 0x06004FF6 RID: 20470 RVA: 0x001E7678 File Offset: 0x001E5878
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildFrame()
	{
		if (this.frame != null)
		{
			global::UnityEngine.Object.Destroy(this.frame);
			this.frame = null;
		}
		this.frame = new GameObject("Frame");
		this.frame.transform.parent = base.transform;
		this.frame.transform.localScale = Vector3.one;
		this.frame.transform.localPosition = Vector3.zero;
		float num = 0.02f;
		Bounds bounds = this.bounds;
		bounds.center = new Vector3(0f, this.bounds.size.y / 2f, 0f);
		Mesh mesh = new Mesh();
		mesh.Clear(false);
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		list.Add(new Vector3(bounds.min.x - num, bounds.min.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x - num, bounds.min.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.min.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.min.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.max.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x - num, bounds.max.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.max.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.max.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.min.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.min.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.min.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x + num, bounds.min.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.max.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.max.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.max.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x + num, bounds.max.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y - num, bounds.min.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y + num, bounds.min.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y - num, bounds.min.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y + num, bounds.min.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y - num, bounds.min.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y + num, bounds.min.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y - num, bounds.min.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y + num, bounds.min.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y + num, bounds.max.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y - num, bounds.max.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y - num, bounds.max.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y + num, bounds.max.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y + num, bounds.max.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y - num, bounds.max.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y - num, bounds.max.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y + num, bounds.max.z - num));
		list.Add(new Vector3(bounds.min.x - num, bounds.min.y + num, bounds.min.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.min.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y + num, bounds.min.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y - num, bounds.min.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.max.y - num, bounds.min.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.max.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y + num, bounds.min.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.min.y + num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.min.y + num, bounds.min.z + num));
		list.Add(new Vector3(bounds.max.x + num, bounds.max.y - num, bounds.min.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y - num, bounds.min.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.max.y - num, bounds.min.z - num));
		list.Add(new Vector3(bounds.min.x - num, bounds.min.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.min.y + num, bounds.max.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y + num, bounds.max.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.min.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.max.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.min.x - num, bounds.max.y - num, bounds.max.z - num));
		list.Add(new Vector3(bounds.min.x + num, bounds.max.y - num, bounds.max.z - num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.min.y + num, bounds.max.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.min.y + num, bounds.max.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.min.y + num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x + num, bounds.max.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y - num, bounds.max.z + num));
		list.Add(new Vector3(bounds.max.x - num, bounds.max.y - num, bounds.max.z - num));
		list.Add(new Vector3(bounds.max.x + num, bounds.max.y - num, bounds.max.z - num));
		for (int i = 0; i < 12; i++)
		{
			list2.Add(i * 8);
			list2.Add(i * 8 + 1);
			list2.Add(i * 8 + 2);
			list2.Add(i * 8 + 2);
			list2.Add(i * 8 + 3);
			list2.Add(i * 8);
			list2.Add(i * 8 + 3);
			list2.Add(i * 8 + 2);
			list2.Add(i * 8 + 7);
			list2.Add(i * 8 + 7);
			list2.Add(i * 8 + 4);
			list2.Add(i * 8 + 3);
			list2.Add(i * 8 + 4);
			list2.Add(i * 8 + 7);
			list2.Add(i * 8 + 6);
			list2.Add(i * 8 + 6);
			list2.Add(i * 8 + 5);
			list2.Add(i * 8 + 4);
			list2.Add(i * 8 + 5);
			list2.Add(i * 8 + 6);
			list2.Add(i * 8 + 1);
			list2.Add(i * 8 + 1);
			list2.Add(i * 8);
			list2.Add(i * 8 + 5);
			list2.Add(i * 8 + 1);
			list2.Add(i * 8 + 6);
			list2.Add(i * 8 + 7);
			list2.Add(i * 8 + 7);
			list2.Add(i * 8 + 2);
			list2.Add(i * 8 + 1);
			list2.Add(i * 8 + 5);
			list2.Add(i * 8);
			list2.Add(i * 8 + 3);
			list2.Add(i * 8 + 3);
			list2.Add(i * 8 + 4);
			list2.Add(i * 8 + 5);
		}
		mesh.SetVertices(list);
		mesh.SetIndices(list2.ToArray(), MeshTopology.Triangles, 0);
		this.frame.AddComponent<MeshFilter>().mesh = mesh;
		this.frame.AddComponent<MeshRenderer>().material = global::UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("Materials/SleeperVolumeFrame"));
		this.SetFrameColor(this.curFrameColor);
	}

	// Token: 0x06004FF7 RID: 20471 RVA: 0x001E8D60 File Offset: 0x001E6F60
	[PublicizedFrom(EAccessModifier.Private)]
	public void addQuad(Vector3 _v1, Vector2 _uv1, Vector3 _v2, Vector2 _uv2, Vector3 _v3, Vector2 _uv3, Vector3 _v4, Vector2 _uv4, BlockFace _face)
	{
		this.m_Vertices.Add(_v1);
		this.m_Vertices.Add(_v2);
		this.m_Vertices.Add(_v3);
		this.m_Vertices.Add(_v4);
		this.m_Uvs.Add(_uv1);
		this.m_Uvs.Add(_uv2);
		this.m_Uvs.Add(_uv3);
		this.m_Uvs.Add(_uv4);
		this.subMeshTriangles[(int)_face].Add(this.currentChunkMeshIndex);
		this.subMeshTriangles[(int)_face].Add(this.currentChunkMeshIndex + 2);
		this.subMeshTriangles[(int)_face].Add(this.currentChunkMeshIndex + 1);
		this.subMeshTriangles[(int)_face].Add(this.currentChunkMeshIndex + 3);
		this.subMeshTriangles[(int)_face].Add(this.currentChunkMeshIndex + 2);
		this.subMeshTriangles[(int)_face].Add(this.currentChunkMeshIndex);
		this.currentChunkMeshIndex += 4;
	}

	// Token: 0x06004FF8 RID: 20472 RVA: 0x001E8E60 File Offset: 0x001E7060
	[PublicizedFrom(EAccessModifier.Private)]
	public void addTriangle(Vector3 _v1, Vector2 _uv1, Vector3 _v2, Vector2 _uv2, Vector3 _v3, Vector2 _uv3, BlockFace _face)
	{
		this.m_Vertices.Add(_v1);
		this.m_Vertices.Add(_v2);
		this.m_Vertices.Add(_v3);
		this.m_Uvs.Add(_uv1);
		this.m_Uvs.Add(_uv2);
		this.m_Uvs.Add(_uv3);
		this.subMeshTriangles[(int)_face].Add(this.currentChunkMeshIndex);
		this.subMeshTriangles[(int)_face].Add(this.currentChunkMeshIndex + 2);
		this.subMeshTriangles[(int)_face].Add(this.currentChunkMeshIndex + 1);
		this.currentChunkMeshIndex += 3;
	}

	// Token: 0x06004FF9 RID: 20473 RVA: 0x001E8F08 File Offset: 0x001E7108
	[PublicizedFrom(EAccessModifier.Private)]
	public bool createMesh()
	{
		float num = -0.5f;
		float num2 = 0f;
		float num3 = -0.5f;
		Bounds bounds = BoundsUtils.BoundsForMinMax(Vector3.zero, Vector3.one);
		num += bounds.min.x;
		num2 += bounds.min.y;
		num3 += bounds.min.z;
		float num4 = bounds.max.x - bounds.min.x;
		float num5 = bounds.max.y - bounds.min.y;
		float num6 = bounds.max.z - bounds.min.z;
		for (int i = 0; i < this.subMeshTriangles.Length; i++)
		{
			this.subMeshTriangles[i].Clear();
		}
		this.m_Uvs.Clear();
		this.m_Vertices.Clear();
		this.currentChunkMeshIndex = 0;
		if (this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockInnerSides)
		{
			this.addQuad(new Vector3(num, num2, num3), new Vector2(1f, 0f), new Vector3(num, num2 + num5, num3), new Vector2(1f, 1f), new Vector3(num + num4, num2 + num5, num3), new Vector2(0f, 1f), new Vector3(num + num4, num2, num3), new Vector2(0f, 0f), BlockFace.South);
			this.addQuad(new Vector3(num, num2, num3 + num6), new Vector2(0f, 0f), new Vector3(num, num2 + num5, num3 + num6), new Vector2(0f, 1f), new Vector3(num, num2 + num5, num3), new Vector2(1f, 1f), new Vector3(num, num2, num3), new Vector2(1f, 0f), BlockFace.West);
			this.addQuad(new Vector3(num + num4, num2, num3 + num6), new Vector2(0f, 0f), new Vector3(num + num4, num2 + num5, num3 + num6), new Vector2(0f, 1f), new Vector3(num, num2 + num5, num3 + num6), new Vector2(1f, 1f), new Vector3(num, num2, num3 + num6), new Vector2(1f, 0f), BlockFace.North);
			this.addQuad(new Vector3(num + num4, num2, num3), new Vector2(1f, 0f), new Vector3(num + num4, num2 + num5, num3), new Vector2(1f, 1f), new Vector3(num + num4, num2 + num5, num3 + num6), new Vector2(0f, 1f), new Vector3(num + num4, num2, num3 + num6), new Vector2(0f, 0f), BlockFace.East);
			this.addQuad(new Vector3(num, num2 + num5, num3), new Vector2(1f, 0f), new Vector3(num, num2 + num5, num3 + num6), new Vector2(1f, 1f), new Vector3(num + num4, num2 + num5, num3 + num6), new Vector2(0f, 1f), new Vector3(num + num4, num2 + num5, num3), new Vector2(0f, 0f), BlockFace.Top);
			this.addQuad(new Vector3(num + num4, num2, num3), new Vector2(0f, 0f), new Vector3(num + num4, num2, num3 + num6), new Vector2(0f, 1f), new Vector3(num, num2, num3 + num6), new Vector2(1f, 1f), new Vector3(num, num2, num3), new Vector2(1f, 0f), BlockFace.Bottom);
		}
		if (this.focusType == RenderCubeType.FaceS || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
		{
			this.addQuad(new Vector3(num + num4, num2, num3), new Vector2(1f, 0f), new Vector3(num + num4, num2 + num5, num3), new Vector2(1f, 1f), new Vector3(num, num2 + num5, num3), new Vector2(0f, 1f), new Vector3(num, num2, num3), new Vector2(0f, 0f), BlockFace.South);
		}
		if (this.focusType == RenderCubeType.FaceW || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
		{
			this.addQuad(new Vector3(num, num2, num3), new Vector2(0f, 0f), new Vector3(num, num2 + num5, num3), new Vector2(0f, 1f), new Vector3(num, num2 + num5, num3 + num6), new Vector2(1f, 1f), new Vector3(num, num2, num3 + num6), new Vector2(1f, 0f), BlockFace.West);
		}
		if (this.focusType == RenderCubeType.FaceN || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
		{
			this.addQuad(new Vector3(num, num2, num3 + num6), new Vector2(0f, 0f), new Vector3(num, num2 + num5, num3 + num6), new Vector2(0f, 1f), new Vector3(num + num4, num2 + num5, num3 + num6), new Vector2(1f, 1f), new Vector3(num + num4, num2, num3 + num6), new Vector2(1f, 0f), BlockFace.North);
		}
		if (this.focusType == RenderCubeType.FaceE || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
		{
			this.addQuad(new Vector3(num + num4, num2, num3 + num6), new Vector2(1f, 0f), new Vector3(num + num4, num2 + num5, num3 + num6), new Vector2(1f, 1f), new Vector3(num + num4, num2 + num5, num3), new Vector2(0f, 1f), new Vector3(num + num4, num2, num3), new Vector2(0f, 0f), BlockFace.East);
		}
		if (this.focusType == RenderCubeType.FaceTop || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
		{
			this.addQuad(new Vector3(num + num4, num2 + num5, num3), new Vector2(1f, 0f), new Vector3(num + num4, num2 + num5, num3 + num6), new Vector2(1f, 1f), new Vector3(num, num2 + num5, num3 + num6), new Vector2(0f, 1f), new Vector3(num, num2 + num5, num3), new Vector2(0f, 0f), BlockFace.Top);
		}
		if (this.focusType == RenderCubeType.FaceBottom || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
		{
			this.addQuad(new Vector3(num, num2, num3), new Vector2(0f, 0f), new Vector3(num, num2, num3 + num6), new Vector2(0f, 1f), new Vector3(num + num4, num2, num3 + num6), new Vector2(1f, 1f), new Vector3(num + num4, num2, num3), new Vector2(1f, 0f), BlockFace.Bottom);
		}
		this.m_MeshFilter.mesh.Clear();
		this.m_MeshFilter.mesh.vertices = this.m_Vertices.ToArray();
		if (this.m_Uvs.Count > 0)
		{
			this.m_MeshFilter.mesh.uv = this.m_Uvs.ToArray();
		}
		this.m_MeshFilter.mesh.subMeshCount = this.subMeshTriangles.Length;
		for (int j = 0; j < this.subMeshTriangles.Length; j++)
		{
			this.m_MeshFilter.mesh.SetTriangles(this.subMeshTriangles[j].ToArray(), j);
		}
		this.m_MeshFilter.mesh.RecalculateNormals();
		if (this.collTag != null)
		{
			this.m_MeshFilter.gameObject.AddComponent<MeshCollider>().sharedMesh = this.copyMeshAndAddBackFaces(this.m_MeshFilter.mesh);
			GameObject gameObject = this.m_MeshFilter.gameObject;
			gameObject.tag = this.collTag;
			gameObject.layer = this.collLayer;
		}
		return true;
	}

	// Token: 0x06004FFA RID: 20474 RVA: 0x001E9738 File Offset: 0x001E7938
	[PublicizedFrom(EAccessModifier.Private)]
	public Mesh copyMeshAndAddBackFaces(Mesh _mesh)
	{
		Vector3[] vertices = _mesh.vertices;
		List<int> list = new List<int>();
		for (int i = 0; i < _mesh.subMeshCount; i++)
		{
			foreach (int num in _mesh.GetTriangles(i))
			{
				list.Add(num);
			}
		}
		int count = list.Count;
		for (int k = 0; k < count; k += 3)
		{
			list.Add(list[k]);
			list.Add(list[k + 2]);
			list.Add(list[k + 1]);
		}
		return new Mesh
		{
			vertices = vertices,
			triangles = list.ToArray()
		};
	}

	// Token: 0x04003D3F RID: 15679
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float boundsPadding = 0.16f;

	// Token: 0x04003D40 RID: 15680
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int maxFacingDirectionDistance = 62500;

	// Token: 0x04003D41 RID: 15681
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly int zTestShaderProperty = Shader.PropertyToID("_ZTest");

	// Token: 0x04003D42 RID: 15682
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly int colorShaderProperty = Shader.PropertyToID("_Color");

	// Token: 0x04003D43 RID: 15683
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SelectionCategory ownerCategory;

	// Token: 0x04003D44 RID: 15684
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string boxName;

	// Token: 0x04003D45 RID: 15685
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public object userData;

	// Token: 0x04003D46 RID: 15686
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject frame;

	// Token: 0x04003D47 RID: 15687
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextMesh captionMesh;

	// Token: 0x04003D48 RID: 15688
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextMesh[] sizeMeshes;

	// Token: 0x04003D49 RID: 15689
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i size = Vector3i.zero;

	// Token: 0x04003D4B RID: 15691
	public Bounds bounds = BoundsUtils.BoundsForMinMax(Vector3.zero, Vector3.one);

	// Token: 0x04003D4C RID: 15692
	public RenderCubeType focusType = RenderCubeType.FullBlockBothSides;

	// Token: 0x04003D4D RID: 15693
	public bool bAlwaysDrawDirection;

	// Token: 0x04003D4E RID: 15694
	public bool bDrawDirection;

	// Token: 0x04003D4F RID: 15695
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float facingDirection;

	// Token: 0x04003D50 RID: 15696
	public Vector3 AxisOrigin;

	// Token: 0x04003D51 RID: 15697
	public readonly List<Vector3> Axises = new List<Vector3>();

	// Token: 0x04003D52 RID: 15698
	public readonly List<Vector3i> AxisesI = new List<Vector3i>();

	// Token: 0x04003D53 RID: 15699
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i hightlightedAxis = Vector3i.zero;

	// Token: 0x04003D54 RID: 15700
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter m_MeshFilter;

	// Token: 0x04003D55 RID: 15701
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer m_MeshRenderer;

	// Token: 0x04003D56 RID: 15702
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<int>[] subMeshTriangles = new List<int>[6];

	// Token: 0x04003D57 RID: 15703
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<Vector2> m_Uvs = new List<Vector2>();

	// Token: 0x04003D58 RID: 15704
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<Vector3> m_Vertices = new List<Vector3>();

	// Token: 0x04003D59 RID: 15705
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Material[] materialsArr = new Material[6];

	// Token: 0x04003D5A RID: 15706
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int collLayer;

	// Token: 0x04003D5B RID: 15707
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string collTag;

	// Token: 0x04003D5C RID: 15708
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentChunkMeshIndex;

	// Token: 0x04003D5D RID: 15709
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bCreated;

	// Token: 0x04003D5E RID: 15710
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly bool[] faceColorsSet = new bool[6];

	// Token: 0x04003D5F RID: 15711
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color curColor;

	// Token: 0x04003D60 RID: 15712
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color curFrameColor = SelectionBox.inActiveFrameColor;

	// Token: 0x04003D61 RID: 15713
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool curShowingThroughWalls;

	// Token: 0x04003D62 RID: 15714
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Color inActiveFrameColor = Color.blue;

	// Token: 0x04003D63 RID: 15715
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Color activeFrameColor = Color.green;

	// Token: 0x04003D64 RID: 15716
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly SelectionBox.SizeTextMeshDefinition[] SizeTextMeshDefs = new SelectionBox.SizeTextMeshDefinition[]
	{
		new SelectionBox.SizeTextMeshDefinition("Top", Vector3.up + new Vector3(0f, 0f, 0.2f), new Vector3(90f, 0f, 0f), new char[] { '↔', '↗', '↕' }),
		new SelectionBox.SizeTextMeshDefinition("Front", Vector3.back * 0.5f + Vector3.up * 0.5f, new Vector3(0f, 0f, 0f), new char[] { '↔', '↕', '↗' }),
		new SelectionBox.SizeTextMeshDefinition("Back", Vector3.forward * 0.5f + Vector3.up * 0.5f, new Vector3(0f, 180f, 0f), new char[] { '↔', '↕', '↗' }),
		new SelectionBox.SizeTextMeshDefinition("Left", Vector3.left * 0.5f + Vector3.up * 0.5f, new Vector3(0f, 90f, 0f), new char[] { '↗', '↕', '↔' }),
		new SelectionBox.SizeTextMeshDefinition("Right", Vector3.right * 0.5f + Vector3.up * 0.5f, new Vector3(0f, -90f, 0f), new char[] { '↗', '↕', '↔' })
	};

	// Token: 0x02000A53 RID: 2643
	[PublicizedFrom(EAccessModifier.Private)]
	public class SizeTextMeshDefinition
	{
		// Token: 0x06004FFD RID: 20477 RVA: 0x001E9A7E File Offset: 0x001E7C7E
		public SizeTextMeshDefinition(string _name, Vector3 _position, Vector3 _rotation, char[] _arrows)
		{
			this.Name = _name;
			this.Position = _position;
			this.Rotation = _rotation;
			this.Arrows = _arrows;
		}

		// Token: 0x04003D65 RID: 15717
		public readonly string Name;

		// Token: 0x04003D66 RID: 15718
		public readonly Vector3 Position;

		// Token: 0x04003D67 RID: 15719
		public readonly Vector3 Rotation;

		// Token: 0x04003D68 RID: 15720
		public readonly char[] Arrows;
	}
}
