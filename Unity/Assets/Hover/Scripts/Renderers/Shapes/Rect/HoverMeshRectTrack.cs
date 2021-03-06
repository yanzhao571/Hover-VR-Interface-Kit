﻿using Hover.Utils;

namespace Hover.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public abstract class HoverMeshRectTrack : HoverMeshRect {
	
		public const string UvStartYName = "UvStartY";
		public const string UvEndYName = "UvEndY";
		public const string IsFillName = "IsFill";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float UvStartY = 0;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float UvEndY = 1;

		[DisableWhenControlled]
		public bool IsFill = false;
		
		private float vPrevUvStartY;
		private float vPrevUvEndY;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool ShouldUpdateMesh() {
			bool shouldUpdate = (
				base.ShouldUpdateMesh() ||
				UvStartY != vPrevUvStartY ||
				UvEndY != vPrevUvEndY
			);

			vPrevUvStartY = UvStartY;
			vPrevUvEndY = UvEndY;

			return shouldUpdate;
		}
		
	}

}
