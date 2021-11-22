using System.Data.SQLite;       // для работы с SQLite
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using EmployeeDataManager.Model;

namespace EmployeeDataManager
{
    // класс для подключения и работы с БД, является моделью в паттерне MVC
    public class DataBaseManager
    {

        private string dataBasePath;                    // путь к файлу БД
        private string currentDirectory;                // текущая дериктория, нахождения исполняемого файла

        private List<Employee> m_employeesData;         // все сотрудники из БД, в виде списка в памяти программы

        // конструктор класса для управления базой данных
        public DataBaseManager()
        {
            // получнеие дериктории исполняемой сборки
            currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dataBasePath=$"{currentDirectory}/data/EmployeesDataBase.db";                       // получение пути к БД
            CreateNewDataBase();                                                                // создание файла БД
        }
        // метод для создания файла БД
        public bool CreateNewDataBase()
        {
            // если файл базы данных не существует
            if (!File.Exists(dataBasePath))
            {
                SQLiteConnection.CreateFile(dataBasePath);          // создание файла базы данных

                CreateDataBaseTables();                             // создание таблицы в БД

                return true;
            }
            // иначе если файл базы данных существует
            else
            {
                return false;
            }
        }
        // метод для создания таблицы сотрудников в БД
        private void CreateDataBaseTables()
        {
            // создание объекта класса SQLite для откырия соединения с БД
            using (SQLiteConnection connect = new SQLiteConnection(@$"Data Source={dataBasePath};"))
            {
                // созадние запроса к базе данных на создание таблицы, если таковой уже не имеется, таблица с данными работников
                string dataBaseCommandText = @"CREATE TABLE IF NOT EXISTS [Employees] 
                                                ( [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [Name] VARCHAR(32), [Surname] VARCHAR(32),[Patronymic] VARCHAR(32),[Birthday] VARCHAR(10),[Address] VARCHAR(128),[Group] VARCHAR(32),[Info] VARCHAR(256))";
                // применение запроса на создание таблицы к базе данных
                SQLiteCommand command = new SQLiteCommand(dataBaseCommandText,connect);     // создание команды для подключённой базы данных

                connect.Open();                 // открытие соединения с базой данных
                command.ExecuteNonQuery();      // выполнение запроса
                connect.Close();                // закрытие соединения с базой данных
            }
        }
        // метод для получения всей информации таблицы
        public List<Employee> GetAllTable()
        {
            // объявление и инициализация буфферной переменной списка, которая будет возвращаться текущим методом
            List<Employee> tableData = new List<Employee>();

            Employee tmpEmployee;                            // буферная переменная хранящая данные сотрудника считанные из БД
            
            // создание объекта класса SQLite для откырия соединения с БД
            using (SQLiteConnection connect = new SQLiteConnection(@$"Data Source={dataBasePath};"))
            {
                // объявление команды для БД, для получения всех сторудников из таблицы Employees
                string dataBaseCommandText = @"SELECT * FROM Employees;";
                
                // открытие соединения с БД
                connect.Open();

                // создание объекта команды SQLite
                SQLiteCommand command = new SQLiteCommand(dataBaseCommandText,connect);

                // создание объекта SQlite для последовательного чтения данных из БД по типу ключ значение
                SQLiteDataReader sqlReader = command.ExecuteReader();

                // чтение данных из БД и запись их в список
                for (int i=1; sqlReader.Read();i++)
                {
                    // инициализация объекта работника
                    tmpEmployee=new Employee
                    {
                        id=sqlReader["id"].ToString(),
                        Name= sqlReader["Name"].ToString(),
                        Surname= sqlReader["Surname"].ToString(),
                        Patronymic= sqlReader["Patronymic"].ToString(),
                        Birthday= sqlReader["Birthday"].ToString(),
                        Address= sqlReader["Address"].ToString(),
                        Group= sqlReader["Group"].ToString(),
                        Info= sqlReader["Info"].ToString()
                    };

                    tableData.Add(tmpEmployee);     // добалвение объекта работника в список
                }

                connect.Close();                // закрытие соединения с БД
                m_employeesData = tableData;    // сохранение данных о сотрудниках в памяти программы
                return m_employeesData;         // возвращение списка из метода
            }
        }
        // метод для доабвления записи о сотруднике в БД
        public bool AddWriteToDataBase(string Name="N/A", string Surname= "N/A", string Patronymic = "N/A", string Birthday = "N/A", string Address = "N/A", string Group = "N/A", string Info = "N/A")
        {
            // создание объекта класса SQLite для откырия соединения с БД
            using (SQLiteConnection connect = new SQLiteConnection(@$"Data Source={dataBasePath};"))
            {
                // обработка возможных исключений для возвращения bool результата опреации
                try
                {
                        // объявление команды для добавления новой записи о сотруднике в БД
                        string dataBaseCommand = @"INSERT INTO [Employees] ([Name], [Surname], [Patronymic], [Birthday], [Address], [Group], [Info]) VALUES(@Name,@Surname,@Patronymic,@Birthday,@Address,@Group,@Info)";

                        // создание объекта команнды SQLite 
                        SQLiteCommand command = new SQLiteCommand(dataBaseCommand,connect);

                        // перенос добовляемых значений сторудника в строку команды для БД
                        command.Parameters.AddWithValue("@Name",Name);
                        command.Parameters.AddWithValue("@Surname",Surname);
                        command.Parameters.AddWithValue("@Patronymic", Patronymic);
                        command.Parameters.AddWithValue("@Birthday", Birthday);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@Group", Group);
                        command.Parameters.AddWithValue("@Info", Info);

                        connect.Open();             // открытие соединения с БД
                        command.ExecuteNonQuery();  // выполнение команды в БД
                        connect.Close();            // закрытие соединения с БД

                        return true;
                }
                catch(Exception exception)
                {
                    connect.Close();                            // закрытие соединения с базой данных
                    Console.WriteLine($"DEBUG::DATA_BASE_MANAGER::ADD_WRITE_TO_DATA_BASE::EXCEPTION: {exception}");
                    return false;
                }
            }
            
        }
        // перегрузка метода для добавления нового сотрудника
        public bool AddWriteToDataBase(Employee employee)
        {
            return AddWriteToDataBase(employee.Name, employee.Surname, employee.Patronymic, employee.Birthday.ToString(), employee.Address,employee.Group, employee.Info);
        }
        // метод для получения данных по конкретному сотруднику по id
        public bool GetEmployeeById(int? employeeId,out Employee concreteEmployee)
        {
            concreteEmployee = new Employee();      // инициализация out переменной хранящей данные сотрудника
            
            // обработка возможных исключений
            try
            {                                     
                foreach (var employee in m_employeesData)
                {
                    if (employeeId == int.Parse(employee.id))
                    {
                        concreteEmployee = employee;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"DEBUG::DATA_BASE_MANAGER::GET_EMPLOYEE_BY_ID::EXCEPTION: {exception}");
                return false;
            }

        }

        // метод для обновления (редактирования) данных о сотруднике
        public bool EditEmployeeData(Employee employee)
        {
            // создание объекта класса SQLite для подключения к БД
            using (SQLiteConnection connect = new SQLiteConnection(@$"Data Source={dataBasePath};"))
            {
                // обработка возможных исключений
                try
                {

                    connect.Open();                             // открытие соединения с базой данных
                    MakeEditEmployeeCommand(employee, connect); // выполнение команды по обновлению данных о сотруднике в БД
                    connect.Close();                            // закрытие соединения с базой данных

                    return true;
                }
                catch (Exception exception)
                {
                    connect.Close();                            // закрытие соединения с базой данных
                    Console.WriteLine($"DEBUG::DATA_BASE_MANAGER::EDIT_EMPLOYEE_DATA::EXCEPTION:{exception}");
                    return false;
                }
            }
        }
        // метод для выполнения команды по обновлению данных
        private void MakeEditEmployeeCommand(Employee employee,SQLiteConnection connect)
        {
            // объявление команды для добавления новой записи о сотруднике в БД
            string dataBaseCommand = @$"UPDATE [Employees] SET [Name]=@Name, [Surname]=@Surname, [Patronymic]=@Patronymic, [Birthday]=@Birthday, [Address]=@Address, [Group]=@Group, [Info]=@Info WHERE id={employee.id}";

            // создание объекта команнды SQLite 
            SQLiteCommand command = new SQLiteCommand(dataBaseCommand, connect);

            // перенос добовляемых значений сторудника в строку команды для БД
            command.Parameters.AddWithValue("@Name", employee.Name);
            command.Parameters.AddWithValue("@Surname", employee.Surname);
            command.Parameters.AddWithValue("@Patronymic", employee.Patronymic);
            command.Parameters.AddWithValue("@Birthday", employee.Birthday);
            command.Parameters.AddWithValue("@Address", employee.Address);
            command.Parameters.AddWithValue("@Group", employee.Group);
            command.Parameters.AddWithValue("@Info", employee.Info);

            command.ExecuteNonQuery();  // выполнение команды в БД

        }
        // метод для удаления сотрудника из бд
        public bool DeleteWriteFromDataBase(int? id)
        {
            // создание объекта класса SQLite для подключения к БД
            using (SQLiteConnection connect = new SQLiteConnection(@$"Data Source={dataBasePath};"))
            {
                // обработка возможных исключений для возвращения bool результата опреации
                try
                {

                    connect.Open();                             // открытие соединения с базой данных
                    MakeDeleteEmployeeCommand(id, connect);     // выполнение команды по удалению сотрудника из БД
                    UpdateIdSequence(connect);                  // выполнение команды по обновлению счётчика БД, сразу после удаления сотрудников
                    connect.Close();                            // закрытие соединения с базой данных

                    return true;
                }
                catch (Exception exception)
                {
                    connect.Close();    // закртие соединения с БД
                    Console.WriteLine($"DEBUG::DATA_BASE_MANAGER::DELETE_WRITE_FROM_DATA_BASE::EXCEPTION:{exception}");
                    return false;
                }
            }
        }

        // метод для создания удаления сотрудника по Id команды и выполнения её для БД
        private void MakeDeleteEmployeeCommand(int? id,SQLiteConnection connect)
        {
            // команда для удаления одного сотрудника из БД в соотвествии с переданным в метод id 
            string dataBaseCommand = $@"DELETE FROM Employees WHERE id={id} AND EXISTS (SELECT 1 FROM Employees WHERE id={id})";

            // создание объекта комманды SQLite 
            SQLiteCommand command = new SQLiteCommand(dataBaseCommand, connect);

            command.ExecuteNonQuery();  // выполнение команды в БД
        }
        // метод для удаления группы сотрудников из БД
        public bool DeleteRangeWritesFeomDataBase(StringBuilder employeesId)
        {
            // создание объекта класса SQLite для подключения к БД
            using (SQLiteConnection connect = new SQLiteConnection(@$"Data Source={dataBasePath};"))
            {
                // обработка возможных исключений для возвращения bool результата опреации
                try
                {
                    // объявление переменных стартового и финального id диапозона удаляемых сотрудников
                    int startEmployeeId;
                    int endEmployeeId;

                    // если в строке с id сотрудников есть символ -
                    if (employeesId.ToString().Contains('-'))
                    {
                        string[] ids = employeesId.ToString().Split('-');

                        // получение из строки стартового числа диапозона и финального числа диапозона
                        startEmployeeId = int.Parse(ids[0]);
                        endEmployeeId = int.Parse(ids[1]);

                        // если стартовое число диапозона больше финального числа диапозона
                        if (startEmployeeId > endEmployeeId)
                        {
                            throw new ArgumentOutOfRangeException("Incorrect employees delete range");        // выбрасывание исключения о том что дипоззон некорректен
                        }

                        connect.Open();     // открытие соединения с БД
                        MakeDeleteEmployeeRangeBetweenCommand(startEmployeeId, endEmployeeId, connect);     // выполнение команды по удалению сотрудников из БД
                        UpdateIdSequence(connect);                                                          // выполнение команды по обновлению счётчика БД, сразу после удаления сотрудников
                        connect.Close();    // закртие соединения с БД

                    }
                    // иначе если в строке с id сотрудников нету символа -
                    else
                    {
                            connect.Open();     // открытие соединения с БД
                            MakeDeleteEmployeeRangeCommand(employeesId, connect);    // выполнение команды по удалению сотрудников из БД
                            UpdateIdSequence(connect);                               // выполнение команды по обновлению счётчика БД, сразу после удаления сотрудников
                            connect.Close();    // закртие соединения с БД
                    }
                    return true;
                }
                catch (Exception exception)
                {
                    connect.Close();    // закртие соединения с БД
                    Console.WriteLine($"DEBUG::DATA_BASE_MANAGER::DELETE_RANGE_WRITES_FROM_DATA_BASE::EXCEPTION:{exception}");
                    return false;
                }
            }
        }
        // метод для создания запроса к БД, для удаления сотрудников с группой соответсвутющих id
        private void MakeDeleteEmployeeRangeCommand(StringBuilder employeesId,SQLiteConnection connect)
        {
            // не законченная команда для удаления сотрудников из БД
            StringBuilder dataBaseCommand = new  StringBuilder($@"DELETE FROM Employees WHERE ID IN (");

            employeesId.Replace(' ',',');       // замена пробелов в строке StringBuilder на запятые

            dataBaseCommand.Append(employeesId);        // дописывание id всех id в команду для БД
            dataBaseCommand.Append(")");                // дописывание закрывающей скобки для команды

            // создание объекта комманды SQLite 
            SQLiteCommand command = new SQLiteCommand(dataBaseCommand.ToString(), connect);

            command.ExecuteNonQuery();  // выполнение команды в БД
        }
        // метод для создания запроса к БД для удаления диапозона сотрудников между двумя id
        private void MakeDeleteEmployeeRangeBetweenCommand(int startRangeEmployeeId,int endRangeEmployeeId, SQLiteConnection connect)
        {
            // не законченная команда для удаления сотрудников из БД
            StringBuilder dataBaseCommand = new StringBuilder($@"DELETE FROM Employees WHERE ID BETWEEN ");

            // дописывание команды для БД
            dataBaseCommand.Append(startRangeEmployeeId.ToString());
            dataBaseCommand.Append(" AND ");
            dataBaseCommand.Append(endRangeEmployeeId.ToString());

            // создание объекта комманды SQLite 
            SQLiteCommand command = new SQLiteCommand(dataBaseCommand.ToString(), connect);

            command.ExecuteNonQuery();  // выполнение команды в БД
        }
        // метод для выполнения команды сброса счётчика id сотрудников в БД
        private void UpdateIdSequence(SQLiteConnection connect)
        {
            // команда для обновления счётчика последовательности числом нынешним имеющимся максимальным id в таблице
            StringBuilder dataBaseCommand = new StringBuilder($@"UPDATE sqlite_sequence SET seq = (SELECT MAX('id') FROM Employees) WHERE name = 'Employees'");

            // создание объекта комманды SQLite 
            SQLiteCommand command = new SQLiteCommand(dataBaseCommand.ToString(), connect);

            command.ExecuteNonQuery();  // выполнение команды в БД
        }
    }
}
