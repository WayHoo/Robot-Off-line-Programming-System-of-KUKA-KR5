using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIMI.PointCloud
{
    class JointAngle
    {
        /// <summary>
        /// 关节角1，弧度制
        /// </summary>
        public double theta1 { get; set; }

        /// <summary>
        /// 关节角2，弧度制
        /// </summary>
        public double theta2 { get; set; }

        /// <summary>
        /// 关节角3，弧度制
        /// </summary>
        public double theta3 { get; set; }

        /// <summary>
        /// 关节角4，弧度制
        /// </summary>
        public double theta4 { get; set; }

        /// <summary>
        /// 关节角5，弧度制
        /// </summary>
        public double theta5 { get; set; }

        /// <summary>
        /// 关节角6，弧度制
        /// </summary>
        public double theta6 { get; set; }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public JointAngle() {
            theta1 = 0;
            theta2 = 0;
            theta3 = 0;
            theta4 = 0;
            theta5 = 0;
            theta6 = 0;
        }

        public JointAngle(double var1,double var2,double var3,double var4,double var5,double var6)
        {
            theta1 = var1;
            theta2 = var2;
            theta3 = var3;
            theta4 = var4;
            theta5 = var5;
            theta6 = var6;
        }
        /// <summary>
        /// 将弧度制的成员变量以角度制的形式输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //角度制输出
            return (theta1 * 180 / Math.PI).ToString("#0.000000") + "\t" +
                (theta2 * 180 / Math.PI).ToString("#0.000000") + "\t" +
                (theta3 * 180 / Math.PI).ToString("#0.000000") + "\t" +
                (theta4 * 180 / Math.PI).ToString("#0.000000") + "\t" +
                (theta5 * 180 / Math.PI).ToString("#0.000000") + "\t" +
                (theta6 * 180 / Math.PI).ToString("#0.000000");
        }
    }
}
