﻿namespace Hover.Items {

	/*================================================================================================*/
	public interface ISelectableItem<T> : ISelectableItem {

		event ItemEvents.ValueChangedHandler<T> OnValueChanged;

		T Value { get; set; }

	}

}
