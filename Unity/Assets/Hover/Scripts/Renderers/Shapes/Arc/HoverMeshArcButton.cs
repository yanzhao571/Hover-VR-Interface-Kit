﻿using Hover.Utils;

namespace Hover.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public abstract class HoverMeshArcButton : HoverMeshArc {
	
		public const string OuterAmountName = "OuterAmount";
		public const string InnerAmountName = "InnerAmount";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float OuterAmount = 1;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float InnerAmount = 0.5f;
		
		[DisableWhenControlled]
		public bool UseUvRelativeToSize = false;

		private float vPrevInner;
		private float vPrevOuter;
		private bool vPrevUseUv;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool ShouldUpdateMesh() {
			bool shouldUpdate = (
				base.ShouldUpdateMesh() ||
				InnerAmount != vPrevInner ||
				OuterAmount != vPrevOuter ||
				UseUvRelativeToSize != vPrevUseUv
			);

			vPrevInner = InnerAmount;
			vPrevOuter = OuterAmount;
			vPrevUseUv = UseUvRelativeToSize;

			return shouldUpdate;
		}
		
	}

}
