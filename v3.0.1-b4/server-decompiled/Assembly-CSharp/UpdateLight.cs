using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012DC RID: 4828
public class UpdateLight : MonoBehaviour
{
	// Token: 0x060098BD RID: 39101 RVA: 0x0039E1EC File Offset: 0x0039C3EC
	public void AddRendererNameToIgnore(string _name)
	{
		if (this.IgnoreNamedRenderersList == null)
		{
			this.IgnoreNamedRenderersList = new List<string>();
		}
		this.IgnoreNamedRenderersList.Add(_name);
	}

	// Token: 0x060098BE RID: 39102 RVA: 0x0039E210 File Offset: 0x0039C410
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

	// Token: 0x060098BF RID: 39103 RVA: 0x0039E2A8 File Offset: 0x0039C4A8
	public static void SetTintColor(Transform _t, Color _color)
	{
		_t.GetComponentsInChildren<Renderer>(UpdateLight.rendererList);
		for (int i = 0; i < UpdateLight.rendererList.Count; i++)
		{
			UpdateLight.SetTintColor(UpdateLight.rendererList[i], _color);
		}
		UpdateLight.rendererList.Clear();
	}

	// Token: 0x060098C0 RID: 39104 RVA: 0x0039E2F0 File Offset: 0x0039C4F0
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

	// Token: 0x060098C1 RID: 39105 RVA: 0x0039E34B File Offset: 0x0039C54B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x060098C2 RID: 39106 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
	}

	// Token: 0x060098C3 RID: 39107 RVA: 0x0039E35B File Offset: 0x0039C55B
	public void Reset()
	{
		this.appliedLit = -1f;
	}

	// Token: 0x060098C4 RID: 39108 RVA: 0x0039E368 File Offset: 0x0039C568
	public virtual void ManagerFirstUpdate()
	{
		base.gameObject.TryGetComponent<Entity>(out this.entity);
		this.currentLit = 0.5f;
		this.appliedLit = -1f;
		this.UpdateLighting(1f);
	}

	// Token: 0x060098C5 RID: 39109 RVA: 0x0039E3A0 File Offset: 0x0039C5A0
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

	// Token: 0x060098C6 RID: 39110 RVA: 0x0039E3F8 File Offset: 0x0039C5F8
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

	// Token: 0x060098C7 RID: 39111 RVA: 0x0039E48C File Offset: 0x0039C68C
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

	// Token: 0x040072FF RID: 29439
	public bool IsDynamicObject;

	// Token: 0x04007300 RID: 29440
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentLit;

	// Token: 0x04007301 RID: 29441
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLit;

	// Token: 0x04007302 RID: 29442
	public float appliedLit = -1f;

	// Token: 0x04007303 RID: 29443
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<string> IgnoreNamedRenderersList;

	// Token: 0x04007304 RID: 29444
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x04007305 RID: 29445
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<Renderer> rendererList = new List<Renderer>();

	// Token: 0x04007306 RID: 29446
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<MeshRenderer> meshRendererList = new List<MeshRenderer>();

	// Token: 0x04007307 RID: 29447
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<SkinnedMeshRenderer> skinnedMeshRendererList = new List<SkinnedMeshRenderer>();

	// Token: 0x04007308 RID: 29448
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static MaterialPropertyBlock props;
}
