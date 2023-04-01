using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArenaGame;

class Basic3DObject
{
    public class Obj3D
    {
        public int start_index; // Where it is in index buffer
        public int triangle_count;
        public Rectangle source_rect;  // Section within texture to sample from
        public Texture2D tex;
        public Vector3 rot; // Optional rotation
        public Vector3 pos;    // Position -
        public Matrix transform;

        public void UpdateTransform()
        {
            if (rot == Vector3.Zero)
            {
                transform = Matrix.CreateTranslation(pos);
            }
            else
            {
                transform = Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * Matrix.CreateTranslation(pos);
            }
        }

        protected void Init(Vector3 Pos, string file){
            start_index = ibuf_start;
            pos = Pos;
            transform = Matrix.CreateTranslation(pos);
            tex = LoadTexture(file);
            if ((source_rect.Width < 1) || (source_rect.Height < 1)) source_rect = new Rectangle(0, 0, tex.Width, tex.Height);
        }

        protected void GetUVCoords(ref float u1, ref float v1, ref float u2, ref float v2){
            u1 = source_rect.X / (float)tex.Width;
            v1 = source_rect.Y / (float)tex.Height;
            u2 = (source_rect.X + source_rect.Width) / (float)tex.Width;
            v2 = (source_rect.Y + source_rect.Height) / (float)tex.Height;
        }

        public void UpdateTransformQuaternion()
        {
            if (rot == Vector3.Zero)
            {
                transform = Matrix.CreateTranslation(pos);
            }
            else
            {
                Quaternion q = Quaternion.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z);
                transform = Matrix.CreateFromQuaternion(q) * Matrix.CreateTranslation(pos);
            }
        }

        public void AddQuad(Vector3 Pos, float width, float height, Vector3 rotation, string textureFile, Rectangle? sourceRect){
            if(sourceRect.HasValue) source_rect = sourceRect.Value;
            Init(pos, textureFile);
            rot = rotation;
            UpdateTransform();

            float u1 = 0, v1 = 0, u2 = 1, v2 = 1, hw = width / 2, hl = height / 2;
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            Vector3 norm = Vector3.Up;
            float y = pos.Y, l = -hw + Pos.X, r = hw + Pos.X, n = -hl + Pos.Z, f = hl + Pos.Z; // Y-coord, Left, Right, Near, Far
            AddVertex(l, y, f, norm, u1, v1); // 0
            AddVertex(r, y, f, norm, u2, v1); // 1
            AddVertex(r, y, n, norm, u2, v2); // 2
            AddVertex(l, y, n, norm, u1, v2); // 3
            AddTriangle(0, 1, 2); triangle_count++;
            AddTriangle(2,3,0); triangle_count++;

            vertexBuffer.SetData<VertexPositionNormalTexture>(vbuf_start*vbytes, vertices, 0, v_cnt, vbytes);
            vbuf_start = v_cnt; v_cnt = 0;
            indexBuffer.SetData<ushort>(ibuf_start*ibytes, indices, 0, i_cnt);
            ibuf_start = i_cnt; i_cnt = 0;
        }

        public void AddCube(Vector3 Pos, Vector3 size, Vector3 rotation, string textureFile, Rectangle? sourceRect){
            if(sourceRect.HasValue) source_rect = sourceRect.Value;
            Init(pos, textureFile); rot = rotation; UpdateTransform();
            float u1 = 0, v1 = 0, u2 = 1, v2 = 1, hw = size.X / 2, hl = size.Y / 2, hh = size.Z / 2; // Uv's , half-width, half-length, half-height
            GetUVCoords(ref u1, ref v1, ref u2, ref v2);
            float t = Pos.Y - hh, b = Pos.Y + hh, l = -hw, r = Pos.X + hw, n = Pos.Z - hl, f = Pos.Z + hl; // Top, Bottom, Left, Right, Near, Far
            
            Vector3 norm = Vector3.Up;
            AddVertex(l,t,f,norm,u1,v1); // 0
            AddVertex(r,t,f,norm,u2,v1); // 1
            AddVertex(r,t,n,norm,u2,v2); // 2
            AddVertex(l,t,n,norm,u1,v2); // 3

            norm = Vector3.Right;
            AddVertex(r,b,f,norm,u1,v1); // 4
            AddVertex(r,b,n,norm,u1,v2); // 5

            norm = Vector3.Down;
            AddVertex(l,b,f,norm,u2,v1); // 6
            AddVertex(l,b,n,norm,u2,v2); // 7

            norm = Vector3.Backward;
            AddVertex(l,t,n,norm,u1,v1); // 8
            AddVertex(r,t,n,norm,u2,v1); // 9
            AddVertex(r,b,n,norm,u2,v2); // 10
            AddVertex(l,b,n,norm,u1,v2); // 11
            
            norm = Vector3.Forward;
            AddVertex(r,t,f,norm,u1,v1); // 12
            AddVertex(l,t,f,norm,u2,v1); // 13
            AddVertex(l,b,f,norm,u2,v2); // 14
            AddVertex(r,b,f,norm,u1,v2); // 15

            AddTriangle(0,1,2); triangle_count++; AddTriangle(2,3,0); triangle_count++; // Top - clockwise order
            AddTriangle(2,1,4); triangle_count++; AddTriangle(4,5,2); triangle_count++;
            AddTriangle(5,4,6); triangle_count++; AddTriangle(6,7,5); triangle_count++;
            AddTriangle(7,6,0); triangle_count++; AddTriangle(0,3,7); triangle_count++;
            AddTriangle(8,9,10); triangle_count++; AddTriangle(10,11,8); triangle_count++;
            AddTriangle(12,13,14); triangle_count++; AddTriangle(14,15,12); triangle_count++;
            
            vertexBuffer.SetData<VertexPositionNormalTexture>(vbuf_start*vbytes, vertices, 0, v_cnt, vbytes);
            vbuf_start = v_cnt; v_cnt = 0;
            indexBuffer.SetData<ushort>(ibuf_start*ibytes, indices, 0, i_cnt);
            ibuf_start = i_cnt; i_cnt = 0;
        }
    }
    
    // Common
    GraphicsDevice gpu;
    // Light light; // <-- Later
    BasicEffect basic_effect;
    Vector3 upDirection;
    const int vbytes = sizeof(float) * 8, ibytes = sizeof(ushort); // Each vertex contains 8 floats ( Postition(1,2,3), Normal(4,5,6), Texture(UV) coordinate) (7,8)

    // Geometry
    Matrix world; // For fixed geometry like landscape mesh
    public List<Obj3D> objects; // List of 3D objects to render
    // Static private allows nested classes to access:
    static ContentManager Content;
    static IndexBuffer indexBuffer; // holds and controls transfer of indices to GPU
    static VertexBuffer vertexBuffer; // holds and controls transfer of vertices to GPU
    static ushort[] indices; // index list for index buffer
    static VertexPositionNormalTexture[] vertices; // vertex list for assembling vertex buffer
    static int i_cnt, ibuf_start=0; // For making texture-sort id's, index-buffer's current starting  position
    static int v_cnt, vbuf_start=0; // v_cnt is for making current object, total_verts keep track of total verts
    static Dictionary<string, Texture2D> textures; // helps to store textures to only once and get desired texture by name

    public Basic3DObject(GraphicsDevice GPU, Vector3 UpDirection, ContentManager content){ // Light new_light
        gpu = GPU;
        world = Matrix.Identity;
        basic_effect = new BasicEffect(gpu); // light = new_light
        vertices = new VertexPositionNormalTexture[65535];
        indices = new ushort[65535];
        Content = content;
        upDirection = UpDirection;
        textures = new Dictionary<string, Texture2D>();
        vertexBuffer = new VertexBuffer(gpu, typeof(VertexPositionNormalTexture), 65535, BufferUsage.WriteOnly);
        indexBuffer = new IndexBuffer(gpu, typeof(ushort), 65535, BufferUsage.WriteOnly);
        objects = new List<Obj3D>();

        basic_effect.Alpha = 1f;
        basic_effect.LightingEnabled = true;
        basic_effect.AmbientLightColor = new Vector3(0.1f, 0.2f, 0.3f);
        basic_effect.DiffuseColor = new Vector3(0.94f, 0.94f, 0.94f);
        basic_effect.EnableDefaultLighting();
        basic_effect.TextureEnabled = true;
    }


    static void AddVertex(float x, float y, float z, Vector3 norm, float u, float v){
        if((vbuf_start + v_cnt) > 65530) { Console.WriteLine("Exceeded vertex buffer size"); return;}
        vertices[v_cnt] = new VertexPositionNormalTexture(new Vector3(x,y,z), norm, new Vector2(u,v));
        v_cnt++;
    }

    static void AddTriangle(ushort a, ushort b, ushort c){
        if((ibuf_start+3) > 65530) {Console.WriteLine("Exceeded index buffer size [may need UInt32 type]"); return;}
        ushort offset = (ushort)vbuf_start;
        a += offset; 
        b += offset; 
        c += offset; 
        indices[i_cnt] = a; i_cnt++;
        indices[i_cnt] = b; i_cnt++;
        indices[i_cnt] = c; i_cnt++;
    }


    static Texture2D LoadTexture(string name){
        Texture2D texture;
        if(textures.TryGetValue(name, out texture)== true){
            return texture;
        }
        else {
            texture = Content.Load<Texture2D>(name);
            textures.Add(name, texture);
            return texture;
        }
    }

    public void AddFloor(float width, float length, Vector3 mid_position, Vector3 rotation, String textureFile, Rectangle? sourceRect){
        Obj3D obj = new Obj3D();
        obj.AddQuad(mid_position, width, length, rotation, textureFile, sourceRect);
        objects.Add(obj);
    }

    public void AddCube(float width, float length, float height, Vector3 mid_postion, Vector3 rotation, String textureFile, Rectangle? sourceRect){
        Obj3D obj = new Obj3D();
        obj.AddCube(mid_postion, new Vector3(width, height, length), rotation, textureFile, sourceRect);
        objects.Add(obj);
    }

    public void Draw(Camera cam){
        gpu.SetVertexBuffer(vertexBuffer);
        gpu.Indices =indexBuffer;
        int obj_cnt = objects.Count, o;
        o = 0; while(o<obj_cnt){
            Obj3D obj = objects[o];
            // Shader param
            basic_effect.Texture = obj.tex;
            basic_effect.World = obj.transform;
            basic_effect.View = cam.view;
            basic_effect.Projection = cam.projection;
            foreach(EffectPass pass in basic_effect.CurrentTechnique.Passes){
                pass.Apply();
                gpu.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, obj.start_index, obj.triangle_count);
                o++;
            }
        }
    }

}