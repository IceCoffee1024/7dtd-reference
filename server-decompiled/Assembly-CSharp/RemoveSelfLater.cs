using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001491 RID: 5265
public class RemoveSelfLater : MonoBehaviour
{
	// Token: 0x0600A53F RID: 42303 RVA: 0x003D7DF3 File Offset: 0x003D5FF3
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		base.StartCoroutine(this.remove());
	}

	// Token: 0x0600A540 RID: 42304 RVA: 0x003D7E02 File Offset: 0x003D6002
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator remove()
	{
		yield return new WaitForSeconds(this.WaitSeconds);
		global::UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04007B1C RID: 31516
	public float WaitSeconds = 1f;
}
