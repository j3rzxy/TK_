using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TK_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик событий кнопки "Вычислить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Count_Click(object sender, RoutedEventArgs e)
        {
            TBRes.Clear();

            try
            {
                if (string.IsNullOrWhiteSpace(TBA.Text))
                {
                    throw new ArgumentException("Поле A не заполнено!");
                }

                if (string.IsNullOrWhiteSpace(TBB.Text))
                {
                    throw new ArgumentException("Поле B не заполнено!");
                }
                if (string.IsNullOrWhiteSpace(TBC.Text) && RadioSqr.IsChecked == true && RadioLine.IsChecked == false)
                {
                    throw new ArgumentException("Поле C не заполнено!");
                }

                if (!RadioLine.IsChecked.Value &&
                    !RadioSqr.IsChecked.Value)
                {
                    throw new ArgumentException("Выберите тип функции!");
                }

                string aText = TBA.Text.Trim().Replace(".", ",");
                string bText = TBB.Text.Trim().Replace(".", ",");
                string cText = TBC.Text.Trim().Replace(".", ",");

                if (!double.TryParse(aText, out double a))
                {
                    throw new FormatException("Поле A должно содержать число!");
                }

                if (!double.TryParse(bText, out double b))
                {
                    throw new FormatException("Поле B должно содержать число!");
                }
                if (!double.TryParse(cText, out double c) && RadioSqr.IsChecked == true && RadioLine.IsChecked == false)
                {
                    throw new FormatException("Поле C должно содержать число!");
                }

                Functions.FunctionType selectedType;
                if (RadioLine.IsChecked == true)
                    selectedType = Functions.FunctionType.Line;
                else if (RadioSqr.IsChecked == true)
                    selectedType = Functions.FunctionType.Square;
                else
                {
                    MessageBox.Show("Выберите функцию!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string strRes;
                string funcName;
                if (Functions.Calculate(a, b, c, selectedType, out strRes, out funcName))
                {
                    TBRes.Text = strRes.ToString();
                }
                else
                {
                    MessageBox.Show("Ошибка вычисления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        /// <summary>
        /// Статический класс для отдельных функций.
        /// </summary>
        public static class Functions
        {
            public enum FunctionType { Line, Square }
            /// <summary>
            /// Функция вычиления заданной функции.
            /// </summary>
            /// <param name="a">Коэффициент a</param>
            /// <param name="b">Коэффициент b</param>
            /// <param name="c">Коэффициент c (только для квадратного уравнения)</param>
            /// <param name="funcType">Тип уравнения</param>
            /// <param name="strRes">Результат вычислений формата строки</param>
            /// <param name="selectedFunction">Выбранный тип уравнений</param>
            /// <returns></returns>
            public static bool Calculate(double a, double b, double c, FunctionType funcType, out string strRes, out string selectedFunction)
            {
                double D = 1;
                double x1 = 1;
                double x2 = 0;
                double result = 0;
                strRes = null;
                selectedFunction = null;

                try
                {
                    if (funcType == FunctionType.Line)
                    {

                        result = -b / a;
                        strRes = result.ToString();
                    }
                    else if (funcType == FunctionType.Square)
                    {
                        D = b * b - 4 * a * c;
                        if (D > 0)
                        {
                            x1 = (-b + D) / 2 * a;
                            x2 = (-b - D) / 2 * a;
                            strRes = "${x1}, {x2}";
                        }
                        else if (D == 0)
                        {
                            x1 = (b) / 2 * a;
                            strRes = "${x1}";
                        }
                        else if (D < 1)
                        {
                            strRes = "Дискриминант меньше нуля. Корней нет.";
                        }
                    }
                    else
                    {
                        return false; // Неизвестная функция
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void RadioLine_Checked(object sender, RoutedEventArgs e)
        {
            TBC.Visibility = Visibility.Collapsed;
            LabelC.Visibility = Visibility.Collapsed;
        }

        private void RadioSqr_Checked(object sender, RoutedEventArgs e)
        {
            TBC.Visibility = Visibility.Visible;
            LabelC.Visibility = Visibility.Visible;
        }
    }
}
