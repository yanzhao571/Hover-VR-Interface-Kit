﻿using System.Collections.Generic;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public abstract class HoverFillRectSlider : HoverFill {
	
		public const string SegmentInfoListName = "SegmentInfoList";
		public const string TickInfoListName = "TickInfoList";
		public const string SizeXName = "SizeX";
		public const int SegmentCount = 4;

		//TODO: DisableWhenControlled
		public List<SliderUtil.SegmentInfo> SegmentInfoList;
		public List<SliderUtil.SegmentInfo> TickInfoList;
		
		[DisableWhenControlled(RangeMin=0)]
		public float SizeX = 0.1f;

		[DisableWhenControlled(RangeMin=0)]
		public float InsetL = 0.01f;

		[DisableWhenControlled(RangeMin=0)]
		public float InsetR = 0.01f;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float TickRelativeSizeX = 0.5f;

		[DisableWhenControlled]
		public bool UseTrackUv = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			if ( SegmentInfoList != null ) {
				UpdateSegmentsWithInfo();
			}

			if ( TickInfoList != null ) {
				UpdateTickCount(TickInfoList.Count);
				UpdateTicksWithInfo();
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract HoverMeshRectTrack GetSegment(int pIndex);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract HoverMeshRectTrack GetTick(int pIndex);
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateTickCount(int pCount);
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateSegmentsWithInfo() {
			int segIndex = 0;
			float insetSizeX = Mathf.Max(0, SizeX-InsetL-InsetR);
			float trackStartY = SegmentInfoList[0].StartPosition;
			float trackEndY = SegmentInfoList[SegmentInfoList.Count-1].EndPosition;

			for ( int i = 0 ; i < SegmentCount ; i++ ) {
				HoverMeshRectTrack seg = GetSegment(i);
				
				seg.Controllers.Set("GameObject.activeSelf", this);
				seg.Controllers.Set("Transform.localPosition.x", this);
				seg.Controllers.Set("Transform.localPosition.y", this);
				seg.Controllers.Set(HoverMeshRect.SizeXName, this);
				seg.Controllers.Set(HoverMeshRect.SizeYName, this);
				seg.Controllers.Set(HoverMeshRectTrack.UvStartYName, this);
				seg.Controllers.Set(HoverMeshRectTrack.UvEndYName, this);
				seg.Controllers.Set(HoverMeshRectTrack.IsFillName, this);
				seg.Controllers.Set(HoverMesh.SortingLayerName, this);

				seg.SizeY = 0;
				seg.SizeX = insetSizeX;
				seg.SortingLayer = SortingLayer;
			}

			for ( int i = 0 ; i < SegmentInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo segInfo = SegmentInfoList[i];

				if ( segInfo.Type != SliderUtil.SegmentType.Track ) {
					continue;
				}

				HoverMeshRectTrack seg = GetSegment(segIndex++);
				seg.SizeY = segInfo.EndPosition-segInfo.StartPosition;
				seg.IsFill = segInfo.IsFill;
				seg.UvStartY = (UseTrackUv ?
					Mathf.InverseLerp(trackStartY, trackEndY, segInfo.StartPosition) : 0);
				seg.UvEndY = (UseTrackUv ?
					Mathf.InverseLerp(trackStartY, trackEndY, segInfo.EndPosition) : 1);

				Vector3 localPos = seg.transform.localPosition;
				localPos.x = (InsetL-InsetR)/2;
				localPos.y = (segInfo.StartPosition+segInfo.EndPosition)/2;
				seg.transform.localPosition = localPos;
			}

			for ( int i = 0 ; i < SegmentCount ; i++ ) {
				HoverMeshRectTrack seg = GetSegment(i);
				RendererUtil.SetActiveWithUpdate(seg, (seg.SizeY != 0));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateTicksWithInfo() {
			float insetSizeX = Mathf.Max(0, SizeX-InsetL-InsetR);

			for ( int i = 0 ; i < TickInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo tickInfo = TickInfoList[i];
				HoverMeshRectTrack tick = GetTick(i);

				tick.Controllers.Set("GameObject.activeSelf", this);
				tick.Controllers.Set("Transform.localPosition.x", this);
				tick.Controllers.Set("Transform.localPosition.y", this);
				tick.Controllers.Set(HoverMeshRect.SizeXName, this);
				tick.Controllers.Set(HoverMeshRect.SizeYName, this);
				tick.Controllers.Set(HoverMesh.SortingLayerName, this);

				RendererUtil.SetActiveWithUpdate(tick, !tickInfo.IsHidden);

				tick.SizeX = insetSizeX*TickRelativeSizeX;
				tick.SizeY = tickInfo.EndPosition-tickInfo.StartPosition;
				tick.SortingLayer = SortingLayer;

				Vector3 localPos = tick.transform.localPosition;
				localPos.x = (InsetL-InsetR)/2;
				localPos.y = (tickInfo.StartPosition+tickInfo.EndPosition)/2;
				tick.transform.localPosition = localPos;
			}
		}
		
	}

}
