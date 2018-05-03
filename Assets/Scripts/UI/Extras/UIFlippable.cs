﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform)), RequireComponent(typeof(Graphic)), DisallowMultipleComponent, AddComponentMenu("UI/Flippable")]
    public class UIFlippable : MonoBehaviour, IMeshModifier
    {

        [SerializeField]
        private bool m_Horizontal = false;
        [SerializeField]
        private bool m_Veritical = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UnityEngine.UI.UIFlippable"/> should be flipped horizontally.
        /// </summary>
        /// <value><c>true</c> if horizontal; otherwise, <c>false</c>.</value>
        public bool horizontal
        {
            get { return this.m_Horizontal; }
            set { this.m_Horizontal = value; this.GetComponent<Graphic>().SetVerticesDirty(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UnityEngine.UI.UIFlippable"/> should be flipped vertically.
        /// </summary>
        /// <value><c>true</c> if vertical; otherwise, <c>false</c>.</value>
        public bool vertical
        {
            get { return this.m_Veritical; }
            set { this.m_Veritical = value; this.GetComponent<Graphic>().SetVerticesDirty(); }
        }

        public void FlipVertical()
        {
            FlipVertical(!vertical);
        }

        public void FlipVertical(bool flip)
        {
            vertical = flip;
        }
        public void FlipHorizontal()
        {
            FlipHorizontal(!horizontal);
        }

        public void FlipHorizontal(bool flip)
        {
            horizontal = flip;
        }

        protected void OnValidate()
        {
            this.GetComponent<Graphic>().SetVerticesDirty();
        }

        public void ModifyVertices(List<UIVertex> verts)
        {
            RectTransform rt = this.transform as RectTransform;

            for (int i = 0; i < verts.Count; ++i)
            {
                UIVertex v = verts[i];

                // Modify positions
                v.position = new Vector3(
                    (this.m_Horizontal ? (v.position.x + (rt.rect.center.x - v.position.x) * 2) : v.position.x),
                    (this.m_Veritical ? (v.position.y + (rt.rect.center.y - v.position.y) * 2) : v.position.y),
                    v.position.z
                );

                // Apply
                verts[i] = v;
            }
        }

        public void ModifyMesh(Mesh mesh)
        {
        }

        public void ModifyMesh(VertexHelper verts)
        {
            List<UIVertex> buffer = new List<UIVertex>();
            verts.GetUIVertexStream(buffer);
            ModifyVertices(buffer);
            verts.AddUIVertexTriangleStream(buffer);
        }
    }
}