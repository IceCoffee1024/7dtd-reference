using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001362 RID: 4962
public class WireNode : MonoBehaviour, IWireNode
{
	// Token: 0x06009C6A RID: 40042 RVA: 0x003B1F10 File Offset: 0x003B0110
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

	// Token: 0x06009C6B RID: 40043 RVA: 0x003B20B4 File Offset: 0x003B02B4
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

	// Token: 0x06009C6C RID: 40044 RVA: 0x003B210D File Offset: 0x003B030D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.Init();
	}

	// Token: 0x06009C6D RID: 40045 RVA: 0x003B2115 File Offset: 0x003B0315
	public void ToggleMeshCollider(bool _bOn)
	{
		this.meshCollider.enabled = _bOn;
	}

	// Token: 0x06009C6E RID: 40046 RVA: 0x003B2123 File Offset: 0x003B0323
	public void SetPulseSpeed(float speed)
	{
		if (this.personalMaterial == null)
		{
			return;
		}
		this.personalMaterial.SetFloat("_PulseSpeed", speed);
	}

	// Token: 0x06009C6F RID: 40047 RVA: 0x003B2145 File Offset: 0x003B0345
	public void TogglePulse(bool isOn)
	{
		if (this.personalMaterial == null)
		{
			return;
		}
		this.personalMaterial.SetColor("_PulseColor", isOn ? this.pulseColor : Color.black);
	}

	// Token: 0x06009C70 RID: 40048 RVA: 0x003B2176 File Offset: 0x003B0376
	public void SetPulseColor(Color color)
	{
		this.pulseColor = color;
	}

	// Token: 0x06009C71 RID: 40049 RVA: 0x003B2180 File Offset: 0x003B0380
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

	// Token: 0x06009C72 RID: 40050 RVA: 0x003B2308 File Offset: 0x003B0508
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

	// Token: 0x06009C73 RID: 40051 RVA: 0x003B2374 File Offset: 0x003B0574
	[PublicizedFrom(EAccessModifier.Private)]
	public void RayCastHeightAt(ref Vector3 position)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(position + Vector3.up - Origin.position, Vector3.down), out raycastHit, 1.75f, 65537) && raycastHit.point.y > position.y)
		{
			position.y = raycastHit.point.y + 0.01f - Origin.position.y;
		}
	}

	// Token: 0x06009C74 RID: 40052 RVA: 0x003B23F0 File Offset: 0x003B05F0
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

	// Token: 0x06009C75 RID: 40053 RVA: 0x003B277C File Offset: 0x003B097C
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

	// Token: 0x06009C76 RID: 40054 RVA: 0x003B28E4 File Offset: 0x003B0AE4
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

	// Token: 0x06009C77 RID: 40055 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x06009C78 RID: 40056 RVA: 0x003B3630 File Offset: 0x003B1830
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

	// Token: 0x06009C79 RID: 40057 RVA: 0x003B368B File Offset: 0x003B188B
	public void FixedUpdate()
	{
		this.MyUpdate();
	}

	// Token: 0x06009C7A RID: 40058 RVA: 0x003B368B File Offset: 0x003B188B
	public void LateUpdate()
	{
		this.MyUpdate();
	}

	// Token: 0x06009C7B RID: 40059 RVA: 0x003B3693 File Offset: 0x003B1893
	public Vector3 GetStartPosition()
	{
		return this.SourcePosition;
	}

	// Token: 0x06009C7C RID: 40060 RVA: 0x003B369B File Offset: 0x003B189B
	public Vector3 GetStartPositionOffset()
	{
		return this.sourceOffset;
	}

	// Token: 0x06009C7D RID: 40061 RVA: 0x003B36A3 File Offset: 0x003B18A3
	public void SetStartPosition(Vector3 pos)
	{
		this.SourcePosition = pos;
	}

	// Token: 0x06009C7E RID: 40062 RVA: 0x003B36AC File Offset: 0x003B18AC
	public void SetStartPositionOffset(Vector3 pos)
	{
		this.sourceOffset = pos;
	}

	// Token: 0x06009C7F RID: 40063 RVA: 0x003B36B5 File Offset: 0x003B18B5
	public Vector3 GetEndPosition()
	{
		return this.LocalPosition;
	}

	// Token: 0x06009C80 RID: 40064 RVA: 0x003B36BD File Offset: 0x003B18BD
	public Vector3 GetEndPositionOffset()
	{
		return this.localOffset;
	}

	// Token: 0x06009C81 RID: 40065 RVA: 0x003B36C5 File Offset: 0x003B18C5
	public void SetEndPosition(Vector3 pos)
	{
		this.LocalPosition = pos;
	}

	// Token: 0x06009C82 RID: 40066 RVA: 0x003B36CE File Offset: 0x003B18CE
	public void SetEndPositionOffset(Vector3 pos)
	{
		this.localOffset = pos;
	}

	// Token: 0x06009C83 RID: 40067 RVA: 0x003A5179 File Offset: 0x003A3379
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009C84 RID: 40068 RVA: 0x003B36D7 File Offset: 0x003B18D7
	public Bounds GetBounds()
	{
		return this.mesh.bounds;
	}

	// Token: 0x06009C85 RID: 40069 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireDip(float _dist)
	{
	}

	// Token: 0x06009C86 RID: 40070 RVA: 0x0003D066 File Offset: 0x0003B266
	public float GetWireDip()
	{
		return 0f;
	}

	// Token: 0x06009C87 RID: 40071 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireRadius(float _radius)
	{
	}

	// Token: 0x06009C88 RID: 40072 RVA: 0x003B36E4 File Offset: 0x003B18E4
	public void Reset()
	{
		this.pulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);
	}

	// Token: 0x06009C89 RID: 40073 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireCanHide(bool _canHide)
	{
	}

	// Token: 0x06009C8A RID: 40074 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetVisible(bool _visible)
	{
	}

	// Token: 0x04007677 RID: 30327
	public const int cLayerMaskRayCast = 65537;

	// Token: 0x04007678 RID: 30328
	public Vector2[] shocks;

	// Token: 0x04007679 RID: 30329
	public Material material;

	// Token: 0x0400767A RID: 30330
	public GameObject Source;

	// Token: 0x0400767B RID: 30331
	public GameObject parentGO;

	// Token: 0x0400767C RID: 30332
	public Color pulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);

	// Token: 0x0400767D RID: 30333
	public Color wireColor = Color.black;

	// Token: 0x0400767E RID: 30334
	public float size;

	// Token: 0x0400767F RID: 30335
	public float weightMod;

	// Token: 0x04007680 RID: 30336
	public float springMod;

	// Token: 0x04007681 RID: 30337
	public float minSegmentLength = 0.25f;

	// Token: 0x04007682 RID: 30338
	public float snagThreshold = 0.5f;

	// Token: 0x04007683 RID: 30339
	public float tensionMultiplier = 2f;

	// Token: 0x04007684 RID: 30340
	public float playerHeight = 1.8f;

	// Token: 0x04007685 RID: 30341
	public Vector3 SourcePosition = Vector3.zero;

	// Token: 0x04007686 RID: 30342
	public Vector3 LocalPosition = Vector3.one;

	// Token: 0x04007687 RID: 30343
	public Vector3 localOffset = Vector3.zero;

	// Token: 0x04007688 RID: 30344
	public Vector3 sourceOffset = Vector3.zero;

	// Token: 0x04007689 RID: 30345
	public Vector3 cameraOffset;

	// Token: 0x0400768A RID: 30346
	public bool attatchSoureToCamera;

	// Token: 0x0400768B RID: 30347
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float playerShockTimer;

	// Token: 0x0400768C RID: 30348
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentTotalForces = 1000f;

	// Token: 0x0400768D RID: 30349
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevSourcePos;

	// Token: 0x0400768E RID: 30350
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentSegmentLength;

	// Token: 0x0400768F RID: 30351
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject rootShockPrefab;

	// Token: 0x04007690 RID: 30352
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] shockTimers = new float[8];

	// Token: 0x04007691 RID: 30353
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] shockIndices = new int[8];

	// Token: 0x04007692 RID: 30354
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] currentShockingVertex = new int[8];

	// Token: 0x04007693 RID: 30355
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<WireNode.ShockSpot> shockSpots;

	// Token: 0x04007694 RID: 30356
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 min = Vector3.one * float.PositiveInfinity;

	// Token: 0x04007695 RID: 30357
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 max = -Vector3.one * float.PositiveInfinity;

	// Token: 0x04007696 RID: 30358
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevSourcePosition = Vector3.zero;

	// Token: 0x04007697 RID: 30359
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevLocalPosition = Vector3.zero;

	// Token: 0x04007698 RID: 30360
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject prevParentGO;

	// Token: 0x04007699 RID: 30361
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevSize = 0.01f;

	// Token: 0x0400769A RID: 30362
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material personalMaterial;

	// Token: 0x0400769B RID: 30363
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numNodes = 15;

	// Token: 0x0400769C RID: 30364
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevWireColor = Color.black;

	// Token: 0x0400769D RID: 30365
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevWeightMod = 1f;

	// Token: 0x0400769E RID: 30366
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevSpringMod = 1f;

	// Token: 0x0400769F RID: 30367
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform parent;

	// Token: 0x040076A0 RID: 30368
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform prevParent;

	// Token: 0x040076A1 RID: 30369
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh mesh;

	// Token: 0x040076A2 RID: 30370
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x040076A3 RID: 30371
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x040076A4 RID: 30372
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshCollider meshCollider;

	// Token: 0x040076A5 RID: 30373
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> points;

	// Token: 0x040076A6 RID: 30374
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> forces;

	// Token: 0x040076A7 RID: 30375
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> verts;

	// Token: 0x040076A8 RID: 30376
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector2> uvs;

	// Token: 0x040076A9 RID: 30377
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector2> uvs2;

	// Token: 0x040076AA RID: 30378
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> normals;

	// Token: 0x040076AB RID: 30379
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<bool> shockNodes;

	// Token: 0x040076AC RID: 30380
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] indices;

	// Token: 0x040076AD RID: 30381
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool usingLocalPosition = true;

	// Token: 0x040076AE RID: 30382
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool usingSourcePosition = true;

	// Token: 0x040076AF RID: 30383
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float checkPlayerConnectTime;

	// Token: 0x040076B0 RID: 30384
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int attachedNode = -1;

	// Token: 0x040076B1 RID: 30385
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int prevAttachedNode = -1;

	// Token: 0x040076B2 RID: 30386
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float slopeSpeed = 1f;

	// Token: 0x040076B3 RID: 30387
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 currLocalPosition;

	// Token: 0x040076B4 RID: 30388
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 currSourcePosition;

	// Token: 0x040076B5 RID: 30389
	public float drag = 0.89f;

	// Token: 0x040076B6 RID: 30390
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 cachedSourcePosition;

	// Token: 0x02001363 RID: 4963
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ShockSpot
	{
		// Token: 0x040076B7 RID: 30391
		public float timer;

		// Token: 0x040076B8 RID: 30392
		public int shockDataIndex;

		// Token: 0x040076B9 RID: 30393
		public int vertex;

		// Token: 0x040076BA RID: 30394
		public GameObject shockPrefab;
	}
}
