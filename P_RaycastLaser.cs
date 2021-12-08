// Copyright © 2021 Pokeyi - https://pokeyi.dev - pokeyi@pm.me - This work is licensed under the MIT License.

// using System;
using UdonSharp;
using UnityEngine;
// using UnityEngine.UI;
// using VRC.SDKBase;
// using VRC.SDK3.Components;
// using VRC.Udon.Common.Interfaces;

namespace Pokeyi.UdonSharp
{
    [AddComponentMenu("Pokeyi.VRChat/P.VRC Raycast Laser")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)] // No networking.
    [RequireComponent(typeof(LineRenderer))] // Make sure object has a line-renderer component.

    public class P_RaycastLaser : UdonSharpBehaviour
    {   // Raycast laser renderer and point-plotter for VRChat:
        [Header(":: VRC Raycast Laser by Pokeyi ::")]
        [Space]
        [Tooltip("Max distance laser will render in forward direction.")]
        [SerializeField] private int maxDistance = 500;
        [Tooltip("Object to plot at laser endpoint.")]
        [SerializeField] private GameObject laserTarget;
        [Tooltip("Object to plot at laser midpoint.")]
        [SerializeField] private GameObject laserMidpoint;
        [Tooltip("Ability to bounce on reflective surface.")]
        [SerializeField] private bool canReflect = false;
        [Tooltip("Partial object name designating reflective surfaces.")]
        [SerializeField] private string reflectNameContains = "Reflective";

        private LineRenderer lineRenderer; // Reference to line renderer component.

        public void Start()
        {   // Assign reference to line renderer, set target and midpoint to 'IgnoreRaycast' layer:
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 3;
            if (laserTarget != null) laserTarget.layer = 2;
            if (laserMidpoint != null) laserMidpoint.layer = 2;
        }

        public void Update()
        {   // Draw raycast laser with line renderer, set midpoint and endpoint:
            if ((lineRenderer == null) || (laserTarget == null) || (laserMidpoint == null)) return;
            lineRenderer.SetPosition(0, transform.position);
            Vector3 targetPos;
            RaycastHit rayHit;
            if (Physics.Raycast(transform.position, transform.forward, out rayHit)) targetPos = rayHit.point;
            else targetPos = transform.forward * maxDistance;
            lineRenderer.SetPosition(1, targetPos);
            laserTarget.transform.position = targetPos;
            laserMidpoint.transform.position = (targetPos + transform.position) / 2;
            if (canReflect && rayHit.collider && rayHit.collider.name.Contains(reflectNameContains))
            {   // If reflective, draw another raycast laser with line renderer, adjust midpoint and endpoint:
                Vector3 reflectPos;
                Vector3 reflectDirection = Vector3.Reflect(transform.forward, rayHit.normal);
                if (Physics.Raycast(targetPos, reflectDirection, out rayHit)) reflectPos = rayHit.point;
                else reflectPos = reflectDirection * maxDistance;
                lineRenderer.SetPosition(2, reflectPos);
                laserTarget.transform.position = reflectPos;
                laserMidpoint.transform.position = targetPos;
            }
            else lineRenderer.SetPosition(2, transform.position);
        }
    }
}

/* MIT License

Copyright (c) 2021 Pokeyi - https://pokeyi.dev - pokeyi@pm.me

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */