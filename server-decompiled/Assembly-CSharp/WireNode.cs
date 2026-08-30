using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200137C RID: 4988
public class WireNode : MonoBehaviour, IWireNode
{
	// Token: 0x06009D65 RID: 40293 RVA: 0x003B71C4 File Offset: 0x003B53C4
	public void Init()
	{
		if (WireNode.rootShockPrefab == null)
		{
			WireNode.rootShockPrefab = Resources.Load<GameObject>("Prefabs/ElectricShock");
		}
		this.prevParent = (this.parent = base.transform.parent);
		if (this.parent != null)
		{
			this.prevParentGO = (this.parentGO = this.parent.gameObject);
			this.usingLocalPosition = false;
		}
		this.shockSpots = new List<WireNode.ShockSpot>();
		base.transform.parent = null;
		base.transform.position = Vector3.zero;
		base.transform.localEulerAngles = Vector3.zero;
		base.transform.localScale = Vector3.one;
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.meshCollider = base.GetComponent<MeshCollider>();
		this.meshCollider.convex = true;
		this.meshCollider.isTrigger = true;
		this.mesh = new Mesh();
		this.mesh.MarkDynamic();
		this.shockNodes = new List<bool>();
		this.points = new List<Vector3>();
		this.forces = new List<Vector3>();
		this.verts = new List<Vector3>();
		this.uvs = new List<Vector2>();
		this.uvs2 = new List<Vector2>();
		this.normals = new List<Vector3>();
		this.indices = new int[24 * (1 + this.numNodes) + 12];
		if (this.personalMaterial == null)
		{
			this.personalMaterial = global::UnityEngine.Object.Instantiate<Material>(this.material);
		}
		this.meshRenderer.material = this.personalMaterial;
	}

	// Token: 0x06009D66 RID: 40294 RVA: 0x003B7368 File Offset: 0x003B5568
	public void OnDestroy()
	{
		if (this.personalMaterial != null)
		{
			global::UnityEngine.Object.Destroy(this.personalMaterial);
			this.personalMaterial = null;
		}
		for (int i = 0; i < this.shockSpots.Count; i++)
		{
			global::UnityEngine.Object.Destroy(this.shockSpots[i].shockPrefab);
		}
	}

	// Token: 0x06009D67 RID: 40295 RVA: 0x003B73C1 File Offset: 0x003B55C1
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.Init();
	}

	// Token: 0x06009D68 RID: 40296 RVA: 0x003B73C9 File Offset: 0x003B55C9
	public void ToggleMeshCollider(bool _bOn)
	{
		this.meshCollider.enabled = _bOn;
	}

	// Token: 0x06009D69 RID: 40297 RVA: 0x003B73D7 File Offset: 0x003B55D7
	public void SetPulseSpeed(float speed)
	{
		if (this.personalMaterial == null)
		{
			return;
		}
		this.personalMaterial.SetFloat("_PulseSpeed", speed);
	}

	// Token: 0x06009D6A RID: 40298 RVA: 0x003B73F9 File Offset: 0x003B55F9
	public void TogglePulse(bool isOn)
	{
		if (this.personalMaterial == null)
		{
			return;
		}
		this.personalMaterial.SetColor("_PulseColor", isOn ? this.pulseColor : Color.black);
	}

	// Token: 0x06009D6B RID: 40299 RVA: 0x003B742A File Offset: 0x003B562A
	public void SetPulseColor(Color color)
	{
		this.pulseColor = color;
	}

	// Token: 0x06009D6C RID: 40300 RVA: 0x003B7434 File Offset: 0x003B5634
	public void PlayShockAtPosition(Vector3 shockPosition)
	{
		if (shockPosition.x < this.min.x - 1f)
		{
			return;
		}
		if (shockPosition.y < this.min.y - 1f)
		{
			return;
		}
		if (shockPosition.z < this.min.z - 1f)
		{
			return;
		}
		if (shockPosition.x > this.max.x + 1f)
		{
			return;
		}
		if (shockPosition.y > this.max.y + 1f)
		{
			return;
		}
		if (shockPosition.z > this.max.z + 1f)
		{
			return;
		}
		int num = 99999;
		float num2 = 99999f;
		for (int i = 1; i < this.points.Count - 1; i++)
		{
			Vector3 vector = this.points[i] - shockPosition;
			if (vector.magnitude < num2)
			{
				num2 = vector.magnitude;
				num = i;
			}
		}
		if (num2 > 1f)
		{
			return;
		}
		if (num < this.shockNodes.Count && this.shockNodes[num])
		{
			return;
		}
		WireNode.ShockSpot shockSpot;
		shockSpot.timer = Time.time;
		shockSpot.vertex = num;
		shockSpot.shockDataIndex = 0;
		shockSpot.shockPrefab = global::UnityEngine.Object.Instantiate<GameObject>(WireNode.rootShockPrefab);
		shockSpot.shockPrefab.transform.position = this.points[num];
		shockSpot.shockPrefab.transform.parent = base.transform;
		this.shockSpots.Add(shockSpot);
	}

	// Token: 0x06009D6D RID: 40301 RVA: 0x003B75BC File Offset: 0x003B57BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdatePlayerIntersection()
	{
		if (GameManager.Instance == null)
		{
			return;
		}
		if (GameManager.Instance.World == null)
		{
			return;
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer == null)
		{
			return;
		}
		if (Time.time > this.playerShockTimer + 0.25f)
		{
			this.playerShockTimer = Time.time;
			this.PlayShockAtPosition(primaryPlayer.GetPosition());
		}
	}

	// Token: 0x06009D6E RID: 40302 RVA: 0x003B7628 File Offset: 0x003B5828
	[PublicizedFrom(EAccessModifier.Private)]
	public void RayCastHeightAt(ref Vector3 position)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(position + Vector3.up - Origin.position, Vector3.down), out raycastHit, 1.75f, 65537) && raycastHit.point.y > position.y)
		{
			position.y = raycastHit.point.y + 0.01f - Origin.position.y;
		}
	}

	// Token: 0x06009D6F RID: 40303 RVA: 0x003B76A4 File Offset: 0x003B58A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateForces()
	{
		if (this.points.Count < 3)
		{
			return;
		}
		float magnitude = (this.points[this.points.Count - 1] - this.points[0]).magnitude;
		if (magnitude == 0f)
		{
			return;
		}
		float num = magnitude / 10000f;
		num = Mathf.Clamp(num, 1E-05f, 10f);
		float num2 = ((this.weightMod > 0f) ? (num * this.weightMod) : num);
		Vector3 vector = Vector3.down * 9.81f * num2;
		float num3 = 0.35f * ((this.weightMod > 0f) ? (1f - this.weightMod * num) : (1f - num)) * this.springMod;
		num3 = Mathf.Clamp(num3, 0.001f, 0.4999f);
		for (int i = 1; i < this.points.Count - 1; i++)
		{
			Vector3 vector2 = this.points[i - 1] - this.points[i];
			vector2 *= this.tensionMultiplier;
			vector2 *= Mathf.Clamp01(vector2.magnitude - this.minSegmentLength);
			Vector3 vector3 = this.points[i + 1] - this.points[i];
			vector3 *= this.tensionMultiplier;
			vector3 *= Mathf.Clamp01(vector3.magnitude - this.minSegmentLength);
			Vector3 vector4 = vector2 + vector3;
			float num4 = ((i >= this.points.Count - 4) ? Mathf.Clamp01(1.1f - (float)(i - (this.points.Count - 5)) / 3f) : 1f);
			float num5 = num3 * num4;
			Vector3 vector5 = vector * (1f + (1f - num4));
			this.forces[i] = this.forces[i] * this.drag + (vector4 + vector5) * num5;
		}
		this.currentTotalForces = 0f;
		for (int j = 1; j < this.points.Count - 1; j++)
		{
			RaycastHit raycastHit;
			if (this.forces[j].magnitude > 0f && this.forces[j].magnitude < this.snagThreshold && Physics.Raycast(new Ray(this.points[j] - Origin.position, this.forces[j].normalized), out raycastHit, this.forces[j].magnitude, 65537) && raycastHit.distance < this.forces[j].magnitude)
			{
				this.forces[j] = Vector3.zero;
			}
			List<Vector3> list = this.points;
			int num6 = j;
			list[num6] += this.forces[j];
			this.currentTotalForces += this.forces[j].magnitude;
		}
	}

	// Token: 0x06009D70 RID: 40304 RVA: 0x003B7A30 File Offset: 0x003B5C30
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreatePoints()
	{
		Vector3 vector = this.currSourcePosition - this.currLocalPosition;
		if (vector.magnitude == 0f)
		{
			return;
		}
		Vector3 vector2 = vector / (float)(1 + this.numNodes);
		if (vector2.magnitude == 0f)
		{
			return;
		}
		Vector3 vector3 = this.currLocalPosition + vector2;
		if (this.points.Count == 0)
		{
			Dictionary<Vector3, bool> dictionary = new Dictionary<Vector3, bool>(Vector3EqualityComparer.Instance);
			dictionary.Add(this.currLocalPosition, true);
			this.points.Add(this.currLocalPosition);
			this.forces.Add(Vector3.zero);
			this.shockNodes.Add(false);
			for (int i = 0; i < this.numNodes; i++)
			{
				this.forces.Add(Vector3.zero);
				this.shockNodes.Add(false);
				if (dictionary.ContainsKey(vector3))
				{
					this.points.Clear();
					this.forces.Clear();
					this.shockNodes.Clear();
					return;
				}
				this.points.Add(vector3);
				dictionary.Add(vector3, true);
				vector3 += vector2;
			}
		}
		if (this.points.Count > 1)
		{
			this.points[0] = this.currLocalPosition;
			this.points[this.points.Count - 1] = this.currSourcePosition;
		}
	}

	// Token: 0x06009D71 RID: 40305 RVA: 0x003B7B98 File Offset: 0x003B5D98
	public void BuildMesh()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		this.usingSourcePosition = this.Source == null;
		if (!this.usingLocalPosition && this.parentGO == null)
		{
			this.usingLocalPosition = true;
			return;
		}
		Vector3 vector = this.currLocalPosition;
		Vector3 vector2 = this.currSourcePosition;
		if (this.prevWireColor != this.wireColor)
		{
			this.prevWireColor = this.wireColor;
			this.personalMaterial.SetColor("_WireColor", this.wireColor);
		}
		for (int i = this.shockSpots.Count - 1; i >= 0; i--)
		{
			if (Time.time > this.shockSpots[i].timer + this.shocks[this.shockSpots[i].shockDataIndex].x)
			{
				WireNode.ShockSpot shockSpot = this.shockSpots[i];
				shockSpot.timer = Time.time;
				shockSpot.shockDataIndex++;
				if (shockSpot.shockDataIndex >= this.shocks.Length)
				{
					global::UnityEngine.Object.Destroy(shockSpot.shockPrefab);
					this.shockSpots.RemoveAt(i);
					this.shockNodes[i] = false;
				}
				else
				{
					this.shockSpots[i] = shockSpot;
				}
			}
		}
		this.currLocalPosition = this.localOffset + (this.usingLocalPosition ? this.LocalPosition : this.parent.position);
		Vector3 vector3 = this.sourceOffset;
		if (!this.usingSourcePosition)
		{
			vector3 = this.sourceOffset.x * Camera.main.transform.right;
			vector3 += this.sourceOffset.y * Camera.main.transform.up;
			vector3 += this.sourceOffset.z * Camera.main.transform.forward;
		}
		this.currSourcePosition = vector3 + (this.usingSourcePosition ? this.SourcePosition : this.cachedSourcePosition);
		if (this.currSourcePosition == this.currLocalPosition)
		{
			return;
		}
		this.prevLocalPosition = this.currLocalPosition;
		this.prevSourcePosition = this.currSourcePosition;
		this.prevWeightMod = this.weightMod;
		this.prevSpringMod = this.springMod;
		this.prevSize = this.size;
		this.mesh.Clear(false);
		this.verts.Clear();
		this.uvs.Clear();
		this.uvs2.Clear();
		this.normals.Clear();
		this.CreatePoints();
		this.UpdateForces();
		this.uvs.Add(Vector2.zero);
		this.uvs.Add(Vector2.zero);
		this.uvs.Add(Vector2.zero);
		this.uvs.Add(Vector2.zero);
		this.uvs2.Add(Vector2.zero);
		this.uvs2.Add(Vector2.zero);
		this.uvs2.Add(Vector2.zero);
		this.uvs2.Add(Vector2.zero);
		for (int j = 1; j < this.points.Count; j++)
		{
			this.uvs.Add(Vector2.right * (float)j / (float)this.points.Count);
			this.uvs.Add(Vector2.right * (float)j / (float)this.points.Count + Vector2.up);
			this.uvs.Add(Vector2.right * (float)j / (float)this.points.Count);
			this.uvs.Add(Vector2.right * (float)j / (float)this.points.Count + Vector2.up);
			bool flag = false;
			for (int k = 0; k < this.shockSpots.Count; k++)
			{
				if (this.shockSpots[k].vertex == j)
				{
					this.uvs2.Add(Vector2.right * this.shocks[this.shockSpots[k].shockDataIndex].y);
					this.uvs2.Add(Vector2.right * this.shocks[this.shockSpots[k].shockDataIndex].y);
					this.uvs2.Add(Vector2.right * this.shocks[this.shockSpots[k].shockDataIndex].y);
					this.uvs2.Add(Vector2.right * this.shocks[this.shockSpots[k].shockDataIndex].y);
					flag = true;
					this.shockNodes[j] = true;
					break;
				}
			}
			if (!flag)
			{
				this.uvs2.Add(Vector2.zero);
				this.uvs2.Add(Vector2.zero);
				this.uvs2.Add(Vector2.zero);
				this.uvs2.Add(Vector2.zero);
			}
		}
		Vector3 vector4 = vector2 - vector;
		this.min = Vector3.one * float.PositiveInfinity;
		this.max = -Vector3.one * float.PositiveInfinity;
		for (int l = 0; l < this.points.Count; l++)
		{
			if (l > 0)
			{
				vector4 = this.points[l] - this.points[l - 1];
			}
			Vector3 vector5 = Vector3.Cross(Vector3.up, vector4.normalized).normalized;
			Vector3 vector6 = Vector3.Cross(vector4.normalized, vector5).normalized;
			this.verts.Add(vector6 * this.size + this.points[l]);
			this.verts.Add(-vector6 * this.size + this.points[l]);
			this.verts.Add(vector5 * this.size + this.points[l]);
			this.verts.Add(-vector5 * this.size + this.points[l]);
			if (l == 0)
			{
				vector6 = Vector3.Lerp(vector6, (vector2 - vector).normalized, 0.5f).normalized;
				vector5 = Vector3.Lerp(vector5, (vector2 - vector).normalized, 0.5f).normalized;
			}
			else if (l == this.points.Count - 1)
			{
				vector6 = Vector3.Lerp(vector6, -(vector2 - vector).normalized, 0.5f).normalized;
				vector5 = Vector3.Lerp(vector5, -(vector2 - vector).normalized, 0.5f).normalized;
			}
			this.normals.Add(vector6);
			this.normals.Add(-vector6);
			this.normals.Add(vector5);
			this.normals.Add(-vector5);
			if (this.points[l].x < this.min.x)
			{
				this.min.x = this.points[l].x;
			}
			if (this.points[l].x > this.max.x)
			{
				this.max.x = this.points[l].x;
			}
			if (this.points[l].y < this.min.y)
			{
				this.min.y = this.points[l].y;
			}
			if (this.points[l].y > this.max.y)
			{
				this.max.y = this.points[l].y;
			}
			if (this.points[l].z < this.min.z)
			{
				this.min.z = this.points[l].z;
			}
			if (this.points[l].z > this.max.z)
			{
				this.max.z = this.points[l].z;
			}
		}
		if (((this.min.x == this.max.x) ? 1 : ((0f + this.min.y == this.max.y) ? 1 : ((0f + this.min.z == this.max.z) ? 1 : 0))) >= 2)
		{
			return;
		}
		int num = 0;
		for (int m = 0; m < this.numNodes; m++)
		{
			this.indices[num++] = 4 * m;
			this.indices[num++] = 4 + 4 * m;
			this.indices[num++] = 7 + 4 * m;
			this.indices[num++] = 7 + 4 * m;
			this.indices[num++] = 3 + 4 * m;
			this.indices[num++] = 4 * m;
			this.indices[num++] = 4 + 4 * m;
			this.indices[num++] = 4 * m;
			this.indices[num++] = 2 + 4 * m;
			this.indices[num++] = 2 + 4 * m;
			this.indices[num++] = 6 + 4 * m;
			this.indices[num++] = 4 + 4 * m;
			this.indices[num++] = 3 + 4 * m;
			this.indices[num++] = 7 + 4 * m;
			this.indices[num++] = 5 + 4 * m;
			this.indices[num++] = 5 + 4 * m;
			this.indices[num++] = 1 + 4 * m;
			this.indices[num++] = 3 + 4 * m;
			this.indices[num++] = 6 + 4 * m;
			this.indices[num++] = 2 + 4 * m;
			this.indices[num++] = 1 + 4 * m;
			this.indices[num++] = 1 + 4 * m;
			this.indices[num++] = 5 + 4 * m;
			this.indices[num++] = 6 + 4 * m;
		}
		this.indices[num++] = 0;
		this.indices[num++] = 3;
		this.indices[num++] = 1;
		this.indices[num++] = 1;
		this.indices[num++] = 2;
		this.indices[num++] = 0;
		this.indices[num++] = 4 + 4 * (this.numNodes - 1);
		this.indices[num++] = 6 + 4 * (this.numNodes - 1);
		this.indices[num++] = 5 + 4 * (this.numNodes - 1);
		this.indices[num++] = 5 + 4 * (this.numNodes - 1);
		this.indices[num++] = 7 + 4 * (this.numNodes - 1);
		this.indices[num++] = 4 + 4 * (this.numNodes - 1);
		if (this.verts.Count < 3)
		{
			return;
		}
		if (this.uvs.Count < 3)
		{
			return;
		}
		if (this.uvs2.Count < 3)
		{
			return;
		}
		if (this.normals.Count < 3)
		{
			return;
		}
		if (this.indices.Length < 3)
		{
			return;
		}
		this.mesh.SetVertices(this.verts);
		this.mesh.uv = this.uvs.ToArray();
		this.mesh.uv2 = this.uvs2.ToArray();
		this.mesh.SetNormals(this.normals);
		this.mesh.SetIndices(this.indices, MeshTopology.Triangles, 0);
		this.meshFilter.mesh = this.mesh;
	}

	// Token: 0x06009D72 RID: 40306 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x06009D73 RID: 40307 RVA: 0x003B88E4 File Offset: 0x003B6AE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void MyUpdate()
	{
		if (this.Source != null)
		{
			if (this.cachedSourcePosition != this.Source.transform.position)
			{
				this.cachedSourcePosition = this.Source.transform.position;
				this.BuildMesh();
				return;
			}
		}
		else
		{
			this.BuildMesh();
		}
	}

	// Token: 0x06009D74 RID: 40308 RVA: 0x003B893F File Offset: 0x003B6B3F
	public void FixedUpdate()
	{
		this.MyUpdate();
	}

	// Token: 0x06009D75 RID: 40309 RVA: 0x003B893F File Offset: 0x003B6B3F
	public void LateUpdate()
	{
		this.MyUpdate();
	}

	// Token: 0x06009D76 RID: 40310 RVA: 0x003B8947 File Offset: 0x003B6B47
	public Vector3 GetStartPosition()
	{
		return this.SourcePosition;
	}

	// Token: 0x06009D77 RID: 40311 RVA: 0x003B894F File Offset: 0x003B6B4F
	public Vector3 GetStartPositionOffset()
	{
		return this.sourceOffset;
	}

	// Token: 0x06009D78 RID: 40312 RVA: 0x003B8957 File Offset: 0x003B6B57
	public void SetStartPosition(Vector3 pos)
	{
		this.SourcePosition = pos;
	}

	// Token: 0x06009D79 RID: 40313 RVA: 0x003B8960 File Offset: 0x003B6B60
	public void SetStartPositionOffset(Vector3 pos)
	{
		this.sourceOffset = pos;
	}

	// Token: 0x06009D7A RID: 40314 RVA: 0x003B8969 File Offset: 0x003B6B69
	public Vector3 GetEndPosition()
	{
		return this.LocalPosition;
	}

	// Token: 0x06009D7B RID: 40315 RVA: 0x003B8971 File Offset: 0x003B6B71
	public Vector3 GetEndPositionOffset()
	{
		return this.localOffset;
	}

	// Token: 0x06009D7C RID: 40316 RVA: 0x003B8979 File Offset: 0x003B6B79
	public void SetEndPosition(Vector3 pos)
	{
		this.LocalPosition = pos;
	}

	// Token: 0x06009D7D RID: 40317 RVA: 0x003B8982 File Offset: 0x003B6B82
	public void SetEndPositionOffset(Vector3 pos)
	{
		this.localOffset = pos;
	}

	// Token: 0x06009D7E RID: 40318 RVA: 0x003AA411 File Offset: 0x003A8611
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009D7F RID: 40319 RVA: 0x003B898B File Offset: 0x003B6B8B
	public Bounds GetBounds()
	{
		return this.mesh.bounds;
	}

	// Token: 0x06009D80 RID: 40320 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireDip(float _dist)
	{
	}

	// Token: 0x06009D81 RID: 40321 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public float GetWireDip()
	{
		return 0f;
	}

	// Token: 0x06009D82 RID: 40322 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireRadius(float _radius)
	{
	}

	// Token: 0x06009D83 RID: 40323 RVA: 0x003B8998 File Offset: 0x003B6B98
	public void Reset()
	{
		this.pulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);
	}

	// Token: 0x06009D84 RID: 40324 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireCanHide(bool _canHide)
	{
	}

	// Token: 0x06009D85 RID: 40325 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetVisible(bool _visible)
	{
	}

	// Token: 0x0400774F RID: 30543
	public const int cLayerMaskRayCast = 65537;

	// Token: 0x04007750 RID: 30544
	public Vector2[] shocks;

	// Token: 0x04007751 RID: 30545
	public Material material;

	// Token: 0x04007752 RID: 30546
	public GameObject Source;

	// Token: 0x04007753 RID: 30547
	public GameObject parentGO;

	// Token: 0x04007754 RID: 30548
	public Color pulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);

	// Token: 0x04007755 RID: 30549
	public Color wireColor = Color.black;

	// Token: 0x04007756 RID: 30550
	public float size;

	// Token: 0x04007757 RID: 30551
	public float weightMod;

	// Token: 0x04007758 RID: 30552
	public float springMod;

	// Token: 0x04007759 RID: 30553
	public float minSegmentLength = 0.25f;

	// Token: 0x0400775A RID: 30554
	public float snagThreshold = 0.5f;

	// Token: 0x0400775B RID: 30555
	public float tensionMultiplier = 2f;

	// Token: 0x0400775C RID: 30556
	public float playerHeight = 1.8f;

	// Token: 0x0400775D RID: 30557
	public Vector3 SourcePosition = Vector3.zero;

	// Token: 0x0400775E RID: 30558
	public Vector3 LocalPosition = Vector3.one;

	// Token: 0x0400775F RID: 30559
	public Vector3 localOffset = Vector3.zero;

	// Token: 0x04007760 RID: 30560
	public Vector3 sourceOffset = Vector3.zero;

	// Token: 0x04007761 RID: 30561
	public Vector3 cameraOffset;

	// Token: 0x04007762 RID: 30562
	public bool attatchSoureToCamera;

	// Token: 0x04007763 RID: 30563
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float playerShockTimer;

	// Token: 0x04007764 RID: 30564
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentTotalForces = 1000f;

	// Token: 0x04007765 RID: 30565
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevSourcePos;

	// Token: 0x04007766 RID: 30566
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentSegmentLength;

	// Token: 0x04007767 RID: 30567
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject rootShockPrefab;

	// Token: 0x04007768 RID: 30568
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] shockTimers = new float[8];

	// Token: 0x04007769 RID: 30569
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] shockIndices = new int[8];

	// Token: 0x0400776A RID: 30570
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] currentShockingVertex = new int[8];

	// Token: 0x0400776B RID: 30571
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<WireNode.ShockSpot> shockSpots;

	// Token: 0x0400776C RID: 30572
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 min = Vector3.one * float.PositiveInfinity;

	// Token: 0x0400776D RID: 30573
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 max = -Vector3.one * float.PositiveInfinity;

	// Token: 0x0400776E RID: 30574
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevSourcePosition = Vector3.zero;

	// Token: 0x0400776F RID: 30575
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevLocalPosition = Vector3.zero;

	// Token: 0x04007770 RID: 30576
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject prevParentGO;

	// Token: 0x04007771 RID: 30577
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevSize = 0.01f;

	// Token: 0x04007772 RID: 30578
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material personalMaterial;

	// Token: 0x04007773 RID: 30579
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numNodes = 15;

	// Token: 0x04007774 RID: 30580
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevWireColor = Color.black;

	// Token: 0x04007775 RID: 30581
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevWeightMod = 1f;

	// Token: 0x04007776 RID: 30582
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevSpringMod = 1f;

	// Token: 0x04007777 RID: 30583
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform parent;

	// Token: 0x04007778 RID: 30584
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform prevParent;

	// Token: 0x04007779 RID: 30585
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh mesh;

	// Token: 0x0400777A RID: 30586
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x0400777B RID: 30587
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x0400777C RID: 30588
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshCollider meshCollider;

	// Token: 0x0400777D RID: 30589
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> points;

	// Token: 0x0400777E RID: 30590
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> forces;

	// Token: 0x0400777F RID: 30591
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> verts;

	// Token: 0x04007780 RID: 30592
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector2> uvs;

	// Token: 0x04007781 RID: 30593
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector2> uvs2;

	// Token: 0x04007782 RID: 30594
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> normals;

	// Token: 0x04007783 RID: 30595
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<bool> shockNodes;

	// Token: 0x04007784 RID: 30596
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] indices;

	// Token: 0x04007785 RID: 30597
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool usingLocalPosition = true;

	// Token: 0x04007786 RID: 30598
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool usingSourcePosition = true;

	// Token: 0x04007787 RID: 30599
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float checkPlayerConnectTime;

	// Token: 0x04007788 RID: 30600
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int attachedNode = -1;

	// Token: 0x04007789 RID: 30601
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int prevAttachedNode = -1;

	// Token: 0x0400778A RID: 30602
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float slopeSpeed = 1f;

	// Token: 0x0400778B RID: 30603
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 currLocalPosition;

	// Token: 0x0400778C RID: 30604
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 currSourcePosition;

	// Token: 0x0400778D RID: 30605
	public float drag = 0.89f;

	// Token: 0x0400778E RID: 30606
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 cachedSourcePosition;

	// Token: 0x0200137D RID: 4989
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ShockSpot
	{
		// Token: 0x0400778F RID: 30607
		public float timer;

		// Token: 0x04007790 RID: 30608
		public int shockDataIndex;

		// Token: 0x04007791 RID: 30609
		public int vertex;

		// Token: 0x04007792 RID: 30610
		public GameObject shockPrefab;
	}
}
