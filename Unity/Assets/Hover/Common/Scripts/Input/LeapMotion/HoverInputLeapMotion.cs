﻿#if !HOVER_IGNORE_LEAP

using System;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using UnityEngine;

namespace Hover.Common.Input.LeapMotion {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInputLeapMotion : MonoBehaviour {

		public HoverCursorDataProvider CursorDataProvider;
		public LeapServiceProvider LeapServiceProvider;
		public bool UseStabilizedPositions = false;
		
		[HideInInspector]
		[SerializeField]
		protected bool _IsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				CursorDataProvider = FindObjectOfType<HoverCursorDataProvider>();
				LeapServiceProvider = FindObjectOfType<LeapServiceProvider>();
				_IsBuilt = true;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !Application.isPlaying ) {
				return;
			}

			if ( CursorDataProvider == null || LeapServiceProvider == null ) {
				Debug.LogError("References to "+typeof(HoverCursorDataProvider).Name+" and "+
					typeof(LeapServiceProvider).Name+" must be set.", this);
				return;
			}

			CursorDataProvider.MarkAllCursorsUnused();
			UpdateCursorsWithHands(LeapServiceProvider.CurrentFrame.Hands);
			CursorDataProvider.ActivateAllCursorsBasedOnUsage();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithHands(List<Hand> pLeapHands) {
			for ( int i = 0 ; i < pLeapHands.Count ; i++ ) {
				UpdateCursorsWithHand(pLeapHands[i]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithHand(Hand pLeapHand) {
			UpdateCursorsWithPalm(pLeapHand);

			for ( int i = 0 ; i < pLeapHand.Fingers.Count ; i++ ) {
				UpdateCursorsWithFinger(pLeapHand, pLeapHand.Fingers[i]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithPalm(Hand pLeapHand) {
			CursorType cursorType = (pLeapHand.IsLeft ? CursorType.LeftPalm : CursorType.RightPalm);

			if ( !CursorDataProvider.HasCursorData(cursorType) ) {
				return;
			}

			Vector palmPos = (UseStabilizedPositions ? 
				pLeapHand.StabilizedPalmPosition : pLeapHand.PalmPosition);

			IHoverCursorDataForInput data = CursorDataProvider.GetCursorDataForInput(cursorType);
			data.SetWorldPosition(palmPos.ToVector3());
			data.SetWorldRotation(pLeapHand.Basis.CalculateRotation());
			data.SetSize(pLeapHand.PalmWidth);
			data.SetUsage(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithFinger(Hand pLeapHand, Finger pLeapFinger) {
			CursorType cursorType = GetFingerCursorType(pLeapHand.IsLeft, pLeapFinger.Type);

			if ( !CursorDataProvider.HasCursorData(cursorType) ) {
				return;
			}

			Vector tipPos = (UseStabilizedPositions ? 
				pLeapFinger.StabilizedTipPosition: pLeapFinger.TipPosition);
			Bone distalBone = pLeapFinger.Bone(Bone.BoneType.TYPE_DISTAL);

			IHoverCursorDataForInput data = CursorDataProvider.GetCursorDataForInput(cursorType);
			data.SetWorldPosition(tipPos.ToVector3());
			data.SetWorldRotation(distalBone.Basis.CalculateRotation());
			data.SetSize(pLeapFinger.Width);
			data.SetUsage(true);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private CursorType GetFingerCursorType(bool pIsLeft, Finger.FingerType pLeapFingerType) {
			switch ( pLeapFingerType ) {
				case Finger.FingerType.TYPE_THUMB:
					return (pIsLeft ? CursorType.LeftThumb : CursorType.RightThumb);
					
				case Finger.FingerType.TYPE_INDEX:
					return (pIsLeft ? CursorType.LeftIndex : CursorType.RightIndex);
					
				case Finger.FingerType.TYPE_MIDDLE:
					return (pIsLeft ? CursorType.LeftMiddle : CursorType.RightMiddle);
					
				case Finger.FingerType.TYPE_RING:
					return (pIsLeft ? CursorType.LeftRing : CursorType.RightRing);
					
				case Finger.FingerType.TYPE_PINKY:
					return (pIsLeft ? CursorType.LeftPinky : CursorType.RightPinky);
			}
			
			throw new Exception("Unhandled cursor combination: "+pIsLeft+" / "+pLeapFingerType);
		}

	}

}

#endif //!HOVER_IGNORE_LEAP
