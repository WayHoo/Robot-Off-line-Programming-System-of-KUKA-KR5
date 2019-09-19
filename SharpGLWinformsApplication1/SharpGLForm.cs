using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SharpGL;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;

namespace BIMI.PointCloud
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        public SharpGLForm()
        {
            InitializeComponent();
        }

        #region 变量

        #region 机器人
        private DHParam DH;
        private List<List<Triangle>> listTrianglesRobot = new List<List<Triangle>>();
        private List<int> jointIndexes = new List<int>();
        private Point robotCenter;
        private double robotScale;
        private double robotScaleRatio;         // 控制机器人显示比例，改变robotScaleRange实现
        private double robotScaleRange = 5;
        private double[] varTheta = { 0, 0, 0, 0, 0, 0 };// 所有关节转动角度（弧度）
        
        private double var0 = 0;    // 关节1转动角（度）
        private double var1 = 0;    // 关节2转动角（度）
        private double var2 = 0;    // 关节3转动角（度）
        private double var3 = 0;    // 关节4转动角（度）
        private double var4 = 0;    // 关节5转动角（度）
        private double var5 = 0;    // 关节6转动角（度）

        private uint robotList;       // 用于显示列表起始编号
        #endregion


        #region 工具姿态矩阵
        private double sx = 0.5, sy = 0, sz = -0.132;//工作台坐标系原点相对于基坐标系的坐标
        private double x = 0, y = 0, z = 0;//目标工作点在工作台坐标系中的坐标
        private double r11 = 1, r21 = 0, r31 = 0;//腕部坐标系X轴相对于参考坐标系的方向余弦
        private double r12 = 0, r22 = -1, r32 = 0;//腕部坐标系Y轴相对于参考坐标系的方向余弦，实际中不会用到
        private double r13 = 0, r23 = 0, r33 = -1;//腕部坐标系Z轴相对于参考坐标系的方向余弦
        private double px = 0, py = 0, pz = 0;//腕部坐标系原点在参考坐标系中的坐标
        private double l = 0.2;//工具坐标系原点到腕部坐标系原点的距离
        private double[] theta1 = { 0, 0, 0, 0, 0, 0, 0, 0 };//弧度制,8组解
        private double[] theta2 = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] theta3 = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] theta4 = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] theta5 = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] theta6 = { 0, 0, 0, 0, 0, 0, 0, 0 };
        #endregion


        #region 仿真起始点和终止点参数
        private List<PathPoint> listPathPoint;//处理G代码所得的直线插补起止点信息
        private List<JointAngle> listJointAngle = new List<JointAngle>();//所有路径点对应的逆解（关节角为弧度制）
        private double[] start_theta = { 0, 0, 0, 0, 0, 0 };//角度制
        private double[] end_theta = { 0, 0, 0, 0, 0, 0 };  //角度制
        private int duration = 0;//仿真持续时间，单位为ms
        private int nowTime = 0;//仿真当前时间，单位为ms
        private double speed = 0;//仿真时中间路径点角速度(始终设置为0，若不为0会出bug，我也不知道为什么？？？)
        private int jointAngleIndex = 1;//遍历listJointAngle的下标
        private bool motionSimuFlag = false;//用于记录是否是在仿真过程中更改trackbar的值，避免trackbar值更改再次触动valueChanged事件
        //三次插值多项式各项系数
        private double[] k0 = { 0, 0, 0, 0, 0, 0 };//常数项系数
        private double[] k1 = { 0, 0, 0, 0, 0, 0 };//一次项系数
        private double[] k2 = { 0, 0, 0, 0, 0, 0 };//二次项系数
        private double[] k3 = { 0, 0, 0, 0, 0, 0 };//三次项系数
        #endregion


        #region G代码数值范围及文件名
        private GCode.GCodeFeature gCodeFeature;//G代码在X、Y方向最大数值变化
        private double maxXRange = 500;//实际绘制曲线时G代码在X方向允许的最大数值变化范围
        private double minXRange = 100;//实际绘制曲线时G代码在X方向允许的最小数值变化范围
        private double maxYRange = 1000;//实际绘制曲线时G代码在Y方向允许的最大数值变化范围
        private double minYRange = 200;//实际绘制曲线时G代码在Y方向允许的最小数值变化范围
        private double curveScaleRatio = 3;//实际G代码数值放大比例（需根据实际读取的G代码进行计算）
        private double maxCurveScaleRatio = 5;//G代码数值最大放大比例（需根据实际读取的G代码进行计算）
        private double minCurveScaleRatio = 1;//G代码数值最小放大比例（需根据实际读取的G代码进行计算）
        private string gCodeFilePath = "";//tap文件绝对路径
        #endregion


        #region 工件
        private List<Triangle> listTrianglesScene = new List<Triangle>();
        private Point sceneCenter;
        private double sceneScale;
        private double sceneScaleRatio;
        private double sceneScaleRange = 5;
        private int gridLineNum = 50; // 网格线数
        private double lightRotate = 0.0f; // 光源1旋转
        #endregion


        #region 相机
        private MyCamera mycamera;
        private const double INITIAL_FOV = 40;          // 初始化相机广角
        private double fov = INITIAL_FOV;               // 相机初始广角
        private double znear = 0.01;                    // 相机透视投影近视点
        private double zfar = 1000;                     // 相机透视投影远视点
        private double fov_speed = 2;                   // 相机放大视图速度
        private double fov_max = 120;                   // 相机最大广角
        private double move_campos_x = 0;               // 按住Q键再点击并移动鼠标控制
        private double move_campos_y = 0;
        private double move_campos_speed = 0.001;
        private bool first_move_campos_mouse = true;
        #endregion


        #region 操作机器人
        private bool mouseDownFlag;
        private double move_robot_x = 0;                // Z和C键控制
        private double move_robot_y = 0;                // S和X键控制
        private double move_robot_z = 0;                // A和D键控制
        private double move_robot_speed = 0.1;
        #endregion


        #region 光源材质
        // 光源
        private float[] lightPos0 = { 30, 30, 30, 0f };             // 光源0（w为1表示位置光源，为0表示方向性光源）
        private float[] lightPos1 = { 20, 20, 20, 1f };             // 光源1
        private float[] lightAmbient = { 0f, 0f, 0f, 1f };          // 环境光
        private float[] lightDiffuse = { 1f, 1f, 1f, 1f };          // 漫射光
        private float[] lightSpecular = { 1f, 1f, 1f, 1f };         // 镜面光
        /*
         * 对于材质设置，一般材质的环境光反射和漫射光反射属性设置相同，可以达到真实的效果
         */
        // 机器人材质
        private float[] robot_no_mat = { 0.0f, 0.0f, 0.0f, 1.0f };                // 无材质颜色
        private float[] robot_mat_ambient_diffuse = { 0.2f, 0.5f, 0.8f, 1.0f };   // 蓝色
        private float[] robot_mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };          // 镜面反射
        private float[] robot_shininess = { 50.0f };                        // 镜面反射指数为50.0
        private float[] robot_mat_emission = { 0.3f, 0.2f, 0.3f, 0.0f };          // 发射光颜色
        // 工件材质
        private float[] sence_no_mat = { 0.0f, 0.0f, 0.0f, 1.0f };                // 无材质颜色
        private float[] sence_mat_ambient_diffuse = { 0.2f, 0.2f, 0.2f, 1.0f };   // 灰色
        private float[] sence_mat_specular = { 0.5f, 0.5f, 0.5f, 0.5f };          // 镜面反射
        private float[] sence_shininess = { 10.0f };                              // 镜面反射指数为10.0

        // 线条材质
        private float[] curve_no_mat = { 0.0f, 0.0f, 0.0f, 1.0f };                // 无材质颜色
        private float[] curve_mat_ambient_diffuse = { 1.0f, 0.0f, 0.0f, 1.0f };   // 红色
        private float[] curve_mat_specular = { 0.5f, 0.5f, 0.5f, 0.5f };          // 镜面反射
        private float[] curve_shininess = { 30.0f };                           // 镜面反射指数为30.0
        #endregion

        #endregion


        #region OpenGLConTrol控件自带标准函数
        /// <summary>
        /// 窗口以一定的帧率自动重绘
        /// </summary>
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.ClearColor(1.0f, 1.0f, 1.0f, 0.5f);     // 设置界面颜色
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            // 改变相机广角实现缩放
            mycamera.FieldOfView = fov;
            // 调用sharpGL实现的继承自Camera（相机基类）的投影函数来设置投影方式（这里为透视投影）和LookAt相机参数
            mycamera.Project(gl);

            // 移动整个视图
            gl.Translate(move_campos_x, move_campos_y, 0);

            // 在X,Z平面上绘制网格
            DrawGrid(gl);

            //控制模型各关节旋转          
            // 关节1
            varTheta[0] = var0 * Math.PI / 180;      // 转动范围：（-170，170)度 
            // 关节2
            varTheta[1] = var1 * Math.PI / 180;      // 转动范围：（-90，90)度 
            // 关节3 
            varTheta[2] = var2 * Math.PI / 180;      // 转动范围：（-220，80)度 
            // 关节4
            varTheta[3] = var3 * Math.PI / 180;      // 转动范围：（-170，170)度 
            // 关节5
            varTheta[4] = var4 * Math.PI / 180;      // 转动范围：（-100，100)度 
            // 关节6
            varTheta[5] = var5 * Math.PI / 180;      // 转动范围：（-220，220)度 

            // 绘制导入的机器人
            DrawRobot(gl);

            // 绘制导入的场景模型
            DrawScene(gl);

            //工具末端点移动过程中绘制曲线
            DrawCurve(gl);

            // 光源LIGHT1绕场景旋转
            gl.PushMatrix();
            gl.Rotate(lightRotate, 0, 1, 0);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, lightPos1);
            gl.PopMatrix();
            // 光源旋转
            lightRotate += 0.5;
        }

        /// <summary>
        /// 窗口初始化
        /// </summary>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;

            // 刷新颜色缓冲区时所用的颜色.
            gl.ClearColor(0, 0, 0, 0);

            // 机器人模型（KUKA KR5）
            AddJoint(@"\Data\STL\Robot\KUKA KR5\BIN\KUKA KR5 MDH - Base-1.STL", 0);
            AddJoint(@"\Data\STL\Robot\KUKA KR5\BIN\KUKA KR5 MDH - 1st Axis Arm-1.STL", 1);
            AddJoint(@"\Data\STL\Robot\KUKA KR5\BIN\KUKA KR5 MDH - 2nd Axis Arm-1.STL", 2);
            AddJoint(@"\Data\STL\Robot\KUKA KR5\BIN\KUKA KR5 MDH - 3rd Axis Arm-2.STL", 3);
            AddJoint(@"\Data\STL\Robot\KUKA KR5\BIN\KUKA KR5 MDH - 4th Axis Arm-2.STL", 4);
            AddJoint(@"\Data\STL\Robot\KUKA KR5\BIN\KUKA KR5 MDH - 5th Axis Arm-1.STL", 5);
            AddJoint(@"\Data\STL\Robot\KUKA KR5\BIN\KUKA KR5 MDH - 6th Axis Arm Tool-2.STL", 6);

            // 场景模型（工件）
            AddScene(@"\Data\STL\Scene\Workstation.STL");

            // DH参数
            SetDH();

            // 初始化相机
            SetCamera();

            // 创建并初始化显示列表
            SetDisList(gl);

            // 环境初始化配置
            SetRenderContext(gl);

            // 为鼠标滚轮注册事件（控件本身无直接的滚轮事件）
            this.openGLControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseWheel);         

            //TrackBar值初始化
            trkbrJoint1.Value = 0;
            trkbrJoint2.Value = 0;
            trkbrJoint3.Value = 0;
            trkbrJoint4.Value = 0;
            trkbrJoint5.Value = 0;
            trkbrJoint6.Value = 0;

            //显示TrackBar值的TextBox初始化
            txtJoint1Value.Text = " " + 0 + "°";
            txtJoint2Value.Text = " " + 0 + "°";
            txtJoint3Value.Text = " " + 0 + "°";
            txtJoint4Value.Text = " " + 0 + "°";
            txtJoint5Value.Text = " " + 0 + "°";
            txtJoint6Value.Text = " " + 0 + "°";

            //按键初始化
            btnSimu.Enabled = false;
            btnPauseStart.Enabled = false;
            btnPauseStart.Visible = false;
            trkbrScaleRatio.Enabled = false;

            //控制ScaleRatio和Accuracy的trackbar初始化
            trkbrScaleRatio.Maximum = 9;
            trkbrScaleRatio.Minimum = 1;
            trkbrScaleRatio.Value = 5;//初始值为中间值
            trkbrAccuracy.Maximum = 10;
            trkbrAccuracy.Minimum = 1;
            trkbrAccuracy.Value = 5;//初始精度为0.5
        }   

        /// <summary>
        /// 窗口大小改变后截面重绘
        /// </summary>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            mycamera.Project(gl);
        }
        #endregion


        #region 机器人数据导入

        /// <summary>
        /// 添加机器人关节数据（BIN格式）
        /// 程序判断文件类型为BIN还是ASCII尚未实现
        /// </summary>
        /// <param name="fileName">关节名</param>
        /// <param name="jointIndex">关节索引</param>
        private void AddJoint(string fileName, int jointIndex)
        {
            // 获取全路径（StartupPath在bin\debug路径下）
            string fn = Application.StartupPath + fileName;
            // 二进制格式数据
            listTrianglesRobot.Add(STLFilter.LoadFromBINFile(fn));
            // 导入的关节数据位置匹配关节索引，后面根据索引顺序绘制
            jointIndexes.Add(jointIndex);

            // 求整个导入模型的特征
            var listTriangleRobot = new List<Triangle>();//此处效率较低，每次都需要重新遍历机器人各个关节三角片数组
            foreach (var list in listTrianglesRobot)
            {
                listTriangleRobot.AddRange(list);
            }
            STLFilter.ModelFeature modelFeature = STLFilter.GetModelFeature(listTriangleRobot);
            // 中心
            robotCenter = modelFeature.modelCenter;
            // 最大尺度
            robotScale = modelFeature.modelScale;

            // 缩放到指定robotScaleRange尺度内的缩放率
            robotScaleRatio = robotScaleRange / robotScale;
        }

        /// <summary>
        /// 添加场景数据（工件）
        /// </summary>
        /// <param name="fileName"></param>
        private void AddScene(string fileName)
        {
            // 获取全路径
            string fn = Application.StartupPath + fileName;
            listTrianglesScene.AddRange(STLFilter.LoadFromBINFile(fn));
            STLFilter.ModelFeature modelFeature = STLFilter.GetModelFeature(listTrianglesScene);
            sceneCenter = modelFeature.modelCenter;
            sceneScale = modelFeature.modelScale;
            sceneScaleRatio = sceneScaleRange / sceneScale;//同机器人缩放比例一致
        }

        /// <summary>
        /// DH参数设置
        /// </summary>
        private void SetDH()
        {          
            // KUKA KR5机器人，MDH建模参数   
            double[] theta = { 0, -Math.PI / 2, 0, 0, 0, 0 };
            double[] d = { 0, 0, 0, 0.404, 0, 0 };
            double[] a = { 0, 0.075, 0.365, 0.09, 0, 0 };
            double[] alpha = { 0, -Math.PI / 2, 0, -Math.PI / 2, Math.PI / 2, -Math.PI / 2 };

            trkbrJoint1.Maximum = 170;
            trkbrJoint1.Minimum = -170;
            trkbrJoint2.Maximum = 90;
            trkbrJoint2.Minimum = -90;
            trkbrJoint3.Maximum = 80;
            trkbrJoint3.Minimum = -220;
            trkbrJoint4.Maximum = 190;
            trkbrJoint4.Minimum = -190;
            trkbrJoint5.Maximum = 120;
            trkbrJoint5.Minimum = -120;
            trkbrJoint6.Maximum = 350;
            trkbrJoint6.Minimum = -350;

            DH = new DHParam(theta, d, a, alpha);
        }

        /// <summary>  
        /// DH参数类
        /// </summary>
        class DHParam
        {
            /// <summary>
            /// 输入DH参数
            /// </summary>
            public DHParam(double[] _theta, double[] _d, double[] _a, double[] _alpha)
            {
                theta = _theta;
                d = _d;
                a = _a;
                alpha = _alpha;
                numOfLinks = theta.Length;
                Matrixs = new List<double[]>();
                Coordinate = new List<PathPoint>();
                Matrixs = GetMatrixs();
            }

            public int numOfLinks;
            public double[] theta;
            public double[] d;
            public double[] a;
            public double[] alpha;
            public List<double[]> Matrixs;
            public List<PathPoint> Coordinate;

            /// <summary>
            /// 四阶矩阵乘法
            /// </summary>
            /// <param name="leftMatrix"></param>
            /// <param name="rightMatrix"></param>
            /// <returns></returns>
            private DenseMatrix MultiMatrix(DenseMatrix leftMatrix,DenseMatrix rightMatrix)
            {
                var matrix = new DenseMatrix(4, 4);
                for(int i = 0;i < 4; i++)
                {
                    for(int j = 0;j < 4; j++)
                    {
                        matrix[i, j] = 0;
                        for(int k = 0;k < 4; k++)
                        {
                            matrix[i, j] += leftMatrix[i, k] * rightMatrix[k, j];
                        }
                    }
                }
                return matrix;
            }

            /// <summary>
            /// 根据六个关节角，求取工具末端点在世界坐标系中的坐标
            /// </summary>
            /// <param name="varTheta">六个角度制关节角的数组</param>
            /// <returns>返回值为PathPoint类型，单位为米</returns>
            public PathPoint GetCoordinate(double[] varTheta)
            {
                for(int i = 0;i < 6; i++)
                {
                    varTheta[i] = varTheta[i] * Math.PI / 180;//角度值参数转换为弧度制参数
                }
                DenseMatrix preMatrix = new DenseMatrix(4, 4);//基座坐标系相对于世界坐标系变换矩阵
                DenseMatrix aftMatrix = new DenseMatrix(4, 4);//工具坐标系相对于腕部坐标系的变换矩阵

                /*********坐标系变换矩阵初始化********/
                for (int i = 0;i < 4; i++)
                {
                    for(int j = 0;j < 4; j++)
                    {
                        preMatrix[i, j] = 0;
                        aftMatrix[i, j] = 0;
                    }
                }
                preMatrix[0, 0] = 1;
                preMatrix[2, 1] = -1;
                preMatrix[1, 2] = 1;
                preMatrix[1, 3] = 0.333;
                preMatrix[3, 3] = 1;

                aftMatrix[0, 0] = 1;
                aftMatrix[1, 1] = 1;
                aftMatrix[2, 2] = 1;
                aftMatrix[3, 3] = 1;
                aftMatrix[2, 3] = 0.2;
                /*********坐标系变换矩阵初始化********/

                DenseMatrix ansMatrix = preMatrix;
                for(int i = 0;i < 6; i++)
                {
                    ansMatrix = MultiMatrix(ansMatrix, DHToMatrix(theta[i], d[i], a[i], alpha[i]));
                    DenseMatrix rotateMatrix = VarDenseMatrix(varTheta[i]);
                    ansMatrix = MultiMatrix(ansMatrix, rotateMatrix);
                }
                ansMatrix = MultiMatrix(ansMatrix, aftMatrix);
                PathPoint pathPoint = new PathPoint(ansMatrix[0, 3], ansMatrix[1, 3], ansMatrix[2, 3]);
                return pathPoint;
            }

            /// <summary>
            /// 由DH参数求取旋转平移变换矩阵
            /// </summary>
            private DenseMatrix DHToMatrix(double _theta, double _d, double _a, double _alpha)
            {
                var matrix = new DenseMatrix(4, 4);
                /*
                 * MDH建模方式
                 * M=Rot(x,alpha)*Trans(x,a)*Rot(z,theta)*Trans(z,d)
                 * 首先绕x方向旋转alpha角，然后朝x方向平移a，接着绕z方向旋转theta角，最后沿z方向平移d
                 */
                matrix[0, 0] = Math.Cos(_theta);
                matrix[0, 1] = -Math.Sin(_theta);
                matrix[0, 2] = 0;
                matrix[0, 3] = _a;
                matrix[1, 0] = Math.Sin(_theta) * Math.Cos(_alpha);
                matrix[1, 1] = Math.Cos(_theta) * Math.Cos(_alpha);
                matrix[1, 2] = -Math.Sin(_alpha);
                matrix[1, 3] = -_d * Math.Sin(_alpha);
                matrix[2, 0] = Math.Sin(_theta) * Math.Sin(_alpha);
                matrix[2, 1] = Math.Cos(_theta) * Math.Sin(_alpha);
                matrix[2, 2] = Math.Cos(_alpha);
                matrix[2, 3] = _d * Math.Cos(_alpha);
                matrix[3, 0] = 0;
                matrix[3, 1] = 0;
                matrix[3, 2] = 0;
                matrix[3, 3] = 1;
                return matrix;
            }

            /// <summary>
            /// 根据旋转角vartheta求取旋转变换矩阵
            /// </summary>
            /// <param name="_vartheta">关节转动角（弧度制）</param>
            /// <returns>列优先存储的矩阵向量</returns>
            public double[] VarMatrix(double _vartheta)
            {
                var matrix = new DenseMatrix(4, 4);
                matrix[0, 0] = Math.Cos(_vartheta);
                matrix[0, 1] = -Math.Sin(_vartheta);
                matrix[0, 2] = 0;
                matrix[0, 3] = 0;
                matrix[1, 0] = Math.Sin(_vartheta);
                matrix[1, 1] = Math.Cos(_vartheta);
                matrix[1, 2] = 0;
                matrix[1, 3] = 0;
                matrix[2, 0] = 0;
                matrix[2, 1] = 0;
                matrix[2, 2] = 1;
                matrix[2, 3] = 0;
                matrix[3, 0] = 0;
                matrix[3, 1] = 0;
                matrix[3, 2] = 0;
                matrix[3, 3] = 1;
                return matrix.ToColumnMajorArray();
            }

            /// <summary>
            /// 根据旋转角vartheta求取旋转变换矩阵
            /// </summary>
            /// <param name="_vartheta"></param>
            /// <returns>矩阵</returns>
            private DenseMatrix VarDenseMatrix(double _vartheta)
            {
                var matrix = new DenseMatrix(4, 4);
                matrix[0, 0] = Math.Cos(_vartheta);
                matrix[0, 1] = -Math.Sin(_vartheta);
                matrix[0, 2] = 0;
                matrix[0, 3] = 0;
                matrix[1, 0] = Math.Sin(_vartheta);
                matrix[1, 1] = Math.Cos(_vartheta);
                matrix[1, 2] = 0;
                matrix[1, 3] = 0;
                matrix[2, 0] = 0;
                matrix[2, 1] = 0;
                matrix[2, 2] = 1;
                matrix[2, 3] = 0;
                matrix[3, 0] = 0;
                matrix[3, 1] = 0;
                matrix[3, 2] = 0;
                matrix[3, 3] = 1;
                return matrix;
            }

            /// <summary>
            /// 获取所有关节间的旋转变换矩阵（以列优先方式存储）
            /// </summary>
            /// <returns>列优先方式存储的所有关节间的旋转变换矩阵</returns>
            private List<double[]> GetMatrixs()
            {
                for (int i = 0; i < numOfLinks; i++)
                {
                    Matrixs.Add(DHToMatrix(theta[i], d[i], a[i], alpha[i]).ToColumnMajorArray());  //OpenGL中矩阵以列优先方式存储
                }
                return Matrixs;
            }
        }
        #endregion


        #region 图形和模型绘制部分

        /// <summary>
        /// 绘制网格平面
        /// </summary>
        private void DrawGrid(OpenGL gl)
        {
            // 绘制网格时关闭光照
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT);
            gl.PushMatrix();
            // 调用显示列表绘制模型
            gl.CallList(robotList + (uint)listTrianglesRobot.Count);
            gl.PopMatrix();
            gl.PopAttrib();
        }

        /// <summary>
        /// 载入到显示列表的网格绘制部分
        /// </summary>
        private void ListOfGrid(OpenGL gl)
        {
            for (int i = -gridLineNum; i <= gridLineNum; i += 2)
            {
                gl.Begin(OpenGL.GL_LINES);     // GL_LINES表示一次绘制多条线段
                {
                    if (i == 0 || i == -gridLineNum || i == gridLineNum)
                        gl.Color(0.5f, 0.5f, 0.5f);
                    else
                        gl.Color(0.15f, 0.15f, 0.15f);
                    //X轴方向
                    gl.Vertex(-gridLineNum, 0f, i);
                    gl.Vertex(gridLineNum, 0f, i);
                    //Z轴方向 
                    gl.Vertex(i, 0f, -gridLineNum);
                    gl.Vertex(i, 0f, gridLineNum);
                }
                gl.End();
            }
        }

        /// <summary>
        /// 绘制导入的机器人（实现关节间的旋转）
        /// </summary>
        private void DrawRobot(OpenGL gl)
        {
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT);
            gl.PushMatrix();
            // 缩放至合适大小
            gl.Scale(robotScaleRatio, robotScaleRatio, robotScaleRatio);
            // 移动机器人
            gl.Translate(move_robot_x, move_robot_y, move_robot_z);

            /*****KUKA KR5*****/
            // 由于基座坐标系并不在底座上，因此需要移动模型到合适显示位置
            gl.Translate(0.0f, 0.333f, 0.0f);
            /*****KUKA KR5*****/       

            // 旋转模型到合适显示位置（对于机器人建模时高度方向为Z向的需要进行旋转，因为在此视图中y向才是高度方向）
            gl.Rotate(-90, 1.0f, 0.0f, 0.0f);

            /*
             * 调用显示列表绘制模型
             * 控制模型的各个关节进行串联运动
             */

            // 绘制机器人基座
            gl.CallList(robotList);

            /*
             * 先将每个关节自身坐标系变换到世界坐标系，在进行关节旋转变换
             */

            // 绘制机器人关节1  
            gl.MultMatrix(DH.Matrixs[0]);
            gl.MultMatrix(DH.VarMatrix(varTheta[0]));
            gl.CallList(robotList + 1);

            // 绘制机器人关节2
            gl.MultMatrix(DH.Matrixs[1]);
            gl.MultMatrix(DH.VarMatrix(varTheta[1]));          
            gl.CallList(robotList + 2);

            // 绘制机器人关节3
            gl.MultMatrix(DH.Matrixs[2]);
            gl.MultMatrix(DH.VarMatrix(varTheta[2]));
            gl.CallList(robotList + 3);

            // 绘制机器人关节4
            gl.MultMatrix(DH.Matrixs[3]);
            gl.MultMatrix(DH.VarMatrix(varTheta[3]));
            gl.CallList(robotList + 4);

            // 绘制机器人关节5
            gl.MultMatrix(DH.Matrixs[4]);
            gl.MultMatrix(DH.VarMatrix(varTheta[4]));
            gl.CallList(robotList + 5);

            // 绘制机器人关节6
            gl.MultMatrix(DH.Matrixs[5]);
            gl.MultMatrix(DH.VarMatrix(varTheta[5]));
            gl.CallList(robotList + 6);

            gl.PopMatrix();
            gl.PopAttrib();
        }


        /// <summary>
        /// 载入到显示列表的机器人绘制部分
        /// </summary>
        /// <param name="gl"></param>
        /// <param name="n">第n个关节</param>
        private void ListOfRobot(OpenGL gl, int n)
        {
            // 开启光照
            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);
            gl.Enable(OpenGL.GL_LIGHT1);

            // 设置模型材质（可以从源STL文件中获取）
            SetRobotMaterial(gl);

            // 绘制模型
            gl.Begin(OpenGL.GL_TRIANGLES);
            for (int i = 0; i < listTrianglesRobot[n].Count; i++)
            {
                gl.Normal(listTrianglesRobot[n][i].Normal.Nx, listTrianglesRobot[n][i].Normal.Ny, listTrianglesRobot[n][i].Normal.Nz);  //设置三角面的法线
                gl.Vertex(listTrianglesRobot[n][i].P1.X, listTrianglesRobot[n][i].P1.Y, listTrianglesRobot[n][i].P1.Z);
                gl.Vertex(listTrianglesRobot[n][i].P2.X, listTrianglesRobot[n][i].P2.Y, listTrianglesRobot[n][i].P2.Z);
                gl.Vertex(listTrianglesRobot[n][i].P3.X, listTrianglesRobot[n][i].P3.Y, listTrianglesRobot[n][i].P3.Z);
            }
            gl.End();
        }

        /// <summary>
        /// 绘制导入的工件
        /// </summary>
        private void DrawScene(OpenGL gl)
        {
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT);
            gl.PushMatrix();

            //为保证工作台的单位尺度与机器人一致，此处使用机器人的缩放尺度
            gl.Scale(robotScaleRatio, robotScaleRatio, robotScaleRatio);

            gl.Translate(0.2f, 0.01f, 0.0f);//将工件沿X轴正方向移动200mm

            gl.CallList(robotList + (uint)listTrianglesRobot.Count + 1);

            gl.PopMatrix();
            gl.PopAttrib();
        }

        /// <summary>
        /// 载入到显示列表的工件绘制部分
        /// </summary>
        private void ListOfScene(OpenGL gl)
        {
            // 设置工件材料
            SetSceneMaterial(gl);

            // 绘制模型
            gl.Begin(OpenGL.GL_TRIANGLES);
            for (int i = 0; i < listTrianglesScene.Count; i++)
            {
                gl.Normal(listTrianglesScene[i].Normal.Nx, listTrianglesScene[i].Normal.Ny, listTrianglesScene[i].Normal.Nz);  //设置三角面的法线
                gl.Vertex(listTrianglesScene[i].P1.X, listTrianglesScene[i].P1.Y, listTrianglesScene[i].P1.Z);
                gl.Vertex(listTrianglesScene[i].P2.X, listTrianglesScene[i].P2.Y, listTrianglesScene[i].P2.Z);
                gl.Vertex(listTrianglesScene[i].P3.X, listTrianglesScene[i].P3.Y, listTrianglesScene[i].P3.Z);
            }
            gl.End();
        }

        /// <summary>
        /// 实现机械臂工具移动过程中末端点绘制曲线
        /// </summary>
        /// <param name="gl"></param>
        private void DrawCurve(OpenGL gl)
        {
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT);
            gl.PushMatrix();

            // 缩放至合适大小
            gl.Scale(robotScaleRatio, robotScaleRatio, robotScaleRatio);

            //设置工具末端绘制的曲线的材质
            SetCurveMaterial(gl);

            //绘制曲线
            double[] initialJoint = { 0, 0, 0, 0, 0, 0 };
            PathPoint firstPoint = DH.GetCoordinate(initialJoint);
            float preCoorX = (float)firstPoint.EX;
            float preCoorY = (float)firstPoint.EY;
            float preCoorZ = (float)firstPoint.EZ;
            gl.Begin(OpenGL.GL_LINES);     // GL_LINES表示一次绘制多条线段
            gl.LineWidth(10.0f);
            for (int i = 0; i < DH.Coordinate.Count; i++)
            {
                {
                    gl.Vertex(preCoorX, preCoorY, preCoorZ);
                    gl.Vertex(DH.Coordinate[i].EX, DH.Coordinate[i].EY, DH.Coordinate[i].EZ);
                    preCoorX = (float)DH.Coordinate[i].EX;
                    preCoorY = (float)DH.Coordinate[i].EY;
                    preCoorZ = (float)DH.Coordinate[i].EZ;
                }
            }
            gl.End();
            gl.PopMatrix();
            gl.PopAttrib();
        }
        #endregion


        #region 显示列表

        private void SetDisList(OpenGL gl)
        {
            // 机器人显示列表
            robotList = gl.GenLists(listTrianglesRobot.Count + 2);
            if (robotList != 0)
            {
                for (int i = 0; i < listTrianglesRobot.Count; i++)
                {
                    gl.NewList(robotList + (uint)i, OpenGL.GL_COMPILE);
                    ListOfRobot(gl, i);
                    gl.EndList();
                }
                gl.NewList(robotList + (uint)listTrianglesRobot.Count, OpenGL.GL_COMPILE);
                ListOfGrid(gl);
                gl.EndList();
                gl.NewList(robotList + (uint)listTrianglesRobot.Count + 1, OpenGL.GL_COMPILE);
                ListOfScene(gl);
                gl.EndList();
            }
        }

        #endregion


        #region 相机设置

        /// <summary>
        /// 相机初始化
        /// </summary>
        private void SetCamera()
        {
            mycamera = new MyCamera
            {
                //透视投影参数
                FieldOfView = fov,
                AspectRatio = (double)Width / (double)Height,
                Near = znear,
                Far = zfar,
            };
        }

        /// <summary>
        /// 自定义FPS相机类（继承sharpGL的PerspectiveCamera类）
        /// </summary>
        class MyCamera : SharpGL.SceneGraph.Cameras.PerspectiveCamera
        {
            /// <summary>
            /// 相机内嵌向量类
            /// </summary>
            public class Vector3
            {
                public double Nx;
                public double Ny;
                public double Nz;
                public Vector3()
                {
                    Nx = 0;
                    Ny = 0;
                    Nz = 0;
                }
                public Vector3(double x, double y, double z)
                {
                    Nx = x;
                    Ny = y;
                    Nz = z;
                }
                public Vector3(Vector3 vector)
                {
                    Nx = vector.Nx;
                    Ny = vector.Ny;
                    Nz = vector.Nz;
                }
                /// <summary>
                /// 叉乘
                /// </summary>
                public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
                {
                    return new Vector3(vector1.Ny * vector2.Nz - vector1.Nz * vector2.Ny,
                                       vector1.Nz * vector2.Nx - vector1.Nx * vector2.Nz,
                                       vector1.Nx * vector2.Ny - vector1.Ny * vector2.Nx);
                }
                /// <summary>
                /// 正规化
                /// </summary>
                public static Vector3 Normalize(Vector3 vector1)
                {
                    double norm = vector1.Nx * vector1.Nx + vector1.Ny * vector1.Ny + vector1.Nz * vector1.Nz;
                    return new Vector3(vector1.Nx / norm, vector1.Ny / norm, vector1.Nz / norm);
                }
                /// <summary>
                /// 向量加
                /// </summary>
                public static Vector3 operator +(Vector3 vector1, Vector3 vector2)
                {
                    return new Vector3(vector1.Nx + vector2.Nx, vector1.Ny + vector2.Ny, vector1.Nz + vector2.Nz);
                }
                /// <summary>
                /// 向量减
                /// </summary>
                public static Vector3 operator -(Vector3 vector1, Vector3 vector2)
                {
                    return new Vector3(vector1.Nx - vector2.Nx, vector1.Ny - vector2.Ny, vector1.Nz - vector2.Nz);
                }
            }

            public Vector3 cameraPos = new Vector3(0, 10, 20);
            public Vector3 cameraFront = new Vector3(0, -5, -18);
            public Vector3 cameraUp = new Vector3(0, 1, 0);
            public SharpGL.SceneGraph.Core.ArcBall arcBall = new SharpGL.SceneGraph.Core.ArcBall();

            public MyCamera()
            {
                Name = "CameraOfMy";
            }

            public override void TransformProjectionMatrix(OpenGL gl)
            {
                // 获取窗口边界后设置轨迹球边界
                int[] viewport = new int[4];
                gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);
                arcBall.SetBounds(viewport[2], viewport[3]);
                // 实现透视投影
                gl.Perspective(FieldOfView, AspectRatio, Near, Far);

                // 调用LookAt相机
                /*
                 * 该函数定义一个视图矩阵，并与当前矩阵相乘。
                 * 第一组eyex, eyey,eyez 相机在世界坐标的位置
                 * 第二组centerx,centery,centerz 相机镜头对准的物体在世界坐标的位置
                 * 第三组upx,upy,upz 相机向上的方向在世界坐标中的方向
                 * 你把相机想象成为你自己的脑袋：
                 * 第一组数据就是脑袋的位置
                 * 第二组数据就是眼睛看的物体的位置
                 * 第三组就是头顶朝向的方向（因为你可以歪着头看同一个物体）。
                 */
                gl.LookAt(cameraPos.Nx, cameraPos.Ny, cameraPos.Nz,
                          (cameraPos + cameraFront).Nx, (cameraPos + cameraFront).Ny, (cameraPos + cameraFront).Nz,
                          cameraUp.Nx, cameraUp.Ny, cameraUp.Nz);

                arcBall.TransformMatrix(gl);
            }
        }
        #endregion


        #region 环境设置
        private void SetRenderContext(OpenGL gl)
        {
            // 设置纹理
            SetTexture(gl);
            // 设置光源
            SetLight(gl);
            // 法线向量规范化（增强显示效果,模型缩放时会影响法向量）,还可以采用GL_RESCALE_NORMAL在变换后恢复单位长度
            gl.Enable(OpenGL.GL_NORMALIZE);
            // 背面隐藏设置时需启用深度测试
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            // 启用阴影平滑
            gl.ShadeModel(OpenGL.GL_SMOOTH);
            // 深度测试的类型
            gl.DepthFunc(OpenGL.GL_LEQUAL);
            // 进行透视修正
            gl.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);
        }

        /// <summary>
        /// 设置纹理
        /// </summary>
        /// <param name="gl"></param>
        private void SetTexture(OpenGL gl)
        {
            // 常用加载纹理方式
            SharpGL.SceneGraph.Assets.Texture texture;
            texture = new SharpGL.SceneGraph.Assets.Texture();
            texture.Create(gl, Application.StartupPath + @"\Data\BMP\earth.bmp");  //创建星球纹理
        }

        /// <summary>
        /// 设置光照参数
        /// </summary>
        private void SetLight(OpenGL gl)
        {

            // 光源LIGHT0
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, lightAmbient);   // 环境光
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, lightDiffuse);   // 漫射光
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, lightSpecular); // 镜面反射光
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, lightPos0);     // 光源位置

            // 光源LIGHT1
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, lightAmbient);   // 环境光
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, lightDiffuse);   // 漫射光
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, lightSpecular); // 镜面反射光
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, lightPos1);     // 光源位置
        }

        /// <summary>
        /// 设置机器人材质
        /// </summary>
        private void SetRobotMaterial(OpenGL gl)
        {

            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT, robot_mat_ambient_diffuse);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_DIFFUSE, robot_mat_ambient_diffuse);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SPECULAR, robot_mat_specular);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SHININESS, robot_shininess);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_EMISSION, robot_no_mat);
        }
        /// <summary>
        /// 设置工件材质
        /// </summary>
        private void SetSceneMaterial(OpenGL gl)
        {
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT, sence_mat_ambient_diffuse);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_DIFFUSE, sence_mat_ambient_diffuse);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SPECULAR, sence_mat_specular);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SHININESS, sence_shininess);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_EMISSION, sence_no_mat);
        }

        /// <summary>
        /// 设置工具末端点绘制的曲线材质，无法直接使用gl.Color()函数来设置光照条件下的线条颜色
        /// </summary>
        /// <param name="gl"></param>
        private void SetCurveMaterial(OpenGL gl)
        {
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT, curve_mat_ambient_diffuse);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_DIFFUSE, curve_mat_ambient_diffuse);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SPECULAR, curve_mat_specular);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SHININESS, curve_shininess);
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_EMISSION, curve_no_mat);
        }
        #endregion


        #region 鼠标、键盘控制模型视图(sharpGL自带ArcBall类)

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            // 鼠标按下标志置位
            this.mouseDownFlag = true;
            // 为相机中的轨迹球类传入鼠标按下时鼠标点的坐标
            mycamera.arcBall.MouseDown(e.X, e.Y);
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownFlag)
            {
                // 按下鼠标右键后移动相机位置
                if (e.Button == MouseButtons.Right)
                {
                    MoveCameraPos(e.X, e.Y);
                }
                // 在按下按下鼠标左键或滚轮后利用轨迹球旋转视角
                else
                {
                    // 为相机中的轨迹球类传入鼠标移动时鼠标点的坐标
                    mycamera.arcBall.MouseMove(e.X, e.Y);
                }
            }
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            // 鼠标按下标志复位
            this.mouseDownFlag = false;
            // 为相机中的轨迹球类传入鼠标弹起时鼠标点的坐标
            mycamera.arcBall.MouseUp(e.X, e.Y);

            first_move_campos_mouse = true;   // 控制模型移动标志
        }

        private void openGLControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                fov -= 1 * fov_speed;   // 放大
            else
                fov += 1 * fov_speed;   // 缩小
            if (fov >= fov_max)
                fov = fov_max;          // 限制放大倍数
            else if (fov <= -fov_max)
                fov = -fov_max;         // 限制放大倍数
            else if (fov <= 1)
                fov = 1;
        }

        private void openGLControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                // 按ZC SX AD键来控制机器人左右前后上下移动
                case Keys.Z: 
                    {
                        move_robot_x -= 1 * move_robot_speed;
                        break;
                    }
                case Keys.C:
                    {
                        move_robot_x += 1 * move_robot_speed;
                        break;
                    }
                case Keys.S:
                    {
                        move_robot_y += 1 * move_robot_speed;
                        break;
                    }
                case Keys.X:
                    {
                        move_robot_y -= 1 * move_robot_speed;
                        break;
                    }
                case Keys.A:
                    {
                        move_robot_z += 1 * move_robot_speed;
                        break;
                    }
                case Keys.D:
                    {
                        move_robot_z -= 1 * move_robot_speed;
                        break;
                    }
            }
        }

        private int LastMoveCamPosX;
        private int LastMoveCamPosY;
        /// <summary>
        /// 鼠标右键移动相机位置
        /// </summary>
        private void MoveCameraPos(int xpos, int ypos)
        {
            if (first_move_campos_mouse)
            {
                LastMoveCamPosX = xpos;
                LastMoveCamPosY = ypos;
                first_move_campos_mouse = false;
            }
            move_campos_x += (xpos - LastMoveCamPosX) * move_campos_speed;
            move_campos_y -= (ypos - LastMoveCamPosY) * move_campos_speed;
        }

        #endregion


        #region 机器人逆运动学求解、仿真三次多项式系数求解
        /// <summary>
        /// 用于求机器人逆解，参数为全局变量的工具坐标系相对于工作台坐标系的变换矩阵
        /// </summary>
        /// <returns></returns>
        private Tuple<bool,JointAngle> IK_Resolve() {
            px = x + sx - l * r13;
            py = y + sy - l * r23;
            pz = z + sz - l * r33;

            /*********求逆解*********/
            //将所有用到Theta23角的地方直接代入sin23和cos23，空间换时间思想
            double a1 = 0.075;
            double a2 = 0.365;
            double a3 = 0.09;
            double d4 = 0.404;

            double[] tmpTheta1 = { 0, 0 };//弧度制，因为Atan2函数的返回值为弧度制        
            tmpTheta1[0] = Math.Atan2(py, px);
            tmpTheta1[1] = Math.Atan2(-py, -px);

            double[,] tmpTheta3 = new double[2, 2]; ;//tmpTheta3的4个值由tmpTheta1和自身正负号决定
            for (int i = 0; i < 2; i++)
            {
                double K = (px * px + py * py + pz * pz + a1 * a1 - a2 * a2 - a3 * a3 - d4 * d4 -
                    2 * a1 * px * Math.Cos(tmpTheta1[i]) - 2 * a1 * py * Math.Sin(tmpTheta1[i])) / (2 * a2);
                tmpTheta3[i, 0] = Math.Atan2(a3, d4) - Math.Atan2(K, Math.Sqrt(a3 * a3 + d4 * d4 - K * K));
                tmpTheta3[i, 1] = Math.Atan2(a3, d4) - Math.Atan2(K, -Math.Sqrt(a3 * a3 + d4 * d4 - K * K));
            }

            double[,] tmpTheta23 = new double[2, 2];//表示tmpTheta2+tmpTheta3的四个值
            double[,] sin23 = new double[2, 2];//sin23由tmpTheta1和tmpTheta3决定
            double[,] cos23 = new double[2, 2];//cos23由tmpTheta1和tmpTheta3决定
            double[,] tmpTheta2 = new double[2, 2];//tmpTheta2的四个值由tmpTheta1和tmpTheta3决定
            for (int i = 0; i < 2; i++)//i为tmpTheta1的下标
            {
                double data1 = px * Math.Cos(tmpTheta1[i]) + py * Math.Sin(tmpTheta1[i]) - a1;
                for (int j = 0; j < 2; j++)//i,j均为tmpTheta3的下标
                {
                    double data2 = a2 * Math.Sin(tmpTheta3[i, j]) - d4;
                    double data3 = a3 + a2 * Math.Cos(tmpTheta3[i, j]);
                    sin23[i, j] = (data1 * data2 - data3 * pz) / (data1 * data1 + pz * pz);
                    cos23[i, j] = (data1 * data3 + data2 * pz) / (data1 * data1 + pz * pz);
                    tmpTheta23[i, j] = Math.Atan2(sin23[i, j], cos23[i, j]);
                    tmpTheta2[i, j] = tmpTheta23[i, j] - tmpTheta3[i, j] + Math.PI / 2;//为什么加上pi/2就妥了，我也不知道
                }
            }
            double[,] tmpTheta4 = new double[2, 2];//tmpTheta4由tmpTheta1和tmpTheta3决定
            for (int i = 0; i < 2; i++)//i为tmpTheta1的下标
            {
                double tmpData1, tmpData2;
                tmpData1 = -r13 * Math.Sin(tmpTheta1[i]) + r23 * Math.Cos(tmpTheta1[i]);
                for (int j = 0; j < 2; j++)//j为tmpTheta3的下标
                {
                    tmpData2 = r33 * sin23[i, j] - r13 * Math.Cos(tmpTheta1[i]) * cos23[i, j] -
                                r23 * Math.Sin(tmpTheta1[i]) * cos23[i, j];
                    if (tmpData1 < 1e-5 && tmpData2 < 1e-5)//如果tmpData1和tmpData2都趋近于0则tmpTheta4可以任取，此处取为上一次的值
                    {
                        tmpTheta4[i, j] = varTheta[3];
                    }
                    else
                    {
                        tmpTheta4[i, j] = Math.Atan2(tmpData1, tmpData2);
                    }
                }
            }
            double[,] tmpTheta5 = new double[2, 2];//tmpTheta5由tmpTheta1和tmpTheta3决定
            for (int i = 0; i < 2; i++)
            {
                double sin5, cos5;
                for (int j = 0; j < 2; j++)
                {
                    sin5 = r33 * sin23[i, j] * Math.Cos(tmpTheta4[i, j]) -
                        r13 * (Math.Cos(tmpTheta1[i]) * cos23[i, j] * Math.Cos(tmpTheta4[i, j]) +
                        Math.Sin(tmpTheta1[i]) * Math.Sin(tmpTheta4[i, j])) -
                        r23 * (Math.Sin(tmpTheta1[i]) * cos23[i, j] * Math.Cos(tmpTheta4[i, j]) -
                        Math.Cos(tmpTheta1[i]) * Math.Sin(tmpTheta4[i, j]));
                    cos5 = -r13 * Math.Cos(tmpTheta1[i]) * sin23[i, j] -
                        r23 * Math.Sin(tmpTheta1[i]) * sin23[i, j] - r33 * cos23[i, j];
                    tmpTheta5[i, j] = Math.Atan2(sin5, cos5);
                }
            }
            double[,] tmpTheta6 = new double[2, 2];//tmpTheta6由tmpTheta1和tmpTheta3决定
            for (int i = 0; i < 2; i++)
            {
                double sin6, cos6;
                for (int j = 0; j < 2; j++)
                {
                    sin6 = r31 * sin23[i, j] * Math.Sin(tmpTheta4[i, j]) -
                        r11 * (Math.Cos(tmpTheta1[i]) * cos23[i, j] * Math.Sin(tmpTheta4[i, j]) -
                        Math.Sin(tmpTheta1[i]) * Math.Cos(tmpTheta4[i, j])) -
                        r21 * (Math.Sin(tmpTheta1[i]) * cos23[i, j] * Math.Sin(tmpTheta4[i, j]) +
                        Math.Cos(tmpTheta1[i]) * Math.Cos(tmpTheta4[i, j]));
                    cos6 = r11 * ((Math.Cos(tmpTheta1[i]) * cos23[i, j] * Math.Cos(tmpTheta4[i, j]) +
                        Math.Sin(tmpTheta1[i]) * Math.Sin(tmpTheta4[i, j])) * Math.Cos(tmpTheta5[i, j]) -
                        Math.Cos(tmpTheta1[i]) * sin23[i, j] * Math.Sin(tmpTheta5[i, j])) +

                        r21 * ((Math.Sin(tmpTheta1[i]) * cos23[i, j] * Math.Cos(tmpTheta4[i, j]) -
                        Math.Cos(tmpTheta1[i]) * Math.Sin(tmpTheta4[i, j])) * Math.Cos(tmpTheta5[i, j]) -
                        Math.Sin(tmpTheta1[i]) * sin23[i, j] * Math.Sin(tmpTheta5[i, j])) -

                        r31 * (sin23[i, j] * Math.Cos(tmpTheta4[i, j]) * Math.Cos(tmpTheta5[i, j]) +
                        cos23[i, j] * Math.Sin(tmpTheta5[i, j]));
                    tmpTheta6[i, j] = Math.Atan2(sin6, cos6);
                }
            }

            for (int i = 0, k = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    theta1[k] = theta1[k + 4] = tmpTheta1[i];
                    theta2[k] = theta2[k + 4] = tmpTheta2[i, j];
                    theta3[k] = theta3[k + 4] = tmpTheta3[i, j];
                    theta4[k] = tmpTheta4[i, j];
                    theta4[k + 4] = tmpTheta4[i, j] + Math.PI;
                    theta5[k] = tmpTheta5[i, j];
                    theta5[k + 4] = -tmpTheta5[i, j];
                    theta6[k] = tmpTheta6[i, j];
                    theta6[k + 4] = tmpTheta6[i, j] + Math.PI;
                    k++;
                }
            }
            /*********求逆解*********/

            bool isResolved = false;//判断是否有逆解标志
            JointAngle tmpJointAngle = null;
            for (int i = 0; i < 8; i++)
            {
                if (theta1[i] * 180 / Math.PI >= trkbrJoint1.Minimum && theta1[i] * 180 / Math.PI <= trkbrJoint1.Maximum &&
                    theta2[i] * 180 / Math.PI >= trkbrJoint2.Minimum && theta2[i] * 180 / Math.PI <= trkbrJoint2.Maximum &&
                    theta3[i] * 180 / Math.PI >= trkbrJoint3.Minimum && theta3[i] * 180 / Math.PI <= trkbrJoint3.Maximum &&
                    theta4[i] * 180 / Math.PI >= trkbrJoint4.Minimum && theta4[i] * 180 / Math.PI <= trkbrJoint4.Maximum &&
                    theta5[i] * 180 / Math.PI >= trkbrJoint5.Minimum && theta5[i] * 180 / Math.PI <= trkbrJoint5.Maximum &&
                    theta6[i] * 180 / Math.PI >= trkbrJoint6.Minimum && theta6[i] * 180 / Math.PI <= trkbrJoint6.Maximum)
                {
                    isResolved = true;
                    //经测试，总是只有第一组解才是最优逆解，因此不必筛选
                    tmpJointAngle = new JointAngle(theta1[i], theta2[i],theta3[i], theta4[i], theta5[i], theta6[i]);
                    break;
                }
            }
            Tuple<bool, JointAngle> retTup = new Tuple<bool, JointAngle>(isResolved, tmpJointAngle);
            return retTup;
        }

        //private Dictionary<double,double> dicDertaTheta = new Dictionary<double, double>();
        /// <summary>
        /// 计算仿真过程中的三次多项式各阶系数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="startSpeed">仿真起始点角速度</param>
        /// <param name="endSpeed">仿真终止点角速度</param>
        private void CalcMotionPara(int index, double startSpeed, double endSpeed)
        {
            end_theta[0] = listJointAngle[index].theta1 * 180 / Math.PI;
            end_theta[1] = listJointAngle[index].theta2 * 180 / Math.PI;
            end_theta[2] = listJointAngle[index].theta3 * 180 / Math.PI;
            end_theta[3] = listJointAngle[index].theta4 * 180 / Math.PI;
            end_theta[4] = listJointAngle[index].theta5 * 180 / Math.PI;
            end_theta[5] = listJointAngle[index].theta6 * 180 / Math.PI;
            double[] dertaTheta = { 0, 0, 0, 0, 0, 0 };//角度制
            for (int i = 0; i < 6; i++)
            {
                dertaTheta[i] = Math.Abs(start_theta[i] - end_theta[i]);
            }
            double maxDertaTheta = 0;
            for (int i = 0; i < 6; i++)
            {
                if (dertaTheta[i] > maxDertaTheta)
                {
                    maxDertaTheta = dertaTheta[i];
                }
            }
            duration = (int)(maxDertaTheta * 1000 * Math.PI / 360);//按照关节角最大变化求仿真持续时间,角速度2rad/s
            duration = (duration / timSimu.Interval) * timSimu.Interval;//处理为10的倍数
            if(duration > 200)
            {
                duration = 200;
            }else if(duration == 0)
            {
                duration = 1;
            }
            /********用于输出*********/
            /*
            if (!dicDertaTheta.ContainsKey(maxDertaTheta))
            {
                dicDertaTheta.Add(maxDertaTheta, duration);
            }
            */
            /********用于输出*********/

            for (int i = 0; i < 6; i++)//计算三次插值多项式的各项系数
            {
                k0[i] = start_theta[i];
                k1[i] = startSpeed;//仿真起始点角速度
                k2[i] = 3 * (end_theta[i] - start_theta[i]) / (duration * duration) - 2 * startSpeed / duration - endSpeed / duration;
                k3[i] = -2 * (end_theta[i] - start_theta[i]) / (duration * duration * duration) + (startSpeed + endSpeed) / duration;
            }
            for(int i = 0;i < 6; i++)
            {
                start_theta[i] = end_theta[i];
            }
        }

        /// <summary>
        /// 计算处理后G代码所有路径点对应的关节角逆解
        /// </summary>
        private void CalcPathJoint()
        {
            /*********计算所有路径点的逆解*********/
            double preX = 0, preY = 0, preZ = 0;//加工起始点坐标
            double curX = 0, curY = 0, curZ = 0;//加工终止点坐标

            listJointAngle.Clear();//重新计算时清理数组

            //G代码中的第一个点的逆解单独计算
            r11 = 1; r21 = 0; r31 = 0;
            r12 = 0; r22 = -1; r32 = 0;
            r13 = 0; r23 = 0; r33 = -1;
            int index = 0;
            for (; index < listPathPoint.Count; index++)
            {
                x = preX = listPathPoint[index].EX * curveScaleRatio / 1000;
                y = preY = listPathPoint[index].EY * curveScaleRatio / 1000;
                z = preZ = listPathPoint[index].EZ * curveScaleRatio / 1000;
                Tuple<bool, JointAngle> tmpTup = IK_Resolve();
                if (tmpTup.Item1)//有逆解
                {
                    /*
                    listJointAngle.Add(new JointAngle(tmpTup.Item2.theta1, tmpTup.Item2.theta2, 
                        tmpTup.Item2.theta3, tmpTup.Item2.theta4, tmpTup.Item2.theta5, tmpTup.Item2.theta6));
                    */
                    //关节角6无条件置零，避免仿真时张牙舞爪
                    listJointAngle.Add(new JointAngle(tmpTup.Item2.theta1, tmpTup.Item2.theta2,
                        tmpTup.Item2.theta3, tmpTup.Item2.theta4, tmpTup.Item2.theta5, 0));
                    break;
                }
            }
            for (int i = index + 1; i < listPathPoint.Count; i++)
            {
                /*
                 * 下面分三种情况。
                 * 第一种：当前点坐标与上一个点坐标完全一致，直接跳过当前点即可；
                 * 第二种：当前点坐标与上一个点坐标仅Z坐标不同，此时变换矩阵仅z值发生变化，坐标轴余弦分量与先前一致；
                 * 第三种：当前点坐标与上一个点坐标Z坐标相同，X、Y坐标发生变化，此时需要根据公式求当前点的坐标变换矩阵；
                 */
                curX = listPathPoint[i].EX * curveScaleRatio / 1000;
                curY = listPathPoint[i].EY * curveScaleRatio / 1000;
                curZ = listPathPoint[i].EZ * curveScaleRatio / 1000;
                if (curX == preX && curY == preY && curZ == preZ)//check the first situation
                {
                    continue;
                }
                else //check the second and the third situation
                {
                    if (curZ == preZ)//check the third situation
                    {
                        double length = Math.Sqrt((curX - preX) * (curX - preX) +
                        (curY - preY) * (curY - preY));
                        double sinTheta = (curY - preY) / length;
                        double cosTheta = (curX - preX) / length;
                        r11 = cosTheta;
                        r21 = sinTheta;
                        r31 = 0;
                        r12 = sinTheta;
                        r22 = -cosTheta;
                        r32 = 0;
                        r13 = 0;
                        r23 = 0;
                        r33 = -1;
                    }
                    x = curX;
                    y = curY;
                    z = curZ;
                    Tuple<bool, JointAngle> tmpTup = IK_Resolve();
                    if (tmpTup.Item1)//有逆解
                    {
                        /*
                        listJointAngle.Add(new JointAngle(tmpTup.Item2.theta1, tmpTup.Item2.theta2,
                            tmpTup.Item2.theta3, tmpTup.Item2.theta4, tmpTup.Item2.theta5, tmpTup.Item2.theta6));
                        */
                        //关节角6无条件置零，避免仿真时张牙舞爪
                        listJointAngle.Add(new JointAngle(tmpTup.Item2.theta1, tmpTup.Item2.theta2,
                            tmpTup.Item2.theta3, tmpTup.Item2.theta4, tmpTup.Item2.theta5, 0));
                        preX = x;
                        preY = y;
                        preZ = z;
                    }
                }
            }
            /*********计算所有路径点的逆解*********/
        }

        /// <summary>
        /// 机器人各关节复位
        /// </summary>
        private void resetJointPos() {
            trkbrJoint1.Value = 0;
            trkbrJoint2.Value = 0;
            trkbrJoint3.Value = 0;
            trkbrJoint4.Value = 0;
            trkbrJoint5.Value = 0;
            trkbrJoint6.Value = 0;
            var0 = 0;
            var1 = 0;
            var2 = 0;
            var3 = 0;
            var4 = 0;
            var5 = 0;
        }
        #endregion


        #region 界面控件事件

        private void btnIK_Click(object sender, EventArgs e)
        {
            //直接从文本文件读取矩阵信息
            /*
            string fileName = Application.StartupPath + @"\Data\TestMatrix\ATSMatrix.txt";
            StreamReader SR = File.OpenText(fileName);
            string[] readLines = (SR.ReadToEnd()).Split('\n'); // Trim()方法在后面针对每行进行分割
            string[] line1 = readLines[0].Trim().Split('\t');    // 第一行
            string[] line2 = readLines[1].Trim().Split('\t');    // 第二行
            string[] line3 = readLines[2].Trim().Split('\t');    // 第三行

            x = Convert.ToDouble(line1[3]);
            y = Convert.ToDouble(line2[3]);
            z = Convert.ToDouble(line3[3]);

            r11 = Convert.ToDouble(line1[0]);
            r21 = Convert.ToDouble(line2[0]);
            r31 = Convert.ToDouble(line3[0]);

            r12 = Convert.ToDouble(line1[1]);
            r22 = Convert.ToDouble(line2[1]);
            r32 = Convert.ToDouble(line3[1]);

            r13 = Convert.ToDouble(line1[2]);
            r23 = Convert.ToDouble(line2[2]);
            r33 = Convert.ToDouble(line3[2]);
            */

            /**********测试读取文本文件***********/
            /*
            string testTXT = "";
            string blankChar = "     ";
            testTXT += r11 + blankChar + r12 + blankChar + r13 + blankChar + x + "\n"
                + r21 + blankChar + r22 + blankChar + r23 + blankChar + y + "\n"
                + r31 + blankChar + r32 + blankChar + r33 + blankChar + z;
            MessageBox.Show(testTXT);
            */
            /**********测试读取文本文件***********/

            x = double.Parse(txtX.Text = txtX.Text == "" ? "0" : txtX.Text);
            y = double.Parse(txtY.Text = txtY.Text == "" ? "0" : txtY.Text);
            z = double.Parse(txtZ.Text = txtZ.Text == "" ? "0" : txtZ.Text);

            r11 = double.Parse(txtr11.Text = txtr11.Text == "" ? "1" : txtr11.Text);
            r21 = double.Parse(txtr21.Text = txtr21.Text == "" ? "0" : txtr21.Text);
            r31 = double.Parse(txtr31.Text = txtr31.Text == "" ? "0" : txtr31.Text);

            r12 = double.Parse(txtr12.Text = txtr12.Text == "" ? "0" : txtr12.Text);
            r22 = double.Parse(txtr22.Text = txtr22.Text == "" ? "-1" : txtr22.Text);
            r32 = double.Parse(txtr32.Text = txtr32.Text == "" ? "0" : txtr32.Text);

            r13 = double.Parse(txtr13.Text = txtr13.Text == "" ? "0" : txtr13.Text);
            r23 = double.Parse(txtr23.Text = txtr23.Text == "" ? "0" : txtr23.Text);
            r33 = double.Parse(txtr33.Text = txtr33.Text == "" ? "-1" : txtr33.Text);

            Tuple<bool, JointAngle> tmpTup = IK_Resolve();
            if (!tmpTup.Item1)
            {
                MessageBox.Show("No inverse solution!");
            }
            else
            {
                var0 = tmpTup.Item2.theta1 * 180 / Math.PI;
                var1 = tmpTup.Item2.theta2 * 180 / Math.PI;
                var2 = tmpTup.Item2.theta3 * 180 / Math.PI;
                var3 = tmpTup.Item2.theta4 * 180 / Math.PI;
                var4 = tmpTup.Item2.theta5 * 180 / Math.PI;
                var5 = tmpTup.Item2.theta6 * 180 / Math.PI;
                trkbrJoint1.Value = (int)Math.Round(var0);
                trkbrJoint2.Value = (int)Math.Round(var1);
                trkbrJoint3.Value = (int)Math.Round(var2);
                trkbrJoint4.Value = (int)Math.Round(var3);
                trkbrJoint5.Value = (int)Math.Round(var4);
                trkbrJoint6.Value = (int)Math.Round(var5);

                string[] str = { "", "", "", "", "", "", "", "" };
                for (int i = 0; i < 8; i++)
                {
                    str[i] += "θ1 = " + (theta1[i] * 180 / Math.PI).ToString("#0.00") + "°\t";
                    str[i] += "θ2 = " + (theta2[i] * 180 / Math.PI).ToString("#0.00") + "°\t";
                    str[i] += "θ3 = " + (theta3[i] * 180 / Math.PI).ToString("#0.00") + "°\t";
                    str[i] += "θ4 = " + (theta4[i] * 180 / Math.PI).ToString("#0.00") + "°\t";
                    str[i] += "θ5 = " + (theta5[i] * 180 / Math.PI).ToString("#0.00") + "°\t";
                    str[i] += "θ6 = " + (theta6[i] * 180 / Math.PI).ToString("#0.00") + "°\n\n";
                }
                MessageBox.Show(str[0] + str[1] + str[2] + str[3] + str[4] + str[5] + str[6] + str[7]);
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Users\胡伟\Desktop";
            ofd.Title = "请选择导入tap文件";
            ofd.Filter = "tap文件|*.tap";
            DialogResult dialogResult = ofd.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                //得到打开的文件路径（包括文件名）
                gCodeFilePath = ofd.FileName.ToString();
                btnSimu.Enabled = true;
                trkbrScaleRatio.Enabled = true;

                //成功打开tap文件即开始初步处理G代码，主要是为了在仿真之前计算出CurveScaleRatio
                GCode.setDis(trkbrAccuracy.Value * 0.1);
                Tuple<List<PathPoint>, GCode.GCodeFeature> GCodeTup = GCode.HandleGCode(gCodeFilePath);
                listPathPoint = GCodeTup.Item1;
                gCodeFeature = GCodeTup.Item2;

                /******计算G代码数值最大、最小和实际放大比例******/
                double maxXScaleRatio = maxXRange / gCodeFeature.XRange;
                double minXScaleRatio = minXRange / gCodeFeature.XRange;
                double maxYScaleRatio = maxYRange / gCodeFeature.YRange;
                double minYScaleRatio = minYRange / gCodeFeature.YRange;
                maxCurveScaleRatio = maxXScaleRatio > maxYScaleRatio ? maxYScaleRatio : maxXScaleRatio;
                minCurveScaleRatio = minXScaleRatio > minYScaleRatio ? minXScaleRatio : minYScaleRatio;
                curveScaleRatio = (maxCurveScaleRatio + minCurveScaleRatio) / 2;
                lblScaleRatio.Text = "ScaleRatio:" + curveScaleRatio.ToString("#0.00");
                /******计算G代码数值最大、最小和实际放大比例******/

                MessageBox.Show("Open tap file successfully!\nPlease click corresponding button to simulate.");
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                MessageBox.Show("Sorry to inform you that you have chose no tap file.\nPlease try again!");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            /*
            //该button现用于测试正解程序
            PathPoint pathPoint = new PathPoint();
            double[] tmpTheta = { 10, 25, -10, 45, -60, 90 };
            pathPoint = DH.GetCoordinate(tmpTheta);
            MessageBox.Show("X = " + pathPoint.EX + "\tY = " + pathPoint.EY + "\tZ = " + pathPoint.EZ);
            */

            //MessageBox.Show("robotScaleRatio = " + robotScaleRatio);

            //MessageBox.Show("GXRange = " + gCodeFeature.XRange + "\tGYRange = " + gCodeFeature.YRange);

            /*
            //该button现用于输出G代码格式的各路径点坐标
            string outFileName = @"\Data\Test\fakeGCode.tap";
            StreamWriter SW = new StreamWriter(Application.StartupPath + outFileName);
            foreach (PathPoint pathPoint in DH.Coordinate)
            {
                string str = "G1X" + (pathPoint.EX * 1000).ToString("#0.000") + "Y" + 
                    (pathPoint.EZ * 1000).ToString("#0.000") + "Z" + 
                    (pathPoint.EY * 1000).ToString("#0.000");
                SW.WriteLine(str);
            }
            SW.Close();
            */
            DH.Coordinate.Clear();
        }

        private void btnSimu_Click(object sender, EventArgs e)
        {
            /**********测试G代码处理程序**********/
            /*
            int pos = gCodeFilePath.LastIndexOf(@"\");
            string tapFileName = gCodeFilePath.Substring(pos + 1);
            string outFileName = @"\Data\GCode\Processed_" + tapFileName;
            StreamWriter SW = new StreamWriter(Application.StartupPath + outFileName);
            foreach (PathPoint pathPoint in listPathPoint)
            {
                SW.WriteLine(pathPoint);
            }
            SW.Close();
            */
            /**********测试G代码处理程序**********/



            /******将关节角输出到文本文件******/
            /*
            string angleOutFileName = @"\Data\Test\jointAngle.txt";
            StreamWriter SW1 = new StreamWriter(Application.StartupPath + angleOutFileName);
            SW1.WriteLine("The total size is " + listJointAngle.Count);
            foreach (JointAngle jointAngle in listJointAngle)
            {
                SW1.WriteLine(jointAngle);
            }
            SW1.Close();
            */
            /******将关节角输出到文本文件******/

            //开始仿真后，如果检测到Accuracy发生了变化，则需要重新处理G代码
            if (trkbrAccuracy.Value * 0.1 != GCode.getDis())
            {
                GCode.setDis(trkbrAccuracy.Value * 0.1);
                Tuple<List<PathPoint>, GCode.GCodeFeature> GCodeTup = GCode.HandleGCode(gCodeFilePath);
                listPathPoint = GCodeTup.Item1;
                gCodeFeature = GCodeTup.Item2;
                /*
                 * 需要注意的是，由于在打开文件的时候已经计算过G代码数值最大、
                 * 最小和实际放大比例，当G代码插补步长改变后，放大比例不会改变，
                 * 因此无需重新计算
                */
            }

            CalcPathJoint();//在打开文件时初步处理了G代码，只是为了提前计算出ScaleRatio，此处计算路径关节角逆解

            resetJointPos();//机器人各关节复位
            for(int i = 0;i < 6; i++)
            {
                start_theta[i] = 0;
            }

            timSimu.Interval = 1;//计时器间隔为1ms
            CalcMotionPara(0, 0, speed);
            jointAngleIndex = 1;//重置下标
            nowTime = 0;//当前时间归零
            DH.Coordinate.Clear();
            timSimu.Enabled = true;//使能计时器
            btnSimu.Enabled = false;
            btnSimu.Visible = false;
            btnPauseStart.Text = "Pause";
            btnPauseStart.Enabled = true;
            btnPauseStart.Visible = true;
            btnOpenFile.Enabled = false;
            btnClear.Enabled = false;
            trkbrScaleRatio.Enabled = false;
            trkbrAccuracy.Enabled = false;
        }

        private void btnPauseStart_Click(object sender, EventArgs e)
        {
            if(btnPauseStart.Text == "Pause")
            {
                btnPauseStart.Text = "Start";
                timSimu.Enabled = false;
            }
            else
            {
                btnPauseStart.Text = "Pause";
                timSimu.Enabled = true;
            }
        }

        private void timSimu_Tick(object sender, EventArgs e)
        {
            nowTime += timSimu.Interval;
            double pow2 = nowTime * nowTime;
            double pow3 = nowTime * nowTime * nowTime;
            var0 = k0[0] + k1[0] * nowTime + k2[0] * pow2 + k3[0] * pow3;
            var1 = k0[1] + k1[1] * nowTime + k2[1] * pow2 + k3[1] * pow3;
            var2 = k0[2] + k1[2] * nowTime + k2[2] * pow2 + k3[2] * pow3;
            var3 = k0[3] + k1[3] * nowTime + k2[3] * pow2 + k3[3] * pow3;
            var4 = k0[4] + k1[4] * nowTime + k2[4] * pow2 + k3[4] * pow3;
            var5 = k0[5] + k1[5] * nowTime + k2[5] * pow2 + k3[5] * pow3;

            double[] tmpTheta = { var0, var1, var2, var3, var4, var5 };
            PathPoint tmpPathPoint = DH.GetCoordinate(tmpTheta);
            DH.Coordinate.Add(tmpPathPoint);

            motionSimuFlag = true;
            trkbrJoint1.Value = (int)Math.Round(var0);
            trkbrJoint2.Value = (int)Math.Round(var1);
            trkbrJoint3.Value = (int)Math.Round(var2);
            trkbrJoint4.Value = (int)Math.Round(var3);
            trkbrJoint5.Value = (int)Math.Round(var4);
            trkbrJoint6.Value = (int)Math.Round(var5);
            motionSimuFlag = false;

            if ( nowTime >= duration)
            {   
                if(++jointAngleIndex < listJointAngle.Count)
                {
                    nowTime = 0;//当前时间归零
                    timSimu.Enabled = false;//关闭计时器
                    if (jointAngleIndex == listJointAngle.Count - 1)
                    {
                        CalcMotionPara(jointAngleIndex, speed, 0);
                    }
                    else
                    {
                        CalcMotionPara(jointAngleIndex, speed, speed);
                    }
                    timSimu.Enabled = true;//使能计时器
                }
                else
                {
                    timSimu.Enabled = false;
                    btnPauseStart.Enabled = false;
                    btnPauseStart.Visible = false;
                    btnSimu.Visible = true;
                    btnSimu.Enabled = true;
                    btnOpenFile.Enabled = true;
                    btnClear.Enabled = true;
                    trkbrScaleRatio.Enabled = true;
                    trkbrAccuracy.Enabled = true;
                    /**********仿真时间输出**********/
                    /*
                    string outFileName = @"\Data\Test\duration.txt";
                    StreamWriter SW = new StreamWriter(Application.StartupPath + outFileName);
                    foreach (KeyValuePair<double,double> kvp in dicDertaTheta)
                    {
                        SW.WriteLine("maxDertaTheta:{0}\tduration:{1}",kvp.Key,kvp.Value);
                    }
                    SW.Close();
                    */
                    /**********仿真时间输出**********/
                }
            }
        }

        private void trkbrScaleRatio_ValueChanged(object sender, EventArgs e)
        {
            curveScaleRatio = minCurveScaleRatio +
                (maxCurveScaleRatio - minCurveScaleRatio) *
                (trkbrScaleRatio.Value - trkbrScaleRatio.Minimum) /
                 (trkbrScaleRatio.Maximum - trkbrScaleRatio.Minimum);
            lblScaleRatio.Text = "ScaleRatio:" + curveScaleRatio.ToString("#0.00");
        }

        private void trkbrAccuracy_ValueChanged(object sender, EventArgs e)
        {
            lblAccuracy.Text = "Accuracy:" + (trkbrAccuracy.Value * 0.1).ToString("0.0") + "mm";
        }

        private void trkbrJoint1_ValueChanged(object sender, EventArgs e)
        {
            if (!motionSimuFlag)//防止仿真过程中var值设定为trackbar的整型值造成关节抖动
            {
                var0 = trkbrJoint1.Value;
            }
            txtJoint1Value.Text = " " + trkbrJoint1.Value + "°";
        }

        private void trkbrJoint2_ValueChanged(object sender, EventArgs e)
        {
            if (!motionSimuFlag)
            {
                var1 = trkbrJoint2.Value;
            }
            txtJoint2Value.Text = " " + trkbrJoint2.Value + "°";
        }

        private void trkbrJoint3_ValueChanged(object sender, EventArgs e)
        {
            if (!motionSimuFlag)
            {
                var2 = trkbrJoint3.Value;
            }
            txtJoint3Value.Text = " " + trkbrJoint3.Value + "°";
        }

        private void trkbrJoint4_ValueChanged(object sender, EventArgs e)
        {
            if (!motionSimuFlag)
            {
                var3 = trkbrJoint4.Value;
            }
            txtJoint4Value.Text = " " + trkbrJoint4.Value + "°";
        }

        private void trkbrJoint5_ValueChanged(object sender, EventArgs e)
        {
            if (!motionSimuFlag)
            {
                var4 = trkbrJoint5.Value;
            }
            txtJoint5Value.Text = " " + trkbrJoint5.Value + "°";
        }

        private void trkbrJoint6_ValueChanged(object sender, EventArgs e)
        {
            if (!motionSimuFlag)
            {
                var5 = trkbrJoint6.Value;
            }
            txtJoint6Value.Text = " " + trkbrJoint6.Value + "°";
        }

        private void btnResetJointPos_Click(object sender, EventArgs e)
        {
            resetJointPos();//机器人各关节复位
        }

        /// <summary>
        /// 初始视角
        /// </summary>
        private void btnResetScene_Click(object sender, EventArgs e)
        {
            // 初始视角
            mycamera.cameraPos = new MyCamera.Vector3(0, 10, 20);
            mycamera.cameraFront = new MyCamera.Vector3(0, -5, -18);
            mycamera.cameraUp = new MyCamera.Vector3(0, 1, 0);

            // 相机位置复位
            move_campos_x = 0;
            move_campos_y = 0;

            fov = INITIAL_FOV;

            // 机器人位置复位
            move_robot_x = 0;
            move_robot_y = 0;

            // 轨迹球复位
            mycamera.arcBall = new SharpGL.SceneGraph.Core.ArcBall();
        }

        /// <summary>
        /// 左视
        /// </summary>
        private void btnLeftScene_Click(object sender, EventArgs e)
        {
            // 左视
            mycamera.cameraPos = new MyCamera.Vector3(-20, 10, 0);
            mycamera.cameraFront = new MyCamera.Vector3(18, -5, 0);
            mycamera.cameraUp = new MyCamera.Vector3(0, 1, 0);

            // 相机位置复位
            move_campos_x = 0;
            move_campos_y = 0;

            fov = INITIAL_FOV;

            // 轨迹球复位
            mycamera.arcBall = new SharpGL.SceneGraph.Core.ArcBall();
        }

        /// <summary>
        /// 右视
        /// </summary>
        private void btnRightScene_Click(object sender, EventArgs e)
        {
            // 右视
            mycamera.cameraPos = new MyCamera.Vector3(20, 10, 0);
            mycamera.cameraFront = new MyCamera.Vector3(-18, -5, 0);
            mycamera.cameraUp = new MyCamera.Vector3(0, 1, 0);

            // 相机位置复位
            move_campos_x = 0;
            move_campos_y = 0;

            fov = INITIAL_FOV;

            // 轨迹球复位
            mycamera.arcBall = new SharpGL.SceneGraph.Core.ArcBall();
        }

        /// <summary>
        /// 俯视
        /// </summary>
        private void btnTopScene_Click(object sender, EventArgs e)
        {
            // 俯视
            mycamera.cameraPos = new MyCamera.Vector3(0, 20, 0.01);        // Z分量不能取0
            mycamera.cameraFront = new MyCamera.Vector3(0, -20, -0.01);
            mycamera.cameraUp = new MyCamera.Vector3(0, 1, 0);

            // 相机位置复位
            move_campos_x = 0;
            move_campos_y = 0;

            fov = INITIAL_FOV;

            // 轨迹球复位
            mycamera.arcBall = new SharpGL.SceneGraph.Core.ArcBall();
        }

        /// <summary>
        /// 后视
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBehindScene_Click(object sender, EventArgs e)
        {
            // 后视
            mycamera.cameraPos = new MyCamera.Vector3(0, 10, -20);
            mycamera.cameraFront = new MyCamera.Vector3(0, -5, 18);
            mycamera.cameraUp = new MyCamera.Vector3(0, 1, 0);

            // 相机位置复位
            move_campos_x = 0;
            move_campos_y = 0;

            fov = INITIAL_FOV;

            // 机器人位置复位
            move_robot_x = 0;
            move_robot_y = 0;

            // 轨迹球复位
            mycamera.arcBall = new SharpGL.SceneGraph.Core.ArcBall();
        }
        #endregion
    }
}
