using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001316 RID: 4886
public class UpdateLight : MonoBehaviour
{
	// Token: 0x06009A64 RID: 39524 RVA: 0x003A8034 File Offset: 0x003A6234
	public void AddRendererNameToIgnore(string _name)
	{
		if (this.IgnoreNamedRenderersList == null)
		{
			this.IgnoreNamedRenderersList = new List<string>();
		}
		this.IgnoreNamedRenderersList.Add(_name);
	}

	// Token: 0x06009A65 RID: 39525 RVA: 0x003A8058 File Offset: 0x003A6258
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

	// Token: 0x06009A66 RID: 39526 RVA: 0x003A80F0 File Offset: 0x003A62F0
	public static void SetTintColor(Transform _t, Color _color)
	{
		_t.GetComponentsInChildren<Renderer>(UpdateLight.rendererList);
		for (int i = 0; i < UpdateLight.rendererList.Count; i++)
		{
			UpdateLight.SetTintColor(UpdateLight.rendererList[i], _color);
		}
		UpdateLight.rendererList.Clear();
	}

	// Token: 0x06009A67 RID: 39527 RVA: 0x003A8138 File Offset: 0x003A6338
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

	// Token: 0x06009A68 RID: 39528 RVA: 0x003A8193 File Offset: 0x003A6393
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x06009A69 RID: 39529 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
	}

	// Token: 0x06009A6A RID: 39530 RVA: 0x003A81A3 File Offset: 0x003A63A3
	public void Reset()
	{
		this.appliedLit = -1f;
	}

	// Token: 0x06009A6B RID: 39531 RVA: 0x003A81B0 File Offset: 0x003A63B0
	public virtual void ManagerFirstUpdate()
	{
		base.gameObject.TryGetComponent<Entity>(out this.entity);
		this.currentLit = 0.5f;
		this.appliedLit = -1f;
		this.UpdateLighting(1f);
	}

	// Token: 0x06009A6C RID: 39532 RVA: 0x003A81E8 File Offset: 0x003A63E8
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

	// Token: 0x06009A6D RID: 39533 RVA: 0x003A8240 File Offset: 0x003A6440
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

	// Token: 0x06009A6E RID: 39534 RVA: 0x003A82D4 File Offset: 0x003A64D4
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

	// Token: 0x04007452 RID: 29778
	public bool IsDynamicObject;

	// Token: 0x04007453 RID: 29779
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentLit;

	// Token: 0x04007454 RID: 29780
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLit;

	// Token: 0x04007455 RID: 29781
	public float appliedLit = -1f;

	// Token: 0x04007456 RID: 29782
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<string> IgnoreNamedRenderersList;

	// Token: 0x04007457 RID: 29783
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x04007458 RID: 29784
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<Renderer> rendererList = new List<Renderer>();

	// Token: 0x04007459 RID: 29785
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<MeshRenderer> meshRendererList = new List<MeshRenderer>();

	// Token: 0x0400745A RID: 29786
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<SkinnedMeshRenderer> skinnedMeshRendererList = new List<SkinnedMeshRenderer>();

	// Token: 0x0400745B RID: 29787
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MaterialPropertyBlock props;
}
