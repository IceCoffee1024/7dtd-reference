using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A77 RID: 2679
public class SelectionBox : MonoBehaviour
{
	// Token: 0x170008C8 RID: 2248
	// (get) Token: 0x06005052 RID: 20562 RVA: 0x001EB09C File Offset: 0x001E929C
	public SelectionCategory Category
	{
		get
		{
			return this.ownerCategory;
		}
	}

	// Token: 0x170008C9 RID: 2249
	// (get) Token: 0x06005053 RID: 20563 RVA: 0x001EB0A4 File Offset: 0x001E92A4
	// (set) Token: 0x06005054 RID: 20564 RVA: 0x001EB0AC File Offset: 0x001E92AC
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

	// Token: 0x170008CA RID: 2250
	// (get) Token: 0x06005055 RID: 20565 RVA: 0x001EB0CB File Offset: 0x001E92CB
	// (set) Token: 0x06005056 RID: 20566 RVA: 0x001EB0D3 File Offset: 0x001E92D3
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

	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x06005057 RID: 20567 RVA: 0x001EB0F2 File Offset: 0x001E92F2
	public Vector3i Size
	{
		get
		{
			return this.size;
		}
	}

	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x06005058 RID: 20568 RVA: 0x001EB0FA File Offset: 0x001E92FA
	// (set) Token: 0x06005059 RID: 20569 RVA: 0x001EB102 File Offset: 0x001E9302
	public Vector3i Position
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x0600505A RID: 20570 RVA: 0x001EB10B File Offset: 0x001E930B
	// (set) Token: 0x0600505B RID: 20571 RVA: 0x001EB113 File Offset: 0x001E9313
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

	// Token: 0x170008CE RID: 2254
	// (get) Token: 0x0600505C RID: 20572 RVA: 0x001EB12B File Offset: 0x001E932B
	public bool IsActive
	{
		get
		{
			return SelectionBoxManager.Instance.Selection == this;
		}
	}

	// Token: 0x0600505D RID: 20573 RVA: 0x001EB140 File Offset: 0x001E9340
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

	// Token: 0x0600505E RID: 20574 RVA: 0x001EB216 File Offset: 0x001E9416
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.camPostRender));
	}

	// Token: 0x0600505F RID: 20575 RVA: 0x001EB238 File Offset: 0x001E9438
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

	// Token: 0x06005060 RID: 20576 RVA: 0x001EB294 File Offset: 0x001E9494
	public void SetOwner(SelectionCategory _selectionCategory)
	{
		this.ownerCategory = _selectionCategory;
	}

	// Token: 0x06005061 RID: 20577 RVA: 0x001EB2A0 File Offset: 0x001E94A0
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

	// Token: 0x06005062 RID: 20578 RVA: 0x001EB300 File Offset: 0x001E9500
	public void ResetAllFacesColor()
	{
		this.SetAllFacesColor(this.curColor, false);
	}

	// Token: 0x06005063 RID: 20579 RVA: 0x001EB30F File Offset: 0x001E950F
	public void SetFaceColor(BlockFace _face, Color _c)
	{
		this.m_MeshRenderer.materials[(int)_face].color = _c;
		this.faceColorsSet[(int)_face] = true;
	}

	// Token: 0x06005064 RID: 20580 RVA: 0x001EB330 File Offset: 0x001E9530
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

	// Token: 0x06005065 RID: 20581 RVA: 0x001EB42D File Offset: 0x001E962D
	public void SetCaptionVisibility(bool _visible)
	{
		if (this.captionMesh == null)
		{
			return;
		}
		this.captionMesh.gameObject.SetActive(_visible);
	}

	// Token: 0x06005066 RID: 20582 RVA: 0x001EB450 File Offset: 0x001E9650
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

	// Token: 0x06005067 RID: 20583 RVA: 0x001EB575 File Offset: 0x001E9775
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

	// Token: 0x06005068 RID: 20584 RVA: 0x001EB5A2 File Offset: 0x001E97A2
	public Vector3i GetScale()
	{
		return new Vector3i(this.bounds.size);
	}

	// Token: 0x06005069 RID: 20585 RVA: 0x001EB5B4 File Offset: 0x001E97B4
	public Transform GetBoxTransform()
	{
		return base.transform.Find("Box");
	}

	// Token: 0x0600506A RID: 20586 RVA: 0x001EB5C6 File Offset: 0x001E97C6
	public void SetFrameActive(bool _active)
	{
		this.SetFrameColor(_active ? SelectionBox.activeFrameColor : SelectionBox.inActiveFrameColor);
	}

	// Token: 0x0600506B RID: 20587 RVA: 0x001EB5DD File Offset: 0x001E97DD
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFrameColor(Color _color)
	{
		this.curFrameColor = _color;
		if (this.frame != null)
		{
			this.frame.GetComponent<MeshRenderer>().material.SetColor(SelectionBox.colorShaderProperty, _color);
		}
	}

	// Token: 0x0600506C RID: 20588 RVA: 0x001EB610 File Offset: 0x001E9810
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

	// Token: 0x0600506D RID: 20589 RVA: 0x001EB687 File Offset: 0x001E9887
	public void EnableCollider(string _tag, int _layer)
	{
		this.collLayer = _layer;
		this.collTag = _tag;
	}

	// Token: 0x0600506E RID: 20590 RVA: 0x001EB697 File Offset: 0x001E9897
	public void HighlightAxis(Vector3i _hightlightedAxis)
	{
		this.hightlightedAxis = _hightlightedAxis;
	}

	// Token: 0x0600506F RID: 20591 RVA: 0x001EB6A0 File Offset: 0x001E98A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (!this.bCreated && this.createMesh())
		{
			this.bCreated = true;
		}
	}

	// Token: 0x06005070 RID: 20592 RVA: 0x001EB6BC File Offset: 0x001E98BC
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

	// Token: 0x06005071 RID: 20593 RVA: 0x001EB808 File Offset: 0x001E9A08
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

	// Token: 0x06005072 RID: 20594 RVA: 0x001EB8E0 File Offset: 0x001E9AE0
	public bool WorldPosWithinBox(Vector3i _pos)
	{
		Vector3i position = this.Position;
		Vector3i vector3i = position + this.size;
		return _pos.x >= position.x && _pos.x < vector3i.x && _pos.y >= position.y && _pos.y < vector3i.y && _pos.z >= position.z && _pos.z < vector3i.z;
	}

	// Token: 0x06005073 RID: 20595 RVA: 0x001EB958 File Offset: 0x001E9B58
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

	// Token: 0x06005074 RID: 20596 RVA: 0x001EC738 File Offset: 0x001EA938
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

	// Token: 0x06005075 RID: 20597 RVA: 0x001EDE20 File Offset: 0x001EC020
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

	// Token: 0x06005076 RID: 20598 RVA: 0x001EDF20 File Offset: 0x001EC120
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

	// Token: 0x06005077 RID: 20599 RVA: 0x001EDFC8 File Offset: 0x001EC1C8
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

	// Token: 0x06005078 RID: 20600 RVA: 0x001EE7F8 File Offset: 0x001EC9F8
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

	// Token: 0x04003DFF RID: 15871
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float boundsPadding = 0.16f;

	// Token: 0x04003E00 RID: 15872
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int maxFacingDirectionDistance = 62500;

	// Token: 0x04003E01 RID: 15873
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly int zTestShaderProperty = Shader.PropertyToID("_ZTest");

	// Token: 0x04003E02 RID: 15874
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly int colorShaderProperty = Shader.PropertyToID("_Color");

	// Token: 0x04003E03 RID: 15875
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SelectionCategory ownerCategory;

	// Token: 0x04003E04 RID: 15876
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string boxName;

	// Token: 0x04003E05 RID: 15877
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public object userData;

	// Token: 0x04003E06 RID: 15878
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject frame;

	// Token: 0x04003E07 RID: 15879
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextMesh captionMesh;

	// Token: 0x04003E08 RID: 15880
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextMesh[] sizeMeshes;

	// Token: 0x04003E09 RID: 15881
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i size = Vector3i.zero;

	// Token: 0x04003E0B RID: 15883
	public Bounds bounds = BoundsUtils.BoundsForMinMax(Vector3.zero, Vector3.one);

	// Token: 0x04003E0C RID: 15884
	public RenderCubeType focusType = RenderCubeType.FullBlockBothSides;

	// Token: 0x04003E0D RID: 15885
	public bool bAlwaysDrawDirection;

	// Token: 0x04003E0E RID: 15886
	public bool bDrawDirection;

	// Token: 0x04003E0F RID: 15887
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float facingDirection;

	// Token: 0x04003E10 RID: 15888
	public Vector3 AxisOrigin;

	// Token: 0x04003E11 RID: 15889
	public readonly List<Vector3> Axises = new List<Vector3>();

	// Token: 0x04003E12 RID: 15890
	public readonly List<Vector3i> AxisesI = new List<Vector3i>();

	// Token: 0x04003E13 RID: 15891
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i hightlightedAxis = Vector3i.zero;

	// Token: 0x04003E14 RID: 15892
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter m_MeshFilter;

	// Token: 0x04003E15 RID: 15893
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer m_MeshRenderer;

	// Token: 0x04003E16 RID: 15894
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<int>[] subMeshTriangles = new List<int>[6];

	// Token: 0x04003E17 RID: 15895
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<Vector2> m_Uvs = new List<Vector2>();

	// Token: 0x04003E18 RID: 15896
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<Vector3> m_Vertices = new List<Vector3>();

	// Token: 0x04003E19 RID: 15897
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Material[] materialsArr = new Material[6];

	// Token: 0x04003E1A RID: 15898
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int collLayer;

	// Token: 0x04003E1B RID: 15899
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string collTag;

	// Token: 0x04003E1C RID: 15900
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentChunkMeshIndex;

	// Token: 0x04003E1D RID: 15901
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bCreated;

	// Token: 0x04003E1E RID: 15902
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly bool[] faceColorsSet = new bool[6];

	// Token: 0x04003E1F RID: 15903
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color curColor;

	// Token: 0x04003E20 RID: 15904
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color curFrameColor = SelectionBox.inActiveFrameColor;

	// Token: 0x04003E21 RID: 15905
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool curShowingThroughWalls;

	// Token: 0x04003E22 RID: 15906
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Color inActiveFrameColor = Color.blue;

	// Token: 0x04003E23 RID: 15907
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Color activeFrameColor = Color.green;

	// Token: 0x04003E24 RID: 15908
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

	// Token: 0x02000A78 RID: 2680
	[PublicizedFrom(EAccessModifier.Private)]
	public class SizeTextMeshDefinition
	{
		// Token: 0x0600507B RID: 20603 RVA: 0x001EEB3E File Offset: 0x001ECD3E
		public SizeTextMeshDefinition(string _name, Vector3 _position, Vector3 _rotation, char[] _arrows)
		{
			this.Name = _name;
			this.Position = _position;
			this.Rotation = _rotation;
			this.Arrows = _arrows;
		}

		// Token: 0x04003E25 RID: 15909
		public readonly string Name;

		// Token: 0x04003E26 RID: 15910
		public readonly Vector3 Position;

		// Token: 0x04003E27 RID: 15911
		public readonly Vector3 Rotation;

		// Token: 0x04003E28 RID: 15912
		public readonly char[] Arrows;
	}
}
