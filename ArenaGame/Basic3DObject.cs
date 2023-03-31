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
        public int traingle_count;
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

}