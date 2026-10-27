using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013AE RID: 5038
public class WireNode : MonoBehaviour, IWireNode
{
	// Token: 0x06009E68 RID: 40552 RVA: 0x003BE144 File Offset: 0x003BC344
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

	// Token: 0x06009E69 RID: 40553 RVA: 0x003BE2E8 File Offset: 0x003BC4E8
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

	// Token: 0x06009E6A RID: 40554 RVA: 0x003BE341 File Offset: 0x003BC541
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.Init();
	}

	// Token: 0x06009E6B RID: 40555 RVA: 0x003BE349 File Offset: 0x003BC549
	public void ToggleMeshCollider(bool _bOn)
	{
		this.meshCollider.enabled = _bOn;
	}

	// Token: 0x06009E6C RID: 40556 RVA: 0x003BE357 File Offset: 0x003BC557
	public void SetPulseSpeed(float speed)
	{
		if (this.personalMaterial == null)
		{
			return;
		}
		this.personalMaterial.SetFloat("_PulseSpeed", speed);
	}

	// Token: 0x06009E6D RID: 40557 RVA: 0x003BE379 File Offset: 0x003BC579
	public void TogglePulse(bool isOn)
	{
		if (this.personalMaterial == null)
		{
			return;
		}
		this.personalMaterial.SetColor("_PulseColor", isOn ? this.pulseColor : Color.black);
	}

	// Token: 0x06009E6E RID: 40558 RVA: 0x003BE3AA File Offset: 0x003BC5AA
	public void SetPulseColor(Color color)
	{
		this.pulseColor = color;
	}

	// Token: 0x06009E6F RID: 40559 RVA: 0x003BE3B4 File Offset: 0x003BC5B4
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

	// Token: 0x06009E70 RID: 40560 RVA: 0x003BE53C File Offset: 0x003BC73C
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

	// Token: 0x06009E71 RID: 40561 RVA: 0x003BE5A8 File Offset: 0x003BC7A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void RayCastHeightAt(ref Vector3 position)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(position + Vector3.up - Origin.position, Vector3.down), out raycastHit, 1.75f, 65537) && raycastHit.point.y > position.y)
		{
			position.y = raycastHit.point.y + 0.01f - Origin.position.y;
		}
	}

	// Token: 0x06009E72 RID: 40562 RVA: 0x003BE624 File Offset: 0x003BC824
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

	// Token: 0x06009E73 RID: 40563 RVA: 0x003BE9B0 File Offset: 0x003BCBB0
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

	// Token: 0x06009E74 RID: 40564 RVA: 0x003BEB18 File Offset: 0x003BCD18
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

	// Token: 0x06009E75 RID: 40565 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x06009E76 RID: 40566 RVA: 0x003BF864 File Offset: 0x003BDA64
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

	// Token: 0x06009E77 RID: 40567 RVA: 0x003BF8BF File Offset: 0x003BDABF
	public void FixedUpdate()
	{
		this.MyUpdate();
	}

	// Token: 0x06009E78 RID: 40568 RVA: 0x003BF8BF File Offset: 0x003BDABF
	public void LateUpdate()
	{
		this.MyUpdate();
	}

	// Token: 0x06009E79 RID: 40569 RVA: 0x003BF8C7 File Offset: 0x003BDAC7
	public Vector3 GetStartPosition()
	{
		return this.SourcePosition;
	}

	// Token: 0x06009E7A RID: 40570 RVA: 0x003BF8CF File Offset: 0x003BDACF
	public Vector3 GetStartPositionOffset()
	{
		return this.sourceOffset;
	}

	// Token: 0x06009E7B RID: 40571 RVA: 0x003BF8D7 File Offset: 0x003BDAD7
	public void SetStartPosition(Vector3 pos)
	{
		this.SourcePosition = pos;
	}

	// Token: 0x06009E7C RID: 40572 RVA: 0x003BF8E0 File Offset: 0x003BDAE0
	public void SetStartPositionOffset(Vector3 pos)
	{
		this.sourceOffset = pos;
	}

	// Token: 0x06009E7D RID: 40573 RVA: 0x003BF8E9 File Offset: 0x003BDAE9
	public Vector3 GetEndPosition()
	{
		return this.LocalPosition;
	}

	// Token: 0x06009E7E RID: 40574 RVA: 0x003BF8F1 File Offset: 0x003BDAF1
	public Vector3 GetEndPositionOffset()
	{
		return this.localOffset;
	}

	// Token: 0x06009E7F RID: 40575 RVA: 0x003BF8F9 File Offset: 0x003BDAF9
	public void SetEndPosition(Vector3 pos)
	{
		this.LocalPosition = pos;
	}

	// Token: 0x06009E80 RID: 40576 RVA: 0x003BF902 File Offset: 0x003BDB02
	public void SetEndPositionOffset(Vector3 pos)
	{
		this.localOffset = pos;
	}

	// Token: 0x06009E81 RID: 40577 RVA: 0x003B1391 File Offset: 0x003AF591
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009E82 RID: 40578 RVA: 0x003BF90B File Offset: 0x003BDB0B
	public Bounds GetBounds()
	{
		return this.mesh.bounds;
	}

	// Token: 0x06009E83 RID: 40579 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireDip(float _dist)
	{
	}

	// Token: 0x06009E84 RID: 40580 RVA: 0x0003D372 File Offset: 0x0003B572
	public float GetWireDip()
	{
		return 0f;
	}

	// Token: 0x06009E85 RID: 40581 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireRadius(float _radius)
	{
	}

	// Token: 0x06009E86 RID: 40582 RVA: 0x003BF918 File Offset: 0x003BDB18
	public void Reset()
	{
		this.pulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);
	}

	// Token: 0x06009E87 RID: 40583 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireCanHide(bool _canHide)
	{
	}

	// Token: 0x06009E88 RID: 40584 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetVisible(bool _visible)
	{
	}

	// Token: 0x0400780B RID: 30731
	public const int cLayerMaskRayCast = 65537;

	// Token: 0x0400780C RID: 30732
	public Vector2[] shocks;

	// Token: 0x0400780D RID: 30733
	public Material material;

	// Token: 0x0400780E RID: 30734
	public GameObject Source;

	// Token: 0x0400780F RID: 30735
	public GameObject parentGO;

	// Token: 0x04007810 RID: 30736
	public Color pulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);

	// Token: 0x04007811 RID: 30737
	public Color wireColor = Color.black;

	// Token: 0x04007812 RID: 30738
	public float size;

	// Token: 0x04007813 RID: 30739
	public float weightMod;

	// Token: 0x04007814 RID: 30740
	public float springMod;

	// Token: 0x04007815 RID: 30741
	public float minSegmentLength = 0.25f;

	// Token: 0x04007816 RID: 30742
	public float snagThreshold = 0.5f;

	// Token: 0x04007817 RID: 30743
	public float tensionMultiplier = 2f;

	// Token: 0x04007818 RID: 30744
	public float playerHeight = 1.8f;

	// Token: 0x04007819 RID: 30745
	public Vector3 SourcePosition = Vector3.zero;

	// Token: 0x0400781A RID: 30746
	public Vector3 LocalPosition = Vector3.one;

	// Token: 0x0400781B RID: 30747
	public Vector3 localOffset = Vector3.zero;

	// Token: 0x0400781C RID: 30748
	public Vector3 sourceOffset = Vector3.zero;

	// Token: 0x0400781D RID: 30749
	public Vector3 cameraOffset;

	// Token: 0x0400781E RID: 30750
	public bool attatchSoureToCamera;

	// Token: 0x0400781F RID: 30751
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float playerShockTimer;

	// Token: 0x04007820 RID: 30752
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentTotalForces = 1000f;

	// Token: 0x04007821 RID: 30753
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevSourcePos;

	// Token: 0x04007822 RID: 30754
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentSegmentLength;

	// Token: 0x04007823 RID: 30755
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject rootShockPrefab;

	// Token: 0x04007824 RID: 30756
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] shockTimers = new float[8];

	// Token: 0x04007825 RID: 30757
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] shockIndices = new int[8];

	// Token: 0x04007826 RID: 30758
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] currentShockingVertex = new int[8];

	// Token: 0x04007827 RID: 30759
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<WireNode.ShockSpot> shockSpots;

	// Token: 0x04007828 RID: 30760
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 min = Vector3.one * float.PositiveInfinity;

	// Token: 0x04007829 RID: 30761
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 max = -Vector3.one * float.PositiveInfinity;

	// Token: 0x0400782A RID: 30762
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevSourcePosition = Vector3.zero;

	// Token: 0x0400782B RID: 30763
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevLocalPosition = Vector3.zero;

	// Token: 0x0400782C RID: 30764
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject prevParentGO;

	// Token: 0x0400782D RID: 30765
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevSize = 0.01f;

	// Token: 0x0400782E RID: 30766
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material personalMaterial;

	// Token: 0x0400782F RID: 30767
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numNodes = 15;

	// Token: 0x04007830 RID: 30768
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevWireColor = Color.black;

	// Token: 0x04007831 RID: 30769
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevWeightMod = 1f;

	// Token: 0x04007832 RID: 30770
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevSpringMod = 1f;

	// Token: 0x04007833 RID: 30771
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform parent;

	// Token: 0x04007834 RID: 30772
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform prevParent;

	// Token: 0x04007835 RID: 30773
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh mesh;

	// Token: 0x04007836 RID: 30774
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x04007837 RID: 30775
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x04007838 RID: 30776
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshCollider meshCollider;

	// Token: 0x04007839 RID: 30777
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> points;

	// Token: 0x0400783A RID: 30778
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> forces;

	// Token: 0x0400783B RID: 30779
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> verts;

	// Token: 0x0400783C RID: 30780
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector2> uvs;

	// Token: 0x0400783D RID: 30781
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector2> uvs2;

	// Token: 0x0400783E RID: 30782
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> normals;

	// Token: 0x0400783F RID: 30783
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<bool> shockNodes;

	// Token: 0x04007840 RID: 30784
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] indices;

	// Token: 0x04007841 RID: 30785
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool usingLocalPosition = true;

	// Token: 0x04007842 RID: 30786
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool usingSourcePosition = true;

	// Token: 0x04007843 RID: 30787
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float checkPlayerConnectTime;

	// Token: 0x04007844 RID: 30788
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int attachedNode = -1;

	// Token: 0x04007845 RID: 30789
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int prevAttachedNode = -1;

	// Token: 0x04007846 RID: 30790
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float slopeSpeed = 1f;

	// Token: 0x04007847 RID: 30791
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 currLocalPosition;

	// Token: 0x04007848 RID: 30792
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 currSourcePosition;

	// Token: 0x04007849 RID: 30793
	public float drag = 0.89f;

	// Token: 0x0400784A RID: 30794
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 cachedSourcePosition;

	// Token: 0x020013AF RID: 5039
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ShockSpot
	{
		// Token: 0x0400784B RID: 30795
		public float timer;

		// Token: 0x0400784C RID: 30796
		public int shockDataIndex;

		// Token: 0x0400784D RID: 30797
		public int vertex;

		// Token: 0x0400784E RID: 30798
		public GameObject shockPrefab;
	}
}
