using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A5D RID: 2653
public class CharacterGazeController : MonoBehaviour
{
	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x06004FBB RID: 20411 RVA: 0x001E5654 File Offset: 0x001E3854
	public Transform LookAtTarget
	{
		get
		{
			if (this.lookAtTarget == null)
			{
				this.lookAtTarget = new GameObject("LookAtTarget").transform;
				this.lookAtTarget.parent = this.rootTransform;
				this.lookAtTarget.localPosition = this.lookAtTargetDefaultPosition;
				return this.lookAtTarget;
			}
			return this.lookAtTarget;
		}
	}

	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x06004FBC RID: 20412 RVA: 0x001E56B3 File Offset: 0x001E38B3
	// (set) Token: 0x06004FBD RID: 20413 RVA: 0x001E56BB File Offset: 0x001E38BB
	public Transform LookAtTransformOverride
	{
		get
		{
			return this.lookAtTransformOverride;
		}
		set
		{
			this.lookAtTransformOverride = value;
		}
	}

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x06004FBE RID: 20414 RVA: 0x001E56C4 File Offset: 0x001E38C4
	public GameRandom Random
	{
		get
		{
			if (this.random != null)
			{
				return this.random;
			}
			return this.random = GameRandomManager.Instance.CreateGameRandom();
		}
	}

	// Token: 0x06004FBF RID: 20415 RVA: 0x001E56F4 File Offset: 0x001E38F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.entityAlive = base.GetComponentInParent<EntityAlive>();
		this.eyeMaterial = new Material(this.eyeSkinnedMeshRenderer.material);
		if (this.eyeMaterial == null || this.eyeMaterial.shader.name != "Game/SDCS/Eye")
		{
			Debug.LogError("Eye Material is not valid");
			base.enabled = false;
			return;
		}
		this.eyeSkinnedMeshRenderer.material = this.eyeMaterial;
		this.gazeTimer = Time.realtimeSinceStartup + this.Random.RandomRange(0.25f, 2f);
	}

	// Token: 0x06004FC0 RID: 20416 RVA: 0x001E5791 File Offset: 0x001E3991
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		CharacterGazeController.instances.Add(this);
	}

	// Token: 0x06004FC1 RID: 20417 RVA: 0x001E579E File Offset: 0x001E399E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (CharacterGazeController.instances.Contains(this))
		{
			CharacterGazeController.instances.Remove(this);
		}
	}

	// Token: 0x06004FC2 RID: 20418 RVA: 0x001E57B9 File Offset: 0x001E39B9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		if (CharacterGazeController.instances.Contains(this))
		{
			CharacterGazeController.instances.Remove(this);
		}
		if (this.lookAtTarget != null)
		{
			global::UnityEngine.Object.Destroy(this.lookAtTarget.gameObject);
		}
	}

	// Token: 0x06004FC3 RID: 20419 RVA: 0x001E57F4 File Offset: 0x001E39F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.entityAlive != null)
		{
			if (this.entityAlive.emodel.IsRagdollActive)
			{
				return;
			}
			this.isDead = this.entityAlive.IsDead();
		}
		this.UpdateLookAtTarget();
		this.UpdateHeadRotation();
		this.UpdateEyeGaze();
	}

	// Token: 0x06004FC4 RID: 20420 RVA: 0x001E5848 File Offset: 0x001E3A48
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLookAtTarget()
	{
		if (this.lookAtTransformOverride != null)
		{
			this.lookAtCamera = true;
			this.LookAtTarget.position = this.lookAtTransformOverride.position;
			return;
		}
		CharacterGazeController characterGazeController = null;
		float num = float.PositiveInfinity;
		for (int i = CharacterGazeController.instances.Count - 1; i >= 0; i--)
		{
			CharacterGazeController characterGazeController2 = CharacterGazeController.instances[i];
			if (characterGazeController2 == null)
			{
				CharacterGazeController.instances.RemoveAt(i);
			}
			else if (characterGazeController2.enabled && characterGazeController2 != this)
			{
				Vector3 vector = characterGazeController2.headTransform.position - this.headTransform.position;
				float num2 = Vector3.Angle(this.neckTransform.forward, vector);
				float magnitude = vector.magnitude;
				if (num2 < this.eyeLookAtTargetAngle && magnitude <= this.maxLookAtDistance && num2 < num)
				{
					characterGazeController = characterGazeController2;
					num = num2;
				}
			}
		}
		if (characterGazeController != null)
		{
			this.lookAtCamera = true;
			this.LookAtTarget.position = characterGazeController.headTransform.position;
		}
		else
		{
			bool flag = this.entityAlive is EntityPlayerLocal;
			EntityPlayerLocal entityPlayerLocal;
			if (GameManager.Instance != null && GameManager.Instance.World != null)
			{
				entityPlayerLocal = GameManager.Instance.World.GetPrimaryPlayer();
			}
			else
			{
				entityPlayerLocal = null;
			}
			Vector3 vector2 = this.rootTransform.TransformPoint(this.lookAtTargetDefaultPosition);
			if (entityPlayerLocal != null && entityPlayerLocal.cameraTransform != null)
			{
				if (flag)
				{
					if (entityPlayerLocal.bFirstPersonView)
					{
						vector2 = entityPlayerLocal.cameraTransform.position;
					}
					else if (entityPlayerLocal.IsCameraFacingCharacter())
					{
						vector2 = entityPlayerLocal.cameraTransform.position;
					}
					else
					{
						vector2 = entityPlayerLocal.cameraTransform.position + entityPlayerLocal.cameraTransform.forward * 10f;
					}
				}
				else if (entityPlayerLocal.bFirstPersonView)
				{
					vector2 = entityPlayerLocal.cameraTransform.position;
				}
				else
				{
					vector2 = entityPlayerLocal.getHeadPosition();
				}
			}
			else if (Camera.main != null)
			{
				vector2 = Camera.main.transform.position;
			}
			Vector3 vector3 = vector2 - this.headTransform.position;
			float num3 = Vector3.Angle(this.neckTransform.forward, vector3);
			float magnitude2 = vector3.magnitude;
			this.lookAtCamera = false;
			if (num3 < this.headLookAtTargetAngle && magnitude2 <= this.maxLookAtDistance)
			{
				this.lookAtCamera = true;
			}
			else if (entityPlayerLocal != null && flag && !entityPlayerLocal.bFirstPersonView && !entityPlayerLocal.inventory.holdingItem.IsActionRunning(entityPlayerLocal.inventory.holdingItemData))
			{
				this.lookAtCamera = true;
			}
			if (this.lookAtCamera)
			{
				this.LookAtTarget.position = vector2;
			}
			else
			{
				this.LookAtTarget.localPosition = this.lookAtTargetDefaultPosition;
			}
		}
		if (Time.realtimeSinceStartup > this.gazeTimer)
		{
			this.lookatOffsetIndex = this.Random.RandomRange(0, this.lookatOffsets.Count);
			this.gazeTimer = Time.realtimeSinceStartup + this.Random.RandomRange(0.25f, 2f);
		}
	}

	// Token: 0x06004FC5 RID: 20421 RVA: 0x001E5B73 File Offset: 0x001E3D73
	public void SnapNextUpdate()
	{
		this.shouldSnapHeadNextUpdate = true;
		this.shouldSnapEyesNextUpdate = true;
	}

	// Token: 0x06004FC6 RID: 20422 RVA: 0x001E5B84 File Offset: 0x001E3D84
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateHeadRotation()
	{
		Vector3 normalized = (this.LookAtTarget.position - this.headTransform.position).normalized;
		float num = Vector3.Angle(this.neckTransform.forward, normalized);
		Vector3 normalized2 = this.neckTransform.InverseTransformDirection(normalized).normalized;
		float num2 = Mathf.Atan2(normalized2.x, normalized2.z) * 57.29578f;
		float num3 = Mathf.Atan2(normalized2.y, new Vector2(normalized2.x, normalized2.z).magnitude) * 57.29578f;
		num2 *= 0.5f;
		float num4 = num3 * 0.75f;
		Quaternion quaternion = Quaternion.AngleAxis(num2, this.neckTransform.up);
		Vector3 vector = quaternion * this.neckTransform.forward;
		Vector3 vector2 = quaternion * this.neckTransform.right;
		Vector3 normalized3 = (Quaternion.AngleAxis(-num4, vector2) * vector).normalized;
		if (num < this.headLookAtTargetAngle && !this.isDead)
		{
			Quaternion quaternion2 = Quaternion.FromToRotation(this.neckTransform.forward, normalized3);
			quaternion2 *= Quaternion.Euler(0f, 0f, -(num2 / this.headLookAtTargetAngle) * 15f);
			if (this.shouldSnapHeadNextUpdate)
			{
				this.currentHeadRotation = quaternion2;
				this.shouldSnapHeadNextUpdate = false;
			}
			else
			{
				this.currentHeadRotation = Quaternion.Slerp(this.currentHeadRotation, quaternion2, this.headRotationSpeed * Time.deltaTime);
			}
		}
		else if (this.shouldSnapHeadNextUpdate)
		{
			this.currentHeadRotation = Quaternion.identity;
			this.shouldSnapHeadNextUpdate = false;
		}
		else
		{
			this.currentHeadRotation = Quaternion.Slerp(this.currentHeadRotation, Quaternion.identity, this.headRotationSpeed * Time.deltaTime);
		}
		this.headTransform.rotation = this.currentHeadRotation * this.neckTransform.rotation;
	}

	// Token: 0x06004FC7 RID: 20423 RVA: 0x001E5D60 File Offset: 0x001E3F60
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEyeGaze()
	{
		Vector3 localPosition = this.LookAtTarget.localPosition;
		if (!this.lookAtCamera)
		{
			this.LookAtTarget.position += this.lookatOffsets[this.lookatOffsetIndex];
		}
		Vector3 vector = this.LookAtTarget.position - this.headTransform.TransformPoint(this.leftEyeLocalPosition);
		Vector3 vector2 = this.LookAtTarget.position - this.headTransform.TransformPoint(this.rightEyeLocalPosition);
		Quaternion quaternion = Quaternion.FromToRotation(this.leftEyeTransform.forward, vector);
		Quaternion quaternion2 = Quaternion.FromToRotation(this.rightEyeTransform.forward, vector2);
		float num = Mathf.Sin(Time.time * this.twitchSpeed) * 0.5f + 0.5f;
		if (this.isDead)
		{
			quaternion = Quaternion.identity;
			quaternion2 = Quaternion.identity;
		}
		if (this.shouldSnapEyesNextUpdate)
		{
			this.currentLeftEyeRotation = quaternion;
			this.currentRightEyeRotation = quaternion2;
			this.shouldSnapEyesNextUpdate = false;
		}
		else
		{
			this.currentLeftEyeRotation = Quaternion.Slerp(this.currentLeftEyeRotation, quaternion, this.eyeRotationSpeed * num * Time.deltaTime);
			this.currentRightEyeRotation = Quaternion.Slerp(this.currentRightEyeRotation, quaternion2, this.eyeRotationSpeed * num * Time.deltaTime);
		}
		this.eyeMaterial.SetVector("_LeftEyeRotation", new Vector4(-this.currentLeftEyeRotation.x, this.currentLeftEyeRotation.y, this.currentLeftEyeRotation.z, this.currentLeftEyeRotation.w));
		this.eyeMaterial.SetVector("_RightEyeRotation", new Vector4(-this.currentRightEyeRotation.x, this.currentRightEyeRotation.y, this.currentRightEyeRotation.z, this.currentRightEyeRotation.w));
		this.eyeMaterial.SetVector("_LeftEyePosition", Vector3.Scale(this.leftEyeLocalPosition, this.leftEyeTransform.lossyScale));
		this.eyeMaterial.SetVector("_RightEyePosition", Vector3.Scale(this.rightEyeLocalPosition, this.rightEyeTransform.lossyScale));
		this.LookAtTarget.localPosition = localPosition;
	}

	// Token: 0x06004FC8 RID: 20424 RVA: 0x001E5F8E File Offset: 0x001E418E
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void Cleanup()
	{
		CharacterGazeController.instances.Clear();
	}

	// Token: 0x04003D66 RID: 15718
	public static List<CharacterGazeController> instances = new List<CharacterGazeController>();

	// Token: 0x04003D67 RID: 15719
	public Transform leftEyeTransform;

	// Token: 0x04003D68 RID: 15720
	public Transform rightEyeTransform;

	// Token: 0x04003D69 RID: 15721
	public Vector3 leftEyeLocalPosition;

	// Token: 0x04003D6A RID: 15722
	public Vector3 rightEyeLocalPosition;

	// Token: 0x04003D6B RID: 15723
	public Transform rootTransform;

	// Token: 0x04003D6C RID: 15724
	public Transform neckTransform;

	// Token: 0x04003D6D RID: 15725
	public Transform headTransform;

	// Token: 0x04003D6E RID: 15726
	[Range(0f, 100f)]
	public float eyeRotationSpeed = 30f;

	// Token: 0x04003D6F RID: 15727
	[Range(0f, 100f)]
	public float headRotationSpeed = 7f;

	// Token: 0x04003D70 RID: 15728
	[Range(0f, 50f)]
	public float twitchSpeed = 25f;

	// Token: 0x04003D71 RID: 15729
	public float eyeLookAtTargetAngle = 35f;

	// Token: 0x04003D72 RID: 15730
	public float headLookAtTargetAngle = 75f;

	// Token: 0x04003D73 RID: 15731
	[Range(0f, 20f)]
	public float maxLookAtDistance = 5f;

	// Token: 0x04003D74 RID: 15732
	public SkinnedMeshRenderer eyeSkinnedMeshRenderer;

	// Token: 0x04003D75 RID: 15733
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material eyeMaterial;

	// Token: 0x04003D76 RID: 15734
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform lookAtTransformOverride;

	// Token: 0x04003D77 RID: 15735
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lookAtCamera;

	// Token: 0x04003D78 RID: 15736
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform lookAtTarget;

	// Token: 0x04003D79 RID: 15737
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameRandom random;

	// Token: 0x04003D7A RID: 15738
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> lookatOffsets = new List<Vector3>
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(-1f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(-2f, 0f, 0f),
		new Vector3(2f, 0f, 0f),
		new Vector3(0f, 0f, -1f),
		new Vector3(0f, 0f, 1f),
		new Vector3(0f, 0f, -2f),
		new Vector3(0f, 0f, 2f)
	};

	// Token: 0x04003D7B RID: 15739
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive entityAlive;

	// Token: 0x04003D7C RID: 15740
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion currentLeftEyeRotation;

	// Token: 0x04003D7D RID: 15741
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion currentRightEyeRotation;

	// Token: 0x04003D7E RID: 15742
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion currentHeadRotation;

	// Token: 0x04003D7F RID: 15743
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float gazeTimer;

	// Token: 0x04003D80 RID: 15744
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int lookatOffsetIndex;

	// Token: 0x04003D81 RID: 15745
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool shouldSnapHeadNextUpdate;

	// Token: 0x04003D82 RID: 15746
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool shouldSnapEyesNextUpdate;

	// Token: 0x04003D83 RID: 15747
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDead;

	// Token: 0x04003D84 RID: 15748
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 lookAtTargetDefaultPosition = new Vector3(0f, 1.7f, 10f);
}
