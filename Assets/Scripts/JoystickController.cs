using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour
{

    public RectTransform center;
    public RectTransform knob;
    public float range;
    public bool fixedJoystick;
    public float MaxRange;
    [HideInInspector]
    public Vector2 direction;

    Vector2 start;

    void Start()
    {
        ShowHide(false);
    }

    void Update()
    {
        Vector2 pos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            ShowHide(true);
            start = pos;

            knob.position = pos;
            center.position = pos;
        }
        else if (Input.GetMouseButton(0))
        {
            float dist = Vector3.Distance(pos, knob.transform.position);

            if (dist < MaxRange)
                knob.position = pos;

            Vector3 kPos = center.position + Vector3.ClampMagnitude(knob.position - center.position, center.sizeDelta.x * range);

            Debug.Log(dist);
            
            //knob.position = new Vector3(Mathf.Clamp(kPos.x, 0, MaxRange), Mathf.Clamp(kPos.y, 0, MaxRange), 0);

            if (knob.position != Input.mousePosition && !fixedJoystick)
            {
                Vector3 outsideBoundsVector = Input.mousePosition - knob.position;
                center.position += outsideBoundsVector;
                

            direction = (knob.position - center.position).normalized;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ShowHide(false);
            direction = Vector2.zero;
        }
    }

    public void ShowHide(bool state)
    {
        center.gameObject.SetActive(state);
        knob.gameObject.SetActive(state);

    }

}
