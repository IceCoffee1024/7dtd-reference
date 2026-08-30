using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012F6 RID: 4854
public class UpdateLight : MonoBehaviour
{
	// Token: 0x060099B8 RID: 39352 RVA: 0x003A3484 File Offset: 0x003A1684
	public void AddRendererNameToIgnore(string _name)
	{
		if (this.IgnoreNamedRenderersList == null)
		{
			this.IgnoreNamedRenderersList = new List<string>();
		}
		this.IgnoreNamedRenderersList.Add(_name);
	}

	// Token: 0x060099B9 RID: 39353 RVA: 0x003A34A8 File Offset: 0x003A16A8
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

	// Token: 0x060099BA RID: 39354 RVA: 0x003A3540 File Offset: 0x003A1740
	public static void SetTintColor(Transform _t, Color _color)
	{
		_t.GetComponentsInChildren<Renderer>(UpdateLight.rendererList);
		for (int i = 0; i < UpdateLight.rendererList.Count; i++)
		{
			UpdateLight.SetTintColor(UpdateLight.rendererList[i], _color);
		}
		UpdateLight.rendererList.Clear();
	}

	// Token: 0x060099BB RID: 39355 RVA: 0x003A3588 File Offset: 0x003A1788
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

	// Token: 0x060099BC RID: 39356 RVA: 0x003A35E3 File Offset: 0x003A17E3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x060099BD RID: 39357 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
	}

	// Token: 0x060099BE RID: 39358 RVA: 0x003A35F3 File Offset: 0x003A17F3
	public void Reset()
	{
		this.appliedLit = -1f;
	}

	// Token: 0x060099BF RID: 39359 RVA: 0x003A3600 File Offset: 0x003A1800
	public virtual void ManagerFirstUpdate()
	{
		base.gameObject.TryGetComponent<Entity>(out this.entity);
		this.currentLit = 0.5f;
		this.appliedLit = -1f;
		this.UpdateLighting(1f);
	}

	// Token: 0x060099C0 RID: 39360 RVA: 0x003A3638 File Offset: 0x003A1838
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

	// Token: 0x060099C1 RID: 39361 RVA: 0x003A3690 File Offset: 0x003A1890
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

	// Token: 0x060099C2 RID: 39362 RVA: 0x003A3724 File Offset: 0x003A1924
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

	// Token: 0x040073D7 RID: 29655
	public bool IsDynamicObject;

	// Token: 0x040073D8 RID: 29656
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentLit;

	// Token: 0x040073D9 RID: 29657
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLit;

	// Token: 0x040073DA RID: 29658
	public float appliedLit = -1f;

	// Token: 0x040073DB RID: 29659
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<string> IgnoreNamedRenderersList;

	// Token: 0x040073DC RID: 29660
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x040073DD RID: 29661
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<Renderer> rendererList = new List<Renderer>();

	// Token: 0x040073DE RID: 29662
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<MeshRenderer> meshRendererList = new List<MeshRenderer>();

	// Token: 0x040073DF RID: 29663
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<SkinnedMeshRenderer> skinnedMeshRendererList = new List<SkinnedMeshRenderer>();

	// Token: 0x040073E0 RID: 29664
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MaterialPropertyBlock props;
}
