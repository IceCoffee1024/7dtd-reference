using System;
using UnityEngine;

// Token: 0x02001319 RID: 4889
public class CloneToTransform : MonoBehaviour
{
	// Token: 0x06009A2B RID: 39467 RVA: 0x003A3FCE File Offset: 0x003A21CE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_transform = base.transform;
	}

	// Token: 0x06009A2C RID: 39468 RVA: 0x003A3FDC File Offset: 0x003A21DC
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

	// Token: 0x06009A2D RID: 39469 RVA: 0x003A4013 File Offset: 0x003A2213
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		this.DestroyClone();
	}

	// Token: 0x06009A2E RID: 39470 RVA: 0x003A401B File Offset: 0x003A221B
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

	// Token: 0x06009A2F RID: 39471 RVA: 0x003A4044 File Offset: 0x003A2244
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.m_clone)
		{
			this.m_clone.SetActive(true);
		}
	}

	// Token: 0x06009A30 RID: 39472 RVA: 0x003A405F File Offset: 0x003A225F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.m_clone)
		{
			this.m_clone.SetActive(false);
		}
	}

	// Token: 0x06009A31 RID: 39473 RVA: 0x003A407C File Offset: 0x003A227C
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

	// Token: 0x06009A32 RID: 39474 RVA: 0x003A42CC File Offset: 0x003A24CC
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

	// Token: 0x04007425 RID: 29733
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_transform;

	// Token: 0x04007426 RID: 29734
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject m_storage;

	// Token: 0x04007427 RID: 29735
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_storageTransform;

	// Token: 0x04007428 RID: 29736
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject m_clone;

	// Token: 0x04007429 RID: 29737
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_cloneTransform;

	// Token: 0x0400742A RID: 29738
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_lastCloneTarget;

	// Token: 0x0400742B RID: 29739
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_lastLocalPosition;

	// Token: 0x0400742C RID: 29740
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion m_lastLocalRotation;

	// Token: 0x0400742D RID: 29741
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_hasParentEntity;

	// Token: 0x0400742E RID: 29742
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_hasParentEntityLocal;

	// Token: 0x0400742F RID: 29743
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity m_parentEntity;
}
