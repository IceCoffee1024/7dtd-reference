using System;
using UnityEngine;

// Token: 0x02001554 RID: 5460
public class BlendshapeTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AB09 RID: 43785 RVA: 0x003FD0CC File Offset: 0x003FB2CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.myAnim = base.GetComponent<Animator>();
		this.myAudio = base.GetComponent<AudioSource>();
		if (this.myAnim != null)
		{
			this.maxLayers = this.myAnim.layerCount;
			Debug.Log("Number of layers in controller is " + this.maxLayers.ToString());
		}
	}

	// Token: 0x0600AB0A RID: 43786 RVA: 0x003FD12C File Offset: 0x003FB32C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space) && this.myAudio != null)
		{
			this.currentAnim++;
			if (this.currentAnim == this.maxLayers)
			{
				this.currentAnim = 0;
				for (int i = 1; i < this.maxLayers - 1; i++)
				{
					this.myAnim.SetLayerWeight(i, 0f);
				}
			}
			Debug.Log("Current Layer is: " + this.currentAnim.ToString());
			this.myAudio.clip = this.audioClips[this.currentAnim];
			this.myAudio.Play();
			this.myAnim.SetLayerWeight(this.currentAnim, 1f);
			this.myAnim.SetTrigger("RestartAnim");
		}
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.Rotate(0f, this.turnRate * Time.deltaTime * -1f, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.Rotate(0f, this.turnRate * Time.deltaTime, 0f);
		}
		Input.GetKeyUp(KeyCode.W);
		Input.GetKeyUp(KeyCode.S);
		Input.GetKeyUp(KeyCode.E);
	}

	// Token: 0x04007F0F RID: 32527
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator myAnim;

	// Token: 0x04007F10 RID: 32528
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource myAudio;

	// Token: 0x04007F11 RID: 32529
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007F12 RID: 32530
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007F13 RID: 32531
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 200f;

	// Token: 0x04007F14 RID: 32532
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007F15 RID: 32533
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007F16 RID: 32534
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentAnim;

	// Token: 0x04007F17 RID: 32535
	public AudioClip[] audioClips;
}
