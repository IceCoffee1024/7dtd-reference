using System;
using UnityEngine;

// Token: 0x02001566 RID: 5478
public class BlendshapeTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AB61 RID: 43873 RVA: 0x003FF4D0 File Offset: 0x003FD6D0
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

	// Token: 0x0600AB62 RID: 43874 RVA: 0x003FF530 File Offset: 0x003FD730
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

	// Token: 0x04007F50 RID: 32592
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator myAnim;

	// Token: 0x04007F51 RID: 32593
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource myAudio;

	// Token: 0x04007F52 RID: 32594
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007F53 RID: 32595
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007F54 RID: 32596
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 200f;

	// Token: 0x04007F55 RID: 32597
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007F56 RID: 32598
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007F57 RID: 32599
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentAnim;

	// Token: 0x04007F58 RID: 32600
	public AudioClip[] audioClips;
}
