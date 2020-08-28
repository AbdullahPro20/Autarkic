using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SickscoreGames;

namespace SickscoreGames.HUDNavigationSystem
{
	[AddComponentMenu (HNS.Name + "/HUD Navigation Canvas"), DisallowMultipleComponent]
	public class HUDNavigationCanvas : MonoBehaviour
	{
		private static HUDNavigationCanvas _Instance;
		public static HUDNavigationCanvas Instance {
			get {
				if (_Instance == null) {
					_Instance = FindObjectOfType<HUDNavigationCanvas> ();
				}
				return _Instance;
			}
		}


		#region Variables
		public _RadarReferences Radar;
		public _CompassBarReferences CompassBar;
		public _IndicatorReferences Indicator;
		public _MinimapReferences Minimap;


		public float CompassBarCurrentDegrees { get; private set; }
		#endregion


		#region Main Methods
		void Awake ()
		{
			_Instance = this;
		}
		#endregion


		#region Radar Methods
		public void InitRadar ()
		{
		
			if (Radar.Panel == null || Radar.Radar == null || Radar.ElementContainer == null) {
				ReferencesMissing ("Radar");
				return;
			}

		
			ShowRadar (true);
		}


		public void ShowRadar (bool value)
		{
			if (Radar.Panel != null)
				Radar.Panel.gameObject.SetActive (value);
		}


		public void UpdateRadar (Transform rotationReference, RadarModes radarType)
		{
	
			if (radarType == RadarModes.RotateRadar) {
	
				Radar.Radar.transform.rotation = Quaternion.Euler (Radar.Panel.transform.eulerAngles.x, Radar.Panel.transform.eulerAngles.y, rotationReference.eulerAngles.y);
				if (Radar.PlayerIndicator != null)
					Radar.PlayerIndicator.transform.rotation = Radar.Panel.transform.rotation;
			} else {
		
				Radar.Radar.transform.rotation = Radar.Panel.transform.rotation;
				if (Radar.PlayerIndicator != null)
					Radar.PlayerIndicator.transform.rotation = Quaternion.Euler (Radar.Panel.transform.eulerAngles.x, Radar.Panel.transform.eulerAngles.y, -rotationReference.eulerAngles.y);
			}
		}
		#endregion


		#region Compass Bar Methods
		public void InitCompassBar ()
		{
		
			if (CompassBar.Panel == null || CompassBar.Compass == null || CompassBar.ElementContainer == null) {
				ReferencesMissing ("Compass Bar");
				return;
			}

		
			ShowCompassBar (true);
		}


		public void ShowCompassBar (bool value)
		{
			if (CompassBar.Panel != null)
				CompassBar.Panel.gameObject.SetActive (value);
		}


		public void UpdateCompassBar (Transform rotationReference)
		{
		
			CompassBar.Compass.uvRect = new Rect ((rotationReference.eulerAngles.y / 360f) - .5f, 0f, 1f, 1f);

			Vector3 perpDirection = Vector3.Cross (Vector3.forward, rotationReference.forward);
			float angle = Vector3.Angle (new Vector3 (rotationReference.forward.x, 0f, rotationReference.forward.z), Vector3.forward);
			CompassBarCurrentDegrees = (perpDirection.y >= 0f) ? angle : 360f - angle;
		}
		#endregion


		#region Indicator Methods
		public void InitIndicators ()
		{
			
			if (Indicator.Panel == null || Indicator.ElementContainer == null) {
				ReferencesMissing ("Indicator");
				return;
			}

			ShowIndicators (true);
		}


		public void ShowIndicators (bool value)
		{
			if (Indicator.Panel != null)
				Indicator.Panel.gameObject.SetActive (value);
		}
		#endregion


		#region Minimap Methods
		public void InitMinimap (HNSMapProfile profile)
		{
	
			if (Minimap.Panel == null || Minimap.MapContainer == null || Minimap.ElementContainer == null) {
				ReferencesMissing ("Minimap");
				return;
			}


			GameObject imageGO = new GameObject (profile.MapTexture.name);
			imageGO.transform.SetParent (Minimap.MapContainer, false);

	
			Image image = imageGO.AddComponent<Image> ();
			image.sprite = profile.MapTexture;
			image.preserveAspect = true;
			image.SetNativeSize ();

	
			if (profile.CustomLayers.Count > 0) {
				int layerCount = 0;
				foreach (CustomLayer layer in profile.CustomLayers.Reverse<CustomLayer> ()) {
					if (layer.sprite == null)
						continue;

				
					GameObject layerGO = new GameObject (layer.name + "_Layer_" + layerCount++);
					layerGO.transform.SetParent (Minimap.MapContainer, false);
					layerGO.SetActive (layer.enabled);

					
					Image layerImage = layerGO.AddComponent<Image> ();
					layerImage.sprite = layer.sprite;
					layerImage.preserveAspect = true;
					layerImage.SetNativeSize ();

					layer.instance = layerGO;
				}
			}

			ShowMinimap (true);
		}


		public void ShowMinimap (bool value)
		{
			if (Minimap.Panel != null)
				Minimap.Panel.gameObject.SetActive (value);
		}


		public void UpdateMinimap (Transform rotationReference, MinimapModes minimapMode, Transform playerTransform, HNSMapProfile profile, float scale)
		{
		
			Quaternion identityRotation = Minimap.Panel.transform.rotation;
			Quaternion mapRotation = identityRotation;
			if (minimapMode == MinimapModes.RotateMinimap)
				mapRotation = Quaternion.Euler (Minimap.Panel.transform.eulerAngles.x, Minimap.Panel.transform.eulerAngles.y, rotationReference.eulerAngles.y);

		
			if (Minimap.PlayerIndicator != null) {
				if (minimapMode == MinimapModes.RotateMinimap)
					Minimap.PlayerIndicator.transform.rotation = identityRotation;
				else
					Minimap.PlayerIndicator.transform.rotation = Quaternion.Euler (Minimap.Panel.transform.eulerAngles.x, Minimap.Panel.transform.eulerAngles.y, -rotationReference.eulerAngles.y);
			}

	
			Vector2 unitScale = profile.GetMapUnitScale ();
			Vector3 posOffset = profile.MapBounds.center - playerTransform.position;
			Vector3 mapPos = new Vector3 (posOffset.x * unitScale.x, posOffset.z * unitScale.y, 0f) * scale;

			
			if (minimapMode == MinimapModes.RotateMinimap)
				mapPos = playerTransform.MinimapRotationOffset (mapPos);

			Minimap.MapContainer.localPosition = new Vector2 (mapPos.x, mapPos.y);
			Minimap.MapContainer.rotation = mapRotation;
			Minimap.MapContainer.localScale = new Vector3 (scale, scale, 1f);
		}
		#endregion


		#region Utility Methods
		void ReferencesMissing (string feature)
		{
			Debug.LogErrorFormat ("{0} references are missing! Please assign them on the HUDNavigationCanvas component.", feature);
			this.enabled = false;
		}
		#endregion


		#region Subclasses
		[System.Serializable]
		public class _RadarReferences
		{
			public RectTransform Panel;
			public RectTransform Radar;
			public RectTransform PlayerIndicator;
			public RectTransform ElementContainer;
		}


		[System.Serializable]
		public class _CompassBarReferences
		{
			public RectTransform Panel;
			public RawImage Compass;
			public RectTransform ElementContainer;
		}


		[System.Serializable]
		public class _IndicatorReferences
		{
			public RectTransform Panel;
			public RectTransform ElementContainer;
		}


		[System.Serializable]
		public class _MinimapReferences
		{
			public RectTransform Panel;
			public RectTransform MapContainer;
			public RectTransform PlayerIndicator;
			public RectTransform ElementContainer;
		}
		#endregion
	}
}
