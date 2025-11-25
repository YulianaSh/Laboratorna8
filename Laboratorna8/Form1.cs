using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Laboratorna8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class MyBook  // клас, спільний для класів MyBooks1, MyBooks2, MyBooks3 
        {
            public int bookNomer { get; set; }
            public String Avtor { get; set; }
            public String Nazva { get; set; }
            public String Vydavnyctvo { get; set; }
            public Int16 RikVyhodu { get; set; }
            // Конструктор класу MyBook з параметрами, що містять початкові значення для усіх полів класу 
            public MyBook(int bookNomer, String Avtor, String Nazva, String Vydavnyctvo, Int16 RikVyhodu)
            {
                this.bookNomer = bookNomer;
                this.Avtor = Avtor;
                this.Nazva = Nazva;
                this.Vydavnyctvo = Vydavnyctvo;
                this.RikVyhodu = RikVyhodu;
            }
            public override string ToString()
            {
                return "Книга №" + bookNomer.ToString() + " Автор:" + Avtor + " Назва:" + Nazva + " Видавництво: "
              + Vydavnyctvo + " Рік:" + RikVyhodu.ToString();
            }
        }
        // Реалізація методу CompareTo інтерфейсу IComparable  
        // Спочатку реалізуємо 1-й спосіб  
        public class MyBooks1 : ArrayList, IEnumerable
        {
            public MyBook[] MyBooksArray { get; set; }
            // Конструктор класу. Він відразу створює масив з кількістю елементів типу Book, що задана у параметрі 
            public MyBooks1(int kilkistKnyh)
            {
                MyBooksArray = new MyBook[kilkistKnyh];
            }
            // Ітератор для класу MyBooks1 створювати не потрібно, оскільки він є у класі ArrayList. 
        }     // завершення реалізації 1-го способу 
              // Клас MyBooks2 ілюструє 2-й спосіб реалізації інтерфейсу IEnumerable 
              // Від класу MyBooks1 він відрізняється двома моментами:  
              // - він не є нащадком класу ArrayList;  
              // - тому він містить власну реалізацію методу GetEnumerator 
        public class MyBooks2 : IEnumerable
        {
            // Конструктор класу. Він відразу створює масив із елементів типу Book кількістю, заданою у параметрі 
            MyBook[] myBooksArray;
            public MyBook[] MyBooksArray { get; set; }
            public MyBooks2(int kilkistKnyh)
            {
                MyBooksArray = new MyBook[kilkistKnyh];
            }
            // Ітератор класу MyBooks. Тут використано ітератор класу Array, оскільки myBooksArray має цей тип 
            public IEnumerator GetEnumerator()
            {
                return myBooksArray.GetEnumerator();
            }
        }
        /* Клас MyBooks3 ілюструє третій спосіб реалізації інтерфейсу IEnumerable 
         * Цей клас підтримує також інтерфейс IEnumerator, який дозволяє рухатись по елементах масиву за 
         * допомогою методу MoveNext. 
         * Від класу MyBooks2 він відрізняється тим, що у класі MyBooks3 створено свій індексатор, а також  
         * повністю з нуля реалізовано інтерфейси IEnumerable та Ienumerator.  
         * Інтерфейс IEnumerable вимагає наявності одного лиш методу GetEnumerator(). 
         * Інтерфейс IEnumerator вимагає наявності 3-х методів: Current(), MoveNext() i Reset(), які  
         * дозволяють позиціонувати елемент масиву. 
         * Додано також метод Add для додавання нових книг у масив. */

        public class MyBooks3 : IEnumerable, IEnumerator
        {
            MyBooks3[] myBooksArray; // масив для індексатора 
            int kilkistKnyh = 0;  // максимальна кількість книг, які можуть бути додані у масив 
            int CurrentNomer = 0;
            int bookNomer { get; set; }
            String Avtor { get; set; }
            String Nazva { get; set; }
            String Vydavnyctvo { get; set; }
            public Int16 RikVyhodu { get; set; }
            int position = -1;
            /* Конструктор класу. Він створює масив із елементів типу MyBooks3 кількістю, яку задано у параметрі, 
             * а також записує книжку */
            public MyBooks3(int kilkistKnyh, int bookNomer, String Avtor, String Nazva, String Vydavnyctvo,
            Int16 RikVyhodu)
            {
                if (kilkistKnyh <= 0) return;
                myBooksArray = new MyBooks3[kilkistKnyh];
                this.kilkistKnyh = kilkistKnyh;
                this.bookNomer = bookNomer; this.Avtor = Avtor; this.Nazva = Nazva; this.Vydavnyctvo =
              Vydavnyctvo;
                this.RikVyhodu = RikVyhodu;
            }
            // Ще один конструктор - з одним параметром. Створює порожній екземпляр і порожній масив для книжок.  
            // Параметр - максимальна кількість книжок. 
            public MyBooks3(int kilkistKnyh)
            {
                myBooksArray = new MyBooks3[kilkistKnyh];
                this.myBooksArray = new MyBooks3[kilkistKnyh];
                this.kilkistKnyh = kilkistKnyh;
                bookNomer = 0;
            }
            // Індексатор класу MyBooks 
            public MyBooks3 this[int index]
            {
                get     // вибрати об'єкт типу MyBook з індексом index 
                {
                    if (index <= kilkistKnyh && index >= 0)
                        return myBooksArray[index];
                    else return null;
                }
                set     // додати книжку з індексом index 
                {
                    if (index < kilkistKnyh) myBooksArray[index] = value;
                }
            }
            // Це ітерарор класу MyBooks, створений з застосуванням ключового слова yield 
            public IEnumerator GetEnumerator()
            {
                foreach (MyBooks3 b in myBooksArray)
                {
                    // Ключове слово yield означає, що елементи будуть повертатись по одному і просовуватись при кожному 
                    // наступному звертанні до методу 
                    yield return (IEnumerator)b;
                }
            }
            // Метод Current() інтерфейсу IEnumerator 
            public object Current
            {
                get
                {
                    if (position >= 0 && position < kilkistKnyh) return myBooksArray[position];
                    else return null;
                }
            }

            // Метод MoveNext() інтерфейсу IEnumerator 
            public bool MoveNext()
            {
                position++;
                return (position < kilkistKnyh);
            }
            // Метод Reset() інтерфейсу IEnumerator 
            public void Reset()
            {
                position = -1;
            }
            public override string ToString()
            {
                return "Книга №" + bookNomer.ToString() + " Автор:" + Avtor + " Назва:" + Nazva + " Видавництво: " +
                Vydavnyctvo + " Рік: " + RikVyhodu.ToString();
            }
            public void Add(int bookNomer, String Avtor, String Nazva, String Vydavnyctvo, Int16
            RikVyhodu, ref int KodError)
            {
                if (CurrentNomer < kilkistKnyh)
                {
                    this.myBooksArray[CurrentNomer] = new MyBooks3(1);
                    this.myBooksArray[CurrentNomer].bookNomer = bookNomer;
                    this.myBooksArray[CurrentNomer].Avtor = Avtor;
                    this.myBooksArray[CurrentNomer].Nazva = Nazva;
                    this.myBooksArray[CurrentNomer].Vydavnyctvo = Vydavnyctvo;
                    this.myBooksArray[CurrentNomer].RikVyhodu = RikVyhodu;
                    CurrentNomer++;
                    KodError = 0;   // якщо не вийшли за межі масиву, то код завершення = 0 
                }
                else KodError = 1;  // якщо вийшли за межі масиву, то код завершення =1 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* По черзі створюємо екземпляри класів MyBooks1,MyBooks2,MyBooks3, частково заповнюємо їх і виводимо 
у текст мітки */
            string ss = "\n Вивід для класу MyBooks1 \n\n ";
            MyBooks1 mbs1 = new MyBooks1(100);
            mbs1.MyBooksArray[0] = new MyBook(1, " Е. Marija Remark", "Три товариші", "Ранок", 1981);
            mbs1.MyBooksArray[1] = new MyBook(2, "Нестайко", "В країні сонячних зайчиків", "Ранок", 1961);
            mbs1.MyBooksArray[2] = new MyBook(3, "Баскаков", "Радіотехнічні кола і сигнали ", "М.: Вища школа",
          2000);
            foreach (MyBook b in mbs1.MyBooksArray)
            {
                if (b != null) ss = ss + b.ToString() + "\n";
            }
            ss = ss + "\n Вивід для класу MyBooks2 \n\n ";
            MyBooks2 mbs2 = new MyBooks2(100);
            mbs2.MyBooksArray[0] = new MyBook(1, "Е. Marija Remark", "Три товариші ", "Ранок", 1981);
            mbs2.MyBooksArray[1] = new MyBook(2, "Нестайко", "У країні сонячних зайчиків", "Ранок", 1961);
            mbs2.MyBooksArray[2] = new MyBook(3, "Баскаков", "Радіотехнічні кола і сигнали ", "М.: Вища школа",
           2000);
            foreach (MyBook b in mbs2.MyBooksArray)
            {
                if (b != null) ss = ss + b.ToString() + "\n";
            }
            ss = ss + "\n Вивід для класу MyBooks2 \n\n ";
            // Тут, завдяки індексатору, ми працюємо з класом MyBooks3 як з масивом 
            // А завдяки ітератору можемо використовувати цикл foreach 
            MyBooks3 mbs3 = new MyBooks3(100); // створено екземпляр класу з допомогою конструктора 
            int KodError = 0;
            mbs3.Add(1, "Е. Marija Remark", "Три товариші ", "Ранок", 1981, ref KodError);

            // Додаємо книги за допомогою методу Show(" Не вдалось додати книжку, оскільки масив переповнено ");  
            mbs3.Add(3, "Баскаков", "Радіотехнічні кола і сигнали ", "М.: Вища школа", 2000, ref KodError);
            if (KodError > 0) MessageBox.Show(" Не вдалось додати книжку, оскільки масив переповнено ");
            mbs3.Add(4, "Загребельний ", "Роксолана", "Світанок", 2000, ref KodError);
            if (KodError > 0) MessageBox.Show("Не вдалось додати книжку, оскільки масив переповнено ");
            mbs3.Add(5, "В. Косик", "Україна і Німеччина у другій світовій війні ", "Наукове товариство ім. Шевченка у Львові",
            1993, ref KodError);
            if (KodError > 0) MessageBox.Show("Не вдалось додати книжку, оскільки масив переповнено ");
            // Додаємо до тексту мітки і виведення для книг з масиву класу MyBooks3 
            foreach (MyBooks3 b in mbs3)
            {
                if (b != null) ss = ss + b.ToString() + " \n";
            }
            // Оскільки, при ініціалізації, значення поля position = -1,  
            // то при першому MoveNext ми стаємо на 0-вий елемент 
            mbs3.MoveNext();
            ss = ss + " \n Вивід поточного елементу \n  \n";
            ss = ss + mbs3.Current.ToString() + " \n";
            ss = ss + " \n Ще раз MoveNext \n \n ";
            mbs3.MoveNext();
            // тепер ми стали на 1-й елемент 
            ss = ss + mbs3.Current.ToString();
            label1.Text = ss;
        }
    }
}
