using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200139C RID: 5020
public class WireNode : MonoBehaviour, IWireNode
{
	// Token: 0x06009E10 RID: 40464 RVA: 0x003BBD40 File Offset: 0x003B9F40
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

	// Token: 0x06009E11 RID: 40465 RVA: 0x003BBEE4 File Offset: 0x003BA0E4
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

	// Token: 0x06009E12 RID: 40466 RVA: 0x003BBF3D File Offset: 0x003BA13D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.Init();
	}

	// Token: 0x06009E13 RID: 40467 RVA: 0x003BBF45 File Offset: 0x003BA145
	public void ToggleMeshCollider(bool _bOn)
	{
		this.meshCollider.enabled = _bOn;
	}

	// Token: 0x06009E14 RID: 40468 RVA: 0x003BBF53 File Offset: 0x003BA153
	public void SetPulseSpeed(float speed)
	{
		if (this.personalMaterial == null)
		{
			return;
		}
		this.personalMaterial.SetFloat("_PulseSpeed", speed);
	}

	// Token: 0x06009E15 RID: 40469 RVA: 0x003BBF75 File Offset: 0x003BA175
	public void TogglePulse(bool isOn)
	{
		if (this.personalMaterial == null)
		{
			return;
		}
		this.personalMaterial.SetColor("_PulseColor", isOn ? this.pulseColor : Color.black);
	}

	// Token: 0x06009E16 RID: 40470 RVA: 0x003BBFA6 File Offset: 0x003BA1A6
	public void SetPulseColor(Color color)
	{
		this.pulseColor = color;
	}

	// Token: 0x06009E17 RID: 40471 RVA: 0x003BBFB0 File Offset: 0x003BA1B0
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

	// Token: 0x06009E18 RID: 40472 RVA: 0x003BC138 File Offset: 0x003BA338
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

	// Token: 0x06009E19 RID: 40473 RVA: 0x003BC1A4 File Offset: 0x003BA3A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RayCastHeightAt(ref Vector3 position)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(position + Vector3.up - Origin.position, Vector3.down), out raycastHit, 1.75f, 65537) && raycastHit.point.y > position.y)
		{
			position.y = raycastHit.point.y + 0.01f - Origin.position.y;
		}
	}

	// Token: 0x06009E1A RID: 40474 RVA: 0x003BC220 File Offset: 0x003BA420
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

	// Token: 0x06009E1B RID: 40475 RVA: 0x003BC5AC File Offset: 0x003BA7AC
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

	// Token: 0x06009E1C RID: 40476 RVA: 0x003BC714 File Offset: 0x003BA914
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

	// Token: 0x06009E1D RID: 40477 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x06009E1E RID: 40478 RVA: 0x003BD460 File Offset: 0x003BB660
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

	// Token: 0x06009E1F RID: 40479 RVA: 0x003BD4BB File Offset: 0x003BB6BB
	public void FixedUpdate()
	{
		this.MyUpdate();
	}

	// Token: 0x06009E20 RID: 40480 RVA: 0x003BD4BB File Offset: 0x003BB6BB
	public void LateUpdate()
	{
		this.MyUpdate();
	}

	// Token: 0x06009E21 RID: 40481 RVA: 0x003BD4C3 File Offset: 0x003BB6C3
	public Vector3 GetStartPosition()
	{
		return this.SourcePosition;
	}

	// Token: 0x06009E22 RID: 40482 RVA: 0x003BD4CB File Offset: 0x003BB6CB
	public Vector3 GetStartPositionOffset()
	{
		return this.sourceOffset;
	}

	// Token: 0x06009E23 RID: 40483 RVA: 0x003BD4D3 File Offset: 0x003BB6D3
	public void SetStartPosition(Vector3 pos)
	{
		this.SourcePosition = pos;
	}

	// Token: 0x06009E24 RID: 40484 RVA: 0x003BD4DC File Offset: 0x003BB6DC
	public void SetStartPositionOffset(Vector3 pos)
	{
		this.sourceOffset = pos;
	}

	// Token: 0x06009E25 RID: 40485 RVA: 0x003BD4E5 File Offset: 0x003BB6E5
	public Vector3 GetEndPosition()
	{
		return this.LocalPosition;
	}

	// Token: 0x06009E26 RID: 40486 RVA: 0x003BD4ED File Offset: 0x003BB6ED
	public Vector3 GetEndPositionOffset()
	{
		return this.localOffset;
	}

	// Token: 0x06009E27 RID: 40487 RVA: 0x003BD4F5 File Offset: 0x003BB6F5
	public void SetEndPosition(Vector3 pos)
	{
		this.LocalPosition = pos;
	}

	// Token: 0x06009E28 RID: 40488 RVA: 0x003BD4FE File Offset: 0x003BB6FE
	public void SetEndPositionOffset(Vector3 pos)
	{
		this.localOffset = pos;
	}

	// Token: 0x06009E29 RID: 40489 RVA: 0x003AEF8D File Offset: 0x003AD18D
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009E2A RID: 40490 RVA: 0x003BD507 File Offset: 0x003BB707
	public Bounds GetBounds()
	{
		return this.mesh.bounds;
	}

	// Token: 0x06009E2B RID: 40491 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireDip(float _dist)
	{
	}

	// Token: 0x06009E2C RID: 40492 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public float GetWireDip()
	{
		return 0f;
	}

	// Token: 0x06009E2D RID: 40493 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireRadius(float _radius)
	{
	}

	// Token: 0x06009E2E RID: 40494 RVA: 0x003BD514 File Offset: 0x003BB714
	public void Reset()
	{
		this.pulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);
	}

	// Token: 0x06009E2F RID: 40495 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetWireCanHide(bool _canHide)
	{
	}

	// Token: 0x06009E30 RID: 40496 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetVisible(bool _visible)
	{
	}

	// Token: 0x040077CA RID: 30666
	public const int cLayerMaskRayCast = 65537;

	// Token: 0x040077CB RID: 30667
	public Vector2[] shocks;

	// Token: 0x040077CC RID: 30668
	public Material material;

	// Token: 0x040077CD RID: 30669
	public GameObject Source;

	// Token: 0x040077CE RID: 30670
	public GameObject parentGO;

	// Token: 0x040077CF RID: 30671
	public Color pulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);

	// Token: 0x040077D0 RID: 30672
	public Color wireColor = Color.black;

	// Token: 0x040077D1 RID: 30673
	public float size;

	// Token: 0x040077D2 RID: 30674
	public float weightMod;

	// Token: 0x040077D3 RID: 30675
	public float springMod;

	// Token: 0x040077D4 RID: 30676
	public float minSegmentLength = 0.25f;

	// Token: 0x040077D5 RID: 30677
	public float snagThreshold = 0.5f;

	// Token: 0x040077D6 RID: 30678
	public float tensionMultiplier = 2f;

	// Token: 0x040077D7 RID: 30679
	public float playerHeight = 1.8f;

	// Token: 0x040077D8 RID: 30680
	public Vector3 SourcePosition = Vector3.zero;

	// Token: 0x040077D9 RID: 30681
	public Vector3 LocalPosition = Vector3.one;

	// Token: 0x040077DA RID: 30682
	public Vector3 localOffset = Vector3.zero;

	// Token: 0x040077DB RID: 30683
	public Vector3 sourceOffset = Vector3.zero;

	// Token: 0x040077DC RID: 30684
	public Vector3 cameraOffset;

	// Token: 0x040077DD RID: 30685
	public bool attatchSoureToCamera;

	// Token: 0x040077DE RID: 30686
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float playerShockTimer;

	// Token: 0x040077DF RID: 30687
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentTotalForces = 1000f;

	// Token: 0x040077E0 RID: 30688
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevSourcePos;

	// Token: 0x040077E1 RID: 30689
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentSegmentLength;

	// Token: 0x040077E2 RID: 30690
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject rootShockPrefab;

	// Token: 0x040077E3 RID: 30691
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] shockTimers = new float[8];

	// Token: 0x040077E4 RID: 30692
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] shockIndices = new int[8];

	// Token: 0x040077E5 RID: 30693
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] currentShockingVertex = new int[8];

	// Token: 0x040077E6 RID: 30694
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<WireNode.ShockSpot> shockSpots;

	// Token: 0x040077E7 RID: 30695
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 min = Vector3.one * float.PositiveInfinity;

	// Token: 0x040077E8 RID: 30696
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 max = -Vector3.one * float.PositiveInfinity;

	// Token: 0x040077E9 RID: 30697
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevSourcePosition = Vector3.zero;

	// Token: 0x040077EA RID: 30698
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevLocalPosition = Vector3.zero;

	// Token: 0x040077EB RID: 30699
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject prevParentGO;

	// Token: 0x040077EC RID: 30700
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevSize = 0.01f;

	// Token: 0x040077ED RID: 30701
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material personalMaterial;

	// Token: 0x040077EE RID: 30702
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numNodes = 15;

	// Token: 0x040077EF RID: 30703
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevWireColor = Color.black;

	// Token: 0x040077F0 RID: 30704
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevWeightMod = 1f;

	// Token: 0x040077F1 RID: 30705
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float prevSpringMod = 1f;

	// Token: 0x040077F2 RID: 30706
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform parent;

	// Token: 0x040077F3 RID: 30707
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform prevParent;

	// Token: 0x040077F4 RID: 30708
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh mesh;

	// Token: 0x040077F5 RID: 30709
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x040077F6 RID: 30710
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x040077F7 RID: 30711
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshCollider meshCollider;

	// Token: 0x040077F8 RID: 30712
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> points;

	// Token: 0x040077F9 RID: 30713
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> forces;

	// Token: 0x040077FA RID: 30714
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> verts;

	// Token: 0x040077FB RID: 30715
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector2> uvs;

	// Token: 0x040077FC RID: 30716
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector2> uvs2;

	// Token: 0x040077FD RID: 30717
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> normals;

	// Token: 0x040077FE RID: 30718
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<bool> shockNodes;

	// Token: 0x040077FF RID: 30719
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] indices;

	// Token: 0x04007800 RID: 30720
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool usingLocalPosition = true;

	// Token: 0x04007801 RID: 30721
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool usingSourcePosition = true;

	// Token: 0x04007802 RID: 30722
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float checkPlayerConnectTime;

	// Token: 0x04007803 RID: 30723
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int attachedNode = -1;

	// Token: 0x04007804 RID: 30724
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int prevAttachedNode = -1;

	// Token: 0x04007805 RID: 30725
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float slopeSpeed = 1f;

	// Token: 0x04007806 RID: 30726
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 currLocalPosition;

	// Token: 0x04007807 RID: 30727
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 currSourcePosition;

	// Token: 0x04007808 RID: 30728
	public float drag = 0.89f;

	// Token: 0x04007809 RID: 30729
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 cachedSourcePosition;

	// Token: 0x0200139D RID: 5021
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ShockSpot
	{
		// Token: 0x0400780A RID: 30730
		public float timer;

		// Token: 0x0400780B RID: 30731
		public int shockDataIndex;

		// Token: 0x0400780C RID: 30732
		public int vertex;

		// Token: 0x0400780D RID: 30733
		public GameObject shockPrefab;
	}
}
