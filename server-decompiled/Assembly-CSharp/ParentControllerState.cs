using System;

// Token: 0x02001108 RID: 4360
public class ParentControllerState
{
	// Token: 0x06008A57 RID: 35415 RVA: 0x0034AF88 File Offset: 0x00349188
	public ParentControllerState(XUiController parentController)
	{
		if (parentController == null)
		{
			return;
		}
		if (parentController.ViewComponent == null)
		{
			this.m_parentControllers = new XUiController[parentController.Children.Count];
			for (int i = 0; i < parentController.Children.Count; i++)
			{
				this.m_parentControllers[i] = parentController.Children[i];
			}
		}
		else
		{
			this.m_parentControllers = new XUiController[] { parentController };
		}
		this.m_isVisible = new bool[this.m_parentControllers.Length];
		this.m_isEscClosable = new bool[this.m_parentControllers.Length];
		for (int j = 0; j < this.m_parentControllers.Length; j++)
		{
			this.m_isVisible[j] = this.m_parentControllers[j].ViewComponent.IsVisible;
			this.m_isEscClosable[j] = this.m_parentControllers[j].WindowGroup.isEscClosable;
		}
	}

	// Token: 0x06008A58 RID: 35416 RVA: 0x0034B068 File Offset: 0x00349268
	public void Hide()
	{
		if (this.m_parentControllers == null)
		{
			return;
		}
		for (int i = 0; i < this.m_parentControllers.Length; i++)
		{
			this.m_parentControllers[i].ViewComponent.IsVisible = false;
			this.m_parentControllers[i].WindowGroup.isEscClosable = false;
		}
	}

	// Token: 0x06008A59 RID: 35417 RVA: 0x0034B0B8 File Offset: 0x003492B8
	public void Restore()
	{
		if (this.m_parentControllers == null)
		{
			return;
		}
		for (int i = 0; i < this.m_parentControllers.Length; i++)
		{
			this.m_parentControllers[i].ViewComponent.IsVisible = this.m_isVisible[i];
			this.m_parentControllers[i].WindowGroup.isEscClosable = this.m_isEscClosable[i];
		}
	}

	// Token: 0x040066BE RID: 26302
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController[] m_parentControllers;

	// Token: 0x040066BF RID: 26303
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool[] m_isVisible;

	// Token: 0x040066C0 RID: 26304
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool[] m_isEscClosable;
}
