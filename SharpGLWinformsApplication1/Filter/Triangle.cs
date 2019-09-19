using System;

namespace BIMI.PointCloud
{
    /// <summary>
    /// 内容摘要: 本类是三角片体类，包括输出标准STL(ASCII)格式方法。
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// 顶点P1
        /// </summary>
        public Point P1 { get; private set; }//自动实现属性，只读

        /// <summary>
        /// 顶点P2
        /// </summary>
        public Point P2 { get; private set; }

        /// <summary>
        /// 顶点P3
        /// </summary>
        public Point P3 { get; private set; }

        /// <summary>
        /// 法线向量
        /// </summary>
        public Vector3f Normal { get; private set; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="n"></param>
        public Triangle(Point v1, Point v2, Point v3, Vector3f n)
        {
            P1 = v1;
            P2 = v2;
            P3 = v3;
            if (Math.Abs(n.Norm() - 1) < 1e-3)
            {
                Normal = n;
            }
            else
                Normal = new Vector3f(1, 0, 0);
        }

        /// <summary>
        /// 输出标准STL(ASCII)格式（未使用）
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string normalLine = "facet normal " + Normal.ToString() + "\n";
            string outLoopLine = "outer loop" + "\n";
            string p1Line = "vertex " + P1.ToXYZString() + "\n";
            string p2Line = "vertex " + P2.ToXYZString() + "\n";
            string p3Line = "vertex " + P3.ToXYZString() + "\n";
            string endLoopLine = "endloop" + "\n";
            string endFacet = "endfacet" + "\n";
            return normalLine + outLoopLine + p1Line + p2Line + p3Line + endLoopLine + endFacet;
        }
    }
}
