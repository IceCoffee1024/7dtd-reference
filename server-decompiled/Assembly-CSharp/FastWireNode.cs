using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200131D RID: 4893
public class FastWireNode : MonoBehaviour, IWireNode
{
	// Token: 0x17001235 RID: 4661
	// (get) Token: 0x06009A3F RID: 39487 RVA: 0x003A46ED File Offset: 0x003A28ED
	// (set) Token: 0x06009A40 RID: 39488 RVA: 0x003A46F5 File Offset: 0x003A28F5
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

	// Token: 0x17001236 RID: 4662
	// (get) Token: 0x06009A41 RID: 39489 RVA: 0x003A46FE File Offset: 0x003A28FE
	// (set) Token: 0x06009A42 RID: 39490 RVA: 0x003A4706 File Offset: 0x003A2906
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

	// Token: 0x06009A43 RID: 39491 RVA: 0x003A4710 File Offset: 0x003A2910
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

	// Token: 0x06009A44 RID: 39492 RVA: 0x003A47DC File Offset: 0x003A29DC
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

	// Token: 0x06009A45 RID: 39493 RVA: 0x003A506E File Offset: 0x003A326E
	public void SetWireColor(Color color)
	{
		if (this.meshRenderer.material == null)
		{
			return;
		}
		this.meshRenderer.material.SetColor("_Color", color);
		this.wireColor = color;
	}

	// Token: 0x06009A46 RID: 39494 RVA: 0x003A50A1 File Offset: 0x003A32A1
	public void SetPulseSpeed(float speed)
	{
		if (this.meshRenderer.material == null)
		{
			return;
		}
		this.meshRenderer.material.SetFloat("_PulseSpeed", speed);
	}

	// Token: 0x06009A47 RID: 39495 RVA: 0x003A50CD File Offset: 0x003A32CD
	public void SetPulseColor(Color color)
	{
		this.pulseColor = color;
	}

	// Token: 0x06009A48 RID: 39496 RVA: 0x003A50D6 File Offset: 0x003A32D6
	public void TogglePulse(bool isOn)
	{
		if (this.meshRenderer.material == null)
		{
			return;
		}
		this.meshRenderer.material.SetColor("_PulseColor", isOn ? this.pulseColor : this.wireColor);
	}

	// Token: 0x06009A49 RID: 39497 RVA: 0x003A5112 File Offset: 0x003A3312
	public void SetStartPosition(Vector3 pos)
	{
		this.StartPosition = pos;
	}

	// Token: 0x06009A4A RID: 39498 RVA: 0x003A511B File Offset: 0x003A331B
	public void SetStartPositionOffset(Vector3 pos)
	{
		this.StartOffset = pos;
	}

	// Token: 0x06009A4B RID: 39499 RVA: 0x003A5124 File Offset: 0x003A3324
	public void SetEndPosition(Vector3 pos)
	{
		this.EndPosition = pos;
	}

	// Token: 0x06009A4C RID: 39500 RVA: 0x003A512D File Offset: 0x003A332D
	public void SetEndPositionOffset(Vector3 pos)
	{
		this.EndOffset = pos;
	}

	// Token: 0x06009A4D RID: 39501 RVA: 0x003A5136 File Offset: 0x003A3336
	public void SetWireDip(float _dist)
	{
		this.maxWireDip = _dist;
	}

	// Token: 0x06009A4E RID: 39502 RVA: 0x003A513F File Offset: 0x003A333F
	public float GetWireDip()
	{
		return this.maxWireDip;
	}

	// Token: 0x06009A4F RID: 39503 RVA: 0x003A5147 File Offset: 0x003A3347
	public void SetWireRadius(float _radius)
	{
		this.wireRadius = _radius;
	}

	// Token: 0x06009A50 RID: 39504 RVA: 0x003A5150 File Offset: 0x003A3350
	public void SetWireCanHide(bool _canHide)
	{
		this.canHide = _canHide;
	}

	// Token: 0x06009A51 RID: 39505 RVA: 0x003A5159 File Offset: 0x003A3359
	public Vector3 GetStartPosition()
	{
		return this.StartPosition;
	}

	// Token: 0x06009A52 RID: 39506 RVA: 0x003A5161 File Offset: 0x003A3361
	public Vector3 GetStartPositionOffset()
	{
		return this.StartOffset;
	}

	// Token: 0x06009A53 RID: 39507 RVA: 0x003A5169 File Offset: 0x003A3369
	public Vector3 GetEndPosition()
	{
		return this.EndPosition;
	}

	// Token: 0x06009A54 RID: 39508 RVA: 0x003A5171 File Offset: 0x003A3371
	public Vector3 GetEndPositionOffset()
	{
		return this.EndOffset;
	}

	// Token: 0x06009A55 RID: 39509 RVA: 0x003A5179 File Offset: 0x003A3379
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009A56 RID: 39510 RVA: 0x003A5181 File Offset: 0x003A3381
	public void SetVisible(bool _visible)
	{
		if (this.canHide)
		{
			base.gameObject.SetActive(_visible);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06009A57 RID: 39511 RVA: 0x003A51A4 File Offset: 0x003A33A4
	public Bounds GetBounds()
	{
		return this.mesh.bounds;
	}

	// Token: 0x06009A58 RID: 39512 RVA: 0x003A51B1 File Offset: 0x003A33B1
	public void Reset()
	{
		this.maxWireDip = 0.25f;
		this.wireRadius = 0.01f;
		this.pulseColor = Color.yellow;
	}

	// Token: 0x0400743C RID: 29756
	public const int cLayerMaskRayCast = 65537;

	// Token: 0x0400743D RID: 29757
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int NODE_COUNT = 15;

	// Token: 0x0400743E RID: 29758
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float BASE_WIRE_RADIUS = 0.01f;

	// Token: 0x0400743F RID: 29759
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float BASE_MIN_WIRE_DIP = 0f;

	// Token: 0x04007440 RID: 29760
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float BASE_MAX_WIRE_DIP = 0.25f;

	// Token: 0x04007441 RID: 29761
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Material BaseMaterial;

	// Token: 0x04007442 RID: 29762
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float maxWireDip = 0.25f;

	// Token: 0x04007443 RID: 29763
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float wireRadius = 0.01f;

	// Token: 0x04007444 RID: 29764
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 startPosition;

	// Token: 0x04007445 RID: 29765
	public Vector3 StartOffset;

	// Token: 0x04007446 RID: 29766
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 endPosition;

	// Token: 0x04007447 RID: 29767
	public Vector3 EndOffset;

	// Token: 0x04007448 RID: 29768
	public Color pulseColor = Color.yellow;

	// Token: 0x04007449 RID: 29769
	public Color wireColor = Color.black;

	// Token: 0x0400744A RID: 29770
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool canHide = true;

	// Token: 0x0400744B RID: 29771
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh mesh;

	// Token: 0x0400744C RID: 29772
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x0400744D RID: 29773
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshCollider meshCollider;

	// Token: 0x0400744E RID: 29774
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x0400744F RID: 29775
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevWireColor = Color.white;
}
