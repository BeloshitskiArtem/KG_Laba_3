using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KG_Laba_3
{
    public partial class Form1 : Form
    {
        public int xn, yn, xk, yk; // концы отрезка
        Bitmap mybitmap; // объект Bitmap для вывода отрезка
        Color current_color; // текущий цвет отрезка
        Color newColor;
        public Form1()
        {
            InitializeComponent();
        }
        // вывод отрезка
        private void CDA(int x1, int y1, int x2, int y2)
        {
            int i, n;
            double xt, yt, dx, dy;

            xn = x1;
            yn = y1;
            xk = x2;
            yk = y2;
            dx = xk - xn;
            dy = yk - yn;
            n = 400;
            xt = xn;
            yt = yn;
            for (i = 1; i <= n; i++)
            {
                Pen myPen = new Pen(current_color, 1);
                mybitmap.SetPixel((int)xt, (int)yt, current_color);
                xt = xt + dx / n;
                yt = yt + dy / n;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                xn = e.X;
                yn = e.Y;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            int i, n;
            double xt, yt, dx, dy;
            xk = e.X;
            yk = e.Y;

            dx = xk - xn;
            dy = yk - yn;
            n = 400;
            xt = xn;
            yt = yn;

            for (i = 1; i <= n; i++)
            {
                //Объявляем объект "g" класса Graphics и предоставляем
                //ему возможность рисования на pictureBox1
                Graphics g = Graphics.FromHwnd(pictureBox1.Handle);

                SolidBrush brush = new SolidBrush(current_color);
                //Рисуем закрашенный прямоугольник
                g.FillRectangle(brush, (int)xt, (int)yt, 1, 1);
                xt = xt + dx / n;
                yt = yt + dy / n;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult D = colorDialog2.ShowDialog();
            if (D == System.Windows.Forms.DialogResult.OK)
            {
                newColor = colorDialog2.Color;
            }
        }

        // Заливка с затравкой (рекурсивная)
        private void Zaliv(int x1, int y1)
        {
            // получаем цвет текущего пикселя с координатами x1, y1
            Color old_color = mybitmap.GetPixel(x1, y1);
            // сравнение цветов происходит в формате RGB
            // для этого используем метод ToArgb объекта Color
            if ((old_color.ToArgb() != current_color.ToArgb()) &&
           (old_color.ToArgb() != newColor.ToArgb()))
            {
                //перекрашиваем пиксель
                mybitmap.SetPixel(x1, y1, newColor);

                //вызываем метод для 4-х соседних пикселей
                Zaliv(x1 + 1, y1);
                Zaliv(x1 - 1, y1);
                Zaliv(x1, y1 + 1);
                Zaliv(x1, y1 - 1);
            }
            else
            {
                //выходим из метода
                return;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //отключаем кнопки
            button1.Enabled = false;
            button2.Enabled = false;
            //создаем новый экземпляр Bitmap размером с элемент
            //pictureBox1 (mybitmap)
            //на нем выводим попиксельно отрезок
            mybitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromHwnd(pictureBox1.Handle))
            {
                if (radioButton1.Checked == true)
                {
                    //рисуем прямоугольник
                    CDA(10, 10, 10, 110);
                    CDA(10, 10, 110, 10);
                    CDA(10, 110, 110, 110);
                    CDA(110, 10, 110, 110);
                    //рисуем треугольник
                    CDA(150, 10, 150, 200); //CDA(300, 100, 100, 600);
                    CDA(150, 50, 150, 150); //CDA(300, 100, 300, 600);
                    CDA(250, 50, 150, 150); //CDA(100, 600, 300, 600);
                    CDA(150, 10, 250, 150);                    
                }
                else
                {
                    if (radioButton2.Checked == true)
                    {
                        //получаем растр созданного рисунка в mybitmap
                        mybitmap = pictureBox1.Image as Bitmap;

                        // вызываем рекурсивную процедуру заливки с затравкой
                        Zaliv(xn, yn);
                    }
                }
                //передаем полученный растр mybitmap в элемент pictureBox
                pictureBox1.Image = mybitmap;
                // обновляем pictureBox и активируем кнопки
                pictureBox1.Refresh();
                button1.Enabled = true;
                button2.Enabled = true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult D = colorDialog1.ShowDialog();
            if (D == System.Windows.Forms.DialogResult.OK)
            {
                current_color = colorDialog1.Color;
            }
        }
    }

}
