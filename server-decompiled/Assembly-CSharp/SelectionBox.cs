using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A6D RID: 2669
public class SelectionBox : MonoBehaviour
{
	// Token: 0x170008C2 RID: 2242
	// (get) Token: 0x0600502E RID: 20526 RVA: 0x001E9FC0 File Offset: 0x001E81C0
	public SelectionCategory Category
	{
		get
		{
			return this.ownerCategory;
		}
	}

	// Token: 0x170008C3 RID: 2243
	// (get) Token: 0x0600502F RID: 20527 RVA: 0x001E9FC8 File Offset: 0x001E81C8
	// (set) Token: 0x06005030 RID: 20528 RVA: 0x001E9FD0 File Offset: 0x001E81D0
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

	// Token: 0x170008C4 RID: 2244
	// (get) Token: 0x06005031 RID: 20529 RVA: 0x001E9FEF File Offset: 0x001E81EF
	// (set) Token: 0x06005032 RID: 20530 RVA: 0x001E9FF7 File Offset: 0x001E81F7
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

	// Token: 0x170008C5 RID: 2245
	// (get) Token: 0x06005033 RID: 20531 RVA: 0x001EA016 File Offset: 0x001E8216
	public Vector3i Size
	{
		get
		{
			return this.size;
		}
	}

	// Token: 0x170008C6 RID: 2246
	// (get) Token: 0x06005034 RID: 20532 RVA: 0x001EA01E File Offset: 0x001E821E
	// (set) Token: 0x06005035 RID: 20533 RVA: 0x001EA026 File Offset: 0x001E8226
	public Vector3i Position
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008C7 RID: 2247
	// (get) Token: 0x06005036 RID: 20534 RVA: 0x001EA02F File Offset: 0x001E822F
	// (set) Token: 0x06005037 RID: 20535 RVA: 0x001EA037 File Offset: 0x001E8237
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

	// Token: 0x170008C8 RID: 2248
	// (get) Token: 0x06005038 RID: 20536 RVA: 0x001EA04F File Offset: 0x001E824F
	public bool IsActive
	{
		get
		{
			return SelectionBoxManager.Instance.Selection == this;
		}
	}

	// Token: 0x06005039 RID: 20537 RVA: 0x001EA064 File Offset: 0x001E8264
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

	// Token: 0x0600503A RID: 20538 RVA: 0x001EA13A File Offset: 0x001E833A
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.camPostRender));
	}

	// Token: 0x0600503B RID: 20539 RVA: 0x001EA15C File Offset: 0x001E835C
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

	// Token: 0x0600503C RID: 20540 RVA: 0x001EA1B8 File Offset: 0x001E83B8
	public void SetOwner(SelectionCategory _selectionCategory)
	{
		this.ownerCategory = _selectionCategory;
	}

	// Token: 0x0600503D RID: 20541 RVA: 0x001EA1C4 File Offset: 0x001E83C4
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

	// Token: 0x0600503E RID: 20542 RVA: 0x001EA224 File Offset: 0x001E8424
	public void ResetAllFacesColor()
	{
		this.SetAllFacesColor(this.curColor, false);
	}

	// Token: 0x0600503F RID: 20543 RVA: 0x001EA233 File Offset: 0x001E8433
	public void SetFaceColor(BlockFace _face, Color _c)
	{
		this.m_MeshRenderer.materials[(int)_face].color = _c;
		this.faceColorsSet[(int)_face] = true;
	}

	// Token: 0x06005040 RID: 20544 RVA: 0x001EA254 File Offset: 0x001E8454
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

	// Token: 0x06005041 RID: 20545 RVA: 0x001EA351 File Offset: 0x001E8551
	public void SetCaptionVisibility(bool _visible)
	{
		if (this.captionMesh == null)
		{
			return;
		}
		this.captionMesh.gameObject.SetActive(_visible);
	}

	// Token: 0x06005042 RID: 20546 RVA: 0x001EA374 File Offset: 0x001E8574
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

	// Token: 0x06005043 RID: 20547 RVA: 0x001EA499 File Offset: 0x001E8699
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

	// Token: 0x06005044 RID: 20548 RVA: 0x001EA4C6 File Offset: 0x001E86C6
	public Vector3i GetScale()
	{
		return new Vector3i(this.bounds.size);
	}

	// Token: 0x06005045 RID: 20549 RVA: 0x001EA4D8 File Offset: 0x001E86D8
	public Transform GetBoxTransform()
	{
		return base.transform.Find("Box");
	}

	// Token: 0x06005046 RID: 20550 RVA: 0x001EA4EA File Offset: 0x001E86EA
	public void SetFrameActive(bool _active)
	{
		this.SetFrameColor(_active ? SelectionBox.activeFrameColor : SelectionBox.inActiveFrameColor);
	}

	// Token: 0x06005047 RID: 20551 RVA: 0x001EA501 File Offset: 0x001E8701
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFrameColor(Color _color)
	{
		this.curFrameColor = _color;
		if (this.frame != null)
		{
			this.frame.GetComponent<MeshRenderer>().material.SetColor(SelectionBox.colorShaderProperty, _color);
		}
	}

	// Token: 0x06005048 RID: 20552 RVA: 0x001EA534 File Offset: 0x001E8734
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

	// Token: 0x06005049 RID: 20553 RVA: 0x001EA5AB File Offset: 0x001E87AB
	public void EnableCollider(string _tag, int _layer)
	{
		this.collLayer = _layer;
		this.collTag = _tag;
	}

	// Token: 0x0600504A RID: 20554 RVA: 0x001EA5BB File Offset: 0x001E87BB
	public void HighlightAxis(Vector3i _hightlightedAxis)
	{
		this.hightlightedAxis = _hightlightedAxis;
	}

	// Token: 0x0600504B RID: 20555 RVA: 0x001EA5C4 File Offset: 0x001E87C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (!this.bCreated && this.createMesh())
		{
			this.bCreated = true;
		}
	}

	// Token: 0x0600504C RID: 20556 RVA: 0x001EA5E0 File Offset: 0x001E87E0
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

	// Token: 0x0600504D RID: 20557 RVA: 0x001EA72C File Offset: 0x001E892C
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

	// Token: 0x0600504E RID: 20558 RVA: 0x001EA804 File Offset: 0x001E8A04
	public bool WorldPosWithinBox(Vector3i _pos)
	{
		Vector3i position = this.Position;
		Vector3i vector3i = position + this.size;
		return _pos.x >= position.x && _pos.x < vector3i.x && _pos.y >= position.y && _pos.y < vector3i.y && _pos.z >= position.z && _pos.z < vector3i.z;
	}

	// Token: 0x0600504F RID: 20559 RVA: 0x001EA87C File Offset: 0x001E8A7C
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

	// Token: 0x06005050 RID: 20560 RVA: 0x001EB65C File Offset: 0x001E985C
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

	// Token: 0x06005051 RID: 20561 RVA: 0x001ECD44 File Offset: 0x001EAF44
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

	// Token: 0x06005052 RID: 20562 RVA: 0x001ECE44 File Offset: 0x001EB044
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

	// Token: 0x06005053 RID: 20563 RVA: 0x001ECEEC File Offset: 0x001EB0EC
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

	// Token: 0x06005054 RID: 20564 RVA: 0x001ED71C File Offset: 0x001EB91C
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

	// Token: 0x04003DDE RID: 15838
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float boundsPadding = 0.16f;

	// Token: 0x04003DDF RID: 15839
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int maxFacingDirectionDistance = 62500;

	// Token: 0x04003DE0 RID: 15840
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly int zTestShaderProperty = Shader.PropertyToID("_ZTest");

	// Token: 0x04003DE1 RID: 15841
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly int colorShaderProperty = Shader.PropertyToID("_Color");

	// Token: 0x04003DE2 RID: 15842
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SelectionCategory ownerCategory;

	// Token: 0x04003DE3 RID: 15843
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string boxName;

	// Token: 0x04003DE4 RID: 15844
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public object userData;

	// Token: 0x04003DE5 RID: 15845
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject frame;

	// Token: 0x04003DE6 RID: 15846
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextMesh captionMesh;

	// Token: 0x04003DE7 RID: 15847
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextMesh[] sizeMeshes;

	// Token: 0x04003DE8 RID: 15848
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i size = Vector3i.zero;

	// Token: 0x04003DEA RID: 15850
	public Bounds bounds = BoundsUtils.BoundsForMinMax(Vector3.zero, Vector3.one);

	// Token: 0x04003DEB RID: 15851
	public RenderCubeType focusType = RenderCubeType.FullBlockBothSides;

	// Token: 0x04003DEC RID: 15852
	public bool bAlwaysDrawDirection;

	// Token: 0x04003DED RID: 15853
	public bool bDrawDirection;

	// Token: 0x04003DEE RID: 15854
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float facingDirection;

	// Token: 0x04003DEF RID: 15855
	public Vector3 AxisOrigin;

	// Token: 0x04003DF0 RID: 15856
	public readonly List<Vector3> Axises = new List<Vector3>();

	// Token: 0x04003DF1 RID: 15857
	public readonly List<Vector3i> AxisesI = new List<Vector3i>();

	// Token: 0x04003DF2 RID: 15858
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i hightlightedAxis = Vector3i.zero;

	// Token: 0x04003DF3 RID: 15859
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter m_MeshFilter;

	// Token: 0x04003DF4 RID: 15860
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer m_MeshRenderer;

	// Token: 0x04003DF5 RID: 15861
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<int>[] subMeshTriangles = new List<int>[6];

	// Token: 0x04003DF6 RID: 15862
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<Vector2> m_Uvs = new List<Vector2>();

	// Token: 0x04003DF7 RID: 15863
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<Vector3> m_Vertices = new List<Vector3>();

	// Token: 0x04003DF8 RID: 15864
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Material[] materialsArr = new Material[6];

	// Token: 0x04003DF9 RID: 15865
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int collLayer;

	// Token: 0x04003DFA RID: 15866
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string collTag;

	// Token: 0x04003DFB RID: 15867
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentChunkMeshIndex;

	// Token: 0x04003DFC RID: 15868
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bCreated;

	// Token: 0x04003DFD RID: 15869
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly bool[] faceColorsSet = new bool[6];

	// Token: 0x04003DFE RID: 15870
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color curColor;

	// Token: 0x04003DFF RID: 15871
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color curFrameColor = SelectionBox.inActiveFrameColor;

	// Token: 0x04003E00 RID: 15872
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool curShowingThroughWalls;

	// Token: 0x04003E01 RID: 15873
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Color inActiveFrameColor = Color.blue;

	// Token: 0x04003E02 RID: 15874
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Color activeFrameColor = Color.green;

	// Token: 0x04003E03 RID: 15875
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

	// Token: 0x02000A6E RID: 2670
	[PublicizedFrom(EAccessModifier.Private)]
	public class SizeTextMeshDefinition
	{
		// Token: 0x06005057 RID: 20567 RVA: 0x001EDA62 File Offset: 0x001EBC62
		public SizeTextMeshDefinition(string _name, Vector3 _position, Vector3 _rotation, char[] _arrows)
		{
			this.Name = _name;
			this.Position = _position;
			this.Rotation = _rotation;
			this.Arrows = _arrows;
		}

		// Token: 0x04003E04 RID: 15876
		public readonly string Name;

		// Token: 0x04003E05 RID: 15877
		public readonly Vector3 Position;

		// Token: 0x04003E06 RID: 15878
		public readonly Vector3 Rotation;

		// Token: 0x04003E07 RID: 15879
		public readonly char[] Arrows;
	}
}
