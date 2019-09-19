using System;

namespace BIMI.PointCloud
{
    /// <summary>
    /// 内容摘要: 本类是3维向量类
    /// </summary>
    public class Vector3f
    {
        /// <summary>
        /// x分量
        /// </summary>
        public double Nx { get; set; }

        /// <summary>
        /// y分量
        /// </summary>
        public double Ny { get; set; }

        /// <summary>
        /// z分量
        /// </summary>
        public double Nz { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Vector3f()
        {
            Nx = 0;
            Ny = 0;
            Nz = 1;
        }

        /// <summary>
        /// float型构造函数
        /// </summary>
        /// <param name="x">x分量值</param>
        /// <param name="y">y分量值</param>
        /// <param name="z">z分量值</param>
        public Vector3f(float x, float y, float z)
        {
            Nx = x;
            Ny = y;
            Nz = z;
        }

        /// <summary>
        /// double型构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3f(double x, double y, double z)
        {
            Nx = x;
            Ny = y;
            Nz = z;
        }

        /// <summary>
        /// 构造函数（AB向量）
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public Vector3f(Point A, Point B)
        {
            Nx = B.X - A.X;
            Ny = B.Y - A.Y;
            Nz = B.Z - A.Z;
        }

        /// <summary>
        /// 向量点积
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Dot(Vector3f v)
        {
            return (Nx * v.Nx) + (Ny * v.Ny) + (Nz * v.Nz);
        }

        /// <summary>
        /// 向量叉乘
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3f Cross(Vector3f v)
        {
            return new Vector3f((Ny * v.Nz) - (Nz * v.Ny), (Nz * v.Nx) - (Nx * v.Nz), (Nx * v.Ny) - (Ny * v.Nx));
        }

        /// <summary>
        /// 向量的模
        /// </summary>
        /// <returns></returns>
        public float Norm()
        {
            return (float)Math.Sqrt((Nx * Nx) + (Ny * Ny) + (Nz * Nz));
        }

        /// <summary>
        /// 数乘向量
        /// </summary>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3f operator *(float d, Vector3f v)
        {
            return new Vector3f(d * v.Nx, d * v.Ny, d * v.Nz);
        }

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <returns></returns>
        public static Vector3f operator +(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.Nx + v2.Nx, v1.Ny + v2.Ny, v1.Nz + v2.Nz);
        }
    }
}
