﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gist;
using Gist.Extensions.AABB;

public class ViewSpaceBounds {
    public Matrix4x4 VoxelModel { get; set; }
    public Matrix4x4 View { get; set; }

    public Bounds LocalBounds { get; private set; }
    public Bounds ViewBounds { get; private set; }

    public Matrix4x4 VoxelUvToLocal { get; private set; }
    public Matrix4x4 ViewUvToLocal { get; private set; }
    public Matrix4x4 ViewUvToVoxelUv { get; private set; }

    public void Init(AbstractVoxelBounds voxelBounds, Matrix4x4 voxelModel) {
        LocalBounds = voxelBounds.LocalBounds;
        VoxelUvToLocal = voxelBounds.VoxelUvToLocalMatrix ();
        VoxelModel = voxelModel;
    }
    public void SetView(Matrix4x4 view) {
        View = view;

        var modelView = View * VoxelModel;
        ViewBounds = LocalBounds.EncapsulateInTargetSpace (modelView);

        ViewUvToLocal = CalculateViewUvToViewLocal (ViewBounds);
        ViewUvToVoxelUv = (modelView * VoxelUvToLocal).inverse * ViewUvToLocal;
    }
    public void DrawGizmos(AbstractVoxelBounds voxelBounds, Color localColor, Color viewColor) {
        Gizmos.color = localColor;
        Gizmos.matrix = VoxelModel;
        voxelBounds.DrawGizmosLocal ();

        Gizmos.color = viewColor;
        Gizmos.matrix = View.inverse * ViewUvToLocal;
        Gizmos.DrawWireCube (0.5f * Vector3.one, 1f * Vector3.one);

        Gizmos.matrix = Matrix4x4.identity;
    }

    static Matrix4x4 CalculateViewUvToViewLocal (Bounds viewBounds) {
        var size = viewBounds.size;
        var min = viewBounds.min;
        var m = Matrix4x4.zero;
        m [0] = size.x;
        m [12] = min.x;
        m [5] = size.y;
        m [13] = min.y;
        m [10] = size.z;
        m [14] = min.z;
        m [15] = 1f;
        return m;
    }
}
