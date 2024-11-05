using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using MatrixCalculation;

namespace MatrixApp
{
    public partial class MainWindow : Window
    {
        private Matrix<double> _matrixA;
        private Matrix<double> _matrixB;
        private Matrix<double> _matrixC;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateMatrices_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(RowsMatrixATextBox.Text, out int m1) || m1 <= 0 ||
                !int.TryParse(ColsMatrixATextBox.Text, out int n1) || n1 <= 0 ||
                !int.TryParse(RowsMatrixBTextBox.Text, out int m2) || m2 <= 0 ||
                !int.TryParse(ColsMatrixBTextBox.Text, out int n2) || n2 <= 0)
            {
                MessageBox.Show("Введите положительные целые числа для размеров матриц.");
                return;
            }

            _matrixA = new Matrix<double>(m1, n1);
            _matrixB = new Matrix<double>(m2, n2);

            DisplayMatrix(_matrixA, MatrixAControl);
            DisplayMatrix(_matrixB, MatrixBControl);
        }

        private void GenerateRandomValues_Click(object sender, RoutedEventArgs e)
        {
            if (_matrixA == null || _matrixB == null)
            {
                MessageBox.Show("Сначала создайте матрицы.");
                return;
            }

            Random rand = new Random();
            _matrixA.Generate((i, j) => rand.NextDouble() * 10);
            _matrixB.Generate((i, j) => rand.NextDouble() * 10);

            DisplayMatrix(_matrixA, MatrixAControl);
            DisplayMatrix(_matrixB, MatrixBControl);
        }

        private void DisplayMatrix(Matrix<double> matrix, ItemsControl control)
        {
            control.Items.Clear();
            var grid = new Grid
            {
                Margin = new Thickness(5)
            };

            for (int i = 0; i < matrix.Rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            for (int j = 0; j < matrix.Cols; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    var textBox = new TextBox
                    {
                        Text = matrix[i, j].ToString("F2"),
                        Margin = new Thickness(2),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                    grid.Children.Add(textBox);
                }
            }
            control.Items.Add(grid);
        }

        private void UpdateMatrix(Matrix<double> matrix, ItemsControl control)
        {
            if (control.Items.Count == 0) return;

            var grid = control.Items[0] as Grid;
            if (grid == null) return;

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    var textBox = grid.Children[i * matrix.Cols + j] as TextBox;
                    if (textBox == null) continue;

                    string input = textBox.Text.Trim();
                    if (double.TryParse(input, NumberStyles.Any, CultureInfo.CurrentCulture, out double value))
                    {
                        matrix[i, j] = value;
                    }
                    else
                    {
                        MessageBox.Show("Введите корректные числовые значения в матрице.");
                        return;
                    }
                }
            }
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (_matrixA == null || _matrixB == null)
            {
                MessageBox.Show("Сначала создайте и заполните матрицы.");
                return;
            }

            UpdateMatrix(_matrixA, MatrixAControl);
            UpdateMatrix(_matrixB, MatrixBControl);

            if (OperationComboBox.SelectedIndex == 0)
            {
                if (_matrixA.Rows == _matrixB.Rows && _matrixA.Cols == _matrixB.Cols)
                {
                    var watch = Stopwatch.StartNew();
                    _matrixC = _matrixA + _matrixB;
                    watch.Stop();
                    DisplayMatrix(_matrixC, MatrixCControl);
                    CalculationTimeTextBlock.Text = $"Время расчета (сложение): {watch.ElapsedMilliseconds} мс";
                }
                else
                {
                    MessageBox.Show("Размеры матриц должны совпадать для сложения.");
                }
            }
            else if (OperationComboBox.SelectedIndex == 1)
            {
                if (_matrixA.Cols == _matrixB.Rows)
                {
                    var watch = Stopwatch.StartNew();
                    _matrixC = _matrixA * _matrixB;
                    watch.Stop();
                    DisplayMatrix(_matrixC, MatrixCControl);
                    CalculationTimeTextBlock.Text = $"Время расчета (умножение): {watch.ElapsedMilliseconds} мс";
                }
                else
                {
                    MessageBox.Show("Для умножения количество столбцов первой матрицы должно быть равно количеству строк второй матрицы.");
                }
            }
        }

        private void SaveResult_Click(object sender, RoutedEventArgs e)
        {
            if (_matrixC == null)
            {
                MessageBox.Show("Рассчитайте результат, прежде чем сохранять его.");
                return;
            }

            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".csv",
                Filter = "CSV files (*.csv)|*.csv"
            };

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                _matrixC.SaveToCsv(dlg.FileName);
                MessageBox.Show("Результат сохранен!");
            }
        }
    }
}
