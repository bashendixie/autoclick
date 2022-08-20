using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ImagesTools
    {
        /// <summary>
        /// 比较图片的相似性
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        public static double Compare_SSIM(Mat im1, Mat im2, int window = 7, float k1 = 0.01f, float k2 = 0.03f, float L = 255f)
        {
            im1.ConvertTo(im1, MatType.CV_32FC1);
            im2.ConvertTo(im2, MatType.CV_32FC1);

            int ndim = im1.Dims;
            double NP = Math.Pow(window, ndim);
            double cov_norm = NP / (NP - 1);
            double C1 = (k1 * L) * (k1 * L);
            double C2 = (k2 * L) * (k2 * L);

            Mat ux = new Mat(), uy = new Mat();
            Mat uxx = im1.Mul(im1);
            Mat uyy = im2.Mul(im2);
            Mat uxy = im1.Mul(im2);

            Cv2.Blur(im1, ux, new OpenCvSharp.Size(window, window), new OpenCvSharp.Point(-1, -1));
            Cv2.Blur(im2, uy, new OpenCvSharp.Size(window, window), new OpenCvSharp.Point(-1, -1));
            Cv2.Blur(uxx, uxx, new OpenCvSharp.Size(window, window), new OpenCvSharp.Point(-1, -1));
            Cv2.Blur(uyy, uyy, new OpenCvSharp.Size(window, window), new OpenCvSharp.Point(-1, -1));
            Cv2.Blur(uxy, uxy, new OpenCvSharp.Size(window, window), new OpenCvSharp.Point(-1, -1));

            Mat ux_sq = ux.Mul(ux);
            Mat uy_sq = uy.Mul(uy);
            Mat uxy_m = ux.Mul(uy);

            Mat vx = cov_norm * (uxx - ux_sq);
            Mat vy = cov_norm * (uyy - uy_sq);
            Mat vxy = cov_norm * (uxy - uxy_m);

            Mat A1 = 2 * uxy_m;
            Mat A2 = 2 * vxy;
            Mat B1 = ux_sq + uy_sq;
            Mat B2 = vx + vy;

            Mat ssim_map = (A1 + C1).Mul(A2 + C2) / (B1 + C1).Mul(B2 + C2);

            Scalar mssim = Cv2.Mean(ssim_map);
            ssim_map.ConvertTo(ssim_map, MatType.CV_8UC1, 255, 0);

            //imshow("ssim", ssim_map);

            return mssim[0];

            /* Mat validImage1, validImage2;
             image1.convertTo(validImage1, CV_32F); //数据类型转换为 float,防止后续计算出现错误
             image2.convertTo(validImage2, CV_32F);

             Mat image1_1 = validImage1.mul(validImage1); //图像乘积
             Mat image2_2 = validImage2.mul(validImage2);
             Mat image1_2 = validImage1.mul(validImage2);

             Mat gausBlur1, gausBlur2, gausBlur12;
             GaussianBlur(validImage1, gausBlur1, Size(11, 11), 1.5); //高斯卷积核计算图像均值
             GaussianBlur(validImage2, gausBlur2, Size(11, 11), 1.5);
             GaussianBlur(image1_2, gausBlur12, Size(11, 11), 1.5);

             Mat imageAvgProduct = gausBlur1.mul(gausBlur2); //均值乘积
             Mat u1Squre = gausBlur1.mul(gausBlur1); //各自均值的平方
             Mat u2Squre = gausBlur2.mul(gausBlur2);

             Mat imageConvariance, imageVariance1, imageVariance2;
             Mat squreAvg1, squreAvg2;
             GaussianBlur(image1_1, squreAvg1, Size(11, 11), 1.5); //图像平方的均值
             GaussianBlur(image2_2, squreAvg2, Size(11, 11), 1.5);

             imageConvariance = gausBlur12 - gausBlur1.mul(gausBlur2);// 计算协方差
             imageVariance1 = squreAvg1 - gausBlur1.mul(gausBlur1); //计算方差
             imageVariance2 = squreAvg2 - gausBlur2.mul(gausBlur2);

             auto member = ((2 * gausBlur1.mul(gausBlur2) + C1).mul(2 * imageConvariance + C2));
             auto denominator = ((u1Squre + u2Squre + C1).mul(imageVariance1 + imageVariance2 + C2));

             Mat ssim = new Mat() ;
             Cv2.Divide(member, denominator, ssim);
             return ssim;*/
        }
    }
}
