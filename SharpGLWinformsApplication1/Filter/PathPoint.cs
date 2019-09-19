using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIMI.PointCloud
{
    /// <summary>
    /// 内容摘要: 本类是关节空间轨迹规划中路径点的处理类，即直线插补路径点信息，
    /// 圆弧插补最终会转换为直线插补，转换算法在GCode类中实现
    /// 注意：该类中点的坐标单位为毫米，在openGL中单位为米，单位转换在SharpGLForm类中完成
    /// </summary>
    class PathPoint
    {
        /// <summary>
        /// 终止点的X坐标
        /// </summary>
        public double EX { get; set; }

        /// <summary>
        /// 终止点的Y坐标
        /// </summary>
        public double EY { get; set; }

        /// <summary>
        /// 终止点的Z坐标
        /// </summary>
        public double EZ { get; set; }

        /// 默认构造器
        /// </summary>
        public PathPoint()
        {
            EX = 0;
            EY = 0;
            EZ = 0;
        }

        /// <summary>
        /// double型含参构造器
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        /// <param name="sz"></param>
        public PathPoint(double ex, double ey, double ez)
        {
            EX = ex;
            EY = ey;
            EZ = ez;
        }

        public override string ToString()
        {
            //四舍五入保留三位小数
            return "G1X" + EX.ToString("#0.000") + "Y" + EY.ToString("#0.000") + "Z" + EZ.ToString("#0.000");
        }
    }
}
