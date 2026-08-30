using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001476 RID: 5238
public class RemoveSelfLater : MonoBehaviour
{
	// Token: 0x0600A442 RID: 42050 RVA: 0x003D2AAF File Offset: 0x003D0CAF
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		base.StartCoroutine(this.remove());
	}

	// Token: 0x0600A443 RID: 42051 RVA: 0x003D2ABE File Offset: 0x003D0CBE
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator remove()
	{
		yield return new WaitForSeconds(this.WaitSeconds);
		global::UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04007A43 RID: 31299
	public float WaitSeconds = 1f;
}
