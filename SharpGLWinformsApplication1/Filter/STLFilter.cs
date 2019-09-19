using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BIMI.PointCloud
{
    /// <summary>
    /// 内容摘要: 本类是STL文件类，包括从STL文件加载ASCII格式点云数据方法。
    /// 修改内容: 添加将STL片体离散成点的方法SamplePointsFromSTL(按点密度或总点数)
    /// 待修改：识别STL文件为二进制格式还是ASCII格式的判断语句有问题（第42行）
    /// </summary>
    class STLFilter
    {
        /// <summary>
        /// 从STL文件加载点云数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>三角片体模型</returns>
        public static List<Triangle> LoadFromFile(string fileName)
        {
            StreamReader SR = File.OpenText(fileName);
            string[] readLines = (SR.ReadToEnd().Trim()).Split('\n');
            SR.Close();
            if (readLines.Count() > 8)  // STL文件为ASCII格式(ASCII格式文件至少有9行)
            {
                return LoadFromASCIIFile(fileName);
            }
            else // STL文件为BIN格式
            {
                return LoadFromBINFile(fileName);
            }
        }

        /// <summary>
        /// 从ASCII格式加载三角片体模型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>三角片体模型</returns>
        public static List<Triangle> LoadFromASCIIFile(string fileName)
        {
            List<Triangle> listTriangle = new List<Triangle>();
            StreamReader SR = File.OpenText(fileName);
            string[] readLines = (SR.ReadToEnd()).Split('\n'); // Trim()方法在后面针对每行进行分割
            if (readLines.Count() > 0 && readLines[0].ToUpper().Contains("SOLID"))  // 第一行包含SOLID
            {
                for (int i = 0; i < (readLines.Count() - 2) / 7; i++)   // 第一行和最后一行不读
                {
                    // 法向
                    string[] temp = readLines[7 * i + 1].Trim().Split(' ');    // 跳过第一行
                    Vector3f nomal = new Vector3f(Convert.ToSingle(temp[2]), Convert.ToSingle(temp[3]), Convert.ToSingle(temp[4]));

                    // 顶点1
                    temp = readLines[7 * i + 2 + 1].Trim().Split(' ');
                    Point vetex1 = new Point(Convert.ToSingle(temp[1]), Convert.ToSingle(temp[2]), Convert.ToSingle(temp[3]));

                    // 顶点2
                    temp = readLines[7 * i + 3 + 1].Trim().Split(' ');
                    Point vetex2 = new Point(Convert.ToSingle(temp[1]), Convert.ToSingle(temp[2]), Convert.ToSingle(temp[3]));

                    // 顶点3
                    temp = readLines[7 * i + 4 + 1].Trim().Split(' ');
                    Point vetex3 = new Point(Convert.ToSingle(temp[1]), Convert.ToSingle(temp[2]), Convert.ToSingle(temp[3]));
                    listTriangle.Add(new Triangle(vetex1, vetex2, vetex3, nomal));
                }
            }
            SR.Close();
            return listTriangle;
        }

        /// <summary>
        /// 从BIN格式加载三角片体模型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>三角片体模型</returns>
        public static List<Triangle> LoadFromBINFile(string fileName)
        {
            List<Triangle> listTriangle = new List<Triangle>();
            FileStream FS = new FileStream(fileName, FileMode.Open);
            byte[] bFileHead = new byte[80];
            FS.Read(bFileHead, 0, 80);
            byte[] triangleCount = new byte[4];
            FS.Read(triangleCount, 0, 4);
            int iTriangleCount = BitConverter.ToInt32(triangleCount, 0);
            for (int i = 0; i < iTriangleCount; i++)
            {
                // 每个三角面片占用固定的50字节
                byte[] triangle = new byte[50];
                FS.Read(triangle, 0, 50);
                // 法向
                float nX = BitConverter.ToSingle(triangle, 4 * 0);
                float nY = BitConverter.ToSingle(triangle, 4 * 1);
                float nZ = BitConverter.ToSingle(triangle, 4 * 2);
                Vector3f nomal = new Vector3f(nX, nY, nZ);

                // 顶点1
                float vetex1X = BitConverter.ToSingle(triangle, 4 * 3);
                float vetex1Y = BitConverter.ToSingle(triangle, 4 * 4);
                float vetex1Z = BitConverter.ToSingle(triangle, 4 * 5);
                Point vetex1 = new Point(vetex1X, vetex1Y, vetex1Z);

                // 顶点2
                float vetex2X = BitConverter.ToSingle(triangle, 4 * 6);
                float vetex2Y = BitConverter.ToSingle(triangle, 4 * 7);
                float vetex2Z = BitConverter.ToSingle(triangle, 4 * 8);
                Point vetex2 = new Point(vetex2X, vetex2Y, vetex2Z);

                // 顶点3
                float vetex3X = BitConverter.ToSingle(triangle, 4 * 9);
                float vetex3Y = BitConverter.ToSingle(triangle, 4 * 10);
                float vetex3Z = BitConverter.ToSingle(triangle, 4 * 11);
                Point vetex3 = new Point(vetex3X, vetex3Y, vetex3Z);
                listTriangle.Add(new Triangle(vetex1, vetex2, vetex3, nomal));
              
                //注：每个三角面片的最后2个字节用来描述三角面片的属性信息(包括颜色属性等)暂时没有用。      
            }
            FS.Close();
            return listTriangle;
        }

        /// <summary>
        /// 模型特征
        /// </summary>
        public struct ModelFeature
        {
            //模型中心
            public Point modelCenter;
            //模型尺度
            public double modelScale;
        }

        /// <summary>
        /// 获取模型特征
        /// </summary>
        /// <param name="listTriangle"></param>
        /// <returns>中心点</returns>
        public static ModelFeature GetModelFeature(List<Triangle> listTriangle)
        {
            try
            {
                List<double> XCoordinate = new List<double>();
                List<double> YCoordinate = new List<double>();
                List<double> ZCoordinate = new List<double>();
                foreach (Triangle triangle in listTriangle)
                {
                    XCoordinate.Add(triangle.P1.X);
                    XCoordinate.Add(triangle.P2.X);
                    XCoordinate.Add(triangle.P3.X);
                    YCoordinate.Add(triangle.P1.Y);
                    YCoordinate.Add(triangle.P2.Y);
                    YCoordinate.Add(triangle.P3.Y);
                    ZCoordinate.Add(triangle.P1.Z);
                    ZCoordinate.Add(triangle.P2.Z);
                    ZCoordinate.Add(triangle.P3.Z);
                }
                XCoordinate.Sort();
                YCoordinate.Sort();
                ZCoordinate.Sort();
                Point tmpModelCenter = new Point((XCoordinate[0] + XCoordinate[XCoordinate.Count - 1]) / 2,
                                             (YCoordinate[0] + YCoordinate[YCoordinate.Count - 1]) / 2,
                                             (ZCoordinate[0] + ZCoordinate[ZCoordinate.Count - 1]) / 2);
                //获取模型X,Y,Z方向最大尺寸
                double XMax = XCoordinate[XCoordinate.Count - 1] - XCoordinate[0];
                double YMax = YCoordinate[YCoordinate.Count - 1] - XCoordinate[0];
                double ZMax = XCoordinate[ZCoordinate.Count - 1] - XCoordinate[0];
                double tmpModelScale = (XMax > YMax) ? (XMax > ZMax ? XMax : ZMax) : (YMax > ZMax ? YMax : ZMax);
                ModelFeature modelFeature = new ModelFeature
                {
                    modelCenter = tmpModelCenter,
                    modelScale = tmpModelScale
                };
                return modelFeature;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("警告！模型数据存在问题");
                return new ModelFeature();
            }
        }

        /// <summary>
        /// 按密度离散点三角片体
        /// </summary>
        /// <param name="mesh">三角片体</param>
        /// <param name="samplingDensity">密度</param>
        /// <returns></returns>
        public static List<Point> SamplePointsFromSTL(List<Triangle> mesh, float samplingDensity)
        {
            List<Point> pointList = new List<Point>();
            Random rnd = new Random();
            foreach (Triangle triangle in mesh)
            {
                Point A = triangle.P1;
                Point B = triangle.P2;
                Point C = triangle.P3;

                Vector3f VA = new Vector3f(A.X, A.Y, A.Z);
                Vector3f AB = new Vector3f(A, B);
                Vector3f AC = new Vector3f(A, C);

                Vector3f N = AB.Cross(AC);
                float area = N.Norm() / 2;

                float pointsToAdd = area * samplingDensity;

                if (pointsToAdd < 1)
                {
                    pointsToAdd = 1;
                }

                for (int i = 0; i < pointsToAdd; ++i)
                {
                    float s = (float)rnd.NextDouble();
                    float t = (float)rnd.NextDouble();
                    if (s + t > 1)
                    {
                        s = 1 - s;
                        t = 1 - t;
                    }
                    Vector3f p = VA + s * AB + t * AC;

                    pointList.Add(new Point(p.Nx, p.Ny, p.Nz, triangle.Normal.Nx, triangle.Normal.Ny, triangle.Normal.Nz));
                    //法线能否使用插值实现
                }
            }
            return pointList;
        }

        /// <summary>
        /// 按总点数离散三角片体
        /// </summary>
        /// <param name="mesh">三角片体</param>
        /// <param name="totalNum">总点数</param>
        /// <returns></returns>
        public static List<Point> SamplePointsFromSTL(List<Triangle> mesh, int totalNum)
        {
            //先计算片体总表面积totalArea，samplingDensity=totalArea/totalNum
            float totalArea = 0.0f;
            foreach (Triangle triangle in mesh)
            {
                Point A = triangle.P1;
                Point B = triangle.P2;
                Point C = triangle.P3;

                Vector3f AB = new Vector3f(A, B);
                Vector3f AC = new Vector3f(A, C);

                totalArea += AB.Cross(AC).Norm();
            }
            totalArea /= 2;

            float samplingDensity = totalArea / totalNum;
            return SamplePointsFromSTL(mesh, samplingDensity);
        }
    }
}
