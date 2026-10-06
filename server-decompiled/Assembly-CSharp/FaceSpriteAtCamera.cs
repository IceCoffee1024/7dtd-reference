using System;
using UnityEngine;

// Token: 0x02001356 RID: 4950
public class FaceSpriteAtCamera : MonoBehaviour
{
	// Token: 0x06009BE1 RID: 39905 RVA: 0x003AE48F File Offset: 0x003AC68F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06009BE2 RID: 39906 RVA: 0x003AE48F File Offset: 0x003AC68F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06009BE3 RID: 39907 RVA: 0x003AE49E File Offset: 0x003AC69E
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		this.mainCamera = null;
	}

	// Token: 0x06009BE4 RID: 39908 RVA: 0x003AE4A8 File Offset: 0x003AC6A8
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

	// Token: 0x0400758E RID: 30094
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mainCamera;
}
