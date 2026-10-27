using System;
using UnityEngine;

// Token: 0x02001365 RID: 4965
public class CloneToTransform : MonoBehaviour
{
	// Token: 0x06009C2A RID: 39978 RVA: 0x003B01E6 File Offset: 0x003AE3E6
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_transform = base.transform;
	}

	// Token: 0x06009C2B RID: 39979 RVA: 0x003B01F4 File Offset: 0x003AE3F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_parentEntity = base.GetComponentInParent<Entity>();
		if (this.m_parentEntity != null)
		{
			this.m_hasParentEntity = true;
		}
		if (this.m_parentEntity is EntityPlayerLocal)
		{
			this.m_hasParentEntityLocal = true;
		}
	}

	// Token: 0x06009C2C RID: 39980 RVA: 0x003B022B File Offset: 0x003AE42B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		this.DestroyClone();
	}

	// Token: 0x06009C2D RID: 39981 RVA: 0x003B0233 File Offset: 0x003AE433
	[PublicizedFrom(EAccessModifier.Private)]
	public void DestroyClone()
	{
		if (!this.m_clone)
		{
			return;
		}
		this.m_cloneTransform = null;
		global::UnityEngine.Object.Destroy(this.m_clone);
		this.m_clone = null;
	}

	// Token: 0x06009C2E RID: 39982 RVA: 0x003B025C File Offset: 0x003AE45C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.m_clone)
		{
			this.m_clone.SetActive(true);
		}
	}

	// Token: 0x06009C2F RID: 39983 RVA: 0x003B0277 File Offset: 0x003AE477
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.m_clone)
		{
			this.m_clone.SetActive(false);
		}
	}

	// Token: 0x06009C30 RID: 39984 RVA: 0x003B0294 File Offset: 0x003AE494
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (!this.m_storage)
		{
			this.m_storage = global::UnityEngine.Object.Instantiate<GameObject>(base.gameObject, this.m_transform, true);
			this.m_storageTransform = this.m_storage.transform;
			this.m_storageTransform.name = this.m_transform.name + "(CloneToTransform)";
			this.m_storage.SetActive(false);
			CloneToTransform cloneToTransform;
			if (this.m_storage.TryGetComponent<CloneToTransform>(out cloneToTransform))
			{
				global::UnityEngine.Object.Destroy(cloneToTransform);
			}
			foreach (object obj in this.m_transform)
			{
				Transform transform = (Transform)obj;
				if (!(transform == this.m_storageTransform))
				{
					global::UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
			foreach (Component component in base.GetComponents<Component>())
			{
				if (!(component == this) && !(component is Transform))
				{
					global::UnityEngine.Object.Destroy(component);
				}
			}
		}
		Transform transform2 = null;
		if ((this.m_hasParentEntityLocal || !this.m_hasParentEntity) && Camera.main)
		{
			transform2 = Camera.main.transform;
		}
		else if (this.m_parentEntity != null && this.m_parentEntity.emodel != null)
		{
			transform2 = this.m_parentEntity.emodel.GetModelTransformParent();
		}
		if (!transform2)
		{
			this.DestroyClone();
			this.m_lastCloneTarget = null;
			return;
		}
		if (!this.m_clone)
		{
			this.m_lastCloneTarget = transform2;
			this.m_clone = global::UnityEngine.Object.Instantiate<GameObject>(this.m_storage, transform2, true);
			this.m_cloneTransform = this.m_clone.transform;
			this.m_cloneTransform.name = this.m_transform.name + "(clone)";
			this.m_clone.SetActive(true);
			CloneToTransform cloneToTransform2;
			if (this.m_clone.TryGetComponent<CloneToTransform>(out cloneToTransform2))
			{
				global::UnityEngine.Object.Destroy(cloneToTransform2);
			}
		}
		if (this.m_lastCloneTarget != transform2)
		{
			this.m_lastCloneTarget = transform2;
			this.m_cloneTransform.parent = transform2;
			this.m_lastLocalPosition = default(Vector3);
			this.m_lastLocalRotation = default(Quaternion);
		}
		this.CheckTransform();
	}

	// Token: 0x06009C31 RID: 39985 RVA: 0x003B04E4 File Offset: 0x003AE6E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckTransform()
	{
		if (this.m_lastLocalPosition == this.m_transform.localPosition && this.m_lastLocalRotation == this.m_transform.localRotation)
		{
			return;
		}
		this.m_lastLocalPosition = this.m_transform.localPosition;
		this.m_lastLocalRotation = this.m_transform.localRotation;
		this.m_cloneTransform.SetPositionAndRotation(this.m_transform.position, this.m_transform.rotation);
	}

	// Token: 0x040075B9 RID: 30137
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_transform;

	// Token: 0x040075BA RID: 30138
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject m_storage;

	// Token: 0x040075BB RID: 30139
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_storageTransform;

	// Token: 0x040075BC RID: 30140
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject m_clone;

	// Token: 0x040075BD RID: 30141
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_cloneTransform;

	// Token: 0x040075BE RID: 30142
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_lastCloneTarget;

	// Token: 0x040075BF RID: 30143
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_lastLocalPosition;

	// Token: 0x040075C0 RID: 30144
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion m_lastLocalRotation;

	// Token: 0x040075C1 RID: 30145
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_hasParentEntity;

	// Token: 0x040075C2 RID: 30146
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_hasParentEntityLocal;

	// Token: 0x040075C3 RID: 30147
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity m_parentEntity;
}
