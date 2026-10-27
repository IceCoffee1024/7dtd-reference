using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001369 RID: 4969
public class FastWireNode : MonoBehaviour, IWireNode
{
	// Token: 0x170012BF RID: 4799
	// (get) Token: 0x06009C3E RID: 39998 RVA: 0x003B0905 File Offset: 0x003AEB05
	// (set) Token: 0x06009C3F RID: 39999 RVA: 0x003B090D File Offset: 0x003AEB0D
	public Vector3 StartPosition
	{
		get
		{
			return this.startPosition;
		}
		set
		{
			this.startPosition = value;
		}
	}

	// Token: 0x170012C0 RID: 4800
	// (get) Token: 0x06009C40 RID: 40000 RVA: 0x003B0916 File Offset: 0x003AEB16
	// (set) Token: 0x06009C41 RID: 40001 RVA: 0x003B091E File Offset: 0x003AEB1E
	public Vector3 EndPosition
	{
		get
		{
			return this.endPosition;
		}
		set
		{
			this.endPosition = value;
		}
	}

	// Token: 0x06009C42 RID: 40002 RVA: 0x003B0928 File Offset: 0x003AEB28
	public void Awake()
	{
		if (FastWireNode.BaseMaterial == null)
		{
			FastWireNode.BaseMaterial = Resources.Load<Material>("Materials/WireMaterial");
		}
		if (this.meshFilter == null)
		{
			this.meshFilter = base.transform.gameObject.AddMissingComponent<MeshFilter>();
		}
		if (this.meshCollider == null)
		{
			this.meshCollider = base.transform.gameObject.AddMissingComponent<MeshCollider>();
			this.meshCollider.convex = true;
			this.meshCollider.isTrigger = true;
		}
		if (this.meshRenderer == null)
		{
			this.meshRenderer = base.transform.gameObject.AddMissingComponent<MeshRenderer>();
			this.meshRenderer.material = FastWireNode.BaseMaterial;
		}
		Utils.SetColliderLayerRecursively(base.gameObject, 29);
	}

	// Token: 0x06009C43 RID: 40003 RVA: 0x003B09F4 File Offset: 0x003AEBF4
	public void BuildMesh()
	{
		Vector3 vector = this.EndPosition + this.EndOffset;
		Vector3 vector2 = this.StartPosition + this.StartOffset;
		float num = Vector3.Distance(vector, vector2);
		if (num > 256f)
		{
			return;
		}
		if (num < 0.01f)
		{
			return;
		}
		Vector3 vector3 = vector2 - vector;
		if (vector3.magnitude == 0f)
		{
			return;
		}
		Vector3 vector4 = vector3 / 16f;
		if (vector4.magnitude == 0f)
		{
			return;
		}
		Vector3 vector5 = vector + vector4;
		List<Vector3> list = new List<Vector3>();
		list.Add(vector);
		for (int i = 0; i < 15; i++)
		{
			if (vector3.normalized != Vector3.up && vector3.normalized != Vector3.down)
			{
				float num2 = Mathf.Abs((8f - (float)(i + 1)) / 8f);
				num2 *= num2;
				list.Add(vector5 - new Vector3(0f, Mathf.Lerp(0f, this.maxWireDip, 1f - num2), 0f));
			}
			else
			{
				list.Add(vector5);
			}
			vector5 += vector4;
		}
		if (list.Count > 1)
		{
			list[0] = vector;
			list[list.Count - 1] = vector2;
		}
		Vector3 vector6 = Vector3.one * float.PositiveInfinity;
		Vector3 vector7 = -Vector3.one * float.PositiveInfinity;
		List<Vector3> list2 = new List<Vector3>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector2> list4 = new List<Vector2>();
		List<Vector2> list5 = new List<Vector2>();
		int[] array = new int[396];
		for (int j = 0; j < list.Count; j++)
		{
			float num3 = (float)j / (float)list.Count * (num * 0.25f);
			list4.Add(Vector2.right * num3);
			list4.Add(Vector2.right * num3 + Vector2.up);
			list4.Add(Vector2.right * num3);
			list4.Add(Vector2.right * num3 + Vector2.up);
			list5.Add(Vector2.zero);
			list5.Add(Vector2.zero);
			list5.Add(Vector2.zero);
			list5.Add(Vector2.zero);
			if (j > 0)
			{
				vector3 = list[j] - list[j - 1];
			}
			Vector3 vector8 = Vector3.Cross(Vector3.up, vector3.normalized).normalized;
			Vector3 vector9 = Vector3.Cross(vector3.normalized, vector8).normalized;
			if (vector3.normalized == Vector3.up || vector3.normalized == Vector3.down)
			{
				vector8 = Vector3.Cross(Vector3.forward, vector3.normalized).normalized;
				vector9 = Vector3.Cross(vector3.normalized, vector8).normalized;
			}
			list2.Add(vector9 * this.wireRadius + list[j]);
			list2.Add(-vector9 * this.wireRadius + list[j]);
			list2.Add(vector8 * this.wireRadius + list[j]);
			list2.Add(-vector8 * this.wireRadius + list[j]);
			if (j == 0)
			{
				vector9 = Vector3.Lerp(vector9, (vector - vector2).normalized, 0.5f).normalized;
				vector8 = Vector3.Lerp(vector8, (vector - vector2).normalized, 0.5f).normalized;
			}
			else if (j == list.Count - 1)
			{
				vector9 = Vector3.Lerp(vector9, -(vector - vector2).normalized, 0.5f).normalized;
				vector8 = Vector3.Lerp(vector8, -(vector - vector2).normalized, 0.5f).normalized;
			}
			list3.Add(vector9);
			list3.Add(-vector9);
			list3.Add(vector8);
			list3.Add(-vector8);
			if (list[j].x < vector6.x)
			{
				vector6.x = list[j].x;
			}
			if (list[j].x > vector7.x)
			{
				vector7.x = list[j].x;
			}
			if (list[j].y < vector6.y)
			{
				vector6.y = list[j].y;
			}
			if (list[j].y > vector7.y)
			{
				vector7.y = list[j].y;
			}
			if (list[j].z < vector6.z)
			{
				vector6.z = list[j].z;
			}
			if (list[j].z > vector7.z)
			{
				vector7.z = list[j].z;
			}
		}
		int num4 = 0;
		for (int k = 0; k < 15; k++)
		{
			array[num4++] = 4 * k;
			array[num4++] = 4 + 4 * k;
			array[num4++] = 7 + 4 * k;
			array[num4++] = 7 + 4 * k;
			array[num4++] = 3 + 4 * k;
			array[num4++] = 4 * k;
			array[num4++] = 4 + 4 * k;
			array[num4++] = 4 * k;
			array[num4++] = 2 + 4 * k;
			array[num4++] = 2 + 4 * k;
			array[num4++] = 6 + 4 * k;
			array[num4++] = 4 + 4 * k;
			array[num4++] = 3 + 4 * k;
			array[num4++] = 7 + 4 * k;
			array[num4++] = 5 + 4 * k;
			array[num4++] = 5 + 4 * k;
			array[num4++] = 1 + 4 * k;
			array[num4++] = 3 + 4 * k;
			array[num4++] = 6 + 4 * k;
			array[num4++] = 2 + 4 * k;
			array[num4++] = 1 + 4 * k;
			array[num4++] = 1 + 4 * k;
			array[num4++] = 5 + 4 * k;
			array[num4++] = 6 + 4 * k;
		}
		array[num4++] = 0;
		array[num4++] = 3;
		array[num4++] = 1;
		array[num4++] = 1;
		array[num4++] = 2;
		array[num4++] = 0;
		array[num4++] = 60;
		array[num4++] = 62;
		array[num4++] = 61;
		array[num4++] = 61;
		array[num4++] = 63;
		array[num4++] = 60;
		if (list2.Count < 3)
		{
			return;
		}
		if (array.Length < 3)
		{
			return;
		}
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
		}
		this.mesh.SetVertices(list2);
		this.mesh.uv = list4.ToArray();
		this.mesh.uv2 = list5.ToArray();
		this.mesh.SetNormals(list3);
		this.mesh.SetIndices(array, MeshTopology.Triangles, 0);
		this.mesh.RecalculateBounds();
		this.meshFilter.mesh = this.mesh;
		this.meshCollider.sharedMesh = this.mesh;
		if (this.prevWireColor != this.wireColor)
		{
			this.prevWireColor = this.wireColor;
			this.SetWireColor(this.wireColor);
		}
	}

	// Token: 0x06009C44 RID: 40004 RVA: 0x003B1286 File Offset: 0x003AF486
	public void SetWireColor(Color color)
	{
		if (this.meshRenderer.material == null)
		{
			return;
		}
		this.meshRenderer.material.SetColor("_Color", color);
		this.wireColor = color;
	}

	// Token: 0x06009C45 RID: 40005 RVA: 0x003B12B9 File Offset: 0x003AF4B9
	public void SetPulseSpeed(float speed)
	{
		if (this.meshRenderer.material == null)
		{
			return;
		}
		this.meshRenderer.material.SetFloat("_PulseSpeed", speed);
	}

	// Token: 0x06009C46 RID: 40006 RVA: 0x003B12E5 File Offset: 0x003AF4E5
	public void SetPulseColor(Color color)
	{
		this.pulseColor = color;
	}

	// Token: 0x06009C47 RID: 40007 RVA: 0x003B12EE File Offset: 0x003AF4EE
	public void TogglePulse(bool isOn)
	{
		if (this.meshRenderer.material == null)
		{
			return;
		}
		this.meshRenderer.material.SetColor("_PulseColor", isOn ? this.pulseColor : this.wireColor);
	}

	// Token: 0x06009C48 RID: 40008 RVA: 0x003B132A File Offset: 0x003AF52A
	public void SetStartPosition(Vector3 pos)
	{
		this.StartPosition = pos;
	}

	// Token: 0x06009C49 RID: 40009 RVA: 0x003B1333 File Offset: 0x003AF533
	public void SetStartPositionOffset(Vector3 pos)
	{
		this.StartOffset = pos;
	}

	// Token: 0x06009C4A RID: 40010 RVA: 0x003B133C File Offset: 0x003AF53C
	public void SetEndPosition(Vector3 pos)
	{
		this.EndPosition = pos;
	}

	// Token: 0x06009C4B RID: 40011 RVA: 0x003B1345 File Offset: 0x003AF545
	public void SetEndPositionOffset(Vector3 pos)
	{
		this.EndOffset = pos;
	}

	// Token: 0x06009C4C RID: 40012 RVA: 0x003B134E File Offset: 0x003AF54E
	public void SetWireDip(float _dist)
	{
		this.maxWireDip = _dist;
	}

	// Token: 0x06009C4D RID: 40013 RVA: 0x003B1357 File Offset: 0x003AF557
	public float GetWireDip()
	{
		return this.maxWireDip;
	}

	// Token: 0x06009C4E RID: 40014 RVA: 0x003B135F File Offset: 0x003AF55F
	public void SetWireRadius(float _radius)
	{
		this.wireRadius = _radius;
	}

	// Token: 0x06009C4F RID: 40015 RVA: 0x003B1368 File Offset: 0x003AF568
	public void SetWireCanHide(bool _canHide)
	{
		this.canHide = _canHide;
	}

	// Token: 0x06009C50 RID: 40016 RVA: 0x003B1371 File Offset: 0x003AF571
	public Vector3 GetStartPosition()
	{
		return this.StartPosition;
	}

	// Token: 0x06009C51 RID: 40017 RVA: 0x003B1379 File Offset: 0x003AF579
	public Vector3 GetStartPositionOffset()
	{
		return this.StartOffset;
	}

	// Token: 0x06009C52 RID: 40018 RVA: 0x003B1381 File Offset: 0x003AF581
	public Vector3 GetEndPosition()
	{
		return this.EndPosition;
	}

	// Token: 0x06009C53 RID: 40019 RVA: 0x003B1389 File Offset: 0x003AF589
	public Vector3 GetEndPositionOffset()
	{
		return this.EndOffset;
	}

	// Token: 0x06009C54 RID: 40020 RVA: 0x003B1391 File Offset: 0x003AF591
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009C55 RID: 40021 RVA: 0x003B1399 File Offset: 0x003AF599
	public void SetVisible(bool _visible)
	{
		if (this.canHide)
		{
			base.gameObject.SetActive(_visible);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06009C56 RID: 40022 RVA: 0x003B13BC File Offset: 0x003AF5BC
	public Bounds GetBounds()
	{
		return this.mesh.bounds;
	}

	// Token: 0x06009C57 RID: 40023 RVA: 0x003B13C9 File Offset: 0x003AF5C9
	public void Reset()
	{
		this.maxWireDip = 0.25f;
		this.wireRadius = 0.01f;
		this.pulseColor = Color.yellow;
	}

	// Token: 0x040075D0 RID: 30160
	public const int cLayerMaskRayCast = 65537;

	// Token: 0x040075D1 RID: 30161
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int NODE_COUNT = 15;

	// Token: 0x040075D2 RID: 30162
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float BASE_WIRE_RADIUS = 0.01f;

	// Token: 0x040075D3 RID: 30163
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float BASE_MIN_WIRE_DIP = 0f;

	// Token: 0x040075D4 RID: 30164
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float BASE_MAX_WIRE_DIP = 0.25f;

	// Token: 0x040075D5 RID: 30165
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Material BaseMaterial;

	// Token: 0x040075D6 RID: 30166
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float maxWireDip = 0.25f;

	// Token: 0x040075D7 RID: 30167
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float wireRadius = 0.01f;

	// Token: 0x040075D8 RID: 30168
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 startPosition;

	// Token: 0x040075D9 RID: 30169
	public Vector3 StartOffset;

	// Token: 0x040075DA RID: 30170
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 endPosition;

	// Token: 0x040075DB RID: 30171
	public Vector3 EndOffset;

	// Token: 0x040075DC RID: 30172
	public Color pulseColor = Color.yellow;

	// Token: 0x040075DD RID: 30173
	public Color wireColor = Color.black;

	// Token: 0x040075DE RID: 30174
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool canHide = true;

	// Token: 0x040075DF RID: 30175
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh mesh;

	// Token: 0x040075E0 RID: 30176
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x040075E1 RID: 30177
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshCollider meshCollider;

	// Token: 0x040075E2 RID: 30178
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x040075E3 RID: 30179
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevWireColor = Color.white;
}
