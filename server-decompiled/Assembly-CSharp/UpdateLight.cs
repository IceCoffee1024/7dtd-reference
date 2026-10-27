using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001328 RID: 4904
public class UpdateLight : MonoBehaviour
{
	// Token: 0x06009ABC RID: 39612 RVA: 0x003AA438 File Offset: 0x003A8638
	public void AddRendererNameToIgnore(string _name)
	{
		if (this.IgnoreNamedRenderersList == null)
		{
			this.IgnoreNamedRenderersList = new List<string>();
		}
		this.IgnoreNamedRenderersList.Add(_name);
	}

	// Token: 0x06009ABD RID: 39613 RVA: 0x003AA45C File Offset: 0x003A865C
	public void SetTintColorForItem(Vector3 _color)
	{
		Color color = new Color(_color.x, _color.y, _color.z, 1f);
		base.gameObject.GetComponentsInChildren<Renderer>(UpdateLight.rendererList);
		for (int i = 0; i < UpdateLight.rendererList.Count; i++)
		{
			Renderer renderer = UpdateLight.rendererList[i];
			if (renderer && (this.IgnoreNamedRenderersList == null || !this.IgnoreNamedRenderersList.ContainsCaseInsensitive(renderer.gameObject.name)))
			{
				UpdateLight.SetTintColor(renderer, color);
			}
		}
		UpdateLight.rendererList.Clear();
	}

	// Token: 0x06009ABE RID: 39614 RVA: 0x003AA4F4 File Offset: 0x003A86F4
	public static void SetTintColor(Transform _t, Color _color)
	{
		_t.GetComponentsInChildren<Renderer>(UpdateLight.rendererList);
		for (int i = 0; i < UpdateLight.rendererList.Count; i++)
		{
			UpdateLight.SetTintColor(UpdateLight.rendererList[i], _color);
		}
		UpdateLight.rendererList.Clear();
	}

	// Token: 0x06009ABF RID: 39615 RVA: 0x003AA53C File Offset: 0x003A873C
	public static void SetTintColor(Renderer _r, Color _color)
	{
		Material[] materials = _r.materials;
		if (materials != null)
		{
			foreach (Material material in materials)
			{
				if (material != null)
				{
					material.SetColor("_Color", _color);
					material.SetColor("TintColor", _color);
					material.SetVector("TintColor", _color);
				}
			}
		}
	}

	// Token: 0x06009AC0 RID: 39616 RVA: 0x003AA597 File Offset: 0x003A8797
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x06009AC1 RID: 39617 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
	}

	// Token: 0x06009AC2 RID: 39618 RVA: 0x003AA5A7 File Offset: 0x003A87A7
	public void Reset()
	{
		this.appliedLit = -1f;
	}

	// Token: 0x06009AC3 RID: 39619 RVA: 0x003AA5B4 File Offset: 0x003A87B4
	public virtual void ManagerFirstUpdate()
	{
		base.gameObject.TryGetComponent<Entity>(out this.entity);
		this.currentLit = 0.5f;
		this.appliedLit = -1f;
		this.UpdateLighting(1f);
	}

	// Token: 0x06009AC4 RID: 39620 RVA: 0x003AA5EC File Offset: 0x003A87EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFloatProp<T>(string varName, float value, List<T> _rends) where T : Renderer
	{
		_rends[0].GetPropertyBlock(UpdateLight.props);
		UpdateLight.props.SetFloat(varName, value);
		for (int i = 0; i < _rends.Count; i++)
		{
			_rends[i].SetPropertyBlock(UpdateLight.props);
		}
	}

	// Token: 0x06009AC5 RID: 39621 RVA: 0x003AA644 File Offset: 0x003A8844
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLit(float _lit)
	{
		this.appliedLit = _lit;
		GameObject gameObject = base.gameObject;
		gameObject.GetComponentsInChildren<MeshRenderer>(true, UpdateLight.meshRendererList);
		gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true, UpdateLight.skinnedMeshRendererList);
		if (UpdateLight.props == null)
		{
			UpdateLight.props = new MaterialPropertyBlock();
		}
		if (UpdateLight.meshRendererList.Count > 0)
		{
			this.SetFloatProp<MeshRenderer>("_MacroAO", _lit, UpdateLight.meshRendererList);
			UpdateLight.meshRendererList.Clear();
		}
		if (UpdateLight.skinnedMeshRendererList.Count > 0)
		{
			this.SetFloatProp<SkinnedMeshRenderer>("_MacroAO", _lit, UpdateLight.skinnedMeshRendererList);
			UpdateLight.skinnedMeshRendererList.Clear();
		}
	}

	// Token: 0x06009AC6 RID: 39622 RVA: 0x003AA6D8 File Offset: 0x003A88D8
	public void UpdateLighting(float _step)
	{
		if (this.entity)
		{
			this.targetLit = this.entity.GetLightBrightness();
		}
		else
		{
			this.targetLit = 1f;
			Vector3i vector3i = World.worldToBlockPos(base.transform.position + Origin.position);
			if (vector3i.y < 255)
			{
				IChunk chunkFromWorldPos = GameManager.Instance.World.GetChunkFromWorldPos(vector3i);
				if (chunkFromWorldPos != null)
				{
					float num = (float)chunkFromWorldPos.GetLight(vector3i.x, vector3i.y, vector3i.z, Chunk.LIGHT_TYPE.SUN);
					float num2 = (float)chunkFromWorldPos.GetLight(vector3i.x, vector3i.y + 1, vector3i.z, Chunk.LIGHT_TYPE.SUN);
					this.targetLit = Utils.FastMax(num, num2);
					this.targetLit /= 15f;
				}
			}
		}
		this.currentLit = Mathf.MoveTowards(this.currentLit, this.targetLit, _step);
		if (this.currentLit != this.appliedLit)
		{
			this.ApplyLit(this.currentLit);
		}
	}

	// Token: 0x04007493 RID: 29843
	public bool IsDynamicObject;

	// Token: 0x04007494 RID: 29844
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentLit;

	// Token: 0x04007495 RID: 29845
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLit;

	// Token: 0x04007496 RID: 29846
	public float appliedLit = -1f;

	// Token: 0x04007497 RID: 29847
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<string> IgnoreNamedRenderersList;

	// Token: 0x04007498 RID: 29848
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x04007499 RID: 29849
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<Renderer> rendererList = new List<Renderer>();

	// Token: 0x0400749A RID: 29850
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<MeshRenderer> meshRendererList = new List<MeshRenderer>();

	// Token: 0x0400749B RID: 29851
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<SkinnedMeshRenderer> skinnedMeshRendererList = new List<SkinnedMeshRenderer>();

	// Token: 0x0400749C RID: 29852
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MaterialPropertyBlock props;
}
