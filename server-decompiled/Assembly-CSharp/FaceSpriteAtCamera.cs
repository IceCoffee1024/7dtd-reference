using System;
using UnityEngine;

// Token: 0x0200131C RID: 4892
public class FaceSpriteAtCamera : MonoBehaviour
{
	// Token: 0x06009A3A RID: 39482 RVA: 0x003A467B File Offset: 0x003A287B
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06009A3B RID: 39483 RVA: 0x003A467B File Offset: 0x003A287B
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06009A3C RID: 39484 RVA: 0x003A468A File Offset: 0x003A288A
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		this.mainCamera = null;
	}

	// Token: 0x06009A3D RID: 39485 RVA: 0x003A4694 File Offset: 0x003A2894
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
		}
		if (this.mainCamera != null)
		{
			base.transform.LookAt(this.mainCamera.transform.position, -Vector3.up);
		}
	}

	// Token: 0x0400743B RID: 29755
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mainCamera;
}
