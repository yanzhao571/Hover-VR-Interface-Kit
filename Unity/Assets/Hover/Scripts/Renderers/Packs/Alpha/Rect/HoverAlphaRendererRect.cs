﻿using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Rect {

	/*================================================================================================*/
	public abstract class HoverAlphaRendererRect : HoverAlphaRenderer {
	
		public const string SizeXName = "_SizeX";
		public const string SizeYName = "_SizeY";

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		private float _SizeX = 0.1f;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		private float _SizeY = 0.1f;
		
		[DisableWhenControlled]
		public AnchorTypeWithCustom Anchor = AnchorTypeWithCustom.MiddleCenter;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float SizeX {
			get { return _SizeX; }
			set { _SizeX = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SizeY {
			get { return _SizeY; }
			set { _SizeY = value; }
		}

	}

}
