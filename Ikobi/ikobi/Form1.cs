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
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //размер таблицы
        public void size_matric()
        {

            try
            {

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

        public void GlavSimpex()
        {
            double[] Bj = new double[column - 1];
            double[,] TableFull = new double[(Int32.Parse(Stobec_pole.Text) + 1), Convert.ToInt32(Stobec_pole.Text) + Convert.ToInt32(Stroka_pole.Text)]; //инициализация двумерного массива из таблицы без Bj
            bool End = false;


            while (End != true)
            {
                End = simplex(Bj, TableFull);
            }
        }
        //создание массива из элементов даты
        public void DataInMass(double[] Bj, double[,] TableFull)
        {
            int Stroka = dataGridView1.RowCount - (Int32.Parse(Stobec_pole.Text) + 1); //galo4ka
            int Stolbec = 1;

            for (int i = 0; i < Convert.ToInt32(Stobec_pole.Text) + 1; i++)//tablefull заполнение
            {

                for (int j = 0; j < Convert.ToInt32(Stobec_pole.Text) + Convert.ToInt32(Stroka_pole.Text); j++)
                {

                    TableFull[i, j] = Convert.ToDouble(dataGridView1.Rows[Stroka].Cells[Stolbec].Value);   // дату в массив
                    Stolbec += 1;
                }
                Stroka += 1;
                Stolbec = 1;

            }

            Stroka = dataGridView1.RowCount - (Int32.Parse(Stobec_pole.Text) + 1); //galo4ka
            for (int i = 0; i < Convert.ToInt32(Stobec_pole.Text) + 1; i++)//tablefull заполнение
            {
                Bj[i] = Convert.ToDouble(dataGridView1[dataGridView1.ColumnCount - 1, Stroka].Value);
                Stroka += 1;
            }
        }

        public bool simplex(double[] Bj, double[,] TableFull)
        {
            bool end = false;
            string VariblindexIandJ;

            DataInMass(Bj, TableFull);


            VariblindexIandJ = VariableInTable(TableFull); //вызов процедуры для поиска минимума или максимума в F строке 

            int indexI, indexJ, g;
            indexI = Convert.ToInt32(Convert.ToString(VariblindexIandJ[0]));
            indexJ = Convert.ToInt32(Convert.ToString(VariblindexIandJ[1]));
            g = Convert.ToInt32(Convert.ToString(VariblindexIandJ[2]));

            int indexiBj = MAX(g, Bj, TableFull[indexI, indexJ]);// поиск минимального или максимального поделенного элемента Bj

            CreateNewLine();

            divison(TableFull, Bj, indexiBj, TableFull[indexI, indexJ]); //деление строки по минимальному или максимальному поделенному элементу Bj

            Slojenie(Bj, TableFull, indexiBj, TableFull[indexI, indexJ]); // рассчёт

            ExportInData(TableFull, Bj);//процедура вывода 

            end = proverca(end, TableFull); // проверка
            return end;
        }
        public void ExportInData(double[,] TableFull, double[] Bj)
        {
            int ii = 0, jj = 0;

            for (int i = (dataGridView1.RowCount - Convert.ToInt32(Stobec_pole.Text) - 1); i < dataGridView1.RowCount; i++)//tablefull заполнение
            {
                jj = 0;
                for (int j = 1; j <= (Convert.ToInt32(Stobec_pole.Text) + Convert.ToInt32(Stroka_pole.Text)); j++)
                {
                    {
                        dataGridView1.Rows[i].Cells[j].Value = Convert.ToString((TableFull[ii, jj]));
                    }
                    jj++;
                }
                ii++;
            }

            jj = 0;
            for (int i = (dataGridView1.RowCount - Convert.ToInt32(Stobec_pole.Text) - 1); i < dataGridView1.RowCount; i++)
            {

                dataGridView1.Rows[i].Cells[dataGridView1.ColumnCount - 1].Value = Bj[jj];

                jj++;
            }
        }

        public bool proverca(bool end, double[,] TableFull)
        {
            int count = 0;
            if (m == false)   //условие для кнопки минимума
            {
                int i = TableFull.GetLength(0) - 1;

                for (int j = 0; j < Convert.ToInt32(Stroka_pole.Text) + Convert.ToInt32(Stobec_pole.Text); j++)
                    if (TableFull[i, j] >= 0)
                    {
                        count = count + 1;
                    }
                if (count == (Convert.ToInt32(Stroka_pole.Text) + Convert.ToInt32(Stobec_pole.Text)))
                    end = true;
            }
            else //условие для кнопки максимума аналогично
            {
                int i = TableFull.GetLength(0) - 1;

                for (int j = 0; j < Convert.ToInt32(Stroka_pole.Text) + Convert.ToInt32(Stobec_pole.Text); j++)

                    if (TableFull[i, j] <= 0)
                    {
                        count = count + 1;
                    }
                if (count == (Convert.ToInt32(Stroka_pole.Text) + Convert.ToInt32(Stobec_pole.Text)))
                        end = true;
            }
            return end;
        }

        public void CreateNewLine()
        {
            column += (Int32.Parse(Stobec_pole.Text) + 3); //количество строк
            dataGridView1.RowCount = column + 1;

            for (int i = 1; i <= (Convert.ToInt32(Stroka_pole.Text) + Convert.ToInt32(Stobec_pole.Text)) + 1; i++) //цикл для заполнения иксов сверху всех
                dataGridView1.Rows[column - Int32.Parse(Stobec_pole.Text) - 1].Cells[i].Value = "x" + i;


            dataGridView1.Rows[column - Int32.Parse(Stobec_pole.Text) - 1].Cells[0].Value = "B";             //выбор первой ячейки для записи В
            dataGridView1.Rows[column - Int32.Parse(Stobec_pole.Text) - 1].Cells[dataGridView1.ColumnCount - 1].Value = "Bj";       //выбор последней ячейки в нулевой строке  для записи Вj         
            int d = dataGridView1.ColumnCount - Convert.ToInt32(Stobec_pole.Text) - 1;     // формула для дополнительных иксов
            int sas = 0;
            for (int i = column - Int32.Parse(Stobec_pole.Text); i < (dataGridView1.RowCount - 1); i++)    //цикл для заполнения иксов слева
            {
                dataGridView1.Rows[i].Cells[0].Value = "x" + (d + sas);
                sas += 1;
            }

            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = "F";  //выбор последней ячейки нулевого столбца для записи Bj
        }
        //поиск минимума или максимума
        public string VariableInTable(double[,] TableFull)
        {
            double g = 0;
            double e = 0;
            int a = TableFull.GetLength(0) - 1;
            int b = 0;
            if (m == false)   //условие для кнопки минимума
            {

                double min = TableFull[TableFull.GetLength(0) - 1, 0];
                g = 3;
                int i = TableFull.GetLength(0) - 1;
                e = min;
                for (int j = 0; j < Convert.ToInt32(Stroka_pole.Text); j++)
                {
                    if ((min > TableFull[i, j]) && (min != TableFull[i, j])) //условие для поиска минимума
                    {
                        min = TableFull[i, j];
                        a = i;
                        b = j;
                        g = 1;
                        e = min;
                    }
                }
                textBox1.Visible = true; //показать скрытый текстбокс
                textBox1.Text = Convert.ToString((a + 1) + " Строка; " + (b + 1) + " Столбец; " + "Минмальный элемент: " + min); //вывод в текстбокс
            }
            else //условие для кнопки максимума аналогично
            {
                double min = TableFull[TableFull.GetLength(0) - 1, 0];
                g = 4;
                e = min;
                int i = TableFull.GetLength(0) - 1;

                for (int j = 0; j < Convert.ToInt32(Stroka_pole.Text); j++)
                {
                    if ((min < TableFull[i, j]) && (min != TableFull[i, j])) //условие для поиска максимума
                    {
                        min = TableFull[i, j];
                        a = i;
                        b = j;
                        g = 2;
                        e = min;
                    }
                }

                textBox1.Visible = true;
                textBox1.Text = Convert.ToString((a + 1) + " Строка; " + (b + 1) + " Столбец; " + "максимальный элемент: " + min);
            }
            string IndexIandJ = Convert.ToString(a) + Convert.ToString(b) + Convert.ToString(g);
            return IndexIandJ;

        }
        //поиск минимума или максимума по Bj
        public int MAX(double g, double[] Bj, double e)
        {
            int indexi = 0;

            double c = 0;
            if (g == 1 || g == 3)
            {

                double min = Bj[0] / e;
                for (int i = 0; i < Bj.GetLength(0) - 1; i++)
                {
                    if (min < (Bj[i] / e))
                    {
                        min = Bj[i];
                        indexi = i;
                    }
                    c = Bj[indexi] / e;

                }
                textBox2.Text = Convert.ToString((indexi + 1) + " Строка, " + "элемент равен " + c);

            }
            if (g == 2 || g == 4)
            {

                double min = Bj[0] / e;
                for (int i = 0; i < Bj.GetLength(0) - 1; i++)
                {
                    if (min > (Bj[i] / e))
                    {
                        min = Bj[i];
                        indexi = i;
                    }
                    c = Bj[indexi] / e;

                }

                textBox2.Text = Convert.ToString((indexi + 1) + " Строка, " + "элемент равен " + c);
            }
            return indexi;
        }
        //деление строки с минимальным или максимальным элементом по Bj
        public void divison(double[,] TableFull, double[] Bj, int indexBj, double dev)
        {
            int jj = 0, ii = 0;

            for (int i = (dataGridView1.RowCount - Convert.ToInt32(Stobec_pole.Text) - 1); i < dataGridView1.RowCount; i++)//tablefull заполнение
            {
                jj = 0;
                for (int j = 1; j <= (Convert.ToInt32(Stobec_pole.Text) + Convert.ToInt32(Stroka_pole.Text)); j++)
                {
                    if (ii == indexBj)
                    {
                        TableFull[ii, jj] = TableFull[ii, jj] / dev;
                        dataGridView1.Rows[i].Cells[j].Value = Convert.ToString(TableFull[ii, jj]);
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[j].Value = Convert.ToString((TableFull[ii, jj]));
                    }

                    jj++;
                }
                ii++;
            }
            jj = 0;
            for (int i = (dataGridView1.RowCount - Convert.ToInt32(Stobec_pole.Text) - 1); i < dataGridView1.RowCount; i++)
            {
                if (jj == indexBj)
                {
                    Bj[jj] = Bj[jj] / dev;
                    dataGridView1.Rows[i].Cells[dataGridView1.ColumnCount - 1].Value = Bj[jj];
                }
                else
                {
                    dataGridView1.Rows[i].Cells[dataGridView1.ColumnCount - 1].Value = Bj[jj];
                }
                jj++;
            }
            DataInMass(Bj, TableFull);
        }
        //Расчёт строк
        public void Slojenie(double[] Bj, double[,] TableFull, int indexBj, double dev)
        {
            int ii = 0, jj = 0;
            ii = 0;
            for (int i = (dataGridView1.RowCount - Convert.ToInt32(Stobec_pole.Text) - 1); i < dataGridView1.RowCount; i++)//tablefull заполнение
            {
                jj = 0;
                for (int j = 1; j <= (Convert.ToInt32(Stobec_pole.Text) + Convert.ToInt32(Stroka_pole.Text)); j++)
                {
                    if (ii != indexBj)
                    {
                        TableFull[ii, jj] = TableFull[ii, jj] - dev * TableFull[indexBj, jj];
                        dataGridView1.Rows[i].Cells[j].Value = Convert.ToString((TableFull[ii, jj]));
                    }
                    jj++;
                }
                ii++;
            }

            jj = 0;
            for (int i = (dataGridView1.RowCount - Convert.ToInt32(Stobec_pole.Text) - 1); i < dataGridView1.RowCount; i++)
            {
                if (jj != indexBj)
                {
                    Bj[jj] = Bj[jj] - dev * Bj[indexBj];
                    dataGridView1.Rows[i].Cells[dataGridView1.ColumnCount - 1].Value = Bj[jj];
                }
                jj++;
            }
        }

        //Ошибка 
        public void warning()
        {
            MessageBox.Show("Ошибка! " +
                "Проверьте введенные данные!");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                line = Convert.ToInt32(Stroka_pole.Text) + Convert.ToInt32(Stobec_pole.Text) + 2; //количество строк
                column = Convert.ToInt32(Stobec_pole.Text) + 2; //количество столбцов
                dataGridView1.RowCount = column;
                dataGridView1.ColumnCount = line;
                size_matric();
            }
            catch
            {
                warning();
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //кнопка высчитать
        private void button3_Click(object sender, EventArgs e)
        {
            GlavSimpex();
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



