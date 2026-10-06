using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001357 RID: 4951
public class FastWireNode : MonoBehaviour, IWireNode
{
	// Token: 0x170012B3 RID: 4787
	// (get) Token: 0x06009BE6 RID: 39910 RVA: 0x003AE501 File Offset: 0x003AC701
	// (set) Token: 0x06009BE7 RID: 39911 RVA: 0x003AE509 File Offset: 0x003AC709
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

	// Token: 0x170012B4 RID: 4788
	// (get) Token: 0x06009BE8 RID: 39912 RVA: 0x003AE512 File Offset: 0x003AC712
	// (set) Token: 0x06009BE9 RID: 39913 RVA: 0x003AE51A File Offset: 0x003AC71A
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

	// Token: 0x06009BEA RID: 39914 RVA: 0x003AE524 File Offset: 0x003AC724
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

	// Token: 0x06009BEB RID: 39915 RVA: 0x003AE5F0 File Offset: 0x003AC7F0
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

	// Token: 0x06009BEC RID: 39916 RVA: 0x003AEE82 File Offset: 0x003AD082
	public void SetWireColor(Color color)
	{
		if (this.meshRenderer.material == null)
		{
			return;
		}
		this.meshRenderer.material.SetColor("_Color", color);
		this.wireColor = color;
	}

	// Token: 0x06009BED RID: 39917 RVA: 0x003AEEB5 File Offset: 0x003AD0B5
	public void SetPulseSpeed(float speed)
	{
		if (this.meshRenderer.material == null)
		{
			return;
		}
		this.meshRenderer.material.SetFloat("_PulseSpeed", speed);
	}

	// Token: 0x06009BEE RID: 39918 RVA: 0x003AEEE1 File Offset: 0x003AD0E1
	public void SetPulseColor(Color color)
	{
		this.pulseColor = color;
	}

	// Token: 0x06009BEF RID: 39919 RVA: 0x003AEEEA File Offset: 0x003AD0EA
	public void TogglePulse(bool isOn)
	{
		if (this.meshRenderer.material == null)
		{
			return;
		}
		this.meshRenderer.material.SetColor("_PulseColor", isOn ? this.pulseColor : this.wireColor);
	}

	// Token: 0x06009BF0 RID: 39920 RVA: 0x003AEF26 File Offset: 0x003AD126
	public void SetStartPosition(Vector3 pos)
	{
		this.StartPosition = pos;
	}

	// Token: 0x06009BF1 RID: 39921 RVA: 0x003AEF2F File Offset: 0x003AD12F
	public void SetStartPositionOffset(Vector3 pos)
	{
		this.StartOffset = pos;
	}

	// Token: 0x06009BF2 RID: 39922 RVA: 0x003AEF38 File Offset: 0x003AD138
	public void SetEndPosition(Vector3 pos)
	{
		this.EndPosition = pos;
	}

	// Token: 0x06009BF3 RID: 39923 RVA: 0x003AEF41 File Offset: 0x003AD141
	public void SetEndPositionOffset(Vector3 pos)
	{
		this.EndOffset = pos;
	}

	// Token: 0x06009BF4 RID: 39924 RVA: 0x003AEF4A File Offset: 0x003AD14A
	public void SetWireDip(float _dist)
	{
		this.maxWireDip = _dist;
	}

	// Token: 0x06009BF5 RID: 39925 RVA: 0x003AEF53 File Offset: 0x003AD153
	public float GetWireDip()
	{
		return this.maxWireDip;
	}

	// Token: 0x06009BF6 RID: 39926 RVA: 0x003AEF5B File Offset: 0x003AD15B
	public void SetWireRadius(float _radius)
	{
		this.wireRadius = _radius;
	}

	// Token: 0x06009BF7 RID: 39927 RVA: 0x003AEF64 File Offset: 0x003AD164
	public void SetWireCanHide(bool _canHide)
	{
		this.canHide = _canHide;
	}

	// Token: 0x06009BF8 RID: 39928 RVA: 0x003AEF6D File Offset: 0x003AD16D
	public Vector3 GetStartPosition()
	{
		return this.StartPosition;
	}

	// Token: 0x06009BF9 RID: 39929 RVA: 0x003AEF75 File Offset: 0x003AD175
	public Vector3 GetStartPositionOffset()
	{
		return this.StartOffset;
	}

	// Token: 0x06009BFA RID: 39930 RVA: 0x003AEF7D File Offset: 0x003AD17D
	public Vector3 GetEndPosition()
	{
		return this.EndPosition;
	}

	// Token: 0x06009BFB RID: 39931 RVA: 0x003AEF85 File Offset: 0x003AD185
	public Vector3 GetEndPositionOffset()
	{
		return this.EndOffset;
	}

	// Token: 0x06009BFC RID: 39932 RVA: 0x003AEF8D File Offset: 0x003AD18D
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009BFD RID: 39933 RVA: 0x003AEF95 File Offset: 0x003AD195
	public void SetVisible(bool _visible)
	{
		if (this.canHide)
		{
			base.gameObject.SetActive(_visible);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06009BFE RID: 39934 RVA: 0x003AEFB8 File Offset: 0x003AD1B8
	public Bounds GetBounds()
	{
		return this.mesh.bounds;
	}

	// Token: 0x06009BFF RID: 39935 RVA: 0x003AEFC5 File Offset: 0x003AD1C5
	public void Reset()
	{
		this.maxWireDip = 0.25f;
		this.wireRadius = 0.01f;
		this.pulseColor = Color.yellow;
	}

	// Token: 0x0400758F RID: 30095
	public const int cLayerMaskRayCast = 65537;

	// Token: 0x04007590 RID: 30096
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int NODE_COUNT = 15;

	// Token: 0x04007591 RID: 30097
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float BASE_WIRE_RADIUS = 0.01f;

	// Token: 0x04007592 RID: 30098
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float BASE_MIN_WIRE_DIP = 0f;

	// Token: 0x04007593 RID: 30099
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float BASE_MAX_WIRE_DIP = 0.25f;

	// Token: 0x04007594 RID: 30100
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Material BaseMaterial;

	// Token: 0x04007595 RID: 30101
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float maxWireDip = 0.25f;

	// Token: 0x04007596 RID: 30102
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float wireRadius = 0.01f;

	// Token: 0x04007597 RID: 30103
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 startPosition;

	// Token: 0x04007598 RID: 30104
	public Vector3 StartOffset;

	// Token: 0x04007599 RID: 30105
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 endPosition;

	// Token: 0x0400759A RID: 30106
	public Vector3 EndOffset;

	// Token: 0x0400759B RID: 30107
	public Color pulseColor = Color.yellow;

	// Token: 0x0400759C RID: 30108
	public Color wireColor = Color.black;

	// Token: 0x0400759D RID: 30109
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool canHide = true;

	// Token: 0x0400759E RID: 30110
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh mesh;

	// Token: 0x0400759F RID: 30111
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x040075A0 RID: 30112
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshCollider meshCollider;

	// Token: 0x040075A1 RID: 30113
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x040075A2 RID: 30114
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevWireColor = Color.white;
}
