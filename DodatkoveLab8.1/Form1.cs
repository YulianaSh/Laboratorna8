using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DodatkoveLab8._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ================================================================
        // 1. Клас "Домашня тварина"
        // Реалізує два важливі інтерфейси:
        //    • IComparable<Pet>  → дозволяє сортувати тварин (наприклад, за віком)
        //    • ICloneable        → дозволяє створювати точну копію об'єкта
        // ================================================================
        public class Pet : IComparable<Pet>, ICloneable
        {
            // Властивості (properties) — сучасний спосіб доступу до полів
            public int Id { get; set; }   // Унікальний номер тварини
            public string Name { get; set; }   // Кличка
            public string Species { get; set; }   // Вид (Кіт, Собака, Папуга тощо)
            public int Age { get; set; }   // Вік у роках
            public string Owner { get; set; }   // Ім'я власника

            // Конструктор з параметрами — дозволяє створити об'єкт одразу з даними
            public Pet(int id, string name, string species, int age, string owner)
            {
                Id = id;
                Name = name;
                Species = species;
                Age = age;
                Owner = owner;
            }

            // Перевизначення ToString() — визначає, як об'єкт виглядатиме при виведенні
            // Використовуємо форматування, щоб стовпчики були рівними
            public override string ToString()
            {
                // {Id,2}   → Id займає 2 символи (вирівнювання праворуч)
                // {Name,-12} → Name займає 12 символів (вирівнювання ліворуч)
                return $"[{Id,2}] {Name,-12} ({Species,-8}) {Age,2} р. → Власник: {Owner}";
            }

            // Реалізація інтерфейсу IComparable<Pet>
            // Визначає порядок сортування: молодші тварини — першими
            public int CompareTo(Pet other)
            {
                if (other == null) return 1;                    // null вважаємо "меншим"
                return this.Age.CompareTo(other.Age);           // порівняння за віком
                // Якщо хочемо сортувати за ім'ям: return string.Compare(this.Name, other.Name);
            }

            // Реалізація інтерфейсу ICloneable
            // Повертає точну (поверхневу) копію поточного об'єкта
            public object Clone()
            {
                return this.MemberwiseClone();  // Створює копію всіх полів об'єкта
            }
        }

        // ================================================================
        // 2. Колекція №1 — найпростіший і найкращий на практиці спосіб
        // Наслідується від готового класу List<Pet>
        // Автоматично підтримує: IEnumerable, ICollection, IList, сортування тощо
        // ================================================================
        public class PetList : List<Pet>
        {
            // Конструктор за замовчуванням
            public PetList() : base() { }

            // Метод для отримання всього вмісту колекції у вигляді відформатованого тексту
            public string GetInfo()
            {
                string info = "=== Колекція 1 — на основі List<Pet> (найкращий спосіб) ==="
                            + Environment.NewLine;              // Environment.NewLine = \r\n у Windows

                foreach (var pet in this)                       // foreach працює автоматично
                    info += pet.ToString() + Environment.NewLine;

                info += Environment.NewLine;                    // порожній рядок між колекціями
                return info;
            }
        }

        // ================================================================
        // 3. Колекція №2 — власна колекція з використанням yield return
        // Дуже зручний і сучасний спосіб створення ітератора
        // ================================================================
        public class PetCollectionYield : IEnumerable<Pet>
        {
            // Внутрішнє сховище — звичайний List<Pet>
            private List<Pet> pets = new List<Pet>();

            // Додавання тварини у колекцію
            public void Add(Pet pet)
            {
                pets.Add(pet);
            }

            // Ітератор — ключове слово yield return робить магію!
            // Компілятор сам створить клас-ітератор за нас
            public IEnumerator<Pet> GetEnumerator()
            {
                foreach (var pet in pets)
                    yield return pet;       // повертає по одному елементу за раз
            }

            // Неузагальнена версія (потрібна для сумісності зі старим кодом)
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            // Виведення всієї колекції
            public override string ToString()
            {
                string info = "=== Колекція 2 — власний ітератор (yield return) ==="
                            + Environment.NewLine;

                foreach (var pet in this)
                    info += pet + Environment.NewLine;

                info += Environment.NewLine;
                return info;
            }
        }

        // ================================================================
        // 4. Колекція №3 — повна власна реалізація IEnumerable + IEnumerator
        // Найскладніший, але найповчальніший варіант
        // Показує, як працює foreach "під капотом"
        // ================================================================
        public class PetCollectionManual : IEnumerable, IEnumerator
        {
            private Pet[] pets;         // Масив для зберігання тварин
            private int capacity;       // Максимальна кількість елементів
            private int count = 0;      // Скільки вже додано
            private int position = -1;  // Поточна позиція ітератора (-1 = перед першим)

            // Конструктор — задаємо розмір колекції
            public PetCollectionManual(int capacity = 50)
            {
                this.capacity = capacity;
                pets = new Pet[capacity];
            }

            // Додавання тварини
            public bool Add(Pet pet)
            {
                if (count < capacity)               // є місце?
                {
                    pets[count++] = pet;            // додаємо і збільшуємо лічильник
                    return true;
                }
                return false;                       // переповнення
            }

            // Індексатор — дозволяє писати: collection[5] = myCat;
            public Pet this[int index]
            {
                get => (index >= 0 && index < count) ? pets[index] : null;
                set { if (index >= 0 && index < capacity) pets[index] = value; }
            }

            // =================== Реалізація IEnumerable ===================
            // Повертає об'єкт-ітератор (у нашому випадку — сам себе)
            public IEnumerator GetEnumerator() => this;

            // =================== Реалізація IEnumerator ===================
            public object Current                       // Поточний елемент
            {
                get
                {
                    if (position >= 0 && position < count)
                        return pets[position];
                    return null;
                }
            }

            // Пересуває вказівник на наступний елемент
            public bool MoveNext()
            {
                position++;
                return position < count;            // true — є ще елементи
            }

            // Повертає ітератор на початок
            public void Reset()
            {
                position = -1;
            }

            // Сортування за віком (використовуємо CompareTo з класу Pet)
            public void SortByAge()
            {
                Array.Sort(pets, 0, count);         // сортуємо тільки заповнену частину
            }

            // Виведення всієї колекції вручну (без foreach)
            public override string ToString()
            {
                string info = "=== Колекція 3 — повна власна реалізація ітератора ==="
                            + Environment.NewLine;

                Reset();                            // починаємо з початку
                while (MoveNext())                  // поки є елементи
                {
                    Pet pet = Current as Pet;
                    if (pet != null)
                        info += pet + Environment.NewLine;
                }

                info += Environment.NewLine;
                return info;
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            string result = "";                     // тут збираємо весь текст

            // ────────────────── Колекція 1 (List<Pet>) ──────────────────
            var list = new PetList();
            list.Add(new Pet(1, "Барсік", "Кіт", 3, "Олена"));
            list.Add(new Pet(2, "Рекс", "Собака", 7, "Іван"));
            list.Add(new Pet(3, "Кеша", "Папуга", 2, "Марія"));
            list.Add(new Pet(4, "Мурка", "Кіт", 12, "Софія"));

            list.Sort();                            // сортування за віком (молодші першими)
            result += list.GetInfo();

            // ────────────────── Колекція 2 (yield return) ──────────────────
            var yieldColl = new PetCollectionYield();
            yieldColl.Add(new Pet(5, "Луна", "Кіт", 1, "Дмитро"));
            yieldColl.Add(new Pet(6, "Бім", "Собака", 5, "Андрій"));
            yieldColl.Add(new Pet(7, "Гоша", "Папуга", 15, "Наталя"));

            result += yieldColl.ToString();

            // ────────────────── Колекція 3 (власний ітератор) ──────────────────
            var manualColl = new PetCollectionManual();
            manualColl.Add(new Pet(8, "Сніжок", "Хом'як", 1, "Діти"));
            manualColl.Add(new Pet(9, "Річард", "Собака", 9, "Петро"));
            manualColl.Add(new Pet(10, "Матильда", "Кіт", 4, "Ольга"));
            manualColl.Add(new Pet(11, "Чіп", "Собака", 0, "Сім'я Іваненків"));

            manualColl.SortByAge();
            result += manualColl.ToString();

            // ────────────────── Демонстрація клонування ──────────────────
            Pet original = new Pet(99, "Тігра", "Кіт", 6, "Віктор");
            Pet clone = (Pet)original.Clone();      // створюємо копію
            clone.Id = 999;
            clone.Name = "Тігра-клон";

            result += "Демонстрація інтерфейсу ICloneable:" + Environment.NewLine;
            result += $"Оригінал:       {original}" + Environment.NewLine;
            result += $"Клон (змінено): {clone}" + Environment.NewLine;

            // Виводимо результат у текстове поле
            textBoxResult.Text = result;
            textBoxResult.SelectionStart = 0;       // прокрутка вгору
        }
    }
}
