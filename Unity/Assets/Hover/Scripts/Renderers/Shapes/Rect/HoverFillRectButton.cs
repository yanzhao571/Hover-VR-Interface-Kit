using System;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public abstract class HoverFillRectButton : HoverFill {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string HighlightProgressName = "HighlightProgress";
		public const string SelectionProgressName = "SelectionProgress";
		
		public abstract HoverMeshRectButton Background { get; }
		public abstract HoverMeshRectButton Highlight { get; }
		public abstract HoverMeshRectButton Selection { get; }
		public abstract HoverMeshRectButton Edge { get; }

		[DisableWhenControlled(RangeMin=0)]
		public float SizeX = 0.1f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float SizeY = 0.1f;
		
		[DisableWhenControlled(RangeMin=0.0001f)]
		public float EdgeThickness = 0.0005f;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float HighlightProgress = 0.7f;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float SelectionProgress = 0.2f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdateGeneralSettings();
			UpdateActiveStates();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateGeneralSettings() {
			SelectionProgress = Mathf.Min(HighlightProgress, SelectionProgress);
			
			Highlight.Controllers.Set("GameObject.activeSelf", this);
			Selection.Controllers.Set("GameObject.activeSelf", this);
			UpdateMeshControl(Background);
			UpdateMeshControl(Highlight);
			UpdateMeshControl(Selection);
			UpdateMeshControl(Edge);
			
			float insetSizeX = Math.Max(0, SizeX-EdgeThickness);
			float insetSizeY = Math.Max(0, SizeY-EdgeThickness);

			Background.SizeX = insetSizeX;
			Background.SizeY = insetSizeY;
			Highlight.SizeX = insetSizeX;
			Highlight.SizeY = insetSizeY;
			Selection.SizeX = insetSizeX;
			Selection.SizeY = insetSizeY;
			Edge.SizeX = SizeX;
			Edge.SizeY = SizeY;
			
			Background.OuterAmount = 1;
			Background.InnerAmount = HighlightProgress;
			Highlight.OuterAmount = HighlightProgress;
			Highlight.InnerAmount = SelectionProgress;
			Selection.OuterAmount = SelectionProgress;
			Selection.InnerAmount = 0;
			Edge.OuterAmount = 1;
			Edge.InnerAmount = 1-EdgeThickness/Mathf.Min(SizeX, SizeY);

			Background.SortingLayer = SortingLayer;
			Highlight.SortingLayer = SortingLayer;
			Selection.SortingLayer = SortingLayer;
			Edge.SortingLayer = SortingLayer;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshControl(HoverMeshRectButton pMesh) {
			pMesh.Controllers.Set(HoverMeshRectButton.SizeXName, this);
			pMesh.Controllers.Set(HoverMeshRectButton.SizeYName, this);
			pMesh.Controllers.Set(HoverMeshRectButton.OuterAmountName, this);
			pMesh.Controllers.Set(HoverMeshRectButton.InnerAmountName, this);
			pMesh.Controllers.Set(HoverMesh.SortingLayerName, this);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveStates() {
			RendererUtil.SetActiveWithUpdate(Highlight, (Highlight.OuterAmount > 0));
			RendererUtil.SetActiveWithUpdate(Selection, (Selection.OuterAmount > 0));
		}
				
	}

}
