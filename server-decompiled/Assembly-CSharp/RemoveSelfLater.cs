using System;
using System.Collections;
using UnityEngine;

// Token: 0x020014C3 RID: 5315
public class RemoveSelfLater : MonoBehaviour
{
	// Token: 0x0600A645 RID: 42565 RVA: 0x003DEE83 File Offset: 0x003DD083
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		base.StartCoroutine(this.remove());
	}

	// Token: 0x0600A646 RID: 42566 RVA: 0x003DEE92 File Offset: 0x003DD092
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator remove()
	{
		yield return new WaitForSeconds(this.WaitSeconds);
		global::UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04007BD8 RID: 31704
	public float WaitSeconds = 1f;
}
