using System;
using UnityEngine;

// Token: 0x02001353 RID: 4947
public class CloneToTransform : MonoBehaviour
{
	// Token: 0x06009BD2 RID: 39890 RVA: 0x003ADDE2 File Offset: 0x003ABFE2
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_transform = base.transform;
	}

	// Token: 0x06009BD3 RID: 39891 RVA: 0x003ADDF0 File Offset: 0x003ABFF0
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

	// Token: 0x06009BD4 RID: 39892 RVA: 0x003ADE27 File Offset: 0x003AC027
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		this.DestroyClone();
	}

	// Token: 0x06009BD5 RID: 39893 RVA: 0x003ADE2F File Offset: 0x003AC02F
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

	// Token: 0x06009BD6 RID: 39894 RVA: 0x003ADE58 File Offset: 0x003AC058
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.m_clone)
		{
			this.m_clone.SetActive(true);
		}
	}

	// Token: 0x06009BD7 RID: 39895 RVA: 0x003ADE73 File Offset: 0x003AC073
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.m_clone)
		{
			this.m_clone.SetActive(false);
		}
	}

	// Token: 0x06009BD8 RID: 39896 RVA: 0x003ADE90 File Offset: 0x003AC090
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

	// Token: 0x06009BD9 RID: 39897 RVA: 0x003AE0E0 File Offset: 0x003AC2E0
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

	// Token: 0x04007578 RID: 30072
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_transform;

	// Token: 0x04007579 RID: 30073
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject m_storage;

	// Token: 0x0400757A RID: 30074
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_storageTransform;

	// Token: 0x0400757B RID: 30075
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject m_clone;

	// Token: 0x0400757C RID: 30076
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_cloneTransform;

	// Token: 0x0400757D RID: 30077
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_lastCloneTarget;

	// Token: 0x0400757E RID: 30078
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_lastLocalPosition;

	// Token: 0x0400757F RID: 30079
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion m_lastLocalRotation;

	// Token: 0x04007580 RID: 30080
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_hasParentEntity;

	// Token: 0x04007581 RID: 30081
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_hasParentEntityLocal;

	// Token: 0x04007582 RID: 30082
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity m_parentEntity;
}
