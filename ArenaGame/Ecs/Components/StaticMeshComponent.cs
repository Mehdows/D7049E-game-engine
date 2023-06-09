﻿using System;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Entities.Prefabs;
using BEPUutilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Matrix = BEPUutilities.Matrix;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame.Ecs.Components;

// TODO: Remove?
// TODO: Ha en StaticMeshComponent som inte behöver en dynamisk kropp (massa)
public class StaticMeshComponent : IComponent
{
    private readonly String modelPath;
    public Matrix Transform;
    public Capsule Capsule { get; set; }
    public Model Model { get; set; }

    public StaticMeshComponent(String modelPath) 
    {
        this.modelPath = modelPath;
    }

    // TODO Ändra så att Capsule inte är hårdkodad
    public new void LoadContent(ContentManager contentManager)
    {
        Capsule = new Capsule(new Vector3(0, 20, 0), 10f, 5f, 10f);
        Capsule.AngularDamping = 0f;
        Capsule.LocalInertiaTensorInverse = new Matrix3x3(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.0f);
        Capsule.Gravity = new Vector3(0, -150.82f, 0);
        Transform =  Matrix.CreateScale(Capsule.Radius/30, Capsule.Length/110, Capsule.Radius/30);
        Model = contentManager.Load<Model>(modelPath);
        
    }
}