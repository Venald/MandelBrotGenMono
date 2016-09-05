using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MandelBrot
{
    class gen
    {
        /// <summary>
        /// Laskee mandelbrotin värin lainattu wikipediasta 
        /// https://en.wikipedia.org/wiki/Mandelbrot_set#Escape_time_algorithm
        /// </summary>
        /// <param name="x">nykyisen pikselin x</param>
        /// <param name="y">nykyisen pikselin y</param>
        /// <param name="xRes">x reso</param>
        /// <param name="yRes">y reso</param>
        /// <returns></returns>
        public static uint MandelBrotGen(float x, float y, float xRes, float yRes, int MAXITERATION)
        {

            float xScaled = Scale(x, 0.0f, xRes, -2.5f, 1f);
            float yScaled = Scale(y, 0.0f, yRes, -1.0f, 1.0f);
            Vector2 useScaling = new Vector2(xScaled, yScaled);
            Matrix userMatrix = new Matrix(
                0.5f, 0.0f, 0.0f, 0.0f,
                0.0f, 0.5f, 0.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 0.0f);
            useScaling = Vector2.Transform(useScaling, userMatrix);
            xScaled = useScaling.X;
            yScaled = useScaling.Y;
            float xM = 0.0f;
            float yM = 0.0f;
            //Vector xMyM = new Vector(xScaled,yScaled);
            double iteration = 0;
            //        while (xM * xM - yM * yM < 4 && iteration < MAXITERATION)
            while (xM * xM - yM * yM < 4 && iteration < MAXITERATION)
            {
                float xtemp = xM * xM - yM * yM + xScaled;
                float ytemp = 2 * xM * yM + yScaled;
                yM = ytemp;
                xM = xtemp;
                iteration += 1;
            }
            if (iteration < MAXITERATION)
            {
                double logZn = Math.Log(xM * xM + yM * yM) / 2.0;
                double nu = Math.Log(logZn / Math.Log(2)) / Math.Log(2);
                iteration = iteration + 1 - nu;
            }

            return
                new Color(0, 0,(float)Math.Sin(MathHelper.Lerp((float) Math.Floor(iteration), (float) Math.Floor(iteration) + 1,(float) (iteration%1.0)))).PackedValue;
        }
        /// <summary>
        /// Skaalaa alueelle
        /// </summary>
        /// <param name="x">numero joka skaalataan</param>
        /// <param name="min">numeron alueen mimini</param>
        /// <param name="max">numeron alueen maksimi</param>
        /// <param name="a">uuden alueen minimi</param>
        /// <param name="b">uuden alueen maksimi</param>
        /// <returns></returns>
        public static float Scale(float x, float min, float max, float a, float b)
        {
            return (b - a) * (x - min) / (max - min) + a;
        }
    }
}
