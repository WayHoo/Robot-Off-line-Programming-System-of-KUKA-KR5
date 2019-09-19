using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BIMI.PointCloud
{
    class GCode
    {
        private static double dis = 0.1;//圆弧插补转化为直线插补的默认步长

        /// <summary>
        /// 静态成员变量dis的set方法
        /// </summary>
        /// <param name="_dis"></param>
        public static void setDis(double _dis)
        {
            dis = _dis;
        }

        /// <summary>
        /// 静态成员变量dis的get方法
        /// </summary>
        /// <returns></returns>
        public static double getDis()
        {
            return dis;
        }

        public struct GCodeFeature
        {
            //G代码在X方向数值变化范围
            public double XRange;
            //G代码在Y方向数值变化范围
            public double YRange;
        }

        /// <summary>
        /// 从startIndex位置开始提取一个双精度浮点数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static double ExtractDouble(string str,int startIndex)
        {
            double value = 0;//提取的数值
            string strValue = "";//字符串形式的数值
            for (int i = startIndex; i < str.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9' || str[i] == '.' || str[i] == '-')
                {
                    strValue += str[i];
                }
                else
                {
                    value = Convert.ToDouble(strValue);
                    break;
                }
            }
                return value;
        }

        public static Tuple<List<PathPoint>,GCodeFeature> HandleGCode(string fileName)
        {
            List<PathPoint> listPathPoint = new List<PathPoint>();
            GCodeFeature retGCodeFeature = new GCodeFeature();
            double maxX = -10E6, maxY = -10E6;
            double minX = 10E6, minY = 10E6;
            StreamReader SR = File.OpenText(fileName);
            string[] readLines = (SR.ReadToEnd()).Split('\n'); // Trim()方法在后面针对每行进行分割
            int GType = 0;//G代码类型(1、2、3)，由于对G0/G1代码处理方式一致，因此归类为G1
            double preX = 0, preY = 0, preZ = 0;//注意初值的设定，后续会做出更改，单位为毫米
            for (int i = 0; i < readLines.Count(); i++)
            {
                readLines[i].Trim();//去掉字符串首尾的空格，正常情况下该操作多余
                if (readLines[i].Length == 0)//去掉空行
                    continue;
                if (readLines[i][0] == 'G')
                {
                    GType = (int)ExtractDouble(readLines[i], 1);
                    GType = (GType == 0 ? 1 : GType);//对G0/G1代码进行归类处理
                }
                else if (readLines[i][0] == 'X' || readLines[i][0] == 'Y' || readLines[i][0] == 'Z')
                    GType = 1;// 特殊G代码归位G1指令处理
                else
                    continue;
                // 经过G代码归类处理后开始进行直线插补转换
                if (GType == 1)
                {
                    double EX = preX, EY = preY, EZ = preZ;
                    int posX = readLines[i].IndexOf('X');
                    int posY = readLines[i].IndexOf('Y');
                    int posZ = readLines[i].IndexOf('Z');
                    if (posX != -1)
                        EX = ExtractDouble(readLines[i], posX + 1);
                    if (posY != -1)
                        EY = ExtractDouble(readLines[i], posY + 1);
                    if (posZ != -1)
                        EZ = ExtractDouble(readLines[i], posZ + 1);
                    double length = Math.Sqrt((EX - preX) * (EX - preX) + (EY - preY) * (EY - preY) + (EZ - preZ) * (EZ - preZ));
                    int pointNum = 2 + (int)(length / dis);
                    if(EZ == preZ)//XOY平面运动
                    {
                        if(EX != preX) // 直线斜率不为0
                        {
                            double dertaX = (EX - preX) * dis / length;
                            double k = (EY - preY) / (EX - preX);
                            for (int j = 1; j <= pointNum - 1; j++)
                            {
                                double tmpX = preX + (j - 1) * dertaX;
                                double tmpY = preY + k * (j - 1) * dertaX;
                                listPathPoint.Add(new PathPoint(tmpX, tmpY, preZ));
                                maxX = tmpX > maxX ? tmpX : maxX;
                                minX = tmpX < minX ? tmpX : minX;
                                maxY = tmpY > maxY ? tmpY : maxY;
                                minY = tmpY < minY ? tmpY : minY;
                            }
                        }
                        else// 直线斜率为0
                        {
                            int sign = EY > preY ? 1 : -1;// 确定符号
                            for (int j = 1; j <= pointNum - 1; j++)
                            {
                                double tmpY = preY + sign * (j - 1) * dis;
                                listPathPoint.Add(new PathPoint(preX, tmpY, preZ));
                            }
                        }
                    }
                    else//竖直运动
                    {
                        int sign = EZ > preZ ? 1 : -1;// 确定符号
                        for (int j = 1; j <= pointNum - 1; j++)
                        {
                            double tmpZ = preZ + sign * (j - 1) * dis;
                            listPathPoint.Add(new PathPoint(preX, preY, tmpZ));
                        }
                    }
                    listPathPoint.Add(new PathPoint(EX, EY, EZ));// 直接将终点纳入路径点
                    maxX = EX > maxX ? EX : maxX;
                    minX = EX < minX ? EX : minX;
                    maxY = EY > maxY ? EY : maxY;
                    minY = EY < minY ? EY : minY;
                    preX = EX;
                    preY = EY;
                    preZ = EZ;
                }
                else if(GType == 2 || GType == 3)//GType为2或3
                {
                    int posX, posY, posI, posJ;
                    double EX = 0, EY = 0, I = 0, J = 0;
                    posX = readLines[i].IndexOf('X');
                    posY = readLines[i].IndexOf('Y');
                    posI = readLines[i].IndexOf('I');
                    posJ = readLines[i].IndexOf('J');
                    if (posX != -1)
                        EX = ExtractDouble(readLines[i], posX + 1);
                    if (posY != -1)
                        EY = ExtractDouble(readLines[i], posY + 1);
                    if (posI != -1)
                        I = ExtractDouble(readLines[i], posI + 1);
                    if (posJ != -1)
                        J = ExtractDouble(readLines[i], posJ + 1);
                    double OX = I + preX;
                    double OY = J + preY;
                    double arfa = (Math.Atan2(preY - OY, preX - OX) + 2 * Math.PI) % (2 * Math.PI);
                    double R = Math.Sqrt(I * I + J * J);
                    double derta_theta = dis / R;
                    double length = Math.Sqrt((EX - preX) * (EX - preX) + (EY - preY) * (EY - preY));
                    double max_theta = 2 * Math.Asin(length / (2 * R));
                    int k = (int)(max_theta / derta_theta);
                    if (GType == 2)//顺时针
                    {
                        for (int j = 1; j <= k; j++)
                        {
                            double tmpX = OX + R * Math.Cos(arfa - j * derta_theta);
                            double tmpY = OY + R * Math.Sin(arfa - j * derta_theta);
                            listPathPoint.Add(new PathPoint(tmpX, tmpY, preZ));
                            maxX = tmpX > maxX ? tmpX : maxX;
                            minX = tmpX < minX ? tmpX : minX;
                            maxY = tmpY > maxY ? tmpY : maxY;
                            minY = tmpY < minY ? tmpY : minY;
                            preX = tmpX;
                            preY = tmpY;
                        }
                    }
                    else if (GType == 3)//逆时针
                    {
                        for (int j = 1; j <= k; j++)
                        {
                            double tmpX = OX + R * Math.Cos(arfa + j * derta_theta);
                            double tmpY = OY + R * Math.Sin(arfa + j * derta_theta);
                            listPathPoint.Add(new PathPoint(tmpX, tmpY, preZ));
                            maxX = tmpX > maxX ? tmpX : maxX;
                            minX = tmpX < minX ? tmpX : minX;
                            maxY = tmpY > maxY ? tmpY : maxY;
                            minY = tmpY < minY ? tmpY : minY;
                            preX = tmpX;
                            preY = tmpY;
                        }
                    }
                    listPathPoint.Add(new PathPoint(EX, EY, preZ));//这段直线插补长度有可能为0
                    maxX = EX > maxX ? EX : maxX;
                    minX = EX < minX ? EX : minX;
                    maxY = EY > maxY ? EY : maxY;
                    minY = EY < minY ? EY : minY;
                }
                else
                {
                    continue;
                }
            }
            SR.Close();
            retGCodeFeature.XRange = Math.Abs(maxX - minX);
            retGCodeFeature.YRange = Math.Abs(maxY - minY);
            Tuple<List<PathPoint>, GCodeFeature> retTup = new Tuple<List<PathPoint>, GCodeFeature>(listPathPoint, retGCodeFeature);
            return retTup;
        }
    }
}
