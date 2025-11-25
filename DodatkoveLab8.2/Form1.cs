using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DodatkoveLab8._2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // Наш власний TextBox, який підтримує клонування
        class MyTextBox : TextBox, ICloneable
        {
            public object Clone()
            {
                // Поверхневе (shallow) клонування
                return this.MemberwiseClone();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Створюємо новий екземпляр нашого класу
            MyTextBox tb2 = new MyTextBox();
            tb2.Text = "Це текст з TextBox2 (клон)";
            tb2.BackColor = Color.LightBlue;
            tb2.Location = new Point(50, 110);
            tb2.Size = new Size(220, 40);
            tb2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // 2. Клонуємо його — отримуємо об’єкт типу object
            object clone = tb2.Clone();

            // 3. Приводимо до типу TextBox і присвоюємо полю textBox1
            // УВАГА! Поле textBox1 у нас вже існує (той, що ми поставили у дизайнері)
            textBox1 = (TextBox)clone;

            // 4. Додаємо новостворений (клонований) контрол на форму
            this.Controls.Add(textBox1);

            // Старий textBox1, який був у дизайнері, тепер «втрачений»,
            // але він все ще існує в пам’яті і залишається на формі,
            // бо його ніхто не видалив з колекції Controls!
          }
       }
    }
