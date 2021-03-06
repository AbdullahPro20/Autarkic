using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		[SerializeField]
		public GameObject a;
		public enum AxisOption
		{
		
			Both, 
			OnlyHorizontal, 
			OnlyVertical 
		}

		public int MovementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; 
		 public string verticalAxisName = "Vertical"; 

		Vector3 m_StartPos;
		bool m_UseX; 
		bool m_UseY; 
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; 
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; 

		void OnEnable()
		{
			CreateVirtualAxes();

		}

        void Start()
        {
            m_StartPos = transform.position;
        }

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;

			
			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(delta.y);
			}
		 
		}

		void CreateVirtualAxes()
		{
			
			 
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);
			 
			 
			if (m_UseY)
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}


		public void OnDrag(PointerEventData data)
		{
			Vector3 newPos = Vector3.zero;



			if (m_UseY)
			{
				int delta = (int)(data.position.y - m_StartPos.y);
				delta = Mathf.Clamp(delta, 0, MovementRange);
				if (delta > 30)
				{
					a.SetActive(true);
				}

				newPos.y = delta;
			}
			
			transform.position = new Vector3(m_StartPos.x + newPos.x, m_StartPos.y + newPos.y, m_StartPos.z + newPos.z);
			UpdateVirtualAxes(transform.position);
		}


		public void OnPointerUp(PointerEventData data)
		{
			transform.position = m_StartPos;
			a.SetActive(false);
			UpdateVirtualAxes(m_StartPos);
		}


		public void OnPointerDown(PointerEventData data) { }

		void OnDisable()
		{
			
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Remove();
	
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis.Remove();
	
			}
		}
	}
}