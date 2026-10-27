using System;
using UnityEngine;

// Token: 0x02001368 RID: 4968
public class FaceSpriteAtCamera : MonoBehaviour
{
	// Token: 0x06009C39 RID: 39993 RVA: 0x003B0893 File Offset: 0x003AEA93
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06009C3A RID: 39994 RVA: 0x003B0893 File Offset: 0x003AEA93
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06009C3B RID: 39995 RVA: 0x003B08A2 File Offset: 0x003AEAA2
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		this.mainCamera = null;
	}

	// Token: 0x06009C3C RID: 39996 RVA: 0x003B08AC File Offset: 0x003AEAAC
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

	// Token: 0x040075CF RID: 30159
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mainCamera;
}
