using UnityEngine;

[System.Serializable]
public struct Vector5
{
    public float X, Y, Z, W, V;

    public Vector5(float x, float y, float z, float w, float v)
    {
        X = x; Y = y; Z = z; W = w; V = v;
    }

    public static Vector5 Zero => new Vector5(0, 0, 0, 0, 0);

    public static Vector5 operator +(Vector5 a, Vector5 b)
        => new Vector5(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W, a.V + b.V);

    public static Vector5 operator *(Vector5 a, float s)
        => new Vector5(a.X * s, a.Y * s, a.Z * s, a.W * s, a.V * s);

    public float Dot(Vector5 b) => X * b.X + Y * b.Y + Z * b.Z + W * b.W + V * b.V;

    public float Magnitude() => Mathf.Sqrt(Dot(this));

    public Vector5 Normalized()
    {
        float m = Magnitude();
        return m > 1e-5f ? this * (1f / m) : Zero;
    }
}
