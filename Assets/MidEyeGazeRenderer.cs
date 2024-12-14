using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metaface.Utilities
{
    public class MidEyeGazeRenderer : MonoBehaviour
    {

        [SerializeField]
        OVREyeGaze leftEye;

        [SerializeField]
        OVREyeGaze rightEye;

        [SerializeField]
        GameObject midRayOB;

        [SerializeField]
        private bool showRays = false;

        [SerializeField]
        private float maxGazeDistance = 1000f;

        private LineRenderer midRay;

        GameObject gazeIndicator;

        public float eyeXOffset, eyeYOffset;

        Transform adjustedEyeL, adjustedEyeR;

        void Start()
        {
            midRay = midRayOB.GetComponent<LineRenderer>();
            gazeIndicator = transform.GetChild(1).gameObject;
        }


        void Update()
        {
            RaycastHit hitMid;
            UpdateMidEye(RaycastMidEye(leftEye, rightEye, out hitMid, midRay, maxGazeDistance), hitMid);
        }


        private void UpdateMidEye(bool didHit, RaycastHit hit)
        {
            if (didHit)
            {
                transform.GetChild(1).transform.position = hit.point;
                gazeIndicator.transform.position = hit.point;
            }
        }


        /// <summary>
        /// Performs a raycast from the particular eye
        /// </summary>
        /// <param name="gaze"></param>
        /// <param name="hit"></param>
        /// <returns></returns>
        private bool RaycastMidEye(OVREyeGaze gazeL, OVREyeGaze gazeR, out RaycastHit hit, LineRenderer visualRay, float distance = 1000f)
        {
            adjustedEyeL = gazeL.transform;
            adjustedEyeR = gazeR.transform;

            adjustedEyeL.eulerAngles = gazeL.transform.eulerAngles + new Vector3(eyeXOffset, eyeYOffset, 0f);
            adjustedEyeR.eulerAngles = gazeR.transform.eulerAngles + new Vector3(eyeXOffset, eyeYOffset, 0f);


            if (showRays)
            {
                midRay.SetPositions(new Vector3[] { (adjustedEyeL.transform.position + adjustedEyeR.transform.position) / 2,
                                                       (adjustedEyeL.transform.forward * distance + adjustedEyeR.transform.forward * distance) / 2 });
            }

            return Physics.Raycast((adjustedEyeL.transform.position + adjustedEyeR.transform.position) / 2,
                ((adjustedEyeL.transform.forward * distance + adjustedEyeR.transform.forward * distance) / 2).normalized, out hit, distance);
        }


    }
}