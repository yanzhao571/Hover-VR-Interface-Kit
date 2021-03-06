using Hover.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Hover.Renderers.Contents {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(RawImage))]
	public class HoverIcon : MonoBehaviour, ITreeUpdateable {
		
		public const string IconTypeName = "IconType";
		public const string CanvasScaleName = "CanvasScale";
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		public enum IconOffset {
			None,
			CheckOuter,
			CheckInner,
			RadioOuter,
			RadioInner,
			Parent,
			Slider,
			Sticky
		}

		public ISettingsControllerMap Controllers { get; private set; }
		
		[DisableWhenControlled(DisplayMessage=true)]
		public IconOffset IconType = IconOffset.CheckOuter;

		[DisableWhenControlled(RangeMin=0.0001f, RangeMax=1)]
		public float CanvasScale = 0.0002f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float SizeX = 0.1f;

		[DisableWhenControlled(RangeMin=0)]
		public float SizeY = 0.1f;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=0.01f)]
		public float UvInset = 0.002f;

		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverIcon() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RawImage ImageComponent {
			get { return GetComponent<RawImage>(); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				BuildIcon();
				_IsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			RawImage icon = ImageComponent;
			RectTransform rectTx = icon.rectTransform;
			const float w = 1/8f;
			const float h = 1;

			icon.uvRect = new UnityEngine.Rect((int)IconType*w+UvInset, UvInset, w-UvInset*2, h-UvInset*2);

			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SizeX/CanvasScale);
			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SizeY/CanvasScale);
			Controllers.TryExpireControllers();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildIcon() {
			RawImage icon = ImageComponent;
			icon.material = Resources.Load<Material>("Materials/HoverRendererStandardIconsMaterial");
			icon.color = Color.white;
			icon.raycastTarget = false;
		}

	}

}
