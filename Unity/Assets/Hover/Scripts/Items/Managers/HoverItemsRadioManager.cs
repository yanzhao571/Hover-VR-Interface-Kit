﻿using System;
using System.Collections.Generic;
using Hover.Items.Types;
using UnityEngine;

namespace Hover.Items.Managers {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemsManager))]
	public class HoverItemsRadioManager : MonoBehaviour {

		private List<HoverItem> vItemsBuffer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vItemsBuffer = new List<HoverItem>();

			HoverItemsManager itemsMan = GetComponent<HoverItemsManager>();

			itemsMan.OnItemListInitialized.AddListener(AddAllItemListeners);
			itemsMan.OnItemAdded.AddListener(AddItemListeners);
			itemsMan.OnItemRemoved.AddListener(RemoveItemListeners);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AddAllItemListeners() {
			HoverItemsManager itemsMan = GetComponent<HoverItemsManager>();

			itemsMan.FillListWithAllItems(vItemsBuffer);

			foreach ( HoverItem item in vItemsBuffer ) {
				AddItemListeners(item);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void AddItemListeners(HoverItem pItem) {
			AddRadioDataListeners(pItem);
			pItem.OnTypeChanged += AddRadioDataListeners;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void RemoveItemListeners(HoverItem pItem) {
			pItem.OnTypeChanged -= AddRadioDataListeners;
			RemoveRadioDataListeners(pItem);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AddRadioDataListeners(HoverItem pItem) {
			IRadioItem radData = (pItem.Data as IRadioItem);

			if ( radData != null ) {
				radData.OnValueChanged += HandleRadioValueChanged;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void RemoveRadioDataListeners(HoverItem pItem) {
			IRadioItem radData = (pItem.Data as IRadioItem);

			if ( radData != null ) {
				radData.OnValueChanged -= HandleRadioValueChanged;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleRadioValueChanged(ISelectableItem<bool> pSelData) {
			IRadioItem radData = (IRadioItem)pSelData;

			if ( !radData.Value ) {
				return;
			}

			DeselectRemainingRadioGroupMembers(radData);
		}

			
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DeselectRemainingRadioGroupMembers(IRadioItem pRadioData) {
			HoverItemsManager itemsMan = GetComponent<HoverItemsManager>();
			string groupId = pRadioData.GroupId;

			Func<HoverItem, bool> filter = (tryItem => {
				IRadioItem match = (tryItem.Data as IRadioItem);
				return (match != null && match != pRadioData && match.GroupId == groupId);
			});

			itemsMan.FillListWithMatchingItems(vItemsBuffer, filter);

			for ( int i = 0 ; i < vItemsBuffer.Count ; i++ ) {
				((IRadioItem)vItemsBuffer[i].Data).Value = false;
			}
		}

	}

}
