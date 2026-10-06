using System;
using System.Collections;
using UnityEngine;

// Token: 0x020014B1 RID: 5297
public class RemoveSelfLater : MonoBehaviour
{
	// Token: 0x0600A5ED RID: 42477 RVA: 0x003DCA7F File Offset: 0x003DAC7F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		base.StartCoroutine(this.remove());
	}

	// Token: 0x0600A5EE RID: 42478 RVA: 0x003DCA8E File Offset: 0x003DAC8E
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator remove()
	{
		yield return new WaitForSeconds(this.WaitSeconds);
		global::UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04007B97 RID: 31639
	public float WaitSeconds = 1f;
}
