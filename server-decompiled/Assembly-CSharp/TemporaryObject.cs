using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000559 RID: 1369
public class TemporaryObject : MonoBehaviour
{
	// Token: 0x06002C73 RID: 11379 RVA: 0x0011AAE8 File Offset: 0x00118CE8
	public void SetLife(float _life)
	{
		this.life = _life;
		float num = Utils.FastMax(_life - 1f, 0.1f);
		float num2 = 0.1f;
		ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			float duration = componentsInChildren[i].main.duration;
			if (duration > num2)
			{
				num2 = duration;
			}
		}
		float num3 = num / num2;
		float num4 = num2 * 0.5f;
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			float duration2 = main.duration;
			if (duration2 >= num4)
			{
				particleSystem.Stop(false);
				main.duration = duration2 * num3;
				ParticleSystem.MinMaxCurve startDelay = main.startDelay;
				ParticleSystemCurveMode particleSystemCurveMode = startDelay.mode;
				if (particleSystemCurveMode != ParticleSystemCurveMode.Constant)
				{
					if (particleSystemCurveMode != ParticleSystemCurveMode.TwoConstants)
					{
						startDelay.curveMultiplier *= num3;
					}
					else
					{
						startDelay.constantMin *= num3;
						startDelay.constantMax *= num3;
					}
				}
				else
				{
					startDelay.constant *= num3;
				}
				main.startDelay = startDelay;
				ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
				particleSystemCurveMode = startLifetime.mode;
				if (particleSystemCurveMode != ParticleSystemCurveMode.Constant)
				{
					if (particleSystemCurveMode != ParticleSystemCurveMode.TwoConstants)
					{
						startLifetime.curveMultiplier *= num3;
					}
					else
					{
						startLifetime.constantMin *= num3;
						startLifetime.constantMax *= num3;
					}
				}
				else
				{
					startLifetime.constant *= num3;
				}
				main.startLifetime = startLifetime;
				particleSystem.Play(false);
			}
		}
	}

	// Token: 0x06002C74 RID: 11380 RVA: 0x0011AC76 File Offset: 0x00118E76
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.coroutine = base.StartCoroutine(this.DestroyLater());
	}

	// Token: 0x06002C75 RID: 11381 RVA: 0x0011AC8A File Offset: 0x00118E8A
	public void Restart()
	{
		base.gameObject.SetActive(true);
		if (this.coroutine != null)
		{
			base.StopCoroutine(this.coroutine);
		}
		this.Start();
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x0011ACB2 File Offset: 0x00118EB2
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator DestroyLater()
	{
		yield return new WaitForSeconds(this.life);
		if (this.destroyMaterials)
		{
			Utils.CleanupMaterialsOfRenderers<Renderer[]>(base.transform.GetComponentsInChildren<Renderer>());
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x0011ACC4 File Offset: 0x00118EC4
	[Conditional("DEBUG_TEMPOBJ")]
	public void LogTO(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} TemporaryObject {1}, id {2}, life {3}, {4}", new object[]
		{
			GameManager.frameCount,
			base.gameObject.GetGameObjectPath(),
			base.gameObject.GetInstanceID(),
			this.life,
			_format
		});
		Log.Warning(_format, _args);
	}

	// Token: 0x04002257 RID: 8791
	public float life = 2f;

	// Token: 0x04002258 RID: 8792
	public bool destroyMaterials;

	// Token: 0x04002259 RID: 8793
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Coroutine coroutine;
}
