using System;

namespace PetsWithIndexer
{
    // Клас, що представляє одну домашню тварину
    public class Pet
    {
        public string Name { get; set; }       // Ім'я тварини
        public string Species { get; set; }     // Вид (собака, кіт, папуга тощо)
        public int Age { get; set; }            // Вік у роках

        public Pet(string name, string species, int age)
        {
            Name = name;
            Species = species;
            Age = age;
        }

        // Перевизначення ToString() для зручного виведення інформації про тварину
        public override string ToString()
        {
            return $"{Name} — {Species}, {Age} років";
        }
    }

    // Клас власника домашніх тварин, який використовує ІНДЕКСАТОР
    public class PetOwner
    {
        // Приватний масив для зберігання тварин
        private Pet[] _pets;

        // Конструктор — приймає масив тварин або створює порожній
        public PetOwner(int capacity = 10)
        {
            _pets = new Pet[capacity];
            Count = 0; // поточна кількість тварин
        }

        // Кількість тварин, які реально додані
        public int Count { get; private set; }

        // === ІНДЕКСАТОР ===
        // Дозволяє звертатись до тварин за індексом так: owner[0], owner[1] тощо
        public Pet this[int index]
        {
            get
            {
                // Перевірка коректності індексу
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException(
                        $"Індекс {index} поза межами. Доступно тварин: {Count}");

                return _pets[index];
            }
            set
            {
                // Перевірка коректності індексу при записі
                if (index < 0 || index >= _pets.Length)
                    throw new IndexOutOfRangeException(
                        $"Індекс {index} недоступний для запису.");

                _pets[index] = value;

                // Якщо це нова тварина (індекс == Count), збільшуємо лічильник
                if (index == Count)
                    Count++;
            }
        }

        // Додатковий індексатор за ім'ям тварини (перевантаження індексатора)
        public Pet this[string name]
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    if (_pets[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        return _pets[i];
                }
                throw new ArgumentException($"Тварина з ім'ям \"{name}\" не знайдена.");
            }
        }

        // Метод для додавання тварини (зручніше, ніж прямий запис через індексатор)
        public void AddPet(Pet pet)
        {
            if (Count >= _pets.Length)
                throw new InvalidOperationException("Досягнуто максимальну кількість тварин.");

            _pets[Count] = pet;
            Count++;
        }

        // Виведення всіх тварин власника
        public void ShowAllPets()
        {
            Console.WriteLine($"Власник має {Count} домашніх тварин:");
            for (int i = 0; i < Count; i++)
            {
                Console.WriteLine($"  [{i}] {_pets[i]}");
            }
        }
    }

    // Головний клас програми
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // для коректного відображення української мови

            // Створюємо власника, який може мати до 10 тварин
            PetOwner owner = new PetOwner(10);

            // Додаємо кількох домашніх тварин
            owner.AddPet(new Pet("Барсик", "Кіт", 5));
            owner.AddPet(new Pet("Рекс", "Собака", 3));
            owner.AddPet(new Pet("Кеша", "Папуга", 2));
            owner.AddPet(new Pet("Мурка", "Кіт", 7));

            // Використовуємо індексатор за номером
            Console.WriteLine("=== Доступ через індексатор за номером ===");
            Console.WriteLine(owner[0]); // Барсик
            Console.WriteLine(owner[2]); // Кеша

            // Зміна тварини через індексатор
            owner[1] = new Pet("Тузик", "Собака", 4);
            Console.WriteLine("\nПісля заміни собаки:");
            owner.ShowAllPets();

            // Використовуємо індексатор за ім'ям
            Console.WriteLine("\n=== Доступ через індексатор за ім'ям ===");
            Console.WriteLine(owner["мурка"]);   // нечутливо до регістру
            Console.WriteLine(owner["Кеша"]);

            // Спроба отримати неіснуючу тварину — отримаємо виняток
            // Console.WriteLine(owner["Немо"]); // викличе ArgumentException

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}