using System;
using UnityEngine;

// Token: 0x02001532 RID: 5426
public class BlendshapeTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AA56 RID: 43606 RVA: 0x003F8278 File Offset: 0x003F6478
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

	// Token: 0x0600AA57 RID: 43607 RVA: 0x003F82D8 File Offset: 0x003F64D8
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

	// Token: 0x04007E8C RID: 32396
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator myAnim;

	// Token: 0x04007E8D RID: 32397
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource myAudio;

	// Token: 0x04007E8E RID: 32398
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007E8F RID: 32399
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007E90 RID: 32400
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 200f;

	// Token: 0x04007E91 RID: 32401
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007E92 RID: 32402
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007E93 RID: 32403
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentAnim;

	// Token: 0x04007E94 RID: 32404
	public AudioClip[] audioClips;
}
