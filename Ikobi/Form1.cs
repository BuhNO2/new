using System;
using System.Windows.Forms;

namespace Ikobi
{
    public partial class Form1 : Form
    {
        public int line;
        int column;
        public int tochn;
        public int pogresh;
        public Form1()
        {
            InitializeComponent();
        }

        //размер таблицы
        public void size_matric()
        {

            try
            {
                line = Convert.ToInt32(Stroka_pole.Text) + Convert.ToInt32(Stobec_pole.Text) + 2; //количество столбцов
                column = Convert.ToInt32(Stobec_pole.Text) + 2; //количество строк
                dataGridView1.RowCount = column;
                dataGridView1.ColumnCount = line;
                for (int i = 1; i <= (Convert.ToInt32(Stroka_pole.Text) + Convert.ToInt32(Stobec_pole.Text)) + 1; i++) //цикл для заполнения иксов сверху всех
                {
                    dataGridView1.Rows[0].Cells[i].Value = "x" + i;
                }
                dataGridView1.Rows[0].Cells[0].Value = "B";             //выбор первой ячейки для записи В
                dataGridView1.Rows[0].Cells[dataGridView1.ColumnCount - 1].Value = "Bj";       //выбор последней ячейки в нулевой строке  для записи Вj         
                int d = dataGridView1.ColumnCount - Convert.ToInt32(Stobec_pole.Text) - 1;     // формула для дополнительных иксов

                for (int i = 0; i < Convert.ToInt32(Stobec_pole.Text); i++)    //цикл для заполнения иксов слева
                {
                    dataGridView1.Rows[i + 1].Cells[0].Value = "x" + (d + i);
                }

                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = "F";  //выбор последней ячейки нулевого столбца для записи Bj
            }
            catch
            {
                warning();  //сообщение
            }
        }

        //создание массива из элементов даты
        public void simplex()
        {
 
            double[,] B = new double[dataGridView1.RowCount-1, dataGridView1.ColumnCount-2]; //инициализация двойного массива
            int a = 1;
            int b = 1;
            for (int i = 0; i < dataGridView1.RowCount-1; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount-2; j++)
                {
                    B[i, j] = Convert.ToDouble(dataGridView1.Rows[b].Cells[a].Value);   // дату в массив
                    a = j + 1;
                    
                }
                b = i + 1;
                a = 1;
            }
            min(B); //вызов процедуры для поиска минимума или максимума


            /* create_martic();*/
        }

        public void massBj()
        {
            double[] A = new double[column]; //инициализация массива для последних столбцов
            for (int i = 1; i < Convert.ToInt32(Stobec_pole.Text); i++) //цикл для заполнения массива элементами Bj
            {
                A[i] = Convert.ToDouble(dataGridView1[Convert.ToInt32(Stobec_pole.Text), i].Value.ToString());
            }
        }

        //поиск минимума или максимума
        public void min(double[,] B)
        {
            int pog =  Convert.ToInt32(Stroka_pole.Text);
            double g = 0;
            double e = 0;
            if (m == false)   //условие для кнопки минимума
            {
                int b = 1;
                int a = 1;
                double min = B[ dataGridView1.RowCount,0]; //выбор  первого элекмента конечной строки

                int i = 3;  //цикл с первого элемета последней строки до количества столбцов                
                    for (int j = 0; j <= Convert.ToInt32(Stroka_pole.Text); j++)    // с первого столбца до количества переменных функции
                    {
                        if ((min > B[i, j]) && (min != B[i, j])) //условие для поиска минимума
                        {
                            min = B[i, j];   //минимальный элемент                    
                            a = i;       //индекс по строкам
                            b = j;       //индекс по столбцам
                            g = 1;      // для условия в процедуре макс
                            e = min;    //для деления в процедуре макс
                        }
                    }                

                textBox1.Visible = true; //показать скрытый текстбокс
                textBox1.Text = Convert.ToString(a + " Строка; " + b + " Столбец; " + "Минмальный элемент: " + min); //вывод в текстбокс
            }
            else //условие для кнопки максимума аналогично
            {
                int a = 1;
                int b = 1;
                double min = B[0, 0];

                for (int i = 0 ; i < dataGridView1.RowCount; i++)
                {
                    for (int j = 0; j <= Convert.ToInt32(Stroka_pole.Text); j++) 
                    {
                        if ((min <= B[i, j]) && (min != B[i, j - 1])) //условие для поиска максимума
                        {
                            min = B[i, j];
                            a = i;
                            b = j;
                            g = 2;
                            e = min;
                        }
                    }
                }
                textBox1.Visible = true;
                textBox1.Text = Convert.ToString(a + " Строка; " + b + " Столбец; " + "максимальный элемент: " + min);
            }
            MAX(g,e);
        }

        public void MAX(double g,double e)
        {
            
            if (g == 1)
            {
                massBj();                
                for(int i = 1; i< Convert.ToInt32(Stobec_pole.Text);i++)
                {                     
                    double c = Convert.ToInt32(Stobec_pole.Text)/e;
                    if (c < c-1)
                    {
                        g = c;
                        textBox2.Text = Convert.ToString(c);
                    }
                }                
            }
            if(g==2)
            {
                massBj();
                for(int i = 1;i< Convert.ToInt32(Stobec_pole.Text);i++)
                {
                    double c = Convert.ToInt32(Stobec_pole.Text)/e;
                    if (c> c-1)
                    {
                        g = c;
                        textBox2.Text = Convert.ToString(c);
                    }
                }
            }

        }

        /*        public void create_martic()
                {
                    dataGridView1.RowCount = line + line;
                    dataGridView1.ColumnCount = column;
                }*/





        //Создание первой матрицы
        public void Create_new_matric_first(double[,] matrica, double[] matricaB)
        {

            line += Convert.ToInt32(Stobec_pole.Text) + 1;//кол-во увеличение даты

            /* iteration_write(matricaB);//вывод*/

        }


        /*        public void iteration_write(double[] matricaXsov)
                {
                    //Вывод икса
                    dataGridView1[0, kol].Value = ("x" + nomer);

                    int popa = kol + Convert.ToInt32(Stobec_pole.Text);
                    int i = 0;
                    // Конец Вывода икса
                    for (int joj = kol; joj < popa; joj++)
                    {
                        dataGridView1[1, joj].Value = matricaXsov[i];

                        i++;
                    }
                    nomer++;
                    kol = popa + 1;

                }*/




        private void Form1_Load(object sender, EventArgs e)
        {

        }



        //ТОчность конец


        //Ошибка 
        public void warning()
        {
            MessageBox.Show("Ошибка! " +
                "Проверьте введенные данные!");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            size_matric();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //кнопка высчитать
        private void button3_Click(object sender, EventArgs e)
        {
            simplex(); //рассчёт
        }

        public bool m = false; //переменная для кнопок
        private void button4_Click(object sender, EventArgs e)
        {
            m = false;  //для работы кнопки минимума
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m = true; //для работы кнопки максимума
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}



