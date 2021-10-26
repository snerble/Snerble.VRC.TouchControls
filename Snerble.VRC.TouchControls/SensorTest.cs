using Snerble.VRC.TouchControls.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls
{
    public class SensorTest
    {
        private GameObject sphere1;
        private GameObject sphere2;

        public TouchUnit touchUnit;

        public SensorTest()
        {
            sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere1.transform.position = new Vector3(74.5f, -7, 4.5f);
            sphere2.transform.position = new Vector3(74.5f, -7, 0.5f);

            sphere1.SetActive(true);
            sphere2.SetActive(true);

            touchUnit = new ToggleTouchUnit(
                new ObjectTouchSensor(sphere1.transform, 0.5f),
                new[] { new ObjectTouchProbe(sphere2.transform, 0.5f) })
            {
                
            };
        }

        public void OnUpdate()
        {
            if (Input.GetKey(KeyCode.UpArrow))
                sphere1.transform.position = sphere1.transform.position + new Vector3(1f / 90f, 0, 0);
            if (Input.GetKey(KeyCode.DownArrow))
                sphere1.transform.position = sphere1.transform.position + new Vector3(-1f / 90, 0, 0);
            if (Input.GetKey(KeyCode.LeftArrow))
                sphere1.transform.position = sphere1.transform.position + new Vector3(0, 0, 1f / 90f);
            if (Input.GetKey(KeyCode.RightArrow))
                sphere1.transform.position = sphere1.transform.position + new Vector3(0, 0, -1f / 90f);

            var measurement = touchUnit.Measure();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Log.Msg("Pos1: {0}", sphere1.transform.position.z);
                Log.Msg("Pos2: {0}", sphere2.transform.position.z);
                Log.Msg("Magn: {0}", (sphere1.transform.position - sphere2.transform.position).magnitude);
                Log.Msg("Measurement: {0}", measurement);
            }

            var colors = Color.Lerp(Color.red, Color.green, measurement);
            sphere1.GetComponent<Renderer>().material.color = colors;
            sphere2.GetComponent<Renderer>().material.color = colors;
        }
    }
}
