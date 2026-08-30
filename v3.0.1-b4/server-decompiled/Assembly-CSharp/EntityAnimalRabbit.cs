using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000480 RID: 1152
[Preserve]
public class EntityAnimalRabbit : EntityAnimal
{
	// Token: 0x060023CB RID: 9163 RVA: 0x000D8F3C File Offset: 0x000D713C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		if (component)
		{
			component.center = new Vector3(0f, 0.15f, 0f);
			component.size = new Vector3(0.4f, 0.4f, 0.4f);
		}
		base.Awake();
		Transform transform = base.transform.Find("Graphics/BlobShadowProjector");
		if (transform)
		{
			transform.gameObject.SetActive(false);
		}
	}

	// Token: 0x060023CC RID: 9164 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsAttackValid()
	{
		return false;
	}

	// Token: 0x060023CD RID: 9165 RVA: 0x000D8FBC File Offset: 0x000D71BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitLocalActivationCommands(Action<EntityActivationCommand> _addCallback)
	{
		_addCallback(new EntityActivationCommand("grab", "hand", null, null));
	}

	// Token: 0x060023CE RID: 9166 RVA: 0x000D8FD5 File Offset: 0x000D71D5
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		return !base.CommandIs(_commandName, "grab") && base.AllowActivationCommand(_commandName, _playerFocusing);
	}

	// Token: 0x060023CF RID: 9167 RVA: 0x000D8FF0 File Offset: 0x000D71F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityActivated(EntityActivationCommand _command, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_command.commandId, "grab"))
		{
			ItemStack itemStack = new ItemStack(ItemClass.GetItem("wildChicken", false), 1);
			_playerFocusing.inventory.SetItem(_playerFocusing.inventory.holdingItemIdx, itemStack);
			base.Collect(_playerFocusing.entityId);
		}
	}

	// Token: 0x060023D0 RID: 9168 RVA: 0x000D904A File Offset: 0x000D724A
	public override string GetActivationText()
	{
		return string.Format(Localization.Get("overlayChickenGrab", false, null), Array.Empty<object>());
	}

	// Token: 0x04001973 RID: 6515
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly FastTags<TagGroup.Global> ChickenTag = FastTags<TagGroup.Global>.Parse("chicken");
}
