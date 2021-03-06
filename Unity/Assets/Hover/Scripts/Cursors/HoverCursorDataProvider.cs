﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hover.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorDataProvider : MonoBehaviour {

		public List<IHoverCursorData> Cursors { get; private set; }
		public List<IHoverCursorData> ExcludedCursors { get; private set; }
		
		private readonly List<IHoverCursorDataForInput> vCursorsForInput;
		private readonly Dictionary<CursorType, IHoverCursorDataForInput> vCursorMap;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverCursorDataProvider() {
			Cursors = new List<IHoverCursorData>();
			ExcludedCursors = new List<IHoverCursorData>();
			
			vCursorsForInput = new List<IHoverCursorDataForInput>();
			vCursorMap = new Dictionary<CursorType, IHoverCursorDataForInput>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			Update();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			gameObject.GetComponentsInChildren(true, vCursorsForInput);
			
			Cursors.Clear();
			ExcludedCursors.Clear();
			vCursorMap.Clear();
			
			for ( int i = 0 ; i < vCursorsForInput.Count ; i++ ) {
				IHoverCursorDataForInput cursor = vCursorsForInput[i];
				
				if ( vCursorMap.ContainsKey(cursor.Type) ) {
					ExcludedCursors.Add(cursor);
					continue;
				}
				
				Cursors.Add(cursor);
				vCursorMap.Add(cursor.Type, cursor);
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool HasCursorData(CursorType pType) {
			return vCursorMap.ContainsKey(pType);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IHoverCursorData GetCursorData(CursorType pType) {
			return GetCursorDataForInput(pType);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IHoverCursorDataForInput GetCursorDataForInput(CursorType pType) {
			if ( !HasCursorData(pType) ) {
				throw new Exception("No '"+pType+"' cursor was found.");
			}
			
			return vCursorMap[pType];
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void MarkAllCursorsUnused() {
			for ( int i = 0 ; i < vCursorsForInput.Count ; i++ ) {
				vCursorsForInput[i].SetUsedByInput(false);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ActivateAllCursorsBasedOnUsage() {
			for ( int i = 0 ; i < Cursors.Count ; i++ ) {
				vCursorsForInput[i].ActivateIfUsedByInput();
			}
		}

	}

}
